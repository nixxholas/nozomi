using Nozomi.Data.ViewModels.AnalysedComponent;

namespace Nozomi.Service.Services.Interfaces
{
    public interface IAnalysedComponentService
    {
        void Create(CreateAnalysedComponentViewModel vm, string userId);

        void Update(UpdateAnalysedComponentViewModel vm, string userId = null);
    }
}