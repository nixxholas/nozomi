using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Infra.Syncing.HostedServices.Interfaces
{
    public interface IComponentAnalysisService
    {
        bool AnalyseOne(AnalysedComponent component);

        bool Stash(AnalysedComponent component);
    }
}