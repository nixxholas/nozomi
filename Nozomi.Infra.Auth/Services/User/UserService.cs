using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.Auth.ViewModels.Account;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.BCL.Repository;

namespace Nozomi.Infra.Auth.Services.User
{
    public class UserService : BaseService<UserService, AuthDbContext>, IUserService
    {
        private readonly UserManager<Base.Auth.Models.User> _userManager;
        
        public UserService(ILogger<UserService> logger, IUnitOfWork<AuthDbContext> unitOfWork,
            UserManager<Base.Auth.Models.User> userManager) 
            : base(logger, unitOfWork)
        {
            _userManager = userManager;
        }

        public UserService(IHttpContextAccessor contextAccessor, ILogger<UserService> logger, 
            IUnitOfWork<AuthDbContext> unitOfWork, UserManager<Base.Auth.Models.User> userManager) 
            : base(contextAccessor, logger, unitOfWork)
        {
            _userManager = userManager;
        }

        public bool HasStripe(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                return _unitOfWork.GetRepository<Base.Auth.Models.User>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(u => u.Id.Equals(userId))
                    .Include(u => u.UserClaims)
                    .Any(u => u.UserClaims != null && u.UserClaims.Count > 0
                              && u.UserClaims.Any(uc => uc.ClaimType
                                  .Equals(NozomiJwtClaimTypes.StripeCustomerId)));
            }
            
            throw new ArgumentNullException($"{_serviceName} HasStripe(): Invalid userId.");
        }

        public Task LinkStripe(string stripeCustId, string userId)
        {
            if (!string.IsNullOrEmpty(stripeCustId) && !string.IsNullOrEmpty(userId))
            {
                var user = _unitOfWork.GetRepository<Base.Auth.Models.User>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(u => u.Id.Equals(userId))
                    .Include(u => u.UserClaims)
                    // Ensure that the user hasn't had stripe propagated for his/her account yet.
                    .SingleOrDefault(u => !u.UserClaims
                        .Any(uc => uc.ClaimType.Equals(NozomiJwtClaimTypes.StripeCustomerId)));

                // Check!
                if (user != null)
                {
                    _unitOfWork.GetRepository<UserClaim>().Add(new UserClaim
                    {
                        UserId = userId,
                        ClaimType = NozomiJwtClaimTypes.StripeCustomerId,
                        ClaimValue = stripeCustId
                    });

                    _unitOfWork.Commit(userId);
                    
                    return Task.CompletedTask;
                }
            }
            
            throw new ArgumentNullException($"{_serviceName} LinkStripe(): Invalid stripeCustId or userId.");
        }

