namespace Presentation.Errors;
public class ValidationErrorResponseApi : ResponseApi
{
    public ValidationErrorResponseApi() : base(400)
    {
    }
    public IEnumerable<string>? Errors { get; set; }

}
