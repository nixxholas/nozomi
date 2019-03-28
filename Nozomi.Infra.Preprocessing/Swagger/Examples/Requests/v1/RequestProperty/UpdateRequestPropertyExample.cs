using Nozomi.Data.AreaModels.v1.RequestProperty;
using Nozomi.Data.Models.Web;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Preprocessing.Swagger.Examples.Requests.v1.RequestProperty
{
    public class UpdateRequestPropertyExample : IExamplesProvider<UpdateRequestProperty>
    {
        public UpdateRequestProperty GetExamples()
        {
            return new UpdateRequestProperty()
            {
                Id = 3,
                RequestPropertyType = RequestPropertyType.HttpQuery,
                Key = "Hello",
                Value = "This is le value"
            };
        }
    }
}