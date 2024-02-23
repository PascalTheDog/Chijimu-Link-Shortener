namespace Chijimu.UI.Configuration;

public class ApiSettings
{
    public Api? ChijimuApi { get; set; }

    public static string Name => nameof(ApiSettings);
}