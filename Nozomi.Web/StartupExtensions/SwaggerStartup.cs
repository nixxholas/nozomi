using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.Currency;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencyPair;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencyPairComponent;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencyPairRequest;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencySource;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.PartialCurrencyPair;
using Nozomi.Preprocessing.Swagger.Examples.Requests.v1.RequestProperty;
using Nozomi.Preprocessing.Swagger.Examples.Responses.Generic;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using UpdateCurrencyPairComponentExample = Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencyPairComponent.UpdateCurrencyPairComponentExample;

namespace Nozomi.Web.StartupExtensions
{
    public static class SwaggerStartup
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc(GlobalApiVariables.CURRENT_API_VERSION, new OpenApiInfo()
                {
                    Version = GlobalApiVariables.CURRENT_API_VERSION,
                    Title = "Nozomi API",
                    Description = "Reference documentation for the usage of Nozomi.",
                    TermsOfService = new Uri("https://nozomi.one/privacy"),
                    Contact = new OpenApiContact()
                    {
                        Name = "Nicholas Chen",
                        Email = "nicholas@counter.network",
                        Url = new Uri("https://twitter.com/nixxholas")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "Copyright (C) Hayate Inc. - All Rights Reserved",
                        Url = new Uri("https://nozomi.one/license")
                    }
                });


                swaggerGenOptions.ExampleFilters();
                swaggerGenOptions.OperationFilter<DescriptionOperationFilter>();
                // Adds an Upload button to endpoints which have [AddSwaggerFileUploadButton]
                // Supported out of the box.
                //swaggerGenOptions.OperationFilter<AddFileParamTypesOperationFilter>();

                // adds any string you like to the request headers - in this case, a correlation id
                // We don't need this yet
                //swaggerGenOptions.OperationFilter<AddHeaderOperationFilter>("correlationId", "Correlation Id for the request", false);

                swaggerGenOptions.OperationFilter<AddResponseHeadersFilter>(); // [SwaggerResponseHeader]
                // swaggerGenOptions.DescribeAllEnumsAsStrings(); // Automatic
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
