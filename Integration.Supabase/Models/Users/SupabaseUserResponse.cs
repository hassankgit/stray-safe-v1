using Newtonsoft.Json;

namespace Integration.Supabase.Models.Users;

public class SupabaseUserResponse
{
    public List<User>? Users { get; set; }

    public string? Aud { get; set; }
}
