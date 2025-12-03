using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using Microsoft.Data.SqlClient.DataClassification;

namespace ApiProject.Service.SchoolExpenses
{

    public interface ISchoolExpensesService
    {
        Task<ApiResponse<List<GetExpenses>>> GetExpensesList();
        Task<ApiResponse<bool>> AddExpenses(ExpensesReq req);
        Task<ApiResponse<bool>> UpdateExpenses(UpdateExpensesModel req);
        Task<ApiResponse<bool>> ChangestatusExpenses(int ExpensesId);
        Task<ApiResponse<List<GetSchoolExpenses>>> Getschlexpenses();
        Task<ApiResponse<bool>> InsertSchoolExpenses(SchoolExpensesReq req);
        Task<ApiResponse<bool>> UpdateSchlExpenses(UpdateschlExpenses req1);
        Task<ApiResponse<List<GetSchoolExpenses>>> GetSchlExpensesList(GetSchoolExpensesListReq req);

    }
}
