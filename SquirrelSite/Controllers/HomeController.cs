using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SquirrelSite.Models;
using SquirrelSite.Services.ImageLogic;

namespace SquirrelSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ImageMetadata _imageMetadata;

        public HomeController(ILogger<HomeController> logger, ImageMetadata imageMetadata)
        {
            _logger = logger;
            _imageMetadata = imageMetadata;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Submit()
        {
            return View();
        }

        public IActionResult SubmitForm(Submission submission)
        {
            Coordinates coords = _imageMetadata.GetCoordinates(submission.Image);
            return RedirectToAction("index");
        }

        public IActionResult ViewSubmissions()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
