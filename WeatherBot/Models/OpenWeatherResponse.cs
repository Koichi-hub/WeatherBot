using System.Text.Json.Serialization;

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
        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }

        [JsonPropertyName("temp_min")]
        public double Min { get; set; }

        [JsonPropertyName("temp_max")]
        public double Max { get; set; }
    }
}
