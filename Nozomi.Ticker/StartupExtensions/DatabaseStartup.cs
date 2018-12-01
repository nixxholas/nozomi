using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Repo.Data;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class DatabaseStartup
    {
        public static void UseAutoDbMigration(this IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<NozomiDbContext>())
                {
                    // Auto Wipe
                    if (env.IsDevelopment())
                    {
                        context.Database.EnsureDeleted();
                    }
                    
                    context.Database.EnsureCreated();
                }
            }
        }
    }
}