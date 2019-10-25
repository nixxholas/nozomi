using System.Collections.Generic;
using Nozomi.Data.Models.Analytical;

namespace Nozomi.Infra.Analysis.Service.HostedServices.Interfaces
{
    public interface IComponentAnalysisService
    {
        bool AnalyseOne(AnalysedComponent component);

        bool Stash(AnalysedComponent component);
    }
}