using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using ApiProject.Service.Current;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
using Microsoft.Win32.SafeHandles;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;


namespace ApiProject.Service.Parents
{
    public class ParentsService : IParentsService
    {

        private readonly ILoginUserService _loginUser;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;

        public ParentsService(ILoginUserService loginUser, ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _loginUser = loginUser;
            _context = context;
            _mapper = mapper;
            _context = context;
            _configuration = configuration;
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

        // StudentInstallment Fee
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
                        sectionId = a.SessionId,
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

                    }).FirstOrDefaultAsync();
                return ApiResponse<getStudentInstallmentModel>.SuccessResponse(InstallFee, "Student installment data fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<getStudentInstallmentModel>.ErrorResponse("Error :" + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddStudentInstallmentFee(AddStudentinstallReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var student = await _context.Student_Renew.Where(a => a.ClassId == req.ClassId && a.StuId == req.StudentId && a.due_fee == 0).FirstOrDefaultAsync();
                if (student != null)
                {
                    return ApiResponse<bool>.ErrorResponse("duefee already availble");
                }

                var GetInstituteCodeName = await _context.institute.FirstOrDefaultAsync(i => i.institute_id == SchoolId);
                var LastCode = await _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).OrderByDescending(s => s.FDId).FirstOrDefaultAsync();

                string instCode = GetInstituteCodeName.instituteCode.Substring(0, 3).ToUpper();

                int newId = (LastCode != null) ? int.Parse(LastCode.ReceiptNo.Split('/')[1]) + 1 : 1;

