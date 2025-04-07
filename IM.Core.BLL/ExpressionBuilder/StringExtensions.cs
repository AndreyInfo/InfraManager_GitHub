using System.Linq;

namespace InfraManager.BLL.ExpressionBuilder
{
    public static class StringExtensions
    {
        /// <summary>
        /// Проверяет совпадает ли заданный символ с первым символом заданной строки
        /// </summary>
        /// <param name="text">Строка, первый символ которой необходимо проверить</param>
        /// <param name="character">Символ</param>
        /// <returns>Истина, если строка не пустая и начинается с указанного символа, ложь - в противном случае</returns>
        public static bool StartsWith(this string text, char character)
        {
            return !string.IsNullOrEmpty(text) && text[0] == character;
        }

        /// <summary>
        /// Проверяет совпадает ли заданный символ с последним символом заданной строки
        /// </summary>
        /// <param name="text">Строка, последний символ которой необходимо проверить</param>
        /// <param name="character">Символ</param>
        /// <returns>Истина, елси строка не пустая и заканчивается указанным символом, ложь - в противном случае</returns>
        public static bool EndsWith(this string text, char character)
        {
            return !string.IsNullOrEmpty(text) && text[text.Length - 1] == character;
        }

        /// <summary>
        /// Считает количество пробелов в левой части строки
        /// </summary>
        /// <param name="text">Строка с пробелами</param>
        /// <returns>Количество пробелом слева в строке</returns>
        public static int CountLeftSpaces(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return text.Length;
            }

            return text.IndexOf(text.First(c => !char.IsWhiteSpace(c)));

        }

        /// <summary>
        /// Отсекает незначимые символы (пробелы и пара открывающая скобка слева + закрывающая скобка справа) математического выражения слева и справа в строке
        /// </summary>
        /// <param name="text">Текст, который нужно освободить от незначимых крайних символов</param>
        /// <returns></returns>
        public static string MathTrim(this string text)
        {
            return text.Trim()
                .TrimParenthesis()
                .Trim();
        }

        /// <summary>
        /// Убирает открывающую и закрывающую скобку, в которые заключено выражение (если есть)
        /// </summary>
        /// <param name="text">Текст выражения</param>
        /// <returns>Текст выражения без незначчащих скобок</returns>
        public static string TrimParenthesis(this string text)
        {
            if (text.StartsWith(SpecialCharacters.OpenParenthesis) &&
                text.EndsWith(SpecialCharacters.CloseParenthesis))
            {
                return text.CutEnds().Trim().TrimParenthesis();
            }

            return text;
        }

        /// <summary>
        /// Убирает из строки первый и последний символы
        /// </summary>
        /// <param name="text">Исходная строка</param>
        /// <returns>Строка без первого и последнего символов</returns>
        public static string CutEnds(this string text)
        {
            return text.Substring(1, text.Length - 2);
        }
    }
}
