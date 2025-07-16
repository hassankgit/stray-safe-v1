using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Supabase.Models.Auth;

[ExcludeFromCodeCoverage]
public class RegisterRequest
{
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
}
