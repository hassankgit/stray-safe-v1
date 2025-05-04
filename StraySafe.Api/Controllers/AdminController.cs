using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StraySafe.Logic.Admin;

namespace StraySafe.Api.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly AdminClient _adminClient;

    public AdminController(AdminClient adminClient)
    {
        _adminClient = adminClient;
    }

    [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
    [HttpGet("Users/All")]
    public async Task<IActionResult> GetAllUsers()
    {
        List<User> users = await _adminClient.GetAllUsers();
        return Ok(users);
    }
}
