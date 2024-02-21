using Microsoft.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Chijimu.Services;

public class ChijimuAPIService
{
    private const string _apiUrl = "api/v1";

    private readonly HttpClient _httpClient;
    private readonly ILogger<ChijimuAPIService> _logger;

    public ChijimuAPIService(ILogger<ChijimuAPIService> logger, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _logger = logger;

        InitialiseHTTPClient();
    }

    private void InitialiseHTTPClient()
    {
        _logger.LogTrace("[{method}()]", nameof(InitialiseHTTPClient));

        _httpClient.BaseAddress = new Uri("https://localhost:7242");
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
    }

    public Task<HttpResponseMessage> GetFullURL(string shortenedUrl)
    {
        _logger.LogTrace("[{method}({param})]", nameof(GetFullURL), shortenedUrl);

        return _httpClient.GetAsync($@"{_apiUrl}/convert/get-full-url/{shortenedUrl}");
    }

    public Task<HttpResponseMessage> ShortenURL(string url)
    {
        _logger.LogTrace("[{method}({param})]", nameof(ShortenURL), url);

        string serialisedContent = JsonSerializer.Serialize(url);
        StringContent content = new(serialisedContent, Encoding.UTF8, Application.Json);

        return _httpClient.PostAsync($"{_apiUrl}/convert/shorten-url", content);
    }
}