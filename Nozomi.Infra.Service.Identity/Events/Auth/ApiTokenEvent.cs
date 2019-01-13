using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.ViewModels.Identity;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Identity.Data;
using Nozomi.Service.Identity.Events.Auth.Interfaces;

namespace Nozomi.Service.Identity.Events.Auth
{
    public class ApiTokenEvent : BaseEvent<ApiTokenEvent, NozomiAuthContext>, IApiTokenEvent
    {
        private IDistributedCache _cache;
        
        public ApiTokenEvent(ILogger<ApiTokenEvent> logger, IUnitOfWork<NozomiAuthContext> unitOfWork,
            IDistributedCache cache) 
            : base(logger, unitOfWork)
        {
            _cache = cache;
        }

        public async Task<ICollection<ApiToken>> ApiTokensByUserId(long userId, bool onlyFunctional = false)
        {
            if (onlyFunctional)
            {
                return _unitOfWork.GetRepository<ApiToken>()
                    .Get(at => at.UserId.Equals(userId) && at.IsEnabled && at.DeletedAt == null).ToList();
            }
            
            return _unitOfWork.GetRepository<ApiToken>()
                .Get(at => at.UserId.Equals(userId)).ToList();
        }

        public bool IsValid(string key)
        {
            return _cache.Get(key).Any();
        }
    }
}