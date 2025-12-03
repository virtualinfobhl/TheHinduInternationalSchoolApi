using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;

namespace ApiProject.Service.Parent_Login
{
    public interface IParentLoginService
    {
        Task<ApiResponse<LoginRes>> ParentLoginAsync(RequestLogin request);
    }
}
