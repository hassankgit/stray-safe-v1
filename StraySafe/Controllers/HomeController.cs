using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StraySafe.Models;
using StraySafe.Services.ImageLogic;
using StraySafe.Services.Admin;
using StraySafe.Services.Admin.Models;

namespace StraySafe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ImageMetadataClient _imageMetadataClient;
        private readonly UserClient _userClient;

        public HomeController(ILogger<HomeController> logger, ImageMetadataClient imageMetadataClient, UserClient userClient)
        {
            _logger = logger;
            _imageMetadataClient = imageMetadataClient;
            _userClient = userClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Submit")]
        public IActionResult SubmitForm(Submission submission)
        {
            Coordinates coords = _imageMetadataClient.GetCoordinates(submission.Image);
            return Ok("index");
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginRequest request)
        {
            bool isLoggedIn = _userClient.Login(request);
            if (isLoggedIn)
            {
                return Ok(isLoggedIn);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
