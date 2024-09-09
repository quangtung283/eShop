using eShop.Data.Configurations;
using eShop.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eShop.API.EF
{
    public class eShopDBContext:IdentityDbContext<User,Role,Guid>
    {
        public eShopDBContext(DbContextOptions options) : base(options) 
        { 
           
        }
       /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("DefaultConnection");
            }

        }*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AppConfiguration());    
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductInCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new ContactCofiguration());
            modelBuilder.ApplyConfiguration(new LanguageConfiguration());
            modelBuilder.ApplyConfiguration(new ProductTranslationConfiguration());
            modelBuilder.ApplyConfiguration(new PromotionConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());


            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles").HasKey(x=>new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogin").HasKey(x=>x.UserId);
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens").HasKey(x=>x.UserId);
            // base.OnModelCreating(modelBuilder);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<AppConfig> AppConfigs { get; set; }    
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; } 
        public DbSet<CategoryTranslation> CategoriesTranslation { get; set; }   
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }    
        public DbSet<ProductTranslation> ProductTranslations { get; set; }  
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }    
        public DbSet<ProductInCategory> ProductInCategories { get; set; }   

    }
}
