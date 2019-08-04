using System.Collections.Generic;
using Nozomi.Data.ResponseModels.AnalysedComponent;

namespace Nozomi.Web.Controllers.APIs.v1.CurrencyType
{
    public interface ICurrencyTypeController
    {
        ICollection<ExtendedAnalysedComponentResponse<string>> GetAll(int page);
    }
}
