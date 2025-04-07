using StraySafe.Nucleus.Database;
using StraySafe.Services.Admin.Models;

namespace StraySafe.Services.Users;
public class UserClient
{
    private readonly DataContext _context;
    public UserClient(DataContext context)
    {
        _context = context;
    }

    public bool Login(LoginRequest request)
    {
        // TODO: integrate with authentication

        return false;
    }
}
