using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
