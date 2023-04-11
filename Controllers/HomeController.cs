using AdminPanel.Models;
using AdminPanel.ModelsDb;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AdminPanel.Controllers
{
    public class HomeController : Controller
    {
        // TODO: Is logger needed?
        private readonly ILogger<HomeController> logger;
        private readonly WarehouseContext context;

        public HomeController(ILogger<HomeController> logger, WarehouseContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string login, string password, string returnUrl)
        {
            Authorization authorization = new Authorization(context);

            if (authorization.IsAuthorized(login, password))
            {
                List<Claim> claims = new List<Claim>() 
                {
                    new Claim(ClaimTypes.Name, authorization.GetUserName(login)),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Login");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return Redirect(returnUrl ?? "/admin");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("index");
        }

        // TODO: Is privacy needed?
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}