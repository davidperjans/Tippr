using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Common.Interfaces.Services;
using Tippr.Infrastructure.Auth;
using Tippr.Infrastructure.Data;
using Tippr.Infrastructure.Identity;
using Tippr.Infrastructure.Repositories;
using Tippr.Infrastructure.Services;

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
            //                      Repositories
            // ===================================================
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IPredictionGroupRepository, PredictionGroupRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ===================================================
            //                      Services
            // ===================================================
            services.AddScoped<IAuthService, AuthService>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IUserReadService, UserReadService>();


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
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddDefaultTokenProviders();

            services.AddDataProtection();

            // ===================================================
            //                 JWT Configuration
            // ===================================================
            services.Configure<JwtSettings>(config.GetSection("JwtSettings"));

            var jwtSettings = config.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            services.AddAuthentication(options =>
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

                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),

                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }
    }
}
