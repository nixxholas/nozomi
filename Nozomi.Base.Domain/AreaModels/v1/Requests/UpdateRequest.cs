using System;

namespace Nozomi.Data.AreaModels.v1.Requests
{
    public class UpdateRequest: CreateRequest
    {
        public Guid Guid { get; set; }
    }
}