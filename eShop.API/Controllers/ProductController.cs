using eShop.Service.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;
        public ProductController(IPublicProductService publicProductService) 
        { 
            _publicProductService = publicProductService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var product = await _publicProductService.GetAll();
            return Ok();
        }
    }
}
