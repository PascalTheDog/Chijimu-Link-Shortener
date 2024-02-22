namespace Chijimu.API.Services.Interfaces;

public interface IUrlService
{
    public Task<string?> GetFullUrlAsync(string shortenedUrl);
    public Task<string?> ShortenUrlAsync(string url);
}