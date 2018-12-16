using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Nozomi.Base.Identity.Models.Identity;

namespace Nozomi.Service.Identity.Stores
{
    public class NozomiUserStore : UserStoreBase<User, long, UserClaim, UserLogin, UserToken>
    {
        public NozomiUserStore(IdentityErrorDescriber describer) : base(describer)
        {
        }

        public override Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        protected override Task<User> FindUserAsync(long userId, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<UserLogin> FindUserLoginAsync(long userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<UserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public override Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        protected override Task<UserToken> FindTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        protected override Task AddUserTokenAsync(UserToken token)
        {
            throw new System.NotImplementedException();
        }

        protected override Task RemoveUserTokenAsync(UserToken token)
        {
            throw new System.NotImplementedException();
        }

        public override IQueryable<User> Users { get; }

        public override Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override Task RemoveLoginAsync(User user, string loginProvider, string providerKey,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }
    }
}