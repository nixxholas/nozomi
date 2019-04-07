using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.Ticker;
using Nozomi.Preprocessing;
using Nozomi.Repo.Data;
using Nozomi.Service;
using Nozomi.Ticker.Areas;
using Swashbuckle.AspNetCore.Swagger;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class StaticStartup
    {
        public static void ConfigureStatics(this IApplicationBuilder app)
        {
//            using (var serviceScope = app.ApplicationServices
//                .GetRequiredService<IServiceScopeFactory>()
//                .CreateScope())
//            {
//                using (var context = serviceScope.ServiceProvider.GetService<NozomiDbContext>())
//                {
//                }
//            }
        }
    }
}