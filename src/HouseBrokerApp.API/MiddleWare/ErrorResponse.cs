﻿namespace HouseBrokerApp.API.MiddleWare;

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public string[]? Details { get; set; }
}
