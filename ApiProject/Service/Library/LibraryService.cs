using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using ApiProject.Service.Current;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using OfficeOpenXml.Table.PivotTable;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;

namespace ApiProject.Service.Library
{
    public class LibraryService : ILibraryService
    {
        private readonly ILoginUserService _loginUser;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LibraryService(ILoginUserService loginUser, ApplicationDbContext context, IMapper mapper)
        {
            _loginUser = loginUser;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<getBooks>>> GetBoodData()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var BookEntity = await _context.BooksTbl.Where(c => c.CompanyId == SchoolId)
                    .Select(c => new getBooks
                    {
                        BookId = c.BookId,
                        BookTitle = c.BookTitle,
                        BookNumber = c.BookNumber,
                        Publisher = c.Publisher,
                        Author = c.Author,
                        Subject = c.Subject,
                        TotalQuantity = c.TotalQuantity,
                        BookPrice = c.BookPrice,
                        Description = c.Description,
                        Date = c.Date,
                        Active = c.Active,
                    }).ToListAsync();
                return ApiResponse<List<getBooks>>.SuccessResponse(BookEntity, "Book list fetched successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<getBooks>>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> Addbook(AddBookReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                int BookId = _context.BooksTbl.DefaultIfEmpty().Max(s => s == null ? 0 : s.BookId) + 1;

                var book = new BooksTbl
                {
                    BookId = BookId,
                    BookTitle = req.BookTitle,
                    BookNumber = req.BookNumber,
                    Publisher = req.Publisher,
                    Author = req.Author,
                    Subject = req.Subject,
                    TotalQuantity = req.TotalQuantity,
                    BookPrice = req.BookPrice,
                    Description = req.Description,
                    Date = req.Date,
                    Active = true,
                    //     CreateDate = DateTime.Now,
                    //     UpdateDate = DateTime.Now,
                    CompanyId = SchoolId,
                    SessionId = SessionId,
                    Userid = UserId,
                };

                _context.BooksTbl.Add(book);
                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "Book data saved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> Updatebook(UpdateBookReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;

                var result = await _context.BooksTbl.Where(c => c.BookId == req.BookId && c.CompanyId == SchoolId).FirstOrDefaultAsync();

                result.BookTitle = req.BookTitle;
                result.BookNumber = req.BookNumber;
                result.Publisher = req.Publisher;
                result.Author = req.Author;
                result.Subject = req.Subject;
                result.TotalQuantity = req.TotalQuantity;
                result.BookPrice = req.BookPrice;
                result.Description = req.Description;
                result.Date = req.Date;
                result.Userid = UserId;
                //    result.UpdateDate = DateTime.Now;
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Book update successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> ChangeStatusBook(int BookId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;

                var Bookentity = await _context.BooksTbl.FirstOrDefaultAsync(p => p.BookId == BookId && p.CompanyId == SchoolId);
                if (Bookentity == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Employee record not found ");
                }

                Bookentity.Active = Bookentity.Active == null ? true : !Bookentity.Active;

                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "status update successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<GetLibraryStudentModel>>> GetLibraryStudent(BulkStudentReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.StudentRenewView.Where(a => (req.ClassId == -1 ? true : a.ClassId == req.ClassId)
                && (req.SectionId == -1 ? true : a.SectionId == req.SectionId) && a.CompanyId == SchoolId
                     && a.SessionId == SessionId && a.StuDetail == true && a.StuFees == true && a.RActive == true)
                     .Select(a => new GetLibraryStudentModel
                     {
                         MemberId = _context.LibraryCardTbl.Where(c => c.stu_id == a.StuId && c.university_id == a.ClassId).Select(c => c.MemberId).FirstOrDefault(),
                         CardNo = _context.LibraryCardTbl.Where(c => c.stu_id == a.StuId && c.university_id == a.ClassId).Select(c => c.LibraryCardNo).FirstOrDefault(),
                         StudentId = a.StuId,
                         SRNo = a.registration_no,
                         stu_name = a.stu_name,
                         father_name = a.father_name,
                         father_mobile = a.father_mobile,
                         mother_name = a.mother_name,
                         ClassId = a.ClassId,
                         SectionId = a.SectionId,
                         ClassName = _context.University.Where(c => c.university_id == a.ClassId && c.CompanyId == SchoolId).Select(c => c.university_name).FirstOrDefault(),
                         SectionName = _context.collegeinfo.Where(c => c.collegeid == a.SectionId && c.CompanyId == SchoolId).Select(c => c.collegename).FirstOrDefault(),

                     }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<GetLibraryStudentModel>>.ErrorResponse("Classwise installment not found ");
                }
                return ApiResponse<List<GetLibraryStudentModel>>.SuccessResponse(res, "Fetch classwise installment data");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetLibraryStudentModel>>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> AddLibraryCardNo(AddcardNoReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;
                int UserId = _loginUser.UserId;

                var existing = await _context.LibraryCardTbl.FirstOrDefaultAsync(p => p.LibraryCardNo == req.LibraryCardNo && p.CompanyId == SchoolId);
                if (existing != null)
                {
                    return ApiResponse<bool>.ErrorResponse("Library Card No. is already insert");
                }
                int LibraryId = _context.LibraryCardTbl.DefaultIfEmpty().Max(s => s == null ? 0 : s.LibraryId) + 1;

                var cardentity = new LibraryCardTbl
                {
                    LibraryId = LibraryId,
                    LibraryCardNo = req.LibraryCardNo,
                    MemberId = LibraryId,
                    stu_id = req.StudentId,
                    university_id = req.ClassId,
                    SectionId = req.SectionId,
                    Emp_Id = req.Emp_Id,
                    MemberType = req.MemberType,
                    Active = true,
                    //   CreateDate = DateTime.Now,
                    //    UpdateDate = DateTime.Now,
                    CompanyId = SchoolId,
                    SessionId = SessionId,
                    Userid = UserId,
                };

                _context.LibraryCardTbl.Add(cardentity);
                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "Library Card No. saved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<GetlibrarayEmployeeModel>>> GetLibraryEmployee()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var EmpEntity = await _context.EmployeeRegister.Where(c => c.CompanyId == SchoolId)
                    .Select(c => new GetlibrarayEmployeeModel
                    {
                        MemberId = _context.LibraryCardTbl.Where(a => a.Emp_Id == c.Emp_Id).Select(a => a.MemberId).FirstOrDefault(),
                        CardNo = _context.LibraryCardTbl.Where(a => a.Emp_Id == c.Emp_Id).Select(a => a.LibraryCardNo).FirstOrDefault(),

                        Emp_Id = c.Emp_Id,
                        Emp_Name = c.Emp_Name,
                        DOB = c.DOB,
                        Mobileno = c.Mobileno,
                        Gendar = c.Gendar,
                        EmailId = c.EmailId,

                    }).ToListAsync();
                return ApiResponse<List<GetlibrarayEmployeeModel>>.SuccessResponse(EmpEntity, "Book list fetched successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetlibrarayEmployeeModel>>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<GetMemberListModel>>> GetMemberList()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var EmpEntity = await _context.LibraryCardTbl.Where(c => c.CompanyId == SchoolId)
                    .Select(c => new GetMemberListModel
                    {
                        MemberId = c.MemberId,
                        CardNo = c.LibraryCardNo,
                        MemberType = c.MemberType,
                        ClassName = _context.University.Where(a => a.university_id == c.university_id && a.CompanyId == SchoolId).Select(a => a.university_name).FirstOrDefault(),
                        SectionName = _context.collegeinfo.Where(a => c.SectionId == c.SectionId && c.CompanyId == SchoolId).Select(c => c.collegename).FirstOrDefault(),

                        Student = _context.StudentRenewView.Where(a => a.StuId == c.stu_id && a.CompanyId == SchoolId).Select(a => new GetStuDetail
                        {
                            StudentId = a.StuId,
                            stu_name = a.stu_name,
                            SRNo = a.registration_no,
                            StuMobileno = a.father_mobile,
                        }).ToList(),

                        Employee = _context.EmployeeRegister.Where(a => a.Emp_Id == c.Emp_Id && a.CompanyId == SchoolId).Select(a => new GetempDetails
                        {
                            Emp_Id = a.Emp_Id,
                            Emp_Name = a.Emp_Name,
                            EmpMobileno = a.Mobileno,
                        }).ToList(),


                    }).ToListAsync();
                return ApiResponse<List<GetMemberListModel>>.SuccessResponse(EmpEntity, "Member list fetched successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetMemberListModel>>.ErrorResponse("Error: " + ex.Message);
            }
        }


        public async Task<ApiResponse<GetTClassbySectionNdStudent>> GetLibraryClassBySectionNdStudent(int ClassId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var Sectiondatas = await _context.collegeinfo.Where(a => a.university_id == ClassId && a.CompanyId == SchoolId)
                       .Select(a => new TSectionDataList
                       {
                           SectionId = a.collegeid,
                           SectionName = a.collegename,
                       }).ToListAsync();


                var StudentDatas = await _context.LibraryCardTbl.Where(a => a.university_id == ClassId && a.CompanyId == SchoolId)
                    .Select(a => new TStudentDataList
                    {
                        StudentId = a.stu_id,
                        StudentName = _context.student_admission.Where(p => p.stu_id == a.stu_id && p.CompanyId == SchoolId).Select(p => p.stu_name).FirstOrDefault(),
                    }).ToListAsync();

                var res = new GetTClassbySectionNdStudent
                {
                    TSectionData = Sectiondatas,
                    TStudentData = StudentDatas,
                };

                return ApiResponse<GetTClassbySectionNdStudent>.SuccessResponse(res, "Fetch successfully class by section and student data ");
            }
            catch (Exception ex)
            {
                return ApiResponse<GetTClassbySectionNdStudent>.ErrorResponse("Error: " + ex.Message);

            }
        }

        public async Task<ApiResponse<List<TStudentDataList>>> GetLibrarySectionByStudent(int ClassId, int SectionId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.LibraryCardTbl.Where(a => a.university_id == ClassId && a.SectionId == SectionId && a.CompanyId == SchoolId)
                    .Select(a => new TStudentDataList
                    {
                        StudentId = a.stu_id,
                        StudentName = _context.student_admission.Where(p => p.stu_id == a.stu_id && p.CompanyId == SchoolId).Select(p => p.stu_name).FirstOrDefault(),
                    }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<TStudentDataList>>.ErrorResponse("No Fetch section by student ");
                }


                return ApiResponse<List<TStudentDataList>>.SuccessResponse(res, "Fetch successfully section by student");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TStudentDataList>>.ErrorResponse("Error: " + ex.Message);

            }
        }
        public async Task<ApiResponse<List<GetempDetails>>> GetEmployeeById(int EmpId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.EmployeeRegister.Where(a => a.Emp_Id == EmpId && a.CompanyId == SchoolId)
                    .Select(a => new GetempDetails
                    {
                        Emp_Id = a.Emp_Id,
                        Emp_Name = a.Emp_Name,
                        EmpMobileno = a.Mobileno,
                        Fatername = a.Father_husband_Name,

                    }).ToListAsync();

                return ApiResponse<List<GetempDetails>>.SuccessResponse(res, "Fetch successfully Employee details data ");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetempDetails>>.ErrorResponse("Error: " + ex.Message);

            }
        }

