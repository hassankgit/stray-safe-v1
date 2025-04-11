using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StraySafe.Services.Users;

namespace StraySafe.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserClient _userClient;

    public UserController(UserClient userClient)
    {
        _userClient = userClient;
    }

    [HttpGet("MyName")]
    public IActionResult MyName()
    {
        return Ok(User.Identity?.Name);
    }
}
