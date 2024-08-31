using System.Text.Json.Serialization;


namespace Ecommerce.Identity.Api.Options;

public sealed class CookieConfig
{
    public bool Secure { get; set; }

    public bool HttpOnly { get; set; }

    public bool IsEssential { get; set; }

    public string Path { get; set; }

    public string Domain { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SameSiteMode SameSite { get; set; }
}