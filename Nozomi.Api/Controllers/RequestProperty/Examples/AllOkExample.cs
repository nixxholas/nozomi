using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.RequestProperty;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.RequestProperty.Examples
{
    public class AllOkExample : IExamplesProvider<ICollection<RequestPropertyViewModel>>
    {
        public ICollection<RequestPropertyViewModel> GetExamples()
        {
            return new List<RequestPropertyViewModel>
            {
                new RequestPropertyViewModel(Guid.NewGuid(), RequestPropertyType.HttpHeader_AcceptEncoding, null, 
                    "utf-8"),
                new RequestPropertyViewModel(Guid.NewGuid(), RequestPropertyType.HttpHeader_Custom, "HelloHeader",
                    "Nice-one")
            };
        }
    }
}