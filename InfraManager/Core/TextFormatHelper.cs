using System;

namespace InfraManager.Core
{
    public static class TextFormatHelper
    {
        public static bool TryParse(string text, out TextFormat textFormat)
        {
            textFormat = TextFormat.PlainText;
            if (text == null)
                return false;
            if (!Enum.IsDefined(typeof(TextFormat), text))
                return false;
            else
                textFormat = (TextFormat)Enum.Parse(typeof(TextFormat), text);
            return true;
        }

        public static TextFormat Parse(string text)
        {
            return (TextFormat)Enum.Parse(typeof(TextFormat), text);
        }

        public static TextFormat ParseOrDefault(string text)
        {
            TextFormat result;
            if (!TryParse(text, out result))
                return TextFormat.PlainText;
            else
                return result;
        }
    }
}
