using eShop.Data.Entities;
using eShop.Service.Catalog.Products;
using eShop.ViewModels.Catalog.ProductImages;
using eShop.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace eShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IManageProductService _manageProductService;
        public ProductController(IPublicProductService publicProductService, IManageProductService manageProductService)
        {
            _manageProductService = manageProductService;
            _publicProductService = publicProductService;
        }
        [HttpGet("GetAll{languageId}")]
        public async Task<IActionResult> GetAll(string languageId)
        {
            var product = await _publicProductService.GetAll(languageId);
            return Ok(product);
        }
        [HttpGet("GetAllByCategoryId{languageId}")]
        public async Task<IActionResult> GetAllByCategoryId(GetPublicProductPagingRequest request)
        {
            var product = await _publicProductService.GetAllByCategoryId(request);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("GetById{productId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _manageProductService.GetById(productId, languageId);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] ProductCreateDTOs createProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = await _manageProductService.Create(createProduct);
            if (productId == 0) return BadRequest();
            var product = await _manageProductService.GetById(productId, createProduct.LanguageId);
            return Created(nameof(GetById), product);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateProductDTOs updateProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _manageProductService.Update(updateProduct);
            if (result == 0) return NotFound();
            return Ok(result);
        }
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var result = await _manageProductService.Delete(productId);
            if (result == 0) return NotFound();
            return Ok();
        }
        [HttpPatch("UpdatePrice/{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var isSuccessful = await _manageProductService.UpdatePrice(productId, newPrice);
            if (isSuccessful) return Ok();
            return BadRequest();
        }
        // hoan thien not productcontroller o manager va public
        [HttpPost("{productId}/images")]
        public async Task<IActionResult> AddImages(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageId = await _manageProductService.AddImage(productId, request);
            if (imageId == 0) { return NotFound(); }
            var image = await _manageProductService.GetImageById(imageId);

            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }
        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateImages(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _manageProductService.UpdateImage(imageId, request);
            if (result == 0) { return NotFound(); }
            return Ok();
        }
        [HttpGet("{imageId}/images/{productId}")]
        public async Task<IActionResult> GetImageById(int imageId, int productId)
        {
            var image = _manageProductService.GetImageById(productId);
            if (image == null) return NotFound();
            return Ok(image);
        }
        [HttpDelete("{imageId}/images/{productId}")]
        public async Task<IActionResult> RemoveImages(int imageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _manageProductService.RemoveImage(imageId);
            if (result == 0) { return NotFound(); }
            return Ok();
        }
    }
}
