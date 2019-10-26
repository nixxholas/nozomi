using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Nozomi.Base.Core;
using Nozomi.Base.Core.Models;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.Models.Web.Websocket;

namespace Nozomi.Data.Models.Currency
{
    public class CurrencyPair : Entity
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
        public ICollection<Request> Requests { get; set; }
        
        public string MainCurrencyAbbrv { get; set; }
        
        public string CounterCurrencyAbbrv { get; set; }

        public Currency MainCurrency()
        {
            if (Source != null && Source.SourceCurrencies != null && Source.SourceCurrencies.Count > 0
                && Source.SourceCurrencies.Any(sc => sc.Currency != null))
                return Source.SourceCurrencies
                    .Where(sc => sc.Currency.Abbreviation.Equals(MainCurrencyAbbrv, StringComparison.InvariantCultureIgnoreCase))
                    .Select(sc => sc.Currency).FirstOrDefault();

            return null;
        }

        public Currency CounterCurrency()
        {
            if (Source != null && Source.SourceCurrencies != null && Source.SourceCurrencies.Count > 0
                && Source.SourceCurrencies.Any(sc => sc.Currency != null))
                return Source.SourceCurrencies
                    .Where(sc => sc.Currency.Abbreviation.Equals(CounterCurrencyAbbrv, StringComparison.InvariantCultureIgnoreCase))
                    .Select(sc => sc.Currency).FirstOrDefault();

            return null;
        }
        
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(MainCurrencyAbbrv) && !string.IsNullOrEmpty(CounterCurrencyAbbrv) 
                   && CurrencyPairType > 0 && (!string.IsNullOrEmpty(APIUrl))
                                          && (!string.IsNullOrEmpty(DefaultComponent))
                                          && (SourceId > 0);
        }
    }
}
