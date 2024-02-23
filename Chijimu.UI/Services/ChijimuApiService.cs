using Chijimu.API.Models;
using Chijimu.UI.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Chijimu.UI.Services;

public class ChijimuApiService
{
    private readonly ApiSettings _apiSettings;
    private readonly HttpClient _httpClient;
    private readonly ILogger<ChijimuApiService> _logger;

    public ChijimuApiService(ILogger<ChijimuApiService> logger, HttpClient httpClient, IOptions<ApiSettings> options)
    {
        _apiSettings = options.Value;
        _httpClient = httpClient;
        _logger = logger;

        InitialiseHttpClient();
    }

    private void InitialiseHttpClient()
    {
        _logger.LogTrace("[{method}()]", nameof(InitialiseHttpClient));

        _httpClient.BaseAddress = new Uri(_apiSettings.ChijimuApi?.UrlBase ?? string.Empty);
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
    }

    public async Task<string?> ShortenUrlAsync(string url)
    {
        _logger.LogTrace("[{method}({param})]", nameof(ShortenUrlAsync), url);

        string serialisedContent = JsonSerializer.Serialize(url);
        StringContent content = new(serialisedContent, Encoding.UTF8, Application.Json);

        HttpResponseMessage response = await _httpClient.PostAsync($"api/v1/url/shorten", content);

        Url? shortenedUrl = response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<Url?>()
            : null;

        return shortenedUrl?.ShortUrl;
    }
}