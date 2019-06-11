using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Preprocessing.Abstracts.Interfaces;

namespace Nozomi.Service.Events.Analysis.Interfaces
{
    public interface IAnalysedComponentEvent
    {
        AnalysedComponent Get(long id, bool track = false, int index = 0);
        
        /// <summary>
        /// For internal consumption, exposes the entire dbset.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="track"></param>
        /// <returns></returns>
        IEnumerable<AnalysedComponent> GetAll(bool filter = false, bool track = false, int index = 0);
        
        IEnumerable<AnalysedComponent> GetAll(int index = 0, bool filter = false, bool track = false);

        ICollection<AnalysedComponent> GetAllCurrencyTypeAnalysedComponents(int index = 0, bool filter = false, bool track = false);

        ICollection<AnalysedComponent> GetAllByCurrency(long currencyId, bool ensureValid = false, bool track = false,
            int index = 0);

        long GetTickerPairComponentsByCurrencyCount(long currencyId, Func<CurrencyPair, bool> predicate);
        
        ICollection<AnalysedComponent> GetTickerPairComponentsByCurrency(long currencyId, bool ensureValid = false, 
            int index = 0, bool track = false,  Expression<Func<AnalysedComponent, bool>> predicate = null, 
            int historicItemIndex = 0);

        ICollection<AnalysedComponent> GetAllByCurrencyType(long currencyTypeId, bool track = false, int index = 0, 
            long ago = long.MinValue);
        
        ICollection<AnalysedComponent> GetAllCurrencyComponentsByType(long currencyTypeId, bool track = false, int index = 0);

        ICollection<AnalysedComponent> GetAllByCorrelation(long analysedComponentId, bool track = false, int index = 0);

        ICollection<AnalysedComponent> GetAllByCurrencyPair(long currencyPairId, bool track = false, int index = 0);

        string GetCurrencyAbbreviation(AnalysedComponent analysedComponent);
    }
}