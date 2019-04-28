using System.Collections.Generic;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Infra.Analysis.Service.Events.Analysis.Interfaces
{
    public interface IAnalysedComponentEvent
    {
        AnalysedComponent Get(long id, bool track = false);
    
        void ConvertToGenericCurrency(ICollection<AnalysedComponent> analysedComponents);
        
        /// <summary>
        /// For internal consumption, exposes the entire dbset.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="track"></param>
        /// <returns></returns>
        IEnumerable<AnalysedComponent> GetAll(bool filter = false, bool track = false);
        
        IEnumerable<AnalysedComponent> GetAll(int index = 0, bool filter = false, bool track = false);

        ICollection<AnalysedComponent> GetAllByCurrency(long currencyId, bool track = false);

        ICollection<AnalysedComponent> GetAllByCorrelation(long analysedComponentId, bool track = false);

        string GetCurrencyAbbreviation(AnalysedComponent analysedComponent);
    }
}