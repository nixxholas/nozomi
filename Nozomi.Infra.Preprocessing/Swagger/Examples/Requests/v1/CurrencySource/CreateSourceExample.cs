using Nozomi.Data.AreaModels.v1.Source;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencySource
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