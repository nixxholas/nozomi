using Nozomi.Data.CurrencyModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nozomi.Data.WebModels
{
    public class CurrencyPairRequest : Request
    {
        public long CurrencyPairId { get; set; }

        public CurrencyPair CurrencyPair { get; set; }

        public new ICollection<CurrencyPairRequestComponent> RequestComponents { get; set; }

        public new bool IsValidForPolling()
        {
            return (!string.IsNullOrEmpty(DataPath) && !string.IsNullOrWhiteSpace(DataPath)
                                                    && RequestType >= 0)
                && (CurrencyPair != null) && CurrencyPair.CurrencyPairComponents != null
                && CurrencyPair.CurrencyPairComponents.Count > 0;
        }
    }
}
