using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nozomi.Base.Core.Responses;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ViewModels.AnalysedHistoricItem;

namespace Nozomi.Service.Events.Analysis.Interfaces
{
    public interface IAnalysedHistoricItemEvent
    {
        AnalysedHistoricItem Latest(long analysedComponentId);

        long Count(long analysedComponentId);

        ICollection<AnalysedHistoricItem> GetAll(long analysedComponentId, TimeSpan since, int page = 0);
        
        IEnumerable<AnalysedHistoricItem> GetAll(long analysedComponentId, bool track = false, int index = 0);

        IEnumerable<AnalysedHistoricItemViewModel> List(Guid guid, int page = 0, int itemsPerPage = 50);

        long GetRelevantComponentQueryCount(long analysedComponentId, Expression<Func<AnalysedHistoricItem, bool>> predicate = null, 
            bool deepTrack = false);

        ICollection<AnalysedHistoricItem> GetRelevantHistorics(long analysedComponentId, 
            Expression<Func<AnalysedHistoricItem, bool>> predicate, int index = 0);

        /// <summary>
        /// This enables the caller to enter the AnalysedComponent in question, traverse to its parent in search for
        /// the related component in question.
        /// </summary>
        /// <param name="analysedComponentId">The component we're utilising to look for the right type</param>
        /// <param name="componentType">The type of the component we're looking for.</param>
        /// <returns>Historicals of the target.</returns>
        NozomiPaginatedResult<AnalysedHistoricItem> TraverseRelatedHistory(long analysedComponentId,
            AnalysedComponentType componentType, int page = 0);

        NozomiPaginatedResult<AnalysedHistoricItem> GetCurrencyPriceHistory(string slug, int index = 0, int perPage = 0);
    }
}