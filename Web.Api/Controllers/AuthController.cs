using Application.Users.Register;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using MediatR;
using Web.Api.Extensions;
using Web.Api.Infrastructure;
using Application.Users.Login;
using Application.Users.GetById;
using Application.Abstractions.Authentication;
using Microsoft.Extensions.Caching.Memory;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public sealed record RegisterRequest(string Email, string FirstName, string LastName, string Password);
        public sealed record LoginRequest(string Email, string Password);


        [HttpPost("login")]
        public async Task<IResult> Login(LoginRequest request, ISender sender, CancellationToken cancellationToken)
        {

            var command = new LoginUserCommand(
            request.Email,
            request.Password);

            Result<string> result = await sender.Send(command, cancellationToken);
            return result.Match(
                   token =>
                   {
                       var cookieOptions = new CookieOptions
                       {
                           HttpOnly = true,
                           Secure = false,
                           SameSite = SameSiteMode.Lax,
                           Expires = DateTime.UtcNow.AddDays(7)
                       };

                       Response.Cookies.Append("jwt", token, cookieOptions);

                       return Results.Ok(new { message = "Login successful" });
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
        public async Task<IResult> GetAuthenticatedUser(ISender sender, IUserContext user, IMemoryCache cache, CancellationToken cancellationToken)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Results.Unauthorized();
            }

            if (!cache.TryGetValue($"user:{user.UserId}", out UserResponse? cachedUser))
            {
                var command = new GetUserByIdQuery(user.UserId);
                Result<UserResponse> result = await sender.Send(command, cancellationToken);

                if (result.IsFailure)
                {
                    return CustomResults.Problem(result);
                }

                cachedUser = result.Value;

                // ✅ Cache user details for 5 minutes
                cache.Set($"user:{user.UserId}", cachedUser, TimeSpan.FromMinutes(5));
            }

            return Results.Ok(cachedUser);
        }
    }
}
