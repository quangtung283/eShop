using eShop.Service.Catalog.Products.DTOs;
using eShop.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Service.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {
        public PagedResult<ProductViewModel> GetAllByCategoryId(int categoryId, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
