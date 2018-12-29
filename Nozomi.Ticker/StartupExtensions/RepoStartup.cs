using Microsoft.Extensions.DependencyInjection;
using Nozomi.Repo.BCL.Context;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Repo.Identity.Data;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class RepoStartup
    {
        public static void ConfigureRepoLayer(this IServiceCollection services)
        {
            // Database
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddTransient<IUnitOfWork<NozomiAuthContext>, UnitOfWork<NozomiAuthContext>>();
            services.AddTransient<IDbContext, NozomiAuthContext>();
            services.AddTransient<IUnitOfWork<NozomiDbContext>, UnitOfWork<NozomiDbContext>>();
            services.AddTransient<IDbContext, NozomiDbContext>();
        }
    }
}