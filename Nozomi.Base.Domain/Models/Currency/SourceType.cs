using System;
using System.Collections.Generic;
using Nozomi.Base.Core;

namespace Nozomi.Data.Models.Currency
{
    public class SourceType : Entity
    {
        public SourceType() {}

        public SourceType(string abbreviation, string name)
        {
            Abbreviation = abbreviation;
            Name = name;
        }
        
        public long Id { get; set; }
        
        public Guid Guid { get; set; }
    
        public string Abbreviation { get; set; }
        
        public string Name { get; set; }
        
        public ICollection<Source> Sources { get; set; }
    }
}