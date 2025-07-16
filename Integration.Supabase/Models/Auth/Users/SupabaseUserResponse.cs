using System.Diagnostics.CodeAnalysis;

namespace Integration.Supabase.Models.Auth.Users;

[ExcludeFromCodeCoverage]
public class SupabaseUserResponse
{
    public List<User>? Users { get; set; }

    public string? Aud { get; set; }
}
