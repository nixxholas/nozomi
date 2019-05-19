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
        
        public ICollection<CurrencyPairSourceCurrency> CurrencyPairSourceCurrencies { get; set; }

        public bool IsValid()
        {
            if (CurrencyPairSourceCurrencies != null && CurrencyPairSourceCurrencies.Count == 2)
            {
                var mainCurr = CurrencyPairSourceCurrencies.FirstOrDefault(cpsc =>
                    cpsc.CurrencySource.Currency.Abbreviation.Equals(MainCurrency,
                        StringComparison.InvariantCultureIgnoreCase));
                var counterCurr = CurrencyPairSourceCurrencies.FirstOrDefault(cpsc =>
                    cpsc.CurrencySource.Currency.Abbreviation.Equals(CounterCurrency,
                        StringComparison.InvariantCultureIgnoreCase));
                
                return (CurrencyPairType > 0) && (!string.IsNullOrEmpty(APIUrl))
                                              && (!string.IsNullOrEmpty(DefaultComponent))
                                              && (CurrencySourceId > 0)
                                              && mainCurr != null && counterCurr != null 
                                              && !mainCurr.CurrencySourceId.Equals(counterCurr.CurrencySourceId)
                                              && mainCurr.CurrencySource.SourceId
                                                  .Equals(counterCurr.CurrencySource.SourceId);
            }
            
            return false;
        }
    }
}
