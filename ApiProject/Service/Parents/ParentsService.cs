using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using ApiProject.Service.Current;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure;
using Azure.Core;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace ApiProject.Service.Parents
{
    public class ParentsService : IParentsService
    {

        private readonly ILoginUserService _loginUser;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBackgroundJobClient _backgroundJobs;


        public ParentsService(ILoginUserService loginUser, ApplicationDbContext context, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IBackgroundJobClient backgroundJob)
        {
            _loginUser = loginUser;
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _backgroundJobs = backgroundJob;
        }

        public async Task<ApiResponse<List<GetStudentModel>>> GetStudentList()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;
                int ParentId = _loginUser.ParentId;
                int StudentId = _loginUser.StudentId;

                var Stufee = await _context.StudentRenewView.Where(a => a.ParentsId == ParentId && a.CompanyId == SchoolId && a.SessionId == SessionId && a.RActive == true)
                    .Select(a => new GetStudentModel
                    {
                        StudentId = a.StuId,
                        stuphoto = a.stu_photo,
                        Studentname = a.stu_name,

                    }).ToListAsync();

                return ApiResponse<List<GetStudentModel>>.SuccessResponse(Stufee, "Student List fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetStudentModel>>.ErrorResponse("Error :" + ex.Message);
            }
        }

        public async Task<ApiResponse<Getparentsreq>> GetStudentToken(int StudentId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;
                int ParentId = _loginUser.ParentId;

                // Step 1: Dummy ParentId find (example)
                var parent = await _context.StudentRenewView.FirstOrDefaultAsync(x => x.StuId == StudentId && x.SessionId == SessionId && x.CompanyId == SchoolId);

                if (parent == null)
                {
                    return ApiResponse<Getparentsreq>.ErrorResponse("Parent not found");
                }

                var token = GenerateJwtToken(parent);
                // Step 2: Prepare payload data
                var payload = new Getparentsreq
                {
                    Token = token,
                    ParentId = parent.ParentsId,
                    StudentId = StudentId
                };

                // Step 4: Return response
                return ApiResponse<Getparentsreq>.SuccessResponse(payload, "Token generated successfully: ");
            }
            catch (Exception ex)
            {
                return ApiResponse<Getparentsreq>.ErrorResponse(ex.Message);
            }
        }
        private string GenerateJwtToken(StudentRenewView user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.ParentsId.ToString()),
                new Claim("ParentId", user.ParentsId.ToString()),
                new Claim("SchoolId", user.CompanyId.ToString()),
                new Claim("SessionId", user.SessionId.ToString()),
                 new Claim("StudentId", user.StuId.ToString()),
                  new Claim("Studentname", user.stu_name.ToString()),
                  new Claim("ClassId", user.ClassId.ToString()),
                // new Claim("ClassName", user.ClassId.ToString()),
                new Claim("ParentName", user.father_name.ToString())
                //new Claim(ClaimTypes.Role, user.Status)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ApiResponse<GetParentDetailsModel>> GetStudentParentsDetail()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;
                int ParentId = _loginUser.ParentId;
                int StudentId = _loginUser.StudentId;

                var parentEntity = await _context.StudentRenewView.Where(a => a.ParentsId == ParentId && a.StuId == StudentId && a.SessionId == SessionId)
                    .Select(a => new GetParentDetailsModel
                    {
                        StudentId = a.StuId,
                        stuphoto = a.stu_photo,
                        Studentname = a.stu_name,
                        SRNo = a.registration_no,
                        DOB = a.DOB,
                        gender = a.gender,
                        cast_category = a.cast_category,
                        Caste = a.cast_category,
                        Religion = a.Religion,
                        FatherName = a.father_name,
                        FatherMobileNo = a.father_mobile,
                        FatherOccupation = a.father_occupation,
                        FatherIncome = a.Fatherlncome,
                        MotherName = a.mother_name,
                        MotherMobileNo = a.mother_mobile,
                        MotherOccupation = a.mother_occupation,
                        MotherIncome = a.MotherIncome,
                        GuardianName = a.GuardianName,
                        GuardianMobileNo = a.GuardianMobileNo,
                        address = a.address,
                        state = a.state,
                        district = a.district,
                        city = a.city,
                        pincode = a.pincode,
                        RollNumber = a.RollNo,
                        ClassId = a.ClassId,
                        SectionId = a.SectionId,
                        ClassName = _context.University.Where(p => p.university_id == a.ClassId && p.CompanyId == SchoolId).Select(p => p.university_name).FirstOrDefault(),
                        SectionName = _context.collegeinfo.Where(s => s.collegeid == a.SectionId && s.CompanyId == SchoolId).Select(s => s.collegename).FirstOrDefault()

                    }).FirstOrDefaultAsync();

                return ApiResponse<GetParentDetailsModel>.SuccessResponse(parentEntity, "Student & Parents details fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<GetParentDetailsModel>.ErrorResponse("Error :" + ex.Message);
            }
        }

        // student attendance Section start
        public async Task<ApiResponse<List<GetAttendanceModel>>> GetAttendanceByMonth(int studentid, int month)
        {
            try
            {
                int schoolId = _loginUser.SchoolId;
                int sessionId = _loginUser.SessionId;

                // All students of class/section
                var students = await _context.StudentRenewView.Where(s => s.StuId == studentid && s.SessionId == sessionId && s.CompanyId == schoolId
                && s.RActive == true && s.StuDetail == true && s.Dropout == false && s.StuFees == true).OrderBy(s => s.stu_name).ToListAsync();

                // Get attendance for the full month
                var year = DateTime.Now.Year;
                var monthName = new DateTime(DateTime.Now.Year, month, 1).ToString("MMMM");
                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var attendance = await _context.Student_Attendance.Where(a => a.StudentId == studentid && a.SessionId == sessionId && a.CompanyId == schoolId
                      && a.Date >= startDate && a.Date <= endDate).ToListAsync();

                var response = students.Select(student =>
                {
                    var dailyAttendance = Enumerable.Range(1, endDate.Day).ToDictionary(
                        day => day,
                        day => attendance.FirstOrDefault(a => a.StudentId == student.StuId && a.Date?.Day == day)?.Status ?? ""
                    );

                    var model = new GetAttendanceModel
                    {
                        monthname = monthName,

                        AttendanceByDate = dailyAttendance,
                        TotalP = dailyAttendance.Values.Count(x => x == "Present"),
                        TotalA = dailyAttendance.Values.Count(x => x == "Absent"),
                        TotalH = dailyAttendance.Values.Count(x => x == "Holiday"),
                        TotalHF = dailyAttendance.Values.Count(x => x == "HalfDay"),
                        TotalL = dailyAttendance.Values.Count(x => x == "Late"),
                    };

                    return model;
                }).ToList();

                return ApiResponse<List<GetAttendanceModel>>.SuccessResponse(response, "Student attendance by month name fetched  successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetAttendanceModel>>.ErrorResponse("Error: " + ex.Message);
            }
        }

        // student test exam detail  start

        public async Task<ApiResponse<List<GetTestTypeModel>>> GetTestType(int studentid)
        {
            try
            {
                int schoolId = _loginUser.SchoolId;
                int sessionId = _loginUser.SessionId;

                var res = await _context.StudentRenewView.Where(p => p.StuId == studentid && p.RActive == true && p.SessionId == sessionId && p.CompanyId == schoolId)
                    .Select(c => new GetTestTypeModel
                    {
                        studentid = c.StuId,
                        testtype = _context.ExamView.Where(a => a.stu_id == studentid && a.university_id == c.ClassId && a.CompanyId == schoolId && a.SessionId == sessionId)
                        .Select(p => new typelist { testexamtype = p.TestType }).Distinct().ToList()
                    }).ToListAsync();

                if (res == null)
                {
                    return ApiResponse<List<GetTestTypeModel>>.ErrorResponse("No data found");
                }

                return ApiResponse<List<GetTestTypeModel>>.SuccessResponse(res, "Test types fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetTestTypeModel>>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<GetTestwiseExamModel>>> GetTestwiseExamMarks(int studentid, string testtype)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.ExamView.Where(c => c.stu_id == studentid && c.TestType == testtype && c.SessionId == SessionId && c.CompanyId == SchoolId)
                    .GroupBy(c => new { c.stu_id, c.TestType, c.university_id }).Select(g => new GetTestwiseExamModel
                    {
                        SubjectName = _context.Subject.Where(a => a.university_id == g.Key.university_id && a.SessionId == SessionId && a.CompanyId == SchoolId && a.active == true)
                        .Select(a => new GetExamSubjectModel
                        {
                            SubjectId = a.subject_id,
                            SubjectName = a.subject_name
                        }).ToList(),

                        TestMarks = _context.TestExamTbl.Where(a => a.university_id == g.Key.university_id && a.stu_id == g.Key.stu_id && a.TestType == g.Key.TestType
                        && a.CompanyId == SchoolId && a.SessionId == SessionId).Select(a => new GetTestMarksModel
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

                        TotalMatks = _context.TestExamTbl.Where(a => a.university_id == g.Key.university_id && a.stu_id == g.Key.stu_id && a.TestType == g.Key.TestType
                        && a.CompanyId == SchoolId && a.SessionId == SessionId).Sum(a => a.Total),

                        MaxTotal = _context.TestExamTbl.Where(a => a.university_id == g.Key.university_id && a.stu_id == g.Key.stu_id && a.TestType == g.Key.TestType &&
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
                    return ApiResponse<List<GetTestwiseExamModel>>.ErrorResponse("No student fee found data");
                }

                return ApiResponse<List<GetTestwiseExamModel>>.SuccessResponse(res, "Fetch student fee data successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetTestwiseExamModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }



        // ==================================      Student payment getway code details  **************************************** //

        public async Task<ApiResponse<getStudentInstallmentModel>> GetStudentInstallmentFee()
        {
            try
            {

                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;
                int ParentId = _loginUser.ParentId;
                int StudentId = _loginUser.StudentId;

                var InstallFee = await _context.StudentRenewView.Where(a => a.StuId == StudentId && a.CompanyId == SchoolId && a.SessionId == SessionId)
                    .Select(a => new getStudentInstallmentModel
                    {
                        StudentId = a.StuId,
                        StudentName = a.stu_name,
                        Srno = a.registration_no,
                        ClassId = a.ClassId,
                        ClassName = _context.University.Where(p => p.university_id == a.ClassId && p.CompanyId == SchoolId).Select(p => p.university_name).FirstOrDefault(),
                        sectionId = a.SectionId,
                        sectionName = _context.collegeinfo.Where(s => s.collegeid == a.SectionId && s.CompanyId == SchoolId).Select(s => s.collegename).FirstOrDefault(),
                        AdmissionPayfee = a.AdmissionPayfee,
                        TotalFee = a.total_fee,
                        PaidFee = a.Rstu_fee,
                        Duefee = a.Rdue_fee,
                        DueInstallment = _context.fee_installment.Where(p => p.stu_id == a.StuId && p.university_id == a.ClassId && p.SessionId == SessionId && p.CompanyId == SchoolId)
                        .Select(p => new GetInstallmentModel
                        {
                            Installmentno = p.Installment,
                            Dueinstallment = p.due_fee,
                        }).ToList(),

                        FeeReceipt = _context.M_FeeDetail.Where(p => p.stu_id == a.StuId && p.ClassId == a.ClassId && p.SessionId == SessionId && p.CompanyId == SchoolId && p.Active == true)
                        .Select(p => new GetFeereceiptModel
                        {
                            FDId = p.FDId,
                            StudentId = p.stu_id,
                            StudentName = _context.student_admission.Where(p => p.stu_id == a.StuId && p.CompanyId == SchoolId).Select(p => p.stu_name).FirstOrDefault(),
                            ReceiptNo = p.ReceiptNo,
                            PayFee = p.PayFees,
                            FeeType = p.Status,
                            Date = p.Date,
                            PaymentMode = p.PaymentMode,
                            Remark = p.Remark,
                        }).ToList(),

                    }).FirstOrDefaultAsync();
                return ApiResponse<getStudentInstallmentModel>.SuccessResponse(InstallFee, "Student installment data fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<getStudentInstallmentModel>.ErrorResponse("Error :" + ex.Message);
            }
        }


        private string _phonePeToken;
        private DateTime _tokenExpiry;
        private object _backgroundJob;

        public async Task<string> GetNewAccessToken()
        {

            string authUrl = "https://api.phonepe.com/apis/identity-manager/v1/oauth/token";               // Production for realy 

            //  string authUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox/v1/oauth/token";                // Sandbox for testing demo  1

            //   string authUrl = "https://api-preprod.phonepe.com/apis/identity-manager/v1/oauth/token";                // Sandbox for testing demo  not use 

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                var content = new FormUrlEncodedContent(new[]
                {
                    //new KeyValuePair<string, string>("client_id", "SHYAWAYUAT_2510101108212"),
                    //new KeyValuePair<string, string>("client_version", "1"),
                    //new KeyValuePair<string, string>("client_secret", "NWQ2YzJlZDktODU3Yi00ZWUzLTk1MTItOTJhZDVkYjkxYmYx"),
                    //new KeyValuePair<string, string>("grant_type", "client_credentials")

                     new KeyValuePair<string, string>("client_id", "SU2602051921478872343710"),
                    new KeyValuePair<string, string>("client_version", "1"),
                    new KeyValuePair<string, string>("client_secret", "541e3c3f-0fc7-4e42-a141-28922b4eba32"),
                    new KeyValuePair<string, string>("grant_type", "client_credentials")

                });

                var response = await client.PostAsync(authUrl, content);
                var result = await response.Content.ReadAsStringAsync();

                Console.WriteLine("TOKEN RESPONSE: " + result);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Token Error: " + result);

                dynamic json = JsonConvert.DeserializeObject(result);

                _phonePeToken = json.access_token;
                _tokenExpiry = DateTime.Now.AddSeconds((int)json.expires_in - 60);

                return _phonePeToken;
            }
        }

        public async Task<string> GetValidToken()
        {
            if (string.IsNullOrEmpty(_phonePeToken) || DateTime.Now >= _tokenExpiry)
            {
                return await GetNewAccessToken();
            }
            return _phonePeToken;
        }

        public async Task<ApiResponse<StudentFeePaymentResult>> AddStudentInstallmentFee(AddStudentinstallReq req)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    int SchoolId = _loginUser.SchoolId;
                    int SessionId = _loginUser.SessionId;

                    // ===========================================  CHECK — Student has no pending due already
                    var student = await _context.Student_Renew.FirstOrDefaultAsync(a => a.ClassId == req.ClassId && a.StuId == req.StudentId && a.due_fee != 0);

                    if (student == null)
                    {
                        return ApiResponse<StudentFeePaymentResult>.ErrorResponse("Due fee already available");
                    }

                    // =========================================== INSTITUTE CODE & RECEIPT NUMBER
                    var GetInstituteCodeName = await _context.institute.FirstOrDefaultAsync(i => i.institute_id == SchoolId);

                    var LastCode = await _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).OrderByDescending(s => s.FDId).FirstOrDefaultAsync();

                    string instCode = GetInstituteCodeName.instituteCode.Substring(0, 3).ToUpper();
                    int newId = (LastCode != null)
                                ? int.Parse(LastCode.ReceiptNo.Split('/')[1]) + 1
                                : 1;

                    string ReceiptCode = $"{instCode}/{newId}";


                    // =========================================== GENERATE ORDER NUMBER
                    int NewOrderNo = 1;
                    var LastOrderNo = await _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).OrderByDescending(s => s.OrderNo)
                        .Select(s => s.OrderNo).FirstOrDefaultAsync();

                    if (!string.IsNullOrEmpty(LastOrderNo))
                    {
                        if (int.TryParse(LastOrderNo, out int last))
                            NewOrderNo = last + 1;
                    }

                    // =========================================== GENERATE PRIMARY KEY FDId
                    int FDId = _context.M_FeeDetail.DefaultIfEmpty().Max(s => s == null ? 0 : s.FDId) + 1;

                    // =========================================== SAVE INSTALLMENT RECORD
                    var fee = new M_FeeDetail
                    {
                        FDId = FDId,
                        stu_id = req.StudentId,
                        ClassId = req.ClassId,
                        OrderNo = NewOrderNo.ToString(),
                        OrderStatus = "Pending",
                        TransactionId = "",
                        ReceiptType = "Online",
                        DueFees = student.due_fee - req.PaidFee,
                        PayFees = req.PaidFee,
                        Cash = 0,
                        Upi = 0,
                        AdmissionPayfee = 0,
                        AFeeDiscount = 0,
                        PramoteFees = 0,
                        Date = DateTime.Now,
                        Status = "InstallmentFee",
                        Active = false,
                        CompanyId = SchoolId,
                        SessionId = SessionId,
                        NetDueFees = student.due_fee - req.PaidFee,
                        PaymentMode = "UPI",
                        PaymentDate = DateTime.UtcNow,
                        Remark = req.Remark,
                        RTS = DateTime.Now,
                        ReceiptNo = ReceiptCode
                    };

                    _context.M_FeeDetail.Add(fee);
                    await _context.SaveChangesAsync();


                    string token = await GetValidToken();
                    if (string.IsNullOrEmpty(token))
                    {
                        await transaction.RollbackAsync();
                        return ApiResponse<StudentFeePaymentResult>.ErrorResponse("Failed to fetch authorization token");
                    }

                    // =========================================== STEP 2 → PAYMENT REQUEST

                    string apiUrl = "https://api.phonepe.com/apis/pg/checkout/v2/sdk/order";            // Production is orginal url

                    //   string apiUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/sdk/order";        //  Sandbox is a testing demo url

                    double amount = Convert.ToDouble(req.PaidFee);

                    var paymentRequest = new
                    {
                        merchantOrderId = NewOrderNo.ToString(),
                        amount = (int)(amount * 100),
                        expireAfter = 300,

                        metaInfo = new
                        {
                            udf1 = "StudentFee",
                            udf2 = req.StudentId.ToString(),
                            udf3 = req.ClassId.ToString(),
                            udf4 = ReceiptCode,
                            udf5 = "InstituteFee"
                        },

                        paymentFlow = new
                        {
                            type = "PG_CHECKOUT",
                            message = "Fee payment",
                            //merchantUrls = new
                            //{
                            //    redirectUrl = $"https://thistestapi.vedusoft.in/api/payment/UpdateStudentPaymentSuccessfully?StudentId={req.StudentId}&ReceiptId={FDId}"
                            //}

                        }
                    };

                    string jsonBody = JsonConvert.SerializeObject(paymentRequest);
                    byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonBody);

                    HttpWebRequest payRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
                    payRequest.Method = "POST";
                    payRequest.ContentType = "application/json";
                    //  payRequest.Headers.Add("Authorization", "Bearer " + token);
                    payRequest.Headers.Add("Authorization", "O-Bearer " + token);

                    using (var stream = payRequest.GetRequestStream())
                        stream.Write(jsonBytes, 0, jsonBytes.Length);

                    string payResponseString;
                    var payResponse = (HttpWebResponse)payRequest.GetResponse();
                    using (var reader = new StreamReader(payResponse.GetResponseStream()))
                        payResponseString = reader.ReadToEnd();

                    dynamic paymentResponse = JsonConvert.DeserializeObject(payResponseString);
                    string redirectUrl = paymentResponse.redirectUrl;

                    // =================         PENDING CHECK LOOP

                    _backgroundJobs.Schedule(
                        () => CheckPaymentStatusBackground1(NewOrderNo.ToString()),
                        TimeSpan.FromSeconds(10)
                        );

                    await transaction.CommitAsync();
                    return ApiResponse<StudentFeePaymentResult>.SuccessResponse(
                        new StudentFeePaymentResult
                        {
                            FDId = fee.FDId,
                            ReceiptNo = ReceiptCode,
                            merchantOrderId = fee.OrderNo,
                            OrderId = paymentResponse.orderId,
                            State = paymentResponse.state,
                            ExpireAt = paymentResponse.expireAt,
                            Token = paymentResponse.token
                        },
                        "Installment fee saved successfully"
                        );
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return ApiResponse<StudentFeePaymentResult>.ErrorResponse("Error: " + ex.Message);
                }
            }
        }

        public async Task CheckPaymentStatus(string orderId)
        {
            try
            {

                string token = await GetValidToken();

                string url = $"https://api.phonepe.com/apis/pg/checkout/v2/order/{orderId}/status";                // Production is orginal url

                //   string url = $"https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/order/{orderId}/status";         //  Sandbox is a testing demo url

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    var response = await client.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("STATUS RESPONSE: " + result);

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("ERROR: " + result);
                        return;
                    }

                    dynamic res = JsonConvert.DeserializeObject(result);

                    string state = res?.state;
                    string txnId = res?.paymentDetails?[0]?.transactionId;

                    if (state == "COMPLETED" || state == "FAILED")
                    {
                        var order = _context.M_FeeDetail.FirstOrDefault(x => x.OrderNo == orderId);

                        if (order != null)
                        {
                            order.OrderStatus = state;
                            order.TransactionId = txnId;

                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Status Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateStudentPaymentSuccessfully(int StudentId, int ReceiptId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var fee = await _context.M_FeeDetail.FirstOrDefaultAsync(a => a.FDId == ReceiptId && a.SessionId == SessionId && a.CompanyId == SchoolId);

                if (fee == null)
                    return ApiResponse<bool>.ErrorResponse("Invalid request!");

                // ✅ TOKEN
                string token = await GetValidToken();
                if (string.IsNullOrEmpty(token))
                    return ApiResponse<bool>.ErrorResponse("Token error");

                // ✅ STATUS CHECK
                string merchantOrderId = fee.OrderNo;

                string url = $"https://api.phonepe.com/apis/pg/checkout/v2/order/{merchantOrderId}/status";         // Production WALA URL ORGINAL 

                // string url = $"https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/order/{merchantOrderId}/status";          //  Sandbox demo url for testing 

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", "O-Bearer " + token);

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return ApiResponse<bool>.ErrorResponse("Status API failed");

                string result = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(result);

                string status = json.state;
                var payment = json.paymentDetails?[0];

                string txnId = payment?.transactionId;

                // ================= SUCCESS
                if (status == "COMPLETED")
                {
                    fee.OrderStatus = "Success";
                    fee.Active = true;
                    fee.TransactionId = txnId ?? "";

                    await _context.SaveChangesAsync();

                    var studentrenewtbl = _context.Student_Renew.FirstOrDefault(s => s.StuId == fee.stu_id && s.ClassId == fee.ClassId);

                    if (studentrenewtbl != null)
                    {
                        studentrenewtbl.due_fee = studentrenewtbl.due_fee - fee.PayFees;
                        studentrenewtbl.stu_fee += fee.PayFees;
                        await _context.SaveChangesAsync();
                    }

                    var installments = _context.fee_installment.Where(u => u.stu_id == fee.stu_id && u.university_id == fee.ClassId).OrderBy(a => a.IntallmentID).ToList();

                    double remainingAmount = Convert.ToDouble(fee.PayFees);

                    foreach (var insta in installments)
                    {
                        if (remainingAmount <= 0) break;

                        if (insta.due_fee > 0)
                        {
                            if (remainingAmount >= insta.due_fee)
                            {
                                remainingAmount -= Convert.ToDouble(insta.due_fee);
                                insta.due_fee = 0;
                            }
                            else
                            {
                                insta.due_fee -= remainingAmount;
                                remainingAmount = 0;
                            }
                            await _context.SaveChangesAsync();
                        }
                    }

                    return ApiResponse<bool>.SuccessResponse(true, "Payment successful");
                }
                // ================= FAILED
                else if (status == "FAILED")
                {
                    fee.OrderStatus = "Failed";
                    fee.Active = false;

                    await _context.SaveChangesAsync();
                    return ApiResponse<bool>.SuccessResponse(true, "Payment Failed");

                }
                // ================= PENDING
                else
                {
                    return ApiResponse<bool>.SuccessResponse(true, "Payment Pending");
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error : " + ex.Message);
            }
        }


        public async Task<object> WebhookStatus()
        {
            try
            {
                var request = _httpContextAccessor.HttpContext.Request;

                request.EnableBuffering();

                string body;
                using (var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true))
                {
                    body = await reader.ReadToEndAsync();
                    request.Body.Position = 0;
                }

                Console.WriteLine($"PhonePe Webhook Body: {body}");

                // Authorization Header
                var authHeader = request.Headers["Authorization"].FirstOrDefault();

                if (string.IsNullOrEmpty(authHeader))
                    return new { success = false, message = "Missing Authorization" };

                if (!VerifyPhonePeAuthorization(authHeader))
                    return new { success = false, message = "Invalid Authorization" };

                // Parse JSON
                var json = JObject.Parse(body);

                string eventType = json["event"]?.ToString();
                var payload = json["payload"];

                if (string.IsNullOrEmpty(eventType) || payload == null)
                    return new { success = true, message = "No event" };

                // Extract Data
                string merchantOrderId = payload["merchantOrderId"]?.ToString();
                string paymentState = payload["state"]?.ToString();

                long? amountInPaisa = payload["amount"]?.Value<long>();
                decimal amount = amountInPaisa.HasValue ? amountInPaisa.Value / 100m : 0;

                // Payment Details
                var paymentDetails = payload["paymentDetails"]?.FirstOrDefault();

                string transactionId = paymentDetails?["transactionId"]?.ToString();
                string paymentMode = paymentDetails?["paymentMode"]?.ToString();

                long? paidAmountPaisa = paymentDetails?["amount"]?.Value<long>();
                decimal paidAmount = paidAmountPaisa.HasValue ? paidAmountPaisa.Value / 100m : 0;

                // Find Fee
                var fee = _context.M_FeeDetail.FirstOrDefault(p => p.OrderNo == merchantOrderId);

                if (fee == null)
                    return new { success = false, message = "Fee not found" };

                // Process Events
                switch (eventType)
                {
                    case "checkout.order.completed":
                        if (paymentState == "COMPLETED")
                            ProcessSuccessfulPayment(fee, transactionId, paymentMode, paidAmount);
                        break;

                    case "checkout.order.failed":
                        if (paymentState == "FAILED")
                            ProcessFailedPayment(fee, transactionId, paymentMode);
                        break;

                    default:
                        ProcessPendingPayment(fee);
                        break;
                }

                return new
                {
                    success = true,
                    orderId = merchantOrderId,
                    status = paymentState
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new { success = false, error = ex.Message };
            }
        }

        private const string WebhookUsername = "thehindu";
        private const string WebhookPassword = "thehindu4092";

        // ✅ Authorization Check
        private bool VerifyPhonePeAuthorization(string authHeader)
        {
            try
            {
                string credentials = $"{WebhookUsername}:{WebhookPassword}";
                string expectedHash = ComputeSHA256Hash(credentials);

                string receivedHash = authHeader.Replace("SHA256", "").Trim();

                return expectedHash.Equals(receivedHash, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        // ✅ SHA256 Hash
        private string ComputeSHA256Hash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);

            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        // ✅ Success
        private async Task ProcessSuccessfulPayment(M_FeeDetail fee, string transactionId, string paymentMode, decimal paidAmount)
        {
            fee.OrderStatus = "Success";
            fee.Active = true;
            fee.TransactionId = transactionId;
            fee.PaymentMode = paymentMode;
            fee.PaymentDate = DateTime.Now;

            await _context.SaveChangesAsync();

            var student = _context.Student_Renew.Where(s => s.StuId == fee.stu_id && s.ClassId == fee.ClassId).FirstOrDefault();

            if (student != null)
            {
                student.due_fee -= fee.PayFees;
                student.stu_fee += fee.PayFees;
                await _context.SaveChangesAsync();
            }

            var installments = _context.fee_installment.Where(x => x.stu_id == fee.stu_id && x.university_id == fee.ClassId).OrderBy(x => x.IntallmentID).ToList();

            double remaining = Convert.ToDouble(fee.PayFees);

            foreach (var i in installments)
            {
                if (remaining <= 0) break;

                if (i.due_fee > 0)
                {
                    if (remaining >= i.due_fee)
                    {
                        remaining -= Convert.ToDouble(i.due_fee);
                        i.due_fee = 0;
                    }
                    else
                    {
                        i.due_fee -= remaining;
                        remaining = 0;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        // ❌ Failed
        private async Task ProcessFailedPayment(M_FeeDetail fee, string transactionId, string paymentMode)
        {
            fee.OrderStatus = "Failed";
            fee.Active = false;
            fee.TransactionId = transactionId;
            fee.PaymentMode = paymentMode;
            fee.PaymentDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        // ⏳ Pending
        private async Task ProcessPendingPayment(M_FeeDetail fee)
        {
            fee.OrderStatus = "Pending";
            fee.Active = false;
            await _context.SaveChangesAsync();
        }

        // ✅ Installments
        private async Task UpdateInstallments1(M_FeeDetail fee)
        {
            var installments = _context.fee_installment.Where(x => x.stu_id == fee.stu_id && x.university_id == fee.ClassId).OrderBy(x => x.IntallmentID).ToList();

            double remaining = Convert.ToDouble(fee.PayFees);

            foreach (var i in installments)
            {
                if (remaining <= 0) break;

                if (i.due_fee > 0)
                {
                    if (remaining >= i.due_fee)
                    {
                        remaining -= Convert.ToDouble(i.due_fee);
                        i.due_fee = 0;
                    }
                    else
                    {
                        i.due_fee -= remaining;
                        remaining = 0;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }




        // ===================== Transport Payment Gateway code for api  ====================== // 

        public async Task<ApiResponse<GetTransportInstallFeeModel>> GetTransportInstallFee()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;
                int ParentId = _loginUser.ParentId;
                int StudentId = _loginUser.StudentId;


                // ==== 1. GET Start Month FROM StuRouteAssignTbl ====
                var startDate = await _context.StuRouteAssignTbl.Where(c => c.stu_id == StudentId && c.CompanyId == SchoolId && c.SessionId == SessionId)
                                    .Select(c => c.Date).FirstOrDefaultAsync();

                int startYear = startDate?.Year ?? 1;
                int startMonthNo = startDate?.Month ?? 1;           // route date ka month number (int)
                string startMonth = new DateTime(startYear, startMonthNo, 1).ToString("MMMM");

                int currentMonthNo = DateTime.Now.Month;           // current month number (int)
                string currentMonth = new DateTime(startYear, currentMonthNo, 1).ToString("MMMM");

                // var validMonths = Enumerable.Range(startMonthNo, currentMonthNo - startMonthNo + 1).Select(m => new DateTime(startYear, m, 1).ToString("MMMM")).ToList();

                var validMonths = new List<string>();

                DateTime start = new DateTime(startYear, startMonthNo, 1);
                DateTime end = DateTime.Now;

                while (start <= end)
                {
                    validMonths.Add(start.ToString("MMMM"));
                    start = start.AddMonths(1);
                }

                var res = await _context.StuRouteAssignTbl.Where(c => c.stu_id == StudentId && c.CompanyId == SchoolId && c.SessionId == SessionId)
                    .Select(c => new GetTransportInstallFeeModel
                    {
                        StuRouteAssignId = c.StuRouteAssignId,
                        StudentId = c.stu_id,
                        ClassId = c.university_id,
                        sectionId = c.SectionId,
                        VehicleId = c.BusId,
                        Routeid = c.RouteId,
                        StoppageId = c.StoppageId,
                        TransportFee = c.TransportFee,
                        TotalFee = c.TTransportFee,
                        PaidFee = c.TPayFee,
                        TDiscount = c.TPayDiscount,
                        OldDueFee = c.OldDueFee,
                        OldPaidFee = c.OldPayFee,

                        StudentName = _context.student_admission.Where(a => a.stu_id == c.stu_id).Select(a => a.stu_name).FirstOrDefault(),
                        Srno = _context.student_admission.Where(a => a.stu_id == c.stu_id).Select(a => a.registration_no).FirstOrDefault(),
                        ClassName = _context.University.Where(a => a.university_id == c.university_id).Select(a => a.university_name).FirstOrDefault(),
                        sectionName = _context.collegeinfo.Where(a => a.collegeid == c.SessionId).Select(a => a.collegename).FirstOrDefault(),
                        VehicleNo = _context.TransBusTbl.Where(a => a.BusId == c.BusId).Select(a => a.VihecleNo).FirstOrDefault(),
                        RouteName = _context.TransRouteTbl.Where(a => a.RouteId == c.RouteId).Select(a => a.Route).FirstOrDefault(),
                        StoppageName = _context.TransStoppageTbl.Where(a => a.StoppageId == c.StoppageId).Select(a => a.Stoppage).FirstOrDefault(),

                        TransInatallment = _context.TransInstallmentTbl.Where(a => a.StuId == c.stu_id && a.CompanyId == SchoolId
                        && validMonths.Contains(a.MonthName)).Select(a => new TInstallmentList
                        {
                            InstallmentFee = a.InstallFee,
                            InstallmentNo = a.InstallmentNo,
                            // TTotalFee = a.TotalTransFee,
                            DueFee = a.DueFee,
                            MonthName = a.MonthName,

                        }).ToList(),

                        TransReceiptList = _context.NewTransportFeeTbl.Where(a => a.stu_id == c.stu_id && a.CompanyId == SchoolId && a.Active == true)
                        .Select(a => new TransReceiptList
                        {
                            TReceiptId = a.NewPaymentId,
                            ReceiptNo = a.ReceiptNo,
                            MonthName = a.MonthName,
                            FeeType = a.FeeType,
                            TotalFee = a.NetTransFee,
                            PayFee = a.PayFee,
                            FeeDiscount = a.Paydiscount,
                            Date = a.Date,
                            PaymentMode = a.PaymentMode,
                            Remark = a.Remark,
                        }).ToList(),

                    }).FirstOrDefaultAsync();

                return ApiResponse<GetTransportInstallFeeModel>.SuccessResponse(res, "Fetch successfully Transport Student data ");
            }
            catch (Exception ex)
            {
                return ApiResponse<GetTransportInstallFeeModel>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<StudentTransportPaymentResult>> AddStudentTransportFee(AddTransportMonthFeeReq req)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    int SchoolId = _loginUser.SchoolId;
                    int SessionId = _loginUser.SessionId;
                    int UserId = _loginUser.UserId;

                    var TransFee = await _context.NewTransportFeeTbl.Where(p => p.MonthName == req.MonthName && p.stu_id == req.StudentId && p.university_id == req.ClassId &&  p.Active == true).FirstOrDefaultAsync();
                    if (TransFee != null)
                    {
                        return ApiResponse<StudentTransportPaymentResult>.ErrorResponse("month name already avaliale");
                    }
                    //if (req.PayFee >= 0)
                    //{
                    //    return ApiResponse<StudentTransportPaymentResult>.ErrorResponse("Invalid Amount");
                    //}

                    // receipt no generate
                    var GetInstituteCodeName = await _context.institute.FirstOrDefaultAsync(i => i.institute_id == SchoolId);
                    var LastCode = await _context.NewTransportFeeTbl.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).OrderByDescending(s => s.NewPaymentId).FirstOrDefaultAsync();

                    string instCode = GetInstituteCodeName.instituteCode.Substring(0, 3).ToUpper();

                    int newId = (LastCode != null) ? int.Parse(LastCode.ReceiptNo.Split('/')[1]) + 1 : 1;
                    string ReceiptCode = $"{instCode}/{newId}";

                    // order no generate
                    int NewOrderNo = 1;
                    var LastOrderNo = _context.NewTransportFeeTbl.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).Select(s => s.OrderNo).ToList()
                        .Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x)).OrderByDescending(x => x).FirstOrDefault();

                    if (LastOrderNo > 0)
                        NewOrderNo = LastOrderNo + 1;

                    int NewPaymentId = _context.NewTransportFeeTbl.DefaultIfEmpty().Max(s => s == null ? 0 : s.NewPaymentId) + 1;

                    var StuInstall = new NewTransportFeeTbl
                    {
                        NewPaymentId = NewPaymentId,
                        ReceiptNo = ReceiptCode,
                        StuRouteAssignId = req.StuRouteAssignId,
                        stu_id = req.StudentId,
                        university_id = req.ClassId,
                        SectionId = req.sectionId,
                        Date = DateTime.UtcNow,
                        BusId = req.VehicleId,
                        RouteId = req.RouteId,
                        StoppageId = req.StoppageId,
                        TransFee = req.PayFee ?? 0,
                        MonthType = req.MonthType,
                        MonthName = req.MonthName,
                        NetTransFee = req.PayFee ?? 0,
                        PayFee = req.PayFee ?? 0,
                        FeeType = "TransportFee",
                        OrderNo = NewOrderNo.ToString(),
                        OrderStatus = "Pending",
                        TransactionId = "",
                        ReceiptType = "Online",
                        Active = false,
                        CompanyId = SchoolId,
                        SessionId = SessionId,
                        Userid = UserId,
                        PaymentMode = "UPI",
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        Remark = req.Remark,
                    };
                    _context.NewTransportFeeTbl.Add(StuInstall);
                    await _context.SaveChangesAsync();


                    string token = await GetValidToken();
                    if (string.IsNullOrEmpty(token))
                    {
                        await transaction.RollbackAsync();
                        return ApiResponse<StudentTransportPaymentResult>.ErrorResponse("Failed to fetch authorization token");
                    }

                    // =========================================== STEP 2 → PAYMENT REQUEST

                    string apiUrl = "https://api.phonepe.com/apis/pg/checkout/v2/sdk/order";            // Production is orginal url

                    //    string apiUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/sdk/order";        //  Sandbox is a testing demo url

                    double amount = Convert.ToDouble(req.PayFee);

                    var paymentRequest = new
                    {
                        merchantOrderId = NewOrderNo.ToString(),
                        amount = (int)(amount * 100),
                        expireAfter = 300,


                        metaInfo = new
                        {
                            udf1 = "StudentFee",
                            udf2 = req.StudentId.ToString(),
                            udf3 = req.ClassId.ToString(),
                            udf4 = ReceiptCode,
                            udf5 = "InstituteFee"
                        },

                        paymentFlow = new
                        {
                            type = "PG_CHECKOUT",
                            message = "Fee payment",
                            //merchantUrls = new
                            //{
                            //    redirectUrl = $"https://thistestapi.vedusoft.in/api/payment/UpdateTransportPaymentSuccessfully?StudentId={req.StudentId}&ReceiptId={NewPaymentId}"
                            //}
                        }
                    };

                    string jsonBody = JsonConvert.SerializeObject(paymentRequest);
                    byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonBody);

                    HttpWebRequest payRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
                    payRequest.Method = "POST";
                    payRequest.ContentType = "application/json";
                    //  payRequest.Headers.Add("Authorization", "Bearer " + token);
                    payRequest.Headers.Add("Authorization", "O-Bearer " + token);

                    using (var stream = payRequest.GetRequestStream())
                        stream.Write(jsonBytes, 0, jsonBytes.Length);

                    string payResponseString;
                    var payResponse = (HttpWebResponse)payRequest.GetResponse();
                    using (var reader = new StreamReader(payResponse.GetResponseStream()))
                        payResponseString = reader.ReadToEnd();

                    dynamic paymentResponse = JsonConvert.DeserializeObject(payResponseString);
                    string redirectUrl = paymentResponse.redirectUrl;

                    // =================         PENDING CHECK LOOP

                    _backgroundJobs.Schedule(
                        () => CheckTransportPaymentStatusBackground(NewOrderNo.ToString()),
                        TimeSpan.FromSeconds(10)
                        );

                    await transaction.CommitAsync();
                    return ApiResponse<StudentTransportPaymentResult>.SuccessResponse(
                        new StudentTransportPaymentResult
                        {
                            NewPaymentId = StuInstall.NewPaymentId,
                            ReceiptNo = ReceiptCode,
                            merchantOrderId = StuInstall.OrderNo,
                            OrderId = paymentResponse.orderId,
                            State = paymentResponse.state,
                            ExpireAt = paymentResponse.expireAt,
                            Token = paymentResponse.token
                        },
                        "Installment fee saved successfully"
                        );

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return ApiResponse<StudentTransportPaymentResult>.ErrorResponse("Error: " + ex.Message);
                }
            }
        }

        public async Task CheckTransportPaymentStatusBackground(string orderId)
        {
            try
            {
                string token = await GetValidToken();

                //   string url = $"https://api.phonepe.com/apis/pg/checkout/v2/order/{orderId}/status";                // Production is orginal url

                string url = $"https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/order/{orderId}/status";         //  Sandbox is a testing demo url

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    var response = await client.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("STATUS RESPONSE: " + result);

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("ERROR: " + result);
                        return;
                    }

                    dynamic res = JsonConvert.DeserializeObject(result);

                    string state = res?.state;
                    string txnId = res?.paymentDetails?[0]?.transactionId;

                    if (state == "COMPLETED" || state == "FAILED")
                    {
                        var order = _context.NewTransportFeeTbl.FirstOrDefault(x => x.OrderNo == orderId);

                        if (order != null)
                        {
                            order.OrderStatus = state;
                            order.TransactionId = txnId;

                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Status Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateTransportPaymentSuccessfully(int StudentId, int ReceiptId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var Tfee = await _context.NewTransportFeeTbl.FirstOrDefaultAsync(a => a.NewPaymentId == ReceiptId && a.SessionId == SessionId && a.CompanyId == SchoolId);

                if (Tfee == null)
                    return ApiResponse<bool>.ErrorResponse("Invalid request!");

                // ✅ TOKEN
                string token = await GetValidToken();
                if (string.IsNullOrEmpty(token))
                    return ApiResponse<bool>.ErrorResponse("Token error");

                // ✅ STATUS CHECK
                string merchantOrderId = Tfee.OrderNo;

                string url = $"https://api.phonepe.com/apis/pg/checkout/v2/order/{merchantOrderId}/status";         // Production WALA URL ORGINAL 

                // string url = $"https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/order/{merchantOrderId}/status";          //  Sandbox demo url for testing 

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", "O-Bearer " + token);

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return ApiResponse<bool>.ErrorResponse("Status API failed");

                string result = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(result);

                string status = json.state;
                var payment = json.paymentDetails?[0];

                string txnId = payment?.transactionId;

                // ================= SUCCESS
                if (status == "COMPLETED")
                {
                    Tfee.OrderStatus = "Success";
                    Tfee.Active = true;
                    Tfee.TransactionId = txnId ?? "";

                    await _context.SaveChangesAsync();

                    var sturouteasign = _context.StuRouteAssignTbl.FirstOrDefault(s => s.stu_id == Tfee.stu_id && s.SessionId == SessionId && s.CompanyId == SchoolId);
                    sturouteasign.TTransportFee += Tfee.NetTransFee;
                    sturouteasign.TDueFee = Tfee.DueFee;
                    sturouteasign.TPayDiscount += Tfee.Paydiscount;
                    sturouteasign.TPayFee += Tfee.PayFee;

                    await _context.SaveChangesAsync();


                    // Month name Seperate   ',' 
                    var month = Tfee.MonthName.Split(',').Select(m => m.Trim()).ToList();

                    // every month for TransInstallmentTbl 
                    var transInstallments = _context.TransInstallmentTbl.Where(u => u.StuId == Tfee.stu_id && u.ClassId == Tfee.university_id && u.SessionId == SessionId &&
                     u.CompanyId == SchoolId && month.Contains(u.MonthName)).ToList();

                    for (int i = 0; i < transInstallments.Count; i++)
                    {
                        transInstallments[i].ReActive = true;
                    }
                    await _context.SaveChangesAsync();

                    // ev.PayFee convert in   double and azume in null  ;; and 0.0 
                    double remainingPay = (Tfee.PayFee ?? 0.0) + (Tfee.Paydiscount ?? 0.0);

                    // get all installment  सभी सक्रिय इंस्टॉलमेंट्स को प्राप्त करें, जिन्हें पहले सक्रिय किया गया है
                    var activeInstallments = _context.TransInstallmentTbl.Where(u => u.StuId == Tfee.stu_id && u.ClassId == Tfee.university_id && u.SessionId == SessionId &&
                    u.CompanyId == SchoolId && u.ReActive == true).OrderBy(u => u.MonthName).ToList();

                    // प्रत्येक इंस्टॉलमेंट के लिए DueFee को घटाएं जब तक कि remainingPay समाप्त न हो जाए
                    for (int i = 0; i < activeInstallments.Count && remainingPay > 0; i++)
                    {
                        var installment = activeInstallments[i];
                        if (installment.DueFee.HasValue && installment.DueFee.Value > 0)
                        {
                            double dueFee = installment.DueFee.Value;
                            if (remainingPay >= dueFee)
                            {
                                remainingPay -= dueFee;
                                installment.DueFee = 0;
                            }
                            else
                            {
                                installment.DueFee = dueFee - remainingPay;
                                remainingPay = 0;
                            }
                            await _context.SaveChangesAsync();
                        }
                    }

                    return ApiResponse<bool>.SuccessResponse(true, "Payment successful");
                }
                // ================= FAILED
                else if (status == "FAILED")
                {
                    Tfee.OrderStatus = "Failed";
                    Tfee.Active = false;

                    await _context.SaveChangesAsync();
                    return ApiResponse<bool>.SuccessResponse(true, "Payment Failed");

                }
                // ================= PENDING
                else
                {
                    return ApiResponse<bool>.SuccessResponse(true, "Payment Pending");

                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error : " + ex.Message);
            }
        }

        // Webhook status code 
        public async Task<object> WebhookTransportStatus()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var request = _httpContextAccessor.HttpContext.Request;

                request.EnableBuffering();

                string body;
                using (var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true))
                {
                    body = await reader.ReadToEndAsync();
                    request.Body.Position = 0;
                }

                Console.WriteLine($"PhonePe Webhook Body: {body}");

                // Authorization Header
                var authHeader = request.Headers["Authorization"].FirstOrDefault();

                if (string.IsNullOrEmpty(authHeader))
                    return new { success = false, message = "Missing Authorization" };

                if (!VerifyPhonePeAuthorization(authHeader))
                    return new { success = false, message = "Invalid Authorization" };

                // Parse JSON
                var json = JObject.Parse(body);

                string eventType = json["event"]?.ToString();
                var payload = json["payload"];

                if (string.IsNullOrEmpty(eventType) || payload == null)
                    return new { success = true, message = "No event" };

                // Extract Data
                string merchantOrderId = payload["merchantOrderId"]?.ToString();
                string paymentState = payload["state"]?.ToString();

                long? amountInPaisa = payload["amount"]?.Value<long>();
                decimal amount = amountInPaisa.HasValue ? amountInPaisa.Value / 100m : 0;

                // Payment Details
                var paymentDetails = payload["paymentDetails"]?.FirstOrDefault();

                string transactionId = paymentDetails?["transactionId"]?.ToString();
                string paymentMode = paymentDetails?["paymentMode"]?.ToString();

                long? paidAmountPaisa = paymentDetails?["amount"]?.Value<long>();
                decimal paidAmount = paidAmountPaisa.HasValue ? paidAmountPaisa.Value / 100m : 0;

                // Find Fee
                var Transfee = _context.NewTransportFeeTbl.FirstOrDefault(p => p.OrderNo == merchantOrderId);

                if (Transfee == null)
                    return new { success = false, message = "Fee not found" };

                // Process Events
                switch (eventType)
                {
                    case "checkout.order.completed":
                        if (paymentState == "COMPLETED")
                            TransProcessSuccessfulPayment(Transfee, transactionId, paymentMode, paidAmount);
                        break;

                    case "checkout.order.failed":
                        if (paymentState == "FAILED")
                            TransProcessFailedPayment(Transfee, transactionId, paymentMode);
                        break;

                    default:
                        TransProcessPendingPayment(Transfee);
                        break;
                }

                return new
                {
                    success = true,
                    orderId = merchantOrderId,
                    status = paymentState
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new { success = false, error = ex.Message };
            }
        }

        private async Task TransProcessSuccessfulPayment(NewTransportFeeTbl Transfee, string transactionId, string paymentMode, decimal paidAmount)
        {
            int SchoolId = _loginUser.SchoolId;
            int SessionId = _loginUser.SessionId;

            Transfee.OrderStatus = "Success";
            Transfee.Active = true;
            Transfee.TransactionId = transactionId;
            Transfee.PaymentMode = paymentMode;
            Transfee.Date = DateTime.Now;

            await _context.SaveChangesAsync();

            var sturouteasign = _context.StuRouteAssignTbl.FirstOrDefault(s => s.stu_id == Transfee.stu_id && s.SessionId == SessionId && s.CompanyId == SchoolId);
            sturouteasign.TTransportFee += Transfee.NetTransFee;
            sturouteasign.TDueFee = Transfee.DueFee;
            sturouteasign.TPayDiscount += Transfee.Paydiscount;
            sturouteasign.TPayFee += Transfee.PayFee;

            await _context.SaveChangesAsync();

            var months = Transfee.MonthName.Split(',').Select(m => m.Trim()).ToList();

            // every month transport installment
            var installments = _context.TransInstallmentTbl.Where(u => u.StuId == Transfee.stu_id && u.ClassId == Transfee.university_id && u.SessionId == SessionId &&
             u.CompanyId == SchoolId && months.Contains(u.MonthName)).ToList();

            for (int i = 0; i < installments.Count; i++)
            {
                installments[i].ReActive = true;
            }
            await _context.SaveChangesAsync();

            double remainingPay = (Transfee.PayFee ?? 0.0) + (Transfee.Paydiscount ?? 0.0);

            var activeInstallments = _context.TransInstallmentTbl.Where(u => u.StuId == Transfee.stu_id && u.ClassId == Transfee.university_id && u.SessionId == SessionId &&
                u.CompanyId == SchoolId && u.ReActive == true).OrderBy(u => u.MonthName).ToList();

            for (int i = 0; i < activeInstallments.Count && remainingPay > 0; i++)
            {
                var installment = activeInstallments[i];
                if (installment.DueFee.HasValue && installment.DueFee.Value > 0)
                {
                    double dueFee = installment.DueFee.Value;
                    if (remainingPay >= dueFee)
                    {
                        remainingPay -= dueFee;
                        installment.DueFee = 0;
                    }
                    else
                    {
                        installment.DueFee = dueFee - remainingPay;
                        remainingPay = 0;
                    }
                    await _context.SaveChangesAsync();
                }
            }
        }

        // ❌ Failed
        private async Task TransProcessFailedPayment(NewTransportFeeTbl Transfee, string transactionId, string paymentMode)
        {
            Transfee.OrderStatus = "Failed";
            Transfee.Active = false;
            Transfee.TransactionId = transactionId;
            Transfee.PaymentMode = paymentMode;
            Transfee.Date = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        // ⏳ Pending
        private async Task TransProcessPendingPayment(NewTransportFeeTbl Transfee)
        {
            Transfee.OrderStatus = "Pending";
            Transfee.Active = false;
            await _context.SaveChangesAsync();
        }

        // ==================================== transport payment code end ======================= //

        public async Task<ApiResponse<bool>> UpdateTransportPaymentSuccessfully1(int StudentId, int ReceiptId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var Tfee = _context.NewTransportFeeTbl.FirstOrDefault(a => a.NewPaymentId == ReceiptId && a.SessionId == SessionId && a.CompanyId == SchoolId);

                if (Tfee == null)
                    return ApiResponse<bool>.ErrorResponse("Invalid request!");

                //Tfee.OrderStatus = "Success";
                //// Tfee.Status = "Success";
                ////   Tfee.TransactionId = txnId ?? "";
                //Tfee.Active = true;
                //await _context.SaveChangesAsync();

                // =========================================== GET TOKEN FROM COOKIE

                string authUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox/v1/oauth/token";

                string client_id = "SHYAWAYUAT_2510101108212";
                string client_version = "1";
                string client_secret = "NWQ2YzJlZDktODU3Yi00ZWUzLTk1MTItOTJhZDVkYjkxYmYx";

                string postData = $"client_id={client_id}&client_version={client_version}&client_secret={client_secret}&grant_type=client_credentials";

                byte[] authBytes = Encoding.UTF8.GetBytes(postData);

                HttpWebRequest authRequest = (HttpWebRequest)WebRequest.Create(authUrl);
                authRequest.Method = "POST";
                authRequest.ContentType = "application/x-www-form-urlencoded";
                authRequest.ContentLength = authBytes.Length;

                using (var stream = authRequest.GetRequestStream())
                    stream.Write(authBytes, 0, authBytes.Length);

                string authResponseStr;
                using (var responses = (HttpWebResponse)authRequest.GetResponse())
                using (var reader = new StreamReader(responses.GetResponseStream()))
                    authResponseStr = reader.ReadToEnd();

                dynamic authResponse = JsonConvert.DeserializeObject(authResponseStr);
                string token = authResponse.access_token;

                if (string.IsNullOrEmpty(token))
                {
                    // await transaction.RollbackAsync();
                    await _context.SaveChangesAsync();
                    return ApiResponse<bool>.ErrorResponse("Failed to fetch authorization token");
                }


                // =========================================== PREPARE STATUS CHECK URL
                string merchantOrderId = Tfee.OrderNo;

                string url = $"https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/order/{merchantOrderId}/status?details=false";

                //   string url = $"https://api.phonepe.com/apis/pg/checkout/v2/order/{orderId}/status";                // Production is orginal url

                //  string url = $"https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/order/{orderId}/status";         //  Sandbox is a testing demo url

                // =========================================== CALL PHONEPE STATUS API (Using HttpClient)
                using var client = new HttpClient();

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "O-Bearer " + token);

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return ApiResponse<bool>.ErrorResponse("Failed to check status!");
                }

                string result = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(result);

                string status = json.state; // COMPLETED / FAILED / PENDING
                var payment = json.paymentDetails?[0];

                string paymentState = payment?.state;

                string txnId = payment?.transactionId;

                if (status == "COMPLETED")
                {
                    Tfee.OrderStatus = "Success";
                    Tfee.Active = true;
                    Tfee.TransactionId = txnId ?? "";
                    await _context.SaveChangesAsync();


                    var sturouteasign = _context.StuRouteAssignTbl.FirstOrDefault(s => s.stu_id == Tfee.stu_id && s.SessionId == SessionId && s.CompanyId == SchoolId);
                    sturouteasign.TTransportFee += Tfee.NetTransFee;
                    sturouteasign.TDueFee = Tfee.DueFee;
                    sturouteasign.TPayDiscount += Tfee.Paydiscount;
                    sturouteasign.TPayFee += Tfee.PayFee;

                    await _context.SaveChangesAsync();


                    // Month name Seperate   ',' 
                    var month = Tfee.MonthName.Split(',').Select(m => m.Trim()).ToList();

                    // every month for TransInstallmentTbl 
                    var transInstallments = _context.TransInstallmentTbl.Where(u => u.StuId == Tfee.stu_id && u.ClassId == Tfee.university_id && u.SessionId == SessionId &&
                     u.CompanyId == SchoolId && month.Contains(u.MonthName)).ToList();

                    for (int i = 0; i < transInstallments.Count; i++)
                    {
                        transInstallments[i].ReActive = true;
                    }
                    await _context.SaveChangesAsync();

                    // ev.PayFee convert in   double and azume in null  ;; and 0.0 
                    double remainingPay = (Tfee.PayFee ?? 0.0) + (Tfee.Paydiscount ?? 0.0);

                    // get all installment  सभी सक्रिय इंस्टॉलमेंट्स को प्राप्त करें, जिन्हें पहले सक्रिय किया गया है
                    var activeInstallments = _context.TransInstallmentTbl.Where(u => u.StuId == Tfee.stu_id && u.ClassId == Tfee.university_id && u.SessionId == SessionId &&
                    u.CompanyId == SchoolId && u.ReActive == true).OrderBy(u => u.MonthName).ToList();

                    // प्रत्येक इंस्टॉलमेंट के लिए DueFee को घटाएं जब तक कि remainingPay समाप्त न हो जाए
                    for (int i = 0; i < activeInstallments.Count && remainingPay > 0; i++)
                    {
                        var installment = activeInstallments[i];
                        if (installment.DueFee.HasValue && installment.DueFee.Value > 0)
                        {
                            double dueFee = installment.DueFee.Value;
                            if (remainingPay >= dueFee)
                            {
                                remainingPay -= dueFee;
                                installment.DueFee = 0;
                            }
                            else
                            {
                                installment.DueFee = dueFee - remainingPay;
                                remainingPay = 0;
                            }
                            await _context.SaveChangesAsync();
                        }
                    }

                    return ApiResponse<bool>.SuccessResponse(true, "Payment successful");
                }
                else if (status == "FAILED")
                {
                    Tfee.OrderStatus = "Failed";
                    Tfee.Active = false;
                    await _context.SaveChangesAsync();

                    return ApiResponse<bool>.SuccessResponse(false, "Payment failed");
                }
                else
                {
                    Tfee.OrderStatus = "Pending";
                    Tfee.Active = false;
                    await _context.SaveChangesAsync();

                    return ApiResponse<bool>.SuccessResponse(false, "Payment pending");
                }

                // return ApiResponse<bool>.SuccessResponse(true, "Student transport fee data fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error : " + ex.Message);
            }
        }


        public async Task<ApiResponse<GetStudentFeeModel>> GetStudentFee()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;
                int StudentId = _loginUser.StudentId;

                var Stufee = await _context.StudentRenewView.Where(a => a.StuId == StudentId && a.CompanyId == SchoolId && a.SessionId == SessionId && a.RActive == true)
                    .Select(a => new GetStudentFeeModel
                    {
                        TotalFee = a.total_fee,
                        PaidAmount = _context.M_FeeDetail.Where(u => u.stu_id == a.StuId && u.ClassId == a.ClassId && u.SessionId == SessionId && u.CompanyId == SchoolId
                        && u.Status == "1" && u.Active == true).Sum(u => u.PayFees) ?? 0,

                        DueAmount = (a.total_fee ?? 0) - (_context.M_FeeDetail.Where(u => u.stu_id == a.StuId && u.ClassId == a.ClassId && u.SessionId == SessionId
                            && u.CompanyId == SchoolId && u.Status == "1" && u.Active == true).Sum(u => u.PayFees) ?? 0),

                    }).FirstOrDefaultAsync();

                return ApiResponse<GetStudentFeeModel>.SuccessResponse(Stufee, "Student fee fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<GetStudentFeeModel>.ErrorResponse("Error :" + ex.Message);
            }
        }

        public async Task<ApiResponse<GetStuFeeInstallmentModel>> GetStudentFeeInstallment()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;
                int StudentId = _loginUser.StudentId;

                var Installfee = await _context.StudentRenewView.Where(a => a.StuId == StudentId && a.CompanyId == SchoolId && a.SessionId == SessionId && a.RActive == true)
                    .Select(a => new GetStuFeeInstallmentModel
                    {
                        Installment = _context.fee_installment.Where(p => p.stu_id == a.StuId && p.university_id == a.ClassId && p.CompanyId == SchoolId && p.SessionId == SessionId)
                        .Select(p => p.FAmount).ToList(),

                    }).FirstOrDefaultAsync();

                return ApiResponse<GetStuFeeInstallmentModel>.SuccessResponse(Installfee, "Student fee installment fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<GetStuFeeInstallmentModel>.ErrorResponse("Error :" + ex.Message);
            }
        }

        public async Task<ApiResponse<GetStuDueInstallmentModel>> GetStudentDueInstallment()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;
                int StudentId = _loginUser.StudentId;

                var studentData = await _context.StudentRenewView.Where(a => a.StuId == StudentId && a.CompanyId == SchoolId && a.SessionId == SessionId
                   && a.RActive == true).Select(a => new
                   {
                       a.StuId,
                       a.ClassId
                   }).FirstOrDefaultAsync();

                if (studentData == null)
                    return ApiResponse<GetStuDueInstallmentModel>.ErrorResponse("Student not found");

                // Get total paid amount
                double PaidAmount = await _context.M_FeeDetail.Where(u => u.stu_id == studentData.StuId && u.ClassId == studentData.ClassId &&
                      u.SessionId == SessionId && u.CompanyId == SchoolId && u.Status == "1" && u.Active == true).SumAsync(u => (double?)u.PayFees) ?? 0;

                // Get all installments
                var Installments = await _context.fee_installment.Where(u => u.stu_id == studentData.StuId && u.university_id == studentData.ClassId &&
                        u.SessionId == SessionId && u.CompanyId == SchoolId).Select(u => u.FAmount).ToListAsync();

                // Update the same Installment list directly
                for (int i = 0; i < Installments.Count; i++)
                {
                    var insAmount = Installments[i];

                    if (PaidAmount >= insAmount)
                    {
                        // fully paid
                        PaidAmount -= (double)insAmount;
                        Installments[i] = 0;
                    }
                    else if (PaidAmount > 0)
                    {
                        // partially paid
                        Installments[i] = insAmount - PaidAmount;
                        PaidAmount = 0;
                    }
                    else
                    {
                        // unpaid
                        Installments[i] = insAmount;
                    }
                }

                var result = new GetStuDueInstallmentModel
                {
                    //   PaidAmount = PaidAmount,
                    DueInstallment = Installments
                };

                return ApiResponse<GetStuDueInstallmentModel>.SuccessResponse(result, "Remaining due installments fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<GetStuDueInstallmentModel>.ErrorResponse("Error: " + ex.Message);
            }
        }


        // change password 
        public async Task<ApiResponse<GetPasswordModel>> GetPassword(int Studentid)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;
                int StudentId = _loginUser.StudentId;

                var Stufee = await _context.StudentRenewView.Where(a => a.StuId == StudentId && a.CompanyId == SchoolId && a.SessionId == SessionId && a.RActive == true)
                    .Select(a => new GetPasswordModel
                    {
                        StudentId = a.StuId,
                        ParentsId = a.ParentsId,
                        Studentname = a.stu_name,
                        Username = a.PUsername,
                        Password = a.PPassword,

                    }).FirstOrDefaultAsync();

                return ApiResponse<GetPasswordModel>.SuccessResponse(Stufee, "Student fee fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<GetPasswordModel>.ErrorResponse("Error :" + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> Changepassword(GetChangepasswordReq req)
        {
            try
            {
                if (req == null || string.IsNullOrEmpty(req.password))
                {
                    return ApiResponse<bool>.ErrorResponse("Invalid request");
                }

                int SchoolId = _loginUser.SchoolId;

                var parent = await _context.ParentsTbl.FirstOrDefaultAsync(p => p.ParentsId == req.ParentsId && p.CompanyId == SchoolId);

                if (parent == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Parent not found");
                }

                parent.Password = req.password;

                var student = await _context.student_admission.FirstOrDefaultAsync(p => p.ParentsId == req.ParentsId && p.CompanyId == SchoolId);

                if (student != null)
                {
                    student.password = req.password;
                }

                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Password changed successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }



        // ============================= other extra code  ===================== 

        public async Task<string> GetNewAccessToken1()
        {
            //   string authUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox/v1/oauth/token";

            string authUrl = "https://api.phonepe.com/apis/identity-manager/v1/oauth/token";

            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("client_id", "SU2602051921478872343710"),
                    new KeyValuePair<string, string>("client_version", "1"),
                    new KeyValuePair<string, string>("client_secret", "541e3c3f-0fc7-4e42-a141-28922b4eba32"),
                    new KeyValuePair<string, string>("grant_type", "client_credentials")

            // new KeyValuePair<string, string>("client_id", "SHYAWAYUAT_2510101108212"),
            //new KeyValuePair<string, string>("client_version", "1"),
            //new KeyValuePair<string, string>("client_secret", "NWQ2YzJlZDktODU3Yi00ZWUzLTk1MTItOTJhZDVkYjkxYmYx"),
            //new KeyValuePair<string, string>("grant_type", "client_credentials")
        
               });

                var response = await client.PostAsync(authUrl, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Token Error: " + result);

                dynamic json = JsonConvert.DeserializeObject(result);

                _phonePeToken = json.access_token;
                _tokenExpiry = DateTime.Now.AddSeconds((int)json.expires_in - 60);

                return _phonePeToken;
            }
        }

        public async Task<string> GetValidToken1()
        {
            if (string.IsNullOrEmpty(_phonePeToken) || DateTime.Now >= _tokenExpiry)
            {
                return await GetNewAccessToken();
            }
            return _phonePeToken;
        }

        public async Task<ApiResponse<StudentFeePaymentResult>> AddStudentInstallmentFee1(AddStudentinstallReq req)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    int SchoolId = _loginUser.SchoolId;
                    int SessionId = _loginUser.SessionId;

                    // ===========================================  CHECK — Student has no pending due already
                    var student = await _context.Student_Renew.FirstOrDefaultAsync(a => a.ClassId == req.ClassId && a.StuId == req.StudentId && a.due_fee != 0);

                    if (student == null)
                    {
                        return ApiResponse<StudentFeePaymentResult>.ErrorResponse("Due fee already available");
                    }

                    // =========================================== INSTITUTE CODE & RECEIPT NUMBER
                    var GetInstituteCodeName = await _context.institute.FirstOrDefaultAsync(i => i.institute_id == SchoolId);

                    var LastCode = await _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).OrderByDescending(s => s.FDId).FirstOrDefaultAsync();

                    string instCode = GetInstituteCodeName.instituteCode.Substring(0, 3).ToUpper();
                    int newId = (LastCode != null)
                                ? int.Parse(LastCode.ReceiptNo.Split('/')[1]) + 1
                                : 1;

                    string ReceiptCode = $"{instCode}/{newId}";


                    // =========================================== GENERATE ORDER NUMBER
                    int NewOrderNo = 1;
                    var LastOrderNo = await _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).OrderByDescending(s => s.OrderNo)
                        .Select(s => s.OrderNo).FirstOrDefaultAsync();

                    if (!string.IsNullOrEmpty(LastOrderNo))
                    {
                        if (int.TryParse(LastOrderNo, out int last))
                            NewOrderNo = last + 1;
                    }

                    // =========================================== GENERATE PRIMARY KEY FDId
                    int FDId = _context.M_FeeDetail.DefaultIfEmpty().Max(s => s == null ? 0 : s.FDId) + 1;

                    // =========================================== SAVE INSTALLMENT RECORD
                    var fee = new M_FeeDetail
                    {
                        FDId = FDId,
                        stu_id = req.StudentId,
                        ClassId = req.ClassId,
                        OrderNo = NewOrderNo.ToString(),
                        OrderStatus = "Pending",
                        TransactionId = "",
                        ReceiptType = "Online",
                        DueFees = student.due_fee - req.PaidFee,
                        PayFees = req.PaidFee,
                        Cash = 0,
                        Upi = 0,
                        AdmissionPayfee = 0,
                        AFeeDiscount = 0,
                        PramoteFees = 0,
                        Date = DateTime.Now,
                        Status = "InstallmentFee",
                        Active = false,
                        CompanyId = SchoolId,
                        SessionId = SessionId,
                        NetDueFees = student.due_fee - req.PaidFee,
                        PaymentMode = "UPI",
                        PaymentDate = DateTime.UtcNow,
                        Remark = req.Remark,
                        RTS = DateTime.Now,
                        ReceiptNo = ReceiptCode
                    };

                    _context.M_FeeDetail.Add(fee);
                    await _context.SaveChangesAsync();


                    string token = await GetValidToken1();
                    if (string.IsNullOrEmpty(token))
                    {
                        await transaction.RollbackAsync();
                        return ApiResponse<StudentFeePaymentResult>.ErrorResponse("Failed to fetch authorization token");
                    }

                    // =========================================== STEP 2 → PAYMENT REQUEST

                    //  string apiUrl = "https://api.phonepe.com/apis/pg/checkout/v2/pay";      // Production ye web  ke liye h 

                    string apiUrl = "https://api.phonepe.com/apis/pg/checkout/v2/sdk/order";    //  Production  mobile ke liye h 


                    double amount = Convert.ToDouble(req.PaidFee);

                    var paymentRequest = new
                    {
                        merchantOrderId = NewOrderNo.ToString(),
                        amount = (int)(amount * 100),
                        expireAfter = 300,


                        metaInfo = new
                        {
                            udf1 = "StudentFee",
                            udf2 = req.StudentId.ToString(),
                            udf3 = req.ClassId.ToString(),
                            udf4 = ReceiptCode,
                            udf5 = "InstituteFee"
                        },

                        paymentFlow = new
                        {
                            type = "PG_CHECKOUT",
                            message = "Fee payment",
                            //merchantUrls = new
                            //{
                            //    redirectUrl = Url.Action("PaymentSuccessfully", "Home",
                            //        new { StudentId = req.StudentId, Receiptid = fee.FDId },
                            //        Request.Url.Scheme)
                            //}
                        }
                    };

                    string jsonBody = JsonConvert.SerializeObject(paymentRequest);
                    byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonBody);

                    HttpWebRequest payRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
                    payRequest.Method = "POST";
                    payRequest.ContentType = "application/json";
                    payRequest.Headers.Add("Authorization", "Bearer " + token);
                    //  payRequest.Headers.Add("Authorization", "O-Bearer " + token);

                    using (var stream = payRequest.GetRequestStream())
                        stream.Write(jsonBytes, 0, jsonBytes.Length);

                    var payResponse = (HttpWebResponse)payRequest.GetResponse();
                    string payResponseString;

                    using (var reader = new StreamReader(payResponse.GetResponseStream()))
                        payResponseString = reader.ReadToEnd();



                    dynamic paymentResponse = JsonConvert.DeserializeObject(payResponseString);
                    string redirectUrl = paymentResponse.redirectUrl;

                    // =================         PENDING CHECK LOOP

                    _backgroundJobs.Schedule(
                        () => CheckPaymentStatusBackground1(NewOrderNo.ToString()),
                        TimeSpan.FromSeconds(10)
                        );


                    await transaction.CommitAsync();
                    return ApiResponse<StudentFeePaymentResult>.SuccessResponse(
                        new StudentFeePaymentResult
                        {
                            FDId = fee.FDId,
                            ReceiptNo = ReceiptCode,
                            merchantOrderId = fee.OrderNo,
                            OrderId = paymentResponse.orderId,
                            State = paymentResponse.state,
                            ExpireAt = paymentResponse.expireAt,
                            Token = paymentResponse.token
                        },
                        "Installment fee saved successfully"
                        );
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return ApiResponse<StudentFeePaymentResult>.ErrorResponse("Error: " + ex.Message);
                }
            }
        }


        public async Task CheckPaymentStatusBackground1(string orderId)
        {
            string token = await GetValidToken1();

            //    string url = "https://api.phonepe.com/apis/pg/checkout/v2/order/" + orderId + "/status";       // Production is orginal url
            string url = $"https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/order/{orderId}/status";         //  Sandbox is a testing demo url

            using (var client = new HttpClient())
            {
                //  client.DefaultRequestHeaders.Add("Authorization", "O-Bearer " + token);

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                Console.WriteLine("STATUS RESPONSE: " + result);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("ERROR: " + result);
                    return;
                }

                dynamic res = JsonConvert.DeserializeObject(result);

                string state = res?.state;
                string txnId = res?.paymentDetails?[0]?.transactionId;

                if (state == "COMPLETED" || state == "FAILED")
                {
                    var order = _context.M_FeeDetail.FirstOrDefault(x => x.OrderNo == orderId);

                    if (order != null)
                    {
                        order.OrderStatus = state;
                        order.TransactionId = txnId;
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }



        //public async Task<ApiResponse<bool>> AddStudentInstallmentFee(AddStudentinstallReq req)
        //{
        //    try
        //    {
        //        int SchoolId = _loginUser.SchoolId;
        //        int SessionId = _loginUser.SessionId;

        //        var student = await _context.Student_Renew.Where(a => a.ClassId == req.ClassId && a.StuId == req.StudentId && a.due_fee == 0).FirstOrDefaultAsync();
        //        if (student != null)
        //        {
        //            return ApiResponse<bool>.ErrorResponse("duefee already availble");
        //        }

        //        var GetInstituteCodeName = await _context.institute.FirstOrDefaultAsync(i => i.institute_id == SchoolId);
        //        var LastCode = await _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).OrderByDescending(s => s.FDId).FirstOrDefaultAsync();

        //        string instCode = GetInstituteCodeName.instituteCode.Substring(0, 3).ToUpper();

        //        int newId = (LastCode != null) ? int.Parse(LastCode.ReceiptNo.Split('/')[1]) + 1 : 1;

        //        string ReceiptCode = $"{instCode}/{newId}";
        //        int NewOrderNo = 1;
        //        var LastOrderNo = await _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).OrderByDescending(s => s.OrderNo)
        //            .Select(s => s.OrderNo).FirstOrDefaultAsync();

        //        if (LastOrderNo != null && LastOrderNo != "")

        //            if (!string.IsNullOrEmpty(LastOrderNo))
        //            {
        //                int lastNum;
        //                if (int.TryParse(LastOrderNo, out lastNum))
        //                {
        //                    NewOrderNo = lastNum + 1;
        //                }
        //            }

        //        int FDId = _context.M_FeeDetail.DefaultIfEmpty().Max(s => s == null ? 0 : s.FDId) + 1;

        //        var StuInstall = new M_FeeDetail
        //        {
        //            FDId = FDId,
        //            stu_id = req.StudentId,
        //            ClassId = req.ClassId,
        //            OrderNo = NewOrderNo.ToString(),
        //            OrderStatus = "Pending",
        //            TransactionId = "",
        //            ReceiptType = "Online",
        //            DueFees = req.Duefee,
        //            PayFees = req.PaidFee,
        //            Cash = 0,
        //            Upi = 0,
        //            AdmissionPayfee = 0,
        //            AFeeDiscount = 0,
        //            PramoteFees = 0,
        //            Date = DateTime.Now,
        //            Status = "Pending",
        //            Active = false,
        //            CompanyId = SchoolId,
        //            SessionId = SessionId,
        //            NetDueFees = req.Duefee,
        //            PaymentMode = "UPI",
        //            PaymentDate = req.PaymentDate,
        //            Remark = req.Remark,
        //            RTS = DateTime.Now,
        //        };
        //        _context.M_FeeDetail.Add(StuInstall);
        //        await _context.SaveChangesAsync();

        //        return ApiResponse<bool>.SuccessResponse(true, "Student installment fee saved successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
        //    }
        //}


        // public async Task<ApiResponse<bool>> UpdateStudentPaymentSuccessfully(int StudentId, int ReceiptId)
        //{
        //    try
        //    {
        //        int CompanyId = _loginUser.SchoolId;
        //        int UserId = _loginUser.UserId;
        //        int SessionId = _loginUser.SessionId;

        //        // =========================================== GET FEE RECORD

        //        var fee = await _context.M_FeeDetail
        //            .FirstOrDefaultAsync(p => p.FDId == ReceiptId && p.SessionId == SessionId);

        //        if (fee == null)
        //            return ApiResponse<bool>.ErrorResponse("Invalid request!");

        //        // =========================================== GET TOKEN FROM COOKIE

        //        string token = _httpContextAccessor.HttpContext.Request.Cookies["paytoken"];

        //        if (string.IsNullOrEmpty(token))
        //            return ApiResponse<bool>.ErrorResponse("Token missing!");

        //        // =========================================== PREPARE STATUS CHECK URL

        //        string merchantOrderId = fee.OrderNo;
        //        string url = $"https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/order/{merchantOrderId}/status?details=false";

        //        // =========================================== CALL PHONEPE STATUS API (Using HttpClient)

        //        using var client = new HttpClient();

        //        client.DefaultRequestHeaders.Clear();
        //        client.DefaultRequestHeaders.Add("Authorization", "O-Bearer " + token);

        //        var response = await client.GetAsync(url);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return ApiResponse<bool>.ErrorResponse("Failed to check status!");
        //        }

        //        string result = await response.Content.ReadAsStringAsync();

        //        dynamic json = JsonConvert.DeserializeObject(result);

        //        string status = json.state; // COMPLETED / FAILED / PENDING
        //        var payment = json.paymentDetails?[0];

        //        string paymentState = payment?.state;
        //        string txnId = payment?.transactionId;

        //        // =========================================== UPDATE DATABASE
        //        if (status == "COMPLETED")
        //        {
        //            // Update M_FeeDetail
        //            fee.OrderStatus = "Success";
        //            fee.Status = "Success";
        //            fee.Active = true;
        //            fee.TransactionId = txnId ?? "";
        //            await _context.SaveChangesAsync();

        //            // Update Student Renew table
        //            var studentrenewtbl = await _context.Student_Renew.FirstOrDefaultAsync(s => s.StuId == fee.stu_id && s.CompanyId == CompanyId &&
        //            s.SessionId == SessionId && s.ClassId == fee.ClassId);

        //            if (studentrenewtbl != null)
        //            {
        //                studentrenewtbl.due_fee -= fee.PayFees;
        //                studentrenewtbl.stu_fee += fee.PayFees;
        //                await _context.SaveChangesAsync();
        //            }

        //            // Installments Adjustment
        //            var installments = await _context.fee_installment.Where(u => u.stu_id == fee.stu_id && u.university_id == fee.ClassId && u.CompanyId == CompanyId &&
        //                    u.SessionId == SessionId).ToListAsync();

        //            double remaining = Convert.ToDouble(fee.PayFees);

        //            foreach (var insta in installments)
        //            {
        //                if (remaining <= 0) break;

        //                if (insta.due_fee > 0)
        //                {
        //                    if (remaining >= insta.due_fee)
        //                    {
        //                        remaining -= (double)insta.due_fee;
        //                        insta.due_fee = 0;
        //                    }
        //                    else
        //                    {
        //                        insta.due_fee -= remaining;
        //                        remaining = 0;
        //                    }

        //                    await _context.SaveChangesAsync();
        //                }
        //            }

        //            return ApiResponse<bool>.SuccessResponse(true, "Payment successful");
        //        }
        //        else if (status == "FAILED")
        //        {
        //            fee.OrderStatus = "Failed";
        //            fee.Status = "Failed";
        //            fee.Active = false;

        //            await _context.SaveChangesAsync();
        //            return ApiResponse<bool>.SuccessResponse(false, "Payment failed");
        //        }
        //        else
        //        {
        //            fee.OrderStatus = "Pending";
        //            fee.Status = "Pending";
        //            fee.Active = false;
        //            await _context.SaveChangesAsync();

        //            return ApiResponse<bool>.SuccessResponse(false, "Payment pending");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
        //    }
        //}


        //public async Task<ApiResponse<bool>> UpdateStudentPaymentSuccessfully(int StudentId, int ReceiptId)
        //{
        //    try
        //    {
        //        int SchoolId = _loginUser.SchoolId;
        //        int UserId = _loginUser.UserId;
        //        int SessionId = _loginUser.SessionId;


        //        // ===================================
        //        // GET FEE RECORD BY RECEIPT ID
        //        // ===================================
        //        var fee = db.M_FeeDetail.FirstOrDefault(p => p.FDId == Receiptid && p.SessionId == SessionId);

        //        if (fee == null)
        //            return Content("Invalid Request!");

        //        // ===================================
        //        // PHONEPE STATUS CHECK URL
        //        // ===================================
        //        string merchantOrderId = fee.OrderNo;
        //        string url = $"https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/order/{merchantOrderId}/status?details=false";

        //        // ===================================
        //        // GET TOKEN FROM COOKIE
        //        // ===================================
        //        string token = Request.Cookies["paytoken"]?.Value;

        //        if (string.IsNullOrEmpty(token))
        //            return Content("Token Missing!");

        //        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        //        req.Method = "GET";
        //        req.ContentType = "application/json";
        //        req.Headers.Add("Authorization", "O-Bearer " + token);

        //        // ===================================
        //        // GET RESPONSE
        //        // ===================================
        //        string result;
        //        using (var response = (HttpWebResponse)req.GetResponse())
        //        using (var reader = new StreamReader(response.GetResponseStream()))
        //        {
        //            result = reader.ReadToEnd();
        //        }

        //        dynamic json = JsonConvert.DeserializeObject(result);

        //        // =============================================
        //        // CORRECTED: PHONEPE RESPONSE PARSING
        //        // =============================================
        //        string status = json.state; // "COMPLETED", "PENDING", "FAILED"
        //                                    // PAYMENT DETAILS (FIRST ENTRY)
        //        var payment = json.paymentDetails?[0];

        //        // PAYMENT STATE
        //        string paymentState = payment?.state;   // COMPLETED / FAILED

        //        // TRANSACTION ID
        //        string txnId = payment?.transactionId;

        //        // =============================================
        //        // UPDATE DB BASED ON PAYMENT STATUS
        //        // =============================================
        //        if (status == "COMPLETED")
        //        {
        //            fee.OrderStatus = "Success";
        //            fee.Status = "Success";
        //            fee.Active = true;
        //            fee.TransactionId = txnId ?? "";
        //            db.SaveChanges();

        //            var studentrenewtbl = db.Student_Renew.Where(s => s.StuId == fee.stu_id && s.CompanyId == CompanyId && s.SessionId == SessionId && s.ClassId == fee.ClassId).FirstOrDefault();

        //            studentrenewtbl.due_fee = studentrenewtbl.due_fee - fee.PayFees;
        //            studentrenewtbl.stu_fee += fee.PayFees;

        //            db.SaveChanges();

        //            var installments = db.fee_installment.Where(u => u.stu_id == fee.stu_id && u.university_id == fee.ClassId && u.CompanyId == CompanyId && u.SessionId == SessionId).Select(a => new
        //            {
        //                a.university_id,
        //                a.IntallmentID,
        //                a.total_fee,
        //                a.Installment,
        //                a.FAmount,

        //            }).ToList();

        //            double subfee = 0;
        //            double subfee2 = Convert.ToDouble(fee.PayFees);

        //            for (int i = 0; i < installments.Count && fee.PayFees > 0; i++)
        //            {
        //                int? installmentid = installments[i].IntallmentID;

        //                fee_installment insta = db.fee_installment.FirstOrDefault(f => f.stu_id == fee.stu_id && f.university_id == fee.ClassId && f.CompanyId == CompanyId && f.SessionId == SessionId && f.IntallmentID == installmentid);

        //                if (insta != null)
        //                {
        //                    if (insta.due_fee != 0)
        //                    {

        //                        if (subfee2 >= insta.due_fee)
        //                        {
        //                            subfee2 = Convert.ToDouble(subfee2) - Convert.ToDouble(insta.due_fee);
        //                            insta.due_fee = 0;
        //                            db.SaveChanges();
        //                        }
        //                        else if (subfee2 != 0)
        //                        {
        //                            insta.due_fee = insta.due_fee - subfee2;
        //                            subfee2 = 0;
        //                            db.SaveChanges();
        //                            break;

        //                        }
        //                        else
        //                        {
        //                            insta.due_fee = insta.due_fee - subfee2;
        //                            subfee2 = 0;
        //                            db.SaveChanges();
        //                            break;

        //                        }
        //                    }
        //                }
        //            }

        //            ViewBag.msg = "Payment Successful";
        //        }
        //        else if (status == "FAILED")
        //        {
        //            fee.OrderStatus = "Failed";
        //            fee.Status = "Failed";
        //            fee.Active = false;
        //            db.SaveChanges();

        //            ViewBag.msg = "Payment Failed";
        //        }
        //        else
        //        {
        //            fee.OrderStatus = "Pending";
        //            fee.Status = "Pending";
        //            fee.Active = false;
        //            db.SaveChanges();

        //            ViewBag.msg = "Payment Pending";
        //        }


        //        return ApiResponse<bool>.SuccessResponse(true, "Student installment fee data fetched successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiResponse<bool>.ErrorResponse("Error : " + ex.Message);
        //    }
        //}

        //var fee = _context.M_FeeDetail.FirstOrDefault(a => a.FDId == ReceiptId && a.SessionId == SessionId && a.CompanyId == SchoolId);

        //fee.OrderStatus = "Success";
        //fee.Status = "Success";
        //fee.Active = true;
        //await _context.SaveChangesAsync();

        //var StudentInstall = _context.Student_Renew.Where(c => c.StuId == fee.stu_id && c.CompanyId == SchoolId && c.SessionId == SessionId && c.ClassId == fee.ClassId).FirstOrDefault();
        //StudentInstall.due_fee = StudentInstall.due_fee - fee.PayFees;
        //StudentInstall.stu_fee += fee.PayFees;
        //await _context.SaveChangesAsync();

        //var Installment = _context.fee_installment.Where(c => c.stu_id == fee.stu_id && c.university_id == fee.ClassId && c.CompanyId == SchoolId && c.SessionId == SessionId)
        //    .Select(c => new GetStuInstallmentModel
        //    {
        //        university_id = c.university_id,
        //        IntallmentID = c.IntallmentID,
        //        total_fee = c.total_fee,
        //        Installment = c.Installment,
        //        FAmount = c.FAmount,
        //    }).ToList();

        //double subfee = 0;
        //double subfee2 = Convert.ToDouble(fee.PayFees);

        //for (int i = 0; i < Installment.Count && fee.PayFees > 0; i++)
        //{
        //    int? installmentid = Installment[i].IntallmentID;

        //    fee_installment insta = _context.fee_installment.FirstOrDefault(f => f.stu_id == fee.stu_id && f.university_id == fee.ClassId && f.CompanyId == SchoolId && f.SessionId == SessionId && f.IntallmentID == installmentid);

        //    if (insta != null)
        //    {
        //        if (insta.due_fee != 0)
        //        {

        //            if (subfee2 >= insta.due_fee)
        //            {
        //                subfee2 = Convert.ToDouble(subfee2) - Convert.ToDouble(insta.due_fee);
        //                insta.due_fee = 0;
        //                await _context.SaveChangesAsync();
        //            }
        //            else if (subfee2 != 0)
        //            {
        //                insta.due_fee = insta.due_fee - subfee2;
        //                subfee2 = 0;
        //                await _context.SaveChangesAsync();
        //                break;

        //            }
        //            else
        //            {
        //                insta.due_fee = insta.due_fee - subfee2;
        //                subfee2 = 0;
        //                await _context.SaveChangesAsync();
        //                break;

        //            }
        //        }
        //    }
        //}


        //var res = new StudentClassExamData
        //{
        //    ExamDatas = ExamData,
        //    // SectionDatas = SectionData,
        //};


        // ================================ student payment gateway code ========================== // 

        //private static string _phonePeToken = "";
        //private static DateTime _tokenExpiry = DateTime.MinValue;

        // ================== PHONEPE TOKEN GENERATE

        //public string GetNewAccessToken()
        //{
        //    // string authUrl = "https://api.phonepe.com/apis/identity-manager/v1/oauth/token";
        //    string authUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox/v1/oauth/token";

        //    string client_id = "SU2602051921478872343710";
        //    string client_version = "1";
        //    string client_secret = "541e3c3f-0fc7-4e42-a141-28922b4eba32";
        //    string grant_type = "client_credentials";

        //    string postData =
        //        $"client_id={client_id}&client_version={client_version}&client_secret={client_secret}&grant_type={grant_type}";
        //    byte[] data = Encoding.UTF8.GetBytes(postData);
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(authUrl);
        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded";

        //    using (var stream = request.GetRequestStream())
        //        stream.Write(data, 0, data.Length);

        //    string result;

        //    using (var response = (HttpWebResponse)request.GetResponse())
        //    using (var reader = new StreamReader(response.GetResponseStream()))
        //        result = reader.ReadToEnd();

        //    dynamic json = JsonConvert.DeserializeObject(result);

        //    string token = json.access_token;
        //    int expiresIn = json.expires_in;

        //    // 🔐 server memory me save
        //    _phonePeToken = token;
        //    _tokenExpiry = DateTime.Now.AddSeconds(expiresIn - 300); // 5 min pehle refresh

        //    return token;
        //}

        //public string GetValidToken()
        //{
        //    if (string.IsNullOrEmpty(_phonePeToken) || DateTime.Now >= _tokenExpiry)
        //    {
        //        return GetNewAccessToken();
        //    }

        //    return _phonePeToken;
        //}

        //public async Task<ApiResponse<StudentFeePaymentResult>> AddStudentInstallmentFee(AddStudentinstallReq req)
        //{
        //    using (var transaction = await _context.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            int SchoolId = _loginUser.SchoolId;
        //            int SessionId = _loginUser.SessionId;

        //            // ===========================================  CHECK — Student has no pending due already
        //            var student = await _context.Student_Renew.FirstOrDefaultAsync(a => a.ClassId == req.ClassId && a.StuId == req.StudentId && a.due_fee != 0);

        //            if (student == null)
        //            {
        //                return ApiResponse<StudentFeePaymentResult>.ErrorResponse("Due fee already available");
        //            }

        //            // =========================================== INSTITUTE CODE & RECEIPT NUMBER
        //            var GetInstituteCodeName = await _context.institute.FirstOrDefaultAsync(i => i.institute_id == SchoolId);

        //            var LastCode = await _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).OrderByDescending(s => s.FDId).FirstOrDefaultAsync();

        //            string instCode = GetInstituteCodeName.instituteCode.Substring(0, 3).ToUpper();
        //            int newId = (LastCode != null)
        //                        ? int.Parse(LastCode.ReceiptNo.Split('/')[1]) + 1
        //                        : 1;

        //            string ReceiptCode = $"{instCode}/{newId}";


        //            // =========================================== GENERATE ORDER NUMBER
        //            int NewOrderNo = 1;
        //            var LastOrderNo = await _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).OrderByDescending(s => s.OrderNo)
        //                .Select(s => s.OrderNo).FirstOrDefaultAsync();

        //            if (!string.IsNullOrEmpty(LastOrderNo))
        //            {
        //                if (int.TryParse(LastOrderNo, out int last))
        //                    NewOrderNo = last + 1;
        //            }

        //            // =========================================== GENERATE PRIMARY KEY FDId
        //            int FDId = _context.M_FeeDetail.DefaultIfEmpty().Max(s => s == null ? 0 : s.FDId) + 1;

        //            // =========================================== SAVE INSTALLMENT RECORD
        //            var fee = new M_FeeDetail
        //            {
        //                FDId = FDId,
        //                stu_id = req.StudentId,
        //                ClassId = req.ClassId,
        //                OrderNo = NewOrderNo.ToString(),
        //                OrderStatus = "Pending",
        //                TransactionId = "",
        //                ReceiptType = "Online",
        //                DueFees = student.due_fee - req.PaidFee,
        //                PayFees = req.PaidFee,
        //                Cash = 0,
        //                Upi = 0,
        //                AdmissionPayfee = 0,
        //                AFeeDiscount = 0,
        //                PramoteFees = 0,
        //                Date = DateTime.Now,
        //                Status = "InstallmentFee",
        //                Active = false,
        //                CompanyId = SchoolId,
        //                SessionId = SessionId,
        //                NetDueFees = student.due_fee - req.PaidFee,
        //                PaymentMode = "UPI",
        //                PaymentDate = DateTime.UtcNow,
        //                Remark = req.Remark,
        //                RTS = DateTime.Now,
        //                ReceiptNo = ReceiptCode
        //            };

        //            _context.M_FeeDetail.Add(fee);
        //            await _context.SaveChangesAsync();


        //            string token = GetValidToken();
        //            if (string.IsNullOrEmpty(token))
        //            {
        //                await transaction.RollbackAsync();
        //                return ApiResponse<StudentFeePaymentResult>.ErrorResponse("Failed to fetch authorization token");
        //            }


        //            // =========================================== STEP 2 → PAYMENT REQUEST
        //            //   string apiUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/sdk/order";

        //            string apiUrl = "https://api.phonepe.com/apis/pg/checkout/v2/pay";

        //            //  string apiUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/pay";

        //            double amount = Convert.ToDouble(req.PaidFee);

        //            var paymentRequest = new
        //            {
        //                merchantOrderId = NewOrderNo.ToString(),
        //                amount = (int)(amount * 100),
        //                expireAfter = 300,


        //                metaInfo = new
        //                {
        //                    udf1 = "StudentFee",
        //                    udf2 = req.StudentId.ToString(),
        //                    udf3 = req.ClassId.ToString(),
        //                    udf4 = ReceiptCode,
        //                    udf5 = "InstituteFee"
        //                },

        //                paymentFlow = new
        //                {
        //                    type = "PG_CHECKOUT",
        //                    message = "Fee payment",
        //                    //merchantUrls = new
        //                    //{
        //                    //    redirectUrl = Url.Action("PaymentSuccessfully", "Home",
        //                    //        new { StudentId = req.StudentId, Receiptid = fee.FDId },
        //                    //        Request.Url.Scheme)
        //                    //}
        //                }
        //            };

        //            string jsonBody = JsonConvert.SerializeObject(paymentRequest);
        //            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonBody);

        //            HttpWebRequest payRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
        //            payRequest.Method = "POST";
        //            payRequest.ContentType = "application/json";
        //            payRequest.Headers.Add("Authorization", "O-Bearer " + token);

        //            using (var stream = payRequest.GetRequestStream())
        //                stream.Write(jsonBytes, 0, jsonBytes.Length);

        //            string paymentResponseStr;
        //            using (var response = (HttpWebResponse)payRequest.GetResponse())
        //            using (var reader = new StreamReader(response.GetResponseStream()))
        //                paymentResponseStr = reader.ReadToEnd();

        //            dynamic paymentResponse = JsonConvert.DeserializeObject(paymentResponseStr);
        //            string redirectUrl = paymentResponse.redirectUrl;

        //            // =================         PENDING CHECK LOOP
        //            BackgroundJob.Schedule(
        //             () => CheckPaymentStatusBackground(NewOrderNo.ToString()),
        //             TimeSpan.FromSeconds(10)
        //             );

        //            await transaction.CommitAsync();
        //            return ApiResponse<StudentFeePaymentResult>.SuccessResponse(
        //                new StudentFeePaymentResult
        //                {
        //                    FDId = fee.FDId,
        //                    ReceiptNo = ReceiptCode,
        //                    merchantOrderId = fee.OrderNo,
        //                    OrderId = paymentResponse.orderId,
        //                    State = paymentResponse.state,
        //                    ExpireAt = paymentResponse.expireAt,
        //                    Token = paymentResponse.token
        //                },
        //                "Installment fee saved successfully"
        //                );
        //        }
        //        catch (Exception ex)
        //        {
        //            await transaction.RollbackAsync();
        //            return ApiResponse<StudentFeePaymentResult>.ErrorResponse("Error: " + ex.Message);
        //        }
        //    }
        //}
        // =========================================== STEP 1 → GET OAUTH TOKEN FROM PHONEPE

        //string authUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox/v1/oauth/token";

        //string client_id = "SYSTEM_USER_PR2511101303561945193890";
        //string client_version = "1";
        //string client_secret = "b0d19337-30c5-45fc-b028-8bc414c6237a";
        //string grant_type = "client_credentials";


        //string client_id = "SHYAWAYUAT_2510101108212";
        //string client_version = "1";
        //string client_secret = "NWQ2YzJlZDktODU3Yi00ZWUzLTk1MTItOTJhZDVkYjkxYmYx";

        //string client_id = "TSPVIRTUALUAT_2512051124";
        //string client_version = "1";
        //string client_secret = "ODcxMGVlYTgtNjNkNy00Y2Q2LWE5ODctZGRiMDQ4YzYyNWM2";

        //string postData = $"client_id={client_id}&client_version={client_version}&client_secret={client_secret}&grant_type=client_credentials";

        //byte[] authBytes = Encoding.UTF8.GetBytes(postData);

        //HttpWebRequest authRequest = (HttpWebRequest)WebRequest.Create(authUrl);
        //authRequest.Method = "POST";
        //authRequest.ContentType = "application/x-www-form-urlencoded";
        //authRequest.ContentLength = authBytes.Length;

        //using (var stream = authRequest.GetRequestStream())
        //    stream.Write(authBytes, 0, authBytes.Length);

        //string authResponseStr;
        //using (var response = (HttpWebResponse)authRequest.GetResponse())
        //using (var reader = new StreamReader(response.GetResponseStream()))
        //    authResponseStr = reader.ReadToEnd();

        //dynamic authResponse = JsonConvert.DeserializeObject(authResponseStr);
        //string token = authResponse.access_token;

        //private string _phonePeToken;
        //private DateTime _tokenExpiry;

        //public async Task<string> GetNewAccessToken()
        //{
        //    //// string authUrl = "https://api.phonepe.com/apis/identity-manager/v1/oauth/token";
        //    //string authUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox/v1/oauth/token";

        //    //using (var client = new HttpClient())
        //    //{
        //    //    var content = new FormUrlEncodedContent(new[]
        //    //    {
        //    //        new KeyValuePair<string, string>("client_id", "SU2602051921478872343710"),
        //    //        new KeyValuePair<string, string>("client_version", "1"),
        //    //        new KeyValuePair<string, string>("client_secret", "541e3c3f-0fc7-4e42-a141-28922b4eba32"),
        //    //        new KeyValuePair<string, string>("grant_type", "client_credentials")
        //    //    });

        //    //    var response = await client.PostAsync(authUrl, content);

        //    //    if (!response.IsSuccessStatusCode)
        //    //        return null;

        //    //    var result = await response.Content.ReadAsStringAsync();
        //    //    dynamic json = JsonConvert.DeserializeObject(result);

        //    //    _phonePeToken = json.access_token;
        //    //    _tokenExpiry = DateTime.Now.AddSeconds((int)json.expires_in - 300);

        //    //    return _phonePeToken;
        //    //}


        //    // string authUrl = "https://api.phonepe.com/apis/identity-manager/v1/oauth/token";
        //    string authUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox/v1/oauth/token";


        //    string client_id = "SU2602051921478872343710";
        //    string client_version = "1";
        //    string client_secret = "541e3c3f-0fc7-4e42-a141-28922b4eba32";
        //    string grant_type = "client_credentials";

        //    string postData =
        //        $"client_id={client_id}&client_version={client_version}&client_secret={client_secret}&grant_type={grant_type}";
        //    byte[] data = Encoding.UTF8.GetBytes(postData);
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(authUrl);
        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded";

        //    using (var stream = request.GetRequestStream())
        //        stream.Write(data, 0, data.Length);

        //    string result;

        //    using (var response = (HttpWebResponse)request.GetResponse())
        //    using (var reader = new StreamReader(response.GetResponseStream()))
        //        result = reader.ReadToEnd();

        //    dynamic json = JsonConvert.DeserializeObject(result);

        //    string token = json.access_token;
        //    int expiresIn = json.expires_in;

        //    // 🔐 server memory me save
        //    _phonePeToken = token;
        //    _tokenExpiry = DateTime.Now.AddSeconds(expiresIn - 300); // 5 min pehle refresh

        //    return token;
        //}

        //public async Task<string> GetValidToken()
        //{
        //    if (string.IsNullOrEmpty(_phonePeToken) || DateTime.Now >= _tokenExpiry)
        //    {
        //        return await GetNewAccessToken();
        //    }

        //    return _phonePeToken;
        //}



        //public async Task<ApiResponse<bool>> UpdateStudentPaymentSuccessfully(int StudentId, int ReceiptId, string orderno)
        //{
        //    try
        //    {
        //        int companyId = _loginUser.SchoolId;
        //        int UserId = _loginUser.UserId;
        //        int sessionId = _loginUser.SessionId;

        //        // ===================== GET FEE RECORD
        //        var fee = await _context.M_FeeDetail.FirstOrDefaultAsync(f => f.FDId == ReceiptId && f.SessionId == sessionId && f.OrderNo == orderno);

        //        if (fee == null)
        //            return ApiResponse<bool>.ErrorResponse("Fee record not found");

        //        // =========================================== STEP 1 → GET OAUTH TOKEN FROM PHONEPE

        //        string authUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox/v1/oauth/token";


        //        //      clientId: TSPVIRTUALUAT_2512051124
        //        //      clientVersion: 1
        //        //      clientSecret: ODcxMGVlYTgtNjNkNy00Y2Q2LWE5ODctZGRiMDQ4YzYyNWM2
        //        //      UAT credentials(End Merchant's TEST MID):
        //        //      MID - TSPVIRTUALUAT


        //        string client_id = "SU2602051921478872343710";
        //        string client_version = "1";
        //        string client_secret = "541e3c3f-0fc7-4e42-a141-28922b4eba32";
        //        string grant_type = "client_credentials";

        //        //string client_id = "TSPVIRTUALUAT_2512051124";
        //        //string client_version = "1";
        //        //string client_secret = "ODcxMGVlYTgtNjNkNy00Y2Q2LWE5ODctZGRiMDQ4YzYyNWM2";
        //        //  string grant_type = "client_credentials";

        //        string postData = $"client_id={client_id}&client_version={client_version}&client_secret={client_secret}&grant_type=client_credentials";

        //        byte[] authBytes = Encoding.UTF8.GetBytes(postData);

        //        HttpWebRequest authRequest = (HttpWebRequest)WebRequest.Create(authUrl);
        //        authRequest.Method = "POST";
        //        authRequest.ContentType = "application/x-www-form-urlencoded";
        //        authRequest.ContentLength = authBytes.Length;

        //        using (var stream = authRequest.GetRequestStream())
        //            stream.Write(authBytes, 0, authBytes.Length);

        //        string authResponseStr;
        //        using (var responses = (HttpWebResponse)authRequest.GetResponse())
        //        using (var reader = new StreamReader(responses.GetResponseStream()))
        //            authResponseStr = reader.ReadToEnd();

        //        dynamic authResponse = JsonConvert.DeserializeObject(authResponseStr);
        //        string token = authResponse.access_token;

        //        if (string.IsNullOrEmpty(token))
        //        {
        //            await _context.SaveChangesAsync();
        //            return ApiResponse<bool>.ErrorResponse("Failed to fetch authorization token");
        //        }

        //        // ===================== PHONEPE STATUS API
        //        string merchantOrderId = fee.OrderNo;

        //        //  string url = $"https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/order/{merchantOrderId}/status?details=true";

        //        string url = $"https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/order/{merchantOrderId}/status?details=false";

        //        using var client = new HttpClient();
        //        client.DefaultRequestHeaders.Clear();
        //        //  client.DefaultRequestHeaders.Add("Authorization", "O-Bearer " + token);
        //        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

        //        var response = await client.GetAsync(url);
        //        if (!response.IsSuccessStatusCode)
        //            return ApiResponse<bool>.ErrorResponse("Unable to fetch payment status");

        //        string result = await response.Content.ReadAsStringAsync();
        //        dynamic json = JsonConvert.DeserializeObject(result);

        //        string orderState = json?.state;

        //        var payment = (json?.paymentDetails != null && json.paymentDetails.Count > 0)
        //            ? json.paymentDetails[0]
        //            : null;

        //        string txnId = payment?.transactionId ?? "";

        //        // ===================== PAYMENT STATUS HANDLING
        //        if (orderState == "COMPLETED")
        //        {
        //            // -------- Update Fee Detail
        //            fee.OrderStatus = "Success";
        //            fee.Status = "Success";
        //            fee.Active = true;
        //            fee.TransactionId = txnId;

        //            await _context.SaveChangesAsync();

        //            // -------- Update Student Renew
        //            var renew = await _context.Student_Renew.FirstOrDefaultAsync(r => r.StuId == fee.stu_id && r.ClassId == fee.ClassId && r.CompanyId == companyId &&
        //                r.SessionId == sessionId);

        //            if (renew != null)
        //            {
        //                renew.due_fee -= fee.PayFees;
        //                renew.stu_fee += fee.PayFees;
        //                await _context.SaveChangesAsync();
        //            }

        //            // -------- INSTALLMENT ADJUSTMENT (FOR LOOP)
        //            var installments = await _context.fee_installment.Where(u => u.stu_id == fee.stu_id && u.university_id == fee.ClassId && u.CompanyId == companyId &&
        //                             u.SessionId == sessionId).ToListAsync();

        //            double remaining = Convert.ToDouble(fee.PayFees);

        //            for (int i = 0; i < installments.Count; i++)
        //            {
        //                if (remaining <= 0)
        //                    break;

        //                if (installments[i].due_fee > 0)
        //                {
        //                    if (remaining >= installments[i].due_fee)
        //                    {
        //                        remaining -= (double)installments[i].due_fee;
        //                        installments[i].due_fee = 0;
        //                    }
        //                    else
        //                    {
        //                        installments[i].due_fee -= remaining;
        //                        remaining = 0;
        //                    }
        //                }
        //            }

        //            await _context.SaveChangesAsync();

        //            return ApiResponse<bool>.SuccessResponse(true, "Payment successful");
        //        }
        //        else if (orderState == "FAILED")
        //        {
        //            fee.OrderStatus = "Failed";
        //            fee.Status = "Failed";
        //            fee.Active = false;

        //            await _context.SaveChangesAsync();
        //            return ApiResponse<bool>.SuccessResponse(false, "Payment failed");
        //        }
        //        else
        //        {
        //            fee.OrderStatus = "Pending";
        //            fee.Status = "Pending";
        //            fee.Active = false;

        //            await _context.SaveChangesAsync();
        //            return ApiResponse<bool>.SuccessResponse(false, "Payment pending");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiResponse<bool>.ErrorResponse("Exception: " + ex.Message);
        //    }
        //}


        //public async Task<ApiResponse<StudentFeePaymentResult>> AddStudentInstallmentFee(AddStudentinstallReq req)
        //{
        //    using (var transaction = await _context.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            // ================= TOKEN =================
        //            string token = await GetValidToken();

        //            if (string.IsNullOrEmpty(token))
        //            {
        //                await transaction.RollbackAsync();
        //                return ApiResponse<StudentFeePaymentResult>.ErrorResponse("Token failed");
        //            }

        //            // ✅ ALWAYS SAME ENVIRONMENT
        //            string apiUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/pay";

        //            double amount = Convert.ToDouble(req.PaidFee);

        //            var paymentRequest = new
        //            {
        //                merchantOrderId = Guid.NewGuid().ToString(),
        //                amount = (int)(amount * 100),
        //                expireAfter = 300,
        //                metaInfo = new
        //                {
        //                    udf1 = "StudentFee",
        //                    udf2 = req.StudentId.ToString(),
        //                    udf3 = req.ClassId.ToString(),
        //                    udf4 = "RCPT001",
        //                    udf5 = "InstituteFee"
        //                },
        //                paymentFlow = new
        //                {
        //                    type = "PG_CHECKOUT",
        //                    message = "Fee payment"
        //                }
        //            };

        //            using (var client = new HttpClient())
        //            {
        //                // ✅ FIXED HEADER
        //                client.DefaultRequestHeaders.Authorization =
        //                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        //                var content = new StringContent(
        //                    JsonConvert.SerializeObject(paymentRequest),
        //                    Encoding.UTF8,
        //                    "application/json"
        //                );

        //                var response = await client.PostAsync(apiUrl, content);
        //                var result = await response.Content.ReadAsStringAsync();

        //                if (!response.IsSuccessStatusCode)
        //                {
        //                    await transaction.RollbackAsync();
        //                    return ApiResponse<StudentFeePaymentResult>.ErrorResponse(result);
        //                }

        //                dynamic data = JsonConvert.DeserializeObject(result);

        //                await transaction.CommitAsync();

        //                return ApiResponse<StudentFeePaymentResult>.SuccessResponse(
        //                    new StudentFeePaymentResult
        //                    {
        //                        OrderId = data.orderId,
        //                        State = data.state,
        //                        Token = data.token
        //                    },
        //                    "Payment Success"
        //                );
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            await transaction.RollbackAsync();
        //            return ApiResponse<StudentFeePaymentResult>.ErrorResponse(ex.Message);
        //        }
        //    }
        //}



        //public async Task<ApiResponse<StudentFeePaymentResult>> AddStudentInstallmentFee(AddStudentinstallReq req)
        //{
        //    using (var transaction = await _context.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            int SchoolId = _loginUser.SchoolId;
        //            int SessionId = _loginUser.SessionId;

        //            // ✅ CHECK DUE
        //            var student = await _context.Student_Renew
        //                .FirstOrDefaultAsync(a => a.ClassId == req.ClassId && a.StuId == req.StudentId && a.due_fee > 0);

        //            if (student == null)
        //            {
        //                return ApiResponse<StudentFeePaymentResult>.ErrorResponse("No due fee found");
        //            }

        //            //   =========================================== INSTITUTE CODE & RECEIPT NUMBER
        //            var GetInstituteCodeName = await _context.institute.FirstOrDefaultAsync(i => i.institute_id == SchoolId);

        //            var LastCode = await _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).OrderByDescending(s => s.FDId).FirstOrDefaultAsync();

        //            string instCode = GetInstituteCodeName.instituteCode.Substring(0, 3).ToUpper();
        //            int newId = (LastCode != null)
        //                        ? int.Parse(LastCode.ReceiptNo.Split('/')[1]) + 1
        //                        : 1;

        //            string ReceiptCode = $"{instCode}/{newId}";


        //            // =========================================== GENERATE ORDER NUMBER
        //            int NewOrderNo = 1;
        //            var LastOrderNo = await _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).OrderByDescending(s => s.OrderNo)
        //                .Select(s => s.OrderNo).FirstOrDefaultAsync();

        //            if (!string.IsNullOrEmpty(LastOrderNo))
        //            {
        //                if (int.TryParse(LastOrderNo, out int last))
        //                    NewOrderNo = last + 1;
        //            }

        //            // =========================================== GENERATE PRIMARY KEY FDId
        //            int FDId = _context.M_FeeDetail.DefaultIfEmpty().Max(s => s == null ? 0 : s.FDId) + 1;

        //            // =========================================== SAVE INSTALLMENT RECORD
        //            var fee = new M_FeeDetail
        //            {
        //                FDId = FDId,
        //                stu_id = req.StudentId,
        //                ClassId = req.ClassId,
        //                OrderNo = NewOrderNo.ToString(),
        //                OrderStatus = "Pending",
        //                TransactionId = "",
        //                ReceiptType = "Online",
        //                DueFees = student.due_fee - req.PaidFee,
        //                PayFees = req.PaidFee,
        //                Cash = 0,
        //                Upi = 0,
        //                AdmissionPayfee = 0,
        //                AFeeDiscount = 0,
        //                PramoteFees = 0,
        //                Date = DateTime.Now,
        //                Status = "InstallmentFee",
        //                Active = false,
        //                CompanyId = SchoolId,
        //                SessionId = SessionId,
        //                NetDueFees = student.due_fee - req.PaidFee,
        //                PaymentMode = "UPI",
        //                PaymentDate = DateTime.UtcNow,
        //                Remark = req.Remark,
        //                RTS = DateTime.Now,
        //                ReceiptNo = ReceiptCode
        //            };

        //            _context.M_FeeDetail.Add(fee);
        //            await _context.SaveChangesAsync();


        //            // ✅ TOKEN
        //            string token = await GetValidToken();

        //            if (string.IsNullOrEmpty(token))
        //            {
        //                await transaction.RollbackAsync();
        //                return ApiResponse<StudentFeePaymentResult>.ErrorResponse("Token failed");
        //            }

        //            // ✅ PAYMENT API (SANDBOX)
        //            string apiUrl = "https://api.phonepe.com/apis/pg/checkout/v2/pay";

        //            //string apiUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/pay";

        //            double amount = Convert.ToDouble(req.PaidFee);

        //            var paymentRequest = new
        //            {
        //                merchantOrderId = NewOrderNo.ToString(),
        //                amount = (int)(amount * 100),
        //                expireAfter = 300,
        //                metaInfo = new
        //                {
        //                    udf1 = "StudentFee",
        //                    udf2 = req.StudentId.ToString(),
        //                    udf3 = req.ClassId.ToString(),
        //                    udf4 = ReceiptCode,
        //                    udf5 = "InstituteFee"
        //                },
        //                paymentFlow = new
        //                {
        //                    type = "PG_CHECKOUT",
        //                    message = "Fee payment"
        //                }
        //            };

        //            string jsonBody = JsonConvert.SerializeObject(paymentRequest);

        //            using (var client = new HttpClient())
        //            {
        //                client.DefaultRequestHeaders.Add("Authorization", "O-Bearer " + token);

        //                var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        //                var response = await client.PostAsync(apiUrl, httpContent);

        //                string responseStr = await response.Content.ReadAsStringAsync();

        //                if (!response.IsSuccessStatusCode)
        //                {
        //                    await transaction.RollbackAsync();
        //                    return ApiResponse<StudentFeePaymentResult>.ErrorResponse("Payment API Error: " + responseStr);
        //                }

        //                dynamic paymentResponse = JsonConvert.DeserializeObject(responseStr);

        //                if (paymentResponse == null || paymentResponse.orderId == null)
        //                {
        //                    await transaction.RollbackAsync();
        //                    return ApiResponse<StudentFeePaymentResult>.ErrorResponse("Invalid Payment Response");
        //                }

        //                await transaction.CommitAsync();

        //                return ApiResponse<StudentFeePaymentResult>.SuccessResponse(
        //                    new StudentFeePaymentResult
        //                    {
        //                        FDId = fee.FDId,
        //                        ReceiptNo = ReceiptCode,
        //                        merchantOrderId = fee.OrderNo,
        //                        OrderId = paymentResponse.orderId,
        //                        State = paymentResponse.state,
        //                        ExpireAt = paymentResponse.expireAt,
        //                        Token = paymentResponse.token
        //                    },
        //                    "Payment initiated successfully"
        //                );
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            await transaction.RollbackAsync();
        //            return ApiResponse<StudentFeePaymentResult>.ErrorResponse("Error: " + ex.Message);
        //        }
        //    }
        //}




        //public async Task CheckPaymentStatusBackground(string orderId)
        //{

        //    //string token = await GetValidToken();

        //    ////  string url = "https://api.phonepe.com/apis/pg/checkout/v2/order/" + orderId + "/status";
        //    //// string url = "https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/order/" + orderId + "/status";
        //    //// ✅ SANDBOX URL (same as payment)

        //    //// string url = $"https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/order/{orderId}/status"; // sandbox url

        //    //string url = $"https://api.phonepe.com/apis/pg/checkout/v2/order/{orderId}/status"; // production url 

        //    //int totalTime = 0;

        //    string token = await GetValidToken();

        //    //  string url = "https://api.phonepe.com/apis/pg/checkout/v2/order/" + orderId + "/status";
        //    string url = "https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/order/" + orderId + "/status";

        //    int totalTime = 0;

        //    while (totalTime <= 300) // 5 minute tak check
        //    {
        //        try
        //        {
        //            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        //            req.Method = "GET";
        //            req.ContentType = "application/json";
        //            req.Headers.Add("Authorization", "Bearer " + token);
        //            //  req.Headers.Add("Authorization", "O-Bearer " + token);


        //            var payResponse = (HttpWebResponse)req.GetResponse();
        //            string payResponseString;

        //            using (var reader = new StreamReader(payResponse.GetResponseStream()))
        //                payResponseString = reader.ReadToEnd();

        //            //string result;

        //            //using (var response = (HttpWebResponse)req.GetResponse())
        //            //using (var reader = new StreamReader(response.GetResponseStream()))
        //            //    result = reader.ReadToEnd();

        //            dynamic res = JsonConvert.DeserializeObject(payResponseString);

        //            string state = res.state;
        //            string txnId = res.paymentDetails?[0]?.transactionId;

        //            if (state == "COMPLETED" || state == "FAILED")
        //            {
        //                var order = _context.M_FeeDetail.FirstOrDefault(x => x.OrderNo == orderId);

        //                if (order != null)
        //                {
        //                    order.OrderStatus = state;
        //                    order.TransactionId = txnId;
        //                    await _context.SaveChangesAsync();
        //                }

        //                break;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return;
        //        }

        //        await Task.Delay(5000); // 5 second wait
        //        totalTime += 5;
        //    }
        //}


        //public async Task CheckPaymentStatusBackground(string orderId)
        //{
        //    string token = await GetValidToken();

        //    string url = $"https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/order/{orderId}/status";

        //    int totalTime = 0;

        //    while (totalTime <= 300)
        //    {
        //        try
        //        {
        //            using (var client = new HttpClient())
        //            {
        //                //  client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        //                client.DefaultRequestHeaders.Add("Authorization", "O-Bearer " + token);
        //                // try this if error: "O-Bearer " + token

        //                var response = await client.GetAsync(url);
        //                var result = await response.Content.ReadAsStringAsync();

        //                if (!response.IsSuccessStatusCode)
        //                {
        //                    Console.WriteLine("PhonePe Error: " + result);
        //                    return;
        //                }

        //                dynamic res = JsonConvert.DeserializeObject(result);

        //                string state = res?.state;
        //                string txnId = res?.paymentDetails != null && res.paymentDetails.Count > 0
        //                                ? res.paymentDetails[0].transactionId
        //                                : null;

        //                if (state == "COMPLETED" || state == "FAILED")
        //                {
        //                    var order = _context.M_FeeDetail.FirstOrDefault(x => x.OrderNo == orderId);

        //                    if (order != null)
        //                    {
        //                        order.OrderStatus = state;
        //                        order.TransactionId = txnId;
        //                        await _context.SaveChangesAsync();
        //                    }

        //                    break;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // ✅ IMPORTANT: log error
        //            Console.WriteLine("Hangfire Error: " + ex.Message);
        //            Console.WriteLine("Inner: " + ex.InnerException?.Message);
        //            return;
        //        }

        //        await Task.Delay(5000);
        //        totalTime += 5;
        //    }
        //}



        // new code 


        // =============================== Transport Fee  deposit 

    }
}
