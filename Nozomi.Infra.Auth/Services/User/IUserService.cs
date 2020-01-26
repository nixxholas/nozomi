using System.Threading.Tasks;
using Nozomi.Base.Auth.ViewModels.Account;

namespace Nozomi.Infra.Auth.Services.User
{
    public interface IUserService
    {
        Task LinkStripe(string stripeCustId, string userId);
        
        Task Update(UpdateUserInputModel vm, string userId);
    }
}