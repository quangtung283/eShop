using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Data.Entities
{
    public class User :IdentityUser<Guid>
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public DateTime Dob {  get; set; }
        public List<Cart> Carts { get; set; }
        public List<Order> Orders { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
