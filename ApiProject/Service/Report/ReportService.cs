using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using ApiProject.Service.Current;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using System.Diagnostics;
using System.Linq;

namespace ApiProject.Service.Report
{
    public class ReportService : IReportService
    {
        private readonly ILoginUserService _loginUser;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReportService(
            ILoginUserService loginUser,
            ApplicationDbContext context,
            IMapper mapper
            )
        {
            _context = context;
            _loginUser = loginUser;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<GetStudentQuickListModel>>> GetQuickStudentReport(getstudentDellistReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.StudentRenewView.Where(c => (req.ClassId == -1 ? true : c.ClassId == req.ClassId)
                && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && (req.studentId == -1 ? true : c.StuId == req.studentId)
                && (string.IsNullOrEmpty(req.srno) || c.registration_no == req.srno)
                && c.RActive == true && c.StuDetail == false && c.StuFees == false && c.SessionId == SessionId && c.CompanyId == SchoolId)
                    .OrderBy(c => c.stu_name).Select(c => new GetStudentQuickListModel
                    {
                        stu_id = c.StuId,
                        ClassId = c.ClassId,
                        SectionId = c.SectionId,
                        stu_name = c.stu_name,
                        Srno = c.registration_no,
                        stu_code = c.stu_code,
                        DOB = c.DOB,
                        RTE = c.RTE,
                        admission_date = c.admission_date,
                        FatherName = c.FatherName,
                        FatherMobileNo = c.FatherMobileNo,
                        MotherName = c.mother_name,

                        ClassName = _context.University.Where(a => a.university_id == c.ClassId && a.CompanyId == SchoolId).Select(a => a.university_name).FirstOrDefault(),
                        SectionName = _context.collegeinfo.Where(a => a.collegeid == c.SectionId && a.CompanyId == SchoolId).Select(a => a.collegename).FirstOrDefault(),


                    }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<GetStudentQuickListModel>>.ErrorResponse("No student fee found data");
                }

                return ApiResponse<List<GetStudentQuickListModel>>.SuccessResponse(res, "Fetch student fee data successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetStudentQuickListModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<GetStudentDetailsLisModel>>> GetStudentDetailReport(GetStudentReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.StudentRenewView.Where(c => (req.ClassId == -1 ? true : c.ClassId == req.ClassId)
                && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && (req.StudentId == -1 ? true : c.StuId == req.StudentId)
                && (string.IsNullOrEmpty(req.srno) || c.registration_no == req.srno)
                && c.RActive == true && c.StuDetail == true && c.StuFees == true && c.SessionId == SessionId && c.CompanyId == SchoolId)
                    .OrderBy(c => c.stu_name).Select(c => new GetStudentDetailsLisModel
                    {
                        stu_id = c.StuId,
                        stu_photo = c.stu_photo,
                        ClassId = c.ClassId,
                        SectionId = c.SectionId,
                        stu_name = c.stu_name,
                        Srno = c.registration_no,
                        stu_code = c.stu_code,
                        Address = c.address,
                        DOB = c.DOB,
                        RTE = c.RTE,
                        admission_date = c.admission_date,
                        FatherName = c.FatherName,
                        FatherMobileNo = c.FatherMobileNo,
                        MotherName = c.mother_name,

                        ClassName = _context.University.Where(a => a.university_id == c.ClassId && a.CompanyId == SchoolId).Select(a => a.university_name).FirstOrDefault(),
                        SectionName = _context.collegeinfo.Where(a => a.collegeid == c.SectionId && a.CompanyId == SchoolId).Select(a => a.collegename).FirstOrDefault(),


                    }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<GetStudentDetailsLisModel>>.ErrorResponse("No student fee found data");
                }

                return ApiResponse<List<GetStudentDetailsLisModel>>.SuccessResponse(res, "Fetch student fee data successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetStudentDetailsLisModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<GetStudentDetailsLisModel>>> GetStudentIDCardReport(GetStudentIDCardReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.StudentRenewView.Where(c => (req.ClassId == -1 ? true : c.ClassId == req.ClassId)
                && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && c.RActive == true && c.StuDetail == true && c.StuFees == true
                && c.SessionId == SessionId && c.CompanyId == SchoolId).OrderBy(c => c.stu_name).Select(c => new GetStudentDetailsLisModel
                {
                    stu_id = c.StuId,
                    stu_photo = c.stu_photo,
                    ClassId = c.ClassId,
                    SectionId = c.SectionId,
                    stu_name = c.stu_name,
                    Srno = c.registration_no,
                    stu_code = c.stu_code,
                    Address = c.address,
                    DOB = c.DOB,
                    admission_date = c.admission_date,
                    FatherName = c.FatherName,
                    FatherMobileNo = c.FatherMobileNo,
                    MotherName = c.mother_name,

                    ClassName = _context.University.Where(a => a.university_id == c.ClassId && a.CompanyId == SchoolId).Select(a => a.university_name).FirstOrDefault(),
                    SectionName = _context.collegeinfo.Where(a => a.collegeid == c.SectionId && a.CompanyId == SchoolId).Select(a => a.collegename).FirstOrDefault(),


                }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<GetStudentDetailsLisModel>>.ErrorResponse("No student fee found data");
                }

                return ApiResponse<List<GetStudentDetailsLisModel>>.SuccessResponse(res, "Fetch student fee data successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetStudentDetailsLisModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<GetStudentFeeDetailsModel>>> GetStudentFeeReport(getstudentDellistReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.StudentRenewView.Where(c => (req.ClassId == -1 ? true : c.ClassId == req.ClassId)
                && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && (req.studentId == -1 ? true : c.StuId == req.studentId)
                && (string.IsNullOrEmpty(req.srno) || c.registration_no == req.srno) && c.RActive == true && c.StuDetail == true && c.StuFees == true
                && c.SessionId == SessionId && c.CompanyId == SchoolId).OrderBy(c => c.stu_name).Select(c => new GetStudentFeeDetailsModel
                {
                    stu_id = c.StuId,
                    ClassId = c.ClassId,
                    SectionId = c.SectionId,
                    stu_name = c.stu_name,
                    Srno = c.registration_no,
                    stu_code = c.stu_code,
                    FatherName = c.FatherName,
                    FatherMobileNo = c.FatherMobileNo,
                    MotherName = c.mother_name,
                    PayAdmissionFee = c.RAdmissionPayfee,
                    TotalFee = c.Rtotal,
                    FeeDiscount = c.Rdiscount,
                    DueOldFee = c.OldDuefees,
                    TotalNetFee = c.Rtotal_fee,
                    PaidFee = c.Rstu_fee,
                    DueFee = c.Rdue_fee,


                    ClassName = _context.University.Where(a => a.university_id == c.ClassId && a.CompanyId == SchoolId).Select(a => a.university_name).FirstOrDefault(),
                    SectionName = _context.collegeinfo.Where(a => a.collegeid == c.SectionId && a.CompanyId == SchoolId).Select(a => a.collegename).FirstOrDefault(),

                    FeeReceipt = _context.M_FeeDetail.Where(a => a.stu_id == c.StuId && a.ClassId == c.ClassId && a.SessionId == SessionId && a.CompanyId == SchoolId && a.Active == true)
                    .Select(a => new FeeReceiptModel
                    {
                        Receiptid = a.FDId,
                        ReceiptNo = a.ReceiptNo,
                        PayFees = a.PayFees,
                        PaymentDate = a.PaymentDate,
                        PaymentMode = a.PaymentMode,
                        FeeType = a.Status,
                        Remark = a.Remark,
                    }).ToList(),


                }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<GetStudentFeeDetailsModel>>.ErrorResponse("No student fee found data");
                }

                return ApiResponse<List<GetStudentFeeDetailsModel>>.SuccessResponse(res, "Fetch student fee data successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetStudentFeeDetailsModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<ClasswiseInstallModel>>> GetClassWiseInstallmentReport(BulkStudentReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.StudentRenewView.Where(c => (req.ClassId == -1 ? true : c.ClassId == req.ClassId)
                && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && c.RActive == true && c.StuDetail == true && c.StuFees == true
                && c.SessionId == SessionId && c.CompanyId == SchoolId).OrderBy(c => c.stu_name).Select(c => new ClasswiseInstallModel
                {
                    stu_id = c.StuId,
                    ClassId = c.ClassId,
                    SectionId = c.SectionId,
                    stu_name = c.stu_name,
                    Srno = c.registration_no,
                    RTE = c.RTE,
                    stu_code = c.stu_code,
                    FatherName = c.FatherName,
                    FatherMobileNo = c.FatherMobileNo,
                    MotherName = c.mother_name,
                    TotalNetFee = c.Rtotal_fee,
                    PaidFee = c.Rstu_fee,
                    DueFee = c.Rdue_fee,


                    ClassName = _context.University.Where(a => a.university_id == c.ClassId && a.CompanyId == SchoolId).Select(a => a.university_name).FirstOrDefault(),
                    SectionName = _context.collegeinfo.Where(a => a.collegeid == c.SectionId && a.CompanyId == SchoolId).Select(a => a.collegename).FirstOrDefault(),

                    Installments = _context.fee_installment.Where(a => a.stu_id == c.StuId && a.university_id == c.ClassId && a.SessionId == SessionId && a.CompanyId == SchoolId && a.active == true)
                    .Select(a => new ClasswiseInstallmentModel
                    {
                        Installment = a.Installment,
                        SInsAmount = a.due_fee,
                    }).ToList(),

                }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<ClasswiseInstallModel>>.ErrorResponse("No student fee found data");
                }

                return ApiResponse<List<ClasswiseInstallModel>>.SuccessResponse(res, "Fetch student fee data successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ClasswiseInstallModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<ClasswiseDueeFeeModel>>> GetClasswiseDueFeeReport(BulkStudentReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.StudentRenewView.Where(c => (req.ClassId == -1 ? true : c.ClassId == req.ClassId)
                && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && c.RActive == true && c.StuDetail == true && c.StuFees == true
                && c.SessionId == SessionId && c.CompanyId == SchoolId).OrderBy(c => c.stu_name).Select(c => new ClasswiseDueeFeeModel
                {
                    stu_id = c.StuId,
                    stu_name = c.stu_name,
                    Srno = c.registration_no,
                    FatherName = c.FatherName,
                    FatherMobileNo = c.FatherMobileNo,
                    MotherName = c.mother_name,
                    DueFee = c.Rdue_fee,


                    ClassName = _context.University.Where(a => a.university_id == c.ClassId && a.CompanyId == SchoolId).Select(a => a.university_name).FirstOrDefault(),
                    SectionName = _context.collegeinfo.Where(a => a.collegeid == c.SectionId && a.CompanyId == SchoolId).Select(a => a.collegename).FirstOrDefault(),



                }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<ClasswiseDueeFeeModel>>.ErrorResponse("No student fee found data");
                }

                return ApiResponse<List<ClasswiseDueeFeeModel>>.SuccessResponse(res, "Fetch student fee data successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ClasswiseDueeFeeModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<AllClasswiseDueeFeeModel>>> GetAllClasswiseDueFeeReport(BulkStudentReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var startDate = await _context.StuRouteAssignTbl.Where(c => c.university_id == req.ClassId && c.CompanyId == SchoolId
                     && c.SessionId == SessionId).Select(c => c.Date).FirstOrDefaultAsync();

                int startMonthNo = startDate?.Month ?? 1;           // route date ka month number (int)
                string startMonth = new DateTime(DateTime.Now.Year, startMonthNo, 1).ToString("MMMM");

                int currentMonthNo = DateTime.Now.Month;           // current month number (int)
                string currentMonth = new DateTime(DateTime.Now.Year, currentMonthNo, 1).ToString("MMMM");

                var validMonths = Enumerable.Range(startMonthNo, currentMonthNo - startMonthNo + 1).Select(m => new DateTime(DateTime.Now.Year, m, 1).ToString("MMMM")).ToList();


                var res = await _context.StudentRenewView.Where(c => (req.ClassId == -1 ? true : c.ClassId == req.ClassId)
                && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && c.RActive == true && c.StuDetail == true && c.StuFees == true
                && c.SessionId == SessionId && c.CompanyId == SchoolId).OrderBy(c => c.stu_name).Select(c => new AllClasswiseDueeFeeModel
                {
                    stu_id = c.StuId,
                    stu_name = c.stu_name,
                    Srno = c.registration_no,
                    FatherName = c.FatherName,
                    FatherMobileNo = c.FatherMobileNo,
                    MotherName = c.mother_name,
                    DueFee = c.Rdue_fee,


                    ClassName = _context.University.Where(a => a.university_id == c.ClassId && a.CompanyId == SchoolId).Select(a => a.university_name).FirstOrDefault(),
                    SectionName = _context.collegeinfo.Where(a => a.collegeid == c.SectionId && a.CompanyId == SchoolId).Select(a => a.collegename).FirstOrDefault(),

                    TransportDueFee = _context.TransInstallmentTbl.Where(a => a.StuId == c.StuId && a.CompanyId == SchoolId && validMonths.Contains(a.MonthName)).Sum(a => a.DueFee),

                }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<AllClasswiseDueeFeeModel>>.ErrorResponse("No student fee found data");
                }

                return ApiResponse<List<AllClasswiseDueeFeeModel>>.SuccessResponse(res, "Fetch student fee data successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<AllClasswiseDueeFeeModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<GetStudentDetailsLisModel>>> GetStudentTCReport(GetStudentIDCardReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.StudentRenewView.Where(c => (req.ClassId == -1 ? true : c.ClassId == req.ClassId)
                && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && c.RActive == false && c.StuDetail == true && c.StuFees == true
                && c.SessionId == SessionId && c.CompanyId == SchoolId).OrderBy(c => c.stu_name).Select(c => new GetStudentDetailsLisModel
                {
                    stu_id = c.StuId,
                    stu_photo = c.stu_photo,
                    ClassId = c.ClassId,
                    SectionId = c.SectionId,
                    stu_name = c.stu_name,
                    Srno = c.registration_no,
                    stu_code = c.stu_code,
                    Address = c.address,
                    DOB = c.DOB,
                    admission_date = c.admission_date,
                    FatherName = c.FatherName,
                    FatherMobileNo = c.FatherMobileNo,
                    MotherName = c.mother_name,

                    ClassName = _context.University.Where(a => a.university_id == c.ClassId && a.CompanyId == SchoolId).Select(a => a.university_name).FirstOrDefault(),
                    SectionName = _context.collegeinfo.Where(a => a.collegeid == c.SectionId && a.CompanyId == SchoolId).Select(a => a.collegename).FirstOrDefault(),

                }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<GetStudentDetailsLisModel>>.ErrorResponse("No student fee found data");
                }

                return ApiResponse<List<GetStudentDetailsLisModel>>.SuccessResponse(res, "Fetch student fee data successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetStudentDetailsLisModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<GetStudentDetailsLisModel>>> GetStudentDropoutReport(GetStudentIDCardReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.StudentRenewView.Where(c => (req.ClassId == -1 ? true : c.ClassId == req.ClassId)
                && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && c.Dropout == true && c.StuDetail == true && c.StuFees == true
                && c.SessionId == SessionId && c.CompanyId == SchoolId).OrderBy(c => c.stu_name).Select(c => new GetStudentDetailsLisModel
                {
                    stu_id = c.StuId,
                    stu_photo = c.stu_photo,
                    ClassId = c.ClassId,
                    SectionId = c.SectionId,
                    stu_name = c.stu_name,
                    Srno = c.registration_no,
                    stu_code = c.stu_code,
                    Address = c.address,
                    DOB = c.DOB,
                    admission_date = c.admission_date,
                    FatherName = c.FatherName,
                    FatherMobileNo = c.FatherMobileNo,
                    MotherName = c.mother_name,

                    ClassName = _context.University.Where(a => a.university_id == c.ClassId && a.CompanyId == SchoolId).Select(a => a.university_name).FirstOrDefault(),
                    SectionName = _context.collegeinfo.Where(a => a.collegeid == c.SectionId && a.CompanyId == SchoolId).Select(a => a.collegename).FirstOrDefault(),

                }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<GetStudentDetailsLisModel>>.ErrorResponse("No student fee found data");
                }

                return ApiResponse<List<GetStudentDetailsLisModel>>.SuccessResponse(res, "Fetch student fee data successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetStudentDetailsLisModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<TestExamMarksmOdel>>> GetTestExamMarks(GetTestExamReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.StudentRenewView.Where(c => (req.ClassId == req.ClassId)
                && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && c.RActive == true && c.StuDetail == true && c.StuFees == true && c.Dropout == false
                && c.SessionId == SessionId && c.CompanyId == SchoolId).OrderBy(c => c.stu_name).Select(c => new TestExamMarksmOdel
                {
                    stu_id = c.StuId,
                    stu_name = c.stu_name,

                    ClassName = _context.University.Where(a => a.university_id == c.ClassId && a.CompanyId == SchoolId).Select(a => a.university_name).FirstOrDefault(),
                    SectionName = _context.collegeinfo.Where(a => a.collegeid == c.SectionId && a.CompanyId == SchoolId).Select(a => a.collegename).FirstOrDefault(),

                    SubjectName = _context.Subject.Where(a => a.university_id == c.ClassId && a.SessionId == SessionId && a.CompanyId == SchoolId && a.active == true)
                    .Select(a => new GetExamSubjectModel
                    {
                        SubjectId = a.subject_id,
                        SubjectName = a.subject_name,
                    }).ToList(),

                    TestMarks = _context.TestExamTbl.Where(a => a.university_id == c.ClassId && a.stu_id == c.StuId && a.TestType == req.TestType && a.CompanyId == SchoolId
                        && a.SessionId == SessionId).Select(a => new GetTestMarksModel
                        {
                            SubjectId = a.subject_id,
                            TestMarks = a.Total,
                            MGrade = a.MGrade,
                            TestType = a.TestType,
                            MarksType = a.MarksType,
                            MaxMarks = _context.Subject.Where(p => p.university_id == a.university_id && p.subject_id == a.subject_id && p.Marks_Type == a.MarksType &&
                                     p.SessionId == SessionId && p.CompanyId == SchoolId).Select(p =>
                                     a.TestType == "Quarterly" ? p.Quarterly : a.TestType == "first_test" ? p.first_test : a.TestType == "second_test" ? p.second_test :
                                      a.TestType == "half_yearly" ? p.half_yearly : a.TestType == "third_test" ? p.third_test : a.TestType == "fourth_test" ? p.fourth_test :
                                      a.TestType == "yearly" ? p.yearly : 0)
                                     .FirstOrDefault(),
                            //     Grade = _context.GradeInfo.Where(p => p.Active == true && p.CompanyId == SchoolId).Select(p => p.grade_name).FirstOrDefault(),
                            Subjects = _context.Subject.Where(p => p.university_id == a.university_id && p.subject_id == a.subject_id && p.SessionId == SessionId
                                && p.CompanyId == SchoolId && p.active == true).Select(p => p.subject_name).FirstOrDefault(),

                        }).ToList(),

                    TotalMatks = _context.TestExamTbl.Where(a => a.university_id == c.ClassId && a.stu_id == c.StuId && a.TestType == req.TestType && a.CompanyId == SchoolId
                   && a.SessionId == SessionId).Sum(a => a.Total),

                    MaxTotal = _context.TestExamTbl.Where(a => a.university_id == c.ClassId && a.stu_id == c.StuId && a.TestType == req.TestType &&
                  a.CompanyId == SchoolId && a.SessionId == SessionId).Select(a => new
                  {
                      Marks = _context.Subject.Where(p => p.university_id == a.university_id && p.subject_id == a.subject_id && p.Marks_Type == a.MarksType &&
                      p.SessionId == SessionId && p.CompanyId == SchoolId)
                      .Select(p => a.TestType == "Quarterly" ? p.Quarterly : a.TestType == "first_test" ? p.first_test : a.TestType == "second_test" ? p.second_test :
                      a.TestType == "half_yearly" ? p.half_yearly : a.TestType == "third_test" ? p.third_test : a.TestType == "fourth_test" ? p.fourth_test :
                  a.TestType == "yearly" ? p.yearly : 0).FirstOrDefault()
                  }).Sum(x => x.Marks),


                }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<TestExamMarksmOdel>>.ErrorResponse("No student fee found data");
                }

                return ApiResponse<List<TestExamMarksmOdel>>.SuccessResponse(res, "Fetch student fee data successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TestExamMarksmOdel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<TestExamMarksmOdel>>> GetTotalTestExamReport(GetTestExamReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var testOrder = new List<string>
                {
                  "Quarterly",  "first_test",   "half_yearly", "second_test",  "third_test", "fourth_test",  "yearly"
                };


                // find index of selected test type
                int maxIndex = testOrder.IndexOf(req.TestType);

                // select all test types up to that index  
                var allowedTestTypes = testOrder.Take(maxIndex + 1).ToList();

                var res = await _context.StudentRenewView.Where(c => (req.ClassId == req.ClassId)
                && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && c.RActive == true && c.StuDetail == true && c.StuFees == true && c.Dropout == false
                && c.SessionId == SessionId && c.CompanyId == SchoolId).OrderBy(c => c.stu_name).Select(c => new TestExamMarksmOdel
                {
                    stu_id = c.StuId,
                    stu_name = c.stu_name,

                    ClassName = _context.University.Where(a => a.university_id == c.ClassId && a.CompanyId == SchoolId).Select(a => a.university_name).FirstOrDefault(),
                    SectionName = _context.collegeinfo.Where(a => a.collegeid == c.SectionId && a.CompanyId == SchoolId).Select(a => a.collegename).FirstOrDefault(),

                    SubjectName = _context.Subject.Where(a => a.university_id == c.ClassId && a.SessionId == SessionId && a.CompanyId == SchoolId && a.active == true)
                    .Select(a => new GetExamSubjectModel
                    {
                        SubjectId = a.subject_id,
                        SubjectName = a.subject_name,
                    }).ToList(),

                    TestMarks = _context.TestExamTbl.Where(a => a.university_id == c.ClassId && a.stu_id == c.StuId && allowedTestTypes.Contains(a.TestType) && a.CompanyId == SchoolId
                    && a.SessionId == SessionId).Select(a => new GetTestMarksModel
                    {
                        SubjectId = a.subject_id,
                        TestMarks = a.Total,
                        MGrade = a.MGrade,
                        TestType = a.TestType,
                        MarksType = a.MarksType,

                        MaxMarks = _context.Subject.Where(p => p.university_id == a.university_id && p.subject_id == a.subject_id && p.Marks_Type == a.MarksType &&
                                        p.SessionId == SessionId && p.CompanyId == SchoolId).Select(p =>
                                        a.TestType == "Quarterly" ? p.Quarterly : a.TestType == "first_test" ? p.first_test : a.TestType == "second_test" ? p.second_test :
                                         a.TestType == "half_yearly" ? p.half_yearly : a.TestType == "third_test" ? p.third_test : a.TestType == "fourth_test" ? p.fourth_test :
                                         a.TestType == "yearly" ? p.yearly : 0)
                                        .FirstOrDefault(),

                        // Grade = _context.GradeInfo.Where(p => p.Active == true && p.CompanyId == SchoolId).Select(p => p.grade_name).FirstOrDefault(),

                        Subjects = _context.Subject.Where(p => p.university_id == a.university_id && p.subject_id == a.subject_id && p.SessionId == SessionId &&
                                p.CompanyId == SchoolId && p.active == true).Select(p => p.subject_name).FirstOrDefault(),

                    }).ToList(),

                    TotalMatks = _context.TestExamTbl.Where(a => a.university_id == c.ClassId && a.stu_id == c.StuId && allowedTestTypes.Contains(a.TestType) && a.CompanyId == SchoolId
                    && a.SessionId == SessionId).Sum(a => a.Total),


                    // MaxMarks पहले ले आओ, फिर बाद में Sum करो
                    MaxTotal = _context.TestExamTbl.Where(a => a.university_id == c.ClassId && a.stu_id == c.StuId && allowedTestTypes.Contains(a.TestType) &&
                    a.CompanyId == SchoolId && a.SessionId == SessionId).Select(a => new
                    {
                        Marks = _context.Subject.Where(p => p.university_id == a.university_id && p.subject_id == a.subject_id && p.Marks_Type == a.MarksType &&
                        p.SessionId == SessionId && p.CompanyId == SchoolId)
                        .Select(p => a.TestType == "Quarterly" ? p.Quarterly : a.TestType == "first_test" ? p.first_test : a.TestType == "second_test" ? p.second_test :
                        a.TestType == "half_yearly" ? p.half_yearly : a.TestType == "third_test" ? p.third_test : a.TestType == "fourth_test" ? p.fourth_test :
                    a.TestType == "yearly" ? p.yearly : 0).FirstOrDefault()
                    }).Sum(x => x.Marks),


                }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<TestExamMarksmOdel>>.ErrorResponse("No student fee found data");
                }

                return ApiResponse<List<TestExamMarksmOdel>>.SuccessResponse(res, "Fetch student fee data successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TestExamMarksmOdel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<StudentMarksheetModel>>> GetStudentMarksheet(GetTestExamReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var testOrder = new List<string>
                {
                  "Quarterly",  "first_test",   "half_yearly", "second_test",  "third_test", "fourth_test",  "yearly"
                };


                // find index of selected test type
                int maxIndex = testOrder.IndexOf(req.TestType);

                // select all test types up to that index  
                var allowedTestTypes = testOrder.Take(maxIndex + 1).ToList();

                var res = await _context.StudentRenewView.Where(c => (req.ClassId == req.ClassId)
                && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && c.RActive == true && c.StuDetail == true && c.StuFees == true && c.Dropout == false
                && c.SessionId == SessionId && c.CompanyId == SchoolId).OrderBy(c => c.stu_name).Select(c => new StudentMarksheetModel
                {
                    stu_id = c.StuId,
                    stu_name = c.stu_name,
                    RollNo = c.RollNo,
                    Srno = c.registration_no,
                    DOB = c.DOB,
                    FatherName = c.father_name,
                    FatherMobileNo = c.father_mobile,
                    MotherName = c.mother_name,

                    ClassName = _context.University.Where(a => a.university_id == c.ClassId && a.CompanyId == SchoolId).Select(a => a.university_name).FirstOrDefault(),
                    SectionName = _context.collegeinfo.Where(a => a.collegeid == c.SectionId && a.CompanyId == SchoolId).Select(a => a.collegename).FirstOrDefault(),

                    TotalMatks = _context.TestExamTbl.Where(a => a.university_id == c.ClassId && a.stu_id == c.StuId && allowedTestTypes.Contains(a.TestType) && a.CompanyId == SchoolId
                    && a.SessionId == SessionId).Sum(a => a.Total),


                    // MaxMarks पहले ले आओ, फिर बाद में Sum करो
                    MaxTotal = _context.TestExamTbl.Where(a => a.university_id == c.ClassId && a.stu_id == c.StuId && allowedTestTypes.Contains(a.TestType) &&
                    a.CompanyId == SchoolId && a.SessionId == SessionId).Select(a => new
                    {
                        Marks = _context.Subject.Where(p => p.university_id == a.university_id && p.subject_id == a.subject_id && p.Marks_Type == a.MarksType &&
                        p.SessionId == SessionId && p.CompanyId == SchoolId)
                        .Select(p => a.TestType == "Quarterly" ? p.Quarterly : a.TestType == "first_test" ? p.first_test : a.TestType == "second_test" ? p.second_test :
                        a.TestType == "half_yearly" ? p.half_yearly : a.TestType == "third_test" ? p.third_test : a.TestType == "fourth_test" ? p.fourth_test :
                    a.TestType == "yearly" ? p.yearly : 0).FirstOrDefault()
                    }).Sum(x => x.Marks),


                }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<StudentMarksheetModel>>.ErrorResponse("No student fee found data");
                }

                return ApiResponse<List<StudentMarksheetModel>>.SuccessResponse(res, "Fetch student fee data successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<StudentMarksheetModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }


        public async Task<PagedResult<GetStudentFeeListModel>> GetStudentFeeDetail(GetStudentFeeListReqModel req)
        {
            int SchoolId = _loginUser.SchoolId;
            int SessionId = _loginUser.SessionId;

            var query = _context.StudentRenewView.Where(p => p.SessionId == SessionId && p.CompanyId == SchoolId
            && p.RActive == true && p.StuDetail == true && p.StuFees == true);

            if (req.ClassId.HasValue)
                query = query.Where(p => p.ClassId == req.ClassId);



            if (!string.IsNullOrEmpty(req.Srno))
                query = query.Where(p => p.registration_no == req.Srno);

            int totalrecords = await query.CountAsync();

            int PageNumber = req.PageNumber > 0 ? req.PageNumber : 1;
            int PageSize = req.PageSize > 0 ? req.PageSize : 10;

            var data = await query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ProjectTo<GetStudentFeeListModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            int totalpages = (int)Math.Ceiling((double)totalrecords / PageSize);

            var pagedResult = new PagedResult<GetStudentFeeListModel>
            {
                Data = data,
                TotalRecords = totalrecords,
                PageNumber = PageNumber,
                PageSize = PageSize,
                TotalPages = totalpages
            };

            return pagedResult;
        }

        //public async Task<ApiResponse<List<GetStudentFeeListModel>>> GetStudentFeeListDetail(GetStudentReq req)
        //{
        //    try
        //    {
        //        int SchoolId = _loginUser.SchoolId;
        //        int UserId = _loginUser.UserId;
        //        int SessionId = _loginUser.SessionId;

        //        var res = await _context.StudentRenewView.Where(c => (req.ClassId == -1 ? true : c.ClassId == req.ClassId)
        //        && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && (req.Fromdate == null || c.RDate >= req.Fromdate)
        //          && (req.Todate == null || c.RDate <= req.Todate)
        //        && (req.StudentId == -1 ? true : c.StuId == req.StudentId) && (string.IsNullOrEmpty(req.srno) || c.registration_no == req.srno)
        //        && c.RActive == true && c.StuDetail == true && c.StuFees == true && c.SessionId == SessionId && c.CompanyId == SchoolId)
        //            .OrderBy(c => c.stu_name).Select(c => new GetStudentFeeListModel
        //            {
        //                StudentId = c.StuId,
        //                ClassId = c.ClassId,
        //                SectionId = c.SectionId,
        //                stu_name = c.stu_name,
        //                SRNo = c.registration_no,
        //                DOB = c.DOB,
        //                RollNo = c.RollNo,
        //                RTE = c.RTE,
        //                ParentsId = c.ParentsId,
        //                admission_date = c.admission_date,
        //                PramoteFees = c.PramoteFees,
        //                AdmissionPayfee = c.AdmissionPayfee,
        //                AFeeDiscount = c.AFeeDiscount,
        //                total = c.total,
        //                discount = c.discount,
        //                OldDuefees = c.OldDuefees,
        //                total_fee = c.total_fee,

        //                ClassName = _context.University.Where(a => a.university_id == c.ClassId && a.CompanyId == SchoolId).Select(a => a.university_name).FirstOrDefault(),
        //                SectionName = _context.collegeinfo.Where(a => a.collegeid == c.SectionId && a.CompanyId == SchoolId).Select(a => a.collegename).FirstOrDefault(),

        //                Studentreceipt = _context.M_FeeDetail.Where(a => a.stu_id == c.StuId && a.ClassId == c.ClassId && a.CompanyId == SchoolId && a.SessionId == SessionId)
        //                .Select(a => new StudentReceiptModel
        //                {

        //                    ReceiptNo = a.ReceiptNo,
        //                    PayFees = a.PayFees,
        //                    PaymentDate = a.PaymentDate,
        //                    PaymentMode = a.PaymentMode,
        //                    Remark = a.Remark,
        //                    FeeType = a.Status,

        //                }).ToList(),

        //            }).ToListAsync();

        //        if (res == null || !res.Any())
        //        {
        //            return ApiResponse<List<GetStudentFeeListModel>>.ErrorResponse("No student fee found data");
        //        }

        //        return ApiResponse<List<GetStudentFeeListModel>>.SuccessResponse(res, "Fetch student fee data successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiResponse<List<GetStudentFeeListModel>>.ErrorResponse("Something went wrong: " + ex.Message);
        //    }
        //}

        //public async Task<ApiResponse<List<DailyCollectionModel>>> DailyCollectionList(DailyCollectionReportReq req)
        //{
        //    try
        //    {
        //        int SchoolId = _loginUser.SchoolId;
        //        int UserId = _loginUser.UserId;
        //        int SessionId = _loginUser.SessionId;

        //        var res = new
        //        {
        //            Student 
        //        }


        //        //var res = await _context.StudentRenewView.Where(c => (req.Classid == -1 ? true : c.ClassId == req.Classid)
        //        //&& (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && c.SchoolId == SchoolId
        //        //&& c.SessionId == SessionId && c.StuDetail == true && c.StuFee == true && c.Active == true)
        //        //   .OrderBy(c => c.stu_name).Select(c => new ClasswiseStudentListModel
        //        //   {
        //        //       StudentId = c.StudentId,
        //        //       stu_name = c.stu_name,
        //        //       SRNo = c.SRNo,
        //        //       RollNo = c.RollNo,
        //        //       RTE = c.RTE,
        //        //       FatherName = c.fathername,
        //        //       FatherMobileNo = c.fathermobileno,
        //        //       MotherName = c.mothername,
        //        //       total_fee = c.total_fee,

        //        //       ClassName = _context.ClassTbl.Where(a => a.ClassId == c.ClassId && a.SchoolId == SchoolId).Select(a => a.ClassName).FirstOrDefault(),
        //        //       SectionName = _context.sectionTbl.Where(a => a.SectionId == c.SectionId && a.SchoolId == c.SchoolId).Select(a => a.SectionName).FirstOrDefault(),

        //        //       PaidFee = _context.FeeReceiptTbl.Where(a => a.StudentId == c.StudentId && a.ClassId == c.ClassId && a.SessionId == SessionId && a.SchoolId == SchoolId && a.Active == true && a.FeeType == "InsPayFee").Sum(a => a.PayFees) ?? 0,

        //        //       ClassInstallments = _context.StuFeeInstallmentTbl.Where(a => a.ClassId == c.ClassId && a.StudentId == c.StudentId && a.SchoolId == SchoolId && a.SessionId == SessionId)
        //        //      .Select(a => new ClasswiseInstallmentModel
        //        //      {
        //        //          Installment = a.Installment,
        //        //          SInsAmount = a.SInsAmount,
        //        //      }).ToList(),


        //        //   }).ToListAsync();

        //        if (res == null || !res.Any())
        //        {
        //            return ApiResponse<List<ClasswiseStudentListModel>>.ErrorResponse("Classwise installment not found ");
        //        }
        //        return ApiResponse<List<ClasswiseStudentListModel>>.SuccessResponse(res, "Fetch classwise installment data");
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiResponse<List<ClasswiseStudentListModel>>.ErrorResponse("Something went wrong: " + ex.Message);
        //    }
        //}


    }
}
