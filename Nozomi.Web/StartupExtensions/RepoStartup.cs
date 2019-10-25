using Microsoft.Extensions.DependencyInjection;
using Nozomi.Data.Interfaces;
using Nozomi.Repo.BCL;
using Nozomi.Repo.Data;

namespace Nozomi.Web.StartupExtensions
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
