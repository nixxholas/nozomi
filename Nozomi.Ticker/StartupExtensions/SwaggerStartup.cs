using System;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.Currency;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencyPair;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencyPairComponent;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencyPairRequest;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencySource;
using Nozomi.Preprocessing.Swagger.Examples.Responses.Generic;
using Nozomi.Ticker.Areas;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using UpdateCurrencyPairComponentExample = Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencyPairComponent.UpdateCurrencyPairComponentExample;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class SwaggerStartup
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            // https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/issues/56
            // services.AddSwaggerExamplesFromAssemblyOf<MyExample>();
            services.AddSwaggerExamplesFromAssemblyOf<CreateCurrencyExample>();
            services.AddSwaggerExamplesFromAssemblyOf<UpdateCurrencyExample>();
            services.AddSwaggerExamplesFromAssemblyOf<CreateCurrencyPairExample>();
            services.AddSwaggerExamplesFromAssemblyOf<CreateCurrencyPairComponentExample>();
            services.AddSwaggerExamplesFromAssemblyOf<UpdateCurrencyPairComponentExample>();
            services.AddSwaggerExamplesFromAssemblyOf<CreateCurrencyPairRequestExample>();
            services.AddSwaggerExamplesFromAssemblyOf<CreateSourceExample>();
            services.AddSwaggerExamplesFromAssemblyOf<NozomiJsonResultExample>();
            services.AddSwaggerExamplesFromAssemblyOf<NozomiStringResultExample>();
            
            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc(GlobalApiVariables.CURRENT_API_VERSION, new OpenApiInfo()
                 {
                     Version = GlobalApiVariables.CURRENT_API_VERSION,
                     Title = "Nozomi API",
                     Description = "Reference documentation for the usage of Nozomi.",
                     TermsOfService = new Uri("https://detabox.com/privacy"),
                     Contact = new OpenApiContact
                     {
                         Name = "Nicholas Chen",
                         Email = "nicholas@counter.network",
                         Url = new Uri("https://twitter.com/nixxholas")
                     },
                     License = new OpenApiLicense
                     {
                         Name = "Copyright (C) Hayate Inc. - All Rights Reserved",
                         Url = new Uri("https://detabox.com/license")
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