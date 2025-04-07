using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace InfraManager.BLL.ExpressionBuilder
{
    /// <summary>
    /// Этот класс преобразует математическое выражение из строки в дерево выражений типа Expression<Func<TValue>>
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparamref name="TArg"/></typeparam>
    public class ExpressionBuilder<TArg, TValue> where TValue : struct
    {
        private readonly Dictionary<char, IBinaryOperatorExpressionBuilder> _operators;
        private readonly IConstantExpressionBuilder<TValue> _constant;
        private readonly Dictionary<string, IFunctionExpressionBuilder> _functions;
        private readonly Dictionary<string, IVariableExpressionBuilder<TArg>> _variables;
        private readonly ParameterExpression _parameter = Expression.Parameter(typeof(TArg));

        /// <summary>
        /// Создает экземпляр класса ExpressionBuilder
        /// </summary>
        /// <param name="binaryOperatorBuilders">Поддерживаемые операторы</param>
        /// <param name="functionBuilders">Поддерживаемые функции</param>
        /// <param name="constantBuilder">Поддерживаемые константы / переменные</param>
        public ExpressionBuilder(
            IEnumerable<IBinaryOperatorExpressionBuilder> binaryOperatorBuilders,
            IEnumerable<IFunctionExpressionBuilder> functionBuilders,
            IConstantExpressionBuilder<TValue> constantBuilder,
            IEnumerable<IVariableExpressionBuilder<TArg>> variableBuilders)
        {
            _operators = binaryOperatorBuilders.ToDictionary(x => x.Operator);
            _functions = functionBuilders.ToDictionary(x => x.Name.ToLower());
            _constant = constantBuilder;
            _variables = variableBuilders.ToDictionary(x => x.Variable.ToLower());
        }

        /// <summary>
        /// Выполняет построение выражения из строкового представления
        /// </summary>
        /// <param name="expressionText">Строка, содержащая текстовое выражение</param>
        /// <returns>Дерево выражений</returns>
        public Expression<Func<TArg, TValue>> Build(string expressionText)
        {
            if (string.IsNullOrWhiteSpace(expressionText))
            {
                throw new ExpressionValidationException(ExpressionValidationException.EmptyExpression);
            }

            return Expression.Lambda<Func<TArg, TValue>>(Parse(expressionText.Trim()), _parameter);
        }

        private Expression Parse(string expressionText)
        {
            var part = FindLowestPriorityPart(expressionText); // поиск той части выражения, которая имеет наименьший приоритет

            // Исходное выражение содержит операторы и был найден тот, который имеет наименьший приоритет
            // Получаем левую и правую часть и рекурсивно строим деревья выражений для них, а потом объединяем
            if (part.FromIndex == part.ToIndex && _operators.ContainsKey(expressionText[part.FromIndex]))
            {
                if (part.FromIndex == 0)
                {
                    throw new ExpressionValidationException(
                        ExpressionValidationException.MissingLeftArgument, 
                        new[] { expressionText[part.FromIndex] });
                }

                var left = Parse(expressionText.Substring(0, part.FromIndex).MathTrim());                

                if (part.FromIndex == expressionText.Length - 1)
                {
                    throw new ExpressionValidationException(
                        ExpressionValidationException.MissingRightArgument,
                        new[] { expressionText[part.FromIndex] });
                }

                var right = Parse(expressionText.Substring(part.FromIndex + 1).MathTrim());

                return _operators[expressionText[part.FromIndex]].Build(left, right);
            }

            // Все выражение представляет собой константу или переменную
            if (part.FromIndex == 0 && part.ToIndex == expressionText.Length - 1)
            {
                var value = expressionText.MathTrim();

                // это переменная
                if (_variables.ContainsKey(value.ToLower()))
                {
                    return _variables[value.ToLower()].Build(_parameter);
                }

                // если не переменная, то считаем что это константа
                if (_constant.TryBuild(value, out var expression))
                {
                    return expression;
                }

                throw new ExpressionValidationException(
                    ExpressionValidationException.UnknownStatement,
                    new[] { expressionText.Trim() });
            }

            // Получаем имя функции
            var functionName = expressionText.Substring(part.FromIndex, part.ToIndex - part.FromIndex + 1).Trim();

            // Функция не поддерживается или имя содержит ошибку
            if (_functions.ContainsKey(functionName.ToLower()))
            {
                // Оставшаяся часть выражения - функция
                // Слева от функции ничего не должно быть => ошибка неизвестное выражение
                if (part.FromIndex > 0)
                {
                    throw new ExpressionValidationException(
                        ExpressionValidationException.UnknownStatement,
                        new[] { expressionText.Substring(0, part.FromIndex).Trim() });                    
                }

                // Парсим правую часть выражения, выделяем параметры и каждый рекурсивно преобразуем в выражение
                var parametersText = expressionText.Substring(part.ToIndex + 1).Trim();

                if (!parametersText.StartsWith(SpecialCharacters.OpenParenthesis)
                    || !parametersText.EndsWith(SpecialCharacters.CloseParenthesis))
                {
                    throw new ExpressionValidationException(
                        ExpressionValidationException.IncorrectSyntax,
                        new[] { parametersText });
                }

                var parameters = SplitFunctionParameters(parametersText.CutEnds());

                if (parameters.Any(p => string.IsNullOrWhiteSpace(p)))
                {
                    throw new ExpressionValidationException(
                        ExpressionValidationException.MissingParameter,
                        new[] { functionName });
                }

                return _functions[functionName.ToLower()]
                    .Build(_parameter, parameters.Select(p => Parse(p)).ToArray());
            }

            // все выражение не известно что
            throw new ExpressionValidationException(
                ExpressionValidationException.UnknownStatement,
                new[] { expressionText });

        }

        private ExpressionPart FindLowestPriorityPart(string text)
        {
            int priorityMultiplier = 0;
            byte maxPriority = (byte)(_operators.Values.Max(x => x.Priority) + 1);
            int characterPosition = 0;
            var minPriority = byte.MaxValue;
            var result = new ExpressionPart { FromIndex = 0, ToIndex = text.Length - 1 };

            foreach(var character in text)
            {                
                priorityMultiplier += character == SpecialCharacters.OpenParenthesis ? maxPriority : 0;                

                if (priorityMultiplier < 0)
                {
                    throw new ExpressionValidationException(ExpressionValidationException.MissingOpenParenthesis, characterPosition);
                }

                byte priority = (byte)(priorityMultiplier
                    + (_operators.ContainsKey(character) ? _operators[character].Priority : maxPriority));

                if (priority < minPriority)
                {
                    result = new ExpressionPart { FromIndex = characterPosition, ToIndex = characterPosition };
                    minPriority = priority;
                }
                else if (priority == minPriority)
                {
                    if (characterPosition == result.ToIndex + 1)
                    {
                        result.ToIndex++;
                    }
                    else
                    {
                        result = new ExpressionPart { FromIndex = characterPosition, ToIndex = characterPosition };
                    }
                }
                characterPosition++;
                priorityMultiplier -= character == SpecialCharacters.CloseParenthesis ? maxPriority : 0;
            }

            if (priorityMultiplier != 0)
            {
                throw new ExpressionValidationException(
                    ExpressionValidationException.MissingCloseParenthesis,
                    text.IndexOf(SpecialCharacters.OpenParenthesis));
            }

            return result;
        }

        private IEnumerable<string> SplitFunctionParameters(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                yield break;
            }

            var parameterExpression = new StringBuilder();
            var parenthesis = 0;

            foreach(var character in text)
            {
                if (character == SpecialCharacters.ParameterSeparator && parenthesis == 0)
                {
                    yield return parameterExpression.ToString();
                    parameterExpression = new StringBuilder();
                }
                else
                {
                    parameterExpression.Append(character);
                }

                parenthesis += character == SpecialCharacters.OpenParenthesis ? 1 : 0;
                parenthesis -= character == SpecialCharacters.CloseParenthesis ? 1 : 0;
            }

            yield return parameterExpression.ToString();
        }
    }
}
