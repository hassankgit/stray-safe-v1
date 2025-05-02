using StraySafe.Data.Database;
using StraySafe.Data.Database.Models.Users;

namespace StraySafe.Logic.Admin;
public class AdminClient
{
    private readonly DataContext _context;
    public AdminClient(DataContext context)
    {
        _context = context;
    }

    public IEnumerable<User> GetAllUsers()
    {
        IEnumerable<User> users = _context.Users.ToList();
        return users;
    }
}
