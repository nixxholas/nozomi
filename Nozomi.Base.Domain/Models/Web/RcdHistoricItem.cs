using System;
using Nozomi.Base.Core;

namespace Nozomi.Data.Models.Web
{
    public class RcdHistoricItem : Entity
    {
        public long Id { get; set; }
        
        public DateTime HistoricDateTime { get; set; }

        public string Value { get; set; }
        
        public long RequestComponentId { get; set; }
        public Component Component { get; set; }
    }
}