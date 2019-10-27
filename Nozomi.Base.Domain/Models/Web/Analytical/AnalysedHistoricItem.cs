using System;
using Nozomi.Base.Core;

namespace Nozomi.Data.Models.Web.Analytical
{
    public class AnalysedHistoricItem : Entity
    {
        public long Id { get; set; }
        
        public long AnalysedComponentId { get; set; }
        
        public AnalysedComponent AnalysedComponent { get; set; }
        
        public string Value { get; set; }
        
        public DateTime HistoricDateTime { get; set; }
    }
}