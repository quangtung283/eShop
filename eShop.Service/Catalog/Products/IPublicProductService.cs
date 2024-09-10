﻿using eShop.Service.Catalog.Products.DTOs;
using eShop.Service.Catalog.Products.DTOs.Public;
using eShop.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Service.Catalog.Products
{
    public interface IPublicProductService
    {
         PagedResult<ProductViewModel> GetAllByCategoryId(GetProductPagingRequest request);
    }
}
