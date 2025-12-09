using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Service.Current;
using ApiProject.Service.Student;
using ApiProject.Service.StudentAttendance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class StudentAttendanceController : ControllerBase
    {
        private readonly IStudentAttendanceService _studentAttendanceService;
        private readonly ILoginUserService _loginUser;

        public StudentAttendanceController(IStudentAttendanceService studentAttendance,
            ILoginUserService loginUser
            )
        {
            _studentAttendanceService = studentAttendance;
            _loginUser = loginUser;

        }

        [Authorize]
        [HttpPost("getclassbystudent")]
        public async Task<IActionResult> getclassbystudent(GetStudentAttendanceReqModel req)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                var response = ApiResponse<string>.ErrorResponse("Validation failed: " + string.Join(" | ", errorMessages));

                return BadRequest(response);
            }
            try
            {
                var res = await _studentAttendanceService.getclassbystudent(req);

                return Ok(res);
            }
            catch (Exception ex)
            {

                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }

        [Authorize]
        [HttpPost("InsertStudentAttendance")]
        public async Task<IActionResult> InsertStudentAttendance(List<StudentAttendanceReqModel> req)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                var response = ApiResponse<string>.ErrorResponse("Validation failed: " + string.Join(" | ", errorMessages));

                return BadRequest(response);
            }
            try
            {
                var res = await _studentAttendanceService.InsertStudentAttendance(req);

                return Ok(res);
            }
            catch (Exception ex)
            {

                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }


        [Authorize]
        [HttpPost("getmonthlyattendance")]
        public async Task<IActionResult> getmonthlyattendance(StudentMonthlyAttendanceReqModel req)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                var response = ApiResponse<string>.ErrorResponse("Validation failed: " + string.Join(" | ", errorMessages));

                return BadRequest(response);
            }
            try
            {
                var res = await _studentAttendanceService.getmonthlyattendance(req);

                return Ok(res);
            }
            catch (Exception ex)
            {

                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }

        //[Authorize]
        //[HttpGet("studentattendance")]
        //public async Task<IActionResult> studentattendance(string srno)
        //{           
        //    try
        //    {

        //        var res = await _studentAttendanceService.studentattendance(srno,_loginUser.SchoolId);

        //        return Ok(res);
        //    }
        //    catch (Exception ex)
        //    {

        //        var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
        //        return BadRequest(response);
        //    }
        //}

        //[HttpGet("schoolstudentattendance")]
        //public async Task<IActionResult> schoolstudentattendance(string srno, int schoolid)
        //{
        //    try
        //    {
        //        var res = await _studentAttendanceService.studentattendance(srno, schoolid);

        //        return Ok(res);
        //    }
        //    catch (Exception ex)
        //    {

        //        var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
        //        return BadRequest(response);
        //    }
        //}

        //[Authorize]
        //[HttpGet("todaystudentattendance")]
        //public async Task<IActionResult> todaystudentattendance()
        //{       
        //    try
        //    {
        //        var res = await _studentAttendanceService.todaystudentattendance();

        //        return Ok(res);
        //    }
        //    catch (Exception ex)
        //    {

        //        var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
        //        return BadRequest(response);
        //    }
        //}
    }
}
