using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using ApiProject.Service.Current;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using System.Diagnostics;

namespace ApiProject.Service.School
{
    public class SchoolService : ISchoolService
    {
        private readonly ILoginUserService _loginUser;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SchoolService(
            ILoginUserService loginUser,
            ApplicationDbContext context,
                        IMapper mapper

            )
        {
            _context = context;
            _loginUser = loginUser;
            _mapper = mapper;

        }



        // *********** School Informaction 
        #region School Informaction
        public async Task<ApiResponse<SchoolDetail>> SchoolDetail()
        {
            try
            {

                int SchoolId = _loginUser.SchoolId;

                var SchoolDetail = await _context.institute.Where(p => p.institute_id == SchoolId).Select(p => new SchoolDetail
                {
                    schoolname = p.institute_name,
                    //    ownername = p.OwnerName,
                    rgtno = p.regist_num,
                    rgstdate = p.regist_date,
                    address = p.address,
                    cityname = p.city_name,
                    landlinenum = p.landline_num,
                    pincode = p.pincode,
                    schoolcode = p.instituteCode,
                    districtname = p.district_name,
                    email = p.email,
                    logoimg = p.logo_img,
                    rlogo = p.institute_img,
                    weburl = p.weburl,
                    statename = p.state_name,
                    mobileno1 = p.mob_num,
                    mobileno2 = p.alternatemob_num,
                    //companyactive = p.activ,
                    //  joiningdate = p.JoiningDate,
                    //  expiredate = p.ExpireDate,
                    //  active = p.Active,

                }).FirstOrDefaultAsync();

                return ApiResponse<SchoolDetail>.SuccessResponse(SchoolDetail, "Fetch School list successfully");
            }
            catch (Exception ex)
            {

                return ApiResponse<SchoolDetail>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<SchoolDetail> schooldetailupdate(SchoolUpdate request)
        {
            int SchoolId = _loginUser.SchoolId;

            var school = await _context.institute.FirstOrDefaultAsync(p => p.institute_id == SchoolId);


            if (request.logo != null)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "SchoolLogo", "Logo");
                var extension = Path.GetExtension(request.logo.FileName);
                var fileNameWithoutExtension = school.institute_id.ToString();


                var existingFiles = Directory.GetFiles(folderPath, fileNameWithoutExtension + ".*");
                foreach (var file in existingFiles)
                {
                    if (System.IO.File.Exists(file))
                    {
                        System.IO.File.Delete(file);
                    }
                }

                // Naya file save karo
                var filePath = Path.Combine(folderPath, fileNameWithoutExtension + extension);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.logo.CopyToAsync(stream);
                }

                school.logo_img = "/image/SchoolLogo/Logo/" + fileNameWithoutExtension + extension;
            }


            // school.SchoolName = request.schoolname;
            // school.OwnerName = request.ownername == null ? " " : request.ownername;
            school.regist_num = request.rgtno == null ? " " : request.rgtno;
            school.regist_date = request.rgstdate;
            school.address = request.address == null ? " " : request.address;
            school.city_name = request.cityname == null ? " " : request.cityname;
            school.landline_num = request.landlinenum == null ? " " : request.landlinenum;
            school.pincode = request.pincode == null ? " " : request.pincode;
            school.district_name = request.districtname == null ? " " : request.districtname;
            school.email = request.email == null ? " " : request.email;
            school.weburl = request.weburl == null ? " " : request.weburl;
            // school.StateName = request.statename == null ? " " : request.statename;
            school.mob_num = request.mobileno1 == null ? " " : request.mobileno1;
            school.alternatemob_num = request.mobileno2 == null ? " " : request.mobileno2;
            //  school.UpdateDate = DateTime.Now.Date;

            await _context.SaveChangesAsync();
            //    return Task<SchoolDetail>.SuccessResponse(school, "Section update successfully.");

