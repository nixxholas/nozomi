using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Infra.Analysis.Service.Services.Interfaces
{
    public interface IAnalysedComponentService
    {
        long Create(AnalysedComponent analysedComponent, long userId = 0);

        bool UpdateValue(long analysedComponentId, string value, long userId = 0);
    }
}