using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using ApiProject.Service.Current;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using System.Linq;
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

                var query = _context.StudentRenewView.Where(s => s.StuId == req.StudentId && s.SessionId == SessionId);

                //if (!string.IsNullOrEmpty(req.srno))
                //    query = query.Where(s => s.SRNo == req.srno);


                query = query.Where(s => s.StuId == req.StudentId.Value);
                query = query.Where(s => s.ClassId == req.ClassId.Value);

                var studentEntity = await query.FirstOrDefaultAsync();

                if (studentEntity == null)
                {
                    return ApiResponse<StudentFeesDetailRes>.ErrorResponse("Student not found.");
                }

                var studentDetail = _mapper.Map<StudentFeesDetailRes>(studentEntity);

                var totalPaid = await _context.M_FeeDetail.Where(f => f.CompanyId == SchoolId && f.SessionId == SessionId &&
                f.ClassId == req.ClassId && f.stu_id == req.StudentId && f.Status == "1" && f.Active == true)
                    .SumAsync(f => (decimal?)f.PayFees) ?? 0;

                studentDetail.TotalPaid = totalPaid;

                return ApiResponse<StudentFeesDetailRes>.SuccessResponse(studentDetail, "Student details fetched successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<StudentFeesDetailRes>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<StudentFeesRes>> insertstudentfees(StudentFeesReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;
                int UserId = _loginUser.UserId;

                // Generate new FRId
                int newFRId = _context.M_FeeDetail.DefaultIfEmpty().Max(r => r == null ? 0 : r.FDId) + 1;

                var RStudentFees = new M_FeeDetail
                {
                    FDId = newFRId,
                    ClassId = req.ClassId,
                    stu_id = req.StudentId,
                    Status = "",
                    Remark = req.Remark,
                    Active = true,
                    OrderStatus = "1",
                    CompanyId = SchoolId,
                    Userid = UserId,
                    SessionId = SessionId,
                    Date = DateTime.Now.Date,
                    //  CreateDate = DateTime.Now,
                    //   UpdateDate = DateTime.Now,
                    PaymentDate = DateTime.Now.Date,
                    PaymentMode = req.PaymentMode,
                    PayFees = req.PayFees,
                    Cash = req.Cash,
                    Upi = req.Upi
                };

                // Generate ReceiptNo
                var existingReceipt = _context.M_FeeDetail
                    .Where(s => s.CompanyId == SchoolId)
                    .OrderByDescending(s => s.FDId)
                    .FirstOrDefault();

                if (existingReceipt == null)
                {
                    RStudentFees.ReceiptNo = "1";
                }
                else
                {
                    int rno = Convert.ToInt32(existingReceipt.ReceiptNo) + 1;
                    RStudentFees.ReceiptNo = rno.ToString();
                }

                _context.M_FeeDetail.Add(RStudentFees);
                await _context.SaveChangesAsync();

                var studentDetail = new StudentFeesRes
                {
                    receiptId = newFRId,

                };

                return ApiResponse<StudentFeesRes>.SuccessResponse(studentDetail, "Student fees inserted successfully.");
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


        public async Task<ApiResponse<PagedResult<StudentFeesCollectionListRes>>> GetDailyFeeCollection(FeesCollectionReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                // 1. Query banate hain
                var query = from fee in _context.M_FeeDetail
                            join student in _context.StudentRenewView
                                on fee.stu_id equals student.StuId
                            where fee.CompanyId == SchoolId
                                  && fee.SessionId == SessionId
                                  && student.CompanyId == SchoolId
                                  && student.SessionId == SessionId
                                  && fee.Active == true
                                  && (req.FromDate == null || fee.PaymentDate >= req.FromDate)
                                  && (req.ToDate == null || fee.PaymentDate <= req.ToDate)
                            select new StudentFeesCollectionListRes
                            {
                                ReceiptNo = fee.ReceiptNo,
                                ReceiptId = fee.FDId,
                                stu_name = student.stu_name,
                                srno = student.registration_no,
                                ClassName = _context.University.Where(a => a.university_id == student.ClassId && a.CompanyId == SchoolId).Select(a => a.university_name).FirstOrDefault(),
                                SectionName = _context.collegeinfo.Where(a => a.collegeid == student.SectionId && a.CompanyId == SchoolId).Select(a => a.collegename).FirstOrDefault(),
                                //  ClassName = student.ClassName,
                                // SectionName = student.SectionName,
                                fathername = student.father_name,
                                fathermobileno = student.father_mobile,
                                PayFees = fee.PayFees,
                                Remark = fee.Remark,
                                PaymentDate = fee.PaymentDate,
                                PaymentMode = fee.PaymentMode
                            };

                // 2. Total records count
                int totalRecords = await query.CountAsync();

                // 3. Pagination setup
                int pageNumber = req.PageNumber > 0 ? req.PageNumber : 1;
                int pageSize = req.PageSize > 0 ? req.PageSize : 10;

                // 4. Apply pagination
                var data = await query.OrderByDescending(x => x.PaymentDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                // 5. Prepare response
                var pagedResult = new PagedResult<StudentFeesCollectionListRes>
                {
                    Data = data,
                    TotalRecords = totalRecords,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
                };

                if (data == null || !data.Any())
                    return ApiResponse<PagedResult<StudentFeesCollectionListRes>>.ErrorResponse("No record found");

                return ApiResponse<PagedResult<StudentFeesCollectionListRes>>.SuccessResponse(pagedResult, "Receipt fetched successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<StudentFeesCollectionListRes>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }


        public async Task<ApiResponse<PagedResult<ClassFeesListRes>>> getclassfees(ClassFeesFilterReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                int pageNumber = req.PageNumber > 0 ? req.PageNumber : 1;
                int pageSize = req.PageSize > 0 ? req.PageSize : 10;

                // Filtered base query
                var baseQuery = from student in _context.StudentRenewView
                                where student.StuId == SchoolId && student.SessionId == SessionId
                                    && (!req.ClassId.HasValue || student.ClassId == req.ClassId)
                                join fee in _context.M_FeeDetail
                                    .Where(f => f.CompanyId == SchoolId && f.SessionId == SessionId &&
                                                f.Status == "1" && f.Active == true)
                                    on student.StuId equals fee.stu_id into feeGroup
                                from fg in feeGroup.DefaultIfEmpty()
                                group fg by student into g
                                select new ClassFeesListRes
                                {
                                    stu_name = g.Key.stu_name,
                                    srno = g.Key.registration_no,
                                    //     ClassName = g.Key.ClassName,
                                    //     SectionName = g.Key.SectionName,
                                    fathername = g.Key.father_name,
                                    fathermobileno = g.Key.father_mobile,
                                    RTE = g.Key.RTE,

                                    admission_fee = g.Key.admission_fee,
                                    PramoteFees = g.Key.PramoteFees,
                                    AFeeDiscount = g.Key.AFeeDiscount,
                                    AdmissionPayfee = g.Key.AdmissionPayfee,
                                    exam_fee = g.Key.exam_fee,
                                    Tution_fee = g.Key.tution_fee,
                                    Develoment_fee = g.Key.Develoment_fee,
                                    Games_fees = g.Key.Games_fees,
                                    total = g.Key.total,
                                    discount = g.Key.discount,
                                    OldDuefees = g.Key.OldDuefees,
                                    total_fee = g.Key.total_fee,

                                    TotalPaid = (decimal)g.Sum(x => x != null ? x.PayFees : 0)
                                };

                var allData = await baseQuery.ToListAsync();
                int totalRecords = allData.Count;

                var pagedData = allData.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                // Grand Totals
                var totals = new ClassFeesTotals
                {
                    TotalAdmissionFee = allData.Sum(x => (decimal?)x.admission_fee ?? 0),
                    TotalDiscount = allData.Sum(x => (decimal?)x.discount ?? 0),
                    TotalPaid = allData.Sum(x => x.TotalPaid),
                    TotalFee = allData.Sum(x => (decimal?)x.total_fee ?? 0)
                };

                var result = new PagedResult<ClassFeesListRes>
                {
                    Data = pagedData,
                    PageNumber = pageNumber,
                    Total = totals,
                    PageSize = pageSize,
                    TotalRecords = totalRecords,
                    TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
                };

                var response = ApiResponse<PagedResult<ClassFeesListRes>>.SuccessResponse(result, "Fee data fetched.");

                return response;
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<ClassFeesListRes>>.ErrorResponse("Something went wrong: " + ex.Message);
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



    }
}
