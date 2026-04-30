using ApiProject.Models.Request;
using ApiProject.Models.Response;
using ApiProject.Service.Parent_Login;
using ApiProject.Service.Parents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ParentController : BaseController
    {
        private readonly IParentsService _ParentsService;
        public ParentController(IParentsService parentsService)
        {
            _ParentsService = parentsService;
        }


        [HttpGet("GetStudentList")]
        public async Task<IActionResult> GetStudentList()
        {
            try
            {
                var res = await _ParentsService.GetStudentList();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpGet("GetStudentToken")]
        public async Task<IActionResult> GetStudentToken(int StudentId)
        {
            try
            {
                var result = await _ParentsService.GetStudentToken(StudentId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        // ==================================    Student details 
        [HttpGet("GetStudentParentsDetails")]
        public async Task<IActionResult> GetStudentParentsDetails()
        {
            try
            {
                var res = await _ParentsService.GetStudentParentsDetail();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        //  ======================================  get attendance by month 

        [HttpGet("GetAttendanceByMonth")]
        public async Task<IActionResult> GetAttendanceByMonth(int StudentId, int month)
        {
            try
            {
                var result = await _ParentsService.GetAttendanceByMonth(StudentId, month);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        // ===========================================================  get exam test type 
        [HttpGet("GetTestType")]
        public async Task<IActionResult> GetTestType(int StudentId)
        {
            try
            {
                var result = await _ParentsService.GetTestType(StudentId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpGet("GetTestwiseExamMarks")]
        public async Task<IActionResult> GetTestwiseExamMarks(int StudentId, string testtype)
        {
            try
            {
                var result = await _ParentsService.GetTestwiseExamMarks(StudentId, testtype);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }


        // ======================================================     student installment fee 
        [HttpGet("GetStudentInstallmentFee")]
        public async Task<IActionResult> GetStudentInstallmentFee()
        {
            try
            {
                var res = await _ParentsService.GetStudentInstallmentFee();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpPost("AddStudentInstallmentFee")]
        public async Task<IActionResult> AddStudentInstallmentFee(AddStudentinstallReq req)
        {
            try
            {
                var res = await _ParentsService.AddStudentInstallmentFee(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("UpdateStudentPaymentSuccessfully")]
        public async Task<IActionResult> UpdateStudentPaymentSuccessfully(int StudentId, int ReceiptId)
        {
            try
            {
                var res = await _ParentsService.UpdateStudentPaymentSuccessfully(StudentId, ReceiptId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        // ======================================================    transport fee 
        [HttpGet("GetTransportInstallFee")]
        public async Task<IActionResult> GetTransportInstallFee()
        {
            try
            {
                var res = await _ParentsService.GetTransportInstallFee();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpPost("AddStudentTransportFee")]
        public async Task<IActionResult> AddStudentTransportFee(AddTransportMonthFeeReq req)
        {
            try
            {
                var res = await _ParentsService.AddStudentTransportFee(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("UpdateTransportPaymentSuccessfully")]
        public async Task<IActionResult> UpdateTransportPaymentSuccessfully(int StudentId, int ReceiptId)
        {
            try
            {
                var res = await _ParentsService.UpdateTransportPaymentSuccessfully(StudentId, ReceiptId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }


        // ====================================================   Other 
        [HttpGet("GetStudentFee")]
        public async Task<IActionResult> GetStudentFee()
        {
            try
            {
                var res = await _ParentsService.GetStudentFee();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpGet("GetStudentFeeInstallment")]
        public async Task<IActionResult> GetStudentFeeInstallment()
        {
            try
            {
                var res = await _ParentsService.GetStudentFeeInstallment();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpGet("GetStudentDueInstallment")]
        public async Task<IActionResult> GetStudentDueInstallment()
        {
            try
            {
                var res = await _ParentsService.GetStudentDueInstallment();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        // change password 
        [HttpGet("GetPassword")]
        public async Task<IActionResult> GetPassword(int StudentId)
        {
            try
            {
                var result = await _ParentsService.GetPassword(StudentId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpPost("Changepassword")]
        public async Task<IActionResult> Changepassword(GetChangepasswordReq req)
        {
            try
            {
                var res = await _ParentsService.Changepassword(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
