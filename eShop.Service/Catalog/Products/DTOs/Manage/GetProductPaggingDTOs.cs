using eShop.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Service.Catalog.Products.DTOs.Manage
{
    public class GetProductPaggingDTOs : PaggingRequestBase
    {
        public string KeyWord { get; set; }
        public List<int> CategoryId { get; set; }
    }
}
