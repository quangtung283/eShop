using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Catalog.Products.Public;
using eShop.ViewModels.Common.DTOs;


namespace eShop.Service.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetProductPagingRequest request);
    }
}
