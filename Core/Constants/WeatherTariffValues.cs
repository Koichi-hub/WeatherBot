using Core.Enums;

namespace Core.Constants
{
    public static class WeatherTariffValues
    {
        public static readonly List<WeatherTariff> AvailableWeatherTariffs = new()
        {
            WeatherTariff.Guest, WeatherTariff.Client, WeatherTariff.Vip
        };

        public static readonly Dictionary<WeatherTariff, int> Limits = new()
        {
            { WeatherTariff.Guest, 5 },
            { WeatherTariff.Client, 100 },
            { WeatherTariff.Vip, 500 }
        };
    }
}
