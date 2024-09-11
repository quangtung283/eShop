﻿using eShop.API.EF;
using eShop.Data.Entities;
using eShop.Service.Common;
using eShop.Utilities.Exceptions;
using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.Catalog.Products.Manage;
using eShop.ViewModels.Common.DTOs;
using eShop.ViewModelsViewModels.Catalog.ProductManage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<int> AddImages(int productId, List<IFormFile> files)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new eShopException("Can not find Id");
            }
            foreach (var file in files)
            {
                var image = new ProductImage()
                {
                    Caption = file.FileName,
                    DateCreate = DateTime.Now,
                    FileSize = file.Length,
                    ImagePath = await this.SaveFile(file),
                    IsDefault = false,
                    SortOrder = 1,
                    ProductId = productId

                };
                _context.ProductImages.Add(image);
            }
            return await _context.SaveChangesAsync();
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
            return await _context.SaveChangesAsync();
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

        public async Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
            var images = await _context.ProductImages
        .Where(x => x.ProductId == productId)
        .Select(x => new ProductImageViewModel()
        {
            Id = x.Id,           
            FileSize = x.FileSize,
            IsDefault = x.IsDefault
        }).ToListAsync();

            return images;
        }

        public async Task<int> RemoveImages(int productId)
        {

            var images = await _context.ProductImages.Where(x => x.ProductId == productId).ToListAsync();
            if (images == null || images.Count == 0) throw new eShopException("No images found");

            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
                _context.ProductImages.Remove(image);
            }

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

        public async Task<int> UpdateImage(int imageId, string caption, bool isDefault)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null) throw new eShopException("Image not found");

            image.Caption = caption;
            image.IsDefault = isDefault;

            if (isDefault)
            {
                // Unset the default flag from other images of the same product
                var otherImages = await _context.ProductImages.Where(x => x.ProductId == image.ProductId && x.Id != imageId).ToListAsync();
                foreach (var otherImage in otherImages)
                {
                    otherImage.IsDefault = false;
                }
            }

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
