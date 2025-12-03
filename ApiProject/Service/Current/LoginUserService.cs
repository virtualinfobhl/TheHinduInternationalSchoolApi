using System.Security.Claims;

namespace ApiProject.Service.Current
{
    public class LoginUserService : ILoginUserService
    {
        public int UserId { get; }

        public int SchoolId { get; }
        public int SessionId { get; }
        public int ParentId { get; }
        public int StudentId { get; }

        public string UserName { get; }

        public LoginUserService(IHttpContextAccessor httpContextAccessor)
        {
            var identity = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;

            if (identity != null && identity.IsAuthenticated)
            {
                UserId = Convert.ToInt32(identity.FindFirst("UserId")?.Value);
                SchoolId = Convert.ToInt32(identity.FindFirst("SchoolId")?.Value);
                SessionId = Convert.ToInt32(identity.FindFirst("SessionId")?.Value);
                ParentId = Convert.ToInt32(identity.FindFirst("ParentId")?.Value);
                StudentId = Convert.ToInt32(identity.FindFirst("StudentId")?.Value);

                UserName = identity.FindFirst(ClaimTypes.Name)?.Value;
            }
        }
    }
}
