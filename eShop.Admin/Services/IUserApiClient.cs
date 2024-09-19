using eShop.ViewModels.Common;
using eShop.ViewModels.System.User;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShop.Admin.Services
{
    public interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequest request);
        Task<PagedResult<UserVm>>GetUsersPaging(GetUserPagingRequest request);
    }
}
