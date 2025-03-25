using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StraySafe.Models;
using StraySafe.Services.ImageLogic;

namespace StraySafe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ImageMetadata _imageMetadata;
        //private readonly DataContext _context;

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

        //public IActionResult Login(User user)
        //{
            //LoginResponse response = _userClient.Login(User user);
        //}

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
