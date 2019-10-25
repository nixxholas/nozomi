using Nozomi.Data.AreaModels.v1.RequestProperty;
using Nozomi.Data.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Preprocessing.Swagger.Examples.Requests.v1.RequestProperty
{
    public class CreateRequestPropertyExample : IExamplesProvider<CreateRequestProperty>
    {
        public CreateRequestProperty GetExamples()
        {
            return new CreateRequestProperty()
            {
                RequestPropertyType = RequestPropertyType.HttpQuery,
                Key = "Hello",
                Value = "This is le value"
            };
        }
    }
}