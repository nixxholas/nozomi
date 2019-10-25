using System;
using Nozomi.Base.Core;
using Nozomi.Base.Core.Models;

namespace Nozomi.Data.Models.Web
{
    public class RcdHistoricItem : Entity
    {
        public long Id { get; set; }
        
        public DateTime HistoricDateTime { get; set; }

        public string Value { get; set; }
        
        public long RequestComponentId { get; set; }
        public RequestComponent RequestComponent { get; set; }
    }
}