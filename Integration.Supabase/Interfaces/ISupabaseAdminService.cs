namespace Integration.Supabase.Interfaces;

public interface ISupabaseAdminService
{
    Task<List<User>> GetAllUsersAsync();
}
