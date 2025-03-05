﻿namespace Tdev702.Contracts.Auth;

public class AuthStateData
{
    public required AuthenticationParameters AuthenticationParameters { get; init; }
    public required DateTime Timestamp { get; init; }
}