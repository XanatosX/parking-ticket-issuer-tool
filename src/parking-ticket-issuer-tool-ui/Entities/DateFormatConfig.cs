using System.Text.Json.Serialization;

public record DateFormatConfig
{
    [JsonPropertyName("id")]
    public string Id { get; init; }

    [JsonPropertyName("format")]
    public string Format { get; init; }

    [JsonPropertyName("display_name")]
    public string DisplayName { get; init; }

    [JsonPropertyName("fallback_display_name")]
    public string FallbackDisplayName { get; init; }

    public static DateFormatConfig Fallback { get; } = new("FALLBACK", "{DAY}.{MONTH}.{YEAR}", "Fallback", "Fallback");

    [JsonConstructor]
    public DateFormatConfig(string id, string format, string displayName, string fallbackDisplayName)
    {
        Id = id;
        Format = format;
        DisplayName = displayName;
        FallbackDisplayName = fallbackDisplayName;
    }
}