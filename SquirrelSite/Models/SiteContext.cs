using Microsoft.EntityFrameworkCore;

namespace SquirrelSite.Models
{
    public class SiteContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public SiteContext(DbContextOptions options) : base(options)
        {

        }
    }
}
