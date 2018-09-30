using System;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.WebModels;
using Nozomi.Repo.Data;
using Nozomi.Repo.Repositories;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Interfaces;

namespace CounterCore.Service.Services
{
    public class CurrencyPairComponentService : BaseService<CurrencyPairComponentService, NozomiDbContext>, ICurrencyPairComponentService
    {
        public CurrencyPairComponentService(ILogger<CurrencyPairComponentService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public bool UpdatePairValue(long id, decimal val)
        {
            var pairToUpd = _unitOfWork
                                     .GetRepository<CurrencyPairComponent>()
                                     .GetQueryable()
                                     .Where(cp => cp.Id.Equals(id))
                                     .Include(cpc => cpc.RequestComponentData)
                                     .SingleOrDefault(cp => cp.DeletedAt == null);

            // Anormaly Detection
            if (pairToUpd != null && !pairToUpd.IsValueAbnormal(val.ToString(CultureInfo.InvariantCulture)))
            {
                var newRcd = new RequestComponentDatum()
                {
                    RequestComponentId = pairToUpd.Id,
                    Value = val.ToString(CultureInfo.InvariantCulture)
                };
                
                _unitOfWork.GetRepository<RequestComponentDatum>().Add(newRcd);
                _unitOfWork.Commit();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
