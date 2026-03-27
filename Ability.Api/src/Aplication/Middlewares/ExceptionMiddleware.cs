using Ability.Api.src.Aplication.Common;
using System.Net;
using System.Text.Json;

namespace Ability.Api.src.Aplication.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro capturado pelo Middleware");

            context.Response.ContentType = "application/json";
            
            var statusCode = ex is FluentValidation.ValidationException
                ? HttpStatusCode.BadRequest
                : HttpStatusCode.InternalServerError;

            context.Response.StatusCode = (int)statusCode;

            var response = new Result<object>
            {
                Success = false,
                Message = statusCode == HttpStatusCode.BadRequest ? "Erro de validação." : "Internal Server Error.",
                Errors = ex.Message 
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = JsonSerializer.Serialize(new { error = "An internal error occurred. Please try again later." });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(result);
    }
}
