using Inframanager.BLL;
using Newtonsoft.Json;
using System;

namespace InfraManager.WebAPIClient
{
    public abstract class NullablePropertyConverter<T> : JsonConverter<NullablePropertyWrapper<T>> where T : struct
    {
        public override NullablePropertyWrapper<T> ReadJson(JsonReader reader, Type objectType, NullablePropertyWrapper<T> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = reader.Value?.ToString();

            if (string.IsNullOrWhiteSpace(value))
            {
                return new NullablePropertyWrapper<T> { IsEmpty = true };
            }

            if (TryParse(value, out var result))
            {
                return new NullablePropertyWrapper<T> { Value = result };
            }

            throw new JsonReaderException($"{reader} is not a valid Guid");
        }

        public override void WriteJson(JsonWriter writer, NullablePropertyWrapper<T> value, JsonSerializer serializer)
        {
            writer.WriteValue(value.IsEmpty ? null : value.Value.ToString());
        }

        protected abstract bool TryParse(string value, out T result);
    }

    /// <summary>
    /// Этот JSON конвертор предназначен для десериализации свойств типа NullablePropertyWrapper<Guid> 
    /// Ожидаемый формат входных данных - Guid строка
    /// </summary>
    public class NullableGuidPropertyConverter : NullablePropertyConverter<Guid>
    {
        protected override bool TryParse(string value, out Guid result)
        {
            return Guid.TryParse(value, out result);
        }
    }

    /// <summary>
    /// Этот JSON конвертор предназначен для десириализации свойств типа NullablePropertyWrapper<int>
    /// Ожидаемый формат входных данных - int (строка)
    /// </summary>
    public class NullableIntPropertyConverter : NullablePropertyConverter<int>
    {
        protected override bool TryParse(string value, out int result)
        {
            return int.TryParse(value, out result);
        }
    }
}
