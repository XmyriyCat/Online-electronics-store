using AdminPanel.ModelsDb;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManufacturersController : Controller
    {
        private readonly WarehouseContext context;

        public ManufacturersController(WarehouseContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("admin/manufacturers")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Manufacturer> manufacturers = await SelectAllManufacturers();
            return View("Index", manufacturers);
        }

        [HttpGet]
        [Route("admin/manufacturers/create")]
        public IActionResult CreateManufacturer()
        {
            return View("Create");
        }

        [HttpPost]
        [Route("admin/manufacturers/create")]
        public async Task<IActionResult> CreateManufacturer(Manufacturer manufacturer)
        {
            await context.Manufacturers.AddAsync(manufacturer);
            context.SaveChanges();
            context.Dispose();
            return BackToMenu();
        }

        [HttpGet]
        [Route("admin/manufacturers/search")]
        public IActionResult SearchManufacturers(string manufacturerName)
        {
            if (manufacturerName is null) 
            {
                return BackToMenu();
            }

            IEnumerable<Manufacturer> searchedManufacturers = SearchManufacturersByName(manufacturerName);
            return View("Index", searchedManufacturers);
        }

        [HttpPost]
        [Route("admin/manufacturers/back")]
        public IActionResult BackToMenu()
        {
            return RedirectPermanent("/admin/manufacturers");
        }

        [HttpGet]
        [Route("admin/manufacturers/edit/{id}")]
        public async Task<IActionResult> EditManufacturer(int id)
        {
            Manufacturer manufacturer = await SearchSingleManufacturerById(id);
            return View("Edit", manufacturer);
        }

        [HttpPost]
        [Route("admin/manufacturers/edit")]
        public IActionResult SaveManufacturer(Manufacturer manufacturer)
        {
            context.Update(manufacturer);
            context.SaveChanges();
            context.Dispose();

            return BackToMenu();
        }

        [HttpGet]
        [Route("admin/manufacturers/delete/{id}")]
        public async Task<IActionResult> DeleteManufacturer(int id)
        {
            Manufacturer manufacturer = await SearchSingleManufacturerById(id);
            return View("Delete", manufacturer);
        }

        [HttpPost]
        [Route("admin/manufacturers/delete")]
        public IActionResult RemoveManufacturer(Manufacturer manufacturer)
        {
            context.Entry(manufacturer).State = EntityState.Deleted;
            context.SaveChanges();
            context.Dispose();

            return BackToMenu();
        }

        public async Task<Manufacturer> SearchSingleManufacturerById(int id)
        {
            return await context.Manufacturers.Where(m => m.Id == id).SingleAsync();
        }

        public IEnumerable<Manufacturer> SearchManufacturersByName(string name)
        {
            name = name.ToLower();
            return context.Manufacturers.Where(m => m.Name.ToLower().Contains(name));
        }

        public async Task<IEnumerable<Manufacturer>> SelectAllManufacturers()
        {
            return await context.Manufacturers.ToListAsync();
        }
    }
}
