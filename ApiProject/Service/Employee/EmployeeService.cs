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
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using ThisApiProject.Data;
using static ApiProject.Models.Response.ClassByFeeInResponse;

namespace ApiProject.Service.Employee
{
    public class EmployeeService : IEmployeeServide
    {

        private readonly ILoginUserService _loginUser;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EmployeeService(ILoginUserService loginUser, ApplicationDbContext context, IMapper mapper)
        {
            _loginUser = loginUser;
            _context = context;
            _mapper = mapper;
        }


        private async Task<string> SaveEmployeeFileAsync(IFormFile file, string folderPath, int Emp_Id, string schoolId, string subFolderName)
        {
            if (file == null || file.Length == 0)
                return null;

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            //var extension = Path.GetExtension(file.FileName);
            //var fileName = $"{studentId}{extension}";
            var extension = ".png"; // 👈 Force PNG
            var fileName = $"{Emp_Id}{extension}";
            var fileNameWithoutExt = Emp_Id.ToString();
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

        // Employee Details Start
        public async Task<ApiResponse<bool>> AddEmployeeDetail(AddEmployeeDetailReq request)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                EmployeeRegister Employee = new EmployeeRegister();
                Employee.Emp_Id = _context.EmployeeRegister.DefaultIfEmpty().Max(r => r == null ? 0 : r.Emp_Id) + 1;

                var allEmployeeDataRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", "ALLSchoolData");
                var EmployeeFolderPath = Path.Combine(allEmployeeDataRoot, SchoolId.ToString());
                // 👉 Check & create Employee folder
                if (!Directory.Exists(EmployeeFolderPath))
                {
                    Directory.CreateDirectory(EmployeeFolderPath);
                }

                if (request.Employeeimg != null)
                {
                    string photoFolder = Path.Combine(EmployeeFolderPath, "EmployeePhoto");
                    Employee.EmployrPhoto = await SaveEmployeeFileAsync(request.Employeeimg, photoFolder, Employee.Emp_Id, SchoolId.ToString(), "EmployeePhoto");
                }
                if (request.Aadharimg != null)
                {
                    string photoFolder = Path.Combine(EmployeeFolderPath, "EAadharPhoto");
                    Employee.AadharcardPhoto = await SaveEmployeeFileAsync(request.Aadharimg, photoFolder, Employee.Emp_Id, SchoolId.ToString(), "EAadharPhoto");
                }
                if (request.Panimg != null)
                {
                    string photoFolder = Path.Combine(EmployeeFolderPath, "EPanCardPhoto");
                    Employee.PancardPhoto = await SaveEmployeeFileAsync(request.Panimg, photoFolder, Employee.Emp_Id, SchoolId.ToString(), "EPanCardPhoto");
                }
                if (request.Educationimg != null)
                {
                    string photoFolder = Path.Combine(EmployeeFolderPath, "EEducationPhoto");
                    Employee.EducationPhoto = await SaveEmployeeFileAsync(request.Educationimg, photoFolder, Employee.Emp_Id, SchoolId.ToString(), "EEducationPhoto");
                }
                if (request.Graduationimg != null)
                {
                    string photoFolder = Path.Combine(EmployeeFolderPath, "EGraductionPhoto");
                    Employee.GraduationPhoto = await SaveEmployeeFileAsync(request.Graduationimg, photoFolder, Employee.Emp_Id, SchoolId.ToString(), "EGraductionPhoto");
                }
                if (request.PostGraduationimg != null)
                {
                    string photoFolder = Path.Combine(EmployeeFolderPath, "EPostEducationPhoto");
                    Employee.PostGraductionPhoto = await SaveEmployeeFileAsync(request.PostGraduationimg, photoFolder, Employee.Emp_Id, SchoolId.ToString(), "EPostEducationPhoto");
                }
                if (request.Resumeimg != null)
                {
                    string photoFolder = Path.Combine(EmployeeFolderPath, "EResumePhoto");
                    Employee.ResumePhoto = await SaveEmployeeFileAsync(request.Resumeimg, photoFolder, Employee.Emp_Id, SchoolId.ToString(), "EResumePhoto");
                }
                if (request.Experienceimg != null)
                {
                    string photoFolder = Path.Combine(EmployeeFolderPath, "EExperiencePhoto");
                    Employee.ExperiencePhoto = await SaveEmployeeFileAsync(request.Experienceimg, photoFolder, Employee.Emp_Id, SchoolId.ToString(), "EExperiencePhoto");
                }
                if (request.BPassbookimg != null)
                {
                    string tcFolder = Path.Combine(EmployeeFolderPath, "EBankpassbookPhoto");
                    Employee.BankPssbookPhoto = await SaveEmployeeFileAsync(request.BPassbookimg, tcFolder, Employee.Emp_Id, SchoolId.ToString(), "EBankpassbookPhoto");
                }

                Employee.JoiningDate = request.JoiningDate;
                Employee.Emp_Type = request.Emp_Type;
                Employee.Emp_Name = request.Emp_Name;
                Employee.Father_husband_Name = request.Father_husband_Name;
                Employee.DOB = request.DOB;
                Employee.Gendar = request.Gendar;
                Employee.Marital_Status = request.Marital_Status;
                Employee.Nationality = request.Nationality;
                Employee.Religion = request.Religion;
                Employee.Cast = request.Cast;
                Employee.Bloodgroup = request.Bloodgroup;
                Employee.Adharcard = request.Aadharcard;

                Employee.Address = request.Address;
                Employee.State = request.State;
                Employee.District = request.District;
                Employee.City = request.City;
                //  Employee.PinCode = request.PinCode;
                Employee.Phoneno = request.Phoneno;
                Employee.Mobileno = request.Mobileno;
                Employee.EmailId = request.EmailId;

                Employee.P_Address = request.P_Address;
                Employee.P_District = request.P_District;
                Employee.P_State = request.P_State;
                Employee.P_City = request.P_City;
                Employee.P_EmailId = request.P_EmailId;
                Employee.P_Phoneno = request.P_Phoneno;
                Employee.P_Mobileno = request.P_Mobileno;
                //   Employee.P_PinCode = request.P_PinCode;

                Employee.Qualification = request.Qualification;
                Employee.Experience = request.Experience;
                Employee.Specialization = request.Specialization;

                Employee.Applied_Post = request.Applied_Post;
                Employee.Appointed_Post = request.Appointed_Post;
                Employee.Basic_Salary = request.Basic_Salary;
                Employee.Allowances = request.Allowances;
                Employee.TotalSalary = request.TotalSalary;

                Employee.Specialnote = request.Specialnote;
                Employee.Adharcard = request.Aadharcard;
                Employee.CheckTerms = true;
                Employee.Active = true;
                Employee.Userid = UserId;
                Employee.CompanyId = SchoolId;
                Employee.SessionId = SessionId;
                Employee.Date = DateTime.Now;
                //   Employee.CreateDate = DateTime.Now;
                //    Employee.UpdateDate = DateTime.Now;

                _context.EmployeeRegister.Add(Employee);
                await _context.SaveChangesAsync();

                EmployeeBankDetailTbl BankDet = new EmployeeBankDetailTbl();

                BankDet.BankId = _context.EmployeeBankDetailTbl.DefaultIfEmpty().Max(r => r == null ? 0 : r.BankId) + 1;

                BankDet.EmployeeId = Employee.Emp_Id;
                BankDet.AHName = request.BankDetail.AHName;
                BankDet.BankName = request.BankDetail.BankName;
                BankDet.BranchName = request.BankDetail.BranchName;
                BankDet.AccountNumber = request.BankDetail.AccountNumber;
                BankDet.IFSCCode = request.BankDetail.IFSCCode;
                BankDet.SchoolId = SchoolId;
                BankDet.SessionId = SessionId;
                BankDet.UserId = UserId;
                BankDet.Active = true;
                BankDet.Date = DateTime.Now;
                BankDet.CreateDate = DateTime.Now;
                BankDet.UpdateDate = DateTime.Now;

                _context.EmployeeBankDetailTbl.Add(BankDet);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Employee saved successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateEmoloyeeDetail(UpdatreEmployeeDetailReq request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            {
                try
                {
                    int SchoolId = _loginUser.SchoolId;
                    int UserId = _loginUser.UserId;
                    int SessionId = _loginUser.SessionId;

                    EmployeeRegister result = await _context.EmployeeRegister.Where(c => c.Emp_Id == request.Emp_Id).FirstOrDefaultAsync();

                    var allEmployeeDataRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", "ALLSchoolData");
                    var EmployeeFolderPath = Path.Combine(allEmployeeDataRoot, SchoolId.ToString());
                    // 👉 Check & create Employee folder
                    if (!Directory.Exists(EmployeeFolderPath))
                    {
                        Directory.CreateDirectory(EmployeeFolderPath);
                    }

                    if (request.Employeeimg != null)
                    {
                        string photoFolder = Path.Combine(EmployeeFolderPath, "EmployeePhoto");
                        result.EmployrPhoto = await SaveEmployeeFileAsync(request.Employeeimg, photoFolder, result.Emp_Id, SchoolId.ToString(), "EmployeePhoto");
                    }
                    if (request.Aadharimg != null)
                    {
                        string photoFolder = Path.Combine(EmployeeFolderPath, "EAadharPhoto");
                        result.AadharcardPhoto = await SaveEmployeeFileAsync(request.Aadharimg, photoFolder, result.Emp_Id, SchoolId.ToString(), "EAadharPhoto");
                    }
                    if (request.Panimg != null)
                    {
                        string photoFolder = Path.Combine(EmployeeFolderPath, "EPanCardPhoto");
                        result.PancardPhoto = await SaveEmployeeFileAsync(request.Panimg, photoFolder, result.Emp_Id, SchoolId.ToString(), "EPanCardPhoto");
                    }
                    if (request.Educationimg != null)
                    {
                        string photoFolder = Path.Combine(EmployeeFolderPath, "EEducationPhoto");
                        result.EducationPhoto = await SaveEmployeeFileAsync(request.Educationimg, photoFolder, result.Emp_Id, SchoolId.ToString(), "EEducationPhoto");
                    }
                    if (request.Graduationimg != null)
                    {
                        string photoFolder = Path.Combine(EmployeeFolderPath, "EGraductionPhoto");
                        result.GraduationPhoto = await SaveEmployeeFileAsync(request.Graduationimg, photoFolder, result.Emp_Id, SchoolId.ToString(), "EGraductionPhoto");
                    }
                    if (request.PostGraduationimg != null)
                    {
                        string photoFolder = Path.Combine(EmployeeFolderPath, "EPostEducationPhoto");
                        result.PostGraductionPhoto = await SaveEmployeeFileAsync(request.PostGraduationimg, photoFolder, result.Emp_Id, SchoolId.ToString(), "EPostEducationPhoto");
                    }
                    if (request.Resumeimg != null)
                    {
                        string photoFolder = Path.Combine(EmployeeFolderPath, "EResumePhoto");
                        result.ResumePhoto = await SaveEmployeeFileAsync(request.Resumeimg, photoFolder, result.Emp_Id, SchoolId.ToString(), "EResumePhoto");
                    }
                    if (request.Experienceimg != null)
                    {
                        string photoFolder = Path.Combine(EmployeeFolderPath, "EExperiencePhoto");
                        result.ExperiencePhoto = await SaveEmployeeFileAsync(request.Experienceimg, photoFolder, result.Emp_Id, SchoolId.ToString(), "EExperiencePhoto");
                    }
                    if (request.BPassbookimg != null)
                    {
                        string tcFolder = Path.Combine(EmployeeFolderPath, "EBankpassbookPhoto");
                        result.BankPssbookPhoto = await SaveEmployeeFileAsync(request.BPassbookimg, tcFolder, result.Emp_Id, SchoolId.ToString(), "EBankpassbookPhoto");
                    }

                    result.Emp_Type = request.Emp_Type;
                    result.JoiningDate = request.JoiningDate;
                    result.Emp_Name = request.Emp_Name;
                    result.Father_husband_Name = request.Father_husband_Name;
                    result.DOB = request.DOB;
                    result.Gendar = request.Gendar;
                    result.Marital_Status = request.Marital_Status;
                    result.Cast = request.Cast;
                    result.Bloodgroup = request.Bloodgroup;
                    result.Religion = request.Religion;
                    result.Nationality = request.Nationality;
                    result.Address = request.Address;
                    result.EmailId = request.EmailId;
                    result.Phoneno = request.Phoneno;
                    result.Mobileno = request.Mobileno;
                    result.State = request.State;
                    result.District = request.District;
                    result.City = request.City;
                    result.P_Address = request.P_Address;
                    result.P_District = request.P_District;
                    result.P_State = request.P_State;
                    result.P_City = request.P_City;
                    result.P_EmailId = request.P_EmailId;
                    result.P_Phoneno = request.P_Phoneno;
                    result.P_Mobileno = request.P_Mobileno;
                    result.Qualification = request.Qualification;
                    result.Experience = request.Experience;
                    result.Specialization = request.Specialization;
                    result.Appointed_Post = request.Appointed_Post;
                    result.Basic_Salary = request.Basic_Salary;
                    result.Allowances = request.Allowances;
                    result.TotalSalary = request.TotalSalary;
                    result.JoiningDate = request.JoiningDate;
                    result.Specialnote = request.Specialnote;
                    result.Adharcard = request.Aadharcard;
                    result.CheckTerms = true;
                    result.Active = true;
                    result.Date = DateTime.Now;
                    // result.UpdateDate = DateTime.Now;

                    _context.SaveChanges();

                    EmployeeBankDetailTbl Bank = await _context.EmployeeBankDetailTbl.Where(p => p.BankId == request.BankDetail.BankId).FirstOrDefaultAsync();

                    Bank.EmployeeId = request.Emp_Id;
                    Bank.IFSCCode = request.BankDetail.IFSCCode;
                    Bank.BankName = request.BankDetail.BankName;
                    Bank.BranchName = request.BankDetail.BranchName;
                    Bank.AHName = request.BankDetail.AHName;
                    Bank.AccountNumber = request.BankDetail.AccountNumber;
                    Bank.UpdateDate = DateTime.Now;
                    _context.SaveChanges();

                    await transaction.CommitAsync();
                    return ApiResponse<bool>.SuccessResponse(true, "Update Employee details saved successful");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
                }
            }
        }

        public async Task<ApiResponse<List<GetEmployeeModel>>> GetEmployeeLit()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var EmployeeEntity = await _context.EmployeeRegister.Where(c => c.CompanyId == SchoolId)
                    .Select(c => new GetEmployeeModel
                    {
                        Emp_Id = c.Emp_Id,
                        Emp_Name = c.Emp_Name,
                        Active = c.Active,
                    }).ToListAsync();
                return ApiResponse<List<GetEmployeeModel>>.SuccessResponse(EmployeeEntity, "Employee list fetched successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetEmployeeModel>>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<List<GetEmployeeListModel>>> EmployeeReport(GetEmployeReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.EmployeeRegister.Where(c => (req.EmpId == -1 ? true : c.Emp_Id == req.EmpId)
                && c.Active == true && c.CompanyId == SchoolId).Select(c => new GetEmployeeListModel
                {
                    Emp_Id = c.Emp_Id,
                    Emp_Name = c.Emp_Name,
                    DOB = c.DOB,
                    Father_husband_Name = c.Father_husband_Name,
                    JoiningDate = c.JoiningDate,
                    Mobileno = c.Mobileno,
                    Address = c.Address,
                    City = c.City,
                    District = c.District,
                    //  PinCode = c.,
                    State = c.State,
                    Basic_Salary = c.Basic_Salary,
                    Allowances = c.Allowances,
                    TotalSalary = c.TotalSalary,
                    EmployeePhoto = c.EmployrPhoto,
                    Gendar = c.Gendar,
                    EmailId = c.EmailId,
                    Date = c.Date,
                    Active = c.Active,

                }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<GetEmployeeListModel>>.ErrorResponse("No Employee list found data");
                }

                return ApiResponse<List<GetEmployeeListModel>>.SuccessResponse(res, "Fetch Employee list data successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetEmployeeListModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> ChangeStatusEmployee(int EmpId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;

                var Employeeentity = await _context.EmployeeRegister.FirstOrDefaultAsync(p => p.Emp_Id == EmpId && p.CompanyId == SchoolId);
                if (Employeeentity == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Employee record not found ");
                }

                Employeeentity.Active = Employeeentity.Active == null ? true : !Employeeentity.Active;

                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "status update successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }

        // Employee Details End

        // Employee Work Allocation Start
        //public async Task<ApiResponse<bool>> GetEmployeeNdClassBySubjectData(int ClassId, int EmpId)
        //{
        //    try
        //    {
        //        int SchoolId = _loginUser.SchoolId;
        //        int UserId = _loginUser.UserId;
        //        int SessionId = _loginUser.SessionId;

        //        var EmpWorkAllo = _context.Emp_Workallocation.Where(e => e.Emp_Id == EmpId).FirstOrDefault();

        //        if (EmpWorkAllo != null)
        //        {

        //            var res = new GetEmpClassbySubjectModel
        //            {
        //                EmpWorkAllocation = _context.Emp_Workallocation.Where(a => a.Emp_Id == EmpId && a.ClassId == ClassId)
        //                  .Select(a => new getEmpWorkAllo
        //                  {
        //                      Emp_Id = a.Emp_Id,
        //                      ClassId = a.ClassId,
        //                      SectionId = a.SectionId,
        //                      SubjectId = a.SubjectId,
        //                  }).ToList(),

        //                Sectiondata = _context.ClassSectionTbl.Where(a => a.ClassId == ClassId && a.SchoolId == SchoolId).Select(a => new SectionData
        //                {
        //                    SectionId = a.SectionId,
        //                    SectionName = _context.sectionTbl.Where(p => p.SectionId == a.SectionId && p.SchoolId == SchoolId).Select(p => p.SectionName).FirstOrDefault(),
        //                    SectionPriority = _context.sectionTbl.Where(p => p.SectionId == a.SectionId && p.SchoolId == SchoolId).Select(p => p.SectionPriority).FirstOrDefault(),

        //                }).ToList(),

        //            };
        //        };
        //        if (res == null)
        //        {
        //            return ApiResponse<bool>.ErrorResponse("No Fetch section by student ");
        //        }

        //        return ApiResponse<bool>.SuccessResponse(res, "Fetch successfully section by student");
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);

        //    }
        //}

        //public async Task<ApiResponse<GetEmpClassbySubjectModel>> GetEmployeeNdClassBySubjectData(int ClassId, int EmpId)
        //{
        //    try
        //    {
        //        int SchoolId = _loginUser.SchoolId;
        //        int UserId = _loginUser.UserId;
        //        int SessionId = _loginUser.SessionId;

        //        var EmpWorkAllo = _context.Emp_Workallocation
        //            .FirstOrDefault(e => e.Emp_Id == EmpId);

        //        if (EmpWorkAllo == null)
        //        {
        //            return ApiResponse<GetEmpClassbySubjectModel>.ErrorResponse("No work allocation found for this employee.");
        //        }

        //        var res = new GetEmpClassbySubjectModel
        //        {
        //            EmpWorkAllocation = _context.Emp_Workallocation
        //                .Where(a => a.Emp_Id == EmpId && a.ClassId == ClassId)
        //                .Select(a => new getEmpWorkAllo
        //                {
        //                    Emp_Id = a.Emp_Id,
        //                    ClassId = a.ClassId,
        //                    SectionId = a.SectionId,
        //                    SubjectId = a.SubjectId,
        //                }).ToList(),

        //            Sectiondata = _context.ClassSectionTbl
        //                .Where(a => a.ClassId == ClassId && a.SchoolId == SchoolId)
        //                .Select(a => new SectionData
        //                {
        //                    SectionId = a.SectionId,
        //                    SectionName = _context.sectionTbl
        //                        .Where(p => p.SectionId == a.SectionId && p.SchoolId == SchoolId)
        //                        .Select(p => p.SectionName)
        //                        .FirstOrDefault(),
        //                    SectionPriority = _context.sectionTbl
        //                        .Where(p => p.SectionId == a.SectionId && p.SchoolId == SchoolId)
        //                        .Select(p => p.SectionPriority)
        //                        .FirstOrDefault(),
        //                }).ToList()
        //        };

        //        if (res == null || (!res.EmpWorkAllocation.Any() && !res.Sectiondata.Any()))
        //        {
        //            return ApiResponse<GetEmpClassbySubjectModel>.ErrorResponse("No data found for the given Class and Employee.");
        //        }

        //        return ApiResponse<GetEmpClassbySubjectModel>.SuccessResponse(res, "Fetch successfully section by student");
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiResponse<GetEmpClassbySubjectModel>.ErrorResponse("Error: " + ex.Message);
        //    }
        //}

        public async Task<ApiResponse<GetEmpClassbySubjectModel>> GetEmployeeNdClassBySubjectData(int EmpId, int ClassId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var EmpWorkAllo = _context.Emp_Workallocation
                    .FirstOrDefault(e => e.Emp_Id == EmpId);

                if (EmpWorkAllo == null)
                {
                    return ApiResponse<GetEmpClassbySubjectModel>.ErrorResponse("No work allocation found for this employee.");
                }

                var res = new GetEmpClassbySubjectModel
                {
                    EmpWorkAllocation = _context.Emp_Workallocation
                        .Where(a => a.Emp_Id == EmpId && a.UniversityId == ClassId)
                        .Select(a => new getEmpWorkAllo
                        {
                            Emp_Id = a.Emp_Id,
                            //ClassId = a.ClassId,
                            //SectionId = a.SectionId,
                            SubjectId = a.SubjectId,
                            Subject = _context.Subject.Where(c => c.subject_id == a.SubjectId && c.CompanyId == SchoolId).Select(c => c.subject_name).FirstOrDefault(),
                        }).ToList(),

                    Sectiondata = _context.ClassSectionTbl
                        .Where(a => a.ClassId == ClassId && a.SchoolId == SchoolId)
                        .Select(a => new SectionData
                        {
                            SectionId = a.SectionId,
                            SectionName = _context.collegeinfo.Where(p => p.collegeid == a.SectionId && p.CompanyId == SchoolId).Select(p => p.collegename).FirstOrDefault(),
                            //  SectionPriority = _context.sectionTbl.Where(p => p.SectionId == a.SectionId && p.SchoolId == SchoolId).Select(p => p.SectionPriority).FirstOrDefault(),

                        }).ToList(),

                    EmpsubjectData = (from cs in _context.ClassSubjectExamTbl
                                      join s in _context.Subject
                                      on cs.SubjectId equals s.subject_id
                                      where cs.ClassId == ClassId && cs.SchoolId == SchoolId
                                      select new GetEmpsubjectData
                                      {
                                          SubjectId = s.subject_id,
                                          SubjectName = s.subject_name,
                                          //    SubjectPriority = s.SubjectPriority
                                      }).Distinct().ToList(),
                };

                if (res == null || (!res.EmpWorkAllocation.Any() && !res.Sectiondata.Any()))
                {
                    return ApiResponse<GetEmpClassbySubjectModel>.ErrorResponse("No data found for the given Class and Employee.");
                }

                return ApiResponse<GetEmpClassbySubjectModel>.SuccessResponse(res, "Fetch successfully section by student");
            }
            catch (Exception ex)
            {
                return ApiResponse<GetEmpClassbySubjectModel>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> AddEmpWorkAllocation(AddWorkallcation req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var existingAllocations = _context.Emp_Workallocation.Where(e => e.Emp_Id == req.WorkAllo.Emp_Id && e.UniversityId == req.WorkAllo.ClassId).ToList();

                if (existingAllocations.Any())
                {
                    _context.Emp_Workallocation.RemoveRange(existingAllocations);
                    _context.SaveChanges();
                }

                int EWorkId = _context.Emp_Workallocation.Select(r => (int?)r.Id).Max() ?? 0;

                for (int i = 0; i < req.subjectWorks.Count; i++)
                {
                    EWorkId++;

                    var work = new Emp_Workallocation
                    {
                        Id = EWorkId,
                        Emp_Id = req.WorkAllo.Emp_Id,
                        UniversityId = req.WorkAllo.ClassId,
                        college_id = req.WorkAllo.SectionId,
                        SubjectId = req.subjectWorks[i].SubjectId,
                        CompanyId = SchoolId,
                        SessionId = SessionId,
                        Active = true,
                        Userid = UserId,
                        JoiningDate = DateTime.Now
                    };

                    _context.Emp_Workallocation.Add(work);
                }
                _context.SaveChanges();


                return ApiResponse<bool>.SuccessResponse(true, "Add Employee WorkAllocation  successfully saved");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<GetEmpWorkAllocationModel>>> EmpWorkallocationReport(GetEmployeReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.EmployeeRegister.Where(c => (req.EmpId == -1 ? true : c.Emp_Id == req.EmpId)
                && c.Active == true && c.CompanyId == SchoolId).Select(c => new GetEmpWorkAllocationModel
                {
                    Emp_Id = c.Emp_Id,
                    Emp_Name = c.Emp_Name,

                    SubjectData = _context.Emp_Workallocation.Where(p => p.Emp_Id == c.Emp_Id && p.CompanyId == SchoolId)
                           .GroupBy(p => new { p.UniversityId })
                            .Select(g => new getSubjectlist
                            {
                                ClassId = g.Key.UniversityId,

                                ClassName = _context.University.Where(m => m.university_id == g.Key.UniversityId).Select(m => m.university_name).FirstOrDefault(),

                                Sujbect = g.Select(r => new SubjectDto
                                {
                                    SubjectId = r.SubjectId,
                                    SubjectName = _context.Subject.Where(s => s.subject_id == r.SubjectId && s.CompanyId == SchoolId).Select(s => s.subject_name).FirstOrDefault(),

                                }).ToList(),

                            }).ToList(),

                }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<GetEmpWorkAllocationModel>>.ErrorResponse("No Employee Work allocation found data");
                }

                return ApiResponse<List<GetEmpWorkAllocationModel>>.SuccessResponse(res, "Fetch Employee Work allocation data successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetEmpWorkAllocationModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }


        // Employee Employee Attendance start
        public async Task<ApiResponse<List<EmpAttendanceDetail>>> GetEmployeeAttendance(getEmployeeAttendance req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.EmployeeRegister.Where(a => a.Active == true).Select(a => new EmpAttendanceDetail
                {
                    Emp_Id = a.Emp_Id,
                    Employeename = a.Emp_Name,
                    Status = _context.Emp_Attendance.Where(p => p.Emp_Id == a.Emp_Id && p.Date == req.Date).Select(p => p.Status).FirstOrDefault(),
                    Note = _context.Emp_Attendance.Where(p => p.Emp_Id == a.Emp_Id && p.Date == req.Date).Select(p => p.Note).FirstOrDefault(),
                    Date = req.Date,
                }).ToListAsync();
                return ApiResponse<List<EmpAttendanceDetail>>.SuccessResponse(res, "Fetch successfully Employee Attendance");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<EmpAttendanceDetail>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> InsertEmployeeAttendance(List<addEmployeeAttendanceReq> EAttendance)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                if (EAttendance != null && EAttendance.Count > 0)
                {
                    int? EmpId = EAttendance[0].Emp_Id;
                    DateTime? date = EAttendance[0].Date;

                    // Delete existing attendance
                    var attendancedelete = await _context.Emp_Attendance
                        .Where(k => k.SessionId == SessionId && k.Emp_Id == EmpId && k.Date == date && k.CompanyId == SchoolId)
                        .ToListAsync();


                    if (attendancedelete.Any())
                    {
                        _context.Emp_Attendance.RemoveRange(attendancedelete);
                    }

                    for (int i = 0; i < EAttendance.Count; i++)
                    {
                        Emp_Attendance EmpAtt = new Emp_Attendance();
                        EmpAtt.EAId = _context.Emp_Attendance.DefaultIfEmpty().Max(r => r == null ? 0 : r.EAId) + 1;
                        EmpAtt.Emp_Id = EAttendance[i].Emp_Id;
                        EmpAtt.Status = EAttendance[i].Status;
                        EmpAtt.Note = EAttendance[i].Note;
                        EmpAtt.Date = EAttendance[i].Date;
                        EmpAtt.Time = DateTime.Now.TimeOfDay;
                        EmpAtt.CompanyId = SchoolId;
                        EmpAtt.Userid = UserId;
                        EmpAtt.SessionId = SessionId;
                        EmpAtt.Active = true;
                        _context.Emp_Attendance.AddRange(EmpAtt);
                        await _context.SaveChangesAsync();
                    }

                    return ApiResponse<bool>.SuccessResponse(true, "Employee Attendance data saved successfully");
                }

                return ApiResponse<bool>.ErrorResponse("Invalid request: Empty data.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<EmpAttendanceReportModel>>> EmployeeAttendanceReport(EmpAttendanceReportReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.EmployeeRegister.Where(a => a.Active == true && a.CompanyId == SchoolId).Select(a => new EmpAttendanceReportModel
                {
                    Emp_Id = a.Emp_Id,
                    Employeename = a.Emp_Name,
                    // Monthname = req.Month,
                    // TotalP = _context.Emp_Attendance.Where(p => p.Emp_Id == c.Emp_Id && p.Date.Value.Month == month && p.SessionId == SessionId && p.SchoolId == SchoolId),
                    TotalP = _context.Emp_Attendance.Where(p => p.Emp_Id == a.Emp_Id && p.Date.Value.Month == req.Months && p.SessionId == SessionId && p.CompanyId == SchoolId && p.Status == "Present").Count(),
                    TotalA = _context.Emp_Attendance.Where(p => p.Emp_Id == a.Emp_Id && p.Date.Value.Month == req.Months && p.SessionId == SessionId && p.CompanyId == SchoolId && p.Status == "Absent").Count(),
                    TotalH = _context.Emp_Attendance.Where(p => p.Emp_Id == a.Emp_Id && p.Date.Value.Month == req.Months && p.SessionId == SessionId && p.CompanyId == SchoolId && p.Status == "Holiday").Count(),
                    TotalL = _context.Emp_Attendance.Where(p => p.Emp_Id == a.Emp_Id && p.Date.Value.Month == req.Months && p.SessionId == SessionId && p.CompanyId == SchoolId && p.Status == "Leave").Count(),
                    TotalHF = _context.Emp_Attendance.Where(p => p.Emp_Id == a.Emp_Id && p.Date.Value.Month == req.Months && p.SessionId == SessionId && p.CompanyId == SchoolId && p.Status == "HalfDay").Count(),

                }).ToListAsync();
                return ApiResponse<List<EmpAttendanceReportModel>>.SuccessResponse(res, "Fetch successfully Employee Attendance");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<EmpAttendanceReportModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        // Employee Saalary 
        public async Task<ApiResponse<List<GetAdvsalaryModel>>> GetAdvanceSalaryList()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var EmployeeEntity = await _context.EmpAdvanceSalaryTbl.Where(c => c.CompanyId == SchoolId)
                    .Select(c => new GetAdvsalaryModel
                    {
                        Emp_Id = c.EmpId,
                        Employeename = _context.EmployeeRegister.Where(a => a.Emp_Id == c.EmpId && a.CompanyId == SchoolId).Select(a => a.Emp_Name).FirstOrDefault(),
                        Date = c.Date,
                        AdvanceAmount = c.AdvanceAmt,
                    }).ToListAsync();
                return ApiResponse<List<GetAdvsalaryModel>>.SuccessResponse(EmployeeEntity, "Employee list fetched successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetAdvsalaryModel>>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> AddAdvanceSalary(AddAdavanceSalaryReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                EmpAdvanceSalaryTbl Employee = new EmpAdvanceSalaryTbl();
                Employee.AdvId = _context.EmpAdvanceSalaryTbl.DefaultIfEmpty().Max(r => r == null ? 0 : r.AdvId) + 1;


                Employee.EmpId = req.Emp_Id;
                Employee.AdvanceAmt = req.AdvanceAmount;
                Employee.PayAdvanceAmt = 0;
                Employee.DueAdvanceAmt = req.AdvanceAmount;
                Employee.Date = req.Date;
                Employee.Createdate = DateTime.Now;
                Employee.Updatedate = DateTime.Now;
                Employee.CompanyId = SchoolId;
                Employee.Userid = UserId;
                Employee.SessionId = SessionId;

                _context.EmpAdvanceSalaryTbl.Add(Employee);
                await _context.SaveChangesAsync();


                return ApiResponse<bool>.SuccessResponse(true, "Employee advance salary saved successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<EmployeeSalaryModel>>> GetEmployeeSalary(int month)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;
                int currentYear = DateTime.UtcNow.Year;

                // 1. GET ALL DATA IN ONE TIME (FAST)
                var Employees = await _context.EmployeeRegister.Where(a => a.CompanyId == SchoolId && a.Active == true).ToListAsync();

                var AttendanceList = await _context.Emp_Attendance.Where(p => p.Date.Value.Month == month && p.SessionId == SessionId && p.Active == true).ToListAsync();

                var AdvanceList = await _context.EmpAdvanceSalaryTbl.Where(a => a.Date.Value.Year == currentYear && a.SessionId == SessionId).ToListAsync();

                var SalaryList = await _context.EmployeeSalary.Where(a => a.Month <= month && a.CompanyId == SchoolId).ToListAsync();

                List<EmployeeSalaryModel> result = new List<EmployeeSalaryModel>();

                int daysInCurrentMonth = DateTime.DaysInMonth(currentYear, month);

                // 2. FAST LOOP – NO DB CALL INSIDE
                foreach (var emp in Employees)
                {
                    // Joining Date Check
                    if (emp.JoiningDate != null && emp.JoiningDate.Value > new DateTime(currentYear, month, daysInCurrentMonth))
                        continue;

                    // ⬇ FILTER FROM IN-MEMORY LIST (NO DB HIT) ⬇
                    var empAttendances = AttendanceList.Where(a => a.Emp_Id == emp.Emp_Id);
                    int Ptotal = empAttendances.Count(a => a.Status == "Present");
                    int Htotal = empAttendances.Count(a => a.Status == "Holiday");
                    int HFtotal = empAttendances.Count(a => a.Status == "HalfDay");
                    int OneDtotal = empAttendances.Count(a => a.Status == "Leave");

                    decimal hfdays = HFtotal / 2M;      // Half Day = 0.5
                    decimal ohfdays = OneDtotal / 4M;   // Leave = 0.25
                    decimal totaldays = Ptotal + Htotal + hfdays + ohfdays;

                    decimal perDaySalary = (decimal)emp.TotalSalary / daysInCurrentMonth;
                    decimal salaryamount = Math.Round(totaldays * perDaySalary);

                    var empAdv = AdvanceList.Where(a => a.EmpId == emp.Emp_Id && a.Date >= emp.JoiningDate && month >= a.Date.Value.Month).Sum(a => a.AdvanceAmt) ?? 0;

                    var empSalary = SalaryList.FirstOrDefault(a => a.Emp_Id == emp.Emp_Id && a.Month == month);

                    var PASReceipt = SalaryList.Where(p => p.Emp_Id == emp.Emp_Id).Sum(p => p.PayAdvAmount) ?? 0;

                    result.Add(new EmployeeSalaryModel
                    {
                        Emp_Id = emp.Emp_Id,
                        Emp_Name = emp.Emp_Name,
                        Basic_Salary = emp.Basic_Salary,
                        Allowance = emp.Allowances,
                        TotalSalary = emp.TotalSalary,
                        totalday = totaldays,
                        Salary = salaryamount,
                        AdvanceSalary = empAdv,
                        PayAdvReAmt = PASReceipt,
                        PayAdvAmount = empSalary?.PayAdvAmount ?? 0,
                        PayAmount = empSalary?.PayAmount ?? 0,
                        Month = month
                    });
                }

                return ApiResponse<List<EmployeeSalaryModel>>.SuccessResponse(result, "Salary generated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<EmployeeSalaryModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }


        public async Task<ApiResponse<bool>> AddEmployeeSalary(List<AddEmployeeSalaryModel> model)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;
                int UserId = _loginUser.UserId;

                if (model != null && model.Count > 0)
                {
                    for (int i = 0; i < model.Count; i++)
                    {
                        var emp = model[i];

                        var salarydata = _context.EmployeeSalary.Where(q => q.CompanyId == SchoolId && q.Emp_Id == emp.Emp_Id && q.Month == emp.Month).FirstOrDefault();

                        if (salarydata == null)
                        {
                            EmployeeSalary Empsalary = new EmployeeSalary();
                            Empsalary.id = _context.EmployeeSalary.DefaultIfEmpty().Max(r => r == null ? 0 : r.id) + 1;
                            var existingReceiptNos = _context.EmployeeSalary.Where(s => s.CompanyId == SchoolId).OrderByDescending(s => s.id).Take(1).FirstOrDefault();

                            if (existingReceiptNos == null)
                            {
                                Empsalary.ReceiptNo = "1";
                            }
                            else
                            {
                                int rno = Convert.ToInt32(existingReceiptNos.ReceiptNo) + 1;
                                Empsalary.ReceiptNo = rno.ToString();
                            }

                            Empsalary.Emp_Id = emp.Emp_Id;
                            Empsalary.Month = emp.Month;
                            Empsalary.TotalDays = emp.totalday.ToString();
                            Empsalary.PayAdvAmount = emp.PayAdvanceAmount == null ? 0 : emp.PayAdvanceAmount;
                            Empsalary.PayAmount = emp.PayAmount == null ? 0 : emp.PayAmount;
                            Empsalary.BasicSalary = emp.Basic_Salary.ToString();
                            Empsalary.Allowance = emp.Allowance.ToString();
                            Empsalary.TotalSalary = emp.TotalSalary.ToString();
                            Empsalary.ESI = "";
                            Empsalary.PF = "";
                            Empsalary.TDS = "";
                            Empsalary.TotalPresent = "0";
                            Empsalary.TotalAbsent = "0";
                            Empsalary.TotalLeave = "0";
                            Empsalary.TotalHoliday = "0";
                            Empsalary.TotalHalfDay = "0";
                            Empsalary.Active = true;
                            Empsalary.PaymentDate = DateTime.Now;
                            Empsalary.CompanyId = SchoolId;
                            Empsalary.SessionId = SessionId;
                            Empsalary.Userid = UserId;

                            _context.EmployeeSalary.Add(Empsalary);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            salarydata.TotalDays = emp.totalday.ToString();
                            salarydata.PayAdvAmount = emp.PayAdvanceAmount == null ? 0 : emp.PayAdvanceAmount;
                            salarydata.PayAmount = emp.PayAmount == null ? 0 : emp.PayAmount;
                            await _context.SaveChangesAsync();
                        }
                    }

                }

                return ApiResponse<bool>.SuccessResponse(true, "Employee salary data saved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }




    }
}

//public async Task<ApiResponse<List<EmployeeSalaryModel>>> GetEmployeeSalary(int month)
//{
//    try
//    {
//        int SchoolId = _loginUser.SchoolId;
//        int UserId = _loginUser.UserId;
//        int SessionId = _loginUser.SessionId;

//        var Employee = _context.EmployeeRegister.Where(a => a.CompanyId == SchoolId && a.Active == true).ToList();

//        List<EmployeeSalaryModel> result = new List<EmployeeSalaryModel>();

//        int currentYear = DateTime.UtcNow.Year;

//        for (int i = 0; i < Employee.Count; i++)
//        {
//            var Empsal = Employee[i];

//            if (Empsal.JoiningDate != null && Empsal.JoiningDate.Value > new DateTime(currentYear, month, DateTime.DaysInMonth(currentYear, month)))
//            {
//                continue;
//            }

//            var Ptotal = _context.Emp_Attendance.Where(p => p.Emp_Id == Empsal.Emp_Id && p.Date.Value.Month == month && p.SessionId == SessionId && p.Status == "Present" && p.Active == true).Count();
//            var Htotal = _context.Emp_Attendance.Where(p => p.Emp_Id == Empsal.Emp_Id && p.Date.Value.Month == month && p.SessionId == SessionId && p.Status == "Holiday" && p.Active == true).Count();
//            var OneDtotal = _context.Emp_Attendance.Where(p => p.Emp_Id == Empsal.Emp_Id && p.Date.Value.Month == month && p.SessionId == SessionId && p.Status == "Leave" && p.Active == true).Count();
//            var HFtotal = _context.Emp_Attendance.Where(p => p.Emp_Id == Empsal.Emp_Id && p.Date.Value.Month == month && p.SessionId == SessionId && p.Status == "HalfDay" && p.Active == true).Count();

//            var Advances = _context.EmpAdvanceSalaryTbl.Where(a => a.EmpId == Empsal.Emp_Id && a.SessionId == SessionId && a.Date >= Empsal.JoiningDate
//              && month >= a.Date.Value.Month && a.Date.Value.Year == currentYear).Sum(a => a.AdvanceAmt) ?? 0;

//            int daysInCurrentMonth = DateTime.DaysInMonth(currentYear, month);

//            var hfdays = (decimal)HFtotal / 2;
//            var ohfdays = (decimal)OneDtotal / 4;

//            var totaldays = Ptotal + Htotal + hfdays + ohfdays;

//            var perdaysalary = (decimal)Empsal.TotalSalary / daysInCurrentMonth;
//            var salaryamount = Math.Round(totaldays * perdaysalary);
//            string monthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

//            var Esalary = _context.EmployeeSalary.Where(a => a.Emp_Id == Empsal.Emp_Id && a.Month == month && a.CompanyId == SchoolId).FirstOrDefault();

//            var PASReceipt = _context.EmployeeSalary.Where(p => p.Emp_Id == Empsal.Emp_Id && p.Month <= month && p.CompanyId == SchoolId).Sum(p => p.PayAdvAmount) ?? 0;

//            result.Add(new EmployeeSalaryModel
//            {
//                Emp_Id = Empsal.Emp_Id,
//                Emp_Name = Empsal.Emp_Name,
//                Basic_Salary = Empsal.Basic_Salary,
//                Allowance = Empsal.Allowances,
//                TotalSalary = Empsal.TotalSalary,
//                AdvanceSalary = Advances,
//                PayAdvReAmt = PASReceipt,
//                totalday = totaldays,
//                Salary = salaryamount,
//                PayAdvAmount = Esalary?.PayAdvAmount ?? 0,
//                PayAmount = Esalary?.PayAmount ?? 0,
//                Month = month

//            });
//        }
//        return ApiResponse<List<EmployeeSalaryModel>>.SuccessResponse(result, "Employee salary saved successfully");

//    }
//    catch (Exception ex)
//    {
//        return ApiResponse<List<EmployeeSalaryModel>>.ErrorResponse("Something went wrong: " + ex.Message);
//    }
//}