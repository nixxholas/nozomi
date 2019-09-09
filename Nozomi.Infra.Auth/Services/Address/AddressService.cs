using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.BCL.Repository;

namespace Nozomi.Infra.Auth.Services.Address
{
    public class AddressService : BaseService<AddressService, AuthDbContext>, IAddressService
    {
        public AddressService(ILogger<AddressService> logger, IUnitOfWork<AuthDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }
    }
}