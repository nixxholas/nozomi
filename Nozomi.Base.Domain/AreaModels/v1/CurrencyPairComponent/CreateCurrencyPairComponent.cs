using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;

namespace Nozomi.Data.AreaModels.v1.CurrencyPairComponent
{
    public class CreateCurrencyPairComponent
    {
        public long ComponentType { get; set; }
        
        public string QueryComponent { get; set; }
        
        public long RequestId { get; set; }
        
        // The Service processing this object will help to create the RequestComponentDatum pegged to this object.
    }
}