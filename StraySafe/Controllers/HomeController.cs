using Microsoft.AspNetCore.Mvc;

namespace StraySafe.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
