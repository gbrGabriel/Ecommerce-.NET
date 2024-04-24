namespace Presentation.Errors;

public class ExceptionApi(int statusCode, string details, string? message = null) : ResponseApi(statusCode, message)
{
    public string Details { get; set; } = details;
}
