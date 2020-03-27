using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Nozomi.Api.Filters
{
    public class XLogoDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // need to check if dextension already exists, otherwise swagger 
            // tries to re-add it and results in error  
            if (!swaggerDoc.Info.Extensions.ContainsKey("x-logo"))
            {
                swaggerDoc.Info.Extensions.Add("x-logo", new OpenApiObject
                    {
                        { "url", new OpenApiString("https://auth.nozomi.one/logo.png") },
                        { "altText", new OpenApiString("Nozomi") }
                    });
            }
        }
    }
}