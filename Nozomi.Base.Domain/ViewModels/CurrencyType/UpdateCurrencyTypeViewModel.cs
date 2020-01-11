using System.ComponentModel;

namespace Nozomi.Data.ViewModels.CurrencyType
{
    public class UpdateCurrencyTypeViewModel : CurrencyTypeViewModel
    {
        [DefaultValue(false)]
        public bool Delete { get; set; }
    }
}