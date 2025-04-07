using System;
using InfraManager.DAL.FormBuilder;
using Newtonsoft.Json;

namespace InfraManager.WebAPIClient;

public class FieldTypesPropertyConverter : JsonConverter<FieldTypes>
{
    public override void WriteJson(JsonWriter writer, FieldTypes value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }

    public override FieldTypes ReadJson(JsonReader reader, Type objectType, FieldTypes existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var value = reader.Value?.ToString();

        if (string.IsNullOrWhiteSpace(value) || !Enum.TryParse<FieldTypes>(value, out var type))
        {
            throw new JsonReaderException($"{reader} is not a valid FieldTypes.");
        }

        return type;
    }
}