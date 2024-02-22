using Chijimu.API.Models;
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

    [HttpGet("get-by-short-url/{shortUrlIdentifier}", Name = "GetByShortUrlIdentifier")]
    public async Task<IActionResult> GetByShortUrlIdentifierAsync(string shortUrlIdentifier)
    {
        Url? url = await _urlService.GetByShortUrlIdentifierAsync(shortUrlIdentifier);

        return Ok(url);
    }

    [HttpPost("shorten")]
    public async Task<IActionResult> ShortenAsync([FromBody]string url)
    {
        Url? addedUrl = await _urlService.ShortenUrlAsync(url);

        return CreatedAtAction("GetByShortUrlIdentifier", new { addedUrl?.ShortUrlIdentifier }, addedUrl);
    }
}