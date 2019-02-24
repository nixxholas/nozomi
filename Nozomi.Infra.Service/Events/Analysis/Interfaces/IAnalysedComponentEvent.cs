using System.Collections.Generic;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Service.Events.Analysis.Interfaces
{
    public interface IAnalysedComponentEvent
    {
        IEnumerable<AnalysedComponent> GetAll(bool filter = false, bool track = false);
    }
}