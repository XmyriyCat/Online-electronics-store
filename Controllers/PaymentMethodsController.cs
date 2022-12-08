using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AdminPanel.ModelsDb;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PaymentMethodsController : Controller
    {
        private readonly WarehouseContext context;

        public PaymentMethodsController(WarehouseContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("admin/paymentMethods")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<PaymentWay> paymentsMethods = await SelectAllPaymentsMethods();

            return View("index", paymentsMethods);
        }

        [HttpGet]
        [Route("admin/paymentMethods/search")]
        public IActionResult SearchPaymentMethod(string paymentName)
        {
            if (paymentName is null)
            {
                return BackToMenu();
            }

            IEnumerable<PaymentWay> searchedPaymentMethods = SearchPaymentMethodsByName(paymentName);

            return View("Index", searchedPaymentMethods);
        }

        [HttpPost]
        [Route("admin/paymentMethods/back")]
        public IActionResult BackToMenu()
        {
            return RedirectPermanent("/admin/paymentMethods");
        }

        [HttpGet]
        [Route("admin/paymentMethods/create")]
        public IActionResult CreatePaymentMethod()
        {
            return View("Create");
        }

        [HttpPost]
        [Route("admin/paymentMethods/create")]
        public async Task<IActionResult> CreatePaymentMethod(PaymentWay paymentWay)
        {
            await context.PaymentWays.AddAsync(paymentWay);
            context.SaveChanges();
            context.Dispose();
            return BackToMenu();
        }

        [HttpGet]
        [Route("admin/paymentMethods/edit/{id}")]
        public async Task<IActionResult> EditPaymentMethod(int id)
        {
            PaymentWay? paymentWay = await SearchSinglePaymentMethodById(id);

            if (paymentWay is null)
            {
                return BadRequest("Payment way is NOT found.");
            }

            return View("Edit", paymentWay);
        }

        [HttpPost]
        [Route("admin/paymentMethods/edit")]
        public IActionResult SavePaymentMethod(PaymentWay paymentWay)
        {
            context.Update(paymentWay);
            context.SaveChanges();
            context.Dispose();

            return BackToMenu();
        }

        [HttpGet]
        [Route("admin/paymentMethods/delete/{id}")]
        public async Task<IActionResult> DeletePaymentMethod(int id)
        {
            PaymentWay? paymentWay = await SearchSinglePaymentMethodById(id);

            if (paymentWay is null)
            {
                return BadRequest("Payment way is NOT found.");
            }

            return View("Delete", paymentWay);
        }

        [HttpPost]
        [Route("admin/paymentMethods/delete")]
        public IActionResult RemovePaymentMethod(PaymentWay paymentWay)
        {
            context.Entry(paymentWay).State = EntityState.Deleted;
            context.SaveChanges();
            context.Dispose();

            return BackToMenu();
        }

        public IEnumerable<PaymentWay> SearchPaymentMethodsByName(string paymentName)
        {
            paymentName = paymentName.ToLower();

            return context.PaymentWays.Where(m => m.Name.ToLower().Contains(paymentName));
        }

        public async Task<IEnumerable<PaymentWay>> SelectAllPaymentsMethods()
        {
            return await context.PaymentWays.ToListAsync();
        }

        public async Task<PaymentWay?> SearchSinglePaymentMethodById(int id)
        {
            return await context.PaymentWays.Where(m => m.Id == id).FirstOrDefaultAsync();
        }
    }
}
