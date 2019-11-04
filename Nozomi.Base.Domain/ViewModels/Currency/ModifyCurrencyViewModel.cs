namespace Nozomi.Data.ViewModels.Currency
{
    public class ModifyCurrencyViewModel : CurrencyViewModel
    {
        public long Id { get; set; }
        
        public bool IsEnabled { get; set; }
    }
}