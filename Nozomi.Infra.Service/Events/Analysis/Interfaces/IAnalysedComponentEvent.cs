using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ViewModels.AnalysedComponent;
using Nozomi.Preprocessing;

namespace Nozomi.Service.Events.Analysis.Interfaces
{
    public interface IAnalysedComponentEvent
    {
        IEnumerable<AnalysedComponentViewModel> All(string currencySlug, string currencyPairGuid, string currencyTypeAbbrv, 
            int index = 0, int itemsPerPage = NozomiServiceConstants.AnalysedComponentTakeoutLimit, string userId = null);
        
        bool Exists(AnalysedComponentType type, long currencyId = 0, string currencySlug = null, 
            string currencyPairGuid = null, string currencyTypeShortForm = null);
        
        AnalysedComponent Get(long id, bool track = false, int index = 0);
        
        AnalysedComponent Get(string guid);

        UpdateAnalysedComponentViewModel Get(Guid guid, string userId = null);
        
        /// <summary>
        /// For internal consumption, exposes the entire dbset.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="track"></param>
        /// <returns></returns>
        IEnumerable<AnalysedComponent> GetAll(bool filter = false, bool track = false, int index = 0);
        
        IEnumerable<AnalysedComponent> GetAll(int index = 0, bool filter = false, bool track = false);

        ICollection<AnalysedComponent> GetAllCurrencyTypeAnalysedComponents(int index = 0, bool filter = false, 
            bool track = false);

        ICollection<AnalysedComponent> GetAllByCurrency(long currencyId, bool ensureValid = false, bool track = false,
            int index = 0);

        long GetTickerPairComponentsByCurrencyCount(long currencyId, Func<CurrencyPair, bool> predicate);

        ICollection<AnalysedComponent> GetAllByCurrencyType(long currencyTypeId, bool track = false, int index = 0, 
            long ago = long.MinValue);
        
        ICollection<AnalysedComponent> GetAllCurrencyComponentsByType(long currencyTypeId, bool track = false, 
            int index = 0);

        ICollection<AnalysedComponent> GetAllByCorrelation(long analysedComponentId, 
            Expression<Func<AnalysedComponent, bool>> predicate = null, 
            Func<AnalysedComponent, bool> clientPredicate = null, int index = 0, bool track = false);

        ICollection<AnalysedComponent> GetAllByCurrencyPair(long currencyPairId, bool track = false, int index = 0);

        string GetCurrencyAbbreviation(AnalysedComponent analysedComponent);

        AnalysedComponent Pop(Guid guid);
    }
}