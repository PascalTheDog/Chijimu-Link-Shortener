using Chijimu.API.Services.Interfaces;
using Chijimu.Data.Repositories.Interfaces;
using System.Text.RegularExpressions;
using ApiModels = Chijimu.API.Models;
using DataModels = Chijimu.Data.Models;

namespace Chijimu.API.Services;

public partial class UrlService : IUrlService
{
    private readonly IHttpContextAccessor _httpContextAccess;
    private readonly ILogger<UrlService> _logger;
    private static readonly Random _rng = new();
    private static readonly char[] _shortUrlCharacters = GetShortUrlCharacters();
    private static readonly Regex _shortUrlIdRegex = ShortUrlIdRegex();
    private static readonly Regex _urlRegex = UrlRegex();
    private readonly IUrlRepository _urlRepository;

    public UrlService(ILogger<UrlService> logger, IUrlRepository urlRepository, IHttpContextAccessor httpContextAccess)
    {
        _httpContextAccess = httpContextAccess;
        _logger = logger;
        _urlRepository = urlRepository;
    }

    private static string ConvertToUrlPart(int intToConvert)
    {
        int possibleValueCount = _shortUrlCharacters.Length;

        string urlPart;

        if (intToConvert >= possibleValueCount)
        {
            urlPart = ConvertToUrlPart(intToConvert / possibleValueCount);
            urlPart += _shortUrlCharacters[intToConvert % possibleValueCount];
        }
        else
        {
            urlPart = _shortUrlCharacters[intToConvert].ToString();
        }

        return urlPart;
    }

    private static string GenerateShortUrl()
    {
        DateTime currentTime = DateTime.UtcNow;

        int randomNumber = _rng.Next(0, _shortUrlCharacters.Length - 1);

        string shortUrl = ConvertToUrlPart(randomNumber);
        shortUrl += ConvertToUrlPart(currentTime.Year % 100);
        shortUrl += ConvertToUrlPart(currentTime.Month);
        shortUrl += ConvertToUrlPart(currentTime.Day);
        shortUrl += ConvertToUrlPart(currentTime.Hour);
        shortUrl += ConvertToUrlPart(currentTime.Minute);
        shortUrl += ConvertToUrlPart(currentTime.Second);
        shortUrl += ConvertToUrlPart(currentTime.Millisecond / 10);

        return shortUrl;
    }

    public async Task<ApiModels.Url?> GetByShortUrlIdentifierAsync(string shortenedUrl)
    {
        _logger.LogTrace("[{method}({param})]", nameof(GetByShortUrlIdentifierAsync), shortenedUrl);

        if (string.IsNullOrEmpty(shortenedUrl))
        {
            throw new ArgumentNullException(nameof(shortenedUrl));
        }

        if (!IsValidShortUrlId(shortenedUrl))
        {
            string errorMessage = "The provided string is not a valid short URL.";
            _logger.LogError("[{method}]: {message}", nameof(GetByShortUrlIdentifierAsync), errorMessage);
            throw new ArgumentException(errorMessage, nameof(shortenedUrl));
        }

        DataModels.Url? urlData = await _urlRepository.GetByShortUrlIdentifierAsync(shortenedUrl);

        ApiModels.Url? url = urlData != null
            ? new()
            {
                FullUrl = urlData.FullUrl,
                ShortUrlBase = GetShortUrlBase(),
                ShortUrlIdentifier = urlData.ShortUrlIdentifier
            }
            : null;

        return url;
    }

    private string GetShortUrlBase()
    {
        HttpContext? httpContext = _httpContextAccess.HttpContext;

        string urlBase;

        if (httpContext != null)
        {
            HttpRequest req = httpContext.Request;
            urlBase = $"{req.Scheme}://{req.Host}/";
        }
        else
        {
            urlBase = string.Empty;
        }

        return urlBase;
    }

    private static char[] GetShortUrlCharacters()
    {
        IEnumerable<int> numbers = Enumerable.Range('0', 10);
        IEnumerable<int> upperLetters = Enumerable.Range('A', 26);
        IEnumerable<int> lowerLetters = Enumerable.Range('a', 26);
        IEnumerable<int> symbols = new int[] { '-', '_' };

        char[] urlCharacters = numbers
            .Concat(upperLetters)
            .Concat(lowerLetters)
            .Concat(symbols)
            .Select(c => (char)c)
            .ToArray();

        return urlCharacters;
    }

    private bool IsValidShortUrlId(string shortUrlId)
    {
        return ShortUrlIdRegex().IsMatch(shortUrlId);
    }

    private bool IsValidUrl(string url)
    {
        return UrlRegex().IsMatch(url);
    }

    public async Task<ApiModels.Url?> ShortenUrlAsync(string url)
    {
        _logger.LogTrace("[{method}({param})]", nameof(ShortenUrlAsync), url);

        if (string.IsNullOrEmpty(url))
        {
            throw new ArgumentNullException(nameof(url));
        }

        if (!IsValidUrl(url))
        {
            string errorMessage = "The provided string was not a valid URL.";
            _logger.LogError("[{method}]: {message}", nameof(ShortenUrlAsync), errorMessage);
            throw new ArgumentException(errorMessage, nameof(url));
        }

        DataModels.Url? existingUrl = await _urlRepository.GetByFullUrlAsync(url);

        DataModels.Url? savedUrl;

        if (existingUrl == null)
        {
            DataModels.Url urlToAdd = new()
            {
                FullUrl = url,
                ShortUrlIdentifier = GenerateShortUrl()
            };

            savedUrl = await _urlRepository.AddUrlAsync(urlToAdd);
        }
        else
        {
            savedUrl = existingUrl;
        }

        ApiModels.Url? returnUrl = savedUrl != null
            ? new()
            {
                FullUrl = savedUrl.FullUrl,
                ShortUrlBase = GetShortUrlBase(),
                ShortUrlIdentifier = savedUrl.ShortUrlIdentifier
            }
            : null;

        return returnUrl;
    }

    [GeneratedRegex("^[a-zA-Z0-9-_]{1,10}?")]
    private static partial Regex ShortUrlIdRegex();

    [GeneratedRegex("^https?://[A-Za-z0-9\\-]{1,63}(\\.[A-Za-z0-9\\-]{1,63}){0,253}(/.*?)??$")]
    private static partial Regex UrlRegex();
}