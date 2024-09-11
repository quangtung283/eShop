using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Catalog.Products.Manage;
using eShop.ViewModels.Common.DTOs;
using eShop.ViewModelsViewModels.Catalog.ProductManage;


namespace eShop.Service.Catalog.Products
{
    public interface IManageProductService
    {
        Task<int> Create(ProductCreateDTOs createProduct);

        Task<int> Update(UpdateProductDTOs updateProduct);

        Task<int> Delete(int productId); 
        Task<bool> UpdatePrice(int productId,decimal newPrice);
        Task<bool> UpdateStock(int productId,int addedQuantity);
        Task AddViewCount(int productId,int viewCount);
        //Task<List<ProductViewModel>> GetAll();
        Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPaggingDTOs getProductPagging);
        
    }
}