            return new SchoolDetail
            {
                //   schoolname = school.SchoolName,
                //   ownername = school.OwnerName,
                rgtno = school.regist_num,
                rgstdate = school.regist_date,
                address = school.address,
                cityname = school.city_name,
                landlinenum = school.landline_num,
                pincode = school.pincode,
                schoolcode = school.district_name,
                email = school.email,
                logoimg = school.institute_img,
                rlogo = school.logo_img,
                weburl = school.weburl,
                //statename = school.StateName,
                mobileno1 = school.mob_num,
                mobileno2 = school.alternatemob_num
            };
        }
        public async Task<ApiResponse<List<State>>> GetState()
        {
            try
            {
                var Entities = await _context.State.OrderBy(c => c.State_Priority).ToListAsync();
                var data = _mapper.Map<List<State>>(Entities);
                return ApiResponse<List<State>>.SuccessResponse(data, "State list fetched");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<State>>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<List<DistrictResModel>>> GetDistrict()
        {
            try
            {
                var Entities = await _context.District.ToListAsync();
                var data = _mapper.Map<List<DistrictResModel>>(Entities);
                return ApiResponse<List<DistrictResModel>>.SuccessResponse(data, "district list fetched");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<DistrictResModel>>.ErrorResponse("Error: " + ex.Message);
            }

        }

        #endregion


        // *********** User Details  
        #region User Details
        public async Task<ApiResponse<List<GetUserReqmodel>>> GetUserDetail()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var UserList = await _context.UserInformation.Where(p => p.CompanyId == SchoolId)
                    .Select(p => new GetUserReqmodel
                    {
                        UserId = p.id,
                        Name = p.Name,
                        Address = p.Address,
                        MobileNo = p.Mobileno,
                        UserName = p.Username,
                        Password = p.Password,
                        JoiningDate = p.JoiningDate,
                        Status = p.Status,
                        Active = p.Active,

                    }).ToListAsync();
                return ApiResponse<List<GetUserReqmodel>>.SuccessResponse(UserList, "Fetch user list successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetUserReqmodel>>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateUser(UpdateUserReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;

                var res = await _context.UserInformation.FirstOrDefaultAsync(a => a.id == req.UserId && a.CompanyId == SchoolId);

                if (res == null)
                {
                    return ApiResponse<bool>.ErrorResponse("User not found.");
                }

                res.Name = req.Name;
                res.Mobileno = req.MobileNo;
                res.Address = req.Address;
                res.Password = req.Password;
                // res.upda = DateTime.Now;

                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "User updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> changestatusUser(int UserId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;

                var UserEntity = await _context.UserInformation.FirstOrDefaultAsync(c => c.id == UserId && c.CompanyId == SchoolId);

                if (UserEntity == null)
                {
                    return ApiResponse<bool>.ErrorResponse("User not found");
                }

                // Status toggle karo
                UserEntity.Active = UserEntity.Active == null ? true : !UserEntity.Active;

                // Changes save karo
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Status updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        #endregion


        // ************ Class Details 
        #region Class Details
        public async Task<ApiResponse<List<ClassALLReqModel>>> getclassList()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var classEntities = await _context.University.Where(cs => cs.CompanyId == SchoolId).ToListAsync();
                //  .OrderBy(s => s.ClassPriority)

                var classList = new List<ClassALLReqModel>();

                foreach (var cls in classEntities)
                {
                    // Class info map
                    var mappedClass = _mapper.Map<ClassALLReqModel>(cls);

                    classList.Add(mappedClass);
                }

                return ApiResponse<List<ClassALLReqModel>>.SuccessResponse(classList, "Class list fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ClassALLReqModel>>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> insertclass(AddClassReq request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            {
                try
                {
                    int SchoolId = _loginUser.SchoolId;
                    int SessionId = _loginUser.SessionId;
                    int UserId = _loginUser.UserId;

                    var existingSection = await _context.University.FirstOrDefaultAsync(s => s.CompanyId == SchoolId && s.university_name == request.university_name);

                    if (existingSection != null)
                    {
                        return ApiResponse<bool>.ErrorResponse("Class name already exists.");
                    }

                    int classid = _context.University.DefaultIfEmpty().Max(s => s == null ? 0 : s.university_id) + 1;

                    var classEntity = new University
                    {
                        university_id = classid,
                        university_name = request.university_name,
                        CompanyId = SchoolId,
                        SessionId = SessionId,
                        Userid = UserId,
                        Active = true,
                    };

                    _context.University.Add(classEntity);
                    await _context.SaveChangesAsync();



                    await transaction.CommitAsync();


                    return ApiResponse<bool>.SuccessResponse(true, "Class saved successfully.");
                }

                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);

                }
            }
        }
        public async Task<ApiResponse<bool>> updateclass(UpdateClassReq request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            {

                try
                {
                    int SchoolId = _loginUser.SchoolId;
                    int UserId = _loginUser.UserId;

                    University University = await _context.University.Where(p => p.university_name == request.university_name && p.university_id != request.university_id && p.CompanyId == SchoolId).FirstOrDefaultAsync();
                    if (University != null)
                    {
                        return ApiResponse<bool>.ErrorResponse("Class Name Already available");

                    }

                    var result = await _context.University.Where(r => r.university_id == request.university_id && r.CompanyId == SchoolId).FirstOrDefaultAsync();

                    result.university_name = request.university_name;
                    result.Userid = UserId;


                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return ApiResponse<bool>.SuccessResponse(true, "Class update successfully.");

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
                }
            }
        }
        public async Task<ApiResponse<bool>> changestatusclass(int classid)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;

