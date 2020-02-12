using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.RequestProperty;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class RequestPropertyService : BaseService<RequestPropertyService, NozomiDbContext>, IRequestPropertyService
    {
        public RequestPropertyService(ILogger<RequestPropertyService> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public RequestPropertyService(IHttpContextAccessor contextAccessor, ILogger<RequestPropertyService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(contextAccessor, logger, unitOfWork)
        {
        }

        public Task Create(CreateRequestPropertyInputModel inputModel, string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task Update(UpdateRequestPropertyInputModel inputModel, string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(string guid, string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}