using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using ApiProject.Service.Current;
using ApiProject.Service.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }


        [HttpPost("GetQuickStudentReport")]
        public async Task<IActionResult> GetQuickStudentReport(getstudentDellistReq req)
        {
            try
            {
                var res = await _reportService.GetQuickStudentReport(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(res);
            }
        }

        [HttpPost("GetStudentDetailReport")]
        public async Task<IActionResult> GetStudentDetailReport(GetStudentReq req)
        {
            try
            {
                var res = await _reportService.GetStudentDetailReport(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(res);
            }
        }

        [HttpPost("GetStudentIDCardReport")]
        public async Task<IActionResult> GetStudentIDCardReport(GetStudentIDCardReq req)
        {
            try
            {
                var res = await _reportService.GetStudentIDCardReport(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(res);
            }
        }

        // Student Fee & Installment
        [HttpPost("GetStudentFeeReport")]
        public async Task<IActionResult> GetStudentFeeReport(getstudentDellistReq req)
        {
            try
            {
                var res = await _reportService.GetStudentFeeReport(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(res);
            }
        }

        [HttpPost("GetClassWiseInstallmentReport")]
        public async Task<IActionResult> GetClassWiseInstallmentReport(BulkStudentReq req)
        {
            try
            {
                var res = await _reportService.GetClassWiseInstallmentReport(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(res);
            }
        }

        [HttpPost("GetAllClasswiseDueFeeReport")]
        public async Task<IActionResult> GetAllClasswiseDueFeeReport(BulkStudentReq req)
        {
            try
            {
                var res = await _reportService.GetAllClasswiseDueFeeReport(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(res);
            }
        }

        [HttpPost("GetClasswiseDueFeeReport")]
        public async Task<IActionResult> GetClasswiseDueFeeReport(BulkStudentReq req)
        {
            try
            {
                var res = await _reportService.GetClasswiseDueFeeReport(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(res);
            }
        }


        // Student TC & Dropdown 
        [HttpPost("GetStudentTCReport")]
        public async Task<IActionResult> GetStudentTCReport(GetStudentIDCardReq req)
        {
            try
            {
                var res = await _reportService.GetStudentTCReport(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(res);
            }
        }


        [HttpPost("GetStudentDropoutReport")]
        public async Task<IActionResult> GetStudentDropoutReport(GetStudentIDCardReq req)
        {
            try
            {
                var res = await _reportService.GetStudentDropoutReport(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(res);
            }
        }


        // Student Exam 
        [HttpPost("GetTestExamMarks")]
        public async Task<IActionResult> GetTestExamMarks(GetTestExamReq req)
        {
            try
            {
                var res = await _reportService.GetTestExamMarks(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(res);
            }
        }

        [HttpPost("GetTotalTestExamReport")]
        public async Task<IActionResult> GetTotalTestExamReport(GetTestExamReq req)
        {
            try
            {
                var res = await _reportService.GetTotalTestExamReport(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(res);
            }
        }

        [HttpPost("GetStudentMarksheet")]
        public async Task<IActionResult> GetStudentMarksheet(GetTestExamReq req)
        {
            try
            {
                var res = await _reportService.GetStudentMarksheet(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(res);
            }
        }


        // 
        [HttpPost("GetStudentFeeList")]
        public async Task<IActionResult> GetStudentFeeList(GetStudentFeeListReqModel req)
        {
            try
            {
                var res = await _reportService.GetStudentFeeDetail(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(res);
            }
        }


        //[HttpPost("GetStudentFeeListDetail")]
        //public async Task<IActionResult> GetStudentFeelistDetail(GetStudentFeeReqModel req)
        //{
        //    try
        //    {
        //        var res = await _reportService.GetStudentFeeListDetail(req);
        //        return Ok(res);
        //    }
        //    catch (Exception ex)
        //    {
        //        var res = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
        //        return BadRequest(res);
        //    }
        //}


        //[HttpPost("GetClasswiseInstallment")]
        //public async Task<IActionResult> GetClasswiseInstallment(GetClasswiseInstallmentListReq req)
        //{
        //    try
        //    {
        //        var res = await _reportService.GetClassWiseInstall(req);
        //        return Ok(res);
        //    }
        //    catch (Exception ex)
        //    {
        //        var res = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
        //        return BadRequest(res);
        //    }
        //}


    }
}
