using Integration.Supabase;
using Integration.Supabase.Interfaces;
using Integration.Supabase.Models.Auth.Users;

internal class SupabaseAdminService : ISupabaseAdminService
{
    private readonly SupabaseService _supabaseService;

    public SupabaseAdminService(SupabaseService supabaseService)
    {
        _supabaseService = supabaseService;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        var response = await _supabaseService.SendGetAsAdmin<SupabaseUserResponse>("auth/v1/admin/users");
        return response?.Users ?? [];
    }
}
