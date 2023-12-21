using Microsoft.Extensions.Primitives;

namespace WeatherBot.Helpers
{
    public static class QueryHelper
    {
        public static string AddQueryParams(string uri, List<KeyValuePair<string, StringValues>> queryParams)
        {
            var values = queryParams.Select(queryParam => $"{queryParam.Key}={queryParam.Value}");

            return $"{uri}?{string.Join("&", values)}";
        }
    }
}
