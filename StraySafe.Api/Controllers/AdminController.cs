using Microsoft.AspNetCore.Mvc;
using StraySafe.Logic.Admin;

namespace StraySafe.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly AdminClient _adminClient;

    public AdminController(AdminClient adminClient)
    {
        _adminClient = adminClient;
    }

    //[HttpGet("AllUsers")]
    //public IActionResult GetAllUsers()
    //{
    //    IEnumerable<User> users = _adminClient.GetAllUsers();
    //    return Ok(users);
    //}
}
