using System.Collections.Generic;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Infra.Analysis.Service.Events.Interfaces
{
    public interface IXAnalysedComponentEvent
    {
        /// <summary>
        /// Obtains the first component that is pending update.
        /// </summary>
        /// <returns></returns>
        AnalysedComponent Top(ICollection<long> acsToFilter = null);

        IEnumerable<AnalysedComponent> GetNextWorkingSet(int index = 0);
    }
}