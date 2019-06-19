using Nozomi.Data.AreaModels.v1.Source;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencySource
{
    public class DeleteSourceExample : IExamplesProvider<DeleteSource>
    {
        public DeleteSource GetExamples()
        {
            return new DeleteSource()
            {
                Id = 1,
                HardDelete = false
            };
        }
    }
}