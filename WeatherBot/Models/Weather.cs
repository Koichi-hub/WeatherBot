namespace WeatherBot.Models
{
    public sealed class Weather
    {
        public string Status { get; set; } = string.Empty;
        public Temperature Temperature { get; set; } = new();
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }

    public sealed class Temperature
    {
        public double FeelsLike { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
    }
}
