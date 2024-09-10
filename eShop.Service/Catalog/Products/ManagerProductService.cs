using eShop.API.EF;
using eShop.Data.Entities;
using eShop.Service.Catalog.Products.DTOs;
using eShop.Service.Catalog.Products.DTOs.Manage;
using eShop.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Service.Catalog.Products
{
    public class ManagerProductService : IManageProductService
    {
        private readonly eShopDBContext _context;
        public ManagerProductService(eShopDBContext context)
        {
            _context = context;
        }

        public async Task AddViewCount(int productId, int viewCount)
        {
           var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateDTOs createProduct)
        {
            var product = new Product()
            {
                Price = createProduct.Price,
                OriginalPrice = createProduct.OriginalPrice,
                Stock = createProduct.Stock,
                ViewCount =0,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name = createProduct.Name,
                        Description = createProduct.Description,
                        Details = createProduct.Details,
                        SeoAlias = createProduct.SeoAlias,
                        SeoDescription = createProduct.SeoDescription,
                        SeoTitle = createProduct.SeoTitle,
                        LanguageId = createProduct.LanguageId,
                    }
                }
            };
            _context.Products.Add(product);
            return await _context.SaveChangesAsync();

        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new Exception();
            _context.Products.Remove(product);
           return await _context.SaveChangesAsync();
        }

        public Task<List<ProductViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPaggingDTOs getProductPagging)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(UpdateProductDTOs updateProduct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            throw new NotImplementedException();
        }
    }
}
