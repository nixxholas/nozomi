using Nozomi.Data.AreaModels.v1.CurrencySource;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Preprocessing.Swagger.Examples.v1.CurrencySource
{
    // Defining the model class automates the annotation.
    // https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters#automatic-annotation
    public class CreateSourceExample : IExamplesProvider<CreateSource>
    {
        public CreateSource GetExamples()
        {
            return new CreateSource()
            {
                Name = "Bittrex",
                Abbreviation = "BTRX",
                ApiDocsUrl = "https://bittrex.zendesk.com/hc/en-us/articles/115003723911"
            };
        }
    }
}