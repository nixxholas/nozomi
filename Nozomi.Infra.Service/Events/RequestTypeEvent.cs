using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class RequestTypeEvent : BaseService<RequestTypeEvent, NozomiDbContext>, IRequestTypeEvent
    {
        private readonly ICollection<KeyValuePair<string, int>> _requestTypeMap;

        public RequestTypeEvent(ILogger<RequestTypeEvent> logger,
            NozomiDbContext context) : base(logger, context)
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