using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;

namespace ApiProject.Service.Login
{
    public interface IAuthService
    {
       // Task<int?> CheckSchoolCodeAsync(string schoolcode);
        Task<ApiResponse<LoginRes>> LoginAsync(RequestLogin request);
    }
}
