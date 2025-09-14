using Microsoft.AspNetCore.Mvc;

namespace Ultimate_POS_Api.Controllers
{
    public class FinanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
