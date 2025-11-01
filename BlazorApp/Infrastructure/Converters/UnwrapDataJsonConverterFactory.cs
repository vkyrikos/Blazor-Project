using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorApp.Infrastructure.Converters;

public sealed class UnwrapDataJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (typeToConvert == typeof(string)) return false;
        if (typeToConvert.IsPrimitive) return false;

        return typeToConvert.IsClass;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var inner = new JsonSerializerOptions(options);
        for (int i = inner.Converters.Count - 1; i >= 0; i--)
        {
            if (inner.Converters[i] is UnwrapDataJsonConverterFactory)
                inner.Converters.RemoveAt(i);
        }

        var converterType = typeof(UnwrapDataJsonConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType, inner)!;
    }
}
