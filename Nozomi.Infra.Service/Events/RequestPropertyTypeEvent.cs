using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class RequestPropertyTypeEvent : BaseService<RequestPropertyTypeEvent, NozomiDbContext>, 
        IRequestPropertyTypeEvent
    {
        private readonly ICollection<KeyValuePair<string, int>> _requestPropertyTypeMap;

        public RequestPropertyTypeEvent(ILogger<RequestPropertyTypeEvent> logger,
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