using Microsoft.Extensions.DependencyInjection;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.Currency;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencySource;
using Nozomi.Preprocessing.Swagger.Examples.Responses.Generic;
using Nozomi.Ticker.Areas;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class SwaggerStartup
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            // https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/issues/56
            services.AddSwaggerExamplesFromAssemblyOf<CreateCurrencyExample>();
            services.AddSwaggerExamplesFromAssemblyOf<CreateSourceExample>();
            services.AddSwaggerExamplesFromAssemblyOf<NozomiJsonResultExample>();
            services.AddSwaggerExamplesFromAssemblyOf<NozomiStringResultExample>();
            
            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc(GlobalApiVariables.CURRENT_API_VERSION, new Info
                 {
                     Version = GlobalApiVariables.CURRENT_API_VERSION,
                     Title = "Nozomi API",
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
                    
                swaggerGenOptions.OperationFilter<AppendAuthorizeToSummaryOperationFilter>(); // Adds "(Auth)" to the summary so that you can see which endpoints have Authorization
                swaggerGenOptions.OperationFilter<AddFileParamTypesOperationFilter>(); // Adds an Upload button to endpoints which have [AddSwaggerFileUploadButton]
                swaggerGenOptions.OperationFilter<AddResponseHeadersFilter>(); // [SwaggerResponseHeader]
                 
                swaggerGenOptions.ExampleFilters();
                swaggerGenOptions.EnableAnnotations();
             });
         }
     }
 }