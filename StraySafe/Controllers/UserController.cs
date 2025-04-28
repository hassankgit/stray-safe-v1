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
    public async Task<IActionResult> MyName()
    {
        // TODO : Delete, only for testing auth
        string name = await _userClient.GetNameById(User.Identity?.Name);
        return Ok(name);
    }
}
