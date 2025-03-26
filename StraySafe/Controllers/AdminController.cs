using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StraySafe.Nucleus.Database.Models.Users;
using StraySafe.Services.Users;

namespace StraySafe.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminClient _adminClient;

        public AdminController(AdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        [HttpGet("AllUsers")]
        public IActionResult GetAllUsers()
        {
            List<User> users = _adminClient.GetAllUsers();
            return Ok(users);
        }
    }
}
