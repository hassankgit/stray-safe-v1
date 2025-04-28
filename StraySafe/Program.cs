using StraySafe.Nucleus.Database;
using StraySafe.Nucleus.Database.Models.Users;
using StraySafe.Services.ImageLogic;
using StraySafe.Services.Users;
using StraySafe.Services.Admin;
using StraySafe.Services.Middleware;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace StraySafe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

            IConfigurationRoot? config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build();

            builder.Services.AddSingleton<IConfiguration>(config);

            builder.Services.AddControllersWithViews();

            ConfigureCors(builder);
            ConfigureDatabase(builder, config);
            ConfigureAuthentication(builder, config);
            ConfigureIdentity(builder);
            ConfigureClients(builder);
            ConfigureSwagger(builder);

            WebApplication? app = builder.Build();

            ConfigureMiddleware(app);

            app.Run();
        }

        private static void ConfigureCors(WebApplicationBuilder builder)
        {
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
        }

        private static void ConfigureDatabase(WebApplicationBuilder builder, IConfiguration config)
        {
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(config.GetConnectionString("straySafe")));
        }

        private static void ConfigureAuthentication(WebApplicationBuilder builder, IConfiguration config)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
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
        }

        private static void ConfigureIdentity(WebApplicationBuilder builder)
        {
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
        }

        private static void ConfigureClients(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<DataContext>();
            builder.Services.AddScoped<JwtService>();
            builder.Services.AddScoped<ImageMetadataClient>();
            builder.Services.AddScoped<AdminClient>();
            builder.Services.AddScoped<UserClient>();
        }

        private static void ConfigureSwagger(WebApplicationBuilder builder)
        {
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
                OpenApiSecurityScheme jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "JWT Authorization header using the Bearer scheme.",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });
        }

        private static void ConfigureMiddleware(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StraySafe API v1");
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
        }
    }
}