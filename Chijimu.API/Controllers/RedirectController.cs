using Chijimu.API.Models;
using Chijimu.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chijimu.API.Controllers;

[ApiController]
[Route("")]
public class RedirectController : ControllerBase
{
    private readonly IUrlService _urlService;

    public RedirectController(IUrlService urlService)
    {
        _urlService = urlService;
    }

    [HttpGet("{shortenedUrl}")]
    public async Task<IActionResult> RedirectAsync(string shortenedUrl)
    {
        Url? url = await _urlService.GetByShortUrlIdentifierAsync(shortenedUrl);

        IActionResult result = url?.FullUrl != null
            ? Redirect(url.FullUrl)
            : NotFound();

        return result;
    }
}