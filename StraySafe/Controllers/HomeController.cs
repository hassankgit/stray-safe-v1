using Microsoft.AspNetCore.Mvc;
using StraySafe.Services.ImageLogic;
using StraySafe.Services.Users;

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
    }
}
