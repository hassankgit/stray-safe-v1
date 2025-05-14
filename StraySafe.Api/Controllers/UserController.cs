using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StraySafe.Logic.Users;
using StraySafe.Logic.Users.Models;

namespace StraySafe.Api.Controllers;

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

    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [HttpGet("Me")]
    public async Task<IActionResult> MyName()
    {
        UserDto user = await _userClient.GetCurrentUser();
        return Ok(user);
    }
}
