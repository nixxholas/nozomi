using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Nozomi.Core;
using Nozomi.Core.Helpers.Native.Collections;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Data.WebModels
{
    public class CurrencyPairRequest : Request
    {
        public long CurrencyPairId { get; set; }

        public CurrencyPair CurrencyPair { get; set; }

        public new bool IsValidForPolling()
        {
            if (RequestComponents == null || !RequestComponents.Any()) return false;
            
            var first = RequestComponents.FirstOrDefault();

            return (!string.IsNullOrEmpty(DataPath) && !string.IsNullOrWhiteSpace(DataPath)
                                                    && RequestType >= 0)
                   && first != null && RequestComponents != null 
                   && RequestComponents.Count > 0 && (first
                       .RequestComponentData
                       .OrderByDescending(rcd => rcd.CreatedAt)
                       .Select(rcd => rcd.CreatedAt).SingleOrDefault()).AddMilliseconds(Delay) >= DateTime.Now;
        }

        public JObject ObscureToPublicJson()
        {
            if (RequestComponents != null
                && RequestComponents.Count > 0)
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
