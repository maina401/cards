using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cards.Core.Util;

public class Serializer<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
{
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (Enum.TryParse<TEnum>(reader.GetString(), true, out var enumValue))
        {
            return enumValue;
        }

        throw new JsonException($"Unable to convert '{reader.GetString()}' to enum {typeof(TEnum).Name}.");
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString().ToUpperInvariant());
    }
}