using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using ApiProject.Service.Login;
using ApiProject.Service.Parent_Login;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : BaseController
    {


        private readonly IAuthService _authService;
        private readonly IParentLoginService _parentLoginService;

        public LoginController(IAuthService authService, IParentLoginService parentLogin)
        {

            _authService = authService;
            _parentLoginService = parentLogin;
        }


        //[HttpGet("check-school-code")]
        //public async Task<IActionResult> CheckSchoolCode(string schoolcode)
        //{
        //    try
        //    {
        //        var schoolId = await _authService.CheckSchoolCodeAsync(schoolcode);

        //        if (schoolId.HasValue)
        //        {
        //            return Ok(new { success = true, SchoolId = schoolId.Value });
        //        }
        //        else
        //        {
        //            return NotFound(new { success = false, message = "School not found or inactive." });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { success = false, error = ex.Message });
        //    }
        //}


        [HttpPost("login")]
        public async Task<IActionResult> Login(RequestLogin request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.LoginType))
                {

                    var response = ApiResponse<string>.ErrorResponse("Login Type is required.");
                    return Ok(response);
                }

                object result = null;

                if (request.LoginType == "SchoolLogin")
                {
                    result = await _authService.LoginAsync(request);
                }
                else if (request.LoginType == "ParentLogin")
                {
                    result = await _parentLoginService.ParentLoginAsync(request);
                }
                else
                {
                    var response = ApiResponse<string>.ErrorResponse("Invalid Login Type");
                    return Ok(response);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return Ok(response);

            }
        }

    }
}
