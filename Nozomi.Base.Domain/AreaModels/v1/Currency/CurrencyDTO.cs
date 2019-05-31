using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.AreaModels.v1.Currency
{
    public class CurrencyDTO
    {
        public long Id { get; set; }
        
        public CurrencyType CurrencyType { get; set; }
        
        public string LogoPath { get; set; }
        
        public string Abbreviation { get; set; }

        public int SourceCount { get; set; }
        
        public string Slug { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string DenominationName { get; set; }
        
        public bool IsEnabled { get; set; }
    }
}