using Nozomi.Data.ViewModels.Dispatch;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IDispatchEvent
    {
        DispatchViewModel Dispatch(DispatchInputModel dispatchInputModel);
    }
}