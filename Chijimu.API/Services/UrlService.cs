using Chijimu.API.Services.Interfaces;
using Chijimu.Data.Models;
using Chijimu.Data.Repositories.Interfaces;

namespace Chijimu.API.Services;

public class UrlService : IUrlService
{
    private readonly ILogger<UrlService> _logger;
    private static readonly Random _rng = new();
    private static readonly char[] _shortUrlCharacters = GetShortUrlCharacters();
    private readonly IUrlRepository _urlRepository;

    public UrlService(ILogger<UrlService> logger, IUrlRepository urlRepository)
    {
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
        shortUrl += ConvertToUrlPart(currentTime.Year);
        shortUrl += ConvertToUrlPart(currentTime.Month);
        shortUrl += ConvertToUrlPart(currentTime.Day);
        shortUrl += ConvertToUrlPart(currentTime.Hour);
        shortUrl += ConvertToUrlPart(currentTime.Minute);
        shortUrl += ConvertToUrlPart(currentTime.Second);
        shortUrl += ConvertToUrlPart(currentTime.Millisecond);

        return shortUrl;
    }

    public async Task<string?> GetFullUrlAsync(string shortenedUrl)
    {
        _logger.LogTrace("[{method}({param})]", nameof(GetFullUrlAsync), shortenedUrl);

        if (string.IsNullOrEmpty(shortenedUrl))
        {
            throw new ArgumentNullException(nameof(shortenedUrl));
        }

        Url? url = await _urlRepository.GetByShortUrlAsync(shortenedUrl);

        return url?.FullUrl;
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

    public async Task<string?> ShortenUrlAsync(string url)
    {
        _logger.LogTrace("[{method}({param})]", nameof(ShortenUrlAsync), url);

        if (string.IsNullOrEmpty(url))
        {
            throw new ArgumentNullException(nameof(url));
        }

        Url? existingUrl = await _urlRepository.GetByFullUrlAsync(url);

        Url? savedUrl;

        if (existingUrl == null)
        {
            Url urlToAdd = new()
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

        return savedUrl.ShortUrlIdentifier;
    }
}