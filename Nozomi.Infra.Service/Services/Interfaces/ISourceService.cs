﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Source;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ISourceService
    {
        NozomiResult<string> Create(CreateSource createSource, string userId = null);

        bool Update(UpdateSource updateSource);

        bool StaffSourceUpdate(UpdateSource updateSource);

        bool Delete(long id, bool hardDelete = false, string userId = null);
    }
}
