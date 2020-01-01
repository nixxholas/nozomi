using System;
using System.Runtime.Serialization;
using Nozomi.Base.BCL;

namespace Nozomi.Data.Models.Web.Analytical
{
    [DataContract]
    public class AnalysedHistoricItem : Entity
    {
        public long Id { get; set; }
        
        public long AnalysedComponentId { get; set; }
        
        public AnalysedComponent AnalysedComponent { get; set; }
        
        [DataMember]
        public string Value { get; set; }
        
        [DataMember]
        public DateTime HistoricDateTime { get; set; }
    }
}