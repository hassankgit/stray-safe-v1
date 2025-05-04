using Integration.Supabase;
using Integration.Supabase.Interfaces;
using StraySafe.Data.Database;

namespace StraySafe.Logic.Admin;
public class AdminClient
{
    private readonly ISupabaseService _supabaseService;
    public AdminClient(ISupabaseService supabaseService)
    {
        _supabaseService = supabaseService;
    }

    public async Task<List<User>> GetAllUsers()
    {
        List<User> users = await _supabaseService.Admin.GetAllUsersAsync();
        return users;
    }
}
