using Nozomi.Data.ViewModels.WebsocketCommand;

namespace Nozomi.Service.Services.Interfaces
{
    public interface IWebsocketCommandService
    {
        void Create(CreateWebsocketCommandInputModel vm, string userId = null);

        void Update(UpdateWebsocketCommandInputModel vm, string userId = null);

        void Delete(string commandGuid, string userId, bool hardDelete = true);
        
        void Delete(long commandId, string userId, bool hardDelete = true);
    }
}