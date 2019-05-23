using System;

namespace Nozomi.Data.AreaModels.v1.Requests
{
    public class UpdateRequest : CreateRequest
    {
        public long Id { get; set; }

        public bool IsEnabled { get; set; }
    }
}