using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using Microsoft.Data.SqlClient.DataClassification;

namespace ApiProject.Service.Report
{
    public interface IReportService
    {

        Task<ApiResponse<List<GetStudentQuickListModel>>> GetQuickStudentReport(getstudentDellistReq req);
        Task<ApiResponse<List<GetStudentDetailsLisModel>>> GetStudentDetailReport(GetStudentReq req);
        Task<ApiResponse<List<GetStudentDetailsLisModel>>> GetStudentIDCardReport(GetStudentIDCardReq req);
        Task<ApiResponse<List<GetStudentFeeDetailsModel>>> GetStudentFeeReport(getstudentDellistReq req);
        Task<ApiResponse<List<ClasswiseInstallModel>>> GetClassWiseInstallmentReport(BulkStudentReq req);
        Task<ApiResponse<List<ClasswiseDueeFeeModel>>> GetClasswiseDueFeeReport(BulkStudentReq req);
        Task<ApiResponse<List<AllClasswiseDueeFeeModel>>> GetAllClasswiseDueFeeReport(BulkStudentReq req);
        Task<ApiResponse<List<GetStudentDetailsLisModel>>> GetStudentTCReport(GetStudentIDCardReq req);
        Task<ApiResponse<List<GetStudentDetailsLisModel>>> GetStudentDropoutReport(GetStudentIDCardReq req);

        Task<ApiResponse<List<TestExamMarksmOdel>>> GetTestExamMarks(GetTestExamReq req);
        Task<ApiResponse<List<TestExamMarksmOdel>>> GetTotalTestExamReport(GetTestExamReq req);

        Task<ApiResponse<List<StudentMarksheetModel>>> GetStudentMarksheet(GetTestExamReq req);







        Task<PagedResult<GetStudentFeeListModel>> GetStudentFeeDetail(GetStudentFeeListReqModel req);
        //    Task<ApiResponse<List<GetStudentFeeListModel>>> GetStudentFeeListDetail(GetStudentFeeReqModel req);
        //  Task<ApiResponse<List<ClasswiseStudentListModel>>> GetClassWiseInstall(GetClasswiseInstallmentListReq req);

        // Task<ApiResponse<List<DailyCollectionModel>>> DailyCollectionList(DailyCollectionReportReq req);


    }
}
