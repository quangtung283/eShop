using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Service.DTOs
{
    public class PagedResult<T>
    {
        List<T> Items {  get; set; }    
        public int TotalRecord { get; set; }
    }
}
