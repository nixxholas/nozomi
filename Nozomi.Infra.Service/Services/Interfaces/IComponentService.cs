﻿using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.RequestComponent;
using Nozomi.Data.ViewModels.Component;

namespace Nozomi.Service.Services.Interfaces
{
    public interface IComponentService
    {
        void Create(CreateComponentInputModel vm, string userId = null);
        
        NozomiResult<string> Create(CreateRequestComponent createRequestComponent, string userId = null);

        bool Checked(long id, string userId = null);

        NozomiResult<string> UpdatePairValue(long id, decimal val);
        NozomiResult<string> UpdatePairValue(long id, string val);

        void Update(UpdateComponentInputModel vm, string userId = null);

        NozomiResult<string> Update(UpdateRequestComponent updateRequestComponent, string userId = null);

        NozomiResult<string> Delete(long id, string userId = null, bool hardDelete = false);
    }
}
