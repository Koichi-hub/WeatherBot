namespace Infrastructure.Settings
{
    public class AppSettings
    {
        public const string SectionName = "AppSettings";

        public string BotToken { get; set; } = null!;

        public long AdminId { get; set; }

        public string ConnectionString { get; set; } = null!;
    }
}
