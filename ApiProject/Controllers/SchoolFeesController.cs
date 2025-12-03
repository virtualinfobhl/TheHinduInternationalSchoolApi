using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Service.School;
using ApiProject.Service.SchoolFees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    [Authorize]
    public class SchoolFeesController : ControllerBase
    {
        private readonly ISchoolFees _schoolFees;

        public SchoolFeesController(
            ISchoolFees schoolFees
            )
        {

            _schoolFees = schoolFees;
        }


        // Student Class Fee
        [HttpGet("GetClassFees")]
        public async Task<IActionResult> GetClassFees()
        {
            try
            {
                var res = await _schoolFees.GetClassFees();
                return Ok(res);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
        }

        [HttpPost("AddClassFees")]
        public async Task<IActionResult> AddClassFees([FromBody] AddClassFeesReq req)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                var response = ApiResponse<string>.ErrorResponse("Validation failed: " + string.Join(" | ", errorMessages));

                return Ok(response);
            }

            try
            {
                var res = await _schoolFees.AddClassFees(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return Ok(response);
            }
        }

        [HttpPost("UpdateClassFees")]
        public async Task<IActionResult> UpdateClassFees([FromBody] UpdateClassFeesReq req)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                var response = ApiResponse<string>.ErrorResponse("Validation failed: " + string.Join(" | ", errorMessages));

                return Ok(response);
            }

            try
            {
                var res = await _schoolFees.UpdateClassFees(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return Ok(response);
            }
        }

        [HttpGet("ChangeStatusClassFees")]
        public async Task<IActionResult> ChangeStatusClassFees(int feesid)
        {
            try
            {
                var res = await _schoolFees.ChangeStatusClassFees(feesid);
                return Ok(res);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
        }


        // fee Installment
        [HttpGet("GetFeeInstallment")]
        public async Task<IActionResult> GetFeeInstallment()
        {
            try
            {
                var res = await _schoolFees.GetFeeInstallment();
                return Ok(res);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
        }
            
        [HttpPost("insertfeesinstallment")]
        public async Task<IActionResult> insertfeesinstallment([FromBody] List<AddFeesInstallmentReq> req)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                var response = ApiResponse<string>.ErrorResponse("Validation failed: " + string.Join(" | ", errorMessages));

                return Ok(response);
            }

            try
            {
                var res = await _schoolFees.insertfeesinstallment(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return Ok(response);
            }
        }

        [HttpPost("updatefeesinstallment")]
        public async Task<IActionResult> updatefeesinstallment([FromBody] List<AddFeesInstallmentReq> req)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                var response = ApiResponse<string>.ErrorResponse("Validation failed: " + string.Join(" | ", errorMessages));

                return Ok(response);
            }

            try
            {
                var res = await _schoolFees.updatefeesinstallment(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }


        // fees collection
        [HttpGet("getclassbystudent")]
        public async Task<IActionResult> getclassbystudent(int classid)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                var response = ApiResponse<string>.ErrorResponse("Validation failed: " + string.Join(" | ", errorMessages));

                return Ok(response);
            }

            try
            {
                var res = await _schoolFees.getclassbystudent(classid);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return Ok(response);
            }
        }

        [HttpPost("getstudentfeesdetail")]
        public async Task<IActionResult> getstudentfeesdetail([FromBody] StudentFeesDetailReq req)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                var response = ApiResponse<string>.ErrorResponse("Validation failed: " + string.Join(" | ", errorMessages));

                return Ok(response);
            }

            try
            {
                var res = await _schoolFees.getstudentfeesdetail(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return Ok(response);
            }
        }

        [HttpPost("insertstudentfees")]
        public async Task<IActionResult> insertstudentfees([FromBody] StudentFeesReq req)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                var response = ApiResponse<string>.ErrorResponse("Validation failed: " + string.Join(" | ", errorMessages));

                return Ok(response);
            }

            try
            {
                var res = await _schoolFees.insertstudentfees(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return Ok(response);
            }
        }

        [HttpGet("getfeereceipt")]
        public async Task<IActionResult> getfeereceipt(int receiptId)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                var response = ApiResponse<string>.ErrorResponse("Validation failed: " + string.Join(" | ", errorMessages));

                return Ok(response);
            }

            try
            {
                var res = await _schoolFees.getfeereceipt(receiptId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return Ok(response);
            }
        }

        [HttpPost("GetDailyFeeCollection")]
        public async Task<IActionResult> GetDailyFeeCollection([FromBody] FeesCollectionReq req)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                var response = ApiResponse<string>.ErrorResponse("Validation failed: " + string.Join(" | ", errorMessages));

                return Ok(response);
            }

            try
            {
                var res = await _schoolFees.GetDailyFeeCollection(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return Ok(response);
            }
        }

        [HttpPost("getclassfees")]
        public async Task<IActionResult> getclassfees([FromBody] ClassFeesFilterReq req)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                var response = ApiResponse<string>.ErrorResponse("Validation failed: " + string.Join(" | ", errorMessages));

                return Ok(response);
            }

            try
            {
                var res = await _schoolFees.getclassfees(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return Ok(response);
            }
        }

        [HttpPost("GetClasswiseTotalFee")]
        public async Task<IActionResult> GetClasswiseTotalFee(int ClassId)
        {
            try
            {
                var res = await _schoolFees.GetClasswiseTotalFee(ClassId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var ressonce = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(ressonce);

            }
        }

        [HttpPost("getclassfeesInstallment")]
        public async Task<IActionResult> getclassfeesInstallment([FromBody] ClassFeesFilterReq req)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                var response = ApiResponse<string>.ErrorResponse("Validation failed: " + string.Join(" | ", errorMessages));

                return Ok(response);
            }

            try
            {
                var res = await _schoolFees.getclassfeesInstallment(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return Ok(response);
            }
        }

    }
}
