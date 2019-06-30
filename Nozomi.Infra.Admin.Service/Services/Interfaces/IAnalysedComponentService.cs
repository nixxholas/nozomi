using Nozomi.Base.Admin.Domain.AreaModels.AnalysedComponent;
using Nozomi.Data.AreaModels.v1.AnalysedComponent;

namespace Nozomi.Infra.Admin.Service.Services.Interfaces
{
    public interface IAnalysedComponentService
    {
        long Create(CreateAnalysedComponent analysedComponent, long userId = 0);

        bool Update(UpdateAnalysedComponent analysedComponent, long userId = 0);

        bool Delete(long analysedComponentId, bool hardDelete = false, long userId = 0);
    }
}