using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tippr.Infrastructure.Data;

namespace Tippr.API.Tests.Common
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var inMemorySettings = new Dictionary<string, string>
                {
                    {"JwtSettings:SecretKey", "DettaArEnSuperHemligNyckelSomMasteVaraMinst32TeckenLang123!"},
                    {"JwtSettings:Issuer", "TipprTest"},
                    {"JwtSettings:Audience", "TipprTestClient"},
                    {"JwtSettings:AccessTokenExpirationMinutes", "15"},
                    {"JwtSettings:RefreshTokenExpirationDays", "7"}
                };

                config.AddInMemoryCollection(inMemorySettings!);
            });

            builder.ConfigureServices(services =>
            {
                // 1. Ta bort den riktiga databas-konfigurationen (SQL Server)
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // 2. Lägg till InMemory-databas istället
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // 3. Seeda databasen om det behövs (t.ex. skapa roller eller admin)
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();
            });
        }
    }
}
