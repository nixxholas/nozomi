using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Nozomi.Base.Identity.Models.Identity;

namespace Nozomi.Service.Identity.Stores
{
    public class NozomiRoleStore : RoleStoreBase<Role, long, UserRole, RoleClaim>
    {
        public NozomiRoleStore(IdentityErrorDescriber describer) : base(describer)
        {
        }

        public override Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override Task<Role> FindByIdAsync(string id, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override Task<Role> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override Task<IList<Claim>> GetClaimsAsync(Role role, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override Task AddClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override Task RemoveClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override IQueryable<Role> Roles { get; }
    }
}