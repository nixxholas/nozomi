using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Auth.Services.Address
{
    public class AddressService : BaseService<AddressService, NozomiDbContext>, IAddressService
    {
        public AddressService(ILogger<AddressService> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }
    }
}