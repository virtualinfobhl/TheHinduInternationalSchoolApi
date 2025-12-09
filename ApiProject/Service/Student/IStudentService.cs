using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using Microsoft.Data.SqlClient.DataClassification;

namespace ApiProject.Service.Student
{
    public interface IStudentService
    {
        // student details
        Task<List<ClassResModel>> GetClass();
        Task<ApiResponse<FeendSectionByClasssModel>> GetClassByFeendSection(int ClassId);
        Task<ClassFeeResModel> GetClassByFee(int classid);
        Task<ApiResponse<GetParentsDetailModel>> GetParentsByMobileNo(string Mobileno);
        Task<ApiResponse<quickadmissionres>> AddStuQuickadmission(quickadmissionmodel request);
        Task<ApiResponse<quickadmissionres>> AddStudentAdmissionAsync(AddStudentReqModel request);
        Task<ApiResponse<quickadmissionres>> updatestudentdata(StudentUpdateReqModel request);

     //   Task<ApiResponse<quickadmissionres>> UpdateStuinstallment(FeeInstallmentReqMOdel request);
        Task<ApiResponse<bool>> studentexcelupload(List<StudentExcelUploadListReq> request);


        // Show Student Bulk Edit
        Task<ApiResponse<List<StudentRollNoResponse>>> ShowStudentBulkEdit(BulkStudentReq request);
        Task<ApiResponse<bool>> UpdateBulkStudentAsync(List<studentRollNoAttendaceReq> request);

        // student fee discount
        Task<ApiResponse<List<StudentDiscountFeeResponse>>> ShowStudentFeeDiscont(BulkStudentReq request);
        Task<ApiResponse<bool>> AddStudentDiscountFee(List<studentDiscountfeeReq> request);


        // student presonality
        Task<ApiResponse<List<StudentPersonalResponse>>> ShowStudentPersonality(BulkStudentReq request);
        Task<ApiResponse<bool>> AddStudentPersonalAsync(List<stuPersonalModelReq> req);


        // Exam Marks data

        Task<ApiResponse<GetClassbySectionSubject>> GetClassbySectionNdSubject(int ClassId);
        Task<ApiResponse<GetSubjectModel>> GetMarksType(int SubjectId);
        Task<ApiResponse<List<StudentexamMarksResponse>>> ShowTestMarksDetails(studentexamMarksReq request);
        Task<ApiResponse<bool>> UpdateStudentMarks(UpdateStudentMarksRequest request);
        Task<ApiResponse<bool>> ExcelStudentMarks(examMarksmodel request);

        // Event Code 
        Task<ApiResponse<List<StudentEventCertificate>>> GetEventDataByIdAsync(int EventId);
        Task<ApiResponse<List<UpdateEventReqModel>>> GetEventList();
        Task<ApiResponse<bool>> AddEventCertificateAsync(EventCartificateModelReq req);
        Task<ApiResponse<GetClassbySectionNdStudent>> GetClassBySectionNdStudent(int ClassId);
        Task<ApiResponse<List<StudentDataList>>> GetSectionByStudentDetail(int ClassId, int SectionId);
        Task<ApiResponse<StudentDetailsById>> GetStudentDetailById(int StudentId);

        Task<ApiResponse<StudentFeeTCModel>> GetStudentDueFeeTC(int StudentId);
        Task<ApiResponse<bool>> GenerateTC(GetStudentTCDropoutReq req);
        Task<ApiResponse<bool>> StudentDropout(GetStudentTCDropoutReq req);

        // Task<ApiResponse<bool>>



        Task<ApiResponse<StudentClassExamData>> GetClassSubjectAsync(int ClassId);
        Task<ApiResponse<List<StudentExamData>>> GetClassExamSubjectAsync(ClassExamMarksModelreq request);
        Task<PagedResult<GetQuickStudentReqModel>> GetQuickStudentList(getstudentlistReq req);
        Task<ApiResponse<List<ClassSectionResModel>>> GetClassBySection(int classid);
        Task<PagedResult<stduentlistres>> GetStudentList(getstudentlistReq request);
        Task<ApiResponse<List<studentreesponse>>> GetSectionbyStudent(int sectionid);
        Task<ApiResponse<getStudentListModel>> getstudentdatabyid(int studentId);
        Task<ApiResponse<ClassByFeeInResponse>> GetClassByFeeInsAsync(int ClassId);


    }
}
