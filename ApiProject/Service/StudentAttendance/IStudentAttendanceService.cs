using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;

namespace ApiProject.Service.StudentAttendance
{
    public interface IStudentAttendanceService
    {
        Task<ApiResponse<bool>> InsertStudentAttendance(List<StudentAttendanceReqModel> req);

        Task<ApiResponse<List<StudentAttendanceListResModel>>> getclassbystudent(GetStudentAttendanceReqModel req);


        Task<ApiResponse<List<StudentMonthlyAttendanceResModel>>> getmonthlyattendance(StudentMonthlyAttendanceReqModel req);


      //  Task<ApiResponse<StudentAttendanceResModel>> studentattendance(string srno,int schoolid);


    //    Task<ApiResponse<TodayAttendancePercentageRes>> todaystudentattendance();




    }
}
