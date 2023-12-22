using Core.Enums;

namespace Core.Entities
{
    public class Session : BaseEntity
    {
        public long UserId { get; set; }

        public string? City { get; set; }

        public int WeatherRequestCount { get; set; }

        public WeatherTariff WeatherTariff { get; set; }

        public bool IsBanned { get; set; }
    }
}
