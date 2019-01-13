using System.Threading.Tasks;
using Nozomi.Base.Identity.ViewModels.Identity;

namespace Nozomi.Service.Identity.Managers.Interfaces
{
    public interface INozomiUserManager
    {
        Task ForceConfirmEmail(long userId);
        Task ForceConfirmEmail(User user);
    }
}