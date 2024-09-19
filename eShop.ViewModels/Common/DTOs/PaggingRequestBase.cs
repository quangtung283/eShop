using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.ViewModels.Common.DTOs
{
    public class PaggingRequestBase :RequetsBase
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
