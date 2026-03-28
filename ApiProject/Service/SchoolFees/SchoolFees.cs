using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using ApiProject.Service.Current;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ApiProject.Service.SchoolFees
{
    public class SchoolFees : ISchoolFees
    {
        private readonly ILoginUserService _loginUser;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public SchoolFees(
            ILoginUserService loginUser,
            ApplicationDbContext context,
                        IMapper mapper

            )
        {
            _context = context;
            _loginUser = loginUser;
            _mapper = mapper;

        }

        public async Task<ApiResponse<List<ClassFeesRes>>> GetClassFees()
        {
            try
            {

                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var feesEntities = await (from fee in _context.fees
                                          join cls in _context.University
                                          on fee.university_id equals cls.university_id
                                          where fee.CompanyId == SchoolId && fee.SessionId == SessionId
                                          select new ClassFeesRes
                                          {
                                              fee_id = fee.fee_id,
                                              ClassId = fee.university_id,
                                              admission_fee = fee.admission_fee,
                                              exam_fee = fee.exam_fee,
                                              tution_fee = fee.tution_fee,
                                              Develoment_fee = fee.Develoment_fee,
                                              Games_fees = fee.Games_fees,
                                              total = fee.total,
                                              active = fee.active,
                                              ClassName = cls.university_name,
                                              //    ClassPriority = cls.ClassPriority
                                          }).ToListAsync();

                var fesslist = _mapper.Map<List<ClassFeesRes>>(feesEntities);


                return ApiResponse<List<ClassFeesRes>>.SuccessResponse(fesslist, "Fess list fetched successfully");

            }
            catch (Exception ex)
            {

                return ApiResponse<List<ClassFeesRes>>.ErrorResponse("Error: " + ex.Message);

            }
        }
        public async Task<ApiResponse<bool>> AddClassFees(AddClassFeesReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;


                var existing = await _context.fees.FirstOrDefaultAsync(p => p.university_id == req.ClassId && p.SessionId == SessionId && p.CompanyId == SchoolId);

                if (existing != null)
                {
                    return ApiResponse<bool>.ErrorResponse("This Class Fee  already Insert");
                }


                int feesid = _context.fees.DefaultIfEmpty().Max(s => s == null ? 0 : s.fee_id) + 1;

                var feesEntity = new fees
                {
                    fee_id = feesid,
                    university_id = req.ClassId,
                    admission_fee = req.admission_fee,
                    tution_fee = req.tution_fee,
                    exam_fee = req.exam_fee,
                    Develoment_fee = req.Develoment_fee,
                    Games_fees = req.Games_fees,
                    total = req.total,
                    active = true,
                    CompanyId = SchoolId,
                    Userid = UserId,
                    SessionId = SessionId,
                    //     CreateDate = DateTime.Now,
                    //    UpdateDate = DateTime.Now
                };

                _context.fees.Add(feesEntity);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Fees inserted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateClassFees(UpdateClassFeesReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var existing = await _context.fees.FirstOrDefaultAsync(p => p.fee_id != req.fee_id && p.university_id == req.ClassId && p.SessionId == SessionId && p.CompanyId == SchoolId);

                if (existing != null)
                {
                    return ApiResponse<bool>.ErrorResponse("This Class Fee already Insert");
                }

                var result = _context.fees.Where(r => r.fee_id == req.fee_id && r.CompanyId == SchoolId).FirstOrDefault();
                result.university_id = req.ClassId;
                result.admission_fee = req.admission_fee;
                result.tution_fee = req.tution_fee;
                result.exam_fee = req.exam_fee;
                result.Develoment_fee = req.Develoment_fee;
                result.Games_fees = req.Games_fees;
                result.total = req.total;
                result.Userid = UserId;
                //  result.UpdateDate = DateTime.Now;
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Fees update successfully.");


            }
            catch (Exception ex)
            {

                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> ChangeStatusClassFees(int FeeId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;

                var classEntity = await _context.fees.FirstOrDefaultAsync(c => c.fee_id == FeeId && c.CompanyId == SchoolId);

                if (classEntity == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Class not found");
                }

                // Status toggle karo
                classEntity.active = classEntity.active == null ? true : !classEntity.active;

                // Changes save karo
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Status updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }



        // Student Fee Installment
        public async Task<ApiResponse<List<FeesInstRes>>> GetFeeInstallment()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.University.Where(c => c.CompanyId == SchoolId).Select(c => new FeesInstRes
                {
                    ClassId = c.university_id,
                    ClassName = c.university_name,
                    total = _context.fees.Where(u => u.university_id == c.university_id && u.SessionId == SessionId && u.CompanyId == SchoolId).Select(u => u.total).FirstOrDefault(),

                    installments = _context.InstallmentTbl.Where(u => u.university_id == c.university_id && u.SessionId == SessionId && u.CompanyId == SchoolId)
                                    .Select(a => new InstallmentDetail
                                    {
                                        InstallmentId = a.InstallmentId,
                                        FeeAmount = a.FeeAmount,
                                        Installment = a.Installment,
                                        Installmentno = a.Installmentno,
                                    }).ToList()
                }).ToListAsync();

                return ApiResponse<List<FeesInstRes>>.SuccessResponse(res, "Fees list fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<FeesInstRes>>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> insertfeesinstallment(List<AddFeesInstallmentReq> req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var existing = await _context.InstallmentTbl.FirstOrDefaultAsync(p => p.university_id == req[0].ClassId && p.SessionId == SessionId && p.CompanyId == SchoolId);

                if (existing != null)
                {
                    return ApiResponse<bool>.ErrorResponse("This Class Fees Installment already Insert");
                }

                if (req != null && req.Count > 0)
                {
                    for (int i = 0; i < req.Count; i++)
                    {
                        InstallmentTbl Install = new InstallmentTbl
                        {
                            InstallmentId = _context.InstallmentTbl.DefaultIfEmpty().Max(r => r == null ? 0 : r.InstallmentId) + 1,
                            university_id = req[i].ClassId,
                            total = req[i].TotalFee,
                            Installmentno = req[i].Installmentno,
                            FeeAmount = req[i].InsAmount,
                            Installment = req[i].Installment,
                            Date = DateTime.Now,
                            Active = true,
                            CompanyId = SchoolId,
                            SessionId = SessionId,
                            Userid = UserId
                        };

                        _context.InstallmentTbl.Add(Install);
                        _context.SaveChanges();
                    }
                }

                return ApiResponse<bool>.SuccessResponse(true, "Fees Installment inserted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> updatefeesinstallment(List<AddFeesInstallmentReq> req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var existingInstallments = _context.InstallmentTbl.Where(s => s.university_id == req[0].ClassId && s.SessionId == SessionId && s.CompanyId == SchoolId).ToList();
                for (int i = 0; i < existingInstallments.Count; i++)
                {
                    _context.InstallmentTbl.Remove(existingInstallments[i]);
                }
                await _context.SaveChangesAsync();

                if (req != null && req.Count > 0)
                {
                    for (int i = 0; i < req.Count; i++)
                    {
                        InstallmentTbl Install = new InstallmentTbl
                        {
                            InstallmentId = _context.InstallmentTbl.DefaultIfEmpty().Max(r => r == null ? 0 : r.InstallmentId) + 1,
                            university_id = req[i].ClassId,
                            total = req[i].TotalFee,
                            Installmentno = req[i].Installmentno,
                            FeeAmount = req[i].InsAmount,
                            Installment = req[i].Installment,
                            Date = DateTime.Now,
                            Active = true,
                            CompanyId = SchoolId,
                            SessionId = SessionId,
                            Userid = UserId
                        };

                        _context.InstallmentTbl.Add(Install);
                        await _context.SaveChangesAsync();
                    }
                }

                return ApiResponse<bool>.SuccessResponse(true, "Fees Installment Updated success.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }

        // fees collection
        public async Task<ApiResponse<List<ClassIdByStudentRes>>> getclassbystudent(int classid)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var result = await _context.StudentRenewView.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId && s.ClassId == classid).OrderBy(s => s.stu_name).ToArrayAsync();

                var studentlist = _mapper.Map<List<ClassIdByStudentRes>>(result);
                if (studentlist == null || studentlist.Count == 0)
                {
                    return ApiResponse<List<ClassIdByStudentRes>>.SuccessResponse(studentlist, "No students found for the given class.");
                }
                return ApiResponse<List<ClassIdByStudentRes>>.SuccessResponse(studentlist, "Student list fetched successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<ClassIdByStudentRes>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<StudentFeesDetailRes>> getstudentfeesdetail(StudentFeesDetailReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var student = await _context.StudentRenewView.Where(c => (req.ClassId == -1 || c.ClassId == req.ClassId) && (req.StudentId == -1 || c.StuId == req.StudentId) &&
                c.SessionId == SessionId && c.CompanyId == SchoolId)
                    .Select(c => new StudentFeesDetailRes
                    {
                        StuId = c.StuId,
                        stu_name = c.stu_name,
                        ClassId = c.ClassId,
                        SectionId = c.SectionId,
                        SrNo = c.registration_no,
                        fathername = c.father_name,
                        fathermobileno = c.FatherMobileNo,
                        RTE = c.RTE,
                        admission_fee = c.Radmission_fee,
                        PramoteFees = c.PramoteFees,
                        AFeeDiscount = c.RAFeeDiscount,
                        AdmissionPayfee = c.RAdmissionPayfee,
                        exam_fee = c.Rexam_fee,
                        Tution_fee = c.RTution_fee,
                        Develoment_fee = c.RDeveloment_fee,
                        Games_fees = c.RGames_fees,
                        total = c.Rtotal,
                        OldDuefees = c.OldDuefees,
                        discount = c.Rdiscount,
                        total_fee = c.Rtotal_fee,
                        TotalPaid = c.Rstu_fee,
                        ClassName = _context.University.Where(a => a.university_id == req.ClassId && a.CompanyId == SchoolId).Select(a => a.university_name).FirstOrDefault(),
                        SectionName = _context.collegeinfo.Where(a => a.collegeid == c.SectionId && a.CompanyId == SchoolId).Select(a => a.collegename).FirstOrDefault(),

                        Installment = _context.fee_installment.Where(a => a.stu_id == c.StuId && a.university_id == c.ClassId && a.SessionId == SessionId && a.CompanyId == SchoolId && a.active == true)
                    .Select(a => new StufeeinstallmentModel
                    {
                        Installmentno = a.Installment,
                        dueInstallmentfee = a.due_fee,
                        FeeAmount = a.FAmount,
                    }).ToList(),


                    }).OrderBy(c => c.stu_name).FirstOrDefaultAsync();

                if (student == null)
                {
                    return ApiResponse<StudentFeesDetailRes>
                        .ErrorResponse("No students found matching the criteria");
                }

                return ApiResponse<StudentFeesDetailRes>
                    .SuccessResponse(student, "Fetched Student Fees Detail successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<StudentFeesDetailRes>
                    .ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<StudentFeesRes>> insertstudentfees(StudentFeesReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;
                int UserId = _loginUser.UserId;

                Student_Renew Section = _context.Student_Renew.Where(p => p.ClassId == req.ClassId && p.StuId == req.StudentId && p.due_fee == 0).FirstOrDefault();
                if (Section != null)
                {
                    return ApiResponse<StudentFeesRes>.ErrorResponse("duefee Already available");
                }
                else
                {
                    string ReceiptCode = "";
                    institute GetInstituteCodeName = _context.institute.Where(i => i.institute_id == SchoolId).FirstOrDefault();
                    M_FeeDetail LastCode = _context.M_FeeDetail.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).OrderByDescending(s => s.FDId).Take(1).FirstOrDefault();
                    string threeLetters = GetInstituteCodeName.instituteCode.Substring(0, 3).ToUpper();

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
                    if (LastCode != null)
                    {
                        var Receipt = LastCode.ReceiptNo.Split('/');
                        ReceiptCode = Receipt[1];

                        Id = int.Parse(ReceiptCode);
                        Id++;
                    }
                    else
                    {
                        Id = 1;
                    }
                    ReceiptCode = threeLetters + "/" + Id;

                    int newFRId = _context.M_FeeDetail.DefaultIfEmpty().Max(r => r == null ? 0 : r.FDId) + 1;

                    var RStudentFees = new M_FeeDetail
                    {
                        FDId = newFRId,
                        ReceiptNo = ReceiptCode,
                        ClassId = req.ClassId,
                        stu_id = req.StudentId,
                        Status = "1",
                        Active = true,
                        CompanyId = SchoolId,
                        Userid = UserId,
                        SessionId = SessionId,
                        Date = DateTime.Now.Date,
                        PaymentDate = DateTime.Now.Date,
                        PaymentMode = req.PaymentMode,
                        PayFees = req.PayFees,
                        Cash = req.Cash,
                        Upi = req.Upi,
                        OrderStatus = "Success",
                        OrderNo = NewOrderNo.ToString(),
                        TransactionId = "",
                        ReceiptType = "Offline",
                        Remark = req.Remark,
                        RTS = DateTime.Now,

                    };

                    _context.M_FeeDetail.Add(RStudentFees);
                    await _context.SaveChangesAsync();

                    var studentrenewtbl = _context.Student_Renew.Where(s => s.StuId == req.StudentId && s.CompanyId == SchoolId && s.SessionId == SessionId
                                && s.ClassId == req.ClassId).FirstOrDefault();
                    studentrenewtbl.due_fee = studentrenewtbl.due_fee - req.PayFees;
                    studentrenewtbl.stu_fee += req.PayFees;
                    await _context.SaveChangesAsync();


                    var installments = _context.fee_installment.Where(u => u.stu_id == req.StudentId && u.university_id == req.ClassId && u.CompanyId == SchoolId
                            && u.SessionId == SessionId).Select(a => new
                            {
                                a.university_id,
                                a.IntallmentID,
                                a.total_fee,
                                a.Installment,
                                a.FAmount,

                            }).ToList();

                    double subfee = 0;
                    double subfee2 = Convert.ToDouble(req.PayFees);

                    for (int i = 0; i < installments.Count && req.PayFees > 0; i++)
                    {
                        int? installmentid = installments[i].IntallmentID;

                        fee_installment result = _context.fee_installment.FirstOrDefault(f => f.stu_id == req.PayFees && f.university_id == req.ClassId && f.CompanyId == SchoolId
                                && f.SessionId == SessionId && f.IntallmentID == installmentid);

                        if (result != null)
                        {
                            if (result.due_fee != 0)
                            {

                                if (subfee2 >= result.due_fee)
                                {
                                    subfee2 = Convert.ToDouble(subfee2) - Convert.ToDouble(result.due_fee);
                                    result.due_fee = 0;
                                    await _context.SaveChangesAsync();
                                }
                                else if (subfee2 != 0)
                                {
                                    result.due_fee = result.due_fee - subfee2;
                                    subfee2 = 0;
                                    await _context.SaveChangesAsync();
                                    break;

                                }
                                else
                                {
                                    result.due_fee = result.due_fee - subfee2;
                                    subfee2 = 0;
                                    await _context.SaveChangesAsync();
                                    break;

                                }
                            }
                        }
                    }


                    var studentDetail = new StudentFeesRes
                    {
                        receiptId = newFRId,
                    };

                    return ApiResponse<StudentFeesRes>.SuccessResponse(studentDetail, "Student fees inserted successfully.");
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<StudentFeesRes>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<StudentFeesReceiptRes>> getfeereceipt(int receiptId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                // Step 1: One-shot query with join and projection
                var data = await (
                    from fee in _context.M_FeeDetail
                    join student in _context.StudentRenewView
                        on fee.stu_id equals student.StuId
                    where fee.CompanyId == SchoolId
                       && fee.SessionId == SessionId
                       && student.CompanyId == SchoolId
                       && student.SessionId == SessionId
                       && fee.FDId == receiptId
                    select new StudentFeesReceiptRes
                    {

                        ReceiptNo = fee.ReceiptNo,
                        stu_name = student.stu_name,
                        srno = student.registration_no,
                        ClassName = _context.University.Where(a => a.university_id == student.ClassId && a.CompanyId == SchoolId).Select(a => a.university_name).FirstOrDefault(),
                        SectionName = _context.collegeinfo.Where(a => a.collegeid == student.SectionId && a.CompanyId == SchoolId).Select(a => a.collegename).FirstOrDefault(),
                        //  ClassName = student.ClassName,
                        //  SectionName = student.SectionName,
                        fathername = student.father_name,
                        fathermobileno = student.father_mobile,
                        PayFees = fee.PayFees,
                        Remark = fee.Remark,
                        PaymentDate = fee.PaymentDate,
                        PaymentMode = fee.PaymentMode

                    }
                ).FirstOrDefaultAsync();

                if (data == null)
                    return ApiResponse<StudentFeesReceiptRes>.ErrorResponse("No record found");

                return ApiResponse<StudentFeesReceiptRes>.SuccessResponse(data, "Student receipt fetched successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<StudentFeesReceiptRes>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }


        //public async Task<ApiResponse<PagedResult<StudentFeesCollectionListRes>>> GetDailyFeeCollection(FeesCollectionReq req)
        //{
        //    try
        //    {
        //        int SchoolId = _loginUser.SchoolId;
        //        int SessionId = _loginUser.SessionId;

        //        // 1. Query banate hain
        //        var query = from fee in _context.M_FeeDetail
        //                    join student in _context.StudentRenewView
        //                        on fee.stu_id equals student.StuId
        //                    where fee.CompanyId == SchoolId
        //                          && fee.SessionId == SessionId
        //                          && student.CompanyId == SchoolId
        //                          && student.SessionId == SessionId
        //                          && fee.Active == true
        //                          && (req.FromDate == null || fee.PaymentDate >= req.FromDate)
        //                          && (req.ToDate == null || fee.PaymentDate <= req.ToDate)
        //                    select new StudentFeesCollectionListRes
        //                    {
        //                        ReceiptNo = fee.ReceiptNo,
        //                        ReceiptId = fee.FDId,
        //                        stu_name = student.stu_name,
        //                        srno = student.registration_no,
        //                        ClassName = _context.University.Where(a => a.university_id == student.ClassId && a.CompanyId == SchoolId).Select(a => a.university_name).FirstOrDefault(),
        //                        SectionName = _context.collegeinfo.Where(a => a.collegeid == student.SectionId && a.CompanyId == SchoolId).Select(a => a.collegename).FirstOrDefault(),
        //                        //  ClassName = student.ClassName,
        //                        // SectionName = student.SectionName,
        //                        fathername = student.father_name,
        //                        fathermobileno = student.father_mobile,
        //                        PayFees = fee.PayFees,
        //                        Remark = fee.Remark,
        //                        PaymentDate = fee.PaymentDate,
        //                        PaymentMode = fee.PaymentMode
        //                    };

        //        // 2. Total records count
        //        int totalRecords = await query.CountAsync();

        //        // 3. Pagination setup
        //        int pageNumber = req.PageNumber > 0 ? req.PageNumber : 1;
        //        int pageSize = req.PageSize > 0 ? req.PageSize : 10;

        //        // 4. Apply pagination
        //        var data = await query.OrderByDescending(x => x.PaymentDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        //        // 5. Prepare response
        //        var pagedResult = new PagedResult<StudentFeesCollectionListRes>
        //        {
        //            Data = data,
        //            TotalRecords = totalRecords,
        //            PageNumber = pageNumber,
        //            PageSize = pageSize,
        //            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
        //        };

        //        if (data == null || !data.Any())
        //            return ApiResponse<PagedResult<StudentFeesCollectionListRes>>.ErrorResponse("No record found");

        //        return ApiResponse<PagedResult<StudentFeesCollectionListRes>>.SuccessResponse(pagedResult, "Receipt fetched successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiResponse<PagedResult<StudentFeesCollectionListRes>>.ErrorResponse("Something went wrong: " + ex.Message);
        //    }
        //}

        // 1 code 

        public async Task<ApiResponse<DailyCollectionReportModel>> GetDailyFeeCollection(FeesCollectionReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                int pageNumber = req.PageNumber > 0 ? req.PageNumber : 1;
                int pageSize = req.PageSize > 0 ? req.PageSize : 10;

                /* ===================== STUDENT FEE ===================== */

                var studentQuery =
                    from fee in _context.M_FeeDetail
                    join student in _context.StudentRenewView
                        on fee.stu_id equals student.StuId
                    where student.SessionId == fee.SessionId
                    join cls in _context.University
                         on student.ClassId equals cls.university_id
                    join sec in _context.collegeinfo
                        on student.SectionId equals sec.collegeid
                    where fee.CompanyId == SchoolId
                    && fee.SessionId == SessionId
                    && fee.Active == true
                    && (req.FromDate == null || fee.PaymentDate >= req.FromDate)
                    && (req.ToDate == null || fee.PaymentDate <= req.ToDate)
                    // && (req.MAdmissionPayfee > 0 || c.PayFees > 0 || c.MPramoteFees > 0)

                    select new StudentFeesCollectionListRes
                    {
                        ReceiptId = fee.FDId,
                        ReceiptNo = fee.ReceiptNo,
                        stu_name = student.stu_name,
                        srno = student.registration_no,
                        ClassName = cls.university_name,
                        SectionName = sec.collegename,

                        fathername = student.father_name,
                        fathermobileno = student.father_mobile,
                        PayFees = fee.PayFees,
                        FeeType = "Tution Fee",
                        Remark = fee.Remark,
                        PaymentDate = fee.PaymentDate,
                        PaymentMode = fee.PaymentMode
                    };

                int studentTotal = await studentQuery.CountAsync();
                var studentData = await studentQuery.OrderByDescending(x => x.ReceiptId).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                /* ===================== TRANSPORT FEE ===================== */

                var transportQuery =
                    from fee in _context.NewTransportFeeTbl
                    join student in _context.StudentRenewView
                        on fee.stu_id equals student.StuId
                    where student.SessionId == fee.SessionId
                    join cls in _context.University
                    on student.ClassId equals cls.university_id
                    join sec in _context.collegeinfo
                        on student.SectionId equals sec.collegeid
                    where fee.CompanyId == SchoolId && fee.SessionId == SessionId && fee.Active == true
                          && (req.FromDate == null || fee.Date >= req.FromDate) && (req.ToDate == null || fee.Date <= req.ToDate)
                    select new StudentFeesCollectionListRes
                    {
                        ReceiptId = fee.NewPaymentId,
                        ReceiptNo = fee.ReceiptNo,
                        stu_name = student.stu_name,
                        srno = student.registration_no,
                        ClassName = cls.university_name,
                        SectionName = sec.collegename,

                        fathername = student.father_name,
                        fathermobileno = student.father_mobile,
                        FeeType = "Transport Fee",
                        PayFees = fee.PayFee,
                        Remark = fee.Remark,
                        PaymentDate = fee.Date,
                        PaymentMode = fee.PaymentMode
                    };

                int transportTotal = await transportQuery.CountAsync();

                var transportData = await transportQuery.OrderByDescending(x => x.ReceiptId).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                /* ===================== FINAL RESPONSE ===================== */

                var response = new DailyCollectionReportModel
                {
                    StudentFee = new PagedResult<StudentFeesCollectionListRes>
                    {
                        Data = studentData,
                        TotalRecords = studentTotal,
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        TotalPages = (int)Math.Ceiling((double)studentTotal / pageSize)
                    },

                    TransportFee = new PagedResult<StudentFeesCollectionListRes>
                    {
                        Data = transportData,
                        TotalRecords = transportTotal,
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        TotalPages = (int)Math.Ceiling((double)transportTotal / pageSize)
                    }

                };
                return ApiResponse<DailyCollectionReportModel>.SuccessResponse(response, "Fee collection fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<DailyCollectionReportModel>.ErrorResponse("Something went wrong : " + ex.Message);
            }
        }

        public async Task<ApiResponse<ClassWiseTotalFeeModel>> GetClasswiseTotalFee(int ClassId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;


                var res = await _context.fees.Where(s => s.university_id == ClassId && s.active == true && s.CompanyId == SchoolId && s.SessionId == SessionId)
                    .Select(c => new ClassWiseTotalFeeModel
                    {
                        ClassId = c.university_id,
                        TotalFee = c.total,
                    }).FirstOrDefaultAsync();
                if (res == null)
                {
                    return ApiResponse<ClassWiseTotalFeeModel>.ErrorResponse("No Fee Fount");
                }

                return ApiResponse<ClassWiseTotalFeeModel>.SuccessResponse(res, "Fetch Classwise total fee.");

            }
            catch (Exception ex)
            {
                return ApiResponse<ClassWiseTotalFeeModel>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<PagedResult<ClassFeesInstaListRes>>> getclassfeesInstallment(ClassFeesFilterReq req)
        {
            try
            {
                int schoolId = _loginUser.SchoolId;
                int sessionId = _loginUser.SessionId;

                var query = from s in _context.StudentRenewView
                            where s.CompanyId == schoolId &&
                                  s.SessionId == sessionId &&
                                  s.RActive == true &&
                                  (req.ClassId == 0 || s.ClassId == req.ClassId) &&
                                  (req.SectionId == 0 || s.SectionId == req.SectionId)
                            select new
                            {
                                s.StuId,
                                s.ClassId,
                                s.stu_name,
                                // s.ClassName,
                                //  s.SectionName,
                                srno = s.registration_no,
                                father_name = s.father_name,
                                s.father_mobile,
                                s.admission_fee,
                                s.total,
                                s.total_fee,
                                s.tution_fee,
                                s.exam_fee,
                                s.Develoment_fee,
                                s.Games_fees,
                                s.discount
                            };

                var totalRecords = await query.CountAsync();

                var resultQuery = await query
                    .OrderBy(x => x.stu_name)
                    .Skip((req.PageNumber - 1) * req.PageSize)
                    .Take(req.PageSize)
                    .ToListAsync();

                var studentIds = resultQuery.Select(x => x.StuId).Distinct().ToList();
                var classIds = resultQuery.Select(x => x.ClassId).Distinct().ToList(); // ✅ fixed this line

                // Batch fetch FeeReceipts
                var feeReceipts = await _context.M_FeeDetail
                    .Where(f => studentIds.Contains((int)f.stu_id) &&
                                classIds.Contains((int)f.ClassId) &&
                                f.SessionId == sessionId &&
                                f.CompanyId == schoolId &&
                                f.Active == true)
                    .ToListAsync();

                // Batch fetch Installments
                var feeInstallments = await _context.fee_installment
                    .Where(i => studentIds.Contains((int)i.stu_id) &&
                                classIds.Contains((int)i.university_id) &&
                                i.SessionId == sessionId &&
                                i.CompanyId == schoolId)
                    .ToListAsync();

                var responseList = resultQuery.Select(x =>
                {
                    var totalPaid = feeReceipts
                        .Where(f => f.stu_id == x.StuId && f.ClassId == x.ClassId && f.Status == "1")
                        .Sum(f => f.PayFees ?? 0); // ✅ fixed to per-student basis

                    var installments = feeInstallments
          .Where(i => i.stu_id == x.StuId && i.university_id == x.ClassId)
          .Select(i => new FeeInstallment
          {
              StudentId = i.stu_id,
              SInsAmount = i.FAmount
          })
          .ToList();


                    return new ClassFeesInstaListRes
                    {
                        stu_name = x.stu_name,
                        srno = x.srno,
                        fathername = x.father_name,
                        fathermobileno = x.father_mobile,
                        AdmissionPayfee = x.admission_fee,
                        total = x.total,
                        discount = x.discount,
                        total_fee = x.total_fee,
                        // ClassName = x.ClassName,
                        // SectionName = x.SectionName,
                        TotalPaid = (decimal)totalPaid,
                        FeeInstallment = installments  // ✅ default to empty list
                    };
                }).ToList();

                var pagedResult = new PagedResult<ClassFeesInstaListRes>
                {
                    Data = responseList,
                    TotalRecords = totalRecords,
                    PageNumber = req.PageNumber,
                    PageSize = req.PageSize,
                    TotalPages = (int)Math.Ceiling((double)totalRecords / req.PageSize)
                };

                return ApiResponse<PagedResult<ClassFeesInstaListRes>>.SuccessResponse(pagedResult, "Data fetched successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<ClassFeesInstaListRes>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateStudentReceipt(ReceiptupdateModel req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var result = _context.M_FeeDetail.Where(r => r.FDId == req.FeeReceiptId && r.stu_id == req.Studentid && r.CompanyId == SchoolId).FirstOrDefault();
                result.PaymentMode = req.PaymentMode;
                result.PaymentDate = req.PaymentDate;
                result.Remark = req.remark;
                result.RTS = DateTime.Now;
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Fees update successfully.");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<ClassWiseTotalFeeModel>> GetClasswiseTotalFee1(int ClassId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;


                var res = await _context.fees.Where(s => s.university_id == ClassId && s.active == true && s.CompanyId == SchoolId && s.SessionId == SessionId)
                    .Select(c => new ClassWiseTotalFeeModel
                    {
                        ClassId = c.university_id,
                        TotalFee = c.total,
                    }).FirstOrDefaultAsync();
                if (res == null)
                {
                    return ApiResponse<ClassWiseTotalFeeModel>.ErrorResponse("No Fee Fount");
                }

                return ApiResponse<ClassWiseTotalFeeModel>.SuccessResponse(res, "Fetch Classwise total fee.");

            }
            catch (Exception ex)
            {
                return ApiResponse<ClassWiseTotalFeeModel>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteStudentReceipt(int receiptId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var StuReceipt = await _context.M_FeeDetail.FirstOrDefaultAsync(c => c.FDId == receiptId && c.CompanyId == SchoolId);

                if (StuReceipt == null)
                {
                    return ApiResponse<bool>.ErrorResponse("User not found");
                }

                StuReceipt.Active = StuReceipt.Active == null ? true : !StuReceipt.Active;
                await _context.SaveChangesAsync();

                var sturenew = _context.Student_Renew.Where(c => c.StuId == StuReceipt.stu_id && c.SessionId == SessionId && c.CompanyId == SchoolId).FirstOrDefault();

                if (sturenew != null)
                {
                    sturenew.stu_fee -= StuReceipt.PayFees;
                    sturenew.due_fee += StuReceipt.PayFees;
                    await _context.SaveChangesAsync();
                }

                var installments = _context.fee_installment.Where(c => c.stu_id == StuReceipt.stu_id && c.SessionId == SessionId && c.CompanyId == SchoolId).ToList();

                if (installments != null && installments.Count > 0)
                {
                    decimal remainingFee = (decimal)sturenew.stu_fee;

                    for (int i = 0; i < installments.Count; i++)
                    {
                        var installmentId = installments[i].Id;

                        fee_installment feeInstall = _context.fee_installment.Where(p => p.stu_id == StuReceipt.stu_id && p.Id == installmentId).FirstOrDefault();

                        if (feeInstall != null)
                        {
                            decimal deduction = Math.Min(remainingFee, Convert.ToDecimal((double)installments[i].FAmount));
                            feeInstall.AdmissionPayfee = StuReceipt.AdmissionPayfee;
                            feeInstall.AFeeDiscount = StuReceipt.AFeeDiscount;
                            // feeInstall.total_fee = result.total_fee;
                            feeInstall.due_fee = Convert.ToDouble((installments[i].FAmount ?? 0) - (double)deduction);
                            feeInstall.FAmount = installments[i].FAmount;
                            feeInstall.Installment = installments[i].Installment;
                            feeInstall.IntallmentID = installments[i].IntallmentID;

                            feeInstall.paid_date = DateTime.Now;
                            feeInstall.Date = DateTime.Now;

                            remainingFee -= deduction;

                            await _context.SaveChangesAsync();
                        }
                    }
                }

                return ApiResponse<bool>.SuccessResponse(true, "Delete Student Fee Receipt successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }


    }
}





