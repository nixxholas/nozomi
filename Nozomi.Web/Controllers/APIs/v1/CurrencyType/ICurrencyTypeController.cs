using System.Collections.Generic;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.AnalysedComponent;

namespace Nozomi.Web.Controllers.APIs.v1.CurrencyType
{
    public interface ICurrencyTypeController
    {
        ICollection<ExtendedAnalysedComponentResponse<EpochValuePair<string>>> GetAll(int page);
    }
}
