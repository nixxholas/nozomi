using System.Collections.Generic;
using Nozomi.Data.ResponseModels.Currency;

namespace Nozomi.Data.ResponseModels.Source
{
    /// <summary>
    /// The Extended Source Response.
    /// </summary>
    public class XSourceResponse : SourceResponse
    {
        
        public ICollection<CurrencyResponse> Currencies { get; set; }
    }
}