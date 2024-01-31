namespace Infrastructure.Helpers
{
    public static class TicketHelper
    {
        private static readonly object randomLock = new();
        private static readonly Random random = new();
        public const int ValueLength = 10;

        public static string GenerateValue()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            lock (randomLock)
            {
                return new string(Enumerable.Repeat(chars, ValueLength)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }
    }
}
