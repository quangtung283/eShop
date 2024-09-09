using eShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.Property(u=>u.FirstName).IsRequired().HasMaxLength(200);
            builder.Property(u=> u.LastName).IsRequired().HasMaxLength(200);
            builder.Property(u => u.Dob).IsRequired();
        }
    }
}
