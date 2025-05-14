namespace StraySafe.Logic.Users.Models;

public class UserDto
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public string? Username { get; set; }
    public string? Role { get; set; } // TODO: Role enums?
    public string? Phone { get; set; }
}


