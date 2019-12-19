using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore.Internal;
using Nozomi.Base.Core;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.Models.Web.Websocket;

namespace Nozomi.Data.Models.Currency
{
    [DataContract]
    public class CurrencyPair : Entity
    {
        public CurrencyPair() {}

        public CurrencyPair(CurrencyPairType currencyPairType, string mainTicker, string counterTicker, 
            string apiUrl, string defaultComponent, long sourceId, bool isEnabled = false)
        {
            CurrencyPairType = currencyPairType;
            MainTicker = mainTicker;
            CounterTicker = counterTicker;
            APIUrl = apiUrl;
            DefaultComponent = defaultComponent;
            SourceId = sourceId;
            IsEnabled = isEnabled;
        }
        
        [Key]
        public long Id { get; set; }
        
        public Guid Guid { get; set; }

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
        
        [DataMember]
        public string MainTicker { get; set; }
        
        [DataMember]
        public string CounterTicker { get; set; }
        
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(MainTicker) && !string.IsNullOrEmpty(CounterTicker) 
                   && CurrencyPairType > 0 && (!string.IsNullOrEmpty(APIUrl))
                                          && (!string.IsNullOrEmpty(DefaultComponent))
                                          && (SourceId > 0);
        }
    }
}
