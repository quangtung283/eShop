using eShop.ViewModels.System.User;

namespace eShop.Service.System
{
    public interface IUserService
    {
        Task<string> Authencate(LoginRequest request);
        Task<bool> Register(RegisterRequest request);
    }
}
