using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebJaguarPortal.Repository;
using WebJaguarPortal.Repository.Interfaces;
using WebJaguarPortal.Services;

namespace WebJaguarPortal.Infrastructure
{
    internal static class DependencyInjectionRegistry
    {
        internal static void AddAuthentications(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        string jwtSigningKey = String.Empty;

                        using (var scope = builder.Services?.BuildServiceProvider().CreateScope())
                        {
                            JaguarDbContext? dbContext = scope?.ServiceProvider.GetService<JaguarDbContext>();

                            if (dbContext != null)
                                jwtSigningKey = dbContext.Settings.First().JWTSigningKey;
                        }

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSigningKey))
                        };
                    });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                   .AddCookie(options =>
                   {
                       options.LoginPath = "/Login";
                       options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                       options.SlidingExpiration = true;
                   });

        }

        internal static void AddDependencies(this IServiceCollection services, WebApplicationBuilder builder)
        {
            #region Database
            services.AddDbContext<JaguarDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies());
            services.AddScoped<DbContext, JaguarDbContext>();
            #endregion

            #region AutoMapper            
            services.AddAutoMapper(typeof(MappingProfile));
            #endregion

            #region Services
            services.AddScoped<TokenService>();
            services.AddScoped<UserService>();
            services.AddScoped<SettingsService>();
            services.AddScoped<ProjectService>();
            services.AddScoped<EmailService>();            
            services.AddScoped<AnalysisService>();
            #endregion

            #region Repositories
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped<UserRepository>();
            services.AddScoped<FileRepository>();
            services.AddScoped<RenewPasswordRepository>();
            services.AddScoped<ProjectRepository>();
            #endregion
        }

        internal static void UseInitialSettings(this WebApplication app)
        {
            // Enable the use of DbContext in your controllers or services
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<JaguarDbContext>();
            // Perform any database migrations or other initialization
            dbContext.Database.Migrate();

            var userService = scope.ServiceProvider.GetRequiredService<UserService>();
            var settingsService = scope.ServiceProvider.GetRequiredService<SettingsService>();
            userService.AddInitialUser();
            settingsService.AddInitialSettings();
        }
    }
}
