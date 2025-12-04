using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using ApiProject.Service.Current;
using ApiProject.Service.Transport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransportController : BaseController
    {
        private readonly ITransportService _transportService;

        public TransportController(ITransportService transportService)
        {
            _transportService = transportService;
        }

        // *************************** Driver start *********************** //

        [HttpGet("GetDriverList")]
        public async Task<IActionResult> GetDriverList()
        {
            try
            {
                var res = await _transportService.GetDriverList();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddDriver")]
        public async Task<IActionResult> AddDriver(AddDriverreq req)
        {
            try
            {
                var res = await _transportService.AddDriver(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateDriver")]
        public async Task<IActionResult> UpdateDriver(UpdateDriver req)
        {
            try
            {
                var res = await _transportService.UpdateDriver(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ChangeStatusDriver")]
        public async Task<IActionResult> ChangeStatusDriver(int DriverId)
        {
            try
            {
                var res = await _transportService.ChangestatusDriver(DriverId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // *************************** Vehicle start *********************** //

        [HttpGet("GetVehicle")]
        public async Task<IActionResult> GetVehicle()
        {
            try
            {
                var res = await _transportService.GetVehicle();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddVehicle")]
        public async Task<IActionResult> AddVehicle(AddVehicleReq req)
        {
            try
            {
                var res = await _transportService.AddVehicle(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateVehicle")]
        public async Task<IActionResult> UpdateVehicle(UpdateVehicleModel req)
        {
            try
            {
                var res = await _transportService.UpdateVehicle(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ChangeStatusVehicle")]
        public async Task<IActionResult> ChangeStstusVehicle(int VehicleId)
        {
            try
            {
                var res = await _transportService.ChangeStatusVehicle(VehicleId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // *************************** Route Start *********************** //
        [HttpGet("GetRoute")]
        public async Task<IActionResult> GetRoute()
        {
            try
            {
                var res = await _transportService.GetRoute();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddRoute")]
        public async Task<IActionResult> AddRoute(AddRouteReq req)
        {
            try
            {
                var res = await _transportService.AddRoute(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("UpdateRoute")]
        public async Task<IActionResult> UpdateRoute(UpdateRouteModel req)
        {
            try
            {
                var res = await _transportService.UpdateRoute(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ChangeStatusRoute")]
        public async Task<IActionResult> ChangeStstusRoute(int RouteId)
        {
            try
            {
                var res = await _transportService.ChangeStatusRoute(RouteId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // *************************** Stoppage start  *********************** //

        [HttpGet("GetStoppage")]
        public async Task<IActionResult> GetStoppage()
        {
            try
            {
                var res = await _transportService.GetStoppage();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddStoppage")]
        public async Task<IActionResult> AddStoppage(AddStoppageReq req)
        {
            try
            {
                var res = await _transportService.AddStoppage(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateStoppage")]
        public async Task<IActionResult> UpdateStoppage(UpdateStoppageModel req)
        {
            try
            {
                var res = await _transportService.UpdateStoppage(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ChangeStatusStoppage")]
        public async Task<IActionResult> ChangeStstusStoppage(int StoppageId)
        {
            try
            {
                var res = await _transportService.ChangeStatusStoppage(StoppageId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        // *************************** Transport Vehicle by Route Assign Start *********************** //
        [HttpGet("GetRouteAssign")]
        public async Task<IActionResult> GetVehicleByRouteAssign()
        {
            try
            {
                var res = await _transportService.GetRouteAssign();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddRouteAssign")]
        public async Task<IActionResult> AddVehicleByRouteAssign(AddRouteAssignRequest model)
        {
            try
            {
                var res = await _transportService.AddRouteAssign(model);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateRouteAssign")]
        public async Task<IActionResult> UpdateVehicleByRouteAssign(UpdateRouteAssignRequest model)
        {
            try
            {
                var res = await _transportService.UpdateRouteAssign(model);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // *************************** Stoppage By  Transport Fee start *********************** //

        [HttpGet("GetStoppageByTransportFee")]
        public async Task<IActionResult> GetStoppageByTransportFee()
        {
            try
            {
                var res = await _transportService.GetTransportFee();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddStoppageByTransportFee")]
        public async Task<IActionResult> AddStoppageByTransportFee(AddTransportFeeReq req)
        {
            try
            {
                var res = await _transportService.AddTransportFee(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateStoppageByTransportFee")]
        public async Task<IActionResult> UpdateStoppageByTransportFee(UpdateTransportFee req)
        {
            try
            {
                var res = await _transportService.UpdateTransportFee(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ChangeStatusStoppageByTransportFee")]
        public async Task<IActionResult> ChangeStstusStoppageByTransportFee(int TFId)
        {
            try
            {
                var res = await _transportService.ChangeStatusTransportFee(TFId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        // *************************** Student Route Assign transport Fee  Start *********************** //
        [HttpPost("GetRouteById")]
        public async Task<IActionResult> GetRouteById(int VehicleId)
        {
            try
            {
                var res = await _transportService.GetRouteById(VehicleId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var ressonce = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(ressonce);

            }
        }

        [HttpPost("GetStoppageById")]
        public async Task<IActionResult> GetStoppageById(int RouteId)
        {
            try
            {
                var res = await _transportService.GetStoppageById(RouteId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var ressonce = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(ressonce);

            }
        }

        [HttpPost("GetTransportFeeById")]
        public async Task<IActionResult> GetTransportFeeById(int StoppageId)
        {
            try
            {
                var res = await _transportService.GetTransportFeeById(StoppageId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var ressonce = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(ressonce);

            }
        }

        [HttpPost("AddstudentRouteAssign")]
        public async Task<IActionResult> AddstudentRouteAssign(StuRouteAssignReq req)
        {
            try
            {
                var res = await _transportService.AddStudentRouteAssign(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateStudentRouteAss")]
        public async Task<IActionResult> UpdateStudentRouteAss(UpdateStuRouteAssignReq req)
        {
            try
            {
                var res = await _transportService.UpdateStudentRouteAssign(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ChangeStatusStudentRouteAssignFee")]
        public async Task<IActionResult> ChangeStatusStudentRouteAssignFee(int TSRAId)
        {
            try
            {
                var res = await _transportService.ChangeStatusStuRouteAssignFee(TSRAId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetStudentRouteAssignByFee")]
        public async Task<IActionResult> GetStudentRouteAssignByFee()
        {
            try
            {
                var res = await _transportService.GetStudentRouteAssign();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        // *************************** Add Student Transport Fee Start *********************** //

        [HttpPost("GetTransClassBySectionNdStudent")]
        public async Task<IActionResult> GetTransClassBySectionNdStudent(int ClassId)
        {
            try
            {
                var res = await _transportService.GetTransClassBySectionNdStudent(ClassId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var ressonce = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(ressonce);

            }
        }

        [HttpPost("GetTransSectionByStudentDetail")]
        public async Task<IActionResult> GetTransSectionByStudentDetail(int ClassId, int SectionId)
        {
            try
            {
                var res = await _transportService.GetTransSectionByStudentDetail(ClassId, SectionId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var ressonce = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(ressonce);

            }
        }

        [HttpPost("GetStudentByTransportData")]
        public async Task<IActionResult> GetStudentByTransportData(int StudentId)
        {
            try
            {
                var res = await _transportService.GetStudentByTransportData(StudentId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var ressonce = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(ressonce);

            }
        }

        [HttpPost("AddStudentTransportFee")]
        public async Task<IActionResult> AddStudentTransportFee(StudentTransportFeeReq req)
        {
            try
            {
                var res = await _transportService.AddStudentTransportFee(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        // ***************************   Transport Report Start *********************** //
        [HttpPost("TransportDetailsReport")]
        public async Task<IActionResult> TransportDetailsReport(TransportDetailReportReq req)
        {
            try
            {
                var res = await _transportService.TransportDetailsReport(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("TransportFeeReport")]
        public async Task<IActionResult> TransportFeeReport(TransportFeeReportReq req)
        {
            try
            {
                var res = await _transportService.TransportFeeReport(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetTransportFeeDetails")]
        public async Task<IActionResult> GetTransportFeeDetails(int StudentId)
        {
            try
            {
                var res = await _transportService.GetTransportFeeDetails(StudentId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetTransportPaidOldFee")]
        public async Task<IActionResult> GetTransportPaidOldFee(int StudentId)
        {
            try
            {
                var res = await _transportService.GetTransportPaidOldFee(StudentId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateTransportOldFee")]
        public async Task<IActionResult> UpdateTransportOldFee(UpdateOldFeeReq req)
        {
            try
            {
                var res = await _transportService.UpdateTransportOldFee(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





    }
}
