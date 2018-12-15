using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Base.Identity.Models;
using Nozomi.Repo.Identity.Stores;
using Nozomi.Service.Identity;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class IdentityServerStartup
    {
        public static void ConfigureIdentityServer(this IServiceCollection services)
        {
            services.AddTransient<IClientStore, ClientStore>();
            services.AddTransient<IResourceStore, ResourceStore>();
            
            services.AddIdentityServer()
                //.AddSigningCredential(cert)
                .AddResourceStore<ResourceStore>()
                .AddClientStore<ClientStore>()
                .AddAspNetIdentity<User>()
                .AddProfileService<NozomiProfileService>();
        }
    }
}