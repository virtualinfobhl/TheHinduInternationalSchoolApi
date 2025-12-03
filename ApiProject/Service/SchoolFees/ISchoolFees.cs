using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;

namespace ApiProject.Service.SchoolFees
{
    public interface ISchoolFees
    {
        // Student Class fee
        Task<ApiResponse<List<ClassFeesRes>>> GetClassFees();
        Task<ApiResponse<bool>> AddClassFees(AddClassFeesReq req);
        Task<ApiResponse<bool>> UpdateClassFees(UpdateClassFeesReq req);
        Task<ApiResponse<bool>> ChangeStatusClassFees(int Feesid);

        // fees installment
        Task<ApiResponse<List<FeesInstRes>>> GetFeeInstallment();
        Task<ApiResponse<bool>> insertfeesinstallment(List<AddFeesInstallmentReq> req);
        Task<ApiResponse<bool>> updatefeesinstallment(List<AddFeesInstallmentReq> req);


        // fees collection

        Task<ApiResponse<List<ClassIdByStudentRes>>> getclassbystudent(int classid);

        Task<ApiResponse<StudentFeesDetailRes>> getstudentfeesdetail(StudentFeesDetailReq req);

        Task<ApiResponse<StudentFeesRes>> insertstudentfees(StudentFeesReq req);

        Task<ApiResponse<StudentFeesReceiptRes>> getfeereceipt(int receiptId);

        //report
        Task<ApiResponse<PagedResult<StudentFeesCollectionListRes>>> GetDailyFeeCollection(FeesCollectionReq req);
        Task<ApiResponse<PagedResult<ClassFeesListRes>>> getclassfees(ClassFeesFilterReq req);
        Task<ApiResponse<ClassWiseTotalFeeModel>> GetClasswiseTotalFee(int ClassId);
        Task<ApiResponse<PagedResult<ClassFeesInstaListRes>>> getclassfeesInstallment(ClassFeesFilterReq req);









    }
}
