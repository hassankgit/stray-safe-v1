using System.ComponentModel.DataAnnotations;

namespace Integration.Supabase.Models.Auth;

public class RegisterRequest
{
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
}
