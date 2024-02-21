using Chijimu.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Chijimu.Core.Services;

public class ConvertService : IConvertService
{
    private readonly ILogger<ConvertService> _logger;

    public ConvertService(ILogger<ConvertService> logger)
    {
        _logger = logger;
    }

    public string GetFullURL(string shortenedUrl)
    {
        _logger.LogTrace("[{method}({param})]", nameof(GetFullURL), shortenedUrl);

        if (shortenedUrl == null)
        {
            throw new ArgumentNullException(nameof(shortenedUrl));
        }

        return "success";
    }

    public string ShortenURL(string url)
    {
        _logger.LogTrace("[{method}({param})]", nameof(ShortenURL), url);

        if (string.IsNullOrEmpty(url))
        {
            throw new ArgumentNullException(nameof(url));
        }

        return "success";
    }
}