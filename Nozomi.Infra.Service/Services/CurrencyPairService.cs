using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPair;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class CurrencyPairService : BaseService<CurrencyPairService, NozomiDbContext>, ICurrencyPairService
    {
        public CurrencyPairService(ILogger<CurrencyPairService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger,
            unitOfWork)
        {
        }

        public NozomiResult<string> Create(CreateCurrencyPair createCurrencyPair, long userId = 0)
        {
            if (createCurrencyPair == null || !createCurrencyPair.IsValid() ||
                // Make sure the pair we're creating doesn't exist.
                _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Any(cp => cp.MainCurrencyAbbrv.Equals(createCurrencyPair.MainCurrencyAbbrv,
                        StringComparison.InvariantCultureIgnoreCase)
                               && cp.CounterCurrencyAbbrv.Equals(createCurrencyPair.CounterCurrencyAbbrv,
                                   StringComparison.InvariantCultureIgnoreCase)
                               && cp.SourceId.Equals(createCurrencyPair.SourceId))) 
                return new NozomiResult<string>(
                    NozomiResultType.Failed, "Please ensure that the payload is valid");
            
            // Check the main ticker
            if (!_unitOfWork.GetRepository<CurrencySource>()
                .GetQueryable()
                .AsNoTracking()
                .Include(cs => cs.Currency)
                .Any(cs => cs.Currency.Abbreviation
                               .Equals(createCurrencyPair.MainCurrencyAbbrv,
                                   StringComparison.InvariantCultureIgnoreCase)
                           && cs.SourceId.Equals(createCurrencyPair.SourceId)))
            {
                // Since this doesn't exist, beep the user.
                return new NozomiResult<string>(
                    NozomiResultType.Failed, "Please ensure that the main ticker is valid, that it exists in that source.");
            }

            // Check the counter ticker
            if (!_unitOfWork.GetRepository<CurrencySource>()
                .GetQueryable()
                .AsNoTracking()
                .Include(cs => cs.Currency)
                .Any(cs => cs.Currency.Abbreviation.Equals(createCurrencyPair.CounterCurrencyAbbrv,
                               StringComparison.InvariantCultureIgnoreCase)
                           && cs.SourceId.Equals(createCurrencyPair.SourceId)))
            {
                // Since this doesn't exist, beep the user.
                return new NozomiResult<string>(
                    NozomiResultType.Failed, "Please ensure that the main ticker is valid, that it exists in that source.");
            }
            
            var currencyPair = new CurrencyPair()
            {
                CurrencyPairType = createCurrencyPair.CurrencyPairType,
                APIUrl = createCurrencyPair.ApiUrl,
                DefaultComponent = createCurrencyPair.DefaultComponent,
                SourceId = createCurrencyPair.SourceId,
                MainCurrencyAbbrv = createCurrencyPair.MainCurrencyAbbrv,
                CounterCurrencyAbbrv = createCurrencyPair.CounterCurrencyAbbrv,
                IsEnabled = createCurrencyPair.IsEnabled
            };
            
            _unitOfWork.GetRepository<CurrencyPair>().Add(currencyPair);
            _unitOfWork.Commit(userId);

            return new NozomiResult<string>(NozomiResultType.Success, "CurrencyPair successfully created");
        }

        public NozomiResult<string> Update(UpdateCurrencyPair updateCurrencyPair, long userId = 0)
        {
            if (updateCurrencyPair == null || !updateCurrencyPair.IsValid()
                || !_unitOfWork.GetRepository<CurrencySource>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Include(cs => cs.Currency)
                    .Any(cs => cs.Currency.Abbreviation.Equals(updateCurrencyPair.MainCurrencyAbbrv)
                        && cs.SourceId.Equals(updateCurrencyPair.SourceId))
                || !_unitOfWork.GetRepository<CurrencySource>()
                .GetQueryable()
                .AsNoTracking()
                .Include(cs => cs.Currency)
                .Any(cs => cs.Currency.Abbreviation.Equals(updateCurrencyPair.CounterCurrencyAbbrv)
                           && cs.SourceId.Equals(updateCurrencyPair.SourceId)))
                return new NozomiResult<string>(
                    NozomiResultType.Failed, "Please ensure that the payload is valid");

            var cpToUpd = _unitOfWork.GetRepository<CurrencyPair>()
                .Get(cp => cp.Id.Equals(updateCurrencyPair.Id) && cp.DeletedAt == null)
                .SingleOrDefault();

            if (cpToUpd == null || _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Any(cp => !cp.Id.Equals(updateCurrencyPair.Id) 
                               && cp.SourceId.Equals(updateCurrencyPair.SourceId)
                               && cp.MainCurrencyAbbrv.Equals(cp.MainCurrencyAbbrv, StringComparison.InvariantCultureIgnoreCase)
                               && cp.CounterCurrencyAbbrv.Equals(cp.CounterCurrencyAbbrv, StringComparison.InvariantCultureIgnoreCase)))
                return new NozomiResult<string>(
                    NozomiResultType.Failed, "Please ensure that the payload is valid and if the data you're " +
                                             "submitting does not contain a pre-existing ticker pair.");

            cpToUpd.MainCurrencyAbbrv = updateCurrencyPair.MainCurrencyAbbrv;
            cpToUpd.CounterCurrencyAbbrv = updateCurrencyPair.CounterCurrencyAbbrv;
            cpToUpd.SourceId = updateCurrencyPair.SourceId;
            cpToUpd.CurrencyPairType = updateCurrencyPair.CurrencyPairType;
            cpToUpd.APIUrl = updateCurrencyPair.ApiUrl;
            cpToUpd.DefaultComponent = updateCurrencyPair.DefaultComponent;
            cpToUpd.IsEnabled = updateCurrencyPair.IsEnabled;
            
            _unitOfWork.GetRepository<CurrencyPair>().Update(cpToUpd);
            _unitOfWork.Commit(userId);
            
            return new NozomiResult<string>(NozomiResultType.Success, "CurrencyPair successfully updated!");
        }

        public NozomiResult<string> Delete(long currencyPairId, long userId = 0, bool hardDelete = false)
        {
            if (currencyPairId > 0)
            {
                var cpToDel = _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .SingleOrDefault(cp => cp.Id.Equals(currencyPairId));

                if (hardDelete && cpToDel != null)
                {
                    _unitOfWork.GetRepository<CurrencyPair>().Delete(cpToDel);
                }
                else if (cpToDel != null && cpToDel.DeletedAt == null)
                {
                    cpToDel.DeletedAt = DateTime.UtcNow;
                    cpToDel.DeletedBy = userId;
                    _unitOfWork.GetRepository<CurrencyPair>().Update(cpToDel);
                }
                else
                {
                    // User has attempted to delete a deleted entity.
                    return new NozomiResult<string>(NozomiResultType.Failed, "Delete failed, the currency pair does not exist");
                }

                _unitOfWork.Commit(userId);

                return new NozomiResult<string>(NozomiResultType.Success, "CurrencyPair has been successfully deleted!");
            }

            return new NozomiResult<string>(NozomiResultType.Failed, "Delete failed.");
        }

        public IEnumerable<CurrencyPair> GetAllActive(int index = 0, bool includeNested = false)
        {
            return !includeNested ? _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .Include(cp => cp.Requests)
                    .ThenInclude(cpr => cpr.RequestComponents)
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                .Skip(index * 20)
                .Take(20) :
                _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                    .Skip(index * 20)
                    .Take(20);
        }

        public IEnumerable<string> GetAllCurrencyPairUrls()
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                .Select(cp => cp.APIUrl)
                .ToList();
        }

        public long[][] GetCurrencySourceMappings()
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .Where(cp => cp.DeletedAt == null)
                .Where(cp => cp.IsEnabled)
                .Include(cp => cp.Source)
                .Select(cp => new long[] {cp.Id, cp.Source.Id})
                .ToArray();
        }
    }
}