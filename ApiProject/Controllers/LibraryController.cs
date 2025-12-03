using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using ApiProject.Service.Current;
using ApiProject.Service.Employee;
using ApiProject.Service.Library;
using ApiProject.Service.Transport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Net;



namespace ApiProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LibraryController : BaseController
    {
        private readonly ILibraryService _libService;

        public LibraryController(ILibraryService libService)
        {
            _libService = libService;
        }

        [HttpGet("GetBoodData")]
        public async Task<IActionResult> GetBoodData()
        {
            try
            {
                var res = await _libService.GetBoodData();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddBook")]
        public async Task<IActionResult> AddBook(AddBookReq req)
        {
            try
            {
                var res = await _libService.Addbook(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Updatebook")]
        public async Task<IActionResult> Updatebook(UpdateBookReq req)
        {
            try
            {
                var res = await _libService.Updatebook(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpGet("ChangeStatusBook")]
        public async Task<IActionResult> ChangeStatusBook(int BookId)
        {
            try
            {
                var res = await _libService.ChangeStatusBook(BookId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetLibraryStudent")]
        public async Task<IActionResult> GetLibraryStudent(BulkStudentReq req)
        {
            try
            {
                var res = await _libService.GetLibraryStudent(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetLibraryEmployee")]
        public async Task<IActionResult> GetLibraryEmployee()
        {
            try
            {
                var res = await _libService.GetLibraryEmployee();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddLibraryCardNo")]
        public async Task<IActionResult> AddLibraryCardNo(AddcardNoReq req)
        {
            try
            {
                var res = await _libService.AddLibraryCardNo(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetLibraryClassBySectionNdStudent")]
        public async Task<IActionResult> GetLibraryClassBySectionNdStudent(int ClassId)
        {
            try
            {
                var res = await _libService.GetLibraryClassBySectionNdStudent(ClassId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var ressonce = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(ressonce);

            }
        }

        [HttpPost("GetLibrarySectionByStudent")]
        public async Task<IActionResult> GetLibrarySectionByStudent(int ClassId, int SectionId)
        {
            try
            {
                var res = await _libService.GetLibrarySectionByStudent(ClassId, SectionId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var ressonce = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(ressonce);

            }
        }

        [HttpPost("GetEmployeeById")]
        public async Task<IActionResult> GetEmployeeById(int EmpId)
        {
            try
            {
                var res = await _libService.GetEmployeeById(EmpId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var ressonce = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(ressonce);

            }
        }

        [HttpPost("getMenmberProfileList")]
        public async Task<IActionResult> getMenmberProfileList(int LibraryId)
        {
            try
            {
                var res = await _libService.getMenmberProfileList(LibraryId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var ressonce = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(ressonce);

            }
        }

        [HttpPost("GetAuthorByBookId")]
        public async Task<IActionResult> GetAuthorByBookId(int BookId)
        {
            try
            {
                var res = await _libService.GetAuthorByBookId(BookId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var ressonce = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(ressonce);

            }
        }

        [HttpPost("AddIssueLibraryBook")]
        public async Task<IActionResult> AddIssueLibraryBook(IssueLibraryBookReq req)
        {
            try
            {
                var res = await _libService.AddIssueLibraryBook(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddReturnDate")]
        public async Task<IActionResult> AddReturnDate(UpdateReturndateReq req)
        {
            try
            {
                var res = await _libService.AddReturnDate(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetBookReport")]
        public async Task<IActionResult> GetBookReport()
        {
            try
            {
                var res = await _libService.GetBookReport();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMemberList")]
        public async Task<IActionResult> GetMemberList()
        {
            try
            {
                var res = await _libService.GetMemberList();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetLibraryReport")]
        public async Task<IActionResult> GetLibraryReport(LibrartReportReq req)
        {
            try
            {
                var res = await _libService.GetLibraryReport(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }

}