        public async Task Update(UpdateUserInputModel vm, string userId)
        {
            if (vm.IsValid())
            {
                var user = _unitOfWork.GetRepository<Base.Auth.Models.User>()
                    .GetQueryable()
                    .AsTracking()
                    .Where(u => u.Id.Equals(userId))
                    .Include(u => u.UserClaims)
                    .SingleOrDefault();
                
                if (user == null)
                    throw new NullReferenceException("This user does not exist.");

                // Update the password if something is there
                if (!string.IsNullOrEmpty(vm.PreviousPassword))
                {
                    var changePasswordRes = await _userManager.ChangePasswordAsync(user,
                        vm.PreviousPassword, vm.Password);
                    
                    if (!changePasswordRes.Succeeded)
                        throw new Exception("Invalid password changing request. Make sure the previous " +
                                            "password matches and that the new password meets the standard " +
                                            "requirements.");
                    
                    _logger.LogInformation($"[{_serviceName}] Update API: Password updated successfully.");
                }
                
                // Update the claims
                if (user.UserClaims == null || user.UserClaims.Count <= 0)
                    user.UserClaims = new List<UserClaim>();
                foreach (var uClaim in vm.UserClaims)
                {
                    // Switch case every type of claim
                    switch (uClaim.Key)
                    {
                        case "email":
                            // Update the email and prepare to send a confirmation mail
                            user.Email = uClaim.Value.GetString();
                            user.NormalizedEmail = uClaim.Value.GetString().ToUpper();
                            
                            if (user.EmailConfirmed)
                                user.EmailConfirmed = false;
                            break;
                        case JwtClaimTypes.PreferredUserName:
                            // Ensure that we're modifying a new username request
                            if (!string.IsNullOrEmpty(uClaim.Value.GetString())
                                && !user.UserName.Equals(uClaim.Value.GetString())
                                // Dupe checks
                                && !_unitOfWork.GetRepository<Base.Auth.Models.User>().GetQueryable()
                                    .Any(u => u.NormalizedUserName.Equals(uClaim.Value.GetString())))
                            {
                                // Update the username
                                user.UserName = uClaim.Value.GetString();
                                user.NormalizedUserName = uClaim.Value.GetString().ToUpper();
                            }
                            break;
                        case JwtClaimTypes.Name:
                            // Ensure that the given and family name are updated
                            if (user.UserClaims.Any(uc => uc.ClaimValue.Equals(JwtClaimTypes.Name))
                                && vm.UserClaims
                                    .Where(uc => !string.IsNullOrEmpty(uc.Value.GetString()))
                                    .Any(uc => uc.Key.Equals(JwtClaimTypes.GivenName))
                                && vm.UserClaims
                                    .Where(uc => !string.IsNullOrEmpty(uc.Value.GetString()))
                                    .Any(uc => uc.Key.Equals(JwtClaimTypes.FamilyName))
                                && !string.Concat(vm.UserClaims
                                    .SingleOrDefault(uc =>
                                        uc.Key.Equals(JwtClaimTypes.GivenName)).Value.GetString(), vm.UserClaims
                                    .SingleOrDefault(uc =>
                                        uc.Key.Equals(JwtClaimTypes.FamilyName)).Value.GetString()).Equals(user
                                    .UserClaims
                                    .SingleOrDefault(uc => uc.ClaimType.Equals(JwtClaimTypes.Name))
                                    ?.ClaimValue))
                            {
                                // Since it is, update it
                                var name = string.Concat(vm.UserClaims
                                    .SingleOrDefault(uc =>
                                        uc.Key.Equals(JwtClaimTypes.GivenName)).Value.GetString(), vm.UserClaims
                                    .SingleOrDefault(uc =>
                                        uc.Key.Equals(JwtClaimTypes.FamilyName)).Value.GetString());

                                if (!string.IsNullOrEmpty(name))
                                {
                                    var existingClaim = user.UserClaims
                                        .SingleOrDefault(uc => uc.ClaimType.Equals(uClaim.Key));

                                    if (existingClaim != null)
                                        existingClaim.ClaimValue = name;
                                }
                            }
                                
                            break;
                        case JwtClaimTypes.GivenName:
                        case JwtClaimTypes.FamilyName:
                        case JwtClaimTypes.WebSite:
                            // Update the claims if it already exists
                            if (user.UserClaims.Any(uc => uc.ClaimType.Equals(uClaim.Key)))
                            {
                                var existingClaim = user.UserClaims
                                    .SingleOrDefault(uc => uc.ClaimType.Equals(uClaim.Key));

                                if (existingClaim != null)
                                    existingClaim.ClaimValue = uClaim.Value.GetString();
                            }
                            else
                            {
                                // Add it since it's not up yet
                                user.UserClaims.Add(new UserClaim
                                {
                                    UserId = userId,
                                    ClaimType = uClaim.Key,
                                    ClaimValue = uClaim.Value.GetString()
                                });
                            }
                            
                            break;
                        // case JwtClaimTypes.AuthenticationMethod:
                        // case JwtClaimTypes.AuthenticationTime:
                        // case JwtClaimTypes.StateHash:
                        // case JwtClaimTypes.SessionId:
                        // case JwtClaimTypes.Subject:
                        default:
                            _logger.LogInformation($"{_serviceName} Update: Ignoring update for " +
                                                   $"{uClaim.Key} for {userId}");
                            break;
                    }
                }
                
                // Push to DB
                _unitOfWork.GetRepository<Base.Auth.Models.User>().Update(user);
                _unitOfWork.Commit(userId);

                return; // Done
            }
            
            throw new ArgumentNullException("Invalid input/s from UpdateUserInputModel.");
        }
        
        public void AddPaymentMethod(string userId, string paymentMethodId)
        {
            const string methodName = "AddPaymentMethod";
            const string claimType = NozomiJwtClaimTypes.StripeCustomerPaymentMethodId;
            
            if(string.IsNullOrEmpty(userId))
                throw new NullReferenceException($"{_serviceName} {methodName}: user Id is null.");
            
            if(string.IsNullOrEmpty(paymentMethodId))
                throw new NullReferenceException($"{_serviceName} {methodName}: Payment method id is null.");

            CreateUserClaim(userId, claimType, paymentMethodId, methodName);
        }

        public void RemovePaymentMethod(string userId, string paymentMethodId)
        {
            const string methodName = "RemovePaymentMethod";
            const string claimType = NozomiJwtClaimTypes.StripeCustomerPaymentMethodId;

            var paymentMethodClaimToRemove = GetUserClaims(userId, claimType)
                .SingleOrDefault(claim => claim.ClaimValue.Equals(paymentMethodId));
            
            DeleteUserClaim(userId, paymentMethodClaimToRemove, methodName);
        }

