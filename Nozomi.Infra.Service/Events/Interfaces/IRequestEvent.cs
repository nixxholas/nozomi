using System.Collections.Generic;
using Nozomi.Data.AreaModels.v1.Requests;
using Nozomi.Data.Models.Web;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IRequestEvent
    {
        Request GetActive(long id, bool track = false);

        /// <summary>
        /// Select all Requests with a limit of 50.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        ICollection<RequestDTO> GetAllDTO(int index);
    }
}