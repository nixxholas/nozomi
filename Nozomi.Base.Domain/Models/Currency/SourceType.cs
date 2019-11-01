using Nozomi.Base.Core;

namespace Nozomi.Data.Models.Currency
{
    public class SourceType : Entity
    {
        public string Abbreviation { get; set; }
        
        public string Name { get; set; }
    }
}