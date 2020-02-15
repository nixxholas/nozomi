using Nozomi.Data.ViewModels.WebsocketCommand;

namespace Nozomi.Service.Services.Interfaces
{
    public interface IWebsocketCommandService
    {
        void Create(CreateWebsocketCommandInputModel vm, string userId);

        void Update(UpdateWebsocketCommandInputModel vm, string userId);

        void Delete(string commandGuid, string userId, bool hardDelete = true);
        
        void Delete(long commandId, string userId, bool hardDelete = true);
    }
}