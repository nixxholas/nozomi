using System.Threading.Tasks;
using Nozomi.Base.Auth.ViewModels.Account;

namespace Nozomi.Infra.Auth.Services.User
{
    public interface IUserService
    {
        Task Update(UpdateUserInputModel vm, string userId);
    }
}