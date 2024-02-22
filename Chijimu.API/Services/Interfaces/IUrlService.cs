using Chijimu.API.Models;

namespace Chijimu.API.Services.Interfaces;

public interface IUrlService
{
    public Task<Url?> GetByShortUrlIdentifierAsync(string shortenedUrl);
    public Task<Url?> ShortenUrlAsync(string url);
}