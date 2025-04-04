using Microsoft.AspNetCore.Identity;

namespace StraySafe.Nucleus.Database.Models.Users;

public class User : IdentityUser
{
    // not showing up in db because there's no get; set;, safe to delete without migration
    public int NumberOfSubmissions = 0;
}
