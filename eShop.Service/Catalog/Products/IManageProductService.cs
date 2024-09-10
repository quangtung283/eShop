using eShop.Service.Catalog.Products.DTOs;
using eShop.Service.Catalog.Products.DTOs.Manage;
using eShop.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Task<List<ProductViewModel>> GetAll();
        Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPaggingDTOs getProductPagging);
        
    }
}
