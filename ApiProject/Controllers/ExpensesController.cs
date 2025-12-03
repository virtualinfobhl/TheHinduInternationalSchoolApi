using ApiProject.Data;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using ApiProject.Service.Current;
using ApiProject.Service.SchoolExpenses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpensesController : BaseController
    {

        private readonly ISchoolExpensesService _schoolExpensesService;

        public ExpensesController(ISchoolExpensesService schoolExpensesService)
        {
            _schoolExpensesService = schoolExpensesService;
        }

        [HttpGet("GetExpensesList")]
        public async Task<IActionResult> GetExpensesList()
        {
            try
            {
                var res = await _schoolExpensesService.GetExpensesList();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddExpenses")]
        public async Task<IActionResult> AddExpenses(ExpensesReq req)
        {
            try
            {
                var res = await _schoolExpensesService.AddExpenses(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateExpenses")]
        public async Task<IActionResult> UpdateExpenses(UpdateExpensesModel req)
        {
            try
            {
                var res = await _schoolExpensesService.UpdateExpenses(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ChangeStatusExpenses")]
        public async Task<IActionResult> ChangeStatusExpenses(int ExpensesId)
        {
            try
            {
                var res = await _schoolExpensesService.ChangestatusExpenses(ExpensesId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetSchlExpenses")]
        public async Task<IActionResult> GetSchlExpenses()
        {
            try
            {
                var res = await _schoolExpensesService.Getschlexpenses();
                return Ok(res);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("InsertSchoolExpenses")]
        public async Task<IActionResult> InsertSchoolExpenses(SchoolExpensesReq req)
        {
            try
            {
                var res = await _schoolExpensesService.InsertSchoolExpenses(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateSchlExpenses")]
        public async Task<IActionResult> UpdateSchlExpenses(UpdateschlExpenses req)
        {
            try
            {
                var res = await _schoolExpensesService.UpdateSchlExpenses(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetSchoolExpensesList")]
        public async Task<IActionResult> GetSchoolExpensesList(GetSchoolExpensesListReq req)
        {
            try
            {
                var res = await _schoolExpensesService.GetSchlExpensesList(req);
                return Ok(res);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }



    }
}
