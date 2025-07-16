using System.Diagnostics.CodeAnalysis;

namespace Integration.Supabase.Models.Auth;

[ExcludeFromCodeCoverage]
public class LoginRequest
{
    public string? Username { get; set; }
    public string? Email { get; set;  }
    public string? Password { get; set; }
}
