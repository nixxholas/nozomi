using System.Threading.Tasks;
using Nozomi.Data.ViewModels.RequestProperty;

namespace Nozomi.Service.Services.Interfaces
{
    public interface IRequestPropertyService
    {
        Task Create(CreateRequestPropertyInputModel inputModel, string userId);
        
        Task Update(UpdateRequestPropertyInputModel inputModel, string userId);

        Task Delete(string guid, string userId, bool hardDelete = true);
    }
}