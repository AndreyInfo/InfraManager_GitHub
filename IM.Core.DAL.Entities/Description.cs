using InfraManager.DAL.ChangeTracking;

namespace InfraManager.DAL
{
    /// <summary>
    /// Эта структура предназначена для хранения данных описания объекта в отформатированном и плоском виде
    /// </summary>
    public class Description : ITrackChangesOf<Description>
    {
        public const int PlainMaxLength = 1000;
        public const int FormattedmaxLength = 4000;

        public Description()
        {
            Plain = string.Empty;
            Formatted = string.Empty;
        }

        public Description(string plain, string formatted, Description originalValue)
        {
            Plain = plain;
            Formatted = formatted;
            _original = originalValue;
        }

        /// <summary>
        /// Возвращает текст описания без форматирования
        /// </summary>
        public string Plain { get; }
        /// <summary>
        /// Возвращает текст описания с форматированием
        /// </summary>
        public string Formatted { get; }

        private Description _original;
        /// <summary>
        /// Возвращает ссылку на значение до изменений
        /// </summary>
        public Description Original => _original ?? this;

        public void SetUnmodified()
        {
            _original = null;
        }
    }
}
