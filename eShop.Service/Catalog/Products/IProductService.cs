using eShop.ViewModels.Catalog.ProductImages;
using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Common.DTOs;
using Microsoft.AspNetCore.Http;


namespace eShop.Service.Catalog.Products
{
    public interface IProductService
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
        Task<int> AddImage(int productId, ProductImageCreateRequest imageCreateRequest);
        Task<ProductImageViewModel> GetImageById(int imageId);
        Task<int> RemoveImage(int imageId);
        Task<int> UpdateImage( int imageId,ProductImageUpdateRequest request);
        Task<List<ProductImageViewModel>> GetListImage(int productId);
        Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request);
        Task<List<ProductViewModel>> GetAll(string languageId);
    }
}
