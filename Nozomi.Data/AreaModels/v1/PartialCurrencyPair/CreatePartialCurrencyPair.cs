namespace Nozomi.Data.AreaModels.v1.PartialCurrencyPair
{
    public class CreatePartialCurrencyPair
    {
        public long CurrencyId { get; set; }
        
        public bool IsMain { get; set; }
    }
}