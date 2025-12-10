using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using Microsoft.AspNetCore.Mvc;
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


        //public async Task<ApiResponse<LoginRes>> LoginAsync(RequestLogin request)
        //{
        //    try
        //    {
        //        var user = await _context.UserInformation.FirstOrDefaultAsync(p => p.Username == request.username && p.Active == true);

        //        if (user.UserRole != "Admin")
        //        {
        //            return ApiResponse<LoginRes>.ErrorResponse("Access denied. Only Admin can login");
        //        }

        //        if (user == null)
        //        {
        //            return ApiResponse<LoginRes>.ErrorResponse("User Does Not Exist");
        //        }
        //        else if (user.Active == false)
        //        {
        //            return ApiResponse<LoginRes>.ErrorResponse("User Deactive By Super Admin");
        //        }
        //        else if (user.Password != request.password)
        //        {
        //            return ApiResponse<LoginRes>.ErrorResponse("User Password Incorrect");
        //        }

        //        var SchoolInfo = await _context.institute.FirstOrDefaultAsync(a => a.institute_id == user.CompanyId);
        //        var sessioninfo = await _context.SessionInfo.FirstOrDefaultAsync(p => p.CompanyId == user.CompanyId && p.Active == true);

        //        var startdate = Convert.ToDateTime(sessioninfo.StartSession).ToString("dd-MMM-yyyy");
        //        var enddate = Convert.ToDateTime(sessioninfo.EndSession).ToString("dd-MMM-yyyy");

        //        var startYear = Convert.ToDateTime(sessioninfo.StartSession).Year.ToString();
        //        var endYear = Convert.ToDateTime(sessioninfo.EndSession).Year.ToString();

        //        var token = GenerateJwtToken(user, sessioninfo.Id);

        //        var data = new LoginRes
        //        {
        //            Token = token,
        //            SchoolName = SchoolInfo.institute_name,
        //            SchoolAddress = SchoolInfo.address,
        //            Pincode = SchoolInfo.pincode,
        //            MobileNo1 = SchoolInfo.mob_num,
        //            UserName = user.Username,
        //            Name = user.Name,
        //            Status = user.Status,
        //            UserType = user.Status,
        //            StartSession = startdate,
        //            EndSession = enddate,
        //            StartSessionYear = startYear,
        //            EndSessionYear = endYear
        //        };
        //        return ApiResponse<LoginRes>.SuccessResponse(data, "successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiResponse<LoginRes>.ErrorResponse("Something went wrong: " + ex.Message);
        //    }
        //}



        public async Task<ApiResponse<LoginRes>> LoginAsync(RequestLogin request)
        {
            try
            {
                var user = await _context.UserInformation.FirstOrDefaultAsync(p => p.Username == request.username && p.Active == true);

                // ✅ User exist check (FIRST)
                if (user == null)
                {
                    return ApiResponse<LoginRes>.ErrorResponse("User Does Not Exist");
                }

                // ✅ Password check
                if (user.Password != request.password)
                {
                    return ApiResponse<LoginRes>.ErrorResponse("User Password Incorrect");
                }

                // ✅ Role check (Admin + Teacher allowed)
                var allowedRoles = new List<string> { "Admin", "Teacher" };

                if (!allowedRoles.Contains(user.UserRole))
                {
                    return ApiResponse<LoginRes>.ErrorResponse("Access denied. Only Admin and Teacher can login");
                }

                // ✅ Active check
                if (user.Active == false)
                {
                    return ApiResponse<LoginRes>.ErrorResponse("User Deactive By Super Admin");
                }

                var schoolInfo = await _context.institute.FirstOrDefaultAsync(a => a.institute_id == user.CompanyId);

                var sessioninfo = await _context.SessionInfo.FirstOrDefaultAsync(p => p.CompanyId == user.CompanyId && p.Active == true);

                var token = GenerateJwtToken(user, sessioninfo.Id);

                var SchoolInfo = await _context.institute.FirstOrDefaultAsync(a => a.institute_id == user.CompanyId);
                //   var sessioninfo = await _context.SessionInfo.FirstOrDefaultAsync(p => p.CompanyId == user.CompanyId && p.Active == true);

                var startdate = Convert.ToDateTime(sessioninfo.StartSession).ToString("dd-MMM-yyyy");
                var enddate = Convert.ToDateTime(sessioninfo.EndSession).ToString("dd-MMM-yyyy");

                var startYear = Convert.ToDateTime(sessioninfo.StartSession).Year.ToString();
                var endYear = Convert.ToDateTime(sessioninfo.EndSession).Year.ToString();

                var data = new LoginRes
                {
                    Token = token,
                    SchoolName = schoolInfo.institute_name,
                    SchoolAddress = schoolInfo.address,
                    Pincode = schoolInfo.pincode,
                    MobileNo1 = schoolInfo.mob_num,
                    UserName = user.Username,
                    Name = user.Name,
                    Status = user.Status,
                    UserType = user.UserRole,
                    StartSession = startdate,
                    EndSession = enddate,
                    StartSessionYear = startYear,
                    EndSessionYear = endYear

                };

                return ApiResponse<LoginRes>.SuccessResponse(data, "Login successfully");
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

// pdf generate
//public ActionResult Luggagepdf(int id)
//{
//    LuggageBooking Data = db.LuggageBookings.Where(p => p.id == id).FirstOrDefault();


//    ViewBag.BalanceAmount = Data.TotalAmt + Data.GstCharge;
//    ViewBag.To = Data.FinaleStation == "" ? Data.ToDestination : Data.FinaleStation;
//    ViewBag.Freight = Data.FrightPerUnit * Data.TotalParcel;
//    ViewBag.LabourCharge = Data.LabourCharge * Data.TotalParcel;
//    ViewBag.Companyname = db.CompanyInformations.Where(c => c.id == Data.CompanyId).Select(c => c.CompanyName).FirstOrDefault();
//    ViewBag.CompanyLogo = db.CompanyInformations.Where(c => c.id == Data.CompanyId).Select(c => c.CompanyLogo).FirstOrDefault();
//    ViewBag.CompanyGstNo = db.CompanyInformations.Where(c => c.id == Data.CompanyId).Select(c => c.GstNo).FirstOrDefault();
//    ViewBag.BAddress = db.BranchInformations.Where(b => b.id == Data.branchid).Select(b => b.BAddress).FirstOrDefault();
//    ViewBag.BName = db.BranchInformations.Where(b => b.id == Data.branchid).Select(b => b.BName).FirstOrDefault();
//    ViewBag.BMobileno = db.BranchInformations.Where(b => b.id == Data.branchid).Select(b => b.BMobileno).FirstOrDefault();
//    ViewBag.UserName = db.UserInformations.Where(q => q.id == Data.userid).Select(q => q.Name).FirstOrDefault();
//    ViewBag.DelAddress = db.Routeinfoes.Where(r => r.id == Data.ToDestId).Select(r => r.DeliveryAddress).FirstOrDefault();

//    return new PartialViewAsPdf("Luggagepdf", Data)
//    {
//        PageOrientation = Rotativa.Options.Orientation.Landscape,
//        PageSize = Rotativa.Options.Size.A5,
//        CustomSwitches = "--footer-center \" [page] Page of [toPage] Pages\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"",
//        FileName = "LuggageBooking" + id + ".pdf"
//    };
//}