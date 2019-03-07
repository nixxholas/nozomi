namespace Nozomi.Data.Models.Web
{
    public class CurrencyRequest : Request
    {
        public long CurrencyId { get; set; }
        
        public Currency.Currency Currency { get; set; }
    }
}