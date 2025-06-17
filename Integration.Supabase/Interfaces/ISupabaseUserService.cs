using Integration.Supabase.Models.Auth;
using Integration.Supabase.Models.Auth.Users;
using Microsoft.AspNetCore.Http;

namespace Integration.Supabase.Interfaces;

public interface ISupabaseUserService
{
    Task<User> GetCurrentUserAsync();
    Task<TokenResponse> Login(LoginRequest request);
    Task<TokenResponse> Register(RegisterRequest request);
    Task<string> UploadImage(IFormFile file);
}
