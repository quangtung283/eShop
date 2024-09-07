using eShop.API.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Data.EF
{
    public class eShopDbContextFactory : IDesignTimeDbContextFactory<eShopDBContext>
    {
        public eShopDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var optionsBuilder =new DbContextOptionsBuilder<eShopDBContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new eShopDBContext(optionsBuilder.Options);
        }
    }
}
