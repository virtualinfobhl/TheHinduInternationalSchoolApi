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
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Writers;
using System.Collections.Generic;
using System.ComponentModel.Design;
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


        public async Task<ApiResponse<PagedResult<GetStudentQuickListModel>>> GetQuickStudentReport(getstudentDellistReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var query = from student in _context.StudentRenewView
                            join cls in _context.University
                                on student.ClassId equals cls.university_id into classJoin
                            from cls in classJoin.DefaultIfEmpty()

                            join sec in _context.collegeinfo
                                on student.SectionId equals sec.collegeid into secJoin
                            from sec in secJoin.DefaultIfEmpty()

                            where student.SessionId == SessionId
                                  //   && student.stu_id == req.StudentId
                                  && student.CompanyId == SchoolId
                                  && student.RActive == true
                                  //   && _context.University.Any(u => u.university_id == req.ClassId && u.CompanyId == SchoolId && u.Active == true)
                                  && (student.StuDetail == false || student.StuFees == false)

                            select new GetStudentQuickListModel
                            {
                                stu_id = student.stu_id,

                                stu_name = student.stu_name,
                                stu_code = student.stu_code,
                                Srno = student.registration_no,
                                DOB = student.DOB,
                                admission_date = student.admission_date,
                                RTE = student.RTE,
                                FatherName = student.FatherName,
                                MotherName = student.MotherName,
                                FatherMobileNo = student.FatherMobileNo,
                                ClassId = student.ClassId,
                                SectionId = student.SectionId,
                                ClassName = cls != null ? cls.university_name : "",
                                SectionName = sec != null ? sec.collegename : "",
                            };

                // 🔎 Filters
                if (!string.IsNullOrEmpty(req.srno))
                    query = query.Where(p => p.Srno == req.srno);

                //if (req.ClassId.HasValue)
                //    query = query.Where(p => p.ClassId == req.ClassId);
                if (req.ClassId.HasValue && req.ClassId.Value > 0)
                {
                    query = query.Where(p => p.ClassId == req.ClassId.Value);
                }
                if (req.SectionId.HasValue && req.SectionId.Value > 0)
                {
                    query = query.Where(p => p.SectionId == req.SectionId.Value);
                }
                if (req.studentId.HasValue && req.studentId.Value > 0)
                {
                    query = query.Where(p => p.stu_id == req.studentId.Value);
                }


                int totalrecords = await query.CountAsync();

                int PageNumber = req.PageNumber > 0 ? req.PageNumber : 1;
                int PageSize = req.PageSize > 0 ? req.PageSize : 50;

                var data = await query
                    .OrderByDescending(p => p.stu_id)
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToListAsync();

                int totalpages = (int)Math.Ceiling((double)totalrecords / PageSize);


                var pagedResult = new PagedResult<GetStudentQuickListModel>
                {
                    Data = data,
                    TotalRecords = totalrecords,
                    PageNumber = PageNumber,
                    PageSize = PageSize,
                    TotalPages = totalpages
                };
                return ApiResponse<PagedResult<GetStudentQuickListModel>>.SuccessResponse(pagedResult, "Fetch student details data successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<GetStudentQuickListModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }

        }
        public async Task<ApiResponse<PagedResult<GetStudentDetailsLisModel>>> GetStudentDetailReport(GetStudentReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var query = from student in _context.StudentRenewView
                            join cls in _context.University
                                on student.ClassId equals cls.university_id into classJoin
                            from cls in classJoin.DefaultIfEmpty()

                            join sec in _context.collegeinfo
                                on student.SectionId equals sec.collegeid into secJoin
                            from sec in secJoin.DefaultIfEmpty()

                            where student.SessionId == SessionId
                                  && student.CompanyId == SchoolId
                                  && student.RActive == true
                                  && student.StuDetail == true
                                  && student.StuFees == true
                                   && student.Dropout == false

                            select new GetStudentDetailsLisModel
                            {
                                stu_id = student.stu_id,
                                stu_photo = student.stu_photo,
                                stu_name = student.stu_name,
                                stu_code = student.stu_code,
                                registration_no = student.registration_no,
                                DOB = student.DOB,
                                admission_date = student.admission_date,
                                RTE = student.RTE,
                                Rollno = student.RollNo,
                                FatherName = student.FatherName,
                                MotherName = student.MotherName,
                                FatherMobileNo = student.FatherMobileNo,
                                ClassId = student.ClassId,
                                SectionId = student.SectionId,
                                ClassName = cls != null ? cls.university_name : "",
                                SectionName = sec != null ? sec.collegename : "",
                                Address = student.address
                            };

                // 🔎 Filters
                if (!string.IsNullOrEmpty(req.srno))
                    query = query.Where(p => p.registration_no == req.srno);

                //if (req.ClassId.HasValue)
                //    query = query.Where(p => p.ClassId == req.ClassId);
                if (req.ClassId.HasValue && req.ClassId.Value > 0)
                {
                    query = query.Where(p => p.ClassId == req.ClassId.Value);
                }
                if (req.SectionId.HasValue && req.SectionId.Value > 0)
                {
                    query = query.Where(p => p.SectionId == req.SectionId.Value);
                }
                if (req.StudentId.HasValue && req.StudentId.Value > 0)
                {
                    query = query.Where(p => p.stu_id == req.StudentId.Value);
                }

                //if (req.Fromdate.HasValue)
                //    query = query.Where(p => p.admission_date >= req.Fromdate);

                //if (req.Todate.HasValue)
                //    query = query.Where(p => p.admission_date <= req.Todate);


                if (req.Fromdate.HasValue)
                {
                    query = query.Where(c => c.admission_date.HasValue && c.admission_date.Value >= req.Fromdate.Value);
                }

                if (req.Todate.HasValue)
                {
                    query = query.Where(c => c.admission_date.HasValue && c.admission_date.Value <= req.Todate.Value);
                }


                //  query = query.Where(c => (!req.Fromdate.HasValue || c.admission_date >= req.Fromdate) && (!req.Todate.HasValue || c.admission_date <= req.Todate));

                int totalrecords = await query.CountAsync();

                int PageNumber = req.PageNumber > 0 ? req.PageNumber : 1;
                int PageSize = req.PageSize > 0 ? req.PageSize : 50;

                var data = await query.OrderByDescending(p => p.stu_id).Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();

                int totalpages = (int)Math.Ceiling((double)totalrecords / PageSize);


                var pagedResult = new PagedResult<GetStudentDetailsLisModel>
                {
                    Data = data,
                    TotalRecords = totalrecords,
                    PageNumber = PageNumber,
                    PageSize = PageSize,
                    TotalPages = totalpages
                };
                return ApiResponse<PagedResult<GetStudentDetailsLisModel>>.SuccessResponse(pagedResult, "Fetch student details data successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<GetStudentDetailsLisModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }
        public async Task<ApiResponse<PagedResult<GetStudentDetailsLisModel>>> GetStudentIDCardReport(GetStudentIDCardReq req)
        {
            try
            {

                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var query = from student in _context.StudentRenewView
                            join cls in _context.University
                                on student.ClassId equals cls.university_id into classJoin
                            from cls in classJoin.DefaultIfEmpty()

                            join sec in _context.collegeinfo
                                on student.SectionId equals sec.collegeid into secJoin
                            from sec in secJoin.DefaultIfEmpty()

                            where student.SessionId == SessionId && student.CompanyId == SchoolId && student.RActive == true && student.StuDetail == true
                                  && student.StuFees == true && student.Dropout == false

                            select new GetStudentDetailsLisModel
                            {
                                stu_id = student.stu_id,
                                stu_photo = student.stu_photo,
                                stu_name = student.stu_name,
                                stu_code = student.stu_code,
                                registration_no = student.registration_no,
                                DOB = student.DOB,
                                admission_date = student.admission_date,
                                RTE = student.RTE,
                                FatherName = student.FatherName,
                                MotherName = student.MotherName,
                                FatherMobileNo = student.FatherMobileNo,
                                ClassId = student.ClassId,
                                SectionId = student.SectionId,
                                ClassName = cls != null ? cls.university_name : "",
                                SectionName = sec != null ? sec.collegename : "",
                                Address = student.address
                            };

                //if (req.ClassId.HasValue)
                //    query = query.Where(p => p.ClassId == req.ClassId);
                if (req.ClassId.HasValue && req.ClassId.Value > 0)
                {
                    query = query.Where(p => p.ClassId == req.ClassId.Value);
                }
                if (req.SectionId.HasValue && req.SectionId.Value > 0)
                {
                    query = query.Where(p => p.SectionId == req.SectionId.Value);
                }

                //if (req.SectionId.HasValue)
                //    query = query.Where(p => p.SectionId == req.SectionId);


                int totalrecords = await query.CountAsync();

                int PageNumber = req.PageNumber > 0 ? req.PageNumber : 1;
                int PageSize = req.PageSize > 0 ? req.PageSize : 50;

                var data = await query.OrderByDescending(p => p.stu_id).Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();

                int totalpages = (int)Math.Ceiling((double)totalrecords / PageSize);

                var pagedResult = new PagedResult<GetStudentDetailsLisModel>
                {
                    Data = data,
                    TotalRecords = totalrecords,
                    PageNumber = PageNumber,
                    PageSize = PageSize,
                    TotalPages = totalpages
                };
                return ApiResponse<PagedResult<GetStudentDetailsLisModel>>.SuccessResponse(pagedResult, "Fetch student  ID card report data successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<GetStudentDetailsLisModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }
        public async Task<ApiResponse<PagedResult<GetStudentFeeDetailsModel>>> GetStudentFeeReport(getstudentDellistReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var query = from student in _context.StudentRenewView
                            join cls in _context.University
                                on student.ClassId equals cls.university_id into classJoin
                            from cls in classJoin.DefaultIfEmpty()

                            join sec in _context.collegeinfo
                                on student.SectionId equals sec.collegeid into secJoin
                            from sec in secJoin.DefaultIfEmpty()

                            where student.SessionId == SessionId
                                  // && student.stu_id == req.studentId
                                  && student.CompanyId == SchoolId
                                  && student.RActive == true
                                  && student.StuDetail == true
                                  && student.StuFees == true
                                  && student.Dropout == false

                            select new GetStudentFeeDetailsModel
                            {
                                stu_id = student.stu_id,
                                stu_name = student.stu_name,
                                stu_code = student.stu_code,
                                registration_no = student.registration_no,
                                RTE = student.RTE,
                                FatherName = student.FatherName,
                                MotherName = student.MotherName,
                                FatherMobileNo = student.FatherMobileNo,
                                ClassId = student.ClassId,
                                SectionId = student.SectionId,
                                ClassName = cls != null ? cls.university_name : "",
                                SectionName = sec != null ? sec.collegename : "",
                                Rollno = student.RollNo,
                                PayAdmissionFee = student.RAdmissionPayfee,
                                PramoteFees = student.PramoteFees,
                                TotalFee = student.Rtotal,
                                FeeDiscount = student.Rdiscount,
                                DueOldFee = student.OldDuefees,
                                TotalNetFee = student.Rtotal_fee,
                                PaidFee = student.Rstu_fee,
                                DueFee = student.Rdue_fee,
                                NDHalfYearly = student.NDHalfYearly,
                                NDYearly = student.NDYearly,
                                UpdateNDdate = student.UpdateNDdate,

                                FeeReceipt = _context.M_FeeDetail.Where(a => a.stu_id == student.stu_id && a.ClassId == student.ClassId && a.SessionId == SessionId
                                && a.CompanyId == SchoolId && a.Active == true).Select(a => new FeeReceiptModel
                                {
                                    Receiptid = a.FDId,
                                    ReceiptNo = a.ReceiptNo,
                                    PayFees = a.PayFees,
                                    AdmissionPayfee = a.AdmissionPayfee,
                                    PramoteFees = a.PramoteFees,
                                    PaymentDate = a.PaymentDate,
                                    PaymentMode = a.PaymentMode,
                                    FeeType = a.Status,
                                    Remark = a.Remark,
                                }).ToList()

                            };

                //if (req.ClassId.HasValue)
                //    query = query.Where(p => p.ClassId == req.ClassId);
                if (req.ClassId.HasValue && req.ClassId.Value > 0)
                {
                    query = query.Where(p => p.ClassId == req.ClassId.Value);
                }
                if (req.SectionId.HasValue && req.SectionId.Value > 0)
                {
                    query = query.Where(p => p.SectionId == req.SectionId.Value);
                }
                if (req.studentId.HasValue && req.studentId.Value > 0)
                {
                    query = query.Where(p => p.stu_id == req.studentId.Value);
                }

                int totalrecords = await query.CountAsync();

                int PageNumber = req.PageNumber > 0 ? req.PageNumber : 1;
                int PageSize = req.PageSize > 0 ? req.PageSize : 50;

                var data = await query
                    .OrderByDescending(p => p.stu_id)
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToListAsync();

                int totalpages = (int)Math.Ceiling((double)totalrecords / PageSize);


                var pagedResult = new PagedResult<GetStudentFeeDetailsModel>
                {
                    Data = data,
                    TotalRecords = totalrecords,
                    PageNumber = PageNumber,
                    PageSize = PageSize,
                    TotalPages = totalpages
                };
                return ApiResponse<PagedResult<GetStudentFeeDetailsModel>>.SuccessResponse(pagedResult, "Fetch student fee data successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<GetStudentFeeDetailsModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }
        /*  public async Task<ApiResponse<bool>> SaveHalfYearlyNoDueFee1(List<HalfYearlyModel> res)
          {
              using var transaction = await _context.Database.BeginTransactionAsync();
              {
                  try
                  {
                      int SchoolId = _loginUser.SchoolId;
                      int UserId = _loginUser.UserId;
                      int SessionId = _loginUser.SessionId;

                      if (res == null || !res.Any())
                      {
                          return ApiResponse<bool>.ErrorResponse("No students selected");
                      }

                      foreach (var item in res)
                      {
                          var result = _context.Student_Renew.FirstOrDefault(s => s.StuId == item.StudentId && s.CompanyId == SchoolId && s.SessionId == SessionId);

                          if (result != null)
                          {
                              result.NDHalfYearly = true;
                              result.UpdateNDdate = DateTime.Now;
                          }
                      }


                      await _context.SaveChangesAsync();

                      await transaction.CommitAsync();
                      return ApiResponse<bool>.SuccessResponse(true, "Half Yearly No Duee Fee saved successfully");

                  }
                  catch (Exception ex)
                  {
                      await transaction.RollbackAsync();
                      return ApiResponse<bool>.ErrorResponse("Something went wrong: " + ex.Message);
                  }
              }
          }
  */




        //Updated 04-04-2025

        public async Task<ApiResponse<bool>> SaveHalfYearlyNoDueFee(List<HalfYearlyModel> res)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int schoolId = _loginUser.SchoolId;
                int sessionId = _loginUser.SessionId;

                if (res == null || !res.Any())
                {
                    return ApiResponse<bool>.ErrorResponse("No students selected");
                }

                var studentIds = res.Select(x => x.StudentId).ToList();

                // Fetch renew records
                var renewList = await _context.Student_Renew
                    .Where(s => s.StuId.HasValue &&
                                studentIds.Contains(s.StuId.Value) &&
                                s.CompanyId == schoolId &&
                                s.SessionId == sessionId)
                    .ToListAsync();

                // Fetch names from StudentRenewView
                var studentViewList = await _context.StudentRenewView
                    .Where(s => studentIds.Contains(s.StuId) &&
                                s.CompanyId == schoolId &&
                                s.SessionId == sessionId)
                    .Select(s => new
                    {
                        s.StuId,
                        s.stu_name
                    })
                    .ToListAsync();

                List<string> failedStudents = new List<string>();

                foreach (var item in res)
                {
                    var renew = renewList.FirstOrDefault(s => s.StuId == item.StudentId);

                    if (renew != null)
                    {
                        renew.NDHalfYearly = true;
                        renew.UpdateNDdate = DateTime.Now;
                    }
                    else
                    {
                        var studentName = studentViewList
                            .FirstOrDefault(s => s.StuId == item.StudentId)?.stu_name
                            ?? $"ID: {item.StudentId}";

                        failedStudents.Add(studentName);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                if (failedStudents.Any())
                {
                    return ApiResponse<bool>.SuccessResponse(
                        false,
                        $"Half Yearly No Due Fee failed for: {string.Join(", ", failedStudents)}"
                    );
                }

                return ApiResponse<bool>.SuccessResponse(
                    true,
                    "Half Yearly No Due Fee saved successfully"
                );
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ApiResponse<bool>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }



        /*        public async Task<ApiResponse<bool>> SaveYearlyNoDueFee1(List<YearlyModel> res)
                {
                    using var transaction = await _context.Database.BeginTransactionAsync();
                    {
                        try
                        {
                            int SchoolId = _loginUser.SchoolId;
                            int UserId = _loginUser.UserId;
                            int SessionId = _loginUser.SessionId;

                            if (res == null || !res.Any())
                            {
                                return ApiResponse<bool>.ErrorResponse("No students selected");
                            }

                            foreach (var item in res)
                            {
                                var result = _context.Student_Renew.FirstOrDefault(s => s.StuId == item.StudentId && s.CompanyId == SchoolId && s.SessionId == SessionId);

                                if (result != null)
                                {
                                    result.NDYearly = true;
                                    result.UpdateNDdate = DateTime.Now;
                                }
                            }

                            await _context.SaveChangesAsync();

                            await transaction.CommitAsync();
                            return ApiResponse<bool>.SuccessResponse(true, "Half Yearly No Duee Fee saved successfully");

                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            return ApiResponse<bool>.ErrorResponse("Something went wrong: " + ex.Message);
                        }
                    }
                }
        */


        //Updated 04-04-2025

        public async Task<ApiResponse<bool>> SaveYearlyNoDueFee(List<YearlyModel> res)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int schoolId = _loginUser.SchoolId;
                int sessionId = _loginUser.SessionId;

                if (res == null || !res.Any())
                {
                    return ApiResponse<bool>.ErrorResponse("No students selected");
                }

                var studentIds = res.Select(x => x.StudentId).ToList();

                // Fetch Student_Renew records
                var renewList = await _context.Student_Renew
                    .Where(s => s.StuId.HasValue &&
                                studentIds.Contains(s.StuId.Value) &&
                                s.CompanyId == schoolId &&
                                s.SessionId == sessionId)
                    .ToListAsync();

                // Fetch student names from view
                var studentViewList = await _context.StudentRenewView
                    .Where(s => studentIds.Contains(s.StuId) &&
                                s.CompanyId == schoolId &&
                                s.SessionId == sessionId)
                    .Select(s => new
                    {
                        s.StuId,
                        s.stu_name
                    })
                    .ToListAsync();

                List<string> failedStudents = new List<string>();

                foreach (var item in res)
                {
                    var renew = renewList.FirstOrDefault(s => s.StuId == item.StudentId);

                    if (renew != null)
                    {
                        renew.NDYearly = true; 
                        renew.UpdateNDdate = DateTime.Now;
                    }
                    else
                    {
                        var studentName = studentViewList
                            .FirstOrDefault(s => s.StuId == item.StudentId)?.stu_name
                            ?? $"ID: {item.StudentId}";

                        failedStudents.Add(studentName);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // If any failed
                if (failedStudents.Any())
                {
                    return ApiResponse<bool>.SuccessResponse(
                         false,
                        $"Yearly No Due Fee failed for: {string.Join(", ", failedStudents)}"
                    );
                }

                // All success
                return ApiResponse<bool>.SuccessResponse(
                    true,
                    "Yearly No Due Fee saved successfully"
                );
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ApiResponse<bool>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<PagedResult<GetStudentNoDueeFeeModel>>> GetStudentNoDuesFeeReport(GetStudentReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var query = from student in _context.StudentRenewView
                            join cls in _context.University
                                on student.ClassId equals cls.university_id into classJoin
                            from cls in classJoin.DefaultIfEmpty()

                            join sec in _context.collegeinfo
                                on student.SectionId equals sec.collegeid into secJoin
                            from sec in secJoin.DefaultIfEmpty()

                            where student.SessionId == SessionId
                                  && student.CompanyId == SchoolId
                                  && student.RActive == true
                                  && student.StuDetail == true
                                  && student.StuFees == true
                                   && student.Dropout == false
                                   && (student.NDHalfYearly == true || student.NDYearly == true)

                            select new GetStudentNoDueeFeeModel
                            {
                                stu_id = student.stu_id,
                                stu_name = student.stu_name,
                                stu_code = student.stu_code,
                                Srno = student.registration_no,
                                ClassId = student.ClassId,
                                SectionId = student.SectionId,
                                ClassName = cls != null ? cls.university_name : "",
                                SectionName = sec != null ? sec.collegename : "",
                                AdmissionPayFee = student.RAdmissionPayfee,
                                TotalFee = student.Rtotal,
                                Discount = student.Rdiscount,
                                TotalNetFee = student.Rtotal_fee,
                                DepositFee = student.Rstu_fee,
                                DueOldFee = student.OldDuefees,
                                DueFee = student.Rdue_fee,
                                NDHalfYearly = student.NDHalfYearly,
                                NDYearly = student.NDYearly,
                                UpdateNDdate = student.UpdateNDdate,
                            };

                // 🔎 Filters
                if (!string.IsNullOrEmpty(req.srno))
                    query = query.Where(p => p.Srno == req.srno);

                //if (req.ClassId.HasValue)
                //    query = query.Where(p => p.ClassId == req.ClassId);
                if (req.ClassId.HasValue && req.ClassId.Value > 0)
                {
                    query = query.Where(p => p.ClassId == req.ClassId.Value);
                }
                if (req.SectionId.HasValue && req.SectionId.Value > 0)
                {
                    query = query.Where(p => p.SectionId == req.SectionId.Value);
                }
                if (req.StudentId.HasValue && req.StudentId.Value > 0)
                {
                    query = query.Where(p => p.stu_id == req.StudentId.Value);
                }

                if (req.Fromdate.HasValue)
                {
                    query = query.Where(c => c.UpdateNDdate.HasValue && c.UpdateNDdate.Value >= req.Fromdate.Value);
                }

                if (req.Todate.HasValue)
                {
                    query = query.Where(c => c.UpdateNDdate.HasValue && c.UpdateNDdate.Value <= req.Todate.Value);
                }


                //  query = query.Where(c => (!req.Fromdate.HasValue || c.admission_date >= req.Fromdate) && (!req.Todate.HasValue || c.admission_date <= req.Todate));

                int totalrecords = await query.CountAsync();

                int PageNumber = req.PageNumber > 0 ? req.PageNumber : 1;
                int PageSize = req.PageSize > 0 ? req.PageSize : 50;

                var data = await query.OrderByDescending(p => p.stu_id).Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();

                int totalpages = (int)Math.Ceiling((double)totalrecords / PageSize);


                var pagedResult = new PagedResult<GetStudentNoDueeFeeModel>
                {
                    Data = data,
                    TotalRecords = totalrecords,
                    PageNumber = PageNumber,
                    PageSize = PageSize,
                    TotalPages = totalpages
                };
                return ApiResponse<PagedResult<GetStudentNoDueeFeeModel>>.SuccessResponse(pagedResult, "Fetch student no duee fee data successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<GetStudentNoDueeFeeModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }
        public async Task<ApiResponse<PagedResult<ClasswiseInstallModel>>> GetClassWiseInstallmentReport(BulkStudentReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var query = from student in _context.StudentRenewView
                            join cls in _context.University
                                on student.ClassId equals cls.university_id into classJoin
                            from cls in classJoin.DefaultIfEmpty()

                            join sec in _context.collegeinfo
                                on student.SectionId equals sec.collegeid into secJoin
                            from sec in secJoin.DefaultIfEmpty()

                            where student.SessionId == SessionId
                                  && student.CompanyId == SchoolId
                                  && student.RActive == true
                                  && student.StuDetail == true
                                  && student.StuFees == true
                                 && student.Dropout == false

                            select new ClasswiseInstallModel
                            {
                                stu_id = student.StuId,
                                ClassId = student.ClassId,
                                SectionId = student.SectionId,
                                stu_name = student.stu_name,
                                registration_no = student.registration_no,
                                RTE = student.RTE,
                                // stu_code = student.stu_code,
                                FatherName = student.FatherName,
                                FatherMobileNo = student.FatherMobileNo,
                                MotherName = student.mother_name,
                                TotalNetFee = student.Rtotal_fee,
                                PaidFee = student.Rstu_fee,
                                DueFee = student.Rdue_fee,
                                ClassName = cls != null ? cls.university_name : "",
                                SectionName = sec != null ? sec.collegename : "",

                                Installments = _context.fee_installment.Where(a => a.stu_id == student.StuId && a.university_id == student.ClassId && a.SessionId == SessionId
                                && a.CompanyId == SchoolId && a.active == true)
                                .Select(a => new ClasswiseInstallmentModel
                                {
                                    Installment = a.Installment,
                                    SInsAmount = a.due_fee,
                                }).ToList()

                            };

                //if (req.ClassId.HasValue)
                //    query = query.Where(p => p.ClassId == req.ClassId);
                if (req.ClassId.HasValue && req.ClassId.Value > 0)
                {
                    query = query.Where(p => p.ClassId == req.ClassId.Value);
                }
                if (req.SectionId.HasValue && req.SectionId.Value > 0)
                {
                    query = query.Where(p => p.SectionId == req.SectionId.Value);
                }

              

                int totalrecords = await query.CountAsync();

                int PageNumber = req.PageNumber > 0 ? req.PageNumber : 1;
                int PageSize = req.PageSize > 0 ? req.PageSize : 50;

                var data = await query
                    .OrderByDescending(p => p.stu_id)
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToListAsync();

                int totalpages = (int)Math.Ceiling((double)totalrecords / PageSize);

               
                var pagedResult = new PagedResult<ClasswiseInstallModel>
                {
                    Data = data,
                    TotalRecords = totalrecords,
                    PageNumber = PageNumber,
                    PageSize = PageSize,
                    TotalPages = totalpages
                };
                return ApiResponse<PagedResult<ClasswiseInstallModel>>.SuccessResponse(pagedResult, "Fetch student Class Wise Installment data successfully.");

            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<ClasswiseInstallModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }
        public async Task<ApiResponse<PagedResult<ClasswiseDueeFeeModel>>> GetClasswiseDueFeeReport(BulkStudentReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var query = from student in _context.StudentRenewView
                            join cls in _context.University
                                on student.ClassId equals cls.university_id into classJoin
                            from cls in classJoin.DefaultIfEmpty()

                            join sec in _context.collegeinfo
                                on student.SectionId equals sec.collegeid into secJoin
                            from sec in secJoin.DefaultIfEmpty()

                            where student.SessionId == SessionId
                                  && student.CompanyId == SchoolId
                                  && student.RActive == true
                                  && student.StuDetail == true
                                  && student.StuFees == true
                                 && student.Dropout == false

                            select new ClasswiseDueeFeeModel
                            {
                                stu_id = student.StuId,
                                classid = student.ClassId,
                                sectionid = student.SectionId,
                                stu_name = student.stu_name,
                                Srno = student.registration_no,
                                FatherName = student.FatherName,
                                FatherMobileNo = student.FatherMobileNo,
                                MotherName = student.mother_name,
                                DueFee = student.Rdue_fee,
                                ClassName = cls != null ? cls.university_name : "",
                                SectionName = sec != null ? sec.collegename : "",

                            };

                //if (req.ClassId.HasValue)
                //    query = query.Where(p => p.ClassId == req.ClassId);
                if (req.ClassId.HasValue && req.ClassId.Value > 0)
                {
                    query = query.Where(p => p.classid == req.ClassId.Value);
                }
                if (req.SectionId.HasValue && req.SectionId.Value > 0)
                {
                    query = query.Where(p => p.sectionid == req.SectionId.Value);
                }

                //if (req.SectionId.HasValue)
                //    query = query.Where(p => p.SectionId == req.SectionId);

                int totalrecords = await query.CountAsync();

                int PageNumber = req.PageNumber > 0 ? req.PageNumber : 1;
                int PageSize = req.PageSize > 0 ? req.PageSize : 50;

                var data = await query
                    .OrderByDescending(p => p.stu_id)
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToListAsync();

                int totalpages = (int)Math.Ceiling((double)totalrecords / PageSize);

                var pagedResult = new PagedResult<ClasswiseDueeFeeModel>
                {
                    Data = data,
                    TotalRecords = totalrecords,
                    PageNumber = PageNumber,
                    PageSize = PageSize,
                    TotalPages = totalpages
                };
                return ApiResponse<PagedResult<ClasswiseDueeFeeModel>>.SuccessResponse(pagedResult, "Fetch student Class Wise Installment data successfully.");

            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<ClasswiseDueeFeeModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }

        }
        //public async Task<ApiResponse<List<AllClasswiseDueeFeeModel>>> GetAllClasswiseDueFeeReport(BulkStudentReq req)
        public async Task<ApiResponse<PagedResult<AllClasswiseDueeFeeModel>>> GetAllClasswiseDueFeeReport(BulkStudentReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var startDate = await _context.StuRouteAssignTbl.Where(c => c.university_id == req.ClassId && c.CompanyId == SchoolId && c.SessionId == SessionId)
                    .Select(c => c.Date).FirstOrDefaultAsync();

                int startMonthNo = startDate?.Month ?? 1;
                int currentMonthNo = DateTime.Now.Month;

                var validMonths = Enumerable.Range(startMonthNo, currentMonthNo - startMonthNo + 1)
                    .Select(m => new DateTime(DateTime.Now.Year, m, 1).ToString("MMMM")).ToList();

                // 🔥 STEP 1: QUERY (IQueryable rakho, ToListAsync mat lagao abhi)
                var query = _context.StudentRenewView.Where(c => (req.ClassId == -1 ? true : c.ClassId == req.ClassId) && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) &&
                        c.RActive == true && c.StuDetail == true && c.StuFees == true && c.SessionId == SessionId && c.CompanyId == SchoolId).OrderBy(c => c.stu_name)
                    .Select(c => new AllClasswiseDueeFeeModel
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

                        TransportDueFee = _context.TransInstallmentTbl.Where(a => a.StuId == c.StuId && a.CompanyId == SchoolId && validMonths.Contains(a.MonthName))
                                .Sum(a => a.DueFee) + _context.StuRouteAssignTbl.Where(a => a.stu_id == c.StuId && a.CompanyId == SchoolId && a.SessionId == SessionId)
                                 .Sum(a => a.OldDueFee),

                    });

                // 🔥 STEP 2: TOTAL RECORDS
                int totalRecords = await query.CountAsync();

                // 🔥 STEP 3: PAGINATION LOGIC
                int PageNumber = req.PageNumber > 0 ? req.PageNumber : 1;
                int PageSize = req.PageSize > 0 ? req.PageSize : 50;

                var data = await query
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToListAsync();

                int totalPages = (int)Math.Ceiling((double)totalRecords / PageSize);

                // 🔥 STEP 4: FINAL RESULT
                var pagedResult = new PagedResult<AllClasswiseDueeFeeModel>
                {
                    Data = data,
                    TotalRecords = totalRecords,
                    PageNumber = PageNumber,
                    PageSize = PageSize,
                    TotalPages = totalPages
                };

                return ApiResponse<PagedResult<AllClasswiseDueeFeeModel>>
                    .SuccessResponse(pagedResult, "Fetch student fee data successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<AllClasswiseDueeFeeModel>>
                    .ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

       
        public async Task<ApiResponse<PagedResult<GetStudentTCLisModel>>> GetStudentTCReport(GetStudentIDCardReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var query = from student in _context.StudentRenewView
                            join cls in _context.University
                                on student.ClassId equals cls.university_id into classJoin
                            from cls in classJoin.DefaultIfEmpty()

                            join sec in _context.collegeinfo
                                on student.SectionId equals sec.collegeid into secJoin
                            from sec in secJoin.DefaultIfEmpty()

                            where student.SessionId == SessionId
                                  && student.CompanyId == SchoolId
                                  && student.RActive == false
                                  && student.StuDetail == true
                                  && student.StuFees == true

                            select new GetStudentTCLisModel
                            {
                                stu_id = student.StuId,
                                stu_photo = student.stu_photo,
                                RTE = student.RTE,
                                DOB = student.DOB,
                                Address = student.address,
                                ClassId = student.ClassId,
                                SectionId = student.SectionId,
                                stu_name = student.stu_name,
                                registration_no = student.registration_no,
                                FatherName = student.FatherName,
                                FatherMobileNo = student.FatherMobileNo,
                                MotherName = student.mother_name,
                                ClassName = cls != null ? cls.university_name : "",
                                SectionName = sec != null ? sec.collegename : "",

                            };

                //if (req.ClassId.HasValue)
                //    query = query.Where(p => p.ClassId == req.ClassId);
                if (req.ClassId.HasValue && req.ClassId.Value > 0)
                {
                    query = query.Where(p => p.ClassId == req.ClassId.Value);
                }
                if (req.SectionId.HasValue && req.SectionId.Value > 0)
                {
                    query = query.Where(p => p.SectionId == req.SectionId.Value);
                }

                //if (req.SectionId.HasValue)
                //    query = query.Where(p => p.SectionId == req.SectionId);

                int totalrecords = await query.CountAsync();

                int PageNumber = req.PageNumber > 0 ? req.PageNumber : 1;
                int PageSize = req.PageSize > 0 ? req.PageSize : 50;

                var data = await query
                    .OrderByDescending(p => p.stu_id)
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToListAsync();

                int totalpages = (int)Math.Ceiling((double)totalrecords / PageSize);

                var pagedResult = new PagedResult<GetStudentTCLisModel>
                {
                    Data = data,
                    TotalRecords = totalrecords,
                    PageNumber = PageNumber,
                    PageSize = PageSize,
                    TotalPages = totalpages
                };

                if (!data.Any())
                {
                    return ApiResponse<PagedResult<GetStudentTCLisModel>>.ErrorResponse("Student TC data not found");
                }

                return ApiResponse<PagedResult<GetStudentTCLisModel>>.SuccessResponse(pagedResult, "Fetch student Class Wise Installment data successfully.");

            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<GetStudentTCLisModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }
        public async Task<ApiResponse<PagedResult<GetStudentDROPOUTLisModel>>> GetStudentDropoutReport(GetStudentIDCardReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var query = from student in _context.StudentRenewView
                            join cls in _context.University
                                on student.ClassId equals cls.university_id into classJoin
                            from cls in classJoin.DefaultIfEmpty()

                            join sec in _context.collegeinfo
                                on student.SectionId equals sec.collegeid into secJoin
                            from sec in secJoin.DefaultIfEmpty()

                            where student.SessionId == SessionId
                                  && student.CompanyId == SchoolId
                                  && student.StuDetail == true
                                  && student.StuFees == true
                                 && student.Dropout == true

                            select new GetStudentDROPOUTLisModel
                            {
                                stu_id = student.StuId,
                                stu_photo = student.stu_photo,
                                RTE = student.RTE,
                                DOB = student.DOB,
                                Address = student.address,
                                ClassId = student.ClassId,
                                SectionId = student.SectionId,
                                stu_name = student.stu_name,
                                registration_no = student.registration_no,
                                FatherName = student.FatherName,
                                FatherMobileNo = student.FatherMobileNo,
                                MotherName = student.mother_name,
                                ClassName = cls != null ? cls.university_name : "",
                                SectionName = sec != null ? sec.collegename : "",

                            };

                //if (req.ClassId.HasValue)
                //    query = query.Where(p => p.ClassId == req.ClassId);
                if (req.ClassId.HasValue && req.ClassId.Value > 0)
                {
                    query = query.Where(p => p.ClassId == req.ClassId.Value);
                }
                if (req.SectionId.HasValue && req.SectionId.Value > 0)
                {
                    query = query.Where(p => p.SectionId == req.SectionId.Value);
                }

                //if (req.SectionId.HasValue)
                //    query = query.Where(p => p.SectionId == req.SectionId);

                int totalrecords = await query.CountAsync();

                int PageNumber = req.PageNumber > 0 ? req.PageNumber : 1;
                int PageSize = req.PageSize > 0 ? req.PageSize : 50;

                var data = await query
                    .OrderByDescending(p => p.stu_id)
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToListAsync();

                int totalpages = (int)Math.Ceiling((double)totalrecords / PageSize);

                var pagedResult = new PagedResult<GetStudentDROPOUTLisModel>
                {
                    Data = data,
                    TotalRecords = totalrecords,
                    PageNumber = PageNumber,
                    PageSize = PageSize,
                    TotalPages = totalpages
                };

                //if (pagedResult.Data == null)
                //{
                //    return ApiResponse<PagedResult<GetStudentDROPOUTLisModel>>.ErrorResponse("Not found data");
                //}
                if (!data.Any())
                {
                    return ApiResponse<PagedResult<GetStudentDROPOUTLisModel>>.ErrorResponse("Student dropout data not found");
                }

                return ApiResponse<PagedResult<GetStudentDROPOUTLisModel>>.SuccessResponse(pagedResult, "Fetch student dropout data successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<GetStudentDROPOUTLisModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        // exam section
        public async Task<ApiResponse<List<TestExamMarksmOdel>>> GetTestExamMarks(GetTestExamReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.StudentRenewView.Where(c => (c.ClassId == req.ClassId)
                && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && c.RActive == true && c.StuDetail == true && c.StuFees == true && c.Dropout == false
                && c.SessionId == SessionId && c.CompanyId == SchoolId).OrderBy(c => c.stu_name).Select(c => new TestExamMarksmOdel
                {
                    stu_id = c.StuId,
                    stu_name = c.stu_name,
                    RollNo = c.RollNo,

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


                var res = await _context.StudentRenewView.Where(c => (c.ClassId == req.ClassId)
                && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && c.RActive == true && c.StuDetail == true && c.StuFees == true && c.Dropout == false
                && c.SessionId == SessionId && c.CompanyId == SchoolId).OrderBy(c => c.stu_name).Select(c => new TestExamMarksmOdel
                {
                    stu_id = c.StuId,
                    stu_name = c.stu_name,
                    RollNo = c.RollNo,
                    Srno = c.registration_no,

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
                    return ApiResponse<List<TestExamMarksmOdel>>.ErrorResponse("No student Total Test Exam Report found data");
                }

                return ApiResponse<List<TestExamMarksmOdel>>.SuccessResponse(res, "Fetch student Total Test Exam Report data successfully");
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

                var res = await _context.StudentRenewView.Where(c => (c.ClassId == req.ClassId)
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
                    //MaxTotal = _context.TestExamTbl.Where(a => a.university_id == c.ClassId && a.stu_id == c.StuId && allowedTestTypes.Contains(a.TestType) &&
                    //a.CompanyId == SchoolId && a.SessionId == SessionId).Select(a => new
                    //{
                    //    Marks = _context.Subject.Where(p => p.university_id == a.university_id && p.subject_id == a.subject_id && p.Marks_Type == a.MarksType &&
                    //    p.SessionId == SessionId && p.CompanyId == SchoolId).Select(p => a.TestType == "Quarterly" ? p.Quarterly : a.TestType == "first_test" ? p.first_test :
                    //    a.TestType == "second_test" ? p.second_test : a.TestType == "half_yearly" ? p.half_yearly : a.TestType == "third_test" ? p.third_test :
                    //    a.TestType == "fourth_test" ? p.fourth_test : a.TestType == "yearly" ? p.yearly : 0).FirstOrDefault()
                    //}).Sum(x => x.Marks),


                    MaxTotal = _context.TestExamTbl.Where(a => a.university_id == c.ClassId && a.stu_id == c.StuId && allowedTestTypes.Contains(a.TestType)
                    && a.CompanyId == SchoolId && a.SessionId == SessionId).Select(a => new
                    {
                        Marks = _context.Subject.Where(p => p.university_id == a.university_id && p.subject_id == a.subject_id && p.Marks_Type == a.MarksType
                        && p.SessionId == SessionId && p.CompanyId == SchoolId).Select(p => a.TestType == "Quarterly" ? p.Quarterly :
                        a.TestType == "first_test" ? p.first_test : a.TestType == "second_test" ? p.second_test : a.TestType == "half_yearly" ? p.half_yearly :
                        a.TestType == "third_test" ? p.third_test : a.TestType == "fourth_test" ? p.fourth_test : a.TestType == "yearly" ? p.yearly : 0).FirstOrDefault()
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
        public async Task<ApiResponse<bool>> CourseCompleted(List<CourseCompletedmodel> res)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            {
                try
                {
                    int SchoolId = _loginUser.SchoolId;
                    int UserId = _loginUser.UserId;
                    int SessionId = _loginUser.SessionId;

                    if (res == null || !res.Any())
                    {
                        return ApiResponse<bool>.ErrorResponse("No students selected");
                    }

                    foreach (var item in res)
                    {
                        var result = _context.Student_Renew.FirstOrDefault(s => s.StuId == item.StudentId && s.CompanyId == SchoolId && s.SessionId == SessionId);

                        if (result != null)
                        {
                            result.completed = true;
                        }
                    }

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return ApiResponse<bool>.SuccessResponse(true, "Student Course Completed saved successfully");

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return ApiResponse<bool>.ErrorResponse("Something went wrong: " + ex.Message);
                }
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

        public async Task<ApiResponse<List<GetStudentQuickListModel>>> GetQuickStudentReport1(getstudentDellistReq req)
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
        public async Task<ApiResponse<List<GetStudentDetailsLisModel>>> GetStudentDetailReport1(GetStudentReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.StudentRenewView.Where(c => (req.ClassId == -1 ? true : c.ClassId == req.ClassId)
                && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && (req.StudentId == -1 ? true : c.StuId == req.StudentId)
                && (string.IsNullOrEmpty(req.srno) || c.registration_no == req.srno) && c.Dropout == false && c.RActive == true && c.StuDetail == true
                && c.StuFees == true && c.SessionId == SessionId && c.CompanyId == SchoolId)
                    .OrderBy(c => c.stu_name).Select(c => new GetStudentDetailsLisModel
                    {
                        stu_id = c.StuId,
                        stu_photo = c.stu_photo,
                        ClassId = c.ClassId,
                        SectionId = c.SectionId,
                        stu_name = c.stu_name,
                        registration_no = c.registration_no,
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
        public async Task<ApiResponse<List<GetStudentDetailsLisModel>>> GetStudentIDCardReport1(GetStudentIDCardReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.StudentRenewView.Where(c => (req.ClassId == -1 ? true : c.ClassId == req.ClassId)
                && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && c.RActive == true && c.StuDetail == true && c.StuFees == true && c.Dropout == false
                && c.SessionId == SessionId && c.CompanyId == SchoolId).OrderBy(c => c.stu_name).Select(c => new GetStudentDetailsLisModel
                {
                    stu_id = c.StuId,
                    stu_photo = c.stu_photo,
                    stu_name = c.stu_name,
                    registration_no = c.registration_no,
                    stu_code = c.stu_code,
                    Address = c.address,
                    DOB = c.DOB,
                    RTE = c.RTE,
                    admission_date = c.admission_date,
                    FatherName = c.FatherName,
                    FatherMobileNo = c.FatherMobileNo,
                    MotherName = c.mother_name,
                    ClassId = c.ClassId,
                    SectionId = c.SectionId,

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
        public async Task<ApiResponse<List<GetStudentFeeDetailsModel>>> GetStudentFeeReport1(getstudentDellistReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.StudentRenewView.Where(c => (req.ClassId == -1 ? true : c.ClassId == req.ClassId)
                && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && (req.studentId == -1 ? true : c.StuId == req.studentId)
                && (string.IsNullOrEmpty(req.srno) || c.registration_no == req.srno) && c.RActive == true && c.StuDetail == true && c.StuFees == true && c.Dropout == false
                && c.SessionId == SessionId && c.CompanyId == SchoolId).OrderBy(c => c.stu_name).Select(c => new GetStudentFeeDetailsModel
                {
                    stu_id = c.StuId,
                    RTE = c.RTE,
                    ClassId = c.ClassId,
                    SectionId = c.SectionId,
                    stu_name = c.stu_name,
                    Rollno = c.RollNo,
                    registration_no = c.registration_no,
                    stu_code = c.stu_code,
                    FatherName = c.FatherName,
                    FatherMobileNo = c.FatherMobileNo,
                    MotherName = c.mother_name,
                    PramoteFees = c.PramoteFees,
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
                        AdmissionPayfee = a.AdmissionPayfee,
                        PramoteFees = a.PramoteFees,
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

        public async Task<ApiResponse<List<ClasswiseInstallModel>>> GetClassWiseInstallmentReport1(BulkStudentReq req)
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
                    registration_no = c.registration_no,
                    RTE = c.RTE,
                    //   stu_code = c.stu_code,
                    FatherName = c.FatherName,
                    FatherMobileNo = c.FatherMobileNo,
                    MotherName = c.mother_name,
                    TotalNetFee = c.Rtotal_fee,
                    PaidFee = c.Rstu_fee,
                    DueFee = c.Rdue_fee,


                    ClassName = _context.University.Where(a => a.university_id == c.ClassId && a.CompanyId == SchoolId).Select(a => a.university_name).FirstOrDefault(),
                    SectionName = _context.collegeinfo.Where(a => a.collegeid == c.SectionId && a.CompanyId == SchoolId).Select(a => a.collegename).FirstOrDefault(),

                    Installments = _context.fee_installment.Where(a => a.stu_id == c.StuId && a.university_id == c.ClassId && a.SessionId == SessionId
                    && a.CompanyId == SchoolId && a.active == true)
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
        public async Task<ApiResponse<List<ClasswiseDueeFeeModel>>> GetClasswiseDueFeeReport1(BulkStudentReq req)
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

        public async Task<ApiResponse<List<GetStudentDetailsLisModel>>> GetStudentTCReport1(GetStudentIDCardReq req)
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
                    RTE = c.RTE,
                    stu_photo = c.stu_photo,
                    ClassId = c.ClassId,
                    SectionId = c.SectionId,
                    stu_name = c.stu_name,
                    registration_no = c.registration_no,
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
        public async Task<ApiResponse<List<GetStudentDetailsLisModel>>> GetStudentDropoutReport1(GetStudentIDCardReq req)
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
                    RTE = c.RTE,
                    stu_photo = c.stu_photo,
                    ClassId = c.ClassId,
                    SectionId = c.SectionId,
                    stu_name = c.stu_name,
                    registration_no = c.registration_no,
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

    }
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



