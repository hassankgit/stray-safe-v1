using StraySafe.Data.Database;
using StraySafe.Logic.ImageLogic;
using StraySafe.Logic.Users;
using StraySafe.Logic.Admin;
using StraySafe.Logic.Middleware;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Supabase;

namespace StraySafe.Api;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

        IConfigurationRoot? config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

        builder.Services.AddSingleton<IConfiguration>(config);

        builder.Services.AddControllersWithViews();

        ConfigureCors(builder);
        ConfigureDatabase(builder, config);
        ConfigureAuthentication(builder, config);
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
                policy.WithOrigins("http://localhost:3000", "https://www.straysafe.net")
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
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Authentication:JwtSecret"]!)),
                ValidateIssuer = false,
                ValidateAudience = true,
                ValidAudience = builder.Configuration["Authentication:ValidAudience"],
                ValidIssuer = builder.Configuration["Authentication:ValidIssuer"],
            };
        });
    }

    private static void ConfigureClients(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ImageMetadataClient>();
        builder.Services.AddScoped<AdminClient>();
        builder.Services.AddScoped<UserClient>();

        // configure supabase client!
        string url = builder.Configuration["Supabase:Url"]!;
        string key = builder.Configuration["Supabase:ServiceRoleKey"]!;
        SupabaseOptions options = new()
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        };
        builder.Services.AddSingleton(provider => new Supabase.Client(url, key, options));
    }

    private static void ConfigureSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "straysafe API",
                Version = "v1",
                Description = "First iteration of StraySafe API",
                Contact = new OpenApiContact
                {
                    Name = "Test Contact",
                    Email = "testing@test.com"
                }
            });
            OpenApiSecurityScheme jwtSecurityScheme = new()
            {
                Name = "JWT Authorization",
                Description = "Enter JWT here",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
            };

            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, jwtSecurityScheme);

            OpenApiSecurityRequirement securityRequirement = new()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    []
                }
            };

            c.AddSecurityRequirement(securityRequirement);
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "straysafe API v1");
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