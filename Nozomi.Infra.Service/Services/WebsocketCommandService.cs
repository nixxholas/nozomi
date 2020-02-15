using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.WebsocketCommand;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class WebsocketCommandService : BaseService<WebsocketCommandService, NozomiDbContext>, 
        IWebsocketCommandService
    {
        public WebsocketCommandService(ILogger<WebsocketCommandService> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public WebsocketCommandService(IHttpContextAccessor contextAccessor, ILogger<WebsocketCommandService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(contextAccessor, logger, unitOfWork)
        {
        }

        public void Create(CreateWebsocketCommandInputModel vm, string userId)
        {
            throw new System.NotImplementedException();
        }

        public void Update(UpdateWebsocketCommandInputModel vm, string userId)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(string commandGuid, string userId, bool hardDelete = true)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(long commandId, string userId, bool hardDelete = true)
        {
            throw new System.NotImplementedException();
        }
    }
}