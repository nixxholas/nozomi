using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Nozomi.Base.Core;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Websocket;

namespace Nozomi.Data.Models.Currency
{
    public class CurrencyPair : BaseEntityModel
    {
        [Key]
        public long Id { get; set; }

        public CurrencyPairType CurrencyPairType { get; set; }

        public string APIUrl { get; set; }
        
        /// <summary>
        /// Which CPC to rely on by default?
        /// </summary>
        public string DefaultComponent { get; set; }

        public long CurrencySourceId { get; set; }
        public Source CurrencySource { get; set; }

        // =========== RELATIONS ============ //
        public ICollection<CurrencyPairRequest> CurrencyPairRequests { get; set; }
        public ICollection<WebsocketRequest> WebsocketRequests { get; set; }
        
        public string MainCurrency { get; set; }
        
        public string CounterCurrency { get; set; }
        
        public ICollection<CurrencyCurrencyPair> CurrencyPairCurrencies{ get; set; }

        public bool IsValid()
        {
            var firstPair = CurrencyPairCurrencies.FirstOrDefault(cp => cp.Currency.Abbrv.Equals(MainCurrency, 
                StringComparison.InvariantCultureIgnoreCase) 
            && cp.Currency.CurrencySourceId.Equals(CurrencySourceId));
            var lastPair = CurrencyPairCurrencies.SingleOrDefault(cp => cp.Currency.Abbrv.Equals(CounterCurrency, 
                                                               StringComparison.InvariantCultureIgnoreCase) 
                                                           && cp.Currency.CurrencySourceId.Equals(CurrencySourceId));

            return (CurrencyPairType > 0) && (!string.IsNullOrEmpty(APIUrl))
                                          && (!string.IsNullOrEmpty(DefaultComponent))
                                          && (CurrencySourceId > 0)
                                          && (CurrencyPairCurrencies.Count == 2)
                                          && (firstPair.CurrencyId != lastPair.CurrencyId);
        }
    }
}
