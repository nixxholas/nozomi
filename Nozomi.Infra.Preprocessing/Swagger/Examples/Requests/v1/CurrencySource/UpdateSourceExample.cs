using System.Collections.Generic;
using Nozomi.Data.AreaModels.v1.Source;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencySource
{
    public class UpdateSourceExample : IExamplesProvider<UpdateSource>
    {
        public UpdateSource GetExamples()
        {
            return new UpdateSource()
            {
                Id = 1,
                Name = "Bittrex",
                Abbreviation = "BTRX",
                ApiDocsUrl = "https://bittrex.zendesk.com/hc/en-us/articles/115003723911",
                UpdateCurrencyPairs = new List<UpdateSource.UpdateCurrencyPair>()
                {
                    new UpdateSource.UpdateCurrencyPair()
                    {
                        Id = 1
                    }
                },
                UpdateSourceCurrencies = new List<UpdateSource.UpdateSourceCurrency>()
                {
                    new UpdateSource.UpdateSourceCurrency()
                    {
                        Id = 3
                    }
                }
            };
        }
    }
}