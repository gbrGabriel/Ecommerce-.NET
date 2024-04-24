namespace Presentation.Errors;

public class ResponseApi(int statusCode, string? message = null)
{
    private static string GetDefaultMessageStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "Bad Request",
            401 => "Not Authorized",
            404 => "Not Found",
            500 => "Internal Error",
            _ => "Not localizad message for StatusCode."
        };
    }

    public int StatusCode { get; set; } = statusCode;
    public string? Message { get; set; } = message ?? GetDefaultMessageStatusCode(statusCode);
}
