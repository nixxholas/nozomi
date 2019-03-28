using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class PartialCurrencyPairEvent : BaseEvent<PartialCurrencyPairEvent, NozomiDbContext>, IPartialCurrencyPairEvent
    {
        public PartialCurrencyPairEvent(ILogger<PartialCurrencyPairEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public ICollection<PartialCurrencyPair> ObtainCounterPCPs(ICollection<PartialCurrencyPair> mainPCPs)
        {
            if (mainPCPs != null && mainPCPs.Count > 0)
            {
                return _unitOfWork.GetRepository<PartialCurrencyPair>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(pcp => !pcp.IsMain && mainPCPs.Any(mpcp => mpcp.CurrencyPairId.Equals(pcp.CurrencyPairId)))
                    .Include(pcp => pcp.Currency)
                    .ToList();
            }

            return null;
        }
    }
}