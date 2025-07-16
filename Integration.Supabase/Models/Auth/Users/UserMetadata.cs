using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Integration.Supabase.Models.Auth.Users;

[ExcludeFromCodeCoverage]
public class UserMetadata
{
    [JsonPropertyName("email_verified")]
    public bool EmailVerified { get; set; }
}
