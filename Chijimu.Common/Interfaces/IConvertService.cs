namespace Chijimu.Core.Services.Interfaces;

public interface IConvertService
{
    public string GetFullURL(string shortenedUrl);
    public string ShortenURL(string url);
}