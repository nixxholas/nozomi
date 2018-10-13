using Microsoft.Extensions.DependencyInjection;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class CorsStartup
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
                });
            
        }
    }
}