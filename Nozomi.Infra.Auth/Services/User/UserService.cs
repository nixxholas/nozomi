using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.Auth.ViewModels.Account;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.BCL.Repository;

namespace Nozomi.Infra.Auth.Services.User
{
    public class UserService : BaseService<UserService, AuthDbContext>, IUserService
    {
        private readonly PasswordHasher<Base.Auth.Models.User> _hasher;
        private readonly UserManager<Base.Auth.Models.User> _userManager;
        
        public UserService(ILogger<UserService> logger, IUnitOfWork<AuthDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public UserService(IHttpContextAccessor contextAccessor, ILogger<UserService> logger, 
            IUnitOfWork<AuthDbContext> unitOfWork) 
            : base(contextAccessor, logger, unitOfWork)
        {
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
                    if (user.UserClaims.Any(uc => uc.ClaimType.Equals(uClaim.Key)))
                    {
                        var existingClaim = user.UserClaims
                            .SingleOrDefault(uc => uc.ClaimType.Equals(uClaim.Key));

                        if (existingClaim != null)
                            existingClaim.ClaimValue = uClaim.Value;
                    }
                    else
                    {
                        user.UserClaims.Add(new UserClaim
                        {
                            UserId = userId,
                            ClaimType = uClaim.Key,
                            ClaimValue = uClaim.Value
                        });
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