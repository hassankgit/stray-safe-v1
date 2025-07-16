using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Integration.Supabase.Models.Auth.Users;

namespace Integration.Supabase.Models.Auth;

[ExcludeFromCodeCoverage]
public class TokenResponse
{
    // TODO: add refresh token param after configuring refresh tokens
    [JsonPropertyName("access_token")]
    public required string Token { get; set; }
    public User? User { get; set; }
}
