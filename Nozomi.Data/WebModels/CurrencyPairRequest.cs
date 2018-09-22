using Nozomi.Data.CurrencyModels;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nozomi.Core;

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

        public JObject ObscureToPublicJson()
        {
            if (CurrencyPair?.CurrencyPairComponents != null
                && CurrencyPair.CurrencyPairComponents.Count > 0)
            {
            
                return new NozomiJObject(true)
                {
                    // FromObject() is faster than Serialization
                    // https://stackoverflow.com/questions/20857432/json-net-jobject-fromobject-vs-jsonconvert-deserializeobjectjobjectjsonconver
                    { "CurrencyPair", JArray.FromObject(CurrencyPair) },
                    { "RequestComponents", JArray.FromObject(RequestComponents) }
                };
            }

            return new JObject(false, "Invalid Request.");
        }
    }
}
