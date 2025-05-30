﻿using Application.Users.Register;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using MediatR;
using Web.Api.Extensions;
using Web.Api.Infrastructure;
using Application.Users.Login;
using Application.Users.GetById;
using Application.Abstractions.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Application.Users.RefreshToken;
using Application.Users.Logout;
using Application.AssignmentItems.Undo;
using SharedKernel.Responses;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public sealed record RegisterRequest(string Email, string FirstName, string LastName, string Password);
        public sealed record LoginRequest(string Email, string Password);
        public sealed record RefreshTokenRequest(string RefreshToken);


        [HttpPost("login")]
        public async Task<IResult> Login(LoginRequest request, ISender sender, CancellationToken cancellationToken)
        {
            var command = new LoginUserCommand(
                request.Email,
                request.Password);

            Result<LoginResponse> result = await sender.Send(command, cancellationToken);
            return result.Match(
                response =>
                {
                    // Set only the refresh token in HTTP-only cookie
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = response.RefreshTokenExpiryTime
                    };

                    Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);

   
                    return Results.Ok(new
                    {
                        jwt = response.AccessToken,
                        message = "Login successful"
                    });
                },
                CustomResults.Problem
            );
        }

        [HttpPost("register")]
        public async Task<IResult> Register(RegisterRequest request, ISender sender, CancellationToken cancellationToken)
        {
            var command = new RegisterUserCommand(
                request.Email,
                request.FirstName,
                request.LastName,
                request.Password);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        }

        [HttpGet("me")]
        public async Task<IResult> GetAuthOrRefresh(ISender sender, IUserContext user, IMemoryCache cache, CancellationToken cancellationToken)
        {
            // If user is authenticated, return user data
            if (User.Identity.IsAuthenticated)
            {
                if (!cache.TryGetValue($"user:{user.UserId}", out UserResponse? cachedUser))
                {
                    var command = new GetUserByIdQuery(user.UserId);
                    Result<UserResponse> result = await sender.Send(command, cancellationToken);

                    if (result.IsFailure)
                    {
                        return CustomResults.Problem(result);
                    }

                    cachedUser = result.Value;
                    cache.Set($"user:{user.UserId}", cachedUser, TimeSpan.FromMinutes(5));
                }

                return Results.Ok(new { user = cachedUser });
            }

            string? refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Results.Unauthorized();
            }

            var refreshCommand = new RefreshTokenCommand(refreshToken);
            Result<RefreshTokenWithUserResponse> refreshResult = await sender.Send(refreshCommand, cancellationToken);

            if (refreshResult.IsFailure)
            {
                return CustomResults.Problem(refreshResult);
            }

            // Return both the new token and user data
            return Results.Ok(new
            {
                token = refreshResult.Value.AccessToken,
                user = refreshResult.Value.User
            });
        }
        [HttpPost("logout")]
        public async Task<IResult> Logout(ISender sender, IUserContext user, CancellationToken cancellationToken)
        {
            // Delete the cookie first
            Response.Cookies.Delete("refreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            // Only revoke the token if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                var command = new LogoutCommand(user.UserId);
                Result<bool> result = await sender.Send(command, cancellationToken);

                if (result.IsFailure)
                {
                    return CustomResults.Problem(result);
                }
            }

            return Results.Ok(new { message = "Logged out successfully" });
        }
    }
}