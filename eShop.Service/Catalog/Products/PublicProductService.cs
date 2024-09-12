using eShop.API.EF;
using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Common.DTOs;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace eShop.Service.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {
        private readonly eShopDBContext _context;

        public PublicProductService(eShopDBContext context)
        {
            _context = context;
        }
        // using EF Core 
        public async Task<List<ProductViewModel>> GetAll(string languageId)
        {
            var products = await _context.Products
         .Include(p => p.ProductTranslations)
         .Include(p => p.ProductInCategories)
             .ThenInclude(pic => pic.Category)
         .Select(p => new ProductViewModel
         {
             Id = p.Id,
             Name = p.ProductTranslations.FirstOrDefault().Name,
             DateCreated = p.DateCreated,
             Description = p.ProductTranslations.FirstOrDefault().Description,
             Details = p.ProductTranslations.FirstOrDefault().Details,
             LanguageId = p.ProductTranslations.FirstOrDefault().LanguageId,
             OriginalPrice = p.OriginalPrice,
             Price = p.Price,
             SeoAlias = p.ProductTranslations.FirstOrDefault().SeoAlias,
             SeoDescription = p.ProductTranslations.FirstOrDefault().SeoDescription,
             SeoTitle = p.ProductTranslations.FirstOrDefault().SeoTitle,            
         })
         .ToListAsync();

            return products;
        }
        // using join and projections 
        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request)
        {
            //1.Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoyId equals c.Id
                        where pt.LanguageId == request.LanguageId
                        select new { p, pt, pic };
            //2.Filter
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.pic.CategoyId == request.CategoryId);
            }
            //3.Paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                }).ToListAsync();
            //4.Select 
            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pageResult;
        }
    }
}
