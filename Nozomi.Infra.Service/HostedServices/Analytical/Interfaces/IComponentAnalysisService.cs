using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Service.HostedServices.Analytical.Interfaces
{
    public interface IComponentAnalysisService
    {
        bool Analyse(AnalysedComponent component);

        bool Stash(AnalysedComponent component);
    }
}