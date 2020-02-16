using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.WebsocketCommandProperty;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class WebsocketCommandPropertyService : BaseService<WebsocketCommandPropertyService, NozomiDbContext>,
        IWebsocketCommandPropertyService
    {
        public WebsocketCommandPropertyService(ILogger<WebsocketCommandPropertyService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public WebsocketCommandPropertyService(IHttpContextAccessor contextAccessor, 
            ILogger<WebsocketCommandPropertyService> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(contextAccessor, logger, unitOfWork)
        {
        }

        public void Create(CreateWebsocketCommandPropertyInputModel vm, string userId)
        {
            throw new System.NotImplementedException();
        }

        public void Update(UpdateWebsocketCommandPropertyInputModel vm, string userId)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(string propertyGuid, string userId, bool hardDelete = true)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(long propertyId, string userId, bool hardDelete = true)
        {
            throw new System.NotImplementedException();
        }
    }
}