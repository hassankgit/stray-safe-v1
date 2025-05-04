using System.Text.Json.Serialization;

namespace Integration.Supabase.Models.Auth;

public class TokenResponse
{
    // TODO: add refresh token param after configuring refresh tokens
    [JsonPropertyName("access_token")]
    public required string Token { get; set; }
    public User? User { get; set; }
}
