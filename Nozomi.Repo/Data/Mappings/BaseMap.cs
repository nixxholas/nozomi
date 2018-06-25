using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nozomi.Repo.Data.Mappings
{
    public abstract class BaseMap<T> where T : class
    {
        public BaseMap(EntityTypeBuilder<T> entityTypeBuilder)
        {
        }
    }
}
