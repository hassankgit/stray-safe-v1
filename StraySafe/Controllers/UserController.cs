using Microsoft.AspNetCore.Mvc;
using StraySafe.Services.Admin;
using StraySafe.Services.Admin.Models;

namespace StraySafe.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserClient _userClient;

        public UserController(UserClient userClient)
        {
            _userClient = userClient;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest request)
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
