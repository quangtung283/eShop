using eShop.ViewModels.Common;
using eShop.ViewModels.System.Languages;

namespace eShop.ApiIntegration
{
    public interface ILanguageApiClient
    {
        Task<ApiResult<List<LanguageVm>>> GetAll();
    }
}
