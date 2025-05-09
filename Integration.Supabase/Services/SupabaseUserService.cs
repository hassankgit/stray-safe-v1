using Integration.Supabase;
using Integration.Supabase.Interfaces;
using Integration.Supabase.Models.Auth;

internal class SupabaseUserService : ISupabaseUserService
{
    private readonly SupabaseService _supabaseService;

    public SupabaseUserService(SupabaseService supabaseService)
    {
        _supabaseService = supabaseService;
    }

    public async Task<User> GetCurrentUserAsync()
    {
        return await _supabaseService.SendGetAsUser<User>("auth/v1/user")
            ?? throw new Exception("Failed to get current user");
    }

    public async Task<TokenResponse> Login(LoginRequest request)
    {
        try
        {
            return await _supabaseService.SendPostAsUser<TokenResponse>("auth/v1/token?grant_type=password", request);
        }
        catch
        {
            return await _supabaseService.SendPostAsUser<TokenResponse>("auth/v1/token?grant_type=password", new LoginRequest()
            {
                Email = request.Username,
                Password = request.Password,
            });
        }
    }

    public async Task<TokenResponse> Register(RegisterRequest request)
    {
        return await _supabaseService.SendPostAsUser<TokenResponse>("auth/v1/signup", request);
    }

}
