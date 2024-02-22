namespace Chijimu.API.Models;

public class Url
{
    public string? FullUrl { get; set; }

    public string? ShortUrlBase { get; set; }

    public string? ShortUrlIdentifier { get; set; }

    public string? ShortUrl =>
        ShortUrlBase + ShortUrlIdentifier;
}