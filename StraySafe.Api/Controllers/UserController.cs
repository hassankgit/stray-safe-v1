using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StraySafe.Logic.Users;

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

    [HttpGet("MyEmail")]
    public IActionResult MyName()
    {
        // TODO : Delete, only for testing auth
        string email = _userClient.GetEmail();
        return Ok(email);
    }
}
