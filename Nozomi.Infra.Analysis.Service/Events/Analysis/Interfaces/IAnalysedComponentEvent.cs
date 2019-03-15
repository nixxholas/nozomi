using System.Collections.Generic;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Infra.Analysis.Service.Events.Analysis.Interfaces
{
    public interface IAnalysedComponentEvent
    {
        void ConvertToGenericCurrency(ICollection<AnalysedComponent> analysedComponents);
        
        IEnumerable<AnalysedComponent> GetAll(bool filter = false, bool track = false);

        ICollection<AnalysedComponent> GetAllByCurrency(long currencyId);

        string GetCurrencyAbbreviation(AnalysedComponent analysedComponent);
    }
}