﻿using SharedKernel;

namespace Domain.Users;

public sealed class User : Entity
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PasswordHash { get; set; }


    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public void SetRefreshToken(string token, DateTime expiryTime)
    {
        RefreshToken = token;
        RefreshTokenExpiryTime = expiryTime;
    }

    public bool IsRefreshTokenValid()
    {
        return RefreshToken != null &&
               RefreshTokenExpiryTime.HasValue &&
               RefreshTokenExpiryTime.Value > DateTime.UtcNow;
    }

    public void RevokeRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiryTime = null;
    }
}