using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Identity.Data;

namespace Nozomi.Service.Identity.Stores
{
    public class NozomiRoleStore : RoleStoreBase<Role, long, UserRole, RoleClaim>
    {
        private readonly IUnitOfWork<NozomiAuthContext> _unitOfWork;
        
        public NozomiRoleStore(IdentityErrorDescriber describer) : base(describer)
        {
        }

        public override Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                if (cancellationToken != null)
                    cancellationToken.ThrowIfCancellationRequested();

                if (role == null)
                    throw new ArgumentNullException(nameof(role));

                var roleEntity = getRoleEntity(role);

                _unitOfWork.GetRepository<Role>().Add(roleEntity);
                _unitOfWork.Commit();

                return Task.FromResult(IdentityResult.Success);
            }
            catch(Exception ex)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
            }
        }

        public override Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                if (cancellationToken != null)
                    cancellationToken.ThrowIfCancellationRequested();

                if (role == null)
                    throw new ArgumentNullException(nameof(role));

                var roleEntity = getRoleEntity(role);

                _unitOfWork.GetRepository<Role>().Update(roleEntity);
                _unitOfWork.Commit();

                return Task.FromResult(IdentityResult.Success);
            }
            catch (Exception ex)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
            }
        }

        public override Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                if (cancellationToken != null)
                    cancellationToken.ThrowIfCancellationRequested();

                if (role == null)
                    throw new ArgumentNullException(nameof(role));

                _unitOfWork.GetRepository<Role>().Delete(r => r.Id.Equals(role.Id));
                _unitOfWork.Commit();

                return Task.FromResult(IdentityResult.Success);
            }
            catch (Exception ex)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
            }
        }

        public override Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken = new CancellationToken())
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(roleId))
                throw new ArgumentNullException(nameof(roleId));

            if(!long.TryParse(roleId, out var id))
                throw new ArgumentOutOfRangeException(nameof(roleId), $"{nameof(roleId)} is not a valid GUID");

            var roleEntity = _unitOfWork.GetRepository<Role>().Get(r => r.Id.Equals(id)).SingleOrDefault();
            return Task.FromResult(getIdentityRole(roleEntity));
        }

        public override Task<Role> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = new CancellationToken())
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(normalizedName))
                throw new ArgumentNullException(nameof(normalizedName));

            var roleEntity = _unitOfWork.GetRepository<Role>()
                .Get(r => r.NormalizedName.Equals(normalizedName)).SingleOrDefault();
            return Task.FromResult(getIdentityRole(roleEntity));
        }

        public override Task<IList<Claim>> GetClaimsAsync(Role role, CancellationToken cancellationToken = new CancellationToken())
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            if (role == null)
                throw new ArgumentNullException(nameof(role));

            IList<Claim> result = _unitOfWork.GetRepository<RoleClaim>()
                 .Get(r => r.Id.Equals(role.Id))
                .Select(x => new Claim(x.ClaimType, x.ClaimValue)).ToList(); 

            return Task.FromResult(result);
        }

        public override Task AddClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = new CancellationToken())
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            if (role == null)
                throw new ArgumentNullException(nameof(role));

            if (claim == null)
                throw new ArgumentNullException(nameof(claim));

            var roleClaimEntity = new RoleClaim
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                RoleId = role.Id
            };

            _unitOfWork.GetRepository<RoleClaim>().Add(roleClaimEntity);
            _unitOfWork.Commit();

            return Task.CompletedTask;
        }

        public override Task RemoveClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = new CancellationToken())
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            if (role == null)
                throw new ArgumentNullException(nameof(role));

            if (claim == null)
                throw new ArgumentNullException(nameof(claim));

            var roleClaimEntity = _unitOfWork.GetRepository<RoleClaim>()
                .Get(r => r.Id.Equals(role.Id))
                .SingleOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);

            if(roleClaimEntity != null)
            {
                _unitOfWork.GetRepository<RoleClaim>().Delete(r => r.Id.Equals(roleClaimEntity.Id));
                _unitOfWork.Commit();
            }

            return Task.CompletedTask;
        }
        
        #region Private Methods
        private Role getRoleEntity(Role value)
        {
            return value == null
                ? default(Role)
                : new Role
                {
                    ConcurrencyStamp = value.ConcurrencyStamp,
                    Id = value.Id,
                    Name = value.Name,
                    NormalizedName = value.NormalizedName
                };
        }

        private Role getIdentityRole(Role value)
        {
            return value == null
                ? default(Role)
                : new Role
                {
                    ConcurrencyStamp = value.ConcurrencyStamp,
                    Id = value.Id,
                    Name = value.Name,
                    NormalizedName = value.NormalizedName
                };
        }
        #endregion

        public override IQueryable<Role> Roles { get; }
    }
}