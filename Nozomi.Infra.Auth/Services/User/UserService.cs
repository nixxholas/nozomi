using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.Auth.ViewModels.Account;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.Events.Interfaces;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.BCL.Repository;

namespace Nozomi.Infra.Auth.Services.User
{
    public class UserService : BaseService<UserService, AuthDbContext>, IUserService
    {
        private readonly IEmailSender _emailSender;
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
                            user.Email = uClaim.Value;
                            user.NormalizedEmail = uClaim.Value.ToUpper();
                            
                            if (user.EmailConfirmed)
                                user.EmailConfirmed = false;
                            break;
                        case "name":
                        case "preferred_username":
                            // Update the username
                            user.UserName = uClaim.Value;
                            user.NormalizedUserName = uClaim.Value.ToUpper();
                            break;
                        // TODO: Anything below here, we need to re-evaluate if this is solid
                        case JwtClaimTypes.AuthenticationMethod:
                        case JwtClaimTypes.AuthenticationTime:
                        case JwtClaimTypes.StateHash:
                        case JwtClaimTypes.SessionId:
                        case JwtClaimTypes.Subject:
                            // Ignore these properties
                            break;
                        default:
                            // Update the claims if it already exists
                            if (user.UserClaims.Any(uc => uc.ClaimType.Equals(uClaim.Key)))
                            {
                                var existingClaim = user.UserClaims
                                    .SingleOrDefault(uc => uc.ClaimType.Equals(uClaim.Key));

                                if (existingClaim != null)
                                    existingClaim.ClaimValue = uClaim.Value;
                            }
                            else
                            {
                                // Add it since it's not up yet
                                user.UserClaims.Add(new UserClaim
                                {
                                    UserId = userId,
                                    ClaimType = uClaim.Key,
                                    ClaimValue = uClaim.Value
                                });
                            }

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
    }
}