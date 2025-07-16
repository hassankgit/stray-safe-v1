using System.Diagnostics.CodeAnalysis;

namespace Integration.Supabase.Models.Auth.Users;

[ExcludeFromCodeCoverage]
public class AppMetadata
{
    public string? Provider { get; set; }
    public List<string>? Providers { get; set; }
}
