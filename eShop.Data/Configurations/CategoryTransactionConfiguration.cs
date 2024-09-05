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
    public class CategoryTransactionConfiguration : IEntityTypeConfiguration<CategoryTranslation>
    {
        public void Configure(EntityTypeBuilder<CategoryTranslation> builder)
        {
           builder.ToTable("CategoryTranslations");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c=>c.Name).IsRequired().HasMaxLength(200);
            builder.Property(c=>c.SeoAlias).IsRequired().HasMaxLength(200);
            builder.Property(c=>c.SeoDescription).IsRequired().HasMaxLength(500);
            builder.Property(c=>c.SeoTitle).IsRequired().HasMaxLength(200);
            builder.Property(c=>c.LanguageId).IsUnicode(false).IsRequired().HasMaxLength(5);
            builder.HasOne(c=>c.Language).WithMany(x=>x.CategoryTranslations).HasForeignKey(c => c.LanguageId);
            builder.HasOne(c => c.Category).WithMany(x => x.CategoryTranslations).HasForeignKey(c => c.CategoryId);
        }
    }
}
