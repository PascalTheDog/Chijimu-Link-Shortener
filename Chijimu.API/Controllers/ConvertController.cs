using Chijimu.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chijimu.API;

[ApiController]
[Route("api/v1/[controller]")]
public class ConvertController : ControllerBase
{
    private readonly IConvertService _shortenService;

    public ConvertController(IConvertService shortenService)
    {
        _shortenService = shortenService;
    }

    [HttpGet("get-full-url/{shortenedUrl}")]
    public ActionResult<string> GetFullURL(string shortenedUrl)
    {
        string fullUrl = _shortenService.GetFullURL(shortenedUrl);

        ActionResult<string> result = !string.IsNullOrEmpty(fullUrl)
            ? fullUrl
            : NotFound();

        return result;
    }

    [HttpPost("shorten-url")]
    public ActionResult<string> ShortenURL([FromBody]string url)
    {
        string shortenedUrl = _shortenService.ShortenURL(url);

        return CreatedAtAction(nameof(GetFullURL), new { shortenedUrl }, shortenedUrl);
    }
}