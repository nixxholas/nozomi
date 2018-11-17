using Microsoft.Extensions.DependencyInjection;
using Nozomi.Ticker.Areas;
using Swashbuckle.AspNetCore.Swagger;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class SwaggerStartup
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc(GlobalApiVariables.CURRENT_API_VERSION, new Info
                 {
                     Version = GlobalApiVariables.CURRENT_API_VERSION,
                     Title = "Nozomi API v1",
                     Description = "Reference documentation for the usage of Nozomi.",
                     TermsOfService = "None",
                     Contact = new Contact
                     {
                         Name = "Nicholas Chen",
                         Email = "nicholas@counter.network",
                         Url = "https://twitter.com/nixxholas"
                     },
                     License = new License
                     {
                         Name = "Copyright (C) Hayate Inc. - All Rights Reserved",
                         Url = ""
                     }
                 });
                 
                 swaggerGenOptions.EnableAnnotations();
             });
         }
     }
 }