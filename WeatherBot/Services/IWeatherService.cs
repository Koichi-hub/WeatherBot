using WeatherBot.Models;

namespace WeatherBot.Services
{
    public interface IWeatherService
    {
        public Task<Weather> GetWeather(string city);
    }
}
