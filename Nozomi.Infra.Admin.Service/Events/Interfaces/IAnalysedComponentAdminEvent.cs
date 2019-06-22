using System.Collections.Generic;
using Nozomi.Base.Admin.Domain.AreaModels.AnalysedComponent;

namespace Nozomi.Infra.Admin.Service.Events.Interfaces
{
    public interface IAnalysedComponentAdminEvent
    {
        ICollection<AnalysedComponentDto> GetAllByCurrency(long currencyId, bool track = false);
        
        ICollection<AnalysedComponentDto> GetAllByCurrencyPair(long currencyPairId, bool track = false);
        
        ICollection<AnalysedComponentDto> GetAllByCurrencyType(long currencyTypeId, bool track = false);
    }
}