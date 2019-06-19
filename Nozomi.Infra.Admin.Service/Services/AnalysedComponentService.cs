using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.AreaModels.v1.AnalysedComponent;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Infra.Admin.Service.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Admin.Service.Services
{
    public class AnalysedComponentService : BaseService<AnalysedComponentService, NozomiDbContext>, IAnalysedComponentService
    {
        public AnalysedComponentService(ILogger<AnalysedComponentService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public long Create(CreateAnalysedComponent analysedComponent, long userId = 0)
        {
            // Make sure the analysed component that is going to be created doesn't exist yet
            if (!_unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsNoTracking()
                .Any(ac =>
                    // Case 1, currency
                    (ac.CurrencyId != null && ac.CurrencyId.Equals(analysedComponent.CurrencyId)
                                           && !ac.ComponentType.Equals(analysedComponent.ComponentType))
                    ||
                    // Case 2, currency pair
                    (ac.CurrencyPairId != null && ac.CurrencyPairId.Equals(analysedComponent.CurrencyPairId)
                                               && !ac.ComponentType.Equals(analysedComponent.ComponentType))
                    ||
                    // Case 3, currency type
                    (ac.CurrencyTypeId != null && ac.CurrencyTypeId.Equals(analysedComponent.CurrencyTypeId)
                                               && !ac.ComponentType.Equals(analysedComponent.ComponentType))
                ))
            {
                _unitOfWork.GetRepository<AnalysedComponent>().Add(new AnalysedComponent
                {
                    ComponentType = analysedComponent.ComponentType,
                    Delay = analysedComponent.Delay,
                    IsDenominated = analysedComponent.IsDenominated,
                    UIFormatting = analysedComponent.UIFormatting,
                    CurrencyId = analysedComponent.CurrencyId,
                    CurrencyPairId = analysedComponent.CurrencyPairId,
                    CurrencyTypeId = analysedComponent.CurrencyTypeId
                });
                var res = _unitOfWork.Commit(userId);

                return res;
            }
            
            return long.MinValue;
        }
    }
}