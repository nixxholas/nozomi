using System.Collections.Generic;
using Nozomi.Web.Models;

namespace Nozomi.Web.Providers
{
    public interface IWeatherProvider
    {
        List<WeatherForecast> GetForecasts();
    }
}
