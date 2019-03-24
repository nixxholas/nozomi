using System;
using Nozomi.Base.Core;

namespace Nozomi.Data.Models.Web
{
    public class RcdHistoricItem : BaseEntityModel
    {
        public long Id { get; set; }
        
        public DateTime HistoricDateTime { get; set; }

        public string Value { get; set; }
        
        public long RequestComponentId { get; set; }
        public RequestComponent RequestComponent { get; set; }
    }
}