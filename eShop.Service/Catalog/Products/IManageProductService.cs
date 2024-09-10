using eShop.Service.Catalog.Products.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Service.Catalog.Products
{
    public interface IManageProductService
    {
        int Create(ProductCreateDTOs createProduct);

        int Update(UpdateProductDTOs updateProduct);

        int Delete(int productId); 
        List<ProductViewModel> GetAll();
        List<ProductViewModel> GetAllPaging(string keyword , int pageSize , int pageIndex);
    }
}
