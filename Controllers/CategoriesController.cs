using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AdminPanel.ModelsDb;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly WarehouseContext context;

        public CategoriesController(WarehouseContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("admin/categories")]
        public IActionResult Index()
        {
            IEnumerable<Category> categories = SelectAllCategories();
            return View("Index", categories);
        }

        [HttpGet]
        [Route("admin/categories/search")]
        public IActionResult SearchCategories(string categoryName)
        {
            if (categoryName is null)
            {
                return BackToMenu();
            }

            IEnumerable<Category> searchedCategoreis = SelectCategoryByName(categoryName);
            return View("Index", searchedCategoreis);
        }

        [HttpGet]
        [Route("admin/categories/edit/{id}")]
        public async Task<IActionResult> EditCategory(int id)
        {
            Category category = await SearchSingleCategoryById(id);
            return View("Edit", category);
        }

        [HttpPost]
        [Route("admin/categories/save")]
        public IActionResult SaveCategory(Category category)
        {
            context.Categories.Update(category);
            context.SaveChanges();
            context.Dispose();

            return BackToMenu();
        }

        [HttpGet]
        [Route("admin/categories/delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            Category category = await SearchSingleCategoryById(id);
            return View("Delete", category);
        }

        [HttpPost]
        [Route("admin/categories/delete")]
        public IActionResult RemoveCategory(Category category)
        {
            context.Entry(category).State = EntityState.Deleted;
            context.SaveChanges();
            context.Dispose();

            return BackToMenu();
        }

        [HttpGet]
        [Route("admin/categories/create")]
        public IActionResult CreateCategory()
        {
            return View("Create");
        }

        [HttpPost]
        [Route("admin/categories/create")]
        public IActionResult CreateCategory(Category category)
        {
            context.Add(category);
            context.SaveChanges();
            context.Dispose();

            return BackToMenu();
        }

        [HttpPost]
        [Route("admin/categories/back")]
        public IActionResult BackToMenu()
        {
            return RedirectPermanent("/admin/categories");
        }

        public async Task<Category> SearchSingleCategoryById(int id)
        {
            return await context.Categories.Where(c => c.Id == id).SingleAsync();
        }

        public IEnumerable<Category> SelectAllCategories()
        {
            return context.Categories;
        }

        public IEnumerable<Category> SelectCategoryByName(string name)
        {
            name = name.Trim().ToLower();
            return SelectAllCategories().Where(c => c.Name.ToLower().Contains(name));
        }
    }
}
