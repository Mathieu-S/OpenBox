using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace OpenBox.WebApi.Controllers;

/// <summary>
/// Controller for unhandled exceptions.
/// </summary>
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    /// <summary>
    /// The development fallback for unhandled exceptions.
    /// </summary>
    /// <param name="webHostEnvironment"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    [Route("/error-local-development")]
    public IActionResult ErrorLocalDevelopment([FromServices] IWebHostEnvironment webHostEnvironment)
    {
        if (webHostEnvironment.EnvironmentName != "Development")
        {
            throw new InvalidOperationException("This shouldn't be invoked in non-development environments.");
        }

        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

        return Problem(title: context?.Error.Message, detail: context?.Error.StackTrace);
    }

    /// <summary>
    /// The production fallback for unhandled exceptions.
    /// </summary>
    /// <returns></returns>
    [Route("/error")]
    public IActionResult Error() => Problem();
}