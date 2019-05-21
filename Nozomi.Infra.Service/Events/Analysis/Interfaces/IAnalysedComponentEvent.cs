using System.Collections.Generic;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Service.Events.Analysis.Interfaces
{
    public interface IAnalysedComponentEvent
    {
        AnalysedComponent Get(long id, bool track = false);
        
        /// <summary>
        /// For internal consumption, exposes the entire dbset.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="track"></param>
        /// <returns></returns>
        IEnumerable<AnalysedComponent> GetAll(bool filter = false, bool track = false);
        
        IEnumerable<AnalysedComponent> GetAll(int index = 0, bool filter = false, bool track = false);

        ICollection<AnalysedComponent> GetAllByCurrency(long currencyId, bool ensureValid = false, bool track = false);
        
        ICollection<AnalysedComponent> GetTickerPairComponentsByCurrency(long currencyId, bool ensureValid = false, bool track = false);

        ICollection<AnalysedComponent> GetAllByCurrencyType(long currencyTypeId, bool track = false);
        
        ICollection<AnalysedComponent> GetAllCurrencyComponentsByType(long currencyTypeId, bool track = false);

        ICollection<AnalysedComponent> GetAllByCorrelation(long analysedComponentId, bool track = false);

        ICollection<AnalysedComponent> GetAllByCurrencyPair(long currencyPairId, bool track = false);

        string GetCurrencyAbbreviation(AnalysedComponent analysedComponent);
    }
}