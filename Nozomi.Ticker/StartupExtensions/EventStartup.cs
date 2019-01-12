using Microsoft.Extensions.DependencyInjection;
using Nozomi.Preprocessing.Events;
using Nozomi.Preprocessing.Events.Interfaces;
using Nozomi.Service.Identity.Events;
using Nozomi.Service.Identity.Events.Interfaces;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class EventStartup
    {
        public static void ConfigureEvents(this IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ISmsSender, SmsSender>();
            services.AddTransient<IStripeEvent, StripeEvent>();
        }
    }
}