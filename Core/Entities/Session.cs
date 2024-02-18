using Core.Enums;

namespace Core.Entities
{
    public class Session : BaseEntity
    {
        public long UserId { get; set; }

        public string? City { get; set; }

        public int WeatherRequestCount { get; set; }

        public DateTime DateLastWeatherRequest { get; set; }

        public WeatherTariff WeatherTariff { get; set; }

        public bool IsBanned { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsAdmin { get; set; }

        public string? WaitResponseCommand { get; set; }
    }
}
