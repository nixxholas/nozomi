using System;
using Nozomi.Data.Models.Web;

namespace Nozomi.Data.ViewModels.RequestProperty
{
    public class UpdateRequestPropertyInputModel : RequestPropertyViewModel
    {
        public UpdateRequestPropertyInputModel(Guid guid, RequestPropertyType type, string key, string value) 
            : base(guid, type, key, value)
        {
        }
    }
}