        public async Task<ApiResponse<List<getMenmberProfileModel>>> getMenmberProfileList(int LibraryId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.LibraryCardTbl.Where(a => a.LibraryId == LibraryId && a.CompanyId == SchoolId)
                    .Select(a => new getMenmberProfileModel
                    {
                        MemberId = a.MemberId,
                        CardNo = a.LibraryCardNo,
                        MemberType = a.MemberType,
                        ClassName = _context.University.Where(p => p.university_id == a.university_id && p.CompanyId == SchoolId).Select(p => p.university_name).FirstOrDefault(),
                        SectionName = _context.collegeinfo.Where(p => p.collegeid == a.SectionId && p.CompanyId == SchoolId).Select(p => p.collegename).FirstOrDefault(),


                        Student = _context.StudentRenewView.Where(p => p.StuId == a.stu_id && a.CompanyId == SchoolId)
                      .Select(p => new GetStuDetail
                      {
                          StudentId = p.StuId,
                          SRNo = p.registration_no,
                          stu_name = p.stu_name,
                          Fatername = p.father_name,
                          StuMobileno = p.father_mobile,
                          Gender = p.gender,

                      }).ToList(),

                        Employee = _context.EmployeeRegister.Where(p => p.Emp_Id == a.Emp_Id && a.CompanyId == SchoolId)
                      .Select(p => new GetempDetails
                      {
                          Emp_Id = p.Emp_Id,
                          Emp_Name = p.Emp_Name,
                          EmpMobileno = p.Mobileno,
                          Fatername = p.Father_husband_Name,
                          Gender = p.Gendar,
                          EmailId = p.EmailId,

                      }).ToList(),

                        Bookdata = _context.BookIssueTbl.Where(p => p.LibraryId == a.LibraryId && a.CompanyId == SchoolId)
                      .Select(p => new LibraryIssudatemodel
                      {
                          BookTitle = _context.BooksTbl.Where(c => c.BookId == p.BookId && c.CompanyId == SchoolId).Select(c => c.BookTitle).FirstOrDefault(),
                          BookNumber = _context.BooksTbl.Where(c => c.BookId == p.BookId && c.CompanyId == SchoolId).Select(c => c.BookNumber).FirstOrDefault(),
                          Quantity = p.Quantity,
                          IssueDate = p.IssueDate,
                          DueReturnDate = p.DueReturnDate,
                          ReturnDate = p.ReturnDate,

                      }).ToList(),

                    }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<getMenmberProfileModel>>.ErrorResponse("No Fetch Menmber Profile List ");
                }

                return ApiResponse<List<getMenmberProfileModel>>.SuccessResponse(res, "Fetch successfully Menmber Profile List data ");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<getMenmberProfileModel>>.ErrorResponse("Error: " + ex.Message);

            }
        }
        public async Task<ApiResponse<List<GetAuthormodel>>> GetAuthorByBookId(int BookId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.BooksTbl.Where(a => a.BookId == BookId && a.CompanyId == SchoolId)
                    .Select(a => new GetAuthormodel
                    {
                        BookId = a.BookId,
                        BookTitle = a.BookTitle,
                        Author = a.Author,

                    }).ToListAsync();

                return ApiResponse<List<GetAuthormodel>>.SuccessResponse(res, "Fetch successfully Author name data  ");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetAuthormodel>>.ErrorResponse("Error: " + ex.Message);

            }
        }

        public async Task<ApiResponse<bool>> AddIssueLibraryBook(IssueLibraryBookReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;
                int UserId = _loginUser.UserId;

                var existing = await _context.BookIssueTbl.FirstOrDefaultAsync(p => p.LibraryId == req.LibraryId && p.ReturnDate == null && p.Active == true && p.CompanyId == SchoolId);
                if (existing != null)
                {
                    return ApiResponse<bool>.ErrorResponse("This Book Already Issued");
                }

                int BIssueId = _context.BookIssueTbl.DefaultIfEmpty().Max(s => s == null ? 0 : s.IssueId) + 1;

                var cardentity = new BookIssueTbl
                {
                    IssueId = BIssueId,
                    LibraryId = req.LibraryId,
                    BookId = req.BookId,
                    stu_id = req.StudentId,
                    Emp_Id = req.EmpId,
                    IssueDate = DateTime.Now,
                    Quantity = req.IssueQuantity,
                    DueReturnDate = req.DueReturnDate,
                    //     CreateDate = DateTime.Now,
                    //     UpdateDate = DateTime.Now,
                    Active = true,
                    CompanyId = SchoolId,
                    SessionId = SessionId,
                    Userid = UserId,
                };

                _context.BookIssueTbl.Add(cardentity);
                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "Library Book Issued saved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> AddReturnDate(UpdateReturndateReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;

                var result = await _context.BookIssueTbl.Where(c => c.IssueId == req.BIssueId && c.CompanyId == SchoolId).FirstOrDefaultAsync();
                result.ReturnDate = req.ReturnDate;
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Return Date saved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<GetBookReportReq>>> GetBookReport()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var bookEntity = await _context.BooksTbl.Where(c => c.CompanyId == SchoolId)
                    .Select(c => new GetBookReportReq
                    {
                        BookId = c.BookId,
                        BookTitle = c.BookTitle,
                        BookNumber = c.BookNumber,
                        Publisher = c.Publisher,
                        Author = c.Author,
                        Subject = c.Subject,
                        TotalQuantity = c.TotalQuantity,
                        BookPrice = c.BookPrice,
                        Description = c.Description,
                        Date = c.Date,

                    }).ToListAsync();
                return ApiResponse<List<GetBookReportReq>>.SuccessResponse(bookEntity, "Book list fetched successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetBookReportReq>>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<GetLibraryReportModel>>> GetLibraryReport(LibrartReportReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.LibraryCardTbl.Where(a => (req.ClassId == -1 ? true : a.university_id == req.ClassId)
                && (req.StudentId == -1 ? true : a.stu_id == req.StudentId) && (req.Emp_Id == -1 ? true : a.Emp_Id == req.Emp_Id)
                && (req.MemberType == null ? true : a.MemberType == req.MemberType) && a.CompanyId == SchoolId
                     && a.SessionId == SessionId && a.Active == true)
                     .Select(a => new GetLibraryReportModel
                     {
                         CardNo = _context.LibraryCardTbl.Where(c => c.stu_id == a.stu_id && c.university_id == a.university_id).Select(c => c.LibraryCardNo).FirstOrDefault(),
                         ClassName = _context.University.Where(c => c.university_id == a.university_id && c.CompanyId == SchoolId).Select(a => a.university_name).FirstOrDefault(),
                         SectionName = _context.collegeinfo.Where(c => c.collegeid == a.SectionId && c.CompanyId == SchoolId).Select(c => c.collegename).FirstOrDefault(),

                         Student = _context.StudentRenewView.Where(c => c.StuId == a.stu_id && c.CompanyId == SchoolId).Select(c => new GetStuDetail
                         {
                             SRNo = c.registration_no,
                             StudentId = c.StuId,
                             stu_name = c.stu_name,
                             StuMobileno = c.father_mobile,
                         }).ToList(),

                         Employee = _context.EmployeeRegister.Where(p => p.Emp_Id == a.Emp_Id && p.CompanyId == SchoolId).Select(p => new GetempDetails
                         {
                             Emp_Id = p.Emp_Id,
                             Emp_Name = p.Emp_Name,
                             EmpMobileno = p.Mobileno,
                         }).ToList(),

                         Librarydate = _context.BookIssueTbl.Where(p => p.LibraryId == a.LibraryId && p.CompanyId == SchoolId).Select(p => new LibraryIssudatemodel
                         {
                             BookTitle = _context.BooksTbl.Where(c => c.BookId == p.BookId && c.CompanyId == SchoolId).Select(c => c.BookTitle).FirstOrDefault(),
                             BookNumber = _context.BooksTbl.Where(c => c.BookId == p.BookId && c.CompanyId == SchoolId).Select(c => c.BookNumber).FirstOrDefault(),
                             IssueDate = p.IssueDate,
                             DueReturnDate = p.DueReturnDate,
                             ReturnDate = p.ReturnDate,
                         }).ToList(),
                     }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<GetLibraryReportModel>>.ErrorResponse("Library record not found ");
                }
                return ApiResponse<List<GetLibraryReportModel>>.SuccessResponse(res, "Fetch Library report data");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetLibraryReportModel>>.ErrorResponse("Error: " + ex.Message);
            }
        }

    }
}
