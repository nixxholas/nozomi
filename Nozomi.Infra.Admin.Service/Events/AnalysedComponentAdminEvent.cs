using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Admin.Domain.AreaModels.AnalysedComponents;
using Nozomi.Infra.Admin.Service.Events.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Admin.Service.Events
{
    public class AnalysedComponentAdminEvent : BaseEvent<AnalysedComponentAdminEvent, NozomiDbContext>, 
        IAnalysedComponentAdminEvent
    {
        public AnalysedComponentAdminEvent(ILogger<AnalysedComponentAdminEvent> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public ICollection<AnalysedComponentDto> GetAllByCurrency(long currencyId)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<AnalysedComponentDto> GetAllByCurrencyPair(long currencyPairId)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<AnalysedComponentDto> GetAllByCurrencyType(long currencyTypeId)
        {
            throw new System.NotImplementedException();
        }
    }
}