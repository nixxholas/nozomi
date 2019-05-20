using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Nozomi.Base.Core;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
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

        public long SourceId { get; set; }
        public Source Source { get; set; }

        // =========== RELATIONS ============ //
        public ICollection<AnalysedComponent> AnalysedComponents { get; set; }
        public ICollection<CurrencyPairRequest> CurrencyPairRequests { get; set; }
        public ICollection<WebsocketRequest> WebsocketRequests { get; set; }
        
        public string MainCurrencyAbbrv { get; set; }
        
        public string CounterCurrencyAbbrv { get; set; }

        [NotMapped]
        public Currency MainCurrency => Source.SourceCurrencies
            .Where(sc => sc.Currency.Abbreviation.Equals(MainCurrencyAbbrv, StringComparison.InvariantCultureIgnoreCase))
            .Select(sc => sc.Currency).FirstOrDefault();

        [NotMapped]
        public Currency CounterCurrency => Source.SourceCurrencies
            .Where(sc =>
                sc.Currency.Abbreviation.Equals(CounterCurrencyAbbrv, StringComparison.InvariantCultureIgnoreCase))
            .Select(sc => sc.Currency).FirstOrDefault();
        
        public bool IsValid()
        {
            if (MainCurrency != null && CounterCurrency != null)
            {
                var mainCurr = MainCurrency.Abbreviation.Equals(MainCurrencyAbbrv,
                        StringComparison.InvariantCultureIgnoreCase);
                var counterCurr = CounterCurrency.Abbreviation.Equals(CounterCurrencyAbbrv,
                        StringComparison.InvariantCultureIgnoreCase);
                
                return (CurrencyPairType > 0) && (!string.IsNullOrEmpty(APIUrl))
                                              && (!string.IsNullOrEmpty(DefaultComponent))
                                              && (SourceId > 0);
            }
            
            return false;
        }
    }
}
