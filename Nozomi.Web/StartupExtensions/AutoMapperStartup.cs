using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Service.AutoMapper;

namespace Nozomi.Web.StartupExtensions
{
    public static class AutoMapperStartup
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(typeof(Startup));

            // Registering Mappings automatically only works if the
            // Automapper Profile classes are in ASP.NET project
            AutoMapperConfig.RegisterMappings();
        }
    }
}
