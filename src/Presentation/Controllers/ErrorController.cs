using Microsoft.AspNetCore.Mvc;
using Presentation.Errors;

namespace Presentation.Controllers;

[Route("errors/{code}")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : AbstractBaseController
{
    public IActionResult Error(int code) => new ObjectResult(new ResponseApi(code));
}
