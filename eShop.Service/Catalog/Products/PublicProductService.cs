using eShop.Service.Catalog.Products.DTOs;
using eShop.Service.Catalog.Products.DTOs.Public;
using eShop.Service.DTOs;


namespace eShop.Service.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {
        public PagedResult<ProductViewModel> GetAllByCategoryId(GetProductPagingRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
