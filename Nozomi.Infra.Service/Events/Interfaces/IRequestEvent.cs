using Nozomi.Data.Models.Web;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IRequestEvent
    {
        Request GetActive(long id, bool track = false);
    }
}