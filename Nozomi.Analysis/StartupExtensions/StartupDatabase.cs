using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Repo.Data;

namespace Nozomi.Analysis.StartupExtensions
{
    public static class StartupDatabase
    {
        public static void UseAutoDbMigration(this IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                var logger = serviceScope.ServiceProvider.GetService<ILogger<Startup>>();

                using (var context = serviceScope.ServiceProvider.GetService<NozomiDbContext>())
                {
                    context.Database.EnsureCreated();
                }
            }
        }
    }
}