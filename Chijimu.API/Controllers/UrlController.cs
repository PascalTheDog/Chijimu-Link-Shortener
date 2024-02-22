using Chijimu.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chijimu.API;

[ApiController]
[Route("api/v1/[controller]")]
public class UrlController : ControllerBase
{
    private readonly IUrlService _urlService;

    public UrlController(IUrlService urlService)
    {
        _urlService = urlService;
    }

    [HttpGet("get-full-url/{shortenedUrl}")]
    public async Task<ActionResult<string>> GetFullUrl(string shortenedUrl)
    {
        string? fullUrl = await _urlService.GetFullUrlAsync(shortenedUrl);

        ActionResult<string> result = !string.IsNullOrEmpty(fullUrl)
            ? fullUrl
            : NotFound();

        return result;
    }

    [HttpPost("shorten")]
    public async Task<ActionResult<string>> ShortenAsync([FromBody]string url)
    {
        string? shortenedUrl = await _urlService.ShortenUrlAsync(url);

        return CreatedAtAction(nameof(GetFullUrl), new { shortenedUrl }, shortenedUrl);
    }
}