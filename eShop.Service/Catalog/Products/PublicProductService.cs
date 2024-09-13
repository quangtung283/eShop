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

        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request)
        {
            // using join and projections 
            /*//1.Select join
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
        }*/
            // Sử dụng Navigation Properties (Thuộc tính dẫn hướng)
            // 1. Truy vấn sản phẩm với thông tin bản dịch và danh mục
            var query = _context.Products
                        .Include(p => p.ProductTranslations) // Lấy thông tin bản dịch
                        .Include(p => p.ProductInCategories) // Lấy thông tin danh mục của sản phẩm
                            .ThenInclude(pic => pic.Category) // Lấy thêm thông tin của danh mục
                        .Where(p => p.ProductTranslations.Any(pt => pt.LanguageId == request.LanguageId)); // Lọc theo ngôn ngữ

            // 2. Lọc theo CategoryId nếu có
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.ProductInCategories.Any(pic => pic.CategoyId == request.CategoryId));
            }

            // 3. Phân trang
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                                  .Take(request.PageSize)
                                  .Select(p => new ProductViewModel()
                                  {
                                      Id = p.Id,
                                      Name = p.ProductTranslations.FirstOrDefault(pt => pt.LanguageId == request.LanguageId).Name,
                                      DateCreated = p.DateCreated,
                                      Description = p.ProductTranslations.FirstOrDefault(pt => pt.LanguageId == request.LanguageId).Description,
                                      Details = p.ProductTranslations.FirstOrDefault(pt => pt.LanguageId == request.LanguageId).Details,
                                      LanguageId = request.LanguageId,
                                      OriginalPrice = p.OriginalPrice,
                                      Price = p.Price,
                                      SeoAlias = p.ProductTranslations.FirstOrDefault(pt => pt.LanguageId == request.LanguageId).SeoAlias,
                                      SeoDescription = p.ProductTranslations.FirstOrDefault(pt => pt.LanguageId == request.LanguageId).SeoDescription,
                                      SeoTitle = p.ProductTranslations.FirstOrDefault(pt => pt.LanguageId == request.LanguageId).SeoTitle,
                                  }).ToListAsync();

            // 4. Trả về kết quả
            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pageResult;
            //Sử dụng LINQ với Anonymous Types
            // Lấy danh sách sản phẩm và thông tin liên quan
         /*   var query = _context.Products
                        .Where(p => p.ProductTranslations.Any(pt => pt.LanguageId == request.LanguageId))
                        .Select(p => new
                        {
                            p.Id,
                            p.DateCreated,
                            p.OriginalPrice,
                            p.Price,
                            Translation = p.ProductTranslations.FirstOrDefault(pt => pt.LanguageId == request.LanguageId),
                            Categories = p.ProductInCategories
                        });

            // Lọc theo CategoryId nếu có
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.Categories.Any(c => c.CategoyId == request.CategoryId));
            }

            // Phân trang và chuyển đổi sang ProductViewModel
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                                  .Take(request.PageSize)
                                  .Select(x => new ProductViewModel()
                                  {
                                      Id = x.Id,
                                      Name = x.Translation.Name,
                                      DateCreated = x.DateCreated,
                                      Description = x.Translation.Description,
                                      Details = x.Translation.Details,
                                      LanguageId = x.Translation.LanguageId,
                                      OriginalPrice = x.OriginalPrice,
                                      Price = x.Price,
                                      SeoAlias = x.Translation.SeoAlias,
                                      SeoDescription = x.Translation.SeoDescription,
                                      SeoTitle = x.Translation.SeoTitle,
                                  }).ToListAsync();

            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pageResult;*/
        }
    }
}
