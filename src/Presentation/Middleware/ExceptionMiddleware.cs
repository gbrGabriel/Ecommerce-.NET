using Presentation.Errors;
using System.Text.Json;

namespace Presentation.Middleware;

public class ExceptionMiddleware(RequestDelegate request, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
{
    private readonly RequestDelegate _request = request;
    private readonly ILogger<ExceptionMiddleware> _logger = logger;
    private readonly IHostEnvironment _environment = environment;
    private readonly JsonSerializerOptions _options = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _request(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            await context.Response.WriteAsync(JsonSerializer.Serialize(
                    new ExceptionApi(500,
                        _environment.IsDevelopment() ? ex.StackTrace ?? string.Empty : string.Empty,
                        ex.Message), _options));
        }
    }
}
