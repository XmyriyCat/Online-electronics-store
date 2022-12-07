using AdminPanel.ModelsDb;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly WarehouseContext context;

        public ProductsController(WarehouseContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("admin/products")]
        public async Task<IActionResult> Index()
        {
            //ViewData["lol"] = "You lol ahahahha!";
            IEnumerable<Product> products = await SelectJoinAllProducts();
            return View(products);
        }

        [HttpGet]
        [Route("admin/products/search")]
        public IActionResult SearchProducts(string productModel)
        {
            if (productModel is null)
            {
                return RedirectPermanent("/admin/products");
            }
            IEnumerable<Product> products = SearchProductsByModel(productModel);
            return View("Index", products);
        }

        [HttpGet]
        [Route("admin/products/create")]
        public IActionResult CreateProduct()
        {
            return View("Create");
        }

        [HttpGet]
        [Route("admin/products/edit/{id}")]
        public async Task<IActionResult> EditProduct(int id)
        {
            Product product = await SearchSingleProductById(id);
            return View("Edit", product);
        }

        [HttpGet]
        [Route("admin/products/delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product product = await SearchSingleProductById(id);
            return View("Delete", product);
        }

        [HttpPost]
        [Route("admin/products/save")]
        public IActionResult SaveChanges(Product product)
        {
            context.Products.Update(product);
            context.SaveChanges();
            context.Dispose();
            return BackToMenu();
        }

        [HttpPost]
        [Route("admin/products/create")]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            await context.Products.AddAsync(product);
            context.SaveChanges();
            context.Dispose();
            return BackToMenu();
        }

        [HttpPost]
        [Route("admin/products/back")]
        public IActionResult BackToMenu()
        {
            return RedirectPermanent("/admin/products");
        }

        [HttpPost]
        [Route("admin/products/remove")]
        public IActionResult RemoveProduct(Product product)
        {
            context.Entry(product).State = EntityState.Deleted;
            context.SaveChanges();
            context.Dispose();

            return BackToMenu();
        }

        public IEnumerable<Product> SearchProductsByModel(string model)
        {
            model = model.Trim().ToLower();
            return SelectJoinAllProducts().Result.Where(p => p.Model.ToLower().Contains(model));
        }

        public async Task<Product> SearchSingleProductById(int id)
        {
            var searchedProduct = await context.Products.OrderBy(p => p.Id)
                .Include(p => p.IdManufacturerNavigation)
                .Include(p => p.IdCategoryNavigation)
                .Where(p => p.Id == id)
                .SingleAsync();

            return searchedProduct;
        }

        public async Task<IEnumerable<Product>> SelectJoinAllProducts()
        {
            var products = await context.Products.OrderBy(p => p.Id)
                .Include(p => p.IdManufacturerNavigation)
                .Include(p => p.IdCategoryNavigation)
                .ToListAsync();

            return products;
        }
    }
}
