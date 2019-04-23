using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Infra.Analysis.Service.Events.Analysis.Interfaces
{
    public interface IAnalysedHistoricItemEvent
    {
        AnalysedHistoricItem Latest(long analysedComponentId);

        ICollection<AnalysedHistoricItem> GetAll(long analysedComponentId, TimeSpan since, int page = 0);
    }
}