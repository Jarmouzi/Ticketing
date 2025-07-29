using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ticketing.DataContext;
using Ticketing.Model;
using Ticketing.Repository;

namespace Ticketing.API.Service
{
    public static class ServiceRegistration
    {

        public static IServiceCollection AddAppServices(this IServiceCollection services, string? connectionStringConfigName)
        {
            services.AddDbContext<TicketingDBContext>(options =>
            {
                options.UseSqlite(connectionStringConfigName);
            });

            services.AddScoped<Ticketing.DataContext.IUnitOfWork, Ticketing.DataContext.UnitOfWork>();

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();

            return services;
        }

        public static async Task<IApplicationBuilder> SeedDataAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {

                var cntx = scope.ServiceProvider.GetRequiredService<TicketingDBContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<IAuthRepository>();
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
