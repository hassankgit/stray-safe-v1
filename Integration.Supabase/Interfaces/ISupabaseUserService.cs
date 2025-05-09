using Integration.Supabase.Models.Auth;

namespace Integration.Supabase.Interfaces;

public interface ISupabaseUserService
{
    Task<User> GetCurrentUserAsync();
    Task<TokenResponse> Login(LoginRequest request);
    Task<TokenResponse> Register(RegisterRequest request);
}
