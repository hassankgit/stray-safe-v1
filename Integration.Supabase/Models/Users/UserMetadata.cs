using Newtonsoft.Json;

namespace Integration.Supabase.Models.Users;

public class UserMetadata
{
    [JsonProperty("email_verified")]
    public bool EmailVerified { get; set; }
}
