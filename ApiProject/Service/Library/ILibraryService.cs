using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using Microsoft.Data.SqlClient.DataClassification;

namespace ApiProject.Service.Library
{
    public interface ILibraryService
    {
        Task<ApiResponse<List<getBooks>>> GetBoodData();
        Task<ApiResponse<bool>> Addbook(AddBookReq req);
        Task<ApiResponse<bool>> Updatebook(UpdateBookReq req);
        Task<ApiResponse<bool>> ChangeStatusBook(int BookId);
        Task<ApiResponse<List<GetLibraryStudentModel>>> GetLibraryStudent(BulkStudentReq req);
        Task<ApiResponse<List<GetlibrarayEmployeeModel>>> GetLibraryEmployee();
        Task<ApiResponse<bool>> AddLibraryCardNo(AddcardNoReq req);
        Task<ApiResponse<List<GetMemberListModel>>> GetMemberList();
        Task<ApiResponse<GetTClassbySectionNdStudent>> GetLibraryClassBySectionNdStudent(int ClassId);
        Task<ApiResponse<List<TStudentDataList>>> GetLibrarySectionByStudent(int ClassId, int SectionId);
        Task<ApiResponse<List<GetempDetails>>> GetEmployeeById(int EmpId);
        Task<ApiResponse<List<getMenmberProfileModel>>> getMenmberProfileList(int LibraryId);
        Task<ApiResponse<List<GetAuthormodel>>> GetAuthorByBookId(int BookId);
        Task<ApiResponse<bool>> AddIssueLibraryBook(IssueLibraryBookReq req);
        Task<ApiResponse<bool>> AddReturnDate(UpdateReturndateReq req);
        Task<ApiResponse<List<GetLibraryReportModel>>> GetLibraryReport(LibrartReportReq rea);
        Task<ApiResponse<List<GetBookReportReq>>> GetBookReport();
        
    }
}
