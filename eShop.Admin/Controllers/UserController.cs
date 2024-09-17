using eShop.Admin.Services;
using eShop.ViewModels.System.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eShop.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        public UserController(IUserApiClient userApiClient ,IConfiguration configuration) 
        { 
            _userApiClient = userApiClient;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
           
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
           await  HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if(!ModelState.IsValid) return View(ModelState);

            var token = await _userApiClient.Authenticate(request);
            var usePrincipal = this.ValidateToken(token);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = true
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                usePrincipal,
                authProperties);

           return RedirectToAction("Index","Home");
        }
        private ClaimsPrincipal ValidateToken(string jwtToken) 
        {
            IdentityModelEventSource.ShowPII = true;
            SecurityToken validatedToken;
            TokenValidationParameters validationsParameters = new TokenValidationParameters();
            validationsParameters.ValidateLifetime = true;
            validationsParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationsParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationsParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token : Key"]));
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationsParameters, out validatedToken);
            return principal;
        }
    }
}
