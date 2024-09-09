using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Data.Entities
{
    public class Role :IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}
