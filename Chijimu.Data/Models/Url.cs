using System.ComponentModel.DataAnnotations;

namespace Chijimu.Data.Models;

public class Url
{
    [Required]
    public string? FullUrl { get; set; }

    [Key]
    public int UrlId { get; set; }

    [Required]
    public string? ShortUrlIdentifier { get; set; }
}