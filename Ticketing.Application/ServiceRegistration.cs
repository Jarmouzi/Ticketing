using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Application.Interfaces;
using Ticketing.Application.Services;
using Ticketing.Application.Services.Auth;
using Ticketing.Domain.ValueObjects;
using Ticketing.Infrastructure.interfaces;
using Ticketing.Infrastructure.Persistence;
using Ticketing.Infrastructure.Repositories;
using Ticketing.Repository;

namespace Ticketing.Application
{
    public static class ServiceRegistration
    {

        public static IServiceCollection AddAppServices(this IServiceCollection services, string? connectionStringConfigName)
        {
            services.AddDbContext<TicketingDBContext>(options =>
            {
                options.UseSqlite(connectionStringConfigName);
            });

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IJWTService, JwtService>();

            return services;
        }

        public static async Task<IApplicationBuilder> SeedDataAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {

                var cntx = scope.ServiceProvider.GetRequiredService<TicketingDBContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<IAuthService>();
                await cntx.Database.EnsureDeletedAsync();

                if (await cntx.Database.EnsureCreatedAsync())
                {
                    await userManager.RegisterAsync("First Admin", "admin@example.com", "string", UserRole.Admin);
                    await userManager.RegisterAsync("Second Admin", "admin2@example.com", "string", UserRole.Admin);
                    await userManager.RegisterAsync("First Employee", "employee@example.com", "string", UserRole.Employee);
                    await userManager.RegisterAsync("Second Employee", "employee2@example.com", "string", UserRole.Employee);
                }
            }
            return app;
        }
    }
}
