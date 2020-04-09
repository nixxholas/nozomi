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

        public ItemPair(string mainTicker, string counterTicker, long sourceId, bool isEnabled = false)
        {
            Id = Guid.NewGuid();
            MainTicker = mainTicker;
            CounterTicker = counterTicker;
            SourceId = sourceId;
            IsEnabled = isEnabled;
        }
        
        [Key]
        public Guid Id { get; set; }

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