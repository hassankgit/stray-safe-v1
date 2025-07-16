using System.Diagnostics.CodeAnalysis;

namespace StraySafe.Logic.Users.Models;

[ExcludeFromCodeCoverage]
public class TokenDto
{
    // TODO : Add refresh token later
    public required string Token { get; set; }
}
