using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using ApiProject.Service.Current;
using AutoMapper;
using Azure;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Net;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiProject.Service.StudentAttendance
{
    public class StudentAttendanceService : IStudentAttendanceService
    {
        private readonly ILoginUserService _loginUser;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public StudentAttendanceService(
            ILoginUserService loginUser,
            ApplicationDbContext context,
            IMapper mapper
            )
        {
            _context = context;
            _loginUser = loginUser;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<StudentAttendanceListResModel>>> getclassbystudent(GetStudentAttendanceReqModel req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.StudentRenewView.Where(c => c.ClassId == req.ClassId && (req.SectionId == -1 || c.SectionId == req.SectionId)
                             && c.RActive == true && c.SessionId == SessionId && c.CompanyId == SchoolId).OrderBy(c => c.stu_name)
                    .Select(c => new StudentAttendanceListResModel
                    {
                        ClassId = c.ClassId,
                        SectionId = c.SectionId,
                        //   ClassName = c.ClassName,
                        //  SectionName = c.SectionName,
                        stu_name = c.stu_name,
                        StudentId = c.StuId,
                        RollNo = c.RollNo,
                        SRNo = c.registration_no,
                        Status = _context.Student_Attendance.Where(p => p.StudentId == c.StuId && p.ClassId == c.ClassId && p.SessionId == SessionId && p.CompanyId == SchoolId && p.Date == req.Date).Select(p => p.Status).FirstOrDefault(),
                        Note = _context.Student_Attendance.Where(p => p.StudentId == c.StuId && p.ClassId == c.ClassId && p.SessionId == SessionId && p.CompanyId == SchoolId && p.Date == req.Date).Select(p => p.Note).FirstOrDefault()
                    })
                    .ToListAsync();

                return ApiResponse<List<StudentAttendanceListResModel>>.SuccessResponse(res);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<StudentAttendanceListResModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> InsertStudentAttendance(List<StudentAttendanceReqModel> req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                if (req != null && req.Count > 0)
                {
                    int? classid = req[0].ClassId;
                    DateTime? date = req[0].Date;

                    // Delete existing attendance
                    var attendancedelete = await _context.Student_Attendance
                        .Where(k => k.SessionId == SessionId && k.ClassId == classid && k.Date == date && k.CompanyId == SchoolId)
                        .ToListAsync();


                    if (attendancedelete.Any())
                    {
                        _context.Student_Attendance.RemoveRange(attendancedelete);
                    }

                    List<Student_Attendance> attendanceList = req.Select(item => new Student_Attendance
                    {
                        StudentId = item.StudentId,
                        ClassId = item.ClassId,
                        Status = item.Status,
                        Note = item.Note == null ? "" : item.Note,
                        CompanyId = SchoolId,
                        Userid = UserId,
                        SessionId = SessionId,
                        Date = item.Date,
                        Time = DateTime.Now.TimeOfDay,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    }).ToList();

                    _context.Student_Attendance.AddRange(attendanceList);
                    await _context.SaveChangesAsync();

                    return ApiResponse<bool>.SuccessResponse(true);
                }

                return ApiResponse<bool>.ErrorResponse("Invalid request: Empty data.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<StudentMonthlyAttendanceResModel>>> getmonthlyattendance(StudentMonthlyAttendanceReqModel req)
        {
            try
            {
                int schoolId = _loginUser.SchoolId;
                int sessionId = _loginUser.SessionId;

                // All students of class/section
                var students = await _context.StudentRenewView.Where(s => s.ClassId == req.ClassId && s.RActive == true && s.SessionId == sessionId && s.CompanyId == schoolId)
                    .OrderBy(s => s.stu_name)
                    .ToListAsync();

                // Get attendance for the full month
                var year = DateTime.Now.Year;
                var startDate = new DateTime(year, req.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var attendance = await _context.Student_Attendance.Where(a => a.ClassId == req.ClassId && a.SessionId == sessionId && a.CompanyId == schoolId && a.Date >= startDate && a.Date <= endDate)
                    .ToListAsync();

                var response = students.Select(student =>
                {
                    var dailyAttendance = Enumerable.Range(1, endDate.Day).ToDictionary(
                        day => day,
                        day => attendance.FirstOrDefault(a => a.StudentId == student.StuId && a.Date?.Day == day)?.Status ?? ""
                    );

                    var model = new StudentMonthlyAttendanceResModel
                    {
                        StudentId = student.StuId,
                        StuName = student.stu_name,
                        classid = student.ClassId,
                        sectionid = student.SectionId,
                          ClassName = _context.University.Where(a => a.university_id == student.ClassId).Select(a => a.university_name).FirstOrDefault(),
                         SectionName = _context.collegeinfo.Where(a => a.collegeid == student.SectionId ).Select(a => a.collegename).FirstOrDefault(),
                        SRNo = student.registration_no,

                        AttendanceByDate = dailyAttendance,
                        TotalP = dailyAttendance.Values.Count(x => x == "P"),
                        TotalA = dailyAttendance.Values.Count(x => x == "A"),
                        TotalH = dailyAttendance.Values.Count(x => x == "H"),
                        TotalHF = dailyAttendance.Values.Count(x => x == "HF"),
                        TotalL = dailyAttendance.Values.Count(x => x == "L"),
                    };

                    return model;
                }).ToList();


                return ApiResponse<List<StudentMonthlyAttendanceResModel>>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<StudentMonthlyAttendanceResModel>>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<StudentAttendanceResModel>> studentattendance(string srno, int schoolid)
        {
            try
            {
                var student = _context.StudentRenewView.FirstOrDefault(c => c.registration_no == srno && c.CompanyId == schoolid);
                if (student != null)
                {
                    DateTime today = DateTime.Today;
                    TimeSpan nowTime = DateTime.Now.TimeOfDay;
                    var studentAttendance = _context.Student_Attendance.FirstOrDefault(s => s.CompanyId == schoolid && s.StudentId == student.stu_id && s.Date == today);

                    if (studentAttendance == null)
                    {
                        var stuatt = new Student_Attendance
                        {
                            StudentId = student.StuId,
                            ClassId = student.ClassId ?? 0,
                            Status = "Present",
                            Time = nowTime,
                            Note = "",
                            CompanyId = schoolid,
                            Userid = 1,
                            Date = today,
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now
                        };
                        _context.Student_Attendance.Add(stuatt);
                        _context.SaveChanges();
                    }

                    //else if (studentAttendance.Time == null)
                    //{
                    //    TimeSpan timeSinceInTime = nowTime - studentAttendance.InTime.Value;
                    //    if (timeSinceInTime.TotalMinutes < 5)
                    //    {

                    //        return ApiResponse<StudentAttendanceResModel>.ErrorResponse("Attendance already recorded. Please wait at least 5 minutes.");


                    //    }
                    //    studentAttendance.Time = nowTime;

                    //    TimeSpan duration = studentAttendance.Time.Value - studentAttendance.InTime.Value;

                    //    studentAttendance.Status = duration.TotalHours >= 6 ? "Present" :
                    //                               duration.TotalHours >= 3 ? "Half Day" : "Absent";

                    //    studentAttendance.UpdateDate = DateTime.Now;
                    //    _context.SaveChanges();
                    //}
                    else
                    {
                        return ApiResponse<StudentAttendanceResModel>.ErrorResponse("Attendance already recorded.");
                    }
                    var data = new StudentAttendanceResModel()
                    {
                        stu_name = student.stu_name,
                        SRNo = student.registration_no,
                        //  ClassName = student.ClassName,
                    };
                    return ApiResponse<StudentAttendanceResModel>.SuccessResponse(data);
                }
                else
                {
                    return ApiResponse<StudentAttendanceResModel>.ErrorResponse("Student not found.");
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<StudentAttendanceResModel>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<TodayAttendancePercentageRes>> todaystudentattendance()
        {
            try
            {
                int schoolId = _loginUser.SchoolId;
                int sessionId = _loginUser.SessionId;

                var today = DateTime.Today;

                var todayAttendance = await _context.Student_Attendance.Where(a => a.Date.Value.Date == today && a.CompanyId == schoolId && a.SessionId == a.SessionId).ToListAsync();

                int totalStudents = todayAttendance.Count;
                int presentCount = todayAttendance.Count(a => a.Status != "A");
                int absentCount = todayAttendance.Count(a => a.Status == "A");

                double presentPercentage = totalStudents > 0 ? (presentCount * 100.0) / totalStudents : 0;
                double absentPercentage = totalStudents > 0 ? (absentCount * 100.0) / totalStudents : 0;

                var data = new TodayAttendancePercentageRes
                {
                    TotalStudents = totalStudents,
                    PresentCount = presentCount,
                    AbsentCount = absentCount,
                    PresentPercentage = Math.Round(presentPercentage, 2),
                    AbsentPercentage = Math.Round(absentPercentage, 2)
                };

                return ApiResponse<TodayAttendancePercentageRes>.SuccessResponse(data);

            }
            catch (Exception ex)
            {

                return ApiResponse<TodayAttendancePercentageRes>.ErrorResponse("Error: " + ex.Message);

            }
        }
    }
}
