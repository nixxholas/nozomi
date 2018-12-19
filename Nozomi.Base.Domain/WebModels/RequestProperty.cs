using System;
using System.Collections.Generic;
using System.Text;

namespace Nozomi.Data.WebModels
{
    public class RequestProperty : BaseEntityModel
    {
        public long Id { get; set; }

        public RequestPropertyType RequestPropertyType { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public long RequestId { get; set; }
        public Request Request { get; set; }
    }
}
