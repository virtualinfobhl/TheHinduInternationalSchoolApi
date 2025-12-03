using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiProject.Service.Login
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //public async Task<int?> CheckSchoolCodeAsync(string schoolcode)
        //{
        //    return await _context.SchoolTbl.Where(p => p.SchoolCode == schoolcode.ToUpper() && p.Active == true).Select(p => (int?)p.SchoolId).FirstOrDefaultAsync();
        //}

        public async Task<ApiResponse<LoginRes>> LoginAsync(RequestLogin request)
        {
            try
            {
                //var company = await _context.SchoolTbl.FirstOrDefaultAsync(p => p.SchoolCode == request.schoolcode.ToUpper() && p.Active == true);

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

                var user = await _context.UserInformation.FirstOrDefaultAsync(p => p.Username == request.username && p.Active == true);

                if (user.UserRole != "Admin")
                {
                    return ApiResponse<LoginRes>.ErrorResponse("Access denied. Only Admin can login");
                }

                if (user == null)
                {
                    return ApiResponse<LoginRes>.ErrorResponse("User Does Not Exist");
                }
                else if (user.Active == false)
                {
                    return ApiResponse<LoginRes>.ErrorResponse("User Deactive By Super Admin");
                }
                else if (user.Password != request.password)
                {
                    return ApiResponse<LoginRes>.ErrorResponse("User Password Incorrect");
                }

                var SchoolInfo = await _context.institute.FirstOrDefaultAsync(a => a.institute_id == user.CompanyId);
                var sessioninfo = await _context.SessionInfo.FirstOrDefaultAsync(p => p.CompanyId == user.CompanyId && p.Active == true);

                var startdate = Convert.ToDateTime(sessioninfo.StartSession).ToString("dd-MMM-yyyy");
                var enddate = Convert.ToDateTime(sessioninfo.EndSession).ToString("dd-MMM-yyyy");

                var startYear = Convert.ToDateTime(sessioninfo.StartSession).Year.ToString();
                var endYear = Convert.ToDateTime(sessioninfo.EndSession).Year.ToString();

                var token = GenerateJwtToken(user, sessioninfo.Id);

                var data = new LoginRes
                {
                    Token = token,
                    SchoolName = SchoolInfo.institute_name,
                    SchoolAddress = SchoolInfo.address,
                    Pincode = SchoolInfo.pincode,
                    MobileNo1 = SchoolInfo.mob_num,
                    UserName = user.Username,
                    Name = user.Name,
                    Status = user.Status,
                    UserType = user.Status,
                    StartSession = startdate,
                    EndSession = enddate,
                    StartSessionYear = startYear,
                    EndSessionYear = endYear
                };
                return ApiResponse<LoginRes>.SuccessResponse(data, "successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<LoginRes>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        private string GenerateJwtToken(UserInformation user, int sessionid)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim("UserId", user.id.ToString()),
                new Claim("SchoolId", user.CompanyId.ToString()),
                new Claim("SessionId", sessionid.ToString()),
                new Claim("UserName", user.Name.ToString()),
                new Claim(ClaimTypes.Role, user.Status)
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
