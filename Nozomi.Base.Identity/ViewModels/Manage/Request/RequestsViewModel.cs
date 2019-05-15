using System.Collections.Generic;
using Nozomi.Data.AreaModels.v1.Requests;

namespace Nozomi.Base.Identity.ViewModels.Manage.Request
{
    public class RequestsViewModel
    {
        public ICollection<RequestDTO> Requests { get; set; }
    }
}