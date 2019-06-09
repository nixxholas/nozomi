using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Infra.Analysis.Service.Events.Interfaces
{
    public interface IXAnalysedComponentEvent
    {
        /// <summary>
        /// Obtains the first component that is pending update.
        /// </summary>
        /// <returns></returns>
        AnalysedComponent Top();
    }
}