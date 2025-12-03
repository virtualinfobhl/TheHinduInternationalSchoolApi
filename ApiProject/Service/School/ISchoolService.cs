using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ApiProject.Service.School
{
    public interface ISchoolService
    {

        // ****** School Informaction
        #region
        Task<ApiResponse<SchoolDetail>> SchoolDetail();
        Task<SchoolDetail> schooldetailupdate(SchoolUpdate update);
        Task<ApiResponse<List<State>>> GetState();
        Task<ApiResponse<List<DistrictResModel>>> GetDistrict();
        #endregion


        // ********* user detail Code
        #region
        Task<ApiResponse<List<GetUserReqmodel>>> GetUserDetail();
        Task<ApiResponse<bool>> UpdateUser(UpdateUserReq req);
        Task<ApiResponse<bool>> changestatusUser(int UserId);
        #endregion


        // ************** CLass code start
        #region
        Task<ApiResponse<List<ClassALLReqModel>>> getclassList();
        Task<ApiResponse<bool>> insertclass(AddClassReq request);
        Task<ApiResponse<bool>> updateclass(UpdateClassReq request);
        Task<ApiResponse<bool>> changestatusclass(int classid);
        #endregion


        // ************** section code start
        #region
        Task<ApiResponse<List<ClassSectionResModel>>> getsection();
        Task<ApiResponse<bool>> insertsection(AddSectionReq request);
        Task<ApiResponse<bool>> updatesection(UpdateSectionReq request);
        Task<ApiResponse<bool>> changestatussection(int sectionid);
        #endregion


        // ************** Subject code start
        #region
        Task<ApiResponse<List<SubjectResModel>>> getsubject();
        Task<ApiResponse<bool>> insertsubject(AddSubjectReq request);
        Task<ApiResponse<bool>> updatesubject(UpdateSubjectReq request);
        Task<ApiResponse<bool>> changestatussubject(int sectionid);
        #endregion


        // *************** grade code start
        #region
        Task<ApiResponse<List<GradeReqModel>>> GetGradeList();
        Task<ApiResponse<bool>> insertgrade(AddGradeReq request);
        Task<ApiResponse<bool>> updategrade(UpdateGradeReq request);
        Task<ApiResponse<bool>> changestatusgrade(int sectionid);
        #endregion


        // *****************  Event Code Start
        #region
        Task<ApiResponse<List<GetEventReqModel>>> GetEvent();
        Task<ApiResponse<bool>> InsertEvent(EventReqModel req);
        Task<ApiResponse<bool>> UpdateEvent(UpdateEventReqModel req);
        Task<ApiResponse<bool>> ChangeEventStatus(int EventId);
        #endregion


        // ***************** Exam Code S
        #region
        //Task<ApiResponse<List<GetExamReqModel>>> GetExam();
        //Task<ApiResponse<bool>> InsertExam(ExamreqModel req);
        //Task<ApiResponse<bool>> UpdateExam(UpdateExamreqModel req);
        //Task<ApiResponse<bool>> ExamChangeStatus(int ExamId);
        #endregion



        // ******************* Exam Marks Code S
        #region
        //Task<ApiResponse<List<GetClassbysubjectMarksmodel>>> GetExamTotalMarks();
        //Task<ApiResponse<bool>> insertExamTotalMarks(ExamMarksModel req);
        #endregion

    }
}
