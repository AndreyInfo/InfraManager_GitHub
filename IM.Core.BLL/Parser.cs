

using System;

namespace InfraManager.BLL
{
    /// <summary>
    /// TODO: Реализовать нормальный парсер с параметром типом или найти 3rd party
    /// </summary>
    public class Parser : IParser, ISelfRegisteredService<IParser>
    {
        public T Parse<T>(string s)
        {
            if (string.IsNullOrWhiteSpace(s) || s == "null") // но какой 3rd party учтет, что пустая строка может быть равна "null"
            {
                return default;
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)s;
            }

            if (typeof(T).IsEnum)
            {
                return (T)Enum.Parse(typeof(T), s);
            }

            var parseType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            var parseMethod = parseType.GetMethod("Parse", new[] { typeof(string) })
                ?? throw new ArgumentException($"The type {typeof(T)} is not supported");

            return (T)parseMethod.Invoke(null, new [] { s });
        }
    }
}
