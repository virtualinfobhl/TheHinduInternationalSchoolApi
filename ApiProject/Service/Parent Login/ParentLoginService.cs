using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiProject.Service.Parent_Login
{
    public class ParentLoginService : IParentLoginService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public ParentLoginService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        public async Task<ApiResponse<LoginRes>> ParentLoginAsync(RequestLogin request)
        {
            try
            {

                //var company = await _context.studentTbl.FirstOrDefaultAsync(p => p.SchoolCode == request.schoolcode.ToUpper() && p.Active == true);

                //if (company == null)
                //{
                //    return ApiResponse<LoginRes>.ErrorResponse("School Does Not Exist");
                //}


                //var expiry = Convert.ToDateTime(company.ExpireDate);
                //double totalDays = (expiry - DateTime.Now).TotalDays;

                //if (totalDays <= 0)
                //{
                //    return ApiResponse<LoginRes>.ErrorResponse("School Software services has expired");
                //}



                var user = await _context.ParentsTbl.FirstOrDefaultAsync(p => p.Username == request.username);
                if (user == null)
                {
                    return ApiResponse<LoginRes>.ErrorResponse("User Deactive By Super Admin");
                }
                else if (user.Password != request.password)
                {
                    return ApiResponse<LoginRes>.ErrorResponse("User Password Incorrect");
                }

                var company = await _context.student_admission.FirstOrDefaultAsync(p => p.ParentsId == user.ParentsId && p.active == true);
                var sessioninfo = await _context.SessionInfo.FirstOrDefaultAsync(p => p.CompanyId == company.CompanyId && p.Active == true);

                var startdate = Convert.ToDateTime(sessioninfo.StartSession).ToString("dd-MMM-yyyy");
                var enddate = Convert.ToDateTime(sessioninfo.EndSession).ToString("dd-MMM-yyyy");
                var startYear = Convert.ToDateTime(sessioninfo.StartSession).Year.ToString();
                var endYear = Convert.ToDateTime(sessioninfo.EndSession).Year.ToString();

                var studentDetail = _context.StudentRenewView.Where(r => r.ParentsId == user.ParentsId && r.CompanyId == company.CompanyId && r.RActive == true && r.SessionId == sessioninfo.Id).FirstOrDefault();

                var token = GenerateJwtToken(user, studentDetail, sessioninfo.Id);


                var data = new LoginRes
                {
                    Token = token,
                   // SchoolCode = company.SchoolCode,
                   // SchoolName = company.,
                    ParentId = user.ParentsId,
                    ParentName = user.FatherName,
                    ParentMobile = user.FatherMobileNo,
                    StudentId = studentDetail?.StuId,
                    StudentName = studentDetail?.stu_name,
                   // Class = studentDetail?.ClassName,
                    SrNo = studentDetail?.registration_no,
                    StartSession = startdate,
                    EndSession = enddate
                };

                return ApiResponse<LoginRes>.SuccessResponse(data, "successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<LoginRes>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        private string GenerateJwtToken(ParentsTbl user, StudentRenewView studentDetail, int sessionid)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.ParentsId.ToString()),
                new Claim("ParentId", user.ParentsId.ToString()),
                new Claim("SchoolId", user.CompanyId.ToString()),
                new Claim("SessionId", sessionid.ToString()),
                 new Claim("StudentId", studentDetail.StuId.ToString()),
                  new Claim("Studentname", studentDetail.stu_name.ToString()),
                  new Claim("ClassId", studentDetail.ClassId.ToString()),
               //  new Claim("ClassName", studentDetail.ClassName.ToString()),
                new Claim("ParentName", user.FatherName.ToString())
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
    }
}
