namespace Integration.Supabase.Models.Auth.Users;

public class SupabaseUserResponse
{
    public List<User>? Users { get; set; }

    public string? Aud { get; set; }
}
