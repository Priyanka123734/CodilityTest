using Microsoft.AspNetCore.Mvc;

namespace CodilityTest.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

