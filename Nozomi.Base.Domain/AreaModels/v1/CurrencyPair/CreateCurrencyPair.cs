using Nozomi.Base.Core.Helpers.Native.Collections;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.AreaModels.v1.CurrencyPair
{
    public class CreateCurrencyPair
    {
        public CurrencyPairType CurrencyPairType { get; set; }
        
        public string ApiUrl { get; set; }
        
        public string DefaultComponent { get; set; }
        
        public long SourceId { get; set; }
        
        public string MainCurrencyAbbrv{ get; set; }
        
        public string CounterCurrencyAbbrv { get; set; }
        
        public bool IsEnabled { get; set; }

        public bool IsValid()
        {
            return CurrencyPairType >= 0 && !string.IsNullOrEmpty(ApiUrl) && SourceId > 0
                   && !MainCurrencyAbbrv.IsNullOrEmpty() && !CounterCurrencyAbbrv.IsNullOrEmpty();
        }
    }
}