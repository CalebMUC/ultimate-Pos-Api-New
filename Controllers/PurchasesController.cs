using Microsoft.AspNetCore.Mvc;

namespace Ultimate_POS_Api.Controllers
{
    public class PurchasesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
