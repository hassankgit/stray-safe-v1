using StraySafe.Services.ImageLogic;
using StraySafe.Nucleus.Database;
using StraySafe.Services.Users;
using Microsoft.OpenApi.Models;

namespace StraySafe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

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

            var app = builder.Build();

            // TODO: configure development environments
            //if (!app.Environment.IsDevelopment())
            //{
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StraySafe API v1");
                });
            //}

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
