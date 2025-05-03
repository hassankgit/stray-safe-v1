namespace Integration.Supabase.Interfaces;

public interface ISupabaseService
{
    Task<T> SendRequestToAdminEndpoint<T>(string endpoint);
    Task<T> SendRequestToUserEndpoint<T>(string endpoint);
    // TODO: Remove these, add them to their respective repos
    Task<List<User>> GetAllUsersAsync();
    Task<User> GetCurrentUser();
}
