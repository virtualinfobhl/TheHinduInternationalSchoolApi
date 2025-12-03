using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using ApiProject.Service.Current;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using OfficeOpenXml;
using OfficeOpenXml.Table.PivotTable;
using System.Collections.Immutable;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing.Printing;
using static ApiProject.Models.Response.ClassByFeeInResponse;



namespace ApiProject.Service.Student
{
    public class StudentService : IStudentService
    {
        private readonly ILoginUserService _loginUser;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public StudentService(
            ILoginUserService loginUser,
            ApplicationDbContext context,
            IMapper mapper
            )
        {
            _context = context;
            _loginUser = loginUser;
            _mapper = mapper;
        }

        // student details
        public async Task<List<ClassResModel>> GetClass()
        {
            int SchoolId = _loginUser.SchoolId;

            var ClassResModel = await _context.University.Where(c => c.CompanyId == SchoolId && c.Active == true).Select(c => new ClassResModel
            {
                ClassId = c.university_id,
                ClassName = c.university_name,
                CActive = c.Active,
            }).ToListAsync();
            return ClassResModel;

        }
        public async Task<ApiResponse<FeendSectionByClasssModel>> GetClassByFeendSection(int ClassId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var Sectiondata = await _context.collegeinfo.Where(a => a.university_id == ClassId && a.CompanyId == SchoolId)
                    .Select(a => new SectionDataList
                    {
                        ClassId = a.university_id,
                        SectionId = a.collegeid,
                        SectionName = a.collegename,

                    }).ToListAsync();

                var Feedata = await _context.fees.Where(c => c.university_id == ClassId && c.CompanyId == SchoolId && c.SessionId == SessionId && c.active == true)
                    .Select(c => new ClassFeeResModel
                    {
                        admission_fee = c.admission_fee,
                        ClassId = c.university_id,
                        tution_fee = c.tution_fee,
                        exam_fee = c.exam_fee,
                        Develoment_fee = c.Develoment_fee,
                        Games_fees = c.Games_fees,
                        total = c.total,
                        active = c.active,

                    }).ToListAsync();

                var installment = await _context.InstallmentTbl.Where(a => a.university_id == ClassId && a.CompanyId == SchoolId && a.SessionId == SessionId)
                    .Select(a => new InstallmentDetail
                    {
                        Installment = a.Installment,
                        Installmentno = a.Installmentno,
                        FeeAmount = a.FeeAmount,
                    }).ToListAsync();

                var res = new FeendSectionByClasssModel
                {
                    SectionList = Sectiondata,
                    FeeList = Feedata,
                    installments = installment,
                };

                return ApiResponse<FeendSectionByClasssModel>.SuccessResponse(res, "Fetch successfully class by section , Fee and installment data ");
            }
            catch (Exception ex)
            {
                return ApiResponse<FeendSectionByClasssModel>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<GetParentsDetailModel>> GetParentsByMobileNo(string Mobileno)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var parentdata = await _context.ParentsTbl.Where(a => a.FatherMobileNo == Mobileno && a.CompanyId == SchoolId)
                    .Select(a => new GetParentsDetailModel
                    {
                        ParentsId = a.ParentsId,
                        father_name = a.FatherName,
                        father_mobile = a.FatherMobileNo,
                        father_occupation = a.FatherOccupation,
                        Fatherlncome = a.FatherIncome,
                        mother_name = a.MotherName,
                        mother_mobile = a.MotherMobileNo,
                        mother_occupation = a.MotherOccupation,
                        MotherIncome = a.MotherIncome,

                    }).FirstOrDefaultAsync();

                return ApiResponse<GetParentsDetailModel>.SuccessResponse(parentdata, "Fetch successfully Mobile no by parents data ");
            }
            catch (Exception ex)
            {
                return ApiResponse<GetParentsDetailModel>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<quickadmissionres>> AddStuQuickadmission(quickadmissionmodel request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            {
                try
                {
                    int SchoolId = _loginUser.SchoolId;
                    int UserId = _loginUser.UserId;
                    int SessionId = _loginUser.SessionId;

                    int ReceiptId = 0;

                    var existingStudent = await _context.student_admission.FirstOrDefaultAsync(p => p.registration_no == request.SRNo && p.CompanyId == SchoolId);

                    if (existingStudent != null)
                        return ApiResponse<quickadmissionres>.ErrorResponse("Student already exists");

                    var studentfee = await _context.fees.Where(p => p.university_id == request.ClassId && p.CompanyId == SchoolId && p.SessionId == SessionId && p.active == true).FirstOrDefaultAsync();
                    if (studentfee == null)
                        return ApiResponse<quickadmissionres>.ErrorResponse("class fees not exists");

                    var feeInstallments = await _context.InstallmentTbl.Where(p => p.university_id == request.ClassId && p.CompanyId == SchoolId && p.SessionId == SessionId).ToListAsync();
                    if (feeInstallments == null)
                        return ApiResponse<quickadmissionres>.ErrorResponse("class fees Installments not exists");

                    int parentid = 0;
                    var parentdata = await _context.ParentsTbl.Where(p => p.FatherMobileNo == request.father_mobile && p.CompanyId == SchoolId).FirstOrDefaultAsync();

                    if (parentdata == null)
                    {
                        //  int ParentsId = (_context.ParentsTbl.DefaultIfEmpty().Max(r => r == null ? 0 : r.ParentsId) + 1);

                        var parent = new ParentsTbl
                        {
                            FatherName = request.father_name,
                            FatherMobileNo = request.father_mobile,
                            MotherName = request.mother_name,
                            GuardianName = request.GuardianName,
                            GuardianMobileNo = request.GuardianMobileNo,
                            Username = request.father_mobile,
                            Password = request.father_mobile,
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            SessionId = SessionId,
                            CompanyId = SchoolId,
                            UserId = UserId
                        };
                        _context.ParentsTbl.Add(parent);
                        await _context.SaveChangesAsync();
                        parentid = parent.ParentsId;

                    }
                    else
                    {
                        parentid = parentdata.ParentsId;
                    }

                    string StudentCode = "";
                    institute GetInstituteCodeName = _context.institute.Where(i => i.institute_id == SchoolId).FirstOrDefault();

                    var sessioninfo = await _context.SessionInfo.FirstOrDefaultAsync(p => p.CompanyId == SchoolId && p.Active == true);
                    var startYearY = Convert.ToDateTime(sessioninfo.StartSession).ToString("yy");

                    student_admission LastCode = _context.student_admission.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId)
                        .OrderByDescending(s => s.stu_id).FirstOrDefault();

                    string threeLetters = GetInstituteCodeName.instituteCode.Substring(0, 3).ToUpper();
                    // int year = startYear;
                    int NewId = 1;

                    if (LastCode != null)
                    {
                        var parts = LastCode.stu_code.Split('/');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int lastId))
                        {
                            NewId = lastId + 1;
                        }
                    }

                    StudentCode = threeLetters + "-" + startYearY + "/" + NewId;

                    var student = new student_admission
                    {
                        stu_id = (_context.student_admission.DefaultIfEmpty().Max(r => r == null ? 0 : r.stu_id) + 1),
                        ParentsId = parentid,
                        //  ParentsId = parentdata.ParentsId,

                        stu_code = StudentCode,
                        registration_no = request.SRNo,
                        stu_name = request.stu_name,
                        DOB = request.DOB,
                        university_id = request.ClassId,
                        college_id = request.SectionId,
                        gender = request.gender,
                        father_name = request.father_name,
                        father_mobile = request.father_mobile,
                        mother_name = request.mother_name,
                        admission_date = request.admission_date,
                        username = request.father_mobile,
                        password = request.father_mobile,
                        fee_status = "AdmissionPayfee",
                        AFeeDiscount = request.AFeeDiscount ?? 0,
                        AdmissionPayfee = studentfee.admission_fee - request.AFeeDiscount,
                        admission_fee = studentfee.admission_fee,
                        tution_fee = studentfee.tution_fee,
                        exam_fee = studentfee.exam_fee,
                        Develoment_fee = studentfee.Develoment_fee,
                        Games_fees = studentfee.Games_fees,
                        total = studentfee.total,
                        total_fee = studentfee.total,
                        due_fee = studentfee.total,
                        discount = 0,
                        stu_fee = 0,
                        StuDetail = false,
                        StuFee = false,
                        active = true,
                        completed = false,
                        date = DateTime.Now,
                        SessionId = SessionId,
                        CompanyId = SchoolId,
                        Userid = UserId
                    };

                    _context.student_admission.Add(student);
                    await _context.SaveChangesAsync();


                    var studentrenew = new Student_Renew
                    {
                        SRId = (_context.Student_Renew.DefaultIfEmpty().Max(r => r == null ? 0 : r.SRId) + 1),
                        StuId = student.stu_id,
                        ParentsId = parentid,
                        //    ParentsId = parentdata.ParentsId,
                        ClassId = request.ClassId,
                        RollNo = "",
                        RTE = request.RTE != "0",
                        Status = "AdmissionPayfee",
                        payment_mode = request.PaymentMode,
                        Active = true,
                        completed = false,
                        StuDetail = false,
                        StuFees = false,
                        Dropout = false,
                        Date = request.admission_date,
                        CompanyId = SchoolId,
                        Userid = UserId,
                        SessionId = SessionId,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };

                    if ((bool)studentrenew.RTE)
                    {
                        studentrenew.admission_fee = 0;
                        studentrenew.PramoteFees = 0;
                        studentrenew.AFeeDiscount = 0;
                        studentrenew.AdmissionPayfee = 0;
                        studentrenew.exam_fee = 0;
                        studentrenew.Tution_fee = 0;
                        studentrenew.Develoment_fee = 0;
                        studentrenew.Games_fees = 0;
                        studentrenew.total = 0;
                        studentrenew.discount = 0;
                        studentrenew.OldDuefees = 0;
                        studentrenew.total_fee = 0;
                        studentrenew.stu_fee = 0;
                        studentrenew.due_fee = 0;
                    }
                    else
                    {
                        studentrenew.admission_fee = studentfee.admission_fee;
                        studentrenew.AFeeDiscount = request.AFeeDiscount ?? 0;
                        studentrenew.AdmissionPayfee = studentfee.admission_fee - studentrenew.AFeeDiscount;
                        studentrenew.exam_fee = studentfee.exam_fee;
                        studentrenew.Tution_fee = studentfee.tution_fee;
                        studentrenew.Develoment_fee = studentfee.Develoment_fee;
                        studentrenew.Games_fees = studentfee.Games_fees;
                        studentrenew.total = studentfee.total;
                        studentrenew.discount = 0;
                        studentrenew.PramoteFees = 0;
                        studentrenew.OldDuefees = 0;
                        studentrenew.total_fee = studentfee.total;
                        studentrenew.stu_fee = 0;
                        studentrenew.due_fee = studentfee.total;

                    }

                    _context.Student_Renew.Add(studentrenew);
                    await _context.SaveChangesAsync();


                    if ((bool)!studentrenew.RTE)
                    {
                        var installments = await _context.InstallmentTbl
                            .Where(p => p.university_id == studentrenew.ClassId && p.CompanyId == SchoolId && p.SessionId == SessionId)
                            .ToListAsync();

                        foreach (var inst in installments)
                        {
                            var feeInstall = new fee_installment
                            {
                                Id = (_context.fee_installment.DefaultIfEmpty().Max(r => r == null ? 0 : r.Id) + 1),
                                stu_id = student.stu_id,
                                university_id = studentrenew.ClassId,
                                paid_date = DateTime.Now,
                                IntallmentID = inst.InstallmentId,
                                total_fee = studentrenew.total_fee,
                                Installment = inst.Installment,
                                due_fee = inst.FeeAmount,
                                FAmount = inst.FeeAmount,
                                active = true,
                                Date = DateTime.Now,
                                SessionId = SessionId,
                                CompanyId = SchoolId,
                                Userid = UserId,
                            };
                            _context.fee_installment.Add(feeInstall);
                            await _context.SaveChangesAsync();
                        }

                        if (studentrenew.AdmissionPayfee + studentrenew.PramoteFees > 0)
                        {
                            string ReceiptCode = "";

                            var lastReceipt = await _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId).OrderByDescending(s => s.FDId).FirstOrDefaultAsync();

                            int NewOrderNo = 1;
                            var LastOrderNo = _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).OrderByDescending(s => s.OrderNo)
                                .Select(s => s.OrderNo).FirstOrDefault();

                            if (LastOrderNo != null && LastOrderNo != "")

                                if (!string.IsNullOrEmpty(LastOrderNo))
                                {
                                    int lastNum;
                                    if (int.TryParse(LastOrderNo, out lastNum))
                                    {
                                        NewOrderNo = lastNum + 1;
                                    }
                                }

                            var Id = 0;
                            if (lastReceipt != null)
                            {
                                var Receipt = lastReceipt.ReceiptNo.Split('/');
                                ReceiptCode = Receipt[1];

                                Id = int.Parse(ReceiptCode);
                                Id++;
                            }
                            else
                            {
                                Id = 1;
                            }
                            ReceiptCode = threeLetters + "/" + Id;

                            int FDId = (_context.M_FeeDetail.DefaultIfEmpty().Max(r => r == null ? 0 : r.FDId) + 1);
                            var receipt = new M_FeeDetail
                            {
                                // ReceiptNo = lastReceipt == null ? "1" : (Convert.ToInt32(lastReceipt.ReceiptNo) + 1).ToString(),
                                ReceiptNo = ReceiptCode,
                                OrderNo = NewOrderNo.ToString(),
                                OrderStatus = "Succcessfully",
                                TransactionId = "",
                                ReceiptType = "Offline",
                                ClassId = studentrenew.ClassId,
                                stu_id = studentrenew.StuId,
                                Status = "AdmissionPayfee",
                                PayFees = 0,
                                AdmissionFees = studentfee.admission_fee,
                                ExamFees = studentfee.exam_fee,
                                Tutionfee = studentfee.tution_fee,
                                Develoment_fee = studentfee.Develoment_fee,
                                Games_fees = studentfee.Games_fees,
                                FeeTotal = studentfee.total,
                                Discount = 0,
                                OldDuefees = 0,
                                TotalFees = studentfee.total,
                                NetDueFees = studentfee.total,
                                DueFees = studentfee.total,
                                Date = DateTime.Now.Date,
                                RTS = DateTime.Now.Date,
                                PaymentDate = DateTime.Now.Date,
                                //  PaymentDate = request.pay

                                PaymentMode = request.PaymentMode,
                                AdmissionPayfee = studentrenew.AdmissionPayfee,
                                AFeeDiscount = studentrenew.AFeeDiscount,
                                Cash = 0,
                                Upi = 0,
                                Remark = "",
                                Active = true,
                                CompanyId = SchoolId,
                                Userid = UserId,
                                SessionId = SessionId,
                            };

                            ReceiptId = FDId;

                            _context.M_FeeDetail.Add(receipt);
                            await _context.SaveChangesAsync();
                        }
                    }

