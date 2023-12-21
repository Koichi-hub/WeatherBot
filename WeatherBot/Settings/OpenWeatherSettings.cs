namespace WeatherBot.Settings
{
    public class OpenWeatherSettings
    {
        public const string SectionName = "OpenWeatherSettings";

        public string Uri { get; set; } = null!;

        public string AppId { get; set; } = null!;

        public string Lang { get; set; } = null!;

        public string Units { get; set; } = null!;
    }
}
