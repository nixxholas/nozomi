using Counter.SDK.SharedModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nozomi.Data.WebModels
{
    public class RequestComponent : BaseEntityModel
    {
        public long Id { get; set; }
        
        public string QueryComponent { get; set; }

        public string Value { get; set; }

        public long RequestId { get; set; }
        public Request Request { get; set; }
    }
}
