using Nozomi.Base.Admin.Domain.AreaModels.AnalysedComponent;

namespace Nozomi.Infra.Admin.Service.Services.Interfaces
{
    public interface IAnalysedComponentService
    {
        long Create(CreateAnalysedComponent analysedComponent, string userId = null);

        bool Update(UpdateAnalysedComponent analysedComponent, string userId = null);

        bool Delete(long analysedComponentId, bool hardDelete = false, string userId = null);
    }
}