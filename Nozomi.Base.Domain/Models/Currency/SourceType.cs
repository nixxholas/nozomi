using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Nozomi.Base.BCL;

namespace Nozomi.Data.Models.Currency
{
    [DataContract]
    public class SourceType : Entity
    {
        public SourceType() {}

        public SourceType(string abbreviation, string name)
        {
            Abbreviation = abbreviation;
            Name = name;
        }
        
        public long Id { get; set; }
        
        [DataMember]
        public Guid Guid { get; set; }
    
        [DataMember]
        public string Abbreviation { get; set; }
        
        [DataMember]
        public string Name { get; set; }
        
        public ICollection<Source> Sources { get; set; }
    }
}