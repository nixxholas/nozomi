using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Identity.Data;
using Nozomi.Service.Identity.Events.Auth.Interfaces;

namespace Nozomi.Service.Identity.Events.Auth
{
    public class ApiTokenEvent : BaseEvent<ApiTokenEvent, NozomiAuthContext>, IApiTokenEvent
    {
        private ICollection<ApiToken> _activeTokens;
        
        public ApiTokenEvent(ILogger<ApiTokenEvent> logger, IUnitOfWork<NozomiAuthContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
            _activeTokens = _unitOfWork.GetRepository<ApiToken>()
                .Get(at => at.IsEnabled && at.DeletedAt == null).ToList();
        }

        public async Task<ICollection<ApiToken>> ApiTokensByUserId(long userId, bool onlyFunctional = false)
        {
            if (onlyFunctional)
            {
                return _activeTokens;
            }
            
            return _unitOfWork.GetRepository<ApiToken>()
                .Get(at => at.UserId.Equals(userId)).ToList();
        }
    }
}