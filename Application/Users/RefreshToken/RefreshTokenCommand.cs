﻿using Application.Abstractions.Messaging;

namespace Application.Users.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : ICommand<string>;