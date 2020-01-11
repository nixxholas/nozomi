using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ViewModels.CurrencyType;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class CurrencyTypeEvent : BaseEvent<CurrencyPairEvent, NozomiDbContext>, ICurrencyTypeEvent
    {
        public CurrencyTypeEvent(ILogger<CurrencyPairEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork)
            : base(logger, unitOfWork)
        {
        }

        public bool Exists(string typeShortForm)
        {
            return _unitOfWork.GetRepository<CurrencyType>()
                .GetQueryable()
                .AsNoTracking()
                .Any(ct => ct.TypeShortForm.Equals(typeShortForm));
        }

        public bool Exists(Guid guid)
        {
            return _unitOfWork.GetRepository<CurrencyType>()
                .GetQueryable()
                .AsNoTracking()
                .Any(ct => ct.Guid.Equals(guid));
        }

        public bool Exists(long id)
        {
            return _unitOfWork.GetRepository<CurrencyType>()
                .GetQueryable()
                .AsNoTracking()
                .Any(ct => ct.Id.Equals(id));
        }

        public IEnumerable<CurrencyTypeViewModel> All(int index = 0, int itemsPerPage = 200)
        {
            if (itemsPerPage > 200 || itemsPerPage <= 0) // Always default to 200
                itemsPerPage = 200;
            
            return _unitOfWork.GetRepository<CurrencyType>()
                .GetQueryable()
                .AsNoTracking()
                .Where(ct => ct.DeletedAt == null && ct.IsEnabled)
                .Skip(index * itemsPerPage)
                .Take(itemsPerPage)
                .Select(ct => new CurrencyTypeViewModel
                {
                    Guid = ct.Guid,
                    Name = ct.Name,
                    TypeShortForm = ct.TypeShortForm
                });
        }

        public CurrencyType Get(string guid, bool track = false)
        {
            if (!string.IsNullOrWhiteSpace(guid))
            {
                var currencyType = _unitOfWork.GetRepository<CurrencyType>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(ct => ct.DeletedAt == null && ct.IsEnabled
                                                      && ct.Guid.Equals(Guid.Parse(guid)));

                if (currencyType.Any())
                {
                    if (track)
                    {
                        currencyType = currencyType
                            .Include(ct => ct.AnalysedComponents)
                            .Include(ct => ct.Currencies)
                            .Include(ct => ct.Requests);
                    }

                    return currencyType.SingleOrDefault();
                }
            }

            return null;
        }

        public CurrencyType Get(long id, bool track = false)
        {
            if (id > 0)
            {
                var currencyType = _unitOfWork.GetRepository<CurrencyType>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(ct => ct.DeletedAt == null && ct.IsEnabled
                                                      && ct.Id.Equals(id));

                if (currencyType.Any())
                {
                    if (track)
                    {
                        currencyType = currencyType
                            .Include(ct => ct.AnalysedComponents)
                            .Include(ct => ct.Currencies)
                            .Include(ct => ct.Requests);
                    }

                    return currencyType.SingleOrDefault();
                }
            }

            return null;
        }

        public ICollection<CurrencyType> GetAll(int index = 0, bool track = false)
        {
            if (index >= 0)
            {
                var cTypes = _unitOfWork.GetRepository<CurrencyType>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(ct => ct.DeletedAt == null && ct.IsEnabled);

                if (track)
                {
                    cTypes = cTypes
                        .Include(ct => ct.AnalysedComponents)
                        .Include(ct => ct.Currencies)
                        .Include(ct => ct.Requests);
                }

                return cTypes
                    .OrderBy(ct => ct.Id)
                    .Skip(index * NozomiServiceConstants.CurrencyTypeTakeoutLimit)
                    .Take(NozomiServiceConstants.CurrencyTypeTakeoutLimit)
                    .ToList();
            }

            return null;
        }

        public ICollection<CurrencyTypeViewModel> ListAll(int page = 0, int itemsPerPage = 50,
            bool orderAscending = true, string orderingParam = "TypeShortForm")
        {
            if (page < 0)
                page = 0;

            if (itemsPerPage < 0 || itemsPerPage > NozomiServiceConstants.CurrencyTypeTakeoutLimit)
                itemsPerPage = NozomiServiceConstants.CurrencyTypeTakeoutLimit;
            
            var query = _unitOfWork.GetRepository<CurrencyType>()
                .GetQueryable()
                .AsNoTracking()
                .Where(ct => ct.DeletedAt == null && ct.IsEnabled);
            
            switch (orderingParam)
            {
                case "Name":
                    if (orderAscending)
                        query = query.OrderBy(ct => ct.Name);
                    else
                        query = query.OrderByDescending(ct => ct.Name);
                    break;
                case "TypeShortForm":
                    if (orderAscending)
                        query = query.OrderBy(ct => ct.TypeShortForm);
                    else
                        query = query.OrderByDescending(ct => ct.TypeShortForm);
                    break;
            }

            return query
                .Skip(page * itemsPerPage)
                .Take(itemsPerPage)
                .Select(ct => new CurrencyTypeViewModel
                {
                    Guid = ct.Guid,
                    TypeShortForm = ct.TypeShortForm,
                    Name = ct.Name
                })
                .ToList();
        }

        /// <summary>
        /// Obtain a tracked version of currency type
        /// </summary>
        /// <param name="guid">GUID of the currency type in question</param>
        /// <returns>Currency type in question</returns>
        public CurrencyType Pop(Guid guid)
        {
            return _unitOfWork.GetRepository<CurrencyType>()
                .GetQueryable()
                .AsTracking()
                .SingleOrDefault(ct => ct.Guid.Equals(guid));
        }
    }
}