using Integration.Supabase.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StraySafe.Logic.Authentication.Models;
using StraySafe.Logic.Users.Models;
using SupabaseReason = Supabase.Gotrue.Exceptions.FailureHint.Reason;

namespace StraySafe.Logic.Users;
public class UserClient
{
    private readonly Supabase.Client _supabase;
    private readonly ISupabaseService _supabaseService;
    private readonly IConfiguration _config;

    public UserClient(Supabase.Client supabase, IConfiguration config, ISupabaseService supabaseService)
    {
        _supabase = supabase;
        _supabaseService = supabaseService;
        _config = config;
    }

    public string GetEmail()
    {
        Supabase.Gotrue.User user = _supabase.Auth.CurrentUser ?? throw new InvalidOperationException("cannot find current user");
        return user.Email ?? throw new InvalidOperationException($"no email found for user with id {user.Id}");
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _supabaseService.GetAllUsersAsync();
    }

    public async Task<User> GetCurrentUser()
    {
        return await _supabaseService.GetCurrentUser();
    }

    public async Task<TokenResponse> Login(LoginRequest request)
    {
        try
        {
            if (request.Username == null || request.Password == null)
            {
                throw new ArgumentException("username, email, and password are required");
            }

            // TODO : make a supabase service that handles the try catch with these methods
            // so that not every method has a try catch
            Supabase.Gotrue.Session? result = await _supabase.Auth.SignIn(request.Username, request.Password);

            if (result?.AccessToken != null)
            {
                return new TokenResponse()
                {
                    Token = result.AccessToken
                };
            }
        }
        catch (Supabase.Gotrue.Exceptions.GotrueException ex)
        {
            SupabaseError error = new SupabaseError();
            error = JsonConvert.DeserializeObject<SupabaseError>(ex.Content!) ??
                throw new JsonSerializationException("Failed to deserialize supabase error JSON");
            error.Reason = ex.Reason;
            string errorMessageToReturn = "";
            switch (error.Reason)
            {
                case SupabaseReason.UserBadEmailAddress:
                    errorMessageToReturn = "email address is invalid";
                    break;
                case SupabaseReason.UserBadPassword:
                    errorMessageToReturn = "password is invalid";
                    break;
                case SupabaseReason.UserBadLogin:
                    errorMessageToReturn = "email or password is wrong D:";
                    break;
            }
            if (!string.IsNullOrEmpty(errorMessageToReturn))
            {
                throw new InvalidOperationException(errorMessageToReturn);
            }

            throw new InvalidOperationException($"unhandled error: {ex.Message}, reason {ex.Reason}");
        }

        throw new InvalidOperationException("login failed for unknown reason");
    }

    public async Task<TokenResponse> Register(RegisterRequest request)
    {
        try
        {
            if (request.Email == null || request.Password == null)
            {
                throw new ArgumentException("please provide an email and password");
            }
            Supabase.Gotrue.Session? result = await _supabase.Auth.SignUp(request.Email, request.Password);
            if (result?.User != null && result?.AccessToken != null)
            {
                return new TokenResponse()
                {
                    Token = result.AccessToken
                };
            }
            throw new InvalidOperationException("registration failed for unknown reason");
        }
        catch (Supabase.Gotrue.Exceptions.GotrueException ex)
        {
            SupabaseError error = new SupabaseError();
            error = JsonConvert.DeserializeObject<SupabaseError>(ex.Content!) ??
                throw new JsonSerializationException("Failed to deserialize supabase error JSON");
            error.Reason = ex.Reason;
            string errorMessageToReturn = "";
            switch (error.Reason)
            {
                case SupabaseReason.UserBadEmailAddress:
                    errorMessageToReturn = "email address is invalid";
                    break;
                case SupabaseReason.UserBadPassword:
                    errorMessageToReturn = "password is invalid";
                    break;
                case SupabaseReason.UserBadLogin:
                    errorMessageToReturn = "email or password is wrong D:";
                    break;
            }
            if (!string.IsNullOrEmpty(errorMessageToReturn))
            {
                throw new InvalidOperationException(errorMessageToReturn);
            }

            throw new InvalidOperationException($"unhandled error: {ex.Message}, reason {ex.Reason}");
        }
    }
}
