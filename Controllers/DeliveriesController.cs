using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AdminPanel.ModelsDb;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DeliveriesController : Controller
    {
        private readonly WarehouseContext context;

        public DeliveriesController(WarehouseContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("admin/deliveries")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Delivery> deliveries = await SelectAllDeliveries();
            return View("Index", deliveries);
        }

        [HttpGet]
        [Route("admin/deliveries/search")]
        public IActionResult SearchDelivery(string deliveryName)
        {
            if (deliveryName is null)
            {
                return BackToMenu();
            }

            IEnumerable<Delivery> foundDeliveries = SearchDeliveriesByName(deliveryName);
            return View("Index", foundDeliveries);
        }

        [HttpPost]
        [Route("admin/deliveries/back")]
        public IActionResult BackToMenu()
        {
            return RedirectPermanent("/admin/deliveries");
        }

        [HttpGet]
        [Route("admin/deliveries/create")]
        public IActionResult CreateDelivery()
        {
            return View("Create");
        }

        [HttpPost]
        [Route("admin/deliveries/create")]
        public async Task<IActionResult> CreateDelivery(Delivery delivery)
        {
            await context.Deliveries.AddAsync(delivery);
            await context.SaveChangesAsync();
            await context.DisposeAsync();

            return BackToMenu();
        }

        [HttpGet]
        [Route("admin/deliveries/edit/{id}")]
        public async Task<IActionResult> EditDelivery(int id)
        {
            var delivery = await SearchSingleDeliveryById(id);

            if (delivery is null)
            {
                return BadRequest("Delivery is not found.");
            }

            return View("Edit", delivery);
        }

        [HttpPost]
        [Route("admin/deliveries/edit")]
        public IActionResult SaveDelivery(Delivery delivery)
        {
            context.Update(delivery);
            context.SaveChanges();
            context.Dispose();

            return BackToMenu();
        }

        [HttpGet]
        [Route("admin/deliveries/delete/{id}")]
        public async Task<IActionResult> DeleteDelivery(int id)
        {
            var delivery = await SearchSingleDeliveryById(id);

            if (delivery is null)
            {
                return BadRequest("Delivery is not found.");
            }

            return View("Delete", delivery);
        }

        [HttpPost]
        [Route("admin/deliveries/delete")]
        public IActionResult RemoveDelivery(Delivery delivery)
        {
            context.Entry(delivery).State = EntityState.Deleted;
            context.SaveChanges();
            context.Dispose();

            return BackToMenu();
        }

        public async Task<Delivery?> SearchSingleDeliveryById(int id)
        {
            return await context.Deliveries.Where(m => m.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Delivery>> SelectAllDeliveries()
        {
            return await context.Deliveries.ToListAsync();
        }

        public IEnumerable<Delivery> SearchDeliveriesByName(string deliveryName)
        {
            deliveryName = deliveryName.ToLower();
            return context.Deliveries.Where(d => d.Name.ToLower().Contains(deliveryName));
        }
    }
}
