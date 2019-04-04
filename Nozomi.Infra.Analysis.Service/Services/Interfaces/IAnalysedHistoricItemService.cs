using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Infra.Analysis.Service.Services.Interfaces
{
    public interface IAnalysedHistoricItemService
    {
        long Create(AnalysedHistoricItem item, long userId = 0);

        bool Push(AnalysedComponent analysedComponent, long userId = 0);
    }
}