using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Localization
{
    internal class StaticResourceEnumLocalizer<T> : ILocalizeEnum<T>
        where T : struct, Enum
    {
        private readonly ILocalizeText _textLocalizer;

        public StaticResourceEnumLocalizer(ILocalizeText textLocalizer)
        {
            _textLocalizer = textLocalizer;
        }

        public string Localize(T value, string locale)
        {
            return _textLocalizer.Localize(ToText(value), locale);
        }

        public string Localize(T value)
        {
            return _textLocalizer.Localize(ToText(value));
        }

        public Task<string> LocalizeAsync(T value, CancellationToken cancellationToken = default)
        {
            return _textLocalizer.LocalizeAsync(ToText(value), cancellationToken);
        }

        private string ToText(T value)
        {
            return $"{typeof(T).Name}_{Enum.GetName(typeof(T), value)}";
        }
    }
}
