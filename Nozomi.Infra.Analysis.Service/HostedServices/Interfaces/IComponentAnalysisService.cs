using System.Collections.Generic;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Infra.Analysis.Service.HostedServices.Interfaces
{
    public interface IComponentAnalysisService
    {
        bool Analyse(ICollection<AnalysedComponent> component);

        bool Stash(AnalysedComponent component);
    }
}