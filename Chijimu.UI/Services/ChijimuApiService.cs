using Microsoft.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Chijimu.UI.Services;

public class ChijimuApiService
{
    private const string _apiUrl = "api/v1";

    private readonly HttpClient _httpClient;
    private readonly ILogger<ChijimuApiService> _logger;

    public ChijimuApiService(ILogger<ChijimuApiService> logger, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _logger = logger;

        InitialiseHttpClient();
    }

    private void InitialiseHttpClient()
    {
        _logger.LogTrace("[{method}()]", nameof(InitialiseHttpClient));

        _httpClient.BaseAddress = new Uri("https://localhost:7242");
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
    }

    public Task<HttpResponseMessage> GetFullUrlAsync(string shortenedUrl)
    {
        _logger.LogTrace("[{method}({param})]", nameof(GetFullUrlAsync), shortenedUrl);

        return _httpClient.GetAsync($@"{_apiUrl}/url/get-full-url/{shortenedUrl}");
    }

    public Task<HttpResponseMessage> ShortenUrlAsync(string url)
    {
        _logger.LogTrace("[{method}({param})]", nameof(ShortenUrlAsync), url);

        string serialisedContent = JsonSerializer.Serialize(url);
        StringContent content = new(serialisedContent, Encoding.UTF8, Application.Json);

        return _httpClient.PostAsync($"{_apiUrl}/url/shorten", content);
    }
}