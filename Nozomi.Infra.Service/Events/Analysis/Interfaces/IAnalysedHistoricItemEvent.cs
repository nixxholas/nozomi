using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Service.Events.Analysis.Interfaces
{
    public interface IAnalysedHistoricItemEvent
    {
        AnalysedHistoricItem Latest(long analysedComponentId);
    }
}