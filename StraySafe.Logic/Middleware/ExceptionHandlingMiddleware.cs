using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StraySafe.Logic.Middleware.Models;

namespace StraySafe.Logic.Middleware;
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, ex);
        }
        catch (InvalidOperationException ex)
        {
            await HandleExceptionAsync(context, StatusCodes.Status409Conflict, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, int statusCode, Exception ex)
    {
        _logger.LogError(ex, "Error occurred.");

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        ErrorResponse errorResponse = new ErrorResponse
        {
            StatusCode = statusCode,
            Message = ex.Message
        };

        string? json = JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(json);
    }
}