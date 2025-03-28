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
        User? user = _context.Users.FirstOrDefault(x => x.Username == request.Username);
        if (user == null)
        {
            throw new Exception($"User with username {request.Username} not found.");
        }

        if (user.Password == request.Password)
        {
            return true;
        }

        return false;
    }
}
