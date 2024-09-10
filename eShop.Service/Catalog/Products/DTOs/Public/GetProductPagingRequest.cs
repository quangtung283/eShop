using eShop.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Service.Catalog.Products.DTOs.Public
{
    public class GetProductPagingRequest :PaggingRequestBase
    {
        public int CategoryId { get; set; }
    }
}
