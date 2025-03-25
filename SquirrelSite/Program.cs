using StraySafe.Services.ImageLogic;
using StraySafe.Nucleus.Database;
using StraySafe.Nucleus.Database.Models.Users;

namespace StraySafe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            DataContext context = new DataContext();

            User? user = context.Users.FirstOrDefault(x => x.Id == 1);
            if (user != null)
            {
                Console.WriteLine($"Username: {user.Username}, Password: {user.Password}");
            }
            else
            {
                throw new Exception("db not working womp womp");
            }

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ImageMetadata>();
            builder.Services.AddScoped<DataContext>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}
