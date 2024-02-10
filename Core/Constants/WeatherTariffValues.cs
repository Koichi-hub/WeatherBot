using Core.Enums;

namespace Core.Constants
{
    public static class WeatherTariffValues
    {
        public const int TotalGuests = 40;
        public const int TotalClients = 20;
        public const int TotalVips = 4;

        public static readonly List<WeatherTariff> AvailableWeatherTariffs = new()
        {
            WeatherTariff.Guest, WeatherTariff.Client, WeatherTariff.Vip
        };

        public static readonly Dictionary<WeatherTariff, int> Limits = new()
        {
            { WeatherTariff.Guest, 5 },
            { WeatherTariff.Client, 20 },
            { WeatherTariff.Vip, 100 }
        };
    }
}
