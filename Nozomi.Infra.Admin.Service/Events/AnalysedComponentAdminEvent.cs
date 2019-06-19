using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Admin.Domain.AreaModels.AnalysedComponents;
using Nozomi.Base.Admin.Domain.AreaModels.AnalysedHistoricItem;
using Nozomi.Data.Models.Web.Analytical;
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

        public ICollection<AnalysedComponentDto> GetAllByCurrency(long currencyId, bool track = false)
        {
            var query = _unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsNoTracking()
                .Where(ac => ac.CurrencyId.Equals(currencyId));

            if (track)
            {
                query = query.Include(ac => ac.AnalysedHistoricItems);
            }
            
            return query
                .Select(ac => new AnalysedComponentDto
                {
                    Id = ac.Id,
                    ComponentType = ac.ComponentType,
                    Value = ac.Value,
                    IsDenominated = ac.IsDenominated,
                    Delay = ac.Delay,
                    IsEnabled = ac.IsEnabled,
                    CreatedAt = ac.CreatedAt,
                    CreatedBy = ac.CreatedBy,
                    ModifiedAt = ac.ModifiedAt,
                    ModifiedBy = ac.ModifiedBy,
                    DeletedAt = ac.DeletedAt,
                    DeletedBy = ac.DeletedBy,
                    AnalysedHistoricItems = ac.AnalysedHistoricItems
                        .Select(ahi => new AnalysedHistoricItemDto
                        {
                            Id = ahi.Id,
                            Value = ahi.Value,
                            HistoricDateTime = ahi.HistoricDateTime,
                            IsEnabled = ahi.IsEnabled,
                            CreatedAt = ahi.CreatedAt,
                            CreatedBy = ahi.CreatedBy,
                            ModifiedAt = ahi.ModifiedAt,
                            ModifiedBy = ahi.ModifiedBy,
                            DeletedAt = ahi.DeletedAt,
                            DeletedBy = ahi.DeletedBy
                        })
                        .ToList()
                })
                .ToList();
        }

        public ICollection<AnalysedComponentDto> GetAllByCurrencyPair(long currencyPairId, bool track = false)
        {
            var query = _unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
                .AsNoTracking()
                .Where(ac => ac.CurrencyPairId.Equals(currencyPairId));

            if (track)
            {
                query = query.Include(ac => ac.AnalysedHistoricItems);
            }
            
            return query
                .Select(ac => new AnalysedComponentDto
                {
                    Id = ac.Id,
                    ComponentType = ac.ComponentType,
                    Value = ac.Value,
                    IsDenominated = ac.IsDenominated,
                    Delay = ac.Delay,
                    IsEnabled = ac.IsEnabled,
                    CreatedAt = ac.CreatedAt,
                    CreatedBy = ac.CreatedBy,
                    ModifiedAt = ac.ModifiedAt,
                    ModifiedBy = ac.ModifiedBy,
                    DeletedAt = ac.DeletedAt,
                    DeletedBy = ac.DeletedBy,
                    AnalysedHistoricItems = ac.AnalysedHistoricItems
                        .Select(ahi => new AnalysedHistoricItemDto
                        {
                            Id = ahi.Id,
                            Value = ahi.Value,
                            HistoricDateTime = ahi.HistoricDateTime,
                            IsEnabled = ahi.IsEnabled,
                            CreatedAt = ahi.CreatedAt,
                            CreatedBy = ahi.CreatedBy,
                            ModifiedAt = ahi.ModifiedAt,
                            ModifiedBy = ahi.ModifiedBy,
                            DeletedAt = ahi.DeletedAt,
                            DeletedBy = ahi.DeletedBy
                        })
                        .ToList()
                })
                .ToList();
        }

        public ICollection<AnalysedComponentDto> GetAllByCurrencyType(long currencyTypeId, bool track = false)
        {
            throw new System.NotImplementedException();
        }
    }
}