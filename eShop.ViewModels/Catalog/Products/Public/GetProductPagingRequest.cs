using eShop.ViewModels.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.ViewModels.Catalog.Products.Public
{
    public class GetProductPagingRequest :PaggingRequestBase
    {
        public int? CategoryId { get; set; }
    }
}