        public void SetDefaultPaymentMethod(string userId, string paymentMethodId)
        {
            const string methodName = "SetDefaultPaymentMethod";
            const string claimType = NozomiJwtClaimTypes.StripeCustomerDefaultPaymentId;
            if(string.IsNullOrEmpty(userId))
                throw new ArgumentNullException($"{_serviceName} {methodName}: User Id is null.");
            if(string.IsNullOrEmpty(paymentMethodId))
                throw  new ArgumentNullException($"{_serviceName} {methodName}: Payment Method Id is null.");

            var defaultPaymentMethodClaim = GetUserClaim(userId, claimType);

            if (defaultPaymentMethodClaim == null)
            {
                CreateUserClaim(userId, claimType,paymentMethodId,methodName);
            }
            else
            {
                defaultPaymentMethodClaim.ClaimValue = paymentMethodId;
                UpdateUserClaim(userId, defaultPaymentMethodClaim,methodName);
            }
        }

        public void AddSubscription(string userId, string subscriptionId)
        {
            const string methodName = "AddSubscription";
            const string claimType = NozomiJwtClaimTypes.StripeSubscriptionId;
            const string archiveClaimType = NozomiJwtClaimTypes.PreviousStripeSubscriptionId;

            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException($"{_serviceName} {methodName}: User Id is null.");
            if (string.IsNullOrEmpty(subscriptionId))
                throw new ArgumentNullException($"{_serviceName} {methodName}: Subscription Id is null.");

            var subscriptionClaim = GetUserClaim(userId, claimType);

            if (subscriptionClaim != null)
            {
                CreateUserClaim(userId, archiveClaimType, subscriptionClaim.ClaimValue, methodName);
                subscriptionClaim.ClaimValue = subscriptionId;
                UpdateUserClaim(userId, subscriptionClaim, methodName);
            }
            else 
            {
                CreateUserClaim(userId, claimType, subscriptionId, methodName);
            }
        }

        public void RemoveSubscription(string userId)
        {
            const string methodName = "RemoveSubscription";
            const string claimType = NozomiJwtClaimTypes.StripeSubscriptionId;
            const string archiveClaimType = NozomiJwtClaimTypes.PreviousStripeSubscriptionId;

            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException($"{_serviceName} {methodName}: User Id is null.");

            var subscriptionClaim = GetUserClaim(userId, claimType);

            if (subscriptionClaim == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: Unable to find user {userId} active subscription");
            
            CreateUserClaim(userId, archiveClaimType, subscriptionClaim.ClaimValue, methodName);
            DeleteUserClaim(userId, subscriptionClaim, methodName);
        }

        private UserClaim GetUserClaim(string userId, string claimType)
        {
            return _unitOfWork.GetRepository<UserClaim>().GetQueryable().AsTracking().SingleOrDefault(claim => claim.ClaimType.Equals(claimType) && claim.UserId.Equals(userId));
        }

        private IEnumerable<UserClaim> GetUserClaims(string userId, string claimType)
        {
            return _unitOfWork.GetRepository<UserClaim>().GetQueryable().AsTracking().Where(claim => claim.ClaimType.Equals(claimType) && claim.UserId.Equals(userId));
        }

        private void CreateUserClaim(string userId, string claimType, string claimValue, string methodName)
        {
            var userClaim = new UserClaim
            {
                ClaimType = claimType,
                ClaimValue = claimValue,
                UserId = userId
            };
            
            _unitOfWork.GetRepository<UserClaim>().Add(userClaim);
            if(_unitOfWork.Commit(userId) != 1)
                throw new InvalidOperationException($"{_serviceName} {methodName}: Failed to create user claim");
        }

        private void UpdateUserClaim(string userId, UserClaim userClaim, string methodName)
        {
            _unitOfWork.GetRepository<UserClaim>().Update(userClaim);
            if (_unitOfWork.Commit(userId) != 1)
                throw new InvalidOperationException($"{_serviceName} {methodName}: Failed to update user claim");
        }

        private void DeleteUserClaim(string userId, UserClaim userClaim, string methodName)
        {
            _unitOfWork.GetRepository<UserClaim>().Delete(userClaim);
            if(_unitOfWork.Commit(userId) != 1)
                throw new InvalidOperationException($"{_serviceName} {methodName}: Failed to delete user claim");
        }

        
    }
}