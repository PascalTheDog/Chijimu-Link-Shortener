using Chijimu.Data.Contexts;
using Chijimu.Data.Models;
using Chijimu.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace Chijimu.Data.Repositories;

public class UrlRepository : IUrlRepository
{
    private readonly ILogger<UrlRepository> _logger;
    private readonly UrlContext _urlContext;

    public UrlRepository(ILogger<UrlRepository> logger, UrlContext urlContext)
    {
        _logger = logger;
        _urlContext = urlContext;
    }

    public async Task<Url> AddUrlAsync(Url url)
    {
        _logger.LogTrace("[{method}({param})]", nameof(AddUrlAsync), url);

        EntityEntry<Url> addedUrl = await _urlContext.Urls.AddAsync(url);
        _ = await _urlContext.SaveChangesAsync();

        return addedUrl.Entity;
    }

    public Task<Url?> GetByFullUrlAsync(string url)
    {
        _logger.LogTrace("[{method}({param})]", nameof(GetByFullUrlAsync), url);

        return _urlContext.Urls
            .AsNoTracking()
            .Where(u => u.FullUrl == url)
            .FirstOrDefaultAsync();
    }

    public Task<Url?> GetByShortUrlAsync(string shortUrlIdentifier)
    {
        _logger.LogTrace("[{method}({param})]", nameof(GetByShortUrlAsync), shortUrlIdentifier);

        return _urlContext.Urls
            .AsNoTracking()
            .Where(u => u.ShortUrlIdentifier == shortUrlIdentifier)
            .FirstOrDefaultAsync();
    }
}