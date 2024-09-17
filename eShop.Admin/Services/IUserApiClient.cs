using eShop.ViewModels.System.User;

namespace eShop.Admin.Services
{
    public interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequest request);
    }
}
