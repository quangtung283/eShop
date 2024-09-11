using eShop.API.EF;
using eShop.Data.Entities;
using eShop.Utilities.Exceptions;
using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Catalog.Products.Manage;
using eShop.ViewModels.Common.DTOs;
using eShop.ViewModelsViewModels.Catalog.ProductManage;
using Microsoft.EntityFrameworkCore;


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
            if (product == null) throw new eShopException("Can not find product");
            _context.Products.Remove(product);
           return await _context.SaveChangesAsync();
        }

       /* public Task<List<ProductViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }*/

        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPaggingDTOs getProductPagging)
        {
            //1.Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoyId equals c.Id
                        select new { p, pt,pic };
            //2.Filter
            if (!string.IsNullOrEmpty(getProductPagging.KeyWord))
                query = query.Where(x => x.pt.Name.Contains(getProductPagging.KeyWord));

            if(getProductPagging.CategoryId.Count > 0)
            {
                query = query.Where(p=>getProductPagging.CategoryId.Contains(p.pic.CategoyId));
            }
            //3.Paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((getProductPagging.PageIndex - 1) * getProductPagging.PageSize)
                .Take(getProductPagging.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id=x.p.Id,
                    Name=x.pt.Name,
                    DateCreated=x.p.DateCreated,
                    Description=x.pt.Description,
                    Details=x.pt.Details,
                    LanguageId=x.pt.LanguageId,
                    OriginalPrice=x.p.OriginalPrice,
                    Price=x.p.Price,
                    SeoAlias=x.pt.SeoAlias,
                    SeoDescription=x.pt.SeoDescription,
                    SeoTitle=x.pt.SeoTitle,
                }).ToListAsync();
            //4.Select 
            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items= data
            };
            return pageResult;
        }

        public async Task<int> Update(UpdateProductDTOs updateProduct)
        {
            var product = _context.Products.Find(updateProduct.Id);
            var productTranslations = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == updateProduct.Id
            && x.LanguageId== updateProduct.LanguageId);
            if (product == null || productTranslations==null) throw new eShopException("Can not find id");
            productTranslations.Name = updateProduct.Name;
            productTranslations.SeoAlias = updateProduct.SeoAlias;
            productTranslations.SeoDescription = updateProduct.SeoDescription;
            productTranslations.SeoTitle = updateProduct.SeoTitle;  
            productTranslations.Description = updateProduct.SeoDescription;
            productTranslations.Details = updateProduct.Details;
           return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = _context.Products.Find(productId);
            if (product == null) throw new eShopException("Can not find id");
            product.Price = newPrice;
            return await _context.SaveChangesAsync()>0;
        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = _context.Products.Find(productId);
            if (product == null) throw new eShopException("Can not find id");
            product.Stock += addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
