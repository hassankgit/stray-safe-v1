using Microsoft.AspNetCore.Mvc;

namespace StraySafe.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
