using Core.Models;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Net.Http.Json;
using WeatherBot.Helpers;
using WeatherBot.Models;

namespace WeatherBot.Services
{
    public class OpenWeatherService : IWeatherService
    {
        private readonly HttpClient httpClient;
        private readonly OpenWeatherSettings openWeatherSettings;

        public OpenWeatherService(
            HttpClient httpClient,
            IOptions<OpenWeatherSettings> openWeatherSettings
        )
        {
            this.openWeatherSettings = openWeatherSettings.Value;
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri(this.openWeatherSettings.Uri);
        }

        public async Task<Weather> GetWeather(string city)
        {
            var openWeatherResponse = await DoRequest(city);
            return MapToWeather(openWeatherResponse);
        }

        private async Task<OpenWeatherResponse> DoRequest(string city)
        {
            try
            {
                return await DoRequestToOpenWeather(city);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Erorr occurred while request to openweather", e);
            }
        }

        private async Task<OpenWeatherResponse> DoRequestToOpenWeather(string city)
        {
            var queryParams = new List<KeyValuePair<string, StringValues>>
                {
                    new KeyValuePair<string, StringValues>("q", new string[] { city, "ru" }),
                    new KeyValuePair<string, StringValues>("appid", openWeatherSettings.AppId),
                    new KeyValuePair<string, StringValues>("lang", openWeatherSettings.Lang),
                    new KeyValuePair<string, StringValues>("units", openWeatherSettings.Units),
                };

            var endpoint = QueryHelper.AddQueryParams("weather", queryParams);

            var openWeatherResponse = await httpClient.GetFromJsonAsync<OpenWeatherResponse>(endpoint);

            return openWeatherResponse ?? new OpenWeatherResponse();
        }

        private Weather MapToWeather(OpenWeatherResponse openWeatherResponse)
        {
            return new Weather
            {
                Status = openWeatherResponse.Weather.FirstOrDefault()?.Description ?? string.Empty,
                City = openWeatherResponse.Name,
                Country = openWeatherResponse.Sys.Country,
                Temperature =
                {
                    FeelsLike = openWeatherResponse.Main.FeelsLike,
                    Max = openWeatherResponse.Main.Max,
                    Min = openWeatherResponse.Main.Min
                }
            };
        }
    }
}
