namespace Nozomi.Data.ResponseModels.Currency
{
    public class CurrencyResponse
    {
        public long Id { get; set; }
        
        public string LogoPath { get; set; }
        
        public long CurrencyTypeId { get; set; }
        
        public string CurrencyType { get; set; }
        
        public string Abbreviation { get; set; }
        
        public string Name { get; set; }
    }
}