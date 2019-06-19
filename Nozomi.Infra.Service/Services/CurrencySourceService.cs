using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencySource;
using Nozomi.Data.Models.Currency;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class CurrencySourceService : BaseService<CurrencySourceService, NozomiDbContext>, ICurrencySourceService
    {
        public CurrencySourceService(ILogger<CurrencySourceService> logger,
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public NozomiResult<string> Create(CreateCurrencySource currencySource, long userId = 0)
        {
            try
            {
                if (_unitOfWork.GetRepository<CurrencySource>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Any(cs => cs.CurrencyId.Equals(currencySource.CurrencyId)
                               && cs.SourceId.Equals(currencySource.SourceId)))
                    return new NozomiResult<string>(NozomiResultType.Failed, "Source to currency binding already exists.");
                
                _unitOfWork.GetRepository<CurrencySource>().Add(new CurrencySource
                {
                    CurrencyId = currencySource.CurrencyId,
                    SourceId = currencySource.SourceId
                });

                _unitOfWork.Commit(userId);

                return new NozomiResult<string>(NozomiResultType.Success, "Source successfully added!");
            }
            catch (Exception ex)
            {
                return new NozomiResult<string>(NozomiResultType.Failed, ex.ToString());
            }
        }

        public NozomiResult<string> Delete(long id, long userId = 0)
        {
            try
            {
                var csToDelete = _unitOfWork.GetRepository<CurrencySource>()
                    .Get(cs => cs.Id.Equals(id) && cs.DeletedAt == null)
                    .SingleOrDefault();

                if (csToDelete == null)
                    return new NozomiResult<string>(NozomiResultType.Failed, "Unable to delete currency source");
                
                csToDelete.DeletedAt = DateTime.UtcNow;
                csToDelete.DeletedBy = userId;

                _unitOfWork.GetRepository<CurrencySource>().Update(csToDelete);
                _unitOfWork.Commit(userId);

                return new NozomiResult<string>(NozomiResultType.Success, "Currency source successfully deleted!");
            }
            catch (Exception ex)
            {
                return new NozomiResult<string>(NozomiResultType.Failed, ex.ToString());
            }
        }
    }
}