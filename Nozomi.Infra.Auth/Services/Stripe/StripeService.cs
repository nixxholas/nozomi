using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.BCL.Repository;

namespace Nozomi.Infra.Auth.Services.Stripe
{
    public class StripeService : BaseService<StripeService, AuthDbContext>, IStripeService
    {
        public StripeService(ILogger<StripeService> logger, IUnitOfWork<AuthDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public StripeService(IHttpContextAccessor contextAccessor, ILogger<StripeService> logger, 
            IUnitOfWork<AuthDbContext> unitOfWork) : base(contextAccessor, logger, unitOfWork)
        {
        }


        public void addCard(string stripeCardTokenId, string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}