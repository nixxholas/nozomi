namespace Nozomi.Data.ResponseModels.Currency
{
    public class CurrencyResponse
    {
        public long Id { get; set; }
        
        public long CurrencyTypeId { get; set; }
        
        public string CurrencyType { get; set; }
        
        public string Abbrv { get; set; }
        
        public string Name { get; set; }
    }
}