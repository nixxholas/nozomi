using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Nozomi.Base.BCL;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Data.Models.Categorisation
{
    [DataContract]
    public class ItemPair : Entity
    {
        public ItemPair() {}

        public ItemPair(CurrencyPairType type, string mainTicker, string counterTicker, 
            string apiUrl, string defaultComponent, long sourceId, bool isEnabled = false)
        {
            Guid = Guid.NewGuid();
            Type = type;
            MainTicker = mainTicker;
            CounterTicker = counterTicker;
            ApiUrl = apiUrl;
            DefaultComponent = defaultComponent;
            SourceId = sourceId;
            IsEnabled = isEnabled;
        }
        
        [Key]
        public long Id { get; set; }
        
        public Guid Guid { get; set; }

        public CurrencyPairType Type { get; set; }

        public string ApiUrl { get; set; }
        
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
    }
}
