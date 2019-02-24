using System;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.Currency;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencyPair;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencyPairComponent;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencyPairRequest;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencySource;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.PartialCurrencyPair;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.RequestProperty;
using Nozomi.Preprocessing.Swagger.Examples.Responses.Generic;
using Nozomi.Ticker.Areas;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using License = Swashbuckle.AspNetCore.Swagger.License;
using UpdateCurrencyPairComponentExample = Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencyPairComponent.UpdateCurrencyPairComponentExample;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class SwaggerStartup
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc(GlobalApiVariables.CURRENT_API_VERSION, new Info()
                {
                    Version = GlobalApiVariables.CURRENT_API_VERSION,
                    Title = "Nozomi API",
                    Description = "Reference documentation for the usage of Nozomi.",
                    TermsOfService = new Uri("https://detabox.com/privacy").ToString(),
                    Contact = new Contact
                    {
                        Name = "Nicholas Chen",
                        Email = "nicholas@counter.network",
                        Url = new Uri("https://twitter.com/nixxholas").ToString()
                    },
                    License = new License
                    {
                        Name = "Copyright (C) Hayate Inc. - All Rights Reserved",
                        Url = new Uri("https://detabox.com/license").ToString()
                    }
                });


                swaggerGenOptions.ExampleFilters();
                swaggerGenOptions.OperationFilter<DescriptionOperationFilter>();
                // Adds an Upload button to endpoints which have [AddSwaggerFileUploadButton]
                swaggerGenOptions.OperationFilter<AddFileParamTypesOperationFilter>(); 
                swaggerGenOptions.OperationFilter<AddHeaderOperationFilter>();
                swaggerGenOptions.OperationFilter<AddResponseHeadersFilter>(); // [SwaggerResponseHeader]
                swaggerGenOptions.DescribeAllEnumsAsStrings();
                // Adds "(Auth)" to the summary so that you can see which endpoints have Authorization
                swaggerGenOptions.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                swaggerGenOptions.OperationFilter<SecurityRequirementsOperationFilter>();
                
                swaggerGenOptions.EnableAnnotations();
            });
            
            // https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/issues/56
            // services.AddSwaggerExamplesFromAssemblyOf<MyExample>();
            services.AddSwaggerExamplesFromAssemblyOf<CreateCurrencyExample>();
            services.AddSwaggerExamplesFromAssemblyOf<UpdateCurrencyExample>();
            services.AddSwaggerExamplesFromAssemblyOf<CreateCurrencyPairExample>();
            services.AddSwaggerExamplesFromAssemblyOf<CreateCurrencyPairComponentExample>();
            services.AddSwaggerExamplesFromAssemblyOf<UpdateCurrencyPairComponentExample>();
            services.AddSwaggerExamplesFromAssemblyOf<CreateCurrencyPairRequestExample>();
            services.AddSwaggerExamplesFromAssemblyOf<UpdateCurrencyPairRequestExample>();
            services.AddSwaggerExamplesFromAssemblyOf<CreateSourceExample>();
            services.AddSwaggerExamplesFromAssemblyOf<DeleteSourceExample>();
            services.AddSwaggerExamplesFromAssemblyOf<UpdateSourceExample>();
            services.AddSwaggerExamplesFromAssemblyOf<CreatePartialCurrencyPairExample>();
            services.AddSwaggerExamplesFromAssemblyOf<CreateRequestPropertyExample>();
            services.AddSwaggerExamplesFromAssemblyOf<UpdateRequestPropertyExample>();
            services.AddSwaggerExamplesFromAssemblyOf<NozomiJsonResultExample>();
            services.AddSwaggerExamplesFromAssemblyOf<NozomiStringResultExample>();
         }
     }
 }