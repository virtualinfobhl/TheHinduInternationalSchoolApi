using ApiProject.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult OkResponse(object? result = null, bool isDisplayMessage = false, string message = "")
        {
            return Ok(new SuccessVM { status = "success", data = result, displayMessage = isDisplayMessage, message = message });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ErrorRepsponse(object response = null, string error = "", bool displaymessage = true)
        {
            return Ok(new ErrorVM { data = response, error = error, displayMessage = displaymessage, status = "failure" });
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ErrorAccessDenied()
        {
            return Ok(new ErrorVM { error = "Access denied.", displayMessage = true, status = "failure" });
        }
    }
}