                var classEntity = await _context.University.FirstOrDefaultAsync(c => c.university_id == classid && c.CompanyId == SchoolId);

                if (classEntity == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Class not found");
                }

                // Status toggle karo
                classEntity.Active = classEntity.Active == null ? true : !classEntity.Active;

                // Changes save karo
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Status updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        #endregion


        // ************ Section Details
        #region Section Details
        public async Task<ApiResponse<List<ClassSectionResModel>>> getsection()
        {
            try
            {

                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var sectionEntities = await _context.collegeinfo.Where(cs => cs.CompanyId == SchoolId).ToListAsync();

                var sectionlist = _mapper.Map<List<ClassSectionResModel>>(sectionEntities);


                return ApiResponse<List<ClassSectionResModel>>.SuccessResponse(sectionlist, "section list fetched successfully");

            }
            catch (Exception ex)
            {

                return ApiResponse<List<ClassSectionResModel>>.ErrorResponse("Error: " + ex.Message);

            }
        }

        public async Task<ApiResponse<bool>> insertsection(AddSectionReq request)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;
                int UserId = _loginUser.UserId;

                // Check if section with same name already exists in the school
                var existingSection = await _context.collegeinfo
                    .FirstOrDefaultAsync(s => s.CompanyId == SchoolId && s.collegename == request.SectionName && s.university_id == request.ClassId);

                if (existingSection != null)
                {
                    return ApiResponse<bool>.ErrorResponse("Section name already exists.");
                }

                // Generate new SectionId
                int newSectionId = _context.collegeinfo.DefaultIfEmpty().Max(s => s == null ? 0 : s.collegeid) + 1;

                var sectionEntity = new collegeinfo
                {
                    collegeid = newSectionId,
                    university_id = request.ClassId,
                    collegename = request.SectionName,
                    CompanyId = SchoolId,
                    SessionId = SessionId,
                    Userid = UserId,
                    active = true,
                };

                _context.collegeinfo.Add(sectionEntity);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Section inserted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> updatesection(UpdateSectionReq request)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;



                collegeinfo section = await _context.collegeinfo.Where(p => p.collegename == request.SectionName && p.collegeid != request.SectionId && p.university_id == request.ClassId && p.CompanyId == SchoolId).FirstOrDefaultAsync();
                if (section != null)
                {
                    return ApiResponse<bool>.ErrorResponse("Section Name Already available");

                }

