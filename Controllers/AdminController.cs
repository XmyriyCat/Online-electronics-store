using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AdminPanel.ModelsDb;
using System.Security.Claims;

namespace AdminPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly WarehouseContext context;

        public AdminController(WarehouseContext context)
        {
            this.context = context;
        }

        [Route("admin")]
        public IActionResult Index()
        {
            SetStatistic();
            return View();
        }

        public void SetStatistic()
        {
            ViewData["ProductsCount"] = context.Products.Count();
        }
    }
}
