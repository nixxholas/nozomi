using System.Linq;
using Newtonsoft.Json.Linq;
using Nozomi.Base.Core;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.Models.Web
{
    public class CurrencyPairRequest : Request
    {
        public long CurrencyPairId { get; set; }

        public CurrencyPair CurrencyPair { get; set; }

        public new bool IsValidForPolling()
        {
            if (RequestComponents == null || !RequestComponents.Any()) return false;
            
            var first = RequestComponents.FirstOrDefault();

            // When RequestComponentDatum was 'Data'
//            if (first?.RequestComponentDatum?.Count > 0)
//            {
//                return (!string.IsNullOrEmpty(DataPath) && !string.IsNullOrWhiteSpace(DataPath)
//                                                        && RequestType >= 0) 
//                                                        && (first.RequestComponentData
//                                                        .OrderByDescending(rcd => rcd.CreatedAt)
//                                                        .Select(rcd => rcd.CreatedAt)
//                                                        .FirstOrDefault()
//                                                        .AddMilliseconds(Delay) <= DateTime.UtcNow);
//            }

            return (!string.IsNullOrEmpty(DataPath) && !string.IsNullOrWhiteSpace(DataPath)
                                                    && RequestType >= 0);
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
