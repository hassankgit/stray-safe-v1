using Microsoft.AspNetCore.Mvc;
using StraySafe.Services.Admin.Models;
using StraySafe.Services.Users;

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
            //  FLOW:
            //      get login request
            //      check if user is valid
            //      if user valid, return a LoginResponse (user information, bearer token)
            //      if user is invalid, throw 401
            bool isLoggedIn = _userClient.Login(request);
            return Ok(isLoggedIn);
        }
    }
}
