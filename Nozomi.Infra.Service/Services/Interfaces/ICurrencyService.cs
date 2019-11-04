using Nozomi.Data.ViewModels.Currency;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencyService
    {
        void Create(CreateCurrencyViewModel vm, string userId);
    }
}