using StraySafe.Nucleus.Database;
using StraySafe.Nucleus.Database.Models.Users;

namespace StraySafe.Services.Users;
public class AdminClient
{
    private readonly DataContext _context;
    public AdminClient(DataContext context)
    {
        _context = context;
    }

    public List<User> GetAllUsers()
    {
        List<User> users = _context.Users.ToList();
        return users;
    }
}
