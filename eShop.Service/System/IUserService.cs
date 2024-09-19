using eShop.ViewModels.Common.DTOs;
using eShop.ViewModels.System.User;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShop.Service.System
{
    public interface IUserService
    {
        Task<string> Authencate(LoginRequest request);
        Task<bool> Register(RegisterRequest request);
        Task<PagedResult<UserVM>> GetUserPaging(GetUserPagingRequest request);
    }
}
