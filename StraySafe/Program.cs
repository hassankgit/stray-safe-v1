using StraySafe.Services.ImageLogic;
using StraySafe.Nucleus.Database;
using StraySafe.Services.Users;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using StraySafe.Nucleus.Database.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using StraySafe.Services.Middleware;

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
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            // initialize db
            builder.Services.AddScoped<DataContext>();
            builder.Services.AddScoped<JwtService>();
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

            // configure Identity
            builder.Services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
            })
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();


            // configuring JWT & Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                ConfigurationManager? config = builder.Configuration;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
                };
            });

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(config.GetConnectionString("dev")));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StraySafe API v1");
                });
            }

            if (app.Environment.IsProduction())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}
