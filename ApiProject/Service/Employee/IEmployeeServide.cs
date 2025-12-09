using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using Microsoft.Data.SqlClient.DataClassification;
using OfficeOpenXml.Sorting;

namespace ApiProject.Service.Employee
{
    public interface IEmployeeServide
    {
        // Employee detail stsrt
        Task<ApiResponse<List<GetEmployeeModel>>> GetEmployeeLit();
        Task<ApiResponse<bool>> AddEmployeeDetail(AddEmployeeDetailReq req);
        Task<ApiResponse<bool>> UpdateEmoloyeeDetail(UpdatreEmployeeDetailReq req);
        Task<ApiResponse<List<GetEmployeeListModel>>> EmployeeReport(GetEmployeReq req);
        Task<ApiResponse<bool>> ChangeStatusEmployee(int EmpId);
        // Employee detail end

        // Employee Work allocation start 
        Task<ApiResponse<GetEmpClassbySubjectModel>> GetEmployeeNdClassBySubjectData(int EmpId, int ClassId);
        Task<ApiResponse<bool>> AddEmpWorkAllocation(AddWorkallcation req);
        Task<ApiResponse<List<GetEmpWorkAllocationModel>>> EmpWorkallocationReport(GetEmployeReq req);
        // Employee Work allocation End 

        // Employee Attendance start 
        Task<ApiResponse<List<EmpAttendanceDetail>>> GetEmployeeAttendance(getEmployeeAttendance req);
        Task<ApiResponse<bool>> InsertEmployeeAttendance(List<addEmployeeAttendanceReq> EAttendance);
        Task<ApiResponse<List<EmpAttendanceReportModel>>> EmployeeAttendanceReport(int Month);

        // Employee Salary
        Task<ApiResponse<List<GetAdvsalaryModel>>> GetAdvanceSalaryList();
        Task<ApiResponse<bool>> AddAdvanceSalary(AddAdavanceSalaryReq req);
        Task<ApiResponse<List<EmployeeSalaryModel>>> GetEmployeeSalary(int month);

        Task<ApiResponse<bool>> AddEmployeeSalary(List<AddEmployeeSalaryModel> model);
    }
}
