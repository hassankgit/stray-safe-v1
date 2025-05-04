using System.Text.Json.Serialization;
using Integration.Supabase.Models.Auth.Users;

public class User
{
    // TODO : create UserDTO in StraySafe.Logic, map to it
    public required string Id { get; set; }
    public required string Email { get; set; }
    public string? Aud { get; set; }
    public string? Role { get; set; }
    public string? Phone { get; set; }

    [JsonPropertyName("email_confirmed_at")]
    public DateTime? EmailConfirmedAt { get; set; }

    [JsonPropertyName("confirmed_at")]
    public DateTime? ConfirmedAt { get; set; }

    [JsonPropertyName("last_sign_in_at")]
    public DateTime? LastSignInAt { get; set; }

    [JsonPropertyName("app_metadata")]
    public AppMetadata? AppMetadata { get; set; }

    [JsonPropertyName("user_metadata")]
    public UserMetadata? UserMetadata { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("is_anonymous")]
    public bool IsAnonymous { get; set; }
}