                var result = await _context.collegeinfo.Where(r => r.collegeid == request.SectionId && r.CompanyId == SchoolId).FirstOrDefaultAsync();
                result.university_id = request.ClassId;
                result.collegename = request.SectionName;
                result.Userid = UserId;
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Section update successfully.");

            }
            catch (Exception ex)
            {

                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> changestatussection(int sectionid)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;


                var classEntity = await _context.collegeinfo.FirstOrDefaultAsync(c => c.collegeid == sectionid && c.CompanyId == SchoolId);

                if (classEntity == null)
                    return ApiResponse<bool>.ErrorResponse("Section not found");


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
        #endregion


        // ************** Subject  Code  
        #region  Subject Details
        public async Task<ApiResponse<List<SubjectResModel>>> getsubject()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;
                int UserId = _loginUser.UserId;

                var subjectEntities = await _context.Subject.Where(cs => cs.CompanyId == SchoolId).ToListAsync();

                var subjectlist = _mapper.Map<List<SubjectResModel>>(subjectEntities);

                return ApiResponse<List<SubjectResModel>>.SuccessResponse(subjectlist, "Subject list fetched successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<SubjectResModel>>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> insertsubject(AddSubjectReq request)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                // Check if section with same name already exists in the school
                var existingSection = await _context.Subject.FirstOrDefaultAsync(s => s.university_id == request.ClassId && (s.subject_name == request.SubjectName) && s.CompanyId == SchoolId);

                if (existingSection != null)
                {
                    return ApiResponse<bool>.ErrorResponse("Subject name & priority are  already exists.");
                }

                // Generate new SectionId
                int SubjectId = _context.Subject.DefaultIfEmpty().Max(s => s == null ? 0 : s.subject_id) + 1;

                var subjectEntity = new Subject
                {
                    subject_id = SubjectId,
                    university_id = request.ClassId,
                    subject_name = request.SubjectName,
                    Priority = request.SubjectPriority,
                    Marks_Type = request.Marks_Type,
                    Quarterly = request.Quarterly,
                    first_test = request.first_test,
                    second_test = request.second_test,
                    third_test = request.third_test,
                    fourth_test = request.fourth_test,
                    half_yearly = request.half_yearly,
                    yearly = request.yearly,
                    CompanyId = SchoolId,
                    Userid = UserId,
                    SessionId = SessionId,
                    active = true,
                };

                _context.Subject.Add(subjectEntity);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Subject inserted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> updatesubject(UpdateSubjectReq request)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;

                Subject subjects = await _context.Subject.Where(p => p.university_id == request.ClassId && p.subject_name == request.SubjectName && p.subject_id != request.SubjectId && p.CompanyId == SchoolId).FirstOrDefaultAsync();
                if (subjects != null)
                {
                    return ApiResponse<bool>.ErrorResponse("Subject Name Already available");
                }

                var result = await _context.Subject.Where(r => r.subject_id == request.SubjectId && r.CompanyId == SchoolId).FirstOrDefaultAsync();

                result.subject_id = request.SubjectId;
                result.university_id = request.ClassId;
                result.subject_name = request.SubjectName;
                result.Priority = request.SubjectPriority;
                result.Marks_Type = request.Marks_Type;
                result.Quarterly = request.Quarterly;
                result.first_test = request.first_test;
                result.second_test = request.second_test;
                result.third_test = request.third_test;
                result.fourth_test = request.fourth_test;
                result.half_yearly = request.half_yearly;
                result.yearly = request.yearly;
                result.Userid = UserId;
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Subject update successfully.");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> changestatussubject(int subjectid)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;


                var classEntity = await _context.Subject.FirstOrDefaultAsync(c => c.subject_id == subjectid && c.CompanyId == SchoolId);

                if (classEntity == null)
                    return ApiResponse<bool>.ErrorResponse("Subject not found");


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
        #endregion


        // **************** Grade  Code 
        #region Grade details
        public async Task<ApiResponse<List<GradeReqModel>>> GetGradeList()
        {
            try
            {

                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var gradeEntities = await _context.GradeInfo.Where(cs => cs.CompanyId == SchoolId && cs.Active == true).ToListAsync();

                var gradelist = _mapper.Map<List<GradeReqModel>>(gradeEntities);


                return ApiResponse<List<GradeReqModel>>.SuccessResponse(gradelist, "grade list fetched successfully");

            }
            catch (Exception ex)
            {

                return ApiResponse<List<GradeReqModel>>.ErrorResponse("Error: " + ex.Message);

            }

        }
        public async Task<ApiResponse<bool>> insertgrade(AddGradeReq request)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;
                int UserId = _loginUser.UserId;

                decimal emPercentFrom = Convert.ToDecimal(request.Percent_From);
                decimal emPercentUpto = Convert.ToDecimal(request.Percent_Upto);

                var existingGrades = await _context.GradeInfo.Where(g => g.CompanyId == SchoolId).ToListAsync();

                GradeInfo eve = existingGrades.FirstOrDefault(p => p.grade_name == request.grade_name ||
                    (Convert.ToDecimal(p.Percent_From) <= emPercentFrom && emPercentFrom <= Convert.ToDecimal(p.Percent_Upto)) ||
                    (Convert.ToDecimal(p.Percent_From) <= emPercentUpto && emPercentUpto <= Convert.ToDecimal(p.Percent_Upto))
                );

                if (eve != null)
                    return ApiResponse<bool>.ErrorResponse("Grade is Already available");

                var newGrade = new GradeInfo
                {
                    grade_id = _context.GradeInfo.DefaultIfEmpty().Max(r => r == null ? 0 : r.grade_id) + 1,
                    grade_name = request.grade_name,
                    Percent_From = request.Percent_From,
                    Percent_Upto = request.Percent_Upto,
                    Active = true,
                    CompanyId = SchoolId,
                    SessionId = SessionId,
                    Userid = UserId,
                };
                _context.GradeInfo.Add(newGrade);
                _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Grade insert successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> updategrade(UpdateGradeReq request)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;

                decimal emPercentFrom = Convert.ToDecimal(request.Percent_From);
                decimal emPercentUpto = Convert.ToDecimal(request.Percent_Upto);

                var existingGrades = await _context.GradeInfo.Where(s => s.CompanyId == SchoolId).ToListAsync();

                GradeInfo eve = existingGrades.FirstOrDefault(p => (p.grade_name == request.grade_name && p.grade_id != request.grade_id) ||
                     (emPercentFrom >= Convert.ToDecimal(p.Percent_From) && emPercentFrom <= Convert.ToDecimal(p.Percent_Upto) && p.grade_id != request.grade_id) ||
                     (emPercentUpto >= Convert.ToDecimal(p.Percent_From) && emPercentUpto <= Convert.ToDecimal(p.Percent_Upto) && p.grade_id != request.grade_id) ||
                     (emPercentFrom <= Convert.ToDecimal(p.Percent_From) && emPercentUpto >= Convert.ToDecimal(p.Percent_Upto) && p.grade_id != request.grade_id)
                );

                if (eve != null)
                {
                    return ApiResponse<bool>.ErrorResponse("Grade is Already available");

                }

                var result = await _context.GradeInfo.Where(r => r.grade_id == request.grade_id && r.CompanyId == SchoolId).FirstOrDefaultAsync();

                result.grade_name = request.grade_name;
                result.Percent_From = request.Percent_From;
                result.Percent_Upto = request.Percent_Upto;
                result.Userid = UserId;

                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Grade update successfully.");

            }
            catch (Exception ex)
            {

                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> changestatusgrade(int gradeid)
        {
            throw new NotImplementedException();
        }
        #endregion


        // *************** Event Code 
        #region Event Details
        public async Task<ApiResponse<List<GetEventReqModel>>> GetEvent()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var EventEvenities = await _context.Event.Where(cs => cs.CompanyId == SchoolId).ToListAsync();

                var Eventlist = _mapper.Map<List<GetEventReqModel>>(EventEvenities);

                return ApiResponse<List<GetEventReqModel>>.SuccessResponse(Eventlist, "Event list fetched successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetEventReqModel>>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> InsertEvent(EventReqModel req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;
                int UserId = _loginUser.UserId;

                var existingEvent = await _context.Event.FirstOrDefaultAsync(a => a.CompanyId == SchoolId && a.EventName == req.Eventname);
                if (existingEvent != null)
                {
                    return ApiResponse<bool>.ErrorResponse("Event Name Already exists.");
                }


                var EventEntity = new Event
                {
                    EventID = _context.Event.DefaultIfEmpty().Max(s => s == null ? 0 : s.EventID) + 1,

                    EventName = req.Eventname,
                    Active = true,
                    Date = DateTime.Now,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    CompanyId = SchoolId,
                    SessionId = SessionId,
                    Userid = UserId,
                };

                _context.Event.Add(EventEntity);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Event Insert Successfully. ");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error:  " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateEvent(UpdateEventReqModel req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;

                Event Event = await _context.Event.Where(a => a.CompanyId == SchoolId && a.EventName == req.Eventname && a.EventID != req.EventId).FirstOrDefaultAsync();
                if (Event != null)
                {
                    return ApiResponse<bool>.ErrorResponse("Event Name Already available.");
                }

                var result = await _context.Event.Where(p => p.EventID == req.EventId && p.CompanyId == SchoolId).FirstOrDefaultAsync();

                result.EventName = req.Eventname;
                result.Userid = UserId;
                result.UpdateDate = DateTime.Now;
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Event Update Successfully.");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> ChangeEventStatus(int EventId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;

                var EventEnetities = await _context.Event.FirstOrDefaultAsync(p => p.EventID == EventId && p.CompanyId == SchoolId);
                if (EventEnetities == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Event not found");
                }

                EventEnetities.Active = EventEnetities.Active == null ? true : !EventEnetities.Active;

                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "Status update successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        #endregion



        /// *************** Exam Code Start 
        #region Exam Details
        //public async Task<ApiResponse<List<GetExamReqModel>>> GetExam()
        //{
        //    try
        //    {
        //        int SchoolId = _loginUser.SchoolId;
        //        int SessionId = _loginUser.SessionId;

        //        var ExamEnetities = await _context.ExamTbl.Where(p => p.SchoolId == SchoolId).ToListAsync();
        //        var Examlist = _mapper.Map<List<GetExamReqModel>>(ExamEnetities);

        //        return ApiResponse<List<GetExamReqModel>>.SuccessResponse(Examlist, "Exam List fetched successfully .");
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiResponse<List<GetExamReqModel>>.ErrorResponse("Error: " + ex.Message);

        //    }
        //}
        //public async Task<ApiResponse<bool>> InsertExam(ExamreqModel req)
        //{
        //    try
        //    {
        //        int SchoolId = _loginUser.SchoolId;
        //        int SessionId = _loginUser.SessionId;
        //        int UserId = _loginUser.UserId;

        //        var existingexam = await _context.ExamTbl.FirstOrDefaultAsync(p => (p.ExamName == req.ExamName || p.ExamPriority == req.ExamPriority) && p.SchoolId == SchoolId);
        //        if (existingexam != null)
        //        {
        //            return ApiResponse<bool>.ErrorResponse("Exam is Already exists..");
        //        }

        //        var ExamEntity = new ExamTbl
        //        {
        //            ExamId = _context.ExamTbl.DefaultIfEmpty().Max(s => s == null ? 0 : s.ExamId) + 1,
        //            ExamName = req.ExamName,
        //            ExamPriority = req.ExamPriority,
        //            EActive = true,
        //            CreateDate = DateTime.Now,
        //            UpdateDate = DateTime.Now,
        //            SchoolId = SchoolId,
        //            UserId = UserId,
        //            SessionId = SessionId,
        //        };

        //        _context.ExamTbl.Add(ExamEntity);
        //        await _context.SaveChangesAsync();

        //        return ApiResponse<bool>.SuccessResponse(true, "Exam insert successfully. ");

        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
        //    }
        //}
        //public async Task<ApiResponse<bool>> UpdateExam(UpdateExamreqModel req)
        //{
        //    try
        //    {
        //        int SchoolId = _loginUser.SchoolId;
        //        int UserId = _loginUser.UserId;

        //        ExamTbl ExiestExam = await _context.ExamTbl.Where(p => (p.ExamName == req.ExamName || p.ExamPriority == req.ExamPriority) && p.ExamId != req.ExamId && p.SchoolId == SchoolId).FirstOrDefaultAsync();
        //        if (ExiestExam != null)
        //        {
        //            return ApiResponse<bool>.ErrorResponse("Exam Is Already available");
        //        }

        //        var result = await _context.ExamTbl.Where(s => s.ExamId == req.ExamId && s.SchoolId == SchoolId).FirstOrDefaultAsync();
        //        result.ExamName = req.ExamName;
        //        result.ExamPriority = req.ExamPriority;
        //        result.UpdateDate = DateTime.Now;
        //        result.UserId = UserId;
        //        await _context.SaveChangesAsync();

        //        return ApiResponse<bool>.SuccessResponse(true, "Update Exam Successfully.");
        //    }
        //    catch (Exception ex)
        //    {

        //        return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
        //    }
        //}
        //public async Task<ApiResponse<bool>> ExamChangeStatus(int ExamId)
        //{
        //    try
        //    {
        //        int SchoolId = _loginUser.SchoolId;

        //        var ExamEnetities = await _context.ExamTbl.FirstOrDefaultAsync(p => p.ExamId == ExamId && p.SchoolId == SchoolId);
        //        if (ExamEnetities == null)
        //        {
        //            return ApiResponse<bool>.ErrorResponse("Exam not found");
        //        }

        //        ExamEnetities.EActive = ExamEnetities.EActive == null ? true : !ExamEnetities.EActive;
        //        await _context.SaveChangesAsync();

        //        return ApiResponse<bool>.SuccessResponse(true, "Status update successfully");

        //    }
        //    catch (Exception ex)
        //    {

        //        return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
        //    }
        //}
        #endregion


        // *************** Exam Marks 
        #region Exam Marks etails
        //public async Task<ApiResponse<List<GetClassbysubjectMarksmodel>>> GetExamTotalMarks()
        //{
        //    try
        //    {
        //        int SchoolId = _loginUser.SchoolId;
        //        int SessionId = _loginUser.SessionId;

        //        var ExamMarksEnetity = await (from exammarks in _context.ClassSubjectExamTbl
        //                                      join cls in _context.University
        //                                       on exammarks.ClassId equals cls.university_id
        //                                      join sub in _context.Subject
        //                                      on exammarks.SubjectId equals sub.subject_id
        //                                      join exam in _context.ExamTbl
        //                                      on exammarks.ExamId equals exam.ExamId
        //                                      where exammarks.SchoolId == SchoolId
        //                                      select new GetClassbysubjectMarksmodel
        //                                      {
        //                                          ClassId = exammarks.ClassId,
        //                                          ClassName = cls.university_name,
        //                                          ExamId = exammarks.ExamId,
        //                                          ExamName = exam.ExamName,
        //                                          SubjectId = exammarks.SubjectId,
        //                                          Subjectname = sub.subject_name,
        //                                          MarksType = exammarks.MarksType,
        //                                          MaxMarks = exammarks.MaxMarks,
        //                                      }).ToListAsync();

        //        var ExamMarkslIst = _mapper.Map<List<GetClassbysubjectMarksmodel>>(ExamMarksEnetity);

        //        return ApiResponse<List<GetClassbysubjectMarksmodel>>.SuccessResponse(ExamMarkslIst, "Fetch exam marks successfully");

        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiResponse<List<GetClassbysubjectMarksmodel>>.ErrorResponse("Error:  " + ex.Message);
        //    }
        //}
        //public async Task<ApiResponse<bool>> insertExamTotalMarks(ExamMarksModel req)
        //{
        //    try
        //    {
        //        int SchoolId = _loginUser.SchoolId;
        //        int SessionId = _loginUser.SessionId;
        //        int UserId = _loginUser.UserId;

        //        var existingexammarks = await _context.ClassSubjectExamTbl.FirstOrDefaultAsync(p => p.ClassId == req.ClassId && p.SubjectId == req.SubjectId && p.ExamId == req.ExamId && p.SchoolId == SchoolId);
        //        if (existingexammarks != null)
        //        {
        //            return ApiResponse<bool>.ErrorResponse("Exam marks already avaiable");
        //        }

        //        var ExammarksEnetity = new ClassSubjectExamTbl
        //        {
        //            CSEId = _context.ClassSubjectExamTbl.DefaultIfEmpty().Max(s => s == null ? 0 : s.CSEId) + 1,
        //            ClassId = req.ClassId,
        //            SubjectId = req.SubjectId,
        //            ExamId = req.ExamId,
        //            MarksType = req.MarksType,
        //            MaxMarks = req.MaxMarks,
        //            CreateDate = DateTime.Now,
        //            UpdateDate = DateTime.Now,
        //            SchoolId = SchoolId,
        //            SessionId = SessionId,
        //            UserId = UserId,
        //        };
        //        _context.ClassSubjectExamTbl.Add(ExammarksEnetity);
        //        await _context.SaveChangesAsync();

        //        return ApiResponse<bool>.SuccessResponse(true, "Exam marks insert successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
        //    }
        //}

        #endregion


    }
}

