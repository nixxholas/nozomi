using Microsoft.Extensions.DependencyInjection;
using Nozomi.Repo.Data;
using Nozomi.Repo.Repositories;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class RepoStartup
    {
        public static void ConfigureRepoLayer(this IServiceCollection services)
        {
            // Database
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddTransient<IUnitOfWork<NozomiDbContext>, UnitOfWork<NozomiDbContext>>();
            services.AddTransient<IDbContext, NozomiDbContext>();
        }
    }
}