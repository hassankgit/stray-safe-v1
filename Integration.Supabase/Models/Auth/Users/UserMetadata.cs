using Newtonsoft.Json;

namespace Integration.Supabase.Models.Auth.Users;

public class UserMetadata
{
    [JsonProperty("email_verified")]
    public bool EmailVerified { get; set; }
}
