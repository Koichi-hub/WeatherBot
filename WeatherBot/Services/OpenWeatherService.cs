using Core.Models;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using WeatherBot.Helpers;
using WeatherBot.Models;

namespace WeatherBot.Services
{
    public class OpenWeatherService : IWeatherService
    {
        private readonly static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly HttpClient httpClient;
        private readonly OpenWeatherSettings openWeatherSettings;

        public OpenWeatherService(
            HttpClient httpClient,
            IOptions<OpenWeatherSettings> openWeatherSettings
        )
        {
            this.openWeatherSettings = openWeatherSettings.Value;
            this.httpClient = httpClient;
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
            catch (Exception ex)
            {
                logger.Error(ex, "GetWeather exception");
                return new OpenWeatherResponse();
            }
        }

        private async Task<OpenWeatherResponse> DoRequestToOpenWeather(string city)
        {
            var queryParams = new List<KeyValuePair<string, StringValues>>
            {
                new ("q", new string[] { city, "ru" }),
                new ("appid", openWeatherSettings.AppId),
                new ("lang", openWeatherSettings.Lang),
                new ("units", openWeatherSettings.Units),
            };

            var endpoint = QueryHelper.AddQueryParams($"{openWeatherSettings.Uri}/weather", queryParams);

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(endpoint),
                Method = HttpMethod.Get
            };

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                logger.Warn($"GetWeather response status = {0}", response.StatusCode);
                return new OpenWeatherResponse();
            }

            var content = await response.Content.ReadAsStringAsync();
            var openWeatherResponse = JsonConvert.DeserializeObject<OpenWeatherResponse>(content);

            if (openWeatherResponse is null)
            {
                logger.Warn($"GetWeather openWeatherResponse is null");
                return new OpenWeatherResponse();
            }

            return openWeatherResponse;
        }

        private static Weather MapToWeather(OpenWeatherResponse openWeatherResponse)
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
