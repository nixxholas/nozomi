using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Enumerators.Interfaces;

namespace Nozomi.Service.Services.Enumerators
{
    public class RequestTypeService : BaseService<RequestTypeService, NozomiDbContext>, IRequestTypeService
    {
        private readonly ICollection<KeyValuePair<string, int>> _requestTypeMap;

        public RequestTypeService(ILogger<RequestTypeService> logger,
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
            _requestTypeMap = new List<KeyValuePair<string, int>>();

            foreach (var name in Enum.GetNames(typeof(RequestType)))
            {
                _requestTypeMap.Add(new KeyValuePair<string, int>(name,
                    (int) Enum.Parse(typeof(RequestType), name)));
            }
        }

        public ICollection<KeyValuePair<string, int>> All()
        {
            return _requestTypeMap;
        }
    }
}