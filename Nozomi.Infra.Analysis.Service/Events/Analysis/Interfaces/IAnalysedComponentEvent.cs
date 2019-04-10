using System.Collections.Generic;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Infra.Analysis.Service.Events.Analysis.Interfaces
{
    public interface IAnalysedComponentEvent
    {
        void ConvertToGenericCurrency(ICollection<AnalysedComponent> analysedComponents);
        
        IEnumerable<AnalysedComponent> GetAll(int index = 0, bool filter = false, bool track = false);

        ICollection<AnalysedComponent> GetAllByCurrency(long currencyId, bool track = false);

        ICollection<AnalysedComponent> GetAllByCorrelation(long analysedComponentId, bool track = false);

        string GetCurrencyAbbreviation(AnalysedComponent analysedComponent);
    }
}