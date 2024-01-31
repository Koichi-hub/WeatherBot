using Core.Enums;

namespace Core.Entities
{
    public class Ticket : BaseEntity
    {
        public int Id { get; set; }

        public string Value { get; set; } = null!;

        public long? SessionId { get; set; }

        public Session? Session { get; set; }

        public bool IsActivated { get; set; }

        public WeatherTariff WeatherTariff { get; set; }
    }
}
