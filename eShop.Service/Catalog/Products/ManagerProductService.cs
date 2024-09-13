﻿using eShop.API.EF;
using eShop.Data.Entities;
using eShop.Service.Common;
using eShop.Utilities.Exceptions;
using eShop.ViewModels.Catalog.ProductImages;
using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Common.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;


namespace eShop.Service.Catalog.Products
{
    public class ManagerProductService : IManageProductService
    {
        private readonly eShopDBContext _context;
        private readonly IStorageService _storageService;

        public ManagerProductService(eShopDBContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest imageCreateRequest)
        {
            var productImage = new ProductImage()
            {
                Caption = imageCreateRequest.Caption,
                DateCreate = DateTime.Now,
                IsDefault = imageCreateRequest.IsDefault,
                ProductId = productId,
                SortOrder = imageCreateRequest.SortOrder,

            };

            if (imageCreateRequest.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(imageCreateRequest.ImageFile);
                productImage.FileSize = imageCreateRequest.ImageFile.Length;
            };
            _context.ProductImages.Add(productImage);
             await _context.SaveChangesAsync();
            return productImage.Id;
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
                ViewCount = 0,
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
            //Save file 
            if (createProduct.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption= "Thumbnail image",
                        DateCreate = DateTime.Now,
                        FileSize = createProduct.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(createProduct.ThumbnailImage),
                        IsDefault =true,
                        SortOrder=1
                    }
                };
            }
            _context.Products.Add(product);
            return product.Id;
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new eShopException("Can not find product");

            var images = _context.ProductImages.Where(x => x.ProductId == productId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);

            }
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
                        select new { p, pt, pic };
            //2.Filter
            if (!string.IsNullOrEmpty(getProductPagging.KeyWord))
                query = query.Where(x => x.pt.Name.Contains(getProductPagging.KeyWord));

            if (getProductPagging.CategoryId.Count > 0)
            {
                query = query.Where(p => getProductPagging.CategoryId.Contains(p.pic.CategoyId));
            }
            //3.Paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((getProductPagging.PageIndex - 1) * getProductPagging.PageSize)
                .Take(getProductPagging.PageSize)
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

        public async Task<ProductViewModel> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            var producTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId
            && x.LanguageId == languageId);
            if (product == null || producTranslation == null) throw new eShopException("can not find product");
            var productViewModel = new ProductViewModel()
            {
                Id = productId,
                DateCreated = product.DateCreated,
                Description = producTranslation != null ? producTranslation.Description : null,
                LanguageId = producTranslation.LanguageId,
                Details = producTranslation != null ? producTranslation.Details : null,
                Name = producTranslation != null ? producTranslation.Name : null,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                SeoAlias = producTranslation != null ? producTranslation.SeoAlias : null,
                SeoDescription = producTranslation != null ? producTranslation.SeoDescription : null,
                SeoTitle = producTranslation != null ? producTranslation.SeoTitle : null,
                Stock = product.Stock,
                ViewCount = product.ViewCount
            };
            return productViewModel;
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null) { throw new eShopException("Cannot find image"); }
            var result = new ProductImageViewModel() 
            { 
                Id = image.Id,
                Caption = image.Caption,
                DateCreate= image.DateCreate,
                FileSize = image.FileSize,
                ImagePath = image.ImagePath,
                IsDefault = image.IsDefault,
                ProductId = image.ProductId,
                SortOrder = image.SortOrder
            };
            return result;
        }

        public async Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId)
                .Select(i => new ProductImageViewModel()
                {
                    Caption = i.Caption,
                    DateCreate = i.DateCreate,
                    FileSize = i.FileSize,
                    Id = i.Id,
                    ImagePath = i.ImagePath,
                    IsDefault = i.IsDefault,
                    ProductId = i.ProductId,
                    SortOrder = i.SortOrder
                }).ToListAsync();
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null) throw new eShopException("Cannot find image");
            _context.ProductImages.Remove(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(UpdateProductDTOs updateProduct)
        {
            var product = _context.Products.Find(updateProduct.Id);
            var productTranslations = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == updateProduct.Id
            && x.LanguageId == updateProduct.LanguageId);
            if (product == null || productTranslations == null) throw new eShopException("Can not find id");
            productTranslations.Name = updateProduct.Name;
            productTranslations.SeoAlias = updateProduct.SeoAlias;
            productTranslations.SeoDescription = updateProduct.SeoDescription;
            productTranslations.SeoTitle = updateProduct.SeoTitle;
            productTranslations.Description = updateProduct.SeoDescription;
            productTranslations.Details = updateProduct.Details;
            //Save file 
            if (updateProduct.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(x => x.IsDefault == true && x.ProductId == updateProduct.Id);
                if (thumbnailImage != null)
                {


                    thumbnailImage.FileSize = updateProduct.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(updateProduct.ThumbnailImage);
                    _context.ProductImages.Update(thumbnailImage);
                }
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateImage( int imageId, ProductImageUpdateRequest request)
        {
            var productInmage = await _context.ProductImages.FindAsync(imageId);
            if (productInmage == null) throw new eShopException("Can not find id ");

            if (request.ImageFile != null)
            {
                productInmage.ImagePath = await this.SaveFile(request.ImageFile);
                productInmage.FileSize = request.ImageFile.Length;
            };
            _context.ProductImages.Update(productInmage);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = _context.Products.Find(productId);
            if (product == null) throw new eShopException("Can not find id");
            product.Price = newPrice;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = _context.Products.Find(productId);
            if (product == null) throw new eShopException("Can not find id");
            product.Stock += addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }
        private async Task<string> SaveFile(IFormFile fromFile)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(fromFile.ContentDisposition).FileName;
            var fileName = $"{Guid.NewGuid()}.{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(fromFile.OpenReadStream(), fileName);
            return fileName;
        }
    }
}
