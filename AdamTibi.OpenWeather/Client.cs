using System.Globalization;
using System.Web;
using Newtonsoft.Json;

namespace Red.OpenWeather;

public class Client : IClient
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;
    private const string BASE_URL = "https://api.openweathermap.org/data/3.0";
    private const string URL_TEMPLATE = "/onecall";

    public Client(string apiKey, HttpClient httpClient)
    {
        _apiKey = apiKey;
        _httpClient = httpClient;
    }

    public async Task<OneCallResponse> OneCallAsync(decimal latitude, decimal longitude, IEnumerable<Excludes> excludes, Units unit, string baseUrl = BASE_URL, string urlTemplate = URL_TEMPLATE)
    {
        var uriBuilder = new UriBuilder(baseUrl + urlTemplate);
        var query = HttpUtility.ParseQueryString("");
        query["lat"] = latitude.ToString(CultureInfo.InvariantCulture);
        query["lon"] = longitude.ToString(CultureInfo.InvariantCulture);
        query["appid"] = _apiKey;
        if (excludes is { } && excludes.Any())
        {
            query["exclude"] = string.Join(',', excludes).ToLower();
        }
        query["units"] = unit.ToString().ToLower();

        uriBuilder.Query = query.ToString();

        var jsonResponse = await _httpClient.GetStringAsync(uriBuilder.Uri.AbsoluteUri);

        if (string.IsNullOrEmpty(jsonResponse))
        {
            throw new InvalidOperationException("No response from the service");
        }

        OneCallResponse? oneCallResponse = JsonConvert.DeserializeObject<OneCallResponse>(jsonResponse);

        if (oneCallResponse is null)
        {
            // This will never be hit
            throw new Exception();
        }

        return oneCallResponse;
    }

}
