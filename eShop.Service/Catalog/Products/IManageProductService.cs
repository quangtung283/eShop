using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Common.DTOs;
using Microsoft.AspNetCore.Http;


namespace eShop.Service.Catalog.Products
{
    public interface IManageProductService
    {
        Task<int> Create(ProductCreateDTOs createProduct);

        Task<int> Update(UpdateProductDTOs updateProduct);
        Task<ProductViewModel> GetById(int productId,string languageId);
        Task<int> Delete(int productId); 
        Task<bool> UpdatePrice(int productId,decimal newPrice);
        Task<bool> UpdateStock(int productId,int addedQuantity);
        Task AddViewCount(int productId,int viewCount);
        //Task<List<ProductViewModel>> GetAll();
        Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPaggingDTOs getProductPagging);
        Task<int> AddImages(int productId,List<IFormFile> files);
        Task<int> RemoveImages(int productId);
        Task<int> UpdateImage(int imageId,string caption,bool isDefault);
        Task<List<ProductImageViewModel>> GetListImage(int productId);  
    }
}
