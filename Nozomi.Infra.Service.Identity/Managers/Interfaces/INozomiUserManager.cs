using System.Threading.Tasks;
using Nozomi.Base.Identity.Models.Identity;

namespace Nozomi.Service.Identity.Managers.Interfaces
{
    public interface INozomiUserManager
    {
        Task ForceConfirmEmail(long userId);
        Task ForceConfirmEmail(User user);
    }
}