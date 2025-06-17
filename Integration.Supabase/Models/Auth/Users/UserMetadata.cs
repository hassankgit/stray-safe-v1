using System.Text.Json.Serialization;

namespace Integration.Supabase.Models.Auth.Users;

public class UserMetadata
{
    [JsonPropertyName("email_verified")]
    public bool EmailVerified { get; set; }
}