                string ReceiptCode = $"{instCode}/{newId}";
                int NewOrderNo = 1;
                var LastOrderNo = await _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).OrderByDescending(s => s.OrderNo)
                    .Select(s => s.OrderNo).FirstOrDefaultAsync();

                if (LastOrderNo != null && LastOrderNo != "")

                    if (!string.IsNullOrEmpty(LastOrderNo))
                    {
                        int lastNum;
                        if (int.TryParse(LastOrderNo, out lastNum))
                        {
                            NewOrderNo = lastNum + 1;
                        }
                    }

                int FDId = _context.M_FeeDetail.DefaultIfEmpty().Max(s => s == null ? 0 : s.FDId) + 1;

                var StuInstall = new M_FeeDetail
                {
                    FDId = FDId,
                    stu_id = req.StudentId,
                    ClassId = req.ClassId,
                    OrderNo = NewOrderNo.ToString(),
                    OrderStatus = "Pending",
                    TransactionId = "",
                    ReceiptType = "Online",
                    DueFees = req.Duefee,
                    PayFees = req.PaidFee,
                    Cash = 0,
                    Upi = 0,
                    AdmissionPayfee = 0,
                    AFeeDiscount = 0,
                    PramoteFees = 0,
                    Date = DateTime.Now,
                    Status = "Pending",
                    Active = false,
                    CompanyId = SchoolId,
                    SessionId = SessionId,
                    NetDueFees = req.Duefee,
                    PaymentMode = "UPI",
                    PaymentDate = req.PaymentDate,
                    Remark = req.Remark,
                    RTS = DateTime.Now,
                };
                _context.M_FeeDetail.Add(StuInstall);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Student installment fee saved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateStudentPaymentSuccessfully(int StudentId, int ReceiptId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;


                var fee = _context.M_FeeDetail.FirstOrDefault(a => a.FDId == ReceiptId && a.SessionId == SessionId && a.CompanyId == SchoolId);
                fee.OrderStatus = "Success";
                fee.Status = "Success";
                fee.Active = true;
                await _context.SaveChangesAsync();

                var StudentInstall = _context.Student_Renew.Where(c => c.StuId == fee.stu_id && c.CompanyId == SchoolId && c.SessionId == SessionId && c.ClassId == fee.ClassId).FirstOrDefault();
                StudentInstall.due_fee = StudentInstall.due_fee - fee.PayFees;
                StudentInstall.stu_fee += fee.PayFees;
                await _context.SaveChangesAsync();

                var Installment = _context.fee_installment.Where(c => c.stu_id == fee.stu_id && c.university_id == fee.ClassId && c.CompanyId == SchoolId && c.SessionId == SessionId)
                    .Select(c => new GetStuInstallmentModel
                    {
                        university_id = c.university_id,
                        IntallmentID = c.IntallmentID,
                        total_fee = c.total_fee,
                        Installment = c.Installment,
                        FAmount = c.FAmount,
                    }).ToList();

                double subfee = 0;
                double subfee2 = Convert.ToDouble(fee.PayFees);

                for (int i = 0; i < Installment.Count && fee.PayFees > 0; i++)
                {
                    int? installmentid = Installment[i].IntallmentID;

                    fee_installment insta = _context.fee_installment.FirstOrDefault(f => f.stu_id == fee.stu_id && f.university_id == fee.ClassId && f.CompanyId == SchoolId && f.SessionId == SessionId && f.IntallmentID == installmentid);

                    if (insta != null)
                    {
                        if (insta.due_fee != 0)
                        {

                            if (subfee2 >= insta.due_fee)
                            {
                                subfee2 = Convert.ToDouble(subfee2) - Convert.ToDouble(insta.due_fee);
                                insta.due_fee = 0;
                                await _context.SaveChangesAsync();
                            }
                            else if (subfee2 != 0)
                            {
                                insta.due_fee = insta.due_fee - subfee2;
                                subfee2 = 0;
                                await _context.SaveChangesAsync();
                                break;

                            }
                            else
                            {
                                insta.due_fee = insta.due_fee - subfee2;
                                subfee2 = 0;
                                await _context.SaveChangesAsync();
                                break;

                            }
                        }
                    }
                }


                //var res = new StudentClassExamData
                //{
                //    ExamDatas = ExamData,
                //    // SectionDatas = SectionData,
                //};

                return ApiResponse<bool>.SuccessResponse(true, "Student installment fee data fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error : " + ex.Message);
            }
        }


        // Transport Fee 
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

                int startMonthNo = startDate?.Month ?? 1;           // route date ka month number (int)
                string startMonth = new DateTime(2024, startMonthNo, 1).ToString("MMMM");

                int currentMonthNo = DateTime.Now.Month;           // current month number (int)
                string currentMonth = new DateTime(2024, currentMonthNo, 1).ToString("MMMM");

                var validMonths = Enumerable.Range(startMonthNo, currentMonthNo - startMonthNo + 1).Select(m => new DateTime(2024, m, 1).ToString("MMMM")).ToList();


                var res = await _context.StuRouteAssignTbl.Where(c => c.stu_id == StudentId && c.CompanyId == SchoolId && c.SessionId == SessionId)
                    .Select(c => new GetTransportInstallFeeModel
                    {
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

                        TransReceiptList = _context.NewTransportFeeTbl.Where(a => a.stu_id == c.stu_id && a.CompanyId == SchoolId)
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
                            Remark  = a.Remark,
                        }).ToList(),

                    }).FirstOrDefaultAsync();

                return ApiResponse<GetTransportInstallFeeModel>.SuccessResponse(res, "Fetch successfully Transport Student data ");
            }
            catch (Exception ex)
            {
                return ApiResponse<GetTransportInstallFeeModel>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> AddStudentTransportFee(AddTransportMonthFeeReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var TransFee = await _context.NewTransportFeeTbl.Where(p => p.MonthName == req.MonthName && p.stu_id == req.StudentId && p.university_id == req.ClassId).FirstOrDefaultAsync();
                if (TransFee != null)
                {
                    return ApiResponse<bool>.ErrorResponse("month name akready avaliale");
                }
                if (req.PayFee < 0)
                {
                    return ApiResponse<bool>.ErrorResponse("Invalid Amount");
                }


                var GetInstituteCodeName = await _context.institute.FirstOrDefaultAsync(i => i.institute_id == SchoolId);
                var LastCode = await _context.NewTransportFeeTbl.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).OrderByDescending(s => s.NewPaymentId).FirstOrDefaultAsync();

                string instCode = GetInstituteCodeName.instituteCode.Substring(0, 3).ToUpper();

                int newId = (LastCode != null) ? int.Parse(LastCode.ReceiptNo.Split('/')[1]) + 1 : 1;

                string ReceiptCode = $"{instCode}/{newId}";
                int NewOrderNo = 1;
                var LastOrderNo = await _context.NewTransportFeeTbl.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).OrderByDescending(s => s.OrderNo)
                    .Select(s => s.OrderNo).FirstOrDefaultAsync();

                if (LastOrderNo != null && LastOrderNo != "")

                    if (!string.IsNullOrEmpty(LastOrderNo))
                    {
                        int lastNum;
                        if (int.TryParse(LastOrderNo, out lastNum))
                        {
                            NewOrderNo = lastNum + 1;
                        }
                    }

                int NewPaymentId = _context.NewTransportFeeTbl.DefaultIfEmpty().Max(s => s == null ? 0 : s.NewPaymentId) + 1;

                var StuInstall = new NewTransportFeeTbl
                {
                    NewPaymentId = NewPaymentId,
                    stu_id = req.StudentId,
                    university_id = req.ClassId,
                    SectionId = req.sectionId,
                    Date = req.Date,
                    BusId = req.VehicleId,
                    RouteId = req.RouteId,
                    StoppageId = req.StoppageId,
                    TransFee = req.PayFee ?? 0,
                    MonthName = req.MonthName,
                    Discount = req.Discount ?? 0,
                    SpclDiscount = req.SpclDiscount ?? 0,
                    NetTransFee = req.PayFee ?? 0,
                    PayFee = req.PayFee ?? 0,
                    Paydiscount = req.PayDiscount ?? 0,
                    FeeType = "TransportFee",
                    OrderNo = NewOrderNo.ToString(),
                    OrderStatus = "Pending",
                    TransactionId = "",
                    ReceiptType = "Online",
                    Active = false,
                    CompanyId = SchoolId,
                    SessionId = SessionId,
                    PaymentMode = "UPI",
                    Remark = req.Remark,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                };
                _context.NewTransportFeeTbl.Add(StuInstall);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Student transport fee saved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateTransportPaymentSuccessfully(int StudentId, int ReceiptId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var Tfee = _context.NewTransportFeeTbl.FirstOrDefault(a => a.NewPaymentId == ReceiptId && a.SessionId == SessionId && a.CompanyId == SchoolId);
                Tfee.OrderStatus = "Success";
                // Tfee.Status = "Success";
             //   Tfee.TransactionId = txnId ?? "";
                Tfee.Active = true;
                await _context.SaveChangesAsync();

                var sturoute = _context.StuRouteAssignTbl.FirstOrDefault(s => s.stu_id == Tfee.stu_id && s.SessionId == SessionId && s.CompanyId == SchoolId);
                sturoute.TTransportFee += Tfee.NetTransFee;
                sturoute.TDueFee = Tfee.DueFee;
                sturoute.TPayDiscount += Tfee.Paydiscount;
                sturoute.TPayFee += Tfee.PayFee;
                await _context.SaveChangesAsync();

                var months = Tfee.MonthName.Split(',').Select(m => m.Trim()).ToList();

                var installments = _context.TransInstallmentTbl.Where(u => u.StuId == Tfee.stu_id && u.ClassId == Tfee.university_id && u.SessionId == SessionId &&
                    u.CompanyId == SchoolId && months.Contains(u.MonthName)).ToList();

                for (int i = 0; i < installments.Count; i++)
                {
                    installments[i].ReActive = true;
                }
                await _context.SaveChangesAsync();

                double remainingPay = (Tfee.PayFee ?? 0.0) + (Tfee.Paydiscount ?? 0.0);

                // सभी सक्रिय इंस्टॉलमेंट्स को प्राप्त करें, जिन्हें पहले सक्रिय किया गया है
                var activeInstallments = _context.TransInstallmentTbl.Where(u => u.StuId == Tfee.stu_id && u.ClassId == Tfee.university_id && u.SessionId == SessionId &&
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

                return ApiResponse<bool>.SuccessResponse(true, "Student transport fee data fetched successfully");
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

        public async Task<ApiResponse<GetStuDueInstallmentModel>> GetStudentDueInstallment2()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;
                int StudentId = _loginUser.StudentId;

                var studentData = await _context.StudentRenewView.Where(a => a.StuId == StudentId && a.CompanyId == SchoolId && a.SessionId == SessionId && a.RActive == true)
                   .Select(a => new
                   {
                       a.StuId,
                       a.ClassId
                   }).FirstOrDefaultAsync();

                if (studentData == null)
                    return ApiResponse<GetStuDueInstallmentModel>.ErrorResponse("Student not found");

                var PaidAmount = await _context.M_FeeDetail.Where(u => u.stu_id == studentData.StuId && u.ClassId == studentData.ClassId && u.SessionId == SessionId
                 && u.CompanyId == SchoolId && u.Status == "1" && u.Active == true).SumAsync(u => u.PayFees) ?? 0;

                var Installment = await _context.fee_installment.Where(u => u.stu_id == studentData.StuId && u.university_id == studentData.ClassId && u.SessionId == SessionId && u.CompanyId == SchoolId)
                    .Select(u => u.FAmount).ToListAsync();


                for (int i = 0; i < Installment.Count; i++)
                {
                    var insAmount = Installment[i];
                    if (PaidAmount >= insAmount)
                    {
                        // This installment is fully paid
                        PaidAmount -= (double)insAmount;

                    }
                    else if (PaidAmount > 0)
                    {
                        // Partially paid installment
                        var remaining = insAmount - PaidAmount;
                        PaidAmount = 0;
                    }
                    else
                    {
                        // Not paid at all
                        Installment.Add(insAmount);
                    }
                }

                var result = new GetStuDueInstallmentModel
                {
                    // PaidAmount = PaidAmount,
                    DueInstallment = Installment
                };


                return ApiResponse<GetStuDueInstallmentModel>.SuccessResponse(result, "Student due installment fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<GetStuDueInstallmentModel>.ErrorResponse("Error :" + ex.Message);
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



    }
}
