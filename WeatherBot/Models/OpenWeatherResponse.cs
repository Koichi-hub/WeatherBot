using Newtonsoft.Json;

namespace WeatherBot.Models
{
    public sealed class OpenWeatherResponse
    {
        public string Name { get; set; } = string.Empty;

        public IList<OpenWeatherResponseWeather> Weather { get; set; } = Enumerable.Empty<OpenWeatherResponseWeather>().ToList();

        public OpenWeatherResponseMain Main { get; set; } = new();

        public OpenWeatherResponseSys Sys { get; set; } = new();
    }

    public sealed class OpenWeatherResponseWeather
    {
        public string Description { get; set; } = string.Empty;
    }

    public sealed class OpenWeatherResponseSys
    {
        public string Country { get; set; } = string.Empty;
    }

    public sealed class OpenWeatherResponseMain
    {
        [JsonProperty("feels_like")]
        public double FeelsLike { get; set; }

        [JsonProperty("temp_min")]
        public double Min { get; set; }

        [JsonProperty("temp_max")]
        public double Max { get; set; }
    }
}
