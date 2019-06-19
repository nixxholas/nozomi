using System.Collections.Generic;
using Nozomi.Base.Admin.Domain.AreaModels.AnalysedComponents;

namespace Nozomi.Infra.Admin.Service.Events.Interfaces
{
    public interface IAnalysedComponentAdminEvent
    {
        ICollection<AnalysedComponentDto> GetAllByCurrency(long currencyId);
        
        ICollection<AnalysedComponentDto> GetAllByCurrencyPair(long currencyPairId);
        
        ICollection<AnalysedComponentDto> GetAllByCurrencyType(long currencyTypeId);
    }
}