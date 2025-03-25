using Microsoft.EntityFrameworkCore;
using StraySafe.Nucleus.Database.Models.Users;

namespace StraySafe.Nucleus.Database;

public class DataContext : DbContext
{
    public DataContext()
    {
        this.Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source=C:\Users\Hassan\source\db\testdb.db");
    }

    public DbSet<User> Users { get; set; }
}
