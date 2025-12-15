using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ApiProject.Service.Parents
{
    public interface IParentsService
    {
        Task<ApiResponse<List<GetStudentModel>>> GetStudentList();
        Task<ApiResponse<GetParentDetailsModel>> GetStudentParentsDetail();
        Task<ApiResponse<Getparentsreq>> GetStudentToken(int StudentId);
        Task<ApiResponse<getStudentInstallmentModel>> GetStudentInstallmentFee();
        Task<ApiResponse<GetTransportInstallFeeModel>> GetTransportInstallFee();
        Task<ApiResponse<StudentFeePaymentResult>> AddStudentInstallmentFee(AddStudentinstallReq Req);


        Task<ApiResponse<bool>> UpdateStudentPaymentSuccessfully(int StudentId, int ReceiptId);
        Task<ApiResponse<bool>> AddStudentTransportFee(AddTransportMonthFeeReq req);
        Task<ApiResponse<bool>> UpdateTransportPaymentSuccessfully(int StudentId, int ReceiptId);
        Task<ApiResponse<GetStudentFeeModel>> GetStudentFee();
        Task<ApiResponse<GetStuFeeInstallmentModel>> GetStudentFeeInstallment();
        Task<ApiResponse<GetStuDueInstallmentModel>> GetStudentDueInstallment();
    }
}
