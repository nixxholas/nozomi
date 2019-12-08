using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Admin.Domain.AreaModels.AnalysedComponent;
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

        public long Create(CreateAnalysedComponent analysedComponent, string userId = null)
        {
            // Make sure the analysed component that is going to be created doesn't exist yet
            if (!_unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsNoTracking()
                .Any(ac =>
                    // Case 1, currency
                    (ac.CurrencyId != null && ac.CurrencyId.Equals(analysedComponent.CurrencyId)
                                           && ac.ComponentType.Equals(analysedComponent.ComponentType))
                    ||
                    // Case 2, currency pair
                    (ac.CurrencyPairId != null && ac.CurrencyPairId.Equals(analysedComponent.CurrencyPairId)
                                               && ac.ComponentType.Equals(analysedComponent.ComponentType))
                    ||
                    // Case 3, currency type
                    (ac.CurrencyTypeId != null && ac.CurrencyTypeId.Equals(analysedComponent.CurrencyTypeId)
                                               && ac.ComponentType.Equals(analysedComponent.ComponentType))
                ))
            {
                var ac = new AnalysedComponent
                {
                    ComponentType = analysedComponent.ComponentType,
                    Delay = analysedComponent.Delay,
                    IsDenominated = analysedComponent.IsDenominated,
                    UIFormatting = analysedComponent.UiFormatting,
                    CurrencyId = analysedComponent.CurrencyId > 0 ? analysedComponent.CurrencyId : (long?) null,
                    CurrencyPairId = analysedComponent.CurrencyPairId > 0 ? analysedComponent.CurrencyPairId : (long?) null,
                    CurrencyTypeId = analysedComponent.CurrencyTypeId > 0 ? analysedComponent.CurrencyTypeId : (long?) null
                };
                _unitOfWork.GetRepository<AnalysedComponent>().Add(ac);
                _unitOfWork.Commit(userId);

                return ac.Id;
            }
            
            return long.MinValue;
        }

        public bool Update(UpdateAnalysedComponent analysedComponent, string userId = null)
        {
            var query = _unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsTracking()
                .SingleOrDefault(ac => ac.DeletedAt == null && ac.Id.Equals(analysedComponent.Id));

            if (query != null)
            {
                query.ComponentType = analysedComponent.ComponentType;
                query.IsDenominated = analysedComponent.IsDenominated;
                query.Delay = analysedComponent.Delay;
                query.IsEnabled = analysedComponent.IsEnabled;
                query.UIFormatting = analysedComponent.UiFormatting;

                _unitOfWork.Commit(userId);

                return true;
            }
            
            return false;
        }

        public bool Delete(long analysedComponentId, bool hardDelete = false, string userId = null)
        {
            if (analysedComponentId > 0)
            {
                var query = _unitOfWork.GetRepository<AnalysedComponent>()
                    .GetQueryable()
                    .AsTracking()
                    .SingleOrDefault(ac => ac.DeletedAt == null && ac.Id.Equals(analysedComponentId));

                if (query != null)
                {
                    if (hardDelete)
                    {
                        _unitOfWork.GetRepository<AnalysedComponent>().Delete(query);
                    }
                    else
                    {
                        query.DeletedAt = DateTime.UtcNow;
                        
                        if (!string.IsNullOrWhiteSpace(userId))
                            query.DeletedById = userId;
                    }

                    _unitOfWork.Commit(userId);

                    return true;
                }
            }

            return false;
        }
    }
}