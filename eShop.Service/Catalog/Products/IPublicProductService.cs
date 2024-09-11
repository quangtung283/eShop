using eShop.Service.Catalog.Products.DTOs;
using eShop.Service.Catalog.Products.DTOs.Public;
using eShop.Service.DTOs;


namespace eShop.Service.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetProductPagingRequest request);
    }
}
