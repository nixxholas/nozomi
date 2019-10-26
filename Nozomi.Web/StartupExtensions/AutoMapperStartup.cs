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

            // https://stackoverflow.com/questions/40275195/how-to-set-up-automapper-in-asp-net-core
//            var mapperConfiguration = AutoMapperConfig.RegisterMappings();
//            var mapper = mapperConfiguration.CreateMapper();
//            services.AddSingleton(mapper);

            //services.AddAutoMapper(typeof(Startup));
            services.AddAutoMapper(typeof(DomainToViewModelMappingProfile), typeof(ViewModelToDomainMappingProfile));
        }
    }
}
