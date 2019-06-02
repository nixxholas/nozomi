using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Service.Events.Analysis.Interfaces
{
    public interface IAnalysedHistoricItemEvent
    {
        AnalysedHistoricItem Latest(long analysedComponentId);

        long Count(long analysedComponentId);

        ICollection<AnalysedHistoricItem> GetAll(long analysedComponentId, TimeSpan since, int page = 0);
        
        IEnumerable<AnalysedHistoricItem> GetAll(long analysedComponentId, bool track = false, int index = 0);
    }
}