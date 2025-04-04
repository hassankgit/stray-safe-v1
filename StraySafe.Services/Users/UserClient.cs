using System.Net;
using System.Web.Http;
using StraySafe.Nucleus.Database;
using StraySafe.Nucleus.Database.Models.Users;
using StraySafe.Services.Admin.Models;

namespace StraySafe.Services.Admin;
public class UserClient
{
    private readonly DataContext _context;
    public UserClient(DataContext context)
    {
        _context = context;
    }

    public bool Login(LoginRequest request)
    {
        // TODO: Modify method to return a LoginResponse with a bearer token string
        // On frontend, store bearer in local storage
        // append it to the header of every request
        // create a middleware that checks the bearer for validity
        // if invalid, throw 401 exception
        // if frontend receives 401, logout and return to home
        User? user = _context.Users.FirstOrDefault(x => x.Username == request.Username);
        if (user == null)
        {
            // TODO: create object that passes in message and throws 401 with message
            HttpResponseMessage msg = new(HttpStatusCode.Unauthorized)
            {
                ReasonPhrase = $"User with username '{request.Username}' is not found"
            };
            throw new HttpResponseException(msg);
        }

        if (user.Password == request.Password)
        {
            return true;
        }

        return false;
    }
}
