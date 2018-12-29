using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.WebModels;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Enumerators.Interfaces;

namespace Nozomi.Service.Services.Enumerators
{
    public class RequestPropertyTypeService : BaseService<RequestPropertyTypeService, NozomiDbContext>, 
        IRequestPropertyTypeService
    {
        private readonly ICollection<KeyValuePair<string, int>> _requestPropertyTypeMap;

        public RequestPropertyTypeService(ILogger<RequestPropertyTypeService> logger,
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
            _requestPropertyTypeMap = new List<KeyValuePair<string, int>>();

            foreach (var name in Enum.GetNames(typeof(RequestPropertyType)))
            {
                _requestPropertyTypeMap.Add(new KeyValuePair<string, int>(name,
                    (int) Enum.Parse(typeof(RequestPropertyType), name)));
            }
        }

        public ICollection<KeyValuePair<string, int>> All()
        {
            return _requestPropertyTypeMap;
        }
    }
}