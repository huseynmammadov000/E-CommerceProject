using Microsoft.AspNetCore.Http;

using Ecommerce.Shared.Exceptions;
using Ecommerce.Shared.Models.Responses;


namespace Ecommerce.Shared.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;


    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BaseException e)
        {
            var apiResponse = new ApiResponse
            {
                Result = false,
                Message = e.Message
            };

            context.Response.StatusCode = StatusCodes.Status200OK;
            await context.Response.WriteAsJsonAsync(apiResponse);
        }
        catch (Exception)
        {
            var apiResponse = new ApiResponse
            {
                Result = false,
                Message = "Something went wrong",
            };

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(apiResponse);
        }
    }
}