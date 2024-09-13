using eShop.Data.Entities;
using eShop.Service.Catalog.Products;
using eShop.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            if(product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("GetById{productId}")]
        public async Task<IActionResult> GetById(int productId,string languageId)
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
            var productId = await _manageProductService.Create(createProduct);
            if (productId == 0) return BadRequest();
            var product = await _manageProductService.GetById(productId, createProduct.LanguageId);
            return Created(nameof(GetById), product);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromForm]UpdateProductDTOs updateProduct)
        {
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
        [HttpPut("UpdatePrice/{id}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var isSuccessful = await _manageProductService.UpdatePrice(productId, newPrice);
            if (isSuccessful) return Ok();
            return BadRequest();
        }
    }
}
