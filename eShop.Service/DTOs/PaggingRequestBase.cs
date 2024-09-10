using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Service.DTOs
{
    public class PaggingRequestBase
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
