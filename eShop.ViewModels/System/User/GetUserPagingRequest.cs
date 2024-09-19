using eShop.ViewModels.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.ViewModels.System.User
{
    public class GetUserPagingRequest :PaggingRequestBase
    {
        public string Keyword { get; set; }
    }
}