                    await transaction.CommitAsync();
                    var result = new quickadmissionres
                    {
                        StudentId = student.stu_id,
                        ReceiptId = ReceiptId
                    };

                    return ApiResponse<quickadmissionres>.SuccessResponse(result, "Admission successful");
                }
                catch (Exception ex)
                {

                    await transaction.RollbackAsync();
                    return ApiResponse<quickadmissionres>.ErrorResponse("Something went wrong: " + ex.Message);
                }
            }
        }
        public async Task<ApiResponse<quickadmissionres>> AddStudentAdmissionAsync(AddStudentReqModel request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            {
                try
                {
                    int SchoolId = _loginUser.SchoolId;
                    int UserId = _loginUser.UserId;
                    int SessionId = _loginUser.SessionId;
                    int ReceiptId = 0;

                    student_admission duplicateStudent = _context.student_admission.Where(p => p.registration_no == request.SRNo && p.CompanyId == SchoolId).FirstOrDefault();

                    if (duplicateStudent != null)
                        return ApiResponse<quickadmissionres>.ErrorResponse("Student already exists");

                    var studentfee = await _context.fees.Where(p => p.university_id == request.ClassId && p.CompanyId == SchoolId && p.SessionId == SessionId && p.active == true).FirstOrDefaultAsync();
                    if (studentfee == null)
                        return ApiResponse<quickadmissionres>.ErrorResponse("class fees not exists");

                    int receiptId = 0;

                    int parentid = 0;
                    var parentdata = await _context.ParentsTbl.Where(p => p.FatherMobileNo == request.FatherMobileNo && p.CompanyId == SchoolId).FirstOrDefaultAsync();

                    if (parentdata == null)
                    {

                        var parent = new ParentsTbl
                        {
                            FatherName = request.FatherName,
                            FatherMobileNo = request.FatherMobileNo,
                            FatherOccupation = request.FatherOccupation,
                            FatherIncome = request.FatherIncome,
                            MotherName = request.MotherName,
                            MotherMobileNo = request.MotherMobileNo,
                            MotherIncome = request.MotherIncome,
                            MotherOccupation = request.MotherOccupation,
                            GuardianName = request.GuardianName,
                            GuardianMobileNo = request.GuardianMobileNo,
                            Username = request.FatherMobileNo,
                            Password = request.FatherMobileNo,
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            SessionId = SessionId,
                            CompanyId = SchoolId,
                            UserId = UserId
                        };
                        _context.ParentsTbl.Add(parent);
                        await _context.SaveChangesAsync();
                        parentid = parent.ParentsId;

                    }
                    else
                    {
                        parentid = parentdata.ParentsId;
                    }

                    string StudentCode = "";
                    institute GetInstituteCodeName = _context.institute.Where(i => i.institute_id == SchoolId).FirstOrDefault();

                    var sessioninfo = await _context.SessionInfo.FirstOrDefaultAsync(p => p.CompanyId == SchoolId && p.Active == true);
                    var startYearY = Convert.ToDateTime(sessioninfo.StartSession).ToString("yy");

                    student_admission LastCode = _context.student_admission.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId)
                        .OrderByDescending(s => s.stu_id).FirstOrDefault();

                    string threeLetters = GetInstituteCodeName.instituteCode.Substring(0, 3).ToUpper();
                    // int year = startYear;
                    int NewId = 1;

                    if (LastCode != null)
                    {
                        var parts = LastCode.stu_code.Split('/');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int lastId))
                        {
                            NewId = lastId + 1;
                        }
                    }

                    StudentCode = threeLetters + "-" + startYearY + "/" + NewId;

                    student_admission student = new student_admission();

                    //var allSchoolDataRoot = Path.Combine("C:", "HostingSpaces", "websites", "vedusoft.in", "wwwroot", "Image", "ALLSchoolData");
                    student.stu_id = _context.student_admission.DefaultIfEmpty().Max(r => r == null ? 0 : r.stu_id) + 1;

                    var allSchoolDataRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", "ALLSchoolData");
                    var schoolFolderPath = Path.Combine(allSchoolDataRoot, SchoolId.ToString());
                    
                    if (!Directory.Exists(schoolFolderPath))
                    {
                        Directory.CreateDirectory(schoolFolderPath);
                    }

                    if (request.stuphoto != null)
                    {
                        string photoFolder = Path.Combine(schoolFolderPath, "StudentPhoto");
                        student.stu_photo = await SaveStudentFileAsync(request.stuphoto, photoFolder, student.stu_id, SchoolId.ToString(), "StudentPhoto");
                    }
                    if (request.stuaadhar != null)
                    {
                        string photoFolder = Path.Combine(schoolFolderPath, "Aadhar");
                        student.stu_aadhar = await SaveStudentFileAsync(request.stuaadhar, photoFolder, student.stu_id, SchoolId.ToString(), "Aadhar");
                    }
                    if (request.stubirth != null)
                    {
                        string photoFolder = Path.Combine(schoolFolderPath, "stubirth");
                        student.stu_birth = await SaveStudentFileAsync(request.stubirth, photoFolder, student.stu_id, SchoolId.ToString(), "stubirth");
                    }
                    if (request.fatheraadhar != null)
                    {
                        string photoFolder = Path.Combine(schoolFolderPath, "father_aadhar");
                        student.father_aadhar = await SaveStudentFileAsync(request.fatheraadhar, photoFolder, student.stu_id, SchoolId.ToString(), "father_aadhar");
                    }
                    if (request.motheraadhar != null)
                    {
                        string photoFolder = Path.Combine(schoolFolderPath, "mother_aadhar");
                        student.mother_aadhar = await SaveStudentFileAsync(request.motheraadhar, photoFolder, student.stu_id, SchoolId.ToString(), "mother_aadhar");
                    }
                    if (request.IncomeCertificate != null)
                    {
                        string photoFolder = Path.Combine(schoolFolderPath, "IncomeCertificate");
                        student.Income_Certificate = await SaveStudentFileAsync(request.IncomeCertificate, photoFolder, student.stu_id, SchoolId.ToString(), "IncomeCertificate");
                    }
                    if (request.JanAadhar != null)
                    {
                        string photoFolder = Path.Combine(schoolFolderPath, "JanAadhar");
                        student.Jan_Aadhar = await SaveStudentFileAsync(request.JanAadhar, photoFolder, student.stu_id, SchoolId.ToString(), "JanAadhar");
                    }
                    if (request.LastMarkSheetPhotos != null)
                    {
                        string photoFolder = Path.Combine(schoolFolderPath, "LastMarkSheetPhoto");
                        student.LastMarkSheetPhoto = await SaveStudentFileAsync(request.LastMarkSheetPhotos, photoFolder, student.stu_id, SchoolId.ToString(), "LastMarkSheetPhoto");
                    }
                    if (request.studentTcfile != null)
                    {
                        string tcFolder = Path.Combine(schoolFolderPath, "LastStudentTC");
                        student.stu_tc = await SaveStudentFileAsync(request.studentTcfile, tcFolder, student.stu_id, SchoolId.ToString(), "LastStudentTC");
                    }

                    //  student.ParentsId = Stuparents.ParentsId;
                    student.ParentsId = parentid;
                    student.registration_no = request.SRNo;
                    student.admission_date = request.admission_date;
                    student.university_id = request.ClassId;
                    student.college_id = request.SectionId;
                    student.stu_code = StudentCode;
                       
                    student.stu_name = request.stu_name;
                    student.DOB = request.DOB;
                    student.gender = request.gender;
                    student.stu_mobile = "";
                    student.email = request.email;
                    student.Religion = request.Religion;
                    student.cast_category = request.cast_category;
                    student.blood_group = request.blood_group;
                    student.Caste = request.Caste;
                    student.AdharCard = "";

                    student.father_name = request.FatherName;
                    student.father_occupation = request.FatherOccupation;
                    student.father_mobile = request.FatherMobileNo;
                    student.Fatherlncome = request.FatherIncome;
                    student.mother_name = request.MotherName;
                    student.mother_mobile = request.MotherMobileNo;
                    student.mother_occupation = request.MotherOccupation;
                    student.MotherIncome = request.MotherIncome;

                    student.address = request.address;
                    student.state = request.state;
                    student.district = request.district;
                    student.city = request.city;
                    student.pincode = request.pincode;

                    student.p_address = request.p_address;
                    student.p_state = request.p_state;
                    student.p_district = request.p_district;
                    student.p_city = request.p_city;
                    student.p_pincode = request.p_pincode;

                    student.username = request.FatherMobileNo;
                    student.password = request.FatherMobileNo;

                    student.LastSchlName = request.LastSchlName;
                    student.LastClass = request.LastClass;
                    student.LastExanTotalMarks = request.LastExanTotalMarks;
                    student.LastDivision = request.LastDivision;
                    student.LastParecentage = request.LastParecentage;
                    student.LastRemarks = request.LastRemarks;

                    student.admission_fee = studentfee.admission_fee;
                    student.tution_fee = studentfee.tution_fee;
                    student.exam_fee = studentfee.exam_fee;
                    student.Develoment_fee = studentfee.Develoment_fee;
                    student.Games_fees = studentfee.Games_fees;
                    student.total = studentfee.total;
                    student.total_fee = studentfee.total - request.admissionReceipt.FeeDiscount;
                    student.due_fee = studentfee.total - request.admissionReceipt.FeeDiscount;
                    student.discount = request.admissionReceipt.FeeDiscount == null ? 0 : request.admissionReceipt.FeeDiscount;
                    //    student.AdmissionPayfee = request.admi/

                    student.AdmissionPayfee = request.admissionReceipt.AdmissionPayFees;
                    student.AFeeDiscount = request.admissionReceipt.AdmissionFeeDiscount;
                    student.stu_fee = 0;
                    student.fee_status = "AdmissionPayfee";
                    student.CompanyId = SchoolId;
                    student.Userid = UserId;
                    student.SessionId = SessionId;
                    student.active = true;
                    student.completed = false;
                    student.StuDetail = true;
                    student.StuFee = true;
                    student.date = DateTime.Now;

                    _context.student_admission.Add(student);
                    await _context.SaveChangesAsync();

                    //Student_Renew studentrenew = new Student_Renew();
                    //studentrenew.SRId = _context.Student_Renew.DefaultIfEmpty().Max(r => r == null ? 0 : r.SRId) + 1;
                    //studentrenew.Active = true;
                    ////  studentrenew.stutc = false;
                    //studentrenew.ParentsId = parentid;
                    //studentrenew.StuId = student.stu_id;
                    //studentrenew.ClassId = request.ClassId;
                    //studentrenew.SessionId = request.SectionId;

                    //var studentfee = _context.fees.Where(p => p.university_id == studentrenew.ClassId && p.CompanyId == SchoolId && p.SessionId == SessionId).FirstOrDefault();

                    //studentrenew.RTE = request.RTE == false ? false : true;
                    //if (studentrenew.RTE == true)
                    //{
                    //    studentrenew.admission_fee = 0;
                    //    studentrenew.PramoteFees = 0;
                    //    studentrenew.AFeeDiscount = 0;
                    //    studentrenew.AdmissionPayfee = 0;
                    //    studentrenew.exam_fee = 0;
                    //    studentrenew.Tution_fee = 0;
                    //    studentrenew.Develoment_fee = 0;
                    //    studentrenew.Games_fees = 0;
                    //    studentrenew.total = 0;
                    //    studentrenew.discount = 0;
                    //    studentrenew.OldDuefees = 0;
                    //    studentrenew.total_fee = 0;
                    //}
                    //else
                    //{
                    //    studentrenew.admission_fee = studentfee.admission_fee;
                    //    studentrenew.AFeeDiscount = request.admissionReceipts.aFeeDiscount == null ? 0 : request.admissionReceipts.aFeeDiscount;
                    //    studentrenew.PramoteFees = request.admissionReceipts.pramoteFees == null ? 0 : request.admissionReceipts.pramoteFees;
                    //    studentrenew.AdmissionPayfee = studentfee.admission_fee - studentrenew.AFeeDiscount;
                    //    studentrenew.exam_fee = studentfee.exam_fee;
                    //    studentrenew.Tution_fee = studentfee.tution_fee;
                    //    studentrenew.Develoment_fee = studentfee.Develoment_fee;
                    //    studentrenew.Games_fees = studentfee.Games_fees;
                    //    studentrenew.total = studentfee.total;
                    //    studentrenew.discount = request.admissionReceipts.discount == null ? 0 : request.admissionReceipts.discount;
                    //    studentrenew.OldDuefees = 0;
                    //    studentrenew.total_fee = studentrenew.total - studentrenew.discount + studentrenew.OldDuefees;
                    //}

                    //studentrenew.CompanyId = SchoolId;
                    //studentrenew.Userid = UserId;
                    //studentrenew.SessionId = SessionId;
                    //studentrenew.CreateDate = DateTime.Now;
                    //studentrenew.UpdateDate = DateTime.Now;
                    //studentrenew.Active = true;
                    //studentrenew.completed = false;

                    //_context.Student_Renew.Add(studentrenew);
                    //await _context.SaveChangesAsync();



                    var studentrenew = new Student_Renew
                    {
                        SRId = (_context.Student_Renew.DefaultIfEmpty().Max(r => r == null ? 0 : r.SRId) + 1),
                        StuId = student.stu_id,
                        ParentsId = parentid,
                        ClassId = request.ClassId,
                        SectionId = request.SectionId,
                        RollNo = "",
                        RTE = request.RTE != "0",
                        Status = "AdmissionPayfee",
                      //  AdmissionPayfee = 
                   //   AFeeDiscount = 
                        Active = true,
                        completed = false,
                        StuDetail = false,
                        StuFees = false,
                        Dropout = false,
                        Date = request.admission_date,
                        CompanyId = SchoolId,
                        Userid = UserId,
                        SessionId = SessionId,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };

                    if ((bool)studentrenew.RTE)
                    {
                        studentrenew.admission_fee = 0;
                        studentrenew.PramoteFees = 0;
                        studentrenew.AFeeDiscount = 0;
                        studentrenew.AdmissionPayfee = 0;
                        studentrenew.exam_fee = 0;
                        studentrenew.Tution_fee = 0;
                        studentrenew.Develoment_fee = 0;
                        studentrenew.Games_fees = 0;
                        studentrenew.total = 0;
                        studentrenew.discount = 0;
                        studentrenew.OldDuefees = 0;
                        studentrenew.total_fee = 0;
                        studentrenew.stu_fee = 0;
                        studentrenew.due_fee = 0;
                    }
                    else
                    {
                        studentrenew.admission_fee = studentfee.admission_fee;
                    
                        studentrenew.AdmissionPayfee = request.admissionReceipt.AdmissionPayFees;
                        studentrenew.AFeeDiscount = request.admissionReceipt.AdmissionFeeDiscount;
                        studentrenew.PramoteFees = 0;

                        studentrenew.Tution_fee = studentfee.tution_fee;
                        studentrenew.exam_fee = studentfee.exam_fee;
                        studentrenew.Develoment_fee = studentfee.Develoment_fee;
                        studentrenew.Games_fees = studentfee.Games_fees;
                        studentrenew.total = studentfee.total;

                        studentrenew.discount = request.admissionReceipt.FeeDiscount == null ? 0 : request.admissionReceipt.FeeDiscount;
                        studentrenew.total_fee = studentfee.total - request.admissionReceipt.FeeDiscount;
                        studentrenew.due_fee = studentfee.total - request.admissionReceipt.FeeDiscount;
                        studentrenew.stu_fee = 0;
                        studentrenew.OldDuefees = 0;

                    }
                    _context.Student_Renew.Add(studentrenew);
                    await _context.SaveChangesAsync();

                    // installment
                    if ((bool)!studentrenew.RTE)
                    {
                        var installments = await _context.InstallmentTbl
                            .Where(p => p.university_id == studentrenew.ClassId && p.CompanyId == SchoolId && p.SessionId == SessionId)
                            .ToListAsync();

                        foreach (var inst in installments)
                        {
                            var feeInstall = new fee_installment
                            {
                                Id = (_context.fee_installment.DefaultIfEmpty().Max(r => r == null ? 0 : r.Id) + 1),
                                stu_id = student.stu_id,
                                university_id = studentrenew.ClassId,
                                paid_date = DateTime.Now,
                                IntallmentID = inst.InstallmentId,
                                total_fee = studentrenew.total_fee,
                                Installment = inst.Installment,
                                due_fee = inst.FeeAmount,
                                FAmount = inst.FeeAmount,
                                active = true,
                                Date = DateTime.Now,
                                SessionId = SessionId,
                                CompanyId = SchoolId,
                                Userid = UserId,
                            };
                            _context.fee_installment.Add(feeInstall);
                            await _context.SaveChangesAsync();
                        }

                        // fee Receipt
                        if (studentrenew.AdmissionPayfee + studentrenew.PramoteFees > 0)
                        {
                            string ReceiptCode = "";

                            var lastReceipt = await _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId).OrderByDescending(s => s.FDId).FirstOrDefaultAsync();

                            int NewOrderNo = 1;
                            var LastOrderNo = _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).OrderByDescending(s => s.OrderNo)
                                .Select(s => s.OrderNo).FirstOrDefault();

                            if (LastOrderNo != null && LastOrderNo != "")

                                if (!string.IsNullOrEmpty(LastOrderNo))
                                {
                                    int lastNum;
                                    if (int.TryParse(LastOrderNo, out lastNum))
                                    {
                                        NewOrderNo = lastNum + 1;
                                    }
                                }

                            var Id = 0;
                            if (lastReceipt != null)
                            {
                                var Receipt = lastReceipt.ReceiptNo.Split('/');
                                ReceiptCode = Receipt[1];

                                Id = int.Parse(ReceiptCode);
                                Id++;
                            }
                            else
                            {
                                Id = 1;
                            }
                            ReceiptCode = threeLetters + "/" + Id;

                            int FDId = (_context.M_FeeDetail.DefaultIfEmpty().Max(r => r == null ? 0 : r.FDId) + 1);
                            var receipt = new M_FeeDetail
                            {
                               
                                ReceiptNo = ReceiptCode,
                                OrderNo = NewOrderNo.ToString(),
                                OrderStatus = "Succcessfully",
                                TransactionId = "",
                                ReceiptType = "Offline",
                                ClassId = studentrenew.ClassId,
                                stu_id = studentrenew.StuId,
                                Status = "AdmissionPayfee",
                                PayFees = 0,
                                AdmissionFees = studentfee.admission_fee,
                                ExamFees = studentfee.exam_fee,
                                Tutionfee = studentfee.tution_fee,
                                Develoment_fee = studentfee.Develoment_fee,
                                Games_fees = studentfee.Games_fees,
                                FeeTotal = studentfee.total,
                                Discount = 0,
                                OldDuefees = 0,
                                TotalFees = studentfee.total - request.admissionReceipt.FeeDiscount,
                                NetDueFees = studentfee.total - request.admissionReceipt.FeeDiscount,
                                DueFees = studentfee.total - request.admissionReceipt.FeeDiscount,
                                Date = DateTime.Now.Date,
                                RTS = DateTime.Now.Date,
                                PaymentDate = DateTime.Now.Date,

                                AdmissionPayfee = studentrenew.AdmissionPayfee,
                                AFeeDiscount = studentrenew.AFeeDiscount,
                                PramoteFees = studentrenew.PramoteFees,
                                Cash = 0,
                                Upi = 0,
                                Remark = "",
                                Active = true,
                                CompanyId = SchoolId,
                                Userid = UserId,
                                SessionId = SessionId,
                            };

                            ReceiptId = FDId;

                            _context.M_FeeDetail.Add(receipt);
                            await _context.SaveChangesAsync();
                        }
                    }


                    //if (studentrenew.RTE == false)
                    //{
                    //    var installments = _context.InstallmentTbl.Where(p => p.university_id == studentrenew.ClassId && p.CompanyId == SchoolId && p.SessionId == SessionId).ToList();

                    //    for (int i = 0; i < installments.Count; i++)
                    //    {
                    //        fee_installment feeInstall = new fee_installment();
                    //        feeInstall.Id = _context.fee_installment.DefaultIfEmpty().Max(r => r == null ? 0 : r.Id) + 1;

                    //        feeInstall.university_id = studentrenew.ClassId;
                    //        feeInstall.FAmount = installments[i].FeeAmount;
                    //        feeInstall.Installment = installments[i].Installment;
                    //        feeInstall.stu_id = student.stu_id;
                    //        feeInstall.SessionId = SessionId;
                    //        feeInstall.CompanyId = SchoolId;
                    //        feeInstall.Userid = UserId;

                    //        _context.fee_installment.Add(feeInstall);
                    //        await _context.SaveChangesAsync();
                    //    }

                    //    if (studentrenew.AdmissionPayfee + studentrenew.PramoteFees > 0)
                    //    {
                    //        string ReceiptCode = "";

                    //        var lastReceipt = await _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId).OrderByDescending(s => s.FDId).FirstOrDefaultAsync();

                    //        int NewOrderNo = 1;
                    //        var LastOrderNo = _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).OrderByDescending(s => s.OrderNo)
                    //            .Select(s => s.OrderNo).FirstOrDefault();

                    //        if (LastOrderNo != null && LastOrderNo != "")

                    //            if (!string.IsNullOrEmpty(LastOrderNo))
                    //            {
                    //                int lastNum;
                    //                if (int.TryParse(LastOrderNo, out lastNum))
                    //                {
                    //                    NewOrderNo = lastNum + 1;
                    //                }
                    //            }

                    //        var Id = 0;
                    //        if (lastReceipt != null)
                    //        {
                    //            var Receipt = lastReceipt.ReceiptNo.Split('/');
                    //            ReceiptCode = Receipt[1];

                    //            Id = int.Parse(ReceiptCode);
                    //            Id++;
                    //        }
                    //        else
                    //        {
                    //            Id = 1;
                    //        }
                    //        ReceiptCode = threeLetters + "/" + Id;

                    //        int FDId = (_context.M_FeeDetail.DefaultIfEmpty().Max(r => r == null ? 0 : r.FDId) + 1);
                    //        var receipt = new M_FeeDetail
                    //        {
                    //            // ReceiptNo = lastReceipt == null ? "1" : (Convert.ToInt32(lastReceipt.ReceiptNo) + 1).ToString(),
                    //            ReceiptNo = ReceiptCode,
                    //            OrderNo = NewOrderNo.ToString(),
                    //            OrderStatus = "Succcessfully",
                    //            TransactionId = "",
                    //            ReceiptType = "Offline",
                    //            ClassId = studentrenew.ClassId,
                    //            stu_id = studentrenew.StuId,
                    //            Status = "AdmissionPayfee",
                    //            PayFees = 0,
                    //            AdmissionFees = studentfee.admission_fee,
                    //            ExamFees = studentfee.exam_fee,
                    //            Tutionfee = studentfee.tution_fee,
                    //            Develoment_fee = studentfee.Develoment_fee,
                    //            Games_fees = studentfee.Games_fees,
                    //            FeeTotal = studentfee.total,
                    //            Discount = 0,
                    //            OldDuefees = 0,
                    //            TotalFees = studentfee.total,
                    //            NetDueFees = studentfee.total,
                    //            DueFees = studentfee.total,
                    //            Date = DateTime.Now.Date,
                    //            RTS = DateTime.Now.Date,
                    //            PaymentDate = DateTime.Now.Date,
                    //          //  PaymentMode = request.p,
                    //            AdmissionPayfee = studentrenew.AdmissionPayfee,
                    //            AFeeDiscount = studentrenew.AFeeDiscount,
                    //            Cash = 0,
                    //            Upi = 0,
                    //            Remark = "",
                    //            Active = true,
                    //            CompanyId = SchoolId,
                    //            Userid = UserId,
                    //            SessionId = SessionId,
                    //        };

                    //        ReceiptId = FDId;

                    //        _context.M_FeeDetail.Add(receipt);
                    //        await _context.SaveChangesAsync();
                    //    }

                    //    //if (studentrenew.AdmissionPayfee + studentrenew.PramoteFees > 0)
                    //    //{
                    //    //    M_FeeDetail RStudentFees = new M_FeeDetail();

                    //    //    RStudentFees.FDId = _context.M_FeeDetail.DefaultIfEmpty().Max(r => r == null ? 0 : r.FDId) + 1;
                    //    //    ReceiptId = RStudentFees.FDId;
                    //    //    int NewOrderNo = 1;
                    //    //    var LastOrderNo = _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).OrderByDescending(s => s.OrderNo)
                    //    //        .Select(s => s.OrderNo).FirstOrDefault();

                    //    //    if (LastOrderNo != null && LastOrderNo != "")

                    //    //        if (!string.IsNullOrEmpty(LastOrderNo))
                    //    //        {
                    //    //            int lastNum;
                    //    //            if (int.TryParse(LastOrderNo, out lastNum))
                    //    //            {
                    //    //                NewOrderNo = lastNum + 1;
                    //    //            }
                    //    //        }

                    //    //    var existingReceiptNos = _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId).OrderByDescending(s => s.FDId).Take(1).FirstOrDefault();

                    //    //    if (existingReceiptNos == null)
                    //    //    {
                    //    //        RStudentFees.ReceiptNo = "1";
                    //    //    }
                    //    //    else
                    //    //    {
                    //    //        int rno = Convert.ToInt32(existingReceiptNos.ReceiptNo) + 1;
                    //    //        RStudentFees.ReceiptNo = rno.ToString();
                    //    //    }
                    //    //    RStudentFees.ClassId = studentrenew.ClassId;
                    //    //    RStudentFees.stu_id = studentrenew.StuId;
                    //    //    RStudentFees.OrderNo = NewOrderNo.ToString();
                    //    //    RStudentFees.OrderStatus = "Succcessfully";
                    //    //    RStudentFees.TransactionId = "";
                    //    //    RStudentFees.ReceiptType = "Offline";

                    //    //    RStudentFees.Remark = "";
                    //    //    RStudentFees.Active = true;
                    //    //    RStudentFees.Status = "AdmissionPayFee";
                    //    //    RStudentFees.CompanyId = SchoolId;
                    //    //    RStudentFees.Userid = UserId;
                    //    //    RStudentFees.SessionId = SessionId;
                    //    //    RStudentFees.Date = DateTime.Now.Date;

                    //    //    RStudentFees.PaymentMode = request.admissionReceipts.PaymentMode;
                    //    //    RStudentFees.PayFees = studentrenew.AdmissionPayfee + studentrenew.PramoteFees;
                    //    //    RStudentFees.PaymentDate = DateTime.Now.Date;
                    //    //    RStudentFees.PayFees = studentrenew.AdmissionPayfee + studentrenew.PramoteFees;
                    //    //    RStudentFees.Cash = 0;
                    //    //    RStudentFees.Upi = 0;

                    //    //    _context.M_FeeDetail.Add(RStudentFees);
                    //    //    await _context.SaveChangesAsync();

                    //    //}
                    //}


                    await transaction.CommitAsync();
                    var result = new quickadmissionres
                    {
                        StudentId = student.stu_id,
                        ReceiptId = ReceiptId
                    };
                    return ApiResponse<quickadmissionres>.SuccessResponse(result, "Student Add successful");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return ApiResponse<quickadmissionres>.ErrorResponse("Error: " + ex.Message);
                }
            }
        }


        public async Task<ApiResponse<quickadmissionres>> updatestudentdata(StudentUpdateReqModel request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            {
                try
                {
                    int SchoolId = _loginUser.SchoolId;
                    int UserId = _loginUser.UserId;
                    int SessionId = _loginUser.SessionId;
                    int ReceiptId = 0;

                    var duplicateStudent = await _context.student_admission.FirstOrDefaultAsync(p => p.registration_no == request.srNo && p.CompanyId == SchoolId && p.stu_id != request.studentId);

                    if (duplicateStudent != null)
                        return ApiResponse<quickadmissionres>.ErrorResponse("Student already exists");

                    var student = await _context.student_admission.FirstOrDefaultAsync(p => p.stu_id == request.studentId && p.CompanyId == SchoolId);
                    if (student == null)
                        return ApiResponse<quickadmissionres>.ErrorResponse("Student not found");

                    int parentid = 0;

                    ParentsTbl parent = await _context.ParentsTbl.Where(r => (r.ParentsId == request.parentid || r.FatherMobileNo == request.fathermobileno) && r.CompanyId == SchoolId).FirstOrDefaultAsync();

                    if (parent != null)
                    {
                        ParentsTbl parentex = await _context.ParentsTbl.Where(r => r.ParentsId != student.ParentsId && r.FatherMobileNo == request.fathermobileno && r.CompanyId == SchoolId).FirstOrDefaultAsync();

                        if (parentex == null)
                        {
                            parent.FatherMobileNo = request.fathermobileno;
                            parent.Username = request.fathermobileno;
                            parent.Password = request.fathermobileno;
                        }
                        //parentid = parent.ParentsId;
                        parent.FatherName = request.fathername;
                        parent.FatherIncome = request.fatherIncome;
                        parent.FatherOccupation = request.fatherOP;
                        parent.MotherName = request.mothername;
                        parent.MotherMobileNo = request.mothermobileno;
                        parent.MotherOccupation = request.motherOP;
                        parent.MotherIncome = request.motherIncome;
                        parent.GuardianName = request.guardianName;
                        parent.GuardianMobileNo = request.guardianMobileNo;
                        _context.SaveChangesAsync();
                    }
                    //else
                    //{
                    //    var parentdata = await _context.ParentsTbl.Where(p => p.FatherMobileNo == request.fathermobileno && p.CompanyId == SchoolId).FirstOrDefaultAsync();
                    //    if (parentdata == null)
                    //    {
                    //        var parentmodel = new ParentsTbl
                    //        {
                    //        //    ParentsId = (_context.ParentsTbl.DefaultIfEmpty().Max(r => r == null ? 0 : r.ParentsId) + 1),
                    //            FatherName = request.fathername,
                    //            Username = request.fathermobileno,
                    //            Password = request.dob.Value.ToString("yyyyMMdd"),
                    //            FatherMobileNo = request.fathermobileno,
                    //            MotherName = request.mothername,
                    //            CreateDate = DateTime.Now,
                    //            UpdateDate = DateTime.Now,
                    //            SessionId = SessionId,
                    //            CompanyId = SchoolId,
                    //            UserId = UserId
                    //        };
                    //        parentid = parentmodel.ParentsId;
                    //        _context.ParentsTbl.Add(parentmodel);
                    //        await _context.SaveChangesAsync();

                    //    }
                    //    else
                    //    {
                    //        parentid = parentdata.ParentsId;
                    //    }

                    //}

                    var allSchoolDataRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", "ALLSchoolData");
                  
                    var schoolFolderPath = Path.Combine(allSchoolDataRoot, SchoolId.ToString());

                   
                    if (!Directory.Exists(schoolFolderPath))
                    {
                        Directory.CreateDirectory(schoolFolderPath);
                    }

                    if (request.stuphoto != null)
                    {
                        string photoFolder = Path.Combine(schoolFolderPath, "StudentPhoto");
                        student.stu_photo = await SaveStudentFileAsync(request.stuphoto, photoFolder, student.stu_id, SchoolId.ToString(), "StudentPhoto");
                    }
                    if (request.stuaadhar != null)
                    {
                        string photoFolder = Path.Combine(schoolFolderPath, "Aadhar");
                        student.stu_aadhar = await SaveStudentFileAsync(request.stuaadhar, photoFolder, student.stu_id, SchoolId.ToString(), "Aadhar");
                    }
                    if (request.stubirth != null)
                    {
                        string photoFolder = Path.Combine(schoolFolderPath, "stubirth");
                        student.stu_birth = await SaveStudentFileAsync(request.stubirth, photoFolder, student.stu_id, SchoolId.ToString(), "stubirth");
                    }
                    if (request.fatheraadhar != null)
                    {
                        string photoFolder = Path.Combine(schoolFolderPath, "father_aadhar");
                        student.father_aadhar = await SaveStudentFileAsync(request.fatheraadhar, photoFolder, student.stu_id, SchoolId.ToString(), "father_aadhar");
                    }
                    if (request.motheraadhar != null)
                    {
                        string photoFolder = Path.Combine(schoolFolderPath, "mother_aadhar");
                        student.mother_aadhar = await SaveStudentFileAsync(request.motheraadhar, photoFolder, student.stu_id, SchoolId.ToString(), "mother_aadhar");
                    }
                    if (request.IncomeCertificate != null)
                    {
                        string photoFolder = Path.Combine(schoolFolderPath, "IncomeCertificate");
                        student.Income_Certificate = await SaveStudentFileAsync(request.IncomeCertificate, photoFolder, student.stu_id, SchoolId.ToString(), "IncomeCertificate");
                    }
                    if (request.JanAadhar != null)
                    {
                        string photoFolder = Path.Combine(schoolFolderPath, "JanAadhar");
                        student.Jan_Aadhar = await SaveStudentFileAsync(request.JanAadhar, photoFolder, student.stu_id, SchoolId.ToString(), "JanAadhar");
                    }
                    if (request.LastMarkSheetPhotos != null)
                    {
                        string photoFolder = Path.Combine(schoolFolderPath, "LastMarkSheetPhoto");
                        student.LastMarkSheetPhoto = await SaveStudentFileAsync(request.LastMarkSheetPhotos, photoFolder, student.stu_id, SchoolId.ToString(), "LastMarkSheetPhoto");
                    }
                    if (request.studentTcfile != null)
                    {
                        string tcFolder = Path.Combine(schoolFolderPath, "LastStudentTC");
                        student.stu_tc = await SaveStudentFileAsync(request.studentTcfile, tcFolder, student.stu_id, SchoolId.ToString(), "LastStudentTC");
                    }

                    student.admission_date = request.admission_date;
                    student.registration_no = request.srNo;
                    student.stu_name = request.stu_name;
                    student.DOB = request.dob;
                    student.gender = request.gender;
                    student.email = request.email;
                    student.Religion = request.religion;
                    student.cast_category = request.cast_category;
                    student.blood_group = request.blood_group;
                    student.Caste = request.caste;
                    student.ParentsId = parentid;

                    student.address = request.address;
                    student.district = request.district;
                    student.city = request.city;
                    student.state = request.state;
                    student.pincode = request.pincode;
                    student.p_address = request.p_address;
                    student.p_district = request.p_district;
                    student.p_city = request.p_city;
                    student.p_state = request.p_state;
                    student.p_pincode = request.p_pincode;

                    student.LastSchlName = request.lastSchlName;
                    student.LastClass = request.lastClass;
                    student.LastExanTotalMarks = request.lastExanTotalMarks;
                    student.LastDivision = request.lastDivision;
                    student.LastParecentage = request.lastParecentage;
                    student.LastRemarks = request.lastRemarks;

                    student.StuDetail = true;
                    student.StuFee = true;
                    student.Userid = UserId;
                    _context.SaveChanges();

                    Student_Renew studentrenew = _context.Student_Renew.Where(r => r.StuId == student.stu_id && r.ClassId == request.classId && r.CompanyId == SchoolId && r.SessionId == SessionId).FirstOrDefault();
                    var studentfee = _context.fees.Where(p => p.university_id == request.classId && p.CompanyId == SchoolId && p.SessionId == SessionId).FirstOrDefault();


                    studentrenew.ClassId = request.classId == null ? studentrenew.ClassId : request.classId;
                    studentrenew.RTE = request.rte == false ? false : true;
                    //studentrenew.RollNo = request.rollNo == null ? "" : request.rollNo;
                    studentrenew.SectionId = request.sectionId == 0 ? 0 : request.sectionId;
                    studentrenew.ParentsId = parentid;
                    if (studentrenew.RTE == true)
                    {
                        studentrenew.admission_fee = 0;
                        studentrenew.PramoteFees = 0;
                        studentrenew.AFeeDiscount = 0;
                        studentrenew.AdmissionPayfee = 0;
                        studentrenew.exam_fee = 0;
                        studentrenew.Tution_fee = 0;
                        studentrenew.Develoment_fee = 0;
                        studentrenew.Games_fees = 0;
                        studentrenew.total = 0;
                        studentrenew.discount = 0;
                        studentrenew.OldDuefees = 0;
                        studentrenew.total_fee = 0;
                    }
                    else
                    {
                        studentrenew.admission_fee = studentfee.admission_fee;
                        studentrenew.PramoteFees = request.admissionReceipt.pramoteFees ?? 0;
                        studentrenew.AFeeDiscount = request.admissionReceipt.AdmissionFeeDiscount == null ? 0 : request.admissionReceipt.AdmissionFeeDiscount;
                        studentrenew.AdmissionPayfee = studentfee.admission_fee - studentrenew.AFeeDiscount;
                        studentrenew.exam_fee = studentfee.exam_fee;
                        studentrenew.Tution_fee = studentfee.tution_fee;
                        studentrenew.Develoment_fee = studentfee.Develoment_fee;
                        studentrenew.Games_fees = studentfee.Games_fees;
                        studentrenew.total = studentfee.total;
                        studentrenew.discount = request.admissionReceipt.FeeDiscount ?? 0;
                        //   studentrenew.OldDuefees = request.admissionReceipt.oldDuefees ?? 0;
                        studentrenew.total_fee = (studentrenew.total ?? 0) - (studentrenew.discount ?? 0) + (studentrenew.OldDuefees ?? 0);
                    }

                    studentrenew.Userid = UserId;
                    studentrenew.UpdateDate = DateTime.Now;
                    _context.SaveChanges();

                    var fee_installmenttbl = _context.fee_installment.Where(c => c.stu_id == studentrenew.StuId && c.university_id == studentrenew.ClassId && c.CompanyId == SchoolId && c.SessionId == SessionId).ToList();
                    if (fee_installmenttbl != null)
                    {
                        _context.fee_installment.RemoveRange(fee_installmenttbl);
                        _context.SaveChanges();
                    }

                    if (studentrenew.RTE == false)
                    {
                        for (int i = 0; i < request.feeInstallments.Count; i++)
                        {
                            fee_installment feeInstall = new fee_installment();
                            feeInstall.Id = _context.fee_installment.DefaultIfEmpty().Max(r => r == null ? 0 : r.Id) + 1;
                            feeInstall.university_id = studentrenew.ClassId;
                            feeInstall.FAmount = request.feeInstallments[i].SInsAmount;
                            feeInstall.stu_id = student.stu_id;
                            feeInstall.SessionId = SessionId;
                            feeInstall.CompanyId = SchoolId;
                            feeInstall.Userid = UserId;
                            //      feeInstall.CreateDate = DateTime.Now;
                            //      feeInstall.UpdateDate = DateTime.Now;

                            _context.fee_installment.Add(feeInstall);
                            _context.SaveChanges();
                        }

                        M_FeeDetail RStudentFeesU = await _context.M_FeeDetail.Where(r => r.stu_id == studentrenew.StuId && r.CompanyId == SchoolId && r.ClassId == studentrenew.ClassId && r.SessionId == SessionId && r.Status == "AdmissionPayFee").FirstOrDefaultAsync();

                        if (studentrenew.AdmissionPayfee + studentrenew.PramoteFees > 0)
                        {
                            if (RStudentFeesU != null)
                            {
                                ReceiptId = RStudentFeesU.FDId;
                                RStudentFeesU.Userid = UserId;
                                //  RStudentFeesU.UpdateDate = DateTime.Now;
                                RStudentFeesU.PaymentDate = DateTime.Now;
                                RStudentFeesU.PaymentMode = request.admissionReceipt.PaymentMode;
                                RStudentFeesU.PayFees = (studentrenew.AdmissionPayfee ?? 0) + (studentrenew.PramoteFees ?? 0);
                                RStudentFeesU.Cash = 0;
                                RStudentFeesU.Upi = 0;
                                _context.SaveChanges();
                            }
                            else
                            {
                                M_FeeDetail RStudentFees = new M_FeeDetail();

                                RStudentFees.FDId = _context.M_FeeDetail.DefaultIfEmpty().Max(r => r == null ? 0 : r.FDId) + 1;
                                var existingReceiptNos = _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId).OrderByDescending(s => s.FDId).Take(1).FirstOrDefault();

                                if (existingReceiptNos == null)
                                {
                                    RStudentFees.ReceiptNo = "1";
                                }
                                else
                                {
                                    int rno = Convert.ToInt32(existingReceiptNos.ReceiptNo) + 1;
                                    RStudentFees.ReceiptNo = rno.ToString();
                                }
                                ReceiptId = RStudentFees.FDId;

                                RStudentFees.ClassId = studentrenew.ClassId;
                                RStudentFees.stu_id = studentrenew.StuId;
                                RStudentFees.Status = "";
                                RStudentFees.Remark = "";
                                RStudentFees.Active = true;
                                RStudentFees.Status = "AdmissionPayFee";
                                RStudentFees.CompanyId = SchoolId;
                                RStudentFees.Userid = UserId;
                                RStudentFees.SessionId = SessionId;
                                RStudentFees.Date = DateTime.Now.Date;
                                //   RStudentFees.CreateDate = DateTime.Now;
                                //   RStudentFees.UpdateDate = DateTime.Now;
                                RStudentFees.PaymentDate = DateTime.Now;
                                RStudentFees.PaymentMode = request.admissionReceipt.PaymentMode;
                                RStudentFees.PayFees = (studentrenew.AdmissionPayfee ?? 0) + (studentrenew.PramoteFees ?? 0);
                                RStudentFees.Cash = 0;
                                RStudentFees.Upi = 0;

                                _context.M_FeeDetail.Add(RStudentFees);
                                _context.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        M_FeeDetail M_FeeDetail = new M_FeeDetail();
                        M_FeeDetail = _context.M_FeeDetail.Where(r => r.stu_id == studentrenew.StuId && r.ClassId == studentrenew.ClassId && r.CompanyId == SchoolId && r.SessionId == SessionId && r.Status == "AdmissionPayFee").FirstOrDefault();
                        if (M_FeeDetail != null)
                        {
                            M_FeeDetail.Active = false;
                            _context.SaveChangesAsync();
                            //_context.M_FeeDetail.Remove(M_FeeDetail);
                        }
                    }

                    await transaction.CommitAsync();
                    var result = new quickadmissionres
                    {
                        StudentId = student.stu_id,
                        ReceiptId = ReceiptId
                    };
                    return ApiResponse<quickadmissionres>.SuccessResponse(result, "Update successful");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return ApiResponse<quickadmissionres>.ErrorResponse("Error: " + ex.Message);
                }
            }
        }

        private async Task<string> SaveStudentFileAsync(IFormFile file, string folderPath, int studentId, string schoolId, string subFolderName)
        {
            if (file == null || file.Length == 0)
                return null;

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            //var extension = Path.GetExtension(file.FileName);
            //var fileName = $"{studentId}{extension}";
            var extension = ".png"; // 👈 Force PNG
            var fileName = $"{studentId}{extension}";
            var fileNameWithoutExt = studentId.ToString();
            var filePath = Path.Combine(folderPath, fileName);

            var existingFiles = Directory.GetFiles(folderPath, fileNameWithoutExt + ".*");
            foreach (var existing in existingFiles)
            {
                if (System.IO.File.Exists(existing))
                    System.IO.File.Delete(existing);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            //return $"Image/ALLSchoolData/{schoolId}/{subFolderName}/{fileName}";
            return fileName;
        }
        public async Task<ApiResponse<bool>> studentexcelupload(List<StudentExcelUploadListReq> excelList)
        {
            int SchoolId = _loginUser.SchoolId;
            int UserId = _loginUser.UserId;
            int SessionId = _loginUser.SessionId;

            List<student_admission> studentsToInsert = new();
            List<ParentsTbl> parentsToInsert = new();
            List<Student_Renew> renewsToInsert = new();
            List<fee_installment> installmentsToInsert = new();
            List<M_FeeDetail> receiptsToInsert = new();

            foreach (var req in excelList)
            {
                // Skip if already exists
                var exists = await _context.student_admission.AnyAsync(x => x.registration_no == req.SRNo && x.CompanyId == SchoolId);
                if (exists)
                    continue;

                int classid = await _context.University.Where(c => c.CompanyId == SchoolId && c.university_name == req.ClassName && c.Active == true).Select(c => c.university_id).FirstOrDefaultAsync();
                if (classid == null || classid == 0)
                    continue;

                int sectionid = await _context.collegeinfo.Where(c => c.CompanyId == SchoolId && c.collegename == req.SectionName && c.active == true).Select(c => c.collegeid).FirstOrDefaultAsync();
                if (sectionid == null || sectionid == 0)
                    continue;
                //// Get fees
                var studentfee = await _context.fees.FirstOrDefaultAsync(x =>
                    x.university_id == classid && x.CompanyId == SchoolId && x.SessionId == SessionId && x.active == true);
                if (studentfee == null)
                    continue;

                //// Installments
                var feeInstallments = await _context.InstallmentTbl
                    .Where(x => x.university_id == classid && x.CompanyId == SchoolId && x.SessionId == SessionId)
                    .ToListAsync();
                //if (!feeInstallments.Any())
                //    continue;

                // Parent
                int parentid;
                var parent = await _context.ParentsTbl.FirstOrDefaultAsync(p => p.FatherMobileNo == req.father_mobile && p.CompanyId == SchoolId);
                if (parent == null)
                {
                    parentid = (_context.ParentsTbl.DefaultIfEmpty().Max(r => r == null ? 0 : r.ParentsId));
                    parent = new ParentsTbl
                    {
                        ParentsId = parentid++,
                        FatherName = req.father_name,
                        Username = req.father_mobile,
                        Password = req.DOB.ToString("yyyyMMdd"),
                        FatherMobileNo = req.father_mobile,
                        MotherName = req.mother_name,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        SessionId = SessionId,
                        CompanyId = SchoolId,
                        UserId = UserId
                    };
                    parentsToInsert.Add(parent);
                }
                else
                {
                    parentid = parent.ParentsId;
                }

                // Student
                int maxStudentId = await _context.student_admission.Select(s => (int?)s.stu_id).MaxAsync() ?? 0;
                int studentId = maxStudentId + 1;
                var student = new student_admission
                {
                    stu_id = studentId,
                    ParentsId = parentid,
                    registration_no = req.SRNo,
                    stu_name = req.stu_name,
                    DOB = req.DOB,
                    father_name = req.father_name,
                    father_mobile = req.father_mobile,
                    mother_name = req.mother_name,
                    admission_date = DateTime.Now.Date,
                    StuDetail = false,
                    StuFee = false,
                    date = DateTime.Now,
                    CompanyId = SchoolId,
                    SessionId = SessionId,
                    Userid = UserId
                };
                studentsToInsert.Add(student);

                // Student Renew
                var renewId = (_context.Student_Renew.DefaultIfEmpty().Max(r => r == null ? 0 : r.SRId));
                var renew = new Student_Renew
                {
                    SRId = renewId++,
                    StuId = student.stu_id,
                    ParentsId = parentid,
                    ClassId = classid,
                    SectionId = sectionid,
                    RTE = req.RTE != "0",
                    Active = true,
                    completed = false,
                    //StudentTc = false,
                    CompanyId = SchoolId,
                    Userid = UserId,
                    SessionId = SessionId,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };

                if (renew.RTE == true)
                {
                    renew.admission_fee = 0;
                    renew.PramoteFees = 0;
                    renew.AFeeDiscount = 0;
                    renew.AdmissionPayfee = 0;
                    renew.exam_fee = 0;
                    renew.Tution_fee = 0;
                    renew.Develoment_fee = 0;
                    renew.Games_fees = 0;
                    renew.total = 0;
                    renew.discount = 0;
                    renew.OldDuefees = 0;
                    renew.total_fee = 0;
                }
                else
                {
                    renew.admission_fee = studentfee.admission_fee;
                    renew.PramoteFees = 0;
                    renew.AdmissionPayfee = req.AdmissionPayfee ?? 0;
                    renew.AFeeDiscount = renew.admission_fee - renew.AdmissionPayfee;
                    renew.exam_fee = studentfee.exam_fee;
                    renew.Tution_fee = studentfee.tution_fee;
                    renew.Develoment_fee = studentfee.Develoment_fee;
                    renew.Games_fees = studentfee.Games_fees;
                    renew.total = studentfee.total;
                    renew.discount = req.Discount ?? 0;
                    renew.OldDuefees = req.OldDuefees ?? 0;
                    renew.total_fee = studentfee.total - renew.discount + renew.OldDuefees;
                }
                renewsToInsert.Add(renew);

                if (renew.RTE == false)
                {
                    if (feeInstallments != null)
                    {
                        int nextFIId = _context.fee_installment.DefaultIfEmpty().Max(r => r == null ? 0 : r.Id) + 1;
                        foreach (var inst in feeInstallments)
                        {
                            installmentsToInsert.Add(new fee_installment
                            {
                                Id = nextFIId++,
                                university_id = renew.ClassId,
                                FAmount = inst.FeeAmount,
                                stu_id = student.stu_id,
                                SessionId = SessionId,
                                CompanyId = SchoolId,
                                Userid = UserId,
                            });
                        }
                    }

                    if (renew.AdmissionPayfee > 0)
                    {
                        var lastReceipt = await _context.M_FeeDetail
                            .Where(s => s.CompanyId == SchoolId)
                            .OrderByDescending(s => s.FDId)
                            .FirstOrDefaultAsync();
                        int frid = (_context.M_FeeDetail.DefaultIfEmpty().Max(r => r == null ? 0 : r.FDId));
                        receiptsToInsert.Add(new M_FeeDetail
                        {
                            FDId = frid++,
                            ReceiptNo = lastReceipt == null ? "1" : (Convert.ToInt32(lastReceipt.ReceiptNo) + 1).ToString(),
                            ClassId = renew.ClassId,
                            stu_id = student.stu_id,
                            Remark = "",
                            Active = true,
                            Status = "AdmissionPayFee",
                            CompanyId = SchoolId,
                            Userid = UserId,
                            SessionId = SessionId,
                            Date = DateTime.Now,
                            PaymentDate = req.PaymentDate,
                            PaymentMode = req.PaymentMode,
                            PayFees = renew.AdmissionPayfee,
                            Cash = 0,
                            Upi = 0
                        });
                    }
                }
            }

            // ✅ Bulk Insert Now
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (parentsToInsert.Any())
                    await _context.AddRangeAsync(parentsToInsert);

                if (studentsToInsert.Any())
                    await _context.AddRangeAsync(studentsToInsert);

                if (renewsToInsert.Any())
                    await _context.AddRangeAsync(renewsToInsert);

                if (installmentsToInsert.Any())
                    await _context.AddRangeAsync(installmentsToInsert);

                if (receiptsToInsert.Any())
                    await _context.AddRangeAsync(receiptsToInsert);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return ApiResponse<bool>.SuccessResponse(true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }


        //  Student Bulk Edit
        public async Task<ApiResponse<List<StudentRollNoResponse>>> ShowStudentBulkEdit(BulkStudentReq request)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var students = await _context.StudentRenewView.Where(c => (request.ClassId == 0 ? true : c.ClassId == request.ClassId)
                                && (request.SectionId == -1 ? true : c.SectionId == request.SectionId)
                                && c.RActive == true && c.StuDetail == true && c.StuFees == true && c.SessionId == SessionId && c.CompanyId == SchoolId)
                            .OrderBy(c => c.stu_name).Select(c => new StudentRollNoResponse
                            {
                                StudentId = c.StuId,
                                ClassId = c.ClassId,
                                SectionId = c.SectionId,
                                StudentName = c.stu_name,
                                StudentPhoto = c.stu_photo,
                                DateOfBirth = c.DOB,
                                Attendance = c.Attendance,
                                SRNo = c.registration_no,
                                RollNo = c.RollNo,
                                ClassName = _context.University.Where(p => p.university_id == c.ClassId && p.CompanyId == SchoolId).Select(p => p.university_name).FirstOrDefault(),
                                Sectionname = _context.collegeinfo.Where(s => s.collegeid == c.SectionId && s.CompanyId == SchoolId).Select(s => s.collegename).FirstOrDefault()


                            }).ToListAsync();
                if (students == null || !students.Any())
                {
                    return ApiResponse<List<StudentRollNoResponse>>.ErrorResponse("No students found matching the criteria");
                }

                return ApiResponse<List<StudentRollNoResponse>>.SuccessResponse(students, "Fetched Student List successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<StudentRollNoResponse>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateBulkStudentAsync(List<studentRollNoAttendaceReq> request)
        {
            if (request == null || !request.Any())
            {
                return ApiResponse<bool>.ErrorResponse("No student data provided");
            }

            int SchoolId = _loginUser.SchoolId;
            int SessionId = _loginUser.SessionId;
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (request == null || request.Count == 0)
                    {
                        return ApiResponse<bool>.ErrorResponse("No student data provided");
                    }

                    if (request != null && request.Count > 0)
                    {
                        for (int i = 0; i < request.Count; i++)
                        {
                            var currentStuRollNo = request[i];
                            var studentRenew = _context.Student_Renew.Where(p => p.StuId == currentStuRollNo.StudentId && p.SessionId == SessionId && p.CompanyId == SchoolId).FirstOrDefault();

                            studentRenew.RollNo = currentStuRollNo.RollNo;
                            studentRenew.Attendance = currentStuRollNo.Attendance;
                            studentRenew.SectionId = currentStuRollNo.SectionId;
                            await _context.SaveChangesAsync();
                        }
                    }

                    await transaction.CommitAsync();
                    return ApiResponse<bool>.SuccessResponse(true, "Successfully updated roll numbers for students");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
                }
            }
        }


        // student fee discount
        public async Task<ApiResponse<List<StudentDiscountFeeResponse>>> ShowStudentFeeDiscont(BulkStudentReq request)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var students = await _context.StudentRenewView.Where(c => (request.ClassId == 0 ? true : c.ClassId == request.ClassId)
                                && (request.SectionId == -1 ? true : c.SectionId == request.SectionId)
                                && c.RActive == true && c.StuDetail == true && c.StuFees == true && c.SessionId == SessionId && c.CompanyId == SchoolId)
                            .OrderBy(c => c.stu_name).Select(c => new StudentDiscountFeeResponse
                            {
                                StudentId = c.StuId,
                                StudentName = c.stu_name,
                                StudentPhoto = c.stu_photo,
                                ClassId = c.ClassId,
                                SectionId = c.SectionId,
                                RollNo = c.RollNo,
                                RTE = c.RTE,
                                SRNo = c.registration_no,
                                total_fee = c.total_fee,
                                discount = c.discount,
                                ClassName = _context.University.Where(p => p.university_id == c.ClassId && p.CompanyId == SchoolId).Select(p => p.university_name).FirstOrDefault(),
                                SectionName = _context.collegeinfo.Where(s => s.collegeid == c.SectionId && s.CompanyId == SchoolId).Select(s => s.collegename).FirstOrDefault()


                            }).ToListAsync();
                if (students == null || !students.Any())
                {
                    return ApiResponse<List<StudentDiscountFeeResponse>>.ErrorResponse("No students found matching the criteria");
                }

                return ApiResponse<List<StudentDiscountFeeResponse>>.SuccessResponse(students, "Fetched Student List successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<StudentDiscountFeeResponse>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddStudentDiscountFee(List<studentDiscountfeeReq> request)
        {
            if (request == null || !request.Any())
            {
                return ApiResponse<bool>.ErrorResponse("No student data provided");
            }

            int SchoolId = _loginUser.SchoolId;
            int UserId = _loginUser.UserId;
            int SessionId = _loginUser.SessionId;
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    if (request == null || request.Count == 0)
                    {
                        return ApiResponse<bool>.ErrorResponse("No student data provided");
                    }

                    if (request != null && request.Count > 0)
                    {
                        for (int i = 0; i < request.Count; i++)
                        {
                            var StudentFeeDiscont = request[i];
                            var studentRenew = _context.Student_Renew.Where(p => p.StuId == StudentFeeDiscont.StudentId && p.SessionId == SessionId && p.CompanyId == SchoolId && p.Active == true).FirstOrDefault();
                            if (studentRenew.RTE == false)
                            {
                                var classfee = _context.fees.Where(f => f.CompanyId == SchoolId && f.SessionId == SessionId && f.university_id == studentRenew.ClassId && f.active == true).FirstOrDefault();

                                if (studentRenew != null)

                                {
                                    studentRenew.admission_fee = classfee.admission_fee;
                                    studentRenew.Tution_fee = classfee.tution_fee;
                                    studentRenew.exam_fee = classfee.exam_fee;
                                    studentRenew.Develoment_fee = classfee.Develoment_fee;
                                    studentRenew.Games_fees = classfee.Games_fees;
                                    studentRenew.total = classfee.total;
                                    studentRenew.discount = StudentFeeDiscont.discount;
                                    studentRenew.total_fee = (studentRenew.total + studentRenew.OldDuefees) - studentRenew.discount;

                                    await _context.SaveChangesAsync();
                                }
                            }
                            var FeeInstalment = _context.fee_installment.Where(c => c.stu_id == studentRenew.StuId && c.CompanyId == SchoolId && c.SessionId == SessionId).ToList();
                            _context.fee_installment.RemoveRange(FeeInstalment);
                            await _context.SaveChangesAsync();

                            var newinstallment = _context.InstallmentTbl.Where(p => p.university_id == studentRenew.ClassId && p.SessionId == SessionId && p.CompanyId == SchoolId).ToList();
                            if (newinstallment != null && newinstallment.Count > 0)
                            {
                                double remainingFee = (double)studentRenew.total_fee / newinstallment.Count;

                                for (int l = 0; l < newinstallment.Count; l++)
                                {
                                    fee_installment feeInstall = new fee_installment();
                                    feeInstall.Id = _context.fee_installment.DefaultIfEmpty().Max(r => r == null ? 0 : r.Id) + 1;

                                    feeInstall.university_id = studentRenew.ClassId;
                                    feeInstall.stu_id = studentRenew.StuId;
                                    feeInstall.FAmount = remainingFee;
                                    feeInstall.Installment = newinstallment[l].Installment;
                                    feeInstall.SessionId = SessionId;
                                    feeInstall.CompanyId = SchoolId;
                                    feeInstall.Userid = UserId;
                                    //      feeInstall.CreateDate = DateTime.Now;
                                    //     feeInstall.UpdateDate = DateTime.Now;

                                    _context.fee_installment.Add(feeInstall);
                                    await _context.SaveChangesAsync();
                                }
                            }
                            var FeeDetail = _context.M_FeeDetail.Where(a => a.stu_id == studentRenew.StuId && a.ClassId == StudentFeeDiscont.ClassId && a.SessionId == SessionId && a.CompanyId == SchoolId && a.Active == true).ToList();

                            if (FeeDetail != null && FeeDetail.Count > 0)
                            {
                                // decimal remainingFee = (decimal)studentRenew.total_fee; 

                                for (int k = 0; k < FeeDetail.Count; k++)
                                {
                                    var FeeDeia = FeeDetail[k].FDId;

                                    M_FeeDetail FeeDetaildata = _context.M_FeeDetail.Where(s => s.FDId == FeeDeia && s.stu_id == studentRenew.StuId && s.CompanyId == SchoolId && s.SessionId == SessionId).FirstOrDefault();

                                    if (FeeDetaildata != null)
                                    {
                                        FeeDetaildata.ReceiptNo = FeeDetail[k].ReceiptNo;
                                        FeeDetaildata.stu_id = FeeDetail[k].stu_id;
                                        FeeDetaildata.ClassId = FeeDetail[k].ClassId;
                                        FeeDetaildata.Status = FeeDetail[k].Status;
                                        FeeDetaildata.PayFees = FeeDetail[k].PayFees;
                                        FeeDetaildata.Cash = FeeDetail[k].Cash;
                                        FeeDetaildata.Upi = FeeDetail[k].Upi;
                                        FeeDetaildata.PaymentMode = FeeDetail[k].PaymentMode;
                                        FeeDetaildata.PaymentDate = FeeDetail[k].PaymentDate;
                                        FeeDetaildata.Remark = FeeDetail[k].Remark;
                                        FeeDetaildata.Date = DateTime.Now;
                                        //       FeeDetaildata.CreateDate = DateTime.Now;
                                        //       FeeDetaildata.UpdateDate = DateTime.Now;

                                        await _context.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                    }

                    await transaction.CommitAsync();
                    return ApiResponse<bool>.SuccessResponse(true, "Successfully Added Discount for students");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
                }
            }
        }


        // student presonality
        public async Task<ApiResponse<List<StudentPersonalResponse>>> ShowStudentPersonality(BulkStudentReq request)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var res = await (from stu in _context.StudentRenewView
                                 join cls in _context.University on stu.ClassId equals cls.university_id into clsJoin
                                 from cls in clsJoin.DefaultIfEmpty()

                                 join sec in _context.collegeinfo on stu.SectionId equals sec.collegeid into secJoin
                                 from sec in secJoin.DefaultIfEmpty()

                                 join per in _context.StuPersonalty
                                 on stu.StuId equals per.StuId into perJoin
                                 from per in perJoin.Where(p => p.CompanyId == SchoolId && p.SessionId == SessionId).DefaultIfEmpty()

                                 where stu.RActive == true && stu.StuDetail == true && stu.StuFees == true && stu.CompanyId == SchoolId
                                     && stu.SessionId == SessionId && (request.ClassId == -1 || stu.ClassId == request.ClassId)
                                     && (request.SectionId == -1 || stu.SectionId == request.SectionId)

                                 orderby stu.stu_name

                                 select new StudentPersonalResponse
                                 {
                                     StudentId = stu.StuId,
                                     ClassId = stu.ClassId,
                                     stu_name = stu.stu_name,
                                     SectionId = stu.SectionId,
                                     ClassName = cls.university_name,
                                     SectionName = sec.collegename,
                                     Direction = per.Direction,
                                     Concentration = per.Concentration,
                                     Discipline = per.Discipline,
                                     Independently = per.Independently,
                                     Intiative = per.Intiative,
                                     Cleanliness = per.Cleanliness,
                                     Etiquette = per.Etiquette,
                                     OtherPro = per.OtherPro,
                                     Passionate = per.Passionate,
                                     Confident = per.Confident,
                                     Responsible = per.Responsible
                                 }).ToListAsync();

                if (res == null || !res.Any())
                    return ApiResponse<List<StudentPersonalResponse>>.ErrorResponse("No students found matching the criteria");

                return ApiResponse<List<StudentPersonalResponse>>.SuccessResponse(res, "Fetch Student Personality successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<StudentPersonalResponse>>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddStudentPersonalAsync(List<stuPersonalModelReq> req)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    int SchoolId = _loginUser.SchoolId;
                    int UserId = _loginUser.UserId;
                    int SessionId = _loginUser.SessionId;

                    if (req != null && req.Count > 0)
                    {
                        for (int i = 0; i < req.Count; i++)
                        {
                            var currentStuPersonal = req[i];

                            var Personal = await _context.StuPersonalty.Where(p => p.CompanyId == currentStuPersonal.StudentId && p.ClassId == currentStuPersonal.ClassId && p.SectionId == currentStuPersonal.SectionId).FirstOrDefaultAsync();

                            if (Personal == null)
                            {
                                Personal = new StuPersonalty
                                {

                                    PersonaltyId = (_context.StuPersonalty.Any() ? _context.StuPersonalty.Max(r => r.PersonaltyId) : 0) + 1,

                                    StuId = currentStuPersonal.StudentId,
                                    ClassId = currentStuPersonal.ClassId,
                                    SectionId = currentStuPersonal.SectionId,
                                    Discipline = currentStuPersonal.Discipline,
                                    Concentration = currentStuPersonal.Concentration,
                                    Intiative = currentStuPersonal.Intiative,
                                    Independently = currentStuPersonal.Independently,
                                    Direction = currentStuPersonal.Direction,
                                    Cleanliness = currentStuPersonal.Cleanliness,
                                    Etiquette = currentStuPersonal.Etiquette,
                                    OtherPro = currentStuPersonal.OtherPro,
                                    Passionate = currentStuPersonal.Passionate,
                                    Confident = currentStuPersonal.Confident,
                                    Responsible = currentStuPersonal.Responsible,

                                    Date = DateTime.Now,
                                    Active = true,
                                    CompanyId = SchoolId,
                                    SessionId = SessionId,
                                    Userid = UserId,
                                };
                                _context.StuPersonalty.Add(Personal);
                                //await _context.SaveChangesAsync();
                            }
                            else
                            {
                                Personal.Discipline = currentStuPersonal.Discipline;
                                Personal.Concentration = currentStuPersonal.Concentration;
                                Personal.Intiative = currentStuPersonal.Intiative;
                                Personal.Independently = currentStuPersonal.Independently;
                                Personal.Direction = currentStuPersonal.Direction;
                                Personal.Cleanliness = currentStuPersonal.Cleanliness;
                                Personal.Etiquette = currentStuPersonal.Etiquette;
                                Personal.OtherPro = currentStuPersonal.OtherPro;
                                Personal.Passionate = currentStuPersonal.Passionate;
                                Personal.Confident = currentStuPersonal.Confident;
                                Personal.Responsible = currentStuPersonal.Responsible;

                            }
                        }
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }

                    return ApiResponse<bool>.SuccessResponse(true, "Student Personality Added Successfully.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
                }
            }
        }


        // exam marks data 
        public async Task<ApiResponse<GetClassbySectionSubject>> GetClassbySectionNdSubject(int ClassId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var Sectiondatas = await _context.collegeinfo.Where(a => a.university_id == ClassId && a.CompanyId == SchoolId)
                    .Select(a => new SectionDataList
                    {
                        ClassId = a.university_id,
                        SectionId = a.collegeid,
                        SectionName = a.collegename,

                    }).ToListAsync();

                var Subjectlist = await _context.Subject.Where(a => a.university_id == ClassId && a.CompanyId == SchoolId)
                    .Select(a => new GetSubjectModel
                    {
                        SubjectId = a.subject_id,
                        SubjectName = a.subject_name,
                    }).ToListAsync();

                var res = new GetClassbySectionSubject
                {
                    SectionData = Sectiondatas,
                    SubjectData = Subjectlist,
                };

                return ApiResponse<GetClassbySectionSubject>.SuccessResponse(res, "Fetch successfully class by section and subject data ");
            }
            catch (Exception ex)
            {
                return ApiResponse<GetClassbySectionSubject>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<GetSubjectModel>> GetMarksType(int SubjectId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var Examtype = await _context.Subject.Where(a => a.subject_id == SubjectId && a.CompanyId == SchoolId)
                    .Select(a => new GetSubjectModel
                    {
                        SubjectId = a.subject_id,
                        SubjectName = a.subject_name,
                        Markstype = a.Marks_Type,

                    }).FirstOrDefaultAsync();

                return ApiResponse<GetSubjectModel>.SuccessResponse(Examtype, "Fetch successfully Subject id by marks type data ");
            }
            catch (Exception ex)
            {
                return ApiResponse<GetSubjectModel>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<StudentexamMarksResponse>>> ShowTestMarksDetails(studentexamMarksReq request)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.StudentRenewView.Where(c =>
                            (request.ClassId == -1 ? false : c.ClassId == request.ClassId) &&
                            (request.SectionId == -1 ? true : c.SectionId == request.SectionId) &&
                            c.RActive == true && c.StuDetail == true && c.StuFees == true && c.Dropout == false && c.SessionId == SessionId &&
                            c.CompanyId == SchoolId).Select(c => new StudentexamMarksResponse
                            {
                                StudentId = c.StuId,
                                stu_name = c.stu_name,
                                ClassId = c.ClassId,
                                SectionId = c.SectionId,
                                SRNo = c.registration_no,
                                Written = _context.TestExamTbl.Where(q => q.university_id == c.ClassId && q.subject_id == request.SubjectId && q.MarksType == request.MatksType
                                    && q.TestType == request.TestType && q.stu_id == c.StuId && q.SessionId == SessionId && q.CompanyId == SchoolId).Select(p => p.Written ?? 0).FirstOrDefault(),
                                Oral = _context.TestExamTbl.Where(q => q.university_id == c.ClassId && q.subject_id == request.SubjectId && q.MarksType == request.MatksType
                                    && q.TestType == request.TestType && q.stu_id == c.StuId && q.SessionId == SessionId && q.CompanyId == SchoolId).Select(p => p.Oral ?? 0).FirstOrDefault(),
                                Pratical = _context.TestExamTbl.Where(q => q.university_id == c.ClassId && q.subject_id == request.SubjectId && q.MarksType == request.MatksType
                                    && q.TestType == request.TestType && q.stu_id == c.StuId && q.SessionId == SessionId && q.CompanyId == SchoolId).Select(p => p.Pratical ?? 0).FirstOrDefault(),
                                Total = _context.TestExamTbl.Where(q => q.university_id == c.ClassId && q.subject_id == request.SubjectId && q.MarksType == request.MatksType
                                    && q.TestType == request.TestType && q.stu_id == c.StuId && q.SessionId == SessionId && q.CompanyId == SchoolId).Select(p => p.Total ?? 0).FirstOrDefault(),

                                MaxTotal = _context.Subject.Where(a => a.university_id == c.ClassId && a.subject_id == request.SubjectId && a.Marks_Type == request.MatksType &&
                                     a.SessionId == SessionId && a.CompanyId == SchoolId).Select(a =>
                                     request.TestType == "Quarterly" ? a.Quarterly : request.TestType == "first_test" ? a.first_test : request.TestType == "second_test" ? a.second_test :
                                      request.TestType == "half_yearly" ? a.half_yearly : request.TestType == "third_test" ? a.third_test : request.TestType == "fourth_test" ? a.fourth_test :
                                      request.TestType == "yearly" ? a.yearly : 0)
                                     .FirstOrDefault()


                            }).OrderBy(c => c.stu_name).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<StudentexamMarksResponse>>.ErrorResponse("No students found matching the criteria");
                }

                return ApiResponse<List<StudentexamMarksResponse>>.SuccessResponse(res, "Fetched Student Marks List successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<StudentexamMarksResponse>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateStudentMarks(UpdateStudentMarksRequest request)
        {
            int SchoolId = _loginUser.SchoolId;
            int UserId = _loginUser.UserId;
            int SessionId = _loginUser.SessionId;
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    if (request.ExamModal != null && request.ExamModal.Count > 0)
                    {
                        for (int i = 0; i < request.ExamModal.Count; i++)
                        {
                            if (request.MatksType == "Marks")
                            {
                                var subject = request.ExamModal[i];
                                var testmatks = await _context.TestExamTbl.Where(q => q.CompanyId == SchoolId && q.university_id == subject.ClassId
                                && q.stu_id == subject.StudentId && q.subject_id == request.SubjectId && q.TestType == request.TestType && q.MarksType == request.MatksType && q.SessionId == SessionId).FirstOrDefaultAsync();

                                if (testmatks == null)
                                {
                                    TestExamTbl subobj = new TestExamTbl
                                    {
                                        TestExamId = _context.TestExamTbl.DefaultIfEmpty().Max(r => r == null ? 0 : r.TestExamId) + 1,
                                        university_id = subject.ClassId,
                                        subject_id = request.SubjectId,
                                        TestType = request.TestType,
                                        MarksType = request.MatksType,
                                        stu_id = subject.StudentId,

                                        Written = subject.Written == null ? 0 : Convert.ToInt64(subject.Written),
                                        Oral = subject.Oral == null ? 0 : Convert.ToInt64(subject.Oral),
                                        Pratical = subject.Pratical == null ? 0 : Convert.ToInt64(subject.Pratical),
                                        Total = subject.Total == null ? 0 : Convert.ToInt64(subject.Total),
                                        Grade = subject.Grade == null ? " " : subject.Grade,
                                        CompanyId = SchoolId,
                                        Userid = UserId,
                                        SessionId = SessionId,

                                    };
                                    _context.TestExamTbl.Add(subobj);
                                    await _context.SaveChangesAsync();
                                }
                                else
                                {
                                    testmatks.Written = subject.Written == null ? 0 : Convert.ToInt64(subject.Written);
                                    testmatks.Oral = subject.Oral == null ? 0 : Convert.ToInt64(subject.Oral);
                                    testmatks.Pratical = subject.Pratical == null ? 0 : Convert.ToInt64(subject.Pratical);
                                    testmatks.Total = subject.Total == null ? 0 : Convert.ToInt64(subject.Total);
                                    testmatks.Grade = subject.Grade == null ? " " : subject.Grade;
                                    await _context.SaveChangesAsync();
                                }

                            }
                            else
                            {
                                var subject = request.ExamModal[i];
                                var testmatks = await _context.TestExamTbl.Where(q => q.CompanyId == SchoolId && q.university_id == subject.ClassId
                                && q.stu_id == subject.StudentId && q.subject_id == request.SubjectId && q.TestType == request.TestType && q.MarksType == request.MatksType && q.SessionId == SessionId).FirstOrDefaultAsync();

                                if (testmatks == null)
                                {
                                    TestExamTbl subobj = new TestExamTbl
                                    {
                                        TestExamId = _context.TestExamTbl.DefaultIfEmpty().Max(r => r == null ? 0 : r.TestExamId) + 1,
                                        university_id = subject.ClassId,
                                        subject_id = request.SubjectId,
                                        stu_id = subject.StudentId,
                                        TestType = request.TestType,
                                        MarksType = request.MatksType,
                                        MGrade = subject.MGrade == null ? " " : subject.MGrade,
                                        CompanyId = SchoolId,
                                        Userid = UserId,
                                        SessionId = SessionId,

                                    };
                                    _context.TestExamTbl.Add(subobj);
                                    await _context.SaveChangesAsync();
                                }
                                else
                                {
                                    testmatks.MGrade = subject.MGrade == null ? " " : subject.MGrade;
                                    testmatks.Grade = subject.Grade == null ? " " : subject.Grade;
                                    await _context.SaveChangesAsync();
                                }
                            }

                        }
                    }

                    await transaction.CommitAsync();
                    return ApiResponse<bool>.SuccessResponse(true, "Successfully updated students marks.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
                }
            }
        }

        // testmarks by excel 
        public async Task<ApiResponse<bool>> ExcelStudentMarks(examMarksmodel request)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                if (request.Marksdata == null || request.Marksdata.Length == 0)
                    return ApiResponse<bool>.ErrorResponse("No file uploaded.");

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", "ExcelUpload");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string fileName = Path.GetFileName(request.Marksdata.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                // Save file to disk
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.Marksdata.CopyToAsync(stream);
                }
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                    //   var SubjMaxMarks = _context.ClassSubjectExamTbl.Where(q => q.SchoolId == SchoolId && q.ExamId == request.ExamId && q.ClassId == request.ClassId && q.SubjectId == request.SubjectId && q.SessionId == SessionId).Select(q => q.MaxMarks).FirstOrDefault();
                    if (worksheet != null)
                    {
                        var marksToAdd = new List<TestExamTbl>();

                        for (int row = 5; row <= worksheet.Dimension.End.Row; row++)
                        {
                            string studentsrno = Convert.ToString(worksheet.Cells[row, 2].Text);
                            string studentname = Convert.ToString(worksheet.Cells[row, 3].Text);

                            int studentid = _context.StudentRenewView.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId && s.registration_no == studentsrno && s.ClassId == request.ClassId && s.RActive == true).Select(s => s.StuId).FirstOrDefault();

                            if (studentid != 0)
                            {
                                var testeaxmdatachk = _context.TestExamTbl.Where(q => q.CompanyId == SchoolId && q.university_id == request.ClassId && q.stu_id == studentid
                                                        && q.subject_id == request.SubjectId && q.TestType == request.TestType && q.SessionId == SessionId && q.MarksType == request.MarksType).FirstOrDefault();
                                if (request.MarksType == "Marks")
                                {
                                    if (testeaxmdatachk == null)
                                    {
                                        long written = string.IsNullOrEmpty(worksheet.Cells[row, 5].Text) ? 0 : Convert.ToInt64(worksheet.Cells[row, 5].Text);
                                        long oral = string.IsNullOrEmpty(worksheet.Cells[row, 6].Text) ? 0 : Convert.ToInt64(worksheet.Cells[row, 6].Text);
                                        long pratical = string.IsNullOrEmpty(worksheet.Cells[row, 7].Text) ? 0 : Convert.ToInt64(worksheet.Cells[row, 7].Text);
                                        long total = written + oral + pratical;

                                        TestExamTbl subobj = new TestExamTbl
                                        {
                                            TestExamId = _context.TestExamTbl.DefaultIfEmpty().Max(r => r == null ? 0 : r.TestExamId) + 1,
                                            university_id = request.ClassId,
                                            subject_id = request.SubjectId,
                                            TestType = request.TestType,
                                            MarksType = request.MarksType,
                                            stu_id = studentid,
                                            Written = written,
                                            Oral = oral,
                                            Pratical = pratical,
                                            Total = total,
                                            CompanyId = SchoolId,
                                            SessionId = SessionId,
                                            Userid = UserId,
                                        };
                                        _context.TestExamTbl.Add(subobj);
                                        await _context.SaveChangesAsync();

                                    }
                                    else
                                    {
                                        testeaxmdatachk.Written = string.IsNullOrEmpty(worksheet.Cells[row, 5].Text) ? 0 : Convert.ToInt64(worksheet.Cells[row, 5].Text);
                                        testeaxmdatachk.Oral = string.IsNullOrEmpty(worksheet.Cells[row, 6].Text) ? 0 : Convert.ToInt64(worksheet.Cells[row, 6].Text);
                                        testeaxmdatachk.Pratical = string.IsNullOrEmpty(worksheet.Cells[row, 7].Text) ? 0 : Convert.ToInt64(worksheet.Cells[row, 7].Text);
                                        testeaxmdatachk.Total = testeaxmdatachk.Written + testeaxmdatachk.Oral + testeaxmdatachk.Pratical;
                                        await _context.SaveChangesAsync();
                                    }
                                }
                                else
                                {
                                    string grade = string.IsNullOrEmpty(worksheet.Cells[row, 4].Text) ? "" : worksheet.Cells[row, 4].Text;

                                    if (grade != "")
                                    {
                                        if (testeaxmdatachk == null)
                                        {
                                            TestExamTbl subobj = new TestExamTbl
                                            {
                                                TestExamId = _context.TestExamTbl.DefaultIfEmpty().Max(r => r == null ? 0 : r.TestExamId) + 1,
                                                university_id = request.ClassId,
                                                subject_id = request.SubjectId,
                                                TestType = request.TestType,
                                                MarksType = request.MarksType,
                                                stu_id = studentid,
                                                Grade = "",
                                                MGrade = string.IsNullOrEmpty(worksheet.Cells[row, 4].Text) ? "" : worksheet.Cells[row, 4].Text,
                                                Active = true,
                                                CompanyId = SchoolId,
                                                SessionId = SessionId,
                                                Userid = UserId,
                                            };

                                            _context.TestExamTbl.Add(subobj);
                                            await _context.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            testeaxmdatachk.Grade = "";
                                            testeaxmdatachk.MGrade = string.IsNullOrEmpty(worksheet.Cells[row, 4].Text) ? "" : worksheet.Cells[row, 4].Text;
                                            await _context.SaveChangesAsync();
                                        }
                                    }
                                }
                            }
                        }
                        if (marksToAdd.Count > 0)
                            _context.TestExamTbl.AddRange(marksToAdd);
                        await _context.SaveChangesAsync();
                    }
                }
                return ApiResponse<bool>.SuccessResponse(true, "Marks uploaded successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }


        // ****************************** Event Code  ****************************** //
        public async Task<ApiResponse<List<UpdateEventReqModel>>> GetEventList()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;

                var EventList = await _context.Event.Where(p => p.CompanyId == SchoolId && p.Active == true)
                    .Select(c => new UpdateEventReqModel
                    {
                        EventId = c.EventID,
                        Eventname = c.EventName,
                    }).ToListAsync();

                return ApiResponse<List<UpdateEventReqModel>>.SuccessResponse(EventList, "Fetch Event  successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<UpdateEventReqModel>>.ErrorResponse("Error: " + ex.Message);

            }
        }
        public async Task<ApiResponse<List<StudentEventCertificate>>> GetEventDataByIdAsync(int EventId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.EventCertificate.Where(c => c.EventId == EventId && c.Active == true
                && c.SessionId == SessionId && c.CompanyId == SchoolId)
                    .Select(c => new StudentEventCertificate
                    {
                        ECId = c.EvId,
                        EventId = c.EventId,
                        EmployeId = c.Emp_Id,
                        StudentId = c.StudentId,
                        ClassId = c.ClassId,
                        SectionId = c.SectionId,
                        RankType = c.RankType,
                        Description = c.Description,
                        IssueDate = c.IssueDate,

                        ClassName = _context.University.Where(p => p.university_id == c.ClassId && p.CompanyId == SchoolId).Select(p => p.university_name).FirstOrDefault(),
                        SectionName = _context.collegeinfo.Where(s => s.collegeid == c.SectionId && s.CompanyId == SchoolId).Select(s => s.collegename).FirstOrDefault(),
                        EventName = _context.Event.Where(u => u.EventID == c.EventId && u.SessionId == SessionId && u.CompanyId == SchoolId).Select(r => r.EventName).FirstOrDefault(),

                        TeacherDetail = _context.EmployeeRegister.Where(a => a.Emp_Id == c.Emp_Id && a.SessionId == SessionId && a.CompanyId == SchoolId)
                        .Select(a => new GetEmployeDataModel
                        {
                            Emp_Name = a.Emp_Name,
                            Father_husband_Name = a.Father_husband_Name,
                        }).FirstOrDefault(),

                        //  TeacherDetail = _context.EmployeeRegisterTbl.Where(u => u.Emp_Id == c.EmployeId && u.SessionId == SessionId && u.SchoolId == SchoolId).Select(u => u.Emp_Name).FirstOrDefault(),

                        StudentDetails = (from stu in _context.StudentRenewView
                                          join prt in _context.ParentsTbl on stu.ParentsId equals prt.ParentsId
                                          where stu.StuId == c.StudentId && stu.SessionId == c.SessionId && stu.CompanyId == c.CompanyId
                                          select new GetStudentDatamodel
                                          {
                                              stu_name = stu.stu_name,
                                              SRNo = stu.registration_no,
                                              ParentsId = stu.ParentsId,
                                              RollNo = stu.RollNo,
                                              FatherName = prt.FatherName,
                                              FatherMobileNo = prt.FatherMobileNo,
                                              MotherName = prt.MotherName,
                                              MotherMobileNo = prt.MotherMobileNo,
                                          }).FirstOrDefault(),

                    }).ToListAsync();

                if (res == null)
                {
                    return ApiResponse<List<StudentEventCertificate>>.ErrorResponse("No Event found ");
                }

                return ApiResponse<List<StudentEventCertificate>>.SuccessResponse(res, "Fetch Event Successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<StudentEventCertificate>>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddEventCertificateAsync(EventCartificateModelReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var existing = await _context.EventCertificate.FirstOrDefaultAsync(p => p.EventId == req.EventId && p.ClassId == req.ClassId
                && p.RankType == req.RankType && p.Emp_Id == req.EmployeId && p.CompanyId == SchoolId && p.SessionId == SessionId);

                if (existing != null)
                {
                    return ApiResponse<bool>.ErrorResponse("This RankType is already assigned for the given EventId, ClassId, and StudentId.");
                }

                int EcId = _context.EventCertificate.DefaultIfEmpty().Max(s => s == null ? 0 : s.EvId) + 1;

                var evententity = new EventCertificate
                {
                    EvId = EcId,
                    EventId = req.EventId,
                    StudentId = req.StudentId,
                    Emp_Id = req.EmployeId,
                    ClassId = req.ClassId,
                    SectionId = req.SectionId,
                    CertificateStatus = req.Status,
                    RankType = req.RankType,
                    Description = req.Description,
                    IssueDate = req.IssueDate,
                    SessionId = SessionId,
                    CompanyId = SchoolId,
                    Userid = UserId,
                    //  CreateDate = DateTime.Now,
                    //   UpdateDate = DateTime.Now,
                    Active = true,

                };
                _context.EventCertificate.Add(evententity);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Student Event Cartificate Careated Successfully");

            }
            catch (Exception ex)
            {

                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<GetClassbySectionNdStudent>> GetClassBySectionNdStudent(int ClassId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var Sectiondatas = await _context.collegeinfo.Where(a => a.university_id == ClassId && a.CompanyId == SchoolId)
                    .Select(a => new SectionDataList
                    {
                        ClassId = a.university_id,
                        SectionId = a.collegeid,
                        SectionName = a.collegename,

                    }).ToListAsync();

                var StudentDatas = await _context.StudentRenewView.Where(a => a.ClassId == ClassId && a.CompanyId == SchoolId)
                    .Select(a => new StudentDataList
                    {
                        ClassId = a.ClassId,
                        StudentId = a.StuId,
                        StudentName = a.stu_name,
                        SectionId = a.SectionId,
                    }).ToListAsync();

                var res = new GetClassbySectionNdStudent
                {
                    SectionData = Sectiondatas,
                    StudentData = StudentDatas,
                };

                return ApiResponse<GetClassbySectionNdStudent>.SuccessResponse(res, "Fetch successfully class by section and student data ");
            }
            catch (Exception ex)
            {
                return ApiResponse<GetClassbySectionNdStudent>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<List<StudentDataList>>> GetSectionByStudentDetail(int ClassId, int SectionId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.StudentRenewView.Where(p => p.ClassId == ClassId && p.SectionId == SectionId && p.CompanyId == SchoolId)
                    .Select(p => new StudentDataList
                    {
                        ClassId = p.ClassId,
                        SectionId = p.SectionId,
                        StudentId = p.StuId,
                        StudentName = p.stu_name,
                    }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<StudentDataList>>.ErrorResponse("No Fetch section by student ");
                }

                return ApiResponse<List<StudentDataList>>.SuccessResponse(res, "Fetch successfully section by student");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<StudentDataList>>.ErrorResponse("Error: " + ex.Message);

            }
        }
        public async Task<ApiResponse<StudentDetailsById>> GetStudentDetailById(int StudentId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var studentDetails = await (from r in _context.StudentRenewView
                                            join p in _context.ParentsTbl on r.ParentsId equals p.ParentsId
                                            where r.StuId == StudentId && r.CompanyId == SchoolId && r.SessionId == SessionId
                                            select new StudentDetailsById
                                            {
                                                StudentName = r.stu_name,
                                                Srno = r.registration_no,
                                                RollNo = r.RollNo,
                                                FatherName = p.FatherName,
                                                MotherName = p.MotherName,
                                                MobileNo = p.FatherMobileNo,
                                            }).FirstOrDefaultAsync();
                if (studentDetails == null)
                    return ApiResponse<StudentDetailsById>.ErrorResponse("Student not found");

                return ApiResponse<StudentDetailsById>.SuccessResponse(studentDetails, "Student details fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<StudentDetailsById>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<StudentFeeTCModel>> GetStudentDueFeeTC(int StudentId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var startDate = await _context.StuRouteAssignTbl.Where(c => c.stu_id == StudentId && c.CompanyId == SchoolId
               && c.SessionId == SessionId).Select(c => c.Date).FirstOrDefaultAsync();

                int startMonthNo = startDate?.Month ?? 1;           // route date ka month number (int)
                string startMonth = new DateTime(DateTime.Now.Year, startMonthNo, 1).ToString("MMMM");

                int currentMonthNo = DateTime.Now.Month;           // current month number (int)
                string currentMonth = new DateTime(DateTime.Now.Year, currentMonthNo, 1).ToString("MMMM");

                var validMonths = Enumerable.Range(startMonthNo, currentMonthNo - startMonthNo + 1).Select(m => new DateTime(DateTime.Now.Year, m, 1).ToString("MMMM")).ToList();

                var res = await _context.StudentRenewView.Where(a => a.StuId == StudentId && a.CompanyId == SchoolId && a.SessionId == SessionId)
                    .Select(a => new StudentFeeTCModel
                    {
                        Srno = a.registration_no,
                        RTE = a.RTE,
                        TotalFee = a.Rtotal_fee,
                        ToPaidFee = a.Rstu_fee,
                        ToDueFee = a.Rdue_fee,
                        TransportDueFee = _context.TransInstallmentTbl.Where(c => c.StuId == a.StuId && c.CompanyId == SchoolId && validMonths.Contains(c.MonthName)).Sum(c => c.DueFee),
                    }).FirstOrDefaultAsync();

                return ApiResponse<StudentFeeTCModel>.SuccessResponse(res, "Fetch successfully class by section and student data ");
            }
            catch (Exception ex)
            {
                return ApiResponse<StudentFeeTCModel>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> GenerateTC(GetStudentTCDropoutReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var studentdata = _context.Student_Renew.Where(c => c.StuId == req.StudentId && c.ClassId == req.ClassId && c.due_fee == 0 && c.CompanyId == SchoolId
               && c.SessionId == SessionId).FirstOrDefault();

                if (studentdata == null)
                    return ApiResponse<bool>.ErrorResponse("Student fee record not found or due exists.");

                studentdata.Active = false;

                var TransFee = _context.StuRouteAssignTbl.Where(c => c.stu_id == req.StudentId && c.university_id == req.ClassId && c.CompanyId == SchoolId
               && c.SessionId == SessionId).FirstOrDefault();
                if (TransFee == null)
                {
                    if (TransFee.TDueFee > 0 || TransFee.OldTransDueFee > 0)
                    {
                        return ApiResponse<bool>.ErrorResponse("Student fee record not found or due exists.");
                    }
                    TransFee.Active = false;

                }

                return ApiResponse<bool>.SuccessResponse(true, "Fetch successfully student TC data ");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> StudentDropout(GetStudentTCDropoutReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var studentdata = _context.Student_Renew.Where(c => c.StuId == req.StudentId && c.ClassId == req.ClassId && c.CompanyId == SchoolId
               && c.SessionId == SessionId).FirstOrDefault();

                if (studentdata == null)
                    return ApiResponse<bool>.ErrorResponse("Student fee record not found or due exists.");

                studentdata.Dropout = true;

                return ApiResponse<bool>.SuccessResponse(true, "Fetch successfully student dropout data ");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<StudentClassExamData>> GetClassSubjectAsync(int ClassId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var ExamDataRaw = await (from Cls in _context.ClassSubjectExamTbl
                                         join Exm in _context.ExamTbl on Cls.ExamId equals Exm.ExamId
                                         where Cls.ClassId == ClassId && Cls.SessionId == SessionId && Cls.SchoolId == SchoolId
                                         select new StudentExamData
                                         {
                                             ClassId = Cls.ClassId,
                                             ExamId = Cls.ExamId,
                                             SchoolId = Cls.SchoolId,
                                             SubjectId = Cls.SubjectId,
                                             ExamName = Exm.ExamName,
                                             ExamPriority = Exm.ExamPriority,
                                         }).OrderBy(exm => exm.ExamPriority).ToListAsync();

                var ExamData = ExamDataRaw.GroupBy(x => x.ExamId).Select(g => g.FirstOrDefault()).ToList();

                //var SectionData = await _context.Classcollegeinfo.Where(p => p.ClassId == ClassId && p.SchoolId == SchoolId)
                //    .Select(p => new SectionData
                //    {
                //        ClassId = p.ClassId,
                //        SectionId = p.SectionId,
                //        SectionName = _context.collegeinfo.Where(a => a.SectionId == p.SectionId && a.SchoolId == SchoolId).Select(a => a.SectionName).FirstOrDefault(),

                //    }).ToListAsync();

                var res = new StudentClassExamData
                {
                    ExamDatas = ExamData,
                    // SectionDatas = SectionData,
                };

                return ApiResponse<StudentClassExamData>.SuccessResponse(res, "Fetch Successfully Class wise Subject");
            }
            catch (Exception ex)
            {
                return ApiResponse<StudentClassExamData>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<StudentExamData>>> GetClassExamSubjectAsync(ClassExamMarksModelreq request)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await (from cls in _context.ClassSubjectExamTbl
                                 join sub in _context.Subject on cls.SubjectId equals sub.subject_id
                                 where cls.ClassId == request.ClassId && cls.ExamId == request.ExamId && cls.SchoolId == SchoolId
                                 select new StudentExamData
                                 {
                                     ClassId = cls.ClassId,
                                     ExamId = cls.ExamId,
                                     SubjectId = cls.SubjectId,
                                     SchoolId = cls.SchoolId,
                                     SubjectName = sub.subject_name,
                                     //     SubjectPriority = sub.SubjectPriority,

                                 }).ToListAsync();

                if (res == null || !res.Any())
                    return ApiResponse<List<StudentExamData>>.ErrorResponse("No Subject found matching the criteria");

                return ApiResponse<List<StudentExamData>>.SuccessResponse(res, "Fetch  Class wise Exam Subject Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<StudentExamData>>.ErrorResponse("Error: " + ex.Message);

            }
        }

        public async Task<ClassFeeResModel> GetClassByFee(int classid)
        {
            int SchoolId = _loginUser.SchoolId;
            int SessionId = _loginUser.SessionId;


            var ClassFeeResModel = await _context.fees.Where(c => c.university_id == classid && c.CompanyId == SchoolId && c.SessionId == SessionId && c.active == true).Select(c => new ClassFeeResModel
            {
                admission_fee = c.admission_fee,
                ClassId = c.university_id,
                tution_fee = c.tution_fee,
                exam_fee = c.exam_fee,
                Develoment_fee = c.Develoment_fee,
                Games_fees = c.Games_fees,
                total = c.total,
                active = c.active,


            }).FirstOrDefaultAsync();
            return ClassFeeResModel;
        }

        public async Task<ApiResponse<List<ClassSectionResModel>>> GetClassBySection(int classid)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var sectionList = await _context.collegeinfo.Where(a => a.university_id == classid && a.CompanyId == SchoolId)
                    .Select(a => new ClassSectionResModel
                    {
                        collegeid = a.collegeid,
                        collegename = a.collegename,
                        university_id = a.university_id,
                    }).ToListAsync();


                return ApiResponse<List<ClassSectionResModel>>.SuccessResponse(sectionList, "section list fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ClassSectionResModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<PagedResult<GetQuickStudentReqModel>> GetQuickStudentList(getstudentlistReq req)
        {
            int SchoolId = _loginUser.SchoolId;
            int SessionId = _loginUser.SessionId;

            var query = _context.StudentRenewView.Where(p => p.SessionId == SessionId && p.CompanyId == SchoolId
            && p.RActive == true && (p.StuDetail == false || p.StuFees == false));

            if (!string.IsNullOrEmpty(req.srno))
                query = query.Where(p => p.registration_no == req.srno);

            if (req.ClassId.HasValue)
                query = query.Where(p => p.ClassId == req.ClassId);

            int totalrecords = await query.CountAsync();

            int PageNumber = req.PageNumber > 0 ? req.PageNumber : 1;
            int PageSize = req.PageSize > 0 ? req.PageSize : 10;

            var data = await query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ProjectTo<GetQuickStudentReqModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            int totalpages = (int)Math.Ceiling((double)totalrecords / PageSize);

            var pagedResult = new PagedResult<GetQuickStudentReqModel>
            {
                Data = data,
                TotalRecords = totalrecords,
                PageNumber = PageNumber,
                PageSize = PageSize,
                TotalPages = totalpages
            };

            return pagedResult;
        }

        public async Task<PagedResult<stduentlistres>> GetStudentList(getstudentlistReq request)
        {
            int SchoolId = _loginUser.SchoolId;
            int SessionId = _loginUser.SessionId;

            // 1. Apply filters
            var query = _context.StudentRenewView.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId
            && s.RActive == true && s.StuDetail == true && s.StuFees == true);

            if (!string.IsNullOrEmpty(request.srno))
                query = query.Where(s => s.registration_no == request.srno);

            //if (!string.IsNullOrEmpty(request.stu_name))
            //    query = query.Where(s => s.stu_name.Contains(request.stu_name));

            if (request.ClassId.HasValue)
                query = query.Where(s => s.ClassId == request.ClassId);

            //if (request.SectionId.HasValue)
            //    query = query.Where(s => s.SectionId == request.SectionId);

            // 2. Total record count before pagination
            int totalRecords = await query.CountAsync();

            // 3. Pagination
            int pageNumber = request.PageNumber > 0 ? request.PageNumber : 1;
            int pageSize = request.PageSize > 0 ? request.PageSize : 10;

            // 4. Fetch paged data
            var data = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .ProjectTo<stduentlistres>(_mapper.ConfigurationProvider).ToListAsync();

            // 5. Total pages
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // 6. Return with pagination info
            var pagedResult = new PagedResult<stduentlistres>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return pagedResult;
        }

        public async Task<ApiResponse<getStudentListModel>> getstudentdatabyid(int studentId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var student = await (from r in _context.StudentRenewView
                                     join p in _context.ParentsTbl on r.ParentsId equals p.ParentsId
                                     where r.StuId == studentId && r.CompanyId == SchoolId && r.SessionId == SessionId
                                     select new getStudentListModel
                                     {
                                         StudentId = r.StuId,
                                         SRNo = r.registration_no,
                                         stu_name = r.stu_name,
                                         email = r.email,
                                         DOB = r.DOB,
                                         gender = r.gender,
                                         ClassId = r.ClassId,
                                         SectionId = r.SectionId,
                                         RTE = r.RTE,
                                         admission_date = r.admission_date,
                                         cast_category = r.cast_category,
                                         Caste = r.Caste,
                                         Religion = r.Religion,
                                         blood_group = r.blood_group,
                                         AdharCard = r.AdharCard,
                                         address = r.address,
                                         district = r.district,
                                         city = r.city,
                                         state = r.state,
                                         pincode = r.pincode,
                                         p_address = r.p_address,
                                         p_district = r.p_district,
                                         p_city = r.p_city,
                                         p_state = r.p_state,
                                         p_pincode = r.p_pincode,
                                         LastClass = r.LastClass,
                                         LastDivision = r.LastDivision,
                                         LastExanTotalMarks = r.LastExanTotalMarks,
                                         LastParecentage = r.LastParecentage,
                                         LastRemarks = r.LastRemarks,
                                         LastSchlName = r.LastSchlName,
                                         AdmissionPayfee = r.AdmissionPayfee,
                                         AFeeDiscount = r.AFeeDiscount,
                                         admission_fee = r.admission_fee,
                                         Tution_fee = r.tution_fee,
                                         exam_fee = r.exam_fee,
                                         Develoment_fee = r.Develoment_fee,
                                         Games_fees = r.Games_fees,
                                         date = r.date,
                                         discount = r.discount,
                                         total = r.total,
                                         total_fee = r.total_fee,
                                         OldDuefees = r.OldDuefees,
                                         PramoteFees = r.PramoteFees,
                                         stu_photo = r.stu_photo,
                                         stu_aadhar = r.stu_aadhar,
                                         stu_birth = r.stu_birth,
                                         father_aadhar = r.father_aadhar,
                                         mother_aadhar = r.mother_aadhar,
                                         Jan_Aadhar = r.Jan_Aadhar,
                                         Income_Certificate = r.Income_Certificate,
                                         LastMarkSheetPhoto = r.LastMarkSheetPhoto,
                                         // LastSchoolTC = r.LastStudentTC,
                                         //   StudentTc = r.StudentTc,
                                         parentid = p.ParentsId,
                                         GuardianName = p.GuardianName,
                                         GuardianMobileNo = p.GuardianMobileNo,
                                         FatherName = p.FatherName,
                                         FatherMobileNo = p.FatherMobileNo,
                                         FatherOccupation = p.FatherOccupation,
                                         FatherIncome = p.FatherIncome,
                                         MotherName = p.MotherName,
                                         MotherMobileNo = p.MotherMobileNo,
                                         MotherOccupation = p.MotherOccupation,
                                         MotherIncome = p.MotherIncome,
                                         FeeInstallments = _context.fee_installment.Where(f => f.stu_id == r.StuId && f.university_id == r.ClassId && f.SessionId == r.SessionId)
                                                             .Select(f => new FeeInstallmentDto
                                                             {
                                                                 SInsAmount = f.FAmount
                                                             }).ToList(),
                                         AdmissionReceipts = _context.M_FeeDetail.Where(f => f.stu_id == r.StuId && f.ClassId == r.ClassId && f.SessionId == r.SessionId && f.Status == "AdmissionPayFee")
                                                             .Select(f => new AdmissionFeeReceiptDto
                                                             {
                                                                 PayFees = f.PayFees,
                                                                 PaymentDate = f.PaymentDate,
                                                                 PaymentMode = f.PaymentMode,
                                                                 Cash = f.Cash,
                                                                 Upi = f.Upi
                                                             }).FirstOrDefault(),
                                     }).FirstOrDefaultAsync();

                if (student == null)
                    return ApiResponse<getStudentListModel>.ErrorResponse("Student not found");

                return ApiResponse<getStudentListModel>.SuccessResponse(student);
            }
            catch (Exception ex)
            {
                return ApiResponse<getStudentListModel>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<studentreesponse>>> GetSectionbyStudent(int sectionid)
        {
            try
            {
                int schoolId = _loginUser.SchoolId;
                int sessionId = _loginUser.SessionId;

                var studentEntities = await _context.StudentRenewView.Where(p => p.SectionId == sectionid && p.SessionId == sessionId && p.CompanyId == schoolId && p.RActive == true)
                    .ToListAsync();

                var studentList = _mapper.Map<List<studentreesponse>>(studentEntities);

                if (studentList == null || studentList.Count == 0)
                {
                    return ApiResponse<List<studentreesponse>>.SuccessResponse(studentList, "No students found for the given class.");
                }
                return ApiResponse<List<studentreesponse>>.SuccessResponse(studentList, "Student list fetched successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<studentreesponse>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<ClassByFeeInResponse>> GetClassByFeeInsAsync(int ClassId)
        {
            try
            {
                int schoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int sessionId = _loginUser.SessionId;

                var feedata = await _context.fees.Where(c => c.CompanyId == schoolId && c.university_id == ClassId).FirstOrDefaultAsync();
                var feeinstallmentdata = await _context.InstallmentTbl.Where(c => c.CompanyId == schoolId && c.university_id == ClassId)
                    .Select(p => new FeeInstallmentClass
                    {
                        // ClassId = p.university_id ?? 0,
                        Installmentno = p.Installmentno,
                        SInsAmount = p.FeeAmount ?? 0
                    }).ToListAsync();

                //  var Sectionname = _context.collegeinfo.Where(a => a.university_id == ClassId && a.CompanyId == schoolId).Select(a => a.collegename).FirstOrDefaultAsync();

                var result = new ClassByFeeInResponse
                {
                    ClassId = feedata.university_id,
                    admission_fee = feedata.admission_fee,
                    tution_fee = feedata.tution_fee,
                    exam_fee = feedata.exam_fee,
                    Develoment_fee = feedata.Develoment_fee,
                    Games_fees = feedata.Games_fees,
                    total = feedata.total,
                    FeeInstallments = feeinstallmentdata,
                    //  Sections = feedata.sec
                };
                if (result == null)
                    return ApiResponse<ClassByFeeInResponse>.ErrorResponse("Installment not found");

                return ApiResponse<ClassByFeeInResponse>.SuccessResponse(result, "Installment fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ClassByFeeInResponse>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }






        //public async Task<ApiResponse<bool>> studentexcelupload(List<StudentExcelUploadListReq> request)
        //{
        //    using var transaction = await _context.Database.BeginTransactionAsync();
        //    {
        //        try
        //        {


        //            await transaction.CommitAsync();
        //            return ApiResponse<bool>.SuccessResponse(true);

        //        }
        //        catch (Exception ex)
        //        {
        //            await transaction.RollbackAsync();

        //            return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
        //        }
        //    }
        //}


    }
}





