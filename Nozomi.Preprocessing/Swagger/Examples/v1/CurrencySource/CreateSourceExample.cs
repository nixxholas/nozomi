using Nozomi.Data.AreaModels.v1.CurrencySource;
using Swashbuckle.AspNetCore.Examples;

namespace Nozomi.Preprocessing.Swagger.Examples.v1.CurrencySource
{
    public class CreateSourceExample : IExamplesProvider
    {
        public object GetExamples()
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