using eShop.ViewModels.Common.DTOs;
using eShop.ViewModels.System.User;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShop.Admin.Services
{
    public interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequest request);
        Task<PagedResult<UserVM>>GetUsersPaging(GetUserPagingRequest request);
    }
}
