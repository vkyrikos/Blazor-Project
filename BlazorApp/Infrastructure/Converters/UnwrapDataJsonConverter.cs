using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorApp.Infrastructure.Converters;

public sealed class UnwrapDataJsonConverter<T> : JsonConverter<T>
{
    private readonly JsonSerializerOptions _innerOptions;

    public UnwrapDataJsonConverter(JsonSerializerOptions innerOptions)
        => _innerOptions = innerOptions;

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return default;

        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        if (root.ValueKind == JsonValueKind.Object &&
            TryGetPropertyIgnoreCase(root, "data", out var dataProp) &&
            dataProp.ValueKind != JsonValueKind.Null)
        {
            return JsonSerializer.Deserialize<T>(dataProp, _innerOptions);
        }

        return JsonSerializer.Deserialize<T>(root, _innerOptions);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, _innerOptions);
    }

    private static bool TryGetPropertyIgnoreCase(JsonElement obj, string name, out JsonElement value)
    {
        foreach (var prop in obj.EnumerateObject())
        {
            if (string.Equals(prop.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                value = prop.Value;
                return true;
            }
        }
        value = default;
        return false;
    }
}
