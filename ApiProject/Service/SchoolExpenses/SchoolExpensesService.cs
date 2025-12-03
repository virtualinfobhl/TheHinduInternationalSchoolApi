using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using ApiProject.Service.Current;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using System.Diagnostics;
using System.Linq;

namespace ApiProject.Service.SchoolExpenses
{
    public class SchoolExpensesService : ISchoolExpensesService
    {
        private readonly ILoginUserService _loginUser;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SchoolExpensesService(ILoginUserService loginUser, ApplicationDbContext context, IMapper mapper)
        {
            _loginUser = loginUser;
            _context = context;
            _mapper = mapper;

        }

        public async Task<ApiResponse<List<GetExpenses>>> GetExpensesList()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var ExpenseEntity = await _context.ExpenseTbl.Where(c => c.CompanyId == SchoolId)
                    .Select(c => new GetExpenses
                    {
                        ExpensesId = c.ExpenseId,
                        ExpenseName = c.ExpenseName,
                        Active = c.Active,
                    }).ToListAsync();
                return ApiResponse<List<GetExpenses>>.SuccessResponse(ExpenseEntity, "Excenses list fetched successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetExpenses>>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> AddExpenses(ExpensesReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var existing = await _context.ExpenseTbl.FirstOrDefaultAsync(p => p.ExpenseName == req.ExpenseName && p.CompanyId == SchoolId);
                if (existing != null)
                {
                    return ApiResponse<bool>.ErrorResponse("This expenses name if already insert");
                }
                int ExpensesId = _context.ExpenseTbl.DefaultIfEmpty().Max(s => s == null ? 0 : s.ExpenseId) + 1;

                var expensesentity = new ExpenseTbl
                {
                    ExpenseId = ExpensesId,
                    ExpenseName = req.ExpenseName,
                    Active = true,
                    //   CreateDate = DateTime.Now,
                    //  UpdateDate = DateTime.Now,
                    CompanyId = SchoolId,
                    SessionId = SessionId,
                    Userid = UserId,
                };

                _context.ExpenseTbl.Add(expensesentity);
                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "Expenses data saved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateExpenses(UpdateExpensesModel req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;

                ExpenseTbl expenses = await _context.ExpenseTbl.Where(a => a.CompanyId == SchoolId && a.ExpenseName == req.ExpenseName && a.ExpenseId != req.ExpensesId).FirstOrDefaultAsync();
                if (expenses != null)
                {
                    return ApiResponse<bool>.ErrorResponse("Expenses name already available.");
                }

                var result = await _context.ExpenseTbl.Where(c => c.ExpenseId == req.ExpensesId && c.CompanyId == SchoolId).FirstOrDefaultAsync();

                result.ExpenseName = req.ExpenseName;
                result.Userid = UserId;
          //      result.UpdateDate = DateTime.Now;
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Expenses update successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> ChangestatusExpenses(int ExpensesId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;

                var expensesentity = await _context.ExpenseTbl.FirstOrDefaultAsync(p => p.ExpenseId == ExpensesId && p.CompanyId == SchoolId);
                if (expensesentity == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Expenses not found ");
                }

                expensesentity.Active = expensesentity.Active == null ? true : !expensesentity.Active;

                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "status update successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<GetSchoolExpenses>>> Getschlexpenses()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var schoolexpensesentity = await _context.SchlExpenseTbl.Where(a => a.CompanyId == SchoolId)
                    .Select(a => new GetSchoolExpenses
                    {
                        SEId = a.SchlExpenseId,
                        ExpensesId = a.ExpenseId,
                        RerNo = a.RefNo,
                        InvoiceDate = a.InvoiceDate,
                        InvoiceNo = a.InvoiceNo,
                        Amount = a.Amount,
                        Description = a.Description,
                        Date = a.Date,
                        Active = a.Active,
                        ExpensesName = _context.ExpenseTbl.Where(p => p.ExpenseId == a.ExpenseId && p.CompanyId == SchoolId).Select(p => p.ExpenseName).FirstOrDefault(),

                    }).ToListAsync();

                return ApiResponse<List<GetSchoolExpenses>>.SuccessResponse(schoolexpensesentity, "Expenses list fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetSchoolExpenses>>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> InsertSchoolExpenses(SchoolExpensesReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                int SEId = _context.SchlExpenseTbl.DefaultIfEmpty().Max(r => r == null ? 0 : r.SchlExpenseId) + 1;

                var SchlExpenses = new SchlExpenseTbl
                {
                    SchlExpenseId = SEId,
                    ExpenseId = req.ExpensesId,
                    InvoiceNo = req.InvoiceNo,
                    InvoiceDate = req.InvoiceDate,
                    Amount = req.Amount,
                    Description = req.Description,
                    Date = req.Date,
                    Active = true,
                    CompanyId = SchoolId,
                    Userid = UserId,
                    SessionId = SessionId,
                  //  CreateDate = DateTime.Now,
                  //  UpdateDate = DateTime.Now,
                };

                // Generate ReceiptNo
                var existingRefno = _context.SchlExpenseTbl.Where(s => s.CompanyId == SchoolId).OrderByDescending(s => s.SchlExpenseId).FirstOrDefault();

                if (existingRefno == null)
                {
                    SchlExpenses.RefNo = "1";
                }
                else
                {
                    int rno = Convert.ToInt32(existingRefno.RefNo) + 1;
                    SchlExpenses.RefNo = rno.ToString();
                }


                _context.SchlExpenseTbl.Add(SchlExpenses);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "School Expenses data insert successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateSchlExpenses(UpdateschlExpenses req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;

                var result = await _context.SchlExpenseTbl.Where(a => a.SchlExpenseId == req.SEId && a.CompanyId == SchoolId).FirstOrDefaultAsync();

                if (result != null)
                {
                    result.ExpenseId = req.ExpensesId;
                    result.InvoiceNo = req.InvoiceNo;
                    result.InvoiceDate = req.InvoiceDate;
                    result.Amount = req.Amount;
                    result.Description = req.Description;
                    result.Date = req.Date;
                    result.Active = true;
              //      result.UpdateDate = DateTime.Now;
                    result.Userid = UserId;
                }
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "School Expenses update successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<GetSchoolExpenses>>> GetSchlExpensesList(GetSchoolExpensesListReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var result = await _context.SchlExpenseTbl.Where(a => (req.ExpensesId == -1 ? true : a.ExpenseId == req.ExpensesId) &&
                (req.FromDate == null ? true : a.Date >= req.FromDate) && (req.ToDate == null ? true : a.Date <= req.ToDate)
                && a.Active == true && a.CompanyId == SchoolId && a.SessionId == SessionId)
                    .Select(a => new GetSchoolExpenses
                    {
                        SEId = a.SchlExpenseId,
                        RerNo = a.RefNo,
                        ExpensesId = a.ExpenseId,
                        InvoiceDate = a.InvoiceDate,
                        InvoiceNo = a.InvoiceNo,
                        Amount = a.Amount,
                        Description = a.Description,
                        Date = a.Date,
                        Active = a.Active,
                        ExpensesName = _context.ExpenseTbl.Where(p => p.ExpenseId == a.ExpenseId && p.CompanyId == SchoolId).Select(p => p.ExpenseName).FirstOrDefault(),

                    }).ToListAsync();

                return ApiResponse<List<GetSchoolExpenses>>.SuccessResponse(result, "School Expenses list fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetSchoolExpenses>>.ErrorResponse("Error: " + ex.Message);
            }
        }


    }
}
