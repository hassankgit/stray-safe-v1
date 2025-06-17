using Integration.Supabase.Models.Auth.Users;

namespace Integration.Supabase.Interfaces;

public interface ISupabaseAdminService
{
    Task<List<User>> GetAllUsersAsync();
}
