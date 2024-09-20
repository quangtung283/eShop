using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace eShop.API.Controllers
{
    [Area("API")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return Ok();
        }
    }
}
