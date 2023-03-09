using System.Text.Json.Serialization;

namespace MonkeyWallet.Core.Dto;

public class Output
{
    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("assets")]
    public List<Asset> Assets { get; set; }
}