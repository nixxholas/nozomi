using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Nozomi.Base.BCL;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Data.Models.Category
{
    public class ItemPair : Entity
    {
        public ItemPair() {}

        public ItemPair(string mainTicker, string counterTicker, 
            string apiUrl, string defaultComponent, long sourceId, bool isEnabled = false)
        {
            Id = Guid.NewGuid();
            MainTicker = mainTicker;
            CounterTicker = counterTicker;
            ApiUrl = apiUrl;
            DefaultComponent = defaultComponent;
            SourceId = sourceId;
            IsEnabled = isEnabled;
        }
        
        [Key]
        public Guid Id { get; set; }

        public string ApiUrl { get; set; }
        
        /// <summary>
        /// Which component to rely on by default?
        /// </summary>
        public string DefaultComponent { get; set; }

        public long SourceId { get; set; }
        public Source Source { get; set; }

        // =========== RELATIONS ============ //
        public ICollection<Request> Requests { get; set; }
        
        [DataMember]
        public string MainTicker { get; set; }
        
        [DataMember]
        public string CounterTicker { get; set; }
    }
}