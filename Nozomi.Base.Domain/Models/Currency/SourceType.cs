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
    
        public string Abbreviation { get; set; }
        
        public string Name { get; set; }
    }
}