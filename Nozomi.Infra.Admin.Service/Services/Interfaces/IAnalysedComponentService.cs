using Nozomi.Data.AreaModels.v1.AnalysedComponent;

namespace Nozomi.Infra.Admin.Service.Services.Interfaces
{
    public interface IAnalysedComponentService
    {
        long Create(CreateAnalysedComponent analysedComponent, long userId = 0);
    }
}