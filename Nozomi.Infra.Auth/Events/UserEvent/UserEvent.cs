using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Global;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.BCL.Repository;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace Nozomi.Infra.Auth.Events.UserEvent
{
    public class UserEvent : BaseEvent<UserEvent, AuthDbContext>, IUserEvent
    {
        public UserEvent(ILogger<UserEvent> logger, IUnitOfWork<AuthDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public bool HasStripe(string userId)
        {
            const string methodName = "HasStripe";
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException($"{_eventName} {methodName}: Invalid userId.");

            return _unitOfWork.GetRepository<Base.Auth.Models.User>()
                .GetQueryable()
                .AsNoTracking()
                .Where(u => u.Id.Equals(userId))
                .Include(u => u.UserClaims)
                .Any(u => u.UserClaims != null && u.UserClaims.Count > 0
                    && u.UserClaims.Any(uc => uc.ClaimType.Equals(NozomiJwtClaimTypes.StripeCustomerId)));
        }

        public Task AddPaymentMethod(string userId, string paymentMethodId)
        {
            throw new System.NotImplementedException();
        }

        public Task RemovePaymentMethod(string userId, string paymentMethodId)
        {
            throw new System.NotImplementedException();
        }
    }
}