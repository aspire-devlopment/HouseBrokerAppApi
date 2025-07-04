using Microsoft.AspNetCore.Mvc;


namespace HouseBrokerApp.API.MiddleWare;

public static class ResponseHelpers
{
    public static IActionResult BadRequestResponse(string message, params string[] details)
    {
        return new BadRequestObjectResult(new ErrorResponse
        {
            Message = message,
            Details = details
        });
    }
}
