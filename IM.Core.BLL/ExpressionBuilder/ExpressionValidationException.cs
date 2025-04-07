using System;

namespace InfraManager.BLL.ExpressionBuilder
{
    /// <summary>
    /// Этот класс представляет базовое исключение парсинга выражения
    /// </summary>
    public class ExpressionValidationException : Exception
    {
        public static string MissingOpenParenthesis = "MissingOpenParenthesis";
        public static string MissingCloseParenthesis = "MissingCloseParenthesis";
        public static string MissingLeftArgument = "MissingLeftArgument";
        public static string MissingRightArgument = "MissingRightArgument";
        public static string UnknownStatement = "UnknownStatement";
        public static string IncorrectSyntax = "IncorrectSyntax";
        public static string ZeroDivision = "ZeroDivision";
        public static string IncorrectParametersQuantity = "IncorrectParametersQuantity";
        public static string MissingParameter = "MissingParameter";
        public static string EmptyExpression = "EmptyExpression";

        /// <summary>
        /// Создает экземпляр ExpressionValidationException
        /// </summary>
        /// <param name="key">Ключ текста ошибки в ресурсах</param>
        public ExpressionValidationException(string key)
            : this(key, new string[0])
        {
        }

        /// <summary>
        /// Создает экземпляр ExpressionValidationException
        /// </summary>
        /// <param name="key">Ключ текста ошибки в ресурсах</param>
        /// <param name="arguments">Набор аргументов для подстановки в текст ошибки</param>
        public ExpressionValidationException(string key, params object[] arguments)
            : base(key)
        {
            MessageKey = $"FormulaError_{key}";
            MessageArguments = arguments;
        }

        /// <summary>
        /// Возвращает ключ текста ошибки в ресурсах
        /// </summary>
        public string MessageKey { get; }

        /// <summary>
        /// Возвращает набор аргументов для подстановки в текст ошибки
        /// </summary>
        public object[] MessageArguments { get; }
    }
}
