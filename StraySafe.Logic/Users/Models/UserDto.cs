using System.Diagnostics.CodeAnalysis;

namespace StraySafe.Logic.Users.Models;

[ExcludeFromCodeCoverage]
public class UserDto
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public string? Username { get; set; }
    public string? Role { get; set; }
    public string? Phone { get; set; }
}


