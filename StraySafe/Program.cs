using StraySafe.Services.ImageLogic;
using StraySafe.Nucleus.Database;
using StraySafe.Services.Admin;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using StraySafe.Nucleus.Database.Models.Users;
using Microsoft.EntityFrameworkCore;
using StraySafe.Services.Users;

namespace StraySafe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            //temporary to let frontend thru
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // initialize db
            builder.Services.AddScoped<DataContext>();

            // clients
            // TODO: separate into different function to unclutter this?
            builder.Services.AddScoped<ImageMetadataClient>();
            builder.Services.AddScoped<AdminClient>();
            builder.Services.AddScoped<UserClient>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "StraySafe API",
                    Version = "v1",
                    Description = "First iteration of StraySafe API",
                    Contact = new OpenApiContact
                    {
                        Name = "Test Contact",
                        Email = "testing@test.com"
                    }
                });
            });

            // Configure authentication
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);
            builder.Services.AddIdentityCore<User>()
                .AddEntityFrameworkStores<DataContext>()
                .AddApiEndpoints();
            //builder.Services.AddIdentityCore

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(config.GetConnectionString("dev")));

            var app = builder.Build();

            // TODO: configure development environments
            if (app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StraySafe API v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // CORS must be configured after routing (idk why)
            app.UseCors();

            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}
