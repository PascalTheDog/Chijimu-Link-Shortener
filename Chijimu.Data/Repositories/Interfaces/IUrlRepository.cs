using Chijimu.Data.Models;

namespace Chijimu.Data.Repositories.Interfaces;

public interface IUrlRepository
{
    public Task<Url> AddUrlAsync(Url url);

    public Task<Url?> GetByFullUrlAsync(string url);

    public Task<Url?> GetByShortUrlAsync(string shortUrlIdentifier);
}