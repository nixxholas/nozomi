using Nozomi.Data.ViewModels.CurrencyType;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencyTypeService
    {
        void Create(CreateCurrencyTypeViewModel vm, string userId = null);

        void Update(UpdateCurrencyTypeViewModel vm, string userId = null);
    }
}