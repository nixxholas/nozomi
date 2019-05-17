using Microsoft.Extensions.DependencyInjection;
using Nozomi.Repo.BCL.Context;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Repo.Identity.Data;

namespace Nozomi.Analysis.StartupExtensions
{
    public static class StartupRepos
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