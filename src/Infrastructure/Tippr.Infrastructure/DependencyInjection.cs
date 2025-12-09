using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tippr.Infrastructure.Data;
using Tippr.Infrastructure.Identity;

namespace Tippr.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            // ===================================================
            //                      Database
            // ===================================================
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            // ===================================================
            //                      Identity
            // ===================================================
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                // Password configuration
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            })
            .AddRoles<IdentityRole>() // Lägg till stöd för roller
            .AddEntityFrameworkStores<ApplicationDbContext>() // Koppla till EF Core
            .AddDefaultTokenProviders();

            services.AddDataProtection();

            return services;
        }
    }
}
