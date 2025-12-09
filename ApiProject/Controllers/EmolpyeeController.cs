using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using ApiProject.Service.Current;
using ApiProject.Service.Employee;
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
    public class EmolpyeeController : BaseController
    {
        private readonly IEmployeeServide _employeeService;

        public EmolpyeeController(IEmployeeServide EmployeeService)
        {
            _employeeService = EmployeeService;
        }

        // ********* Employee Details Start 
        [HttpPost("AddEmployeeDetail")]
        public async Task<IActionResult> AddEmployeeDetail(AddEmployeeDetailReq request)
        {
            try
            {
                var res = await _employeeService.AddEmployeeDetail(request);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }


        [HttpPost("UpdateEmoloyeeDetail")]
        public async Task<IActionResult> UpdateEmoloyeeDetail(UpdatreEmployeeDetailReq request)
        {
            try
            {
                var res = await _employeeService.UpdateEmoloyeeDetail(request);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }


        [HttpGet("GetEmployeeLit")]
        public async Task<IActionResult> GetEmployeeLit()
        {
            try
            {
                var res = await _employeeService.GetEmployeeLit();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("EmployeeReport")]
        public async Task<IActionResult> EmployeeReport(GetEmployeReq req)
        {
            try
            {
                var res = await _employeeService.EmployeeReport(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(res);
            }
        }


        [HttpGet("ChangeStatusEmployee")]
        public async Task<IActionResult> ChangeStatusEmployee(int EmpId)
        {
            try
            {
                var res = await _employeeService.ChangeStatusEmployee(EmpId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // ********* Employee Details End 



        // ********* Employee Worl alocation Start 
        [HttpPost("AddEmpWorkAllocation")]
        public async Task<IActionResult> AddEmpWorkAllocation(AddWorkallcation req)
        {
            try
            {
                var res = await _employeeService.AddEmpWorkAllocation(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetEmployeeNdClassBySubjectData")]
        public async Task<IActionResult> GetEmployeeNdClassBySubjectData(int EmpId, int ClassId)
        {
            try
            {
                var res = await _employeeService.GetEmployeeNdClassBySubjectData(EmpId, ClassId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("EmpWorkallocationReport")]
        public async Task<IActionResult> EmpWorkallocationReport(GetEmployeReq req)
        {
            try
            {
                var res = await _employeeService.EmpWorkallocationReport(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(res);
            }
        }

        // ********* Employee Worl alocation End 




        // ********* Employee Attendance Start 
        [HttpPost("GetEmployeeAttendance")]
        public async Task<IActionResult> GetEmployeeAttendance(getEmployeeAttendance req)
        {
            try
            {
                var res = await _employeeService.GetEmployeeAttendance(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("InsertEmployeeAttendance")]
        public async Task<IActionResult> InsertEmployeeAttendance(List<addEmployeeAttendanceReq> EAttendance)
        {
            try
            {
                var res = await _employeeService.InsertEmployeeAttendance(EAttendance);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("EmployeeAttendanceReport")]
        public async Task<IActionResult> EmployeeAttendanceReport(int Month)
        {
            try
            {
                var res = await _employeeService.EmployeeAttendanceReport(Month);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // ********* Employee Attendance end 


        // Employee Salary 
        [HttpGet("GetAdvanceSalaryList")]
        public async Task<IActionResult> GetAdvanceSalaryList()
        {
            try
            {
                var res = await _employeeService.GetAdvanceSalaryList();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("AddAdvanceSalary")]
        public async Task<IActionResult> AddAdvanceSalary(AddAdavanceSalaryReq req)
        {
            try
            {
                var res = await _employeeService.AddAdvanceSalary(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }


        [HttpGet("GetEmployeeSalary")]
        public async Task<IActionResult> GetEmployeeSalary(int month)
        {
            try
            {
                var res = await _employeeService.GetEmployeeSalary(month);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddEmployeeSalary")]
        public async Task<IActionResult> AddEmployeeSalary(List<AddEmployeeSalaryModel> model)
        {
            try
            {
                var res = await _employeeService.AddEmployeeSalary(model);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
