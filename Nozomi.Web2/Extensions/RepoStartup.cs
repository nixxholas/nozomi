using Microsoft.Extensions.DependencyInjection;
using Nozomi.Repo.Data;

namespace Nozomi.Web2.Extensions
{
    public static class RepoStartup
    {
        public static void ConfigureRepoLayer(this IServiceCollection services)
        {
            // Database
            // services.AddTransient<NozomiDbContext, UnitOfWork<NozomiDbContext>>();
            // services.AddTransient<IDbContext, NozomiDbContext>();
        }
    }
}
