using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Identity.Data;
using Nozomi.Service.Identity.Stores.Interfaces;

namespace Nozomi.Service.Identity.Stores
{
    public class NozomiUserStore : UserStoreBase<User, long, UserClaim, UserLogin, UserToken>, INozomiUserStore
    {
        private readonly IUnitOfWork<NozomiAuthContext> _unitOfWork;
        
        public NozomiUserStore(IdentityErrorDescriber describer,
            IUnitOfWork<NozomiAuthContext> unitOfWork) : base(describer)
        {
            _unitOfWork = unitOfWork;
        }

        public override Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                if (cancellationToken != null)
                    cancellationToken.ThrowIfCancellationRequested();

                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                var userEntity = getUserEntity(user);

                _unitOfWork.GetRepository<User>().Add(userEntity);
                _unitOfWork.Commit();

                return Task.FromResult(IdentityResult.Success);
            }
            catch (Exception ex)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
            }
        }

        public override Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                if (cancellationToken != null)
                    cancellationToken.ThrowIfCancellationRequested();

                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                var userEntity = getUserEntity(user);

                _unitOfWork.GetRepository<User>().Update(userEntity);
                _unitOfWork.Commit();

                return Task.FromResult(IdentityResult.Success);
            }
            catch (Exception ex)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
            }
        }

        public override Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                if (cancellationToken != null)
                    cancellationToken.ThrowIfCancellationRequested();

                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                _unitOfWork.GetRepository<User>().Delete(u => u.Id.Equals(user.Id));
                _unitOfWork.Commit();

                return Task.FromResult(IdentityResult.Success);
            }
            catch (Exception ex)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
            }
        }

        public override Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken = new CancellationToken())
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            if (!long.TryParse(userId, out long id))
                throw new ArgumentOutOfRangeException(nameof(userId), $"{nameof(userId)} is not a valid GUID");

            var userEntity = _unitOfWork.GetRepository<User>().Get(u => u.Id.Equals(id)).SingleOrDefault();

            return Task.FromResult(getApplicationUser(userEntity));
        }

        public override Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = new CancellationToken())
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            var userEntity = _unitOfWork.GetRepository<User>()
                .Get(u => u.NormalizedUserName.Equals(normalizedUserName))
                .SingleOrDefault();

            return Task.FromResult(getApplicationUser(userEntity));
        }

        protected override Task<User> FindUserAsync(long userId, CancellationToken cancellationToken)
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            var userEntity = _unitOfWork.GetRepository<User>()
                .Get(u => u.Id.Equals(userId))
                .SingleOrDefault();

            return Task.FromResult(getApplicationUser(userEntity));
        }

        protected override Task<UserLogin> FindUserLoginAsync(long userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            var userLoginEntity = _unitOfWork.GetRepository<UserLogin>()
                .Get(ul => ul.UserId.Equals(userId) && ul.LoginProvider.Equals(loginProvider) 
                                                    && ul.ProviderKey.Equals(providerKey))
                .SingleOrDefault();

            return Task.FromResult(userLoginEntity);
        }

        protected override Task<UserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            var userLoginEntity = _unitOfWork.GetRepository<UserLogin>()
                .Get(ul => ul.LoginProvider.Equals(loginProvider) && ul.ProviderKey.Equals(providerKey))
                .SingleOrDefault();

            return Task.FromResult(userLoginEntity);
        }

        public override Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken = new CancellationToken())
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            IList<Claim> result = _unitOfWork.GetRepository<UserClaim>().Get(uc => uc.UserId.Equals(user.Id))
                .Select(x => new Claim(x.ClaimType, x.ClaimValue)).ToList();

            return Task.FromResult(result);
        }

        public override Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = new CancellationToken())
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (claims == null)
                throw new ArgumentNullException(nameof(claims));

            var claimEntities = claims.Select(x => getUserClaimEntity(x, user.Id));
            if (claimEntities.Any())
            {
                claimEntities.ToList().ForEach(claimEntity =>
                {
                    _unitOfWork.GetRepository<UserClaim>().Add(claimEntity);
                });

                _unitOfWork.Commit();
            }

            return Task.CompletedTask;
        }

        public override Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (claim == null)
                throw new ArgumentNullException(nameof(claim));

            if (newClaim == null)
                throw new ArgumentNullException(nameof(newClaim));

            var claimEntity = _unitOfWork.GetRepository<UserClaim>().Get(uc => uc.UserId.Equals(user.Id))
                .SingleOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);

            if(claimEntity != null)
            {
                claimEntity.ClaimType = newClaim.Type;
                claimEntity.ClaimValue = newClaim.Value;

                _unitOfWork.GetRepository<UserClaim>().Update(claimEntity);
                _unitOfWork.Commit();
            }

            return Task.CompletedTask;
        }

        public override Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = new CancellationToken())
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (claims == null)
                throw new ArgumentNullException(nameof(claims));

            var userClaimEntities = _unitOfWork.GetRepository<UserClaim>().Get(uc => uc.UserId.Equals(user.Id));
            if (claims.Any())
            {
                claims.ToList().ForEach(claim =>
                {
                    var userClaimEntity = userClaimEntities.SingleOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
                    _unitOfWork.GetRepository<UserClaim>().Delete(uc => uc.Id.Equals(userClaimEntity.Id));
                });

                _unitOfWork.Commit();
            }

            return Task.CompletedTask;
        }

        public override Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = new CancellationToken())
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            if (claim == null)
                throw new ArgumentNullException(nameof(claim));

            // Do not implicitly declare the collection type
            // https://stackoverflow.com/questions/30051725/cannot-implicitly-convert-type-system-collections-ilist-to-system-collections
            IList<User> result = _unitOfWork.GetRepository<UserClaim>()
                .GetQueryable()
                .Include(uc => uc.User)
                .Where(uc => uc.ClaimType.Equals(claim.Type) && uc.ClaimValue.Equals(claim.Value))
                .Select(uc => uc.User)
                .ToList();

            return Task.FromResult(result);
        }

        protected override Task<UserToken> FindTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(loginProvider))
                throw new ArgumentNullException(nameof(loginProvider));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            var userTokenEntity = _unitOfWork.GetRepository<UserToken>()
                .Get(ut => ut.UserId.Equals(user.Id) && ut.LoginProvider.Equals(loginProvider) && ut.Name.Equals(name))
                .SingleOrDefault();

            return Task.FromResult(userTokenEntity);
        }

        public Task ForceConfirmEmailAsync(long userId)
        {
            if (userId <= 0)
                throw new ArgumentNullException(nameof(userId));

            var user = _unitOfWork.GetRepository<User>()
                .Get(u => u.Id.Equals(userId))
                .SingleOrDefault(u => !u.EmailConfirmed);
            
            if (user == null)
                throw new InvalidDataException(nameof(user));

            user.EmailConfirmed = true;
            _unitOfWork.GetRepository<User>().Update(user);
            _unitOfWork.Commit();

            return Task.CompletedTask;
        }

        public Task ForceConfirmEmailAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var userData = _unitOfWork.GetRepository<User>()
                .Get(u => u.Id.Equals(user.Id) 
                          || u.NormalizedEmail.Equals(user.NormalizedEmail)
                          || u.NormalizedEmail.Equals(user.Email.ToUpper()))
                .SingleOrDefault(u => !u.EmailConfirmed);
            
            if (userData == null)
                throw new InvalidDataException(nameof(user));

            userData.EmailConfirmed = true;
            _unitOfWork.GetRepository<User>().Update(userData);
            _unitOfWork.Commit();

            return Task.CompletedTask;
        }

        protected override Task AddUserTokenAsync(UserToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            _unitOfWork.GetRepository<UserToken>().Add(token);
            _unitOfWork.Commit();

            return Task.CompletedTask;
        }

        protected override Task RemoveUserTokenAsync(UserToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            var userTokenEntity = _unitOfWork.GetRepository<UserToken>()
                .Get(ut => ut.UserId.Equals(token.UserId) && ut.LoginProvider.Equals(token.LoginProvider)
                           && ut.Name.Equals(token.Name))
                            .SingleOrDefault();
            if (userTokenEntity != null)
            {
                _unitOfWork.GetRepository<UserToken>().Delete(userTokenEntity);
                _unitOfWork.Commit();
            }

            return Task.CompletedTask;
        }

        public override IQueryable<User> Users { get; }

        public override Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken = new CancellationToken())
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (login == null)
                throw new ArgumentNullException(nameof(login));

            if (string.IsNullOrWhiteSpace(login.LoginProvider))
                throw new ArgumentNullException(nameof(login.LoginProvider));

            if (string.IsNullOrWhiteSpace(login.ProviderKey))
                throw new ArgumentNullException(nameof(login.ProviderKey));

            var loginEntity = new UserLogin
            {
                LoginProvider = login.LoginProvider,
                ProviderDisplayName = login.ProviderDisplayName,
                ProviderKey = login.ProviderKey,
                UserId = user.Id
            };

            _unitOfWork.GetRepository<UserLogin>().Add(loginEntity);
            _unitOfWork.Commit();

            return Task.CompletedTask;
        }

        public override Task RemoveLoginAsync(User user, string loginProvider, string providerKey,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(loginProvider))
                throw new ArgumentNullException(nameof(loginProvider));

            if (string.IsNullOrWhiteSpace(providerKey))
                throw new ArgumentNullException(nameof(providerKey));

            _unitOfWork.GetRepository<UserLogin>().Delete(ul => ul.LoginProvider.Equals(loginProvider) 
                                                                && ul.ProviderKey.Equals(providerKey));
            _unitOfWork.Commit();
            
            return Task.CompletedTask;
        }

        public override Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken = new CancellationToken())
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            IList<UserLoginInfo> result = _unitOfWork.GetRepository<UserLogin>().Get(ul => ul.UserId.Equals(user.Id))
                .Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey, x.ProviderDisplayName))
                .ToList();

            return Task.FromResult(result);
        }

        public override Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = new CancellationToken())
        {
            if (string.IsNullOrWhiteSpace(normalizedEmail))
                throw new ArgumentNullException(nameof(normalizedEmail));

            var userEntity = _unitOfWork.GetRepository<User>().Get(u => u.NormalizedEmail.Equals(normalizedEmail))
                .SingleOrDefault();

            return Task.FromResult(getApplicationUser(userEntity));
        }
        
        #region Private Methods
        private User getUserEntity(User user)
        {
            if (user == null)
                return null;

            var result = new User();
            populateUserEntity(result, user);

            return result;
        }

        private void populateUserEntity(User entity, User user)
        {
            entity.AccessFailedCount = user.AccessFailedCount;
            entity.ConcurrencyStamp = user.ConcurrencyStamp;
            entity.Email = user.Email;
            entity.EmailConfirmed = user.EmailConfirmed;
            entity.Id = user.Id;
            entity.LockoutEnabled = user.LockoutEnabled;
            entity.LockoutEnd = user.LockoutEnd;
            entity.NormalizedEmail = user.NormalizedEmail;
            entity.NormalizedUserName = user.NormalizedUserName;
            entity.PasswordHash = user.PasswordHash;
            entity.PhoneNumber = user.PhoneNumber;
            entity.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            entity.SecurityStamp = user.SecurityStamp;
            entity.TwoFactorEnabled = user.TwoFactorEnabled;
            entity.UserName = user.UserName;
        }

        private User getApplicationUser(User entity)
        {
            if (entity == null)
                return null;

            var result = new User();
            populateApplicationUser(result, entity);

            return result;
        }

        private void populateApplicationUser(User user, User entity)
        {
            user.AccessFailedCount = entity.AccessFailedCount;
            user.ConcurrencyStamp = entity.ConcurrencyStamp;
            user.Email = entity.Email;
            user.EmailConfirmed = entity.EmailConfirmed;
            user.Id = entity.Id;
            user.LockoutEnabled = entity.LockoutEnabled;
            user.LockoutEnd = entity.LockoutEnd;
            user.NormalizedEmail = entity.NormalizedEmail;
            user.NormalizedUserName = entity.NormalizedUserName;
            user.PasswordHash = entity.PasswordHash;
            user.PhoneNumber = entity.PhoneNumber;
            user.PhoneNumberConfirmed = entity.PhoneNumberConfirmed;
            user.SecurityStamp = entity.SecurityStamp;
            user.TwoFactorEnabled = entity.TwoFactorEnabled;
            user.UserName = entity.UserName;
        }

        private UserClaim getUserClaimEntity(Claim value, long userId)
        {
            return value == null
                ? default(UserClaim)
                : new UserClaim
                {
                    ClaimType = value.Type,
                    ClaimValue = value.Value,
                    UserId = userId
                };
        }
        #endregion
    }
}