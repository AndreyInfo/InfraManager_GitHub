using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace InfraManager.Core.Extensions
{
    public static class StringExtensions
    {
        #region IsNullOrEmpty
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        #endregion

        #region EmptyOnNullEmptyOrWhitespace
        public static string EmptyOnNullEmptyOrWhitespace(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return string.Empty;
            else
                return s;
        }
        #endregion

        #region Left
        public static string Left(this string source, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "count must be non-negative.");
            if (string.IsNullOrEmpty(source))
                return source;
            return source.Substring(0, Math.Min(count, source.Length));
        }
        #endregion

        #region Right
        public static string Right(this string source, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "count must be non-negative.");
            if (string.IsNullOrEmpty(source))
                return source;
            return source.Substring(Math.Max(0, source.Length - count), Math.Min(count, source.Length));
        }
        #endregion

        #region Replace
        public static string Replace(this string source, string pattern, string replacement, StringComparison comparisonType)
        {
            if (source == null)
                return null;
            //
            if (string.IsNullOrEmpty(pattern))
                return source;
            //
            int patternLength = pattern.Length;
            int current = -1;
            int previous = 0;
            StringBuilder result = new StringBuilder();
            //
            while (true)
            {
                current = source.IndexOf(pattern, current + 1, comparisonType);
                //
                if (current < 0)
                {
                    result.Append(source, previous, source.Length - previous);
                    break;
                }
                //
                result.Append(source, previous, current - previous);
                result.Append(replacement);
                //
                previous = current + patternLength;
            }
            //
            return result.ToString();
        }
        #endregion

        #region Truncate
        public static string Truncate(this string source, int length)
        {
            if (source == null)
                return null;
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", "length is less than zero.");
            return source.Length <= length ? source : source.Substring(0, length);
        }
        #endregion

        #region ObjectFormat
        public static string ObjectFormat(this string format, params object[] args)
        {
            return ObjectFormat(format, true, args);
        }

        public static string ObjectFormat(this string format, bool throwOnError, params object[] args)
        {
            return ObjectFormat(format, null, throwOnError, args);
        }

        public static string ObjectFormat(this string format, IFormatProvider formatProvider, params object[] args)
        {
            return ObjectFormat(format, null, true, args);
        }

        public static string ObjectFormat(this string format, IFormatProvider formatProvider, bool throwOnError, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");
            //
            if (throwOnError)
                return string.Join(
                    string.Empty,
                    (
                        from clause in GetClauseEnumerator(format)
                        select clause.FormatClause(formatProvider, args)
                    ).ToArray());
            else
                return string.Join(
                    string.Empty,
                    GetClauseEnumerator(format).
                        Select(
                            x => { try { return x.FormatClause(formatProvider, args); } catch { return string.Concat("{", x.Clause, "}"); } }).
                        ToArray());
        }


        #region interfaces
        private interface _IClause
        {
            string Clause { get; }

            string FormatClause(IFormatProvider formatProvider, params object[] args);
        }
        #endregion


        #region classes
        private class _LiteralClause : _IClause
        {
            #region constructors
            public _LiteralClause(string clause)
            {
                this.Clause = clause;
            }
            #endregion


            #region interface _IClause
            public string Clause { get; private set; }

            public string FormatClause(IFormatProvider formatProvider, params object[] args)
            {
                return Clause.Replace("{{", "{").Replace("}}", "}");
            }
            #endregion
        }

        private class _PropertyClause : _IClause
        {
            #region fields
            private int _index;
            private string _property;
            private string _format;
            #endregion


            #region constructors
            public _PropertyClause(string clause)
            {
                this.Clause = clause;
                string[] parts = clause.Split(new char[] { ':' });
                if (parts.Length == 1)
                {
                    _index = 0;
                    _property = parts[0];
                    _format = null;
                }
                else if (parts.Length == 2)
                {
                    if (int.TryParse(parts[0], out _index))
                    {
                        _property = parts[1];
                        _format = null;
                    }
                    else
                    {
                        _property = parts[0];
                        _format = parts[1];
                    }
                }
                else if (parts.Length == 3)
                {
                    _index = int.Parse(parts[0]);
                    _property = parts[1];
                    _format = parts[2];
                }
                else
                {
                    throw new ArgumentException("invalid format string.", "format");
                }
            }
            #endregion


            #region interface _IClause
            public string Clause { get; private set; }

            public string FormatClause(IFormatProvider formatProvider, params object[] args)
            {
                ICustomFormatter formatter = null;
                if (formatProvider != null)
                    formatter = (ICustomFormatter)formatProvider.GetFormat(typeof(ICustomFormatter));
                try
                {
                    object value = DataBinder.Eval(args[_index], _property);
                    if (value == null)
                        return string.Empty;
                    string result = null;
                    if (_format.IsNullOrEmpty())
                        result = value.ToString();
                    else
                    {
                        if (formatter != null)
                            result = formatter.Format(_format, value, formatProvider);
                        if (result == null)
                        {
                            if (value is IFormattable)
                                result = ((IFormattable)value).ToString(_format, formatProvider);
                            else
                                result = value.ToString();
                        }
                        if (result == null)
                            result = string.Empty;
                    }
                    return result;
                }
                catch (TargetInvocationException e)
                {
                    throw new FormatException(
                        string.Format(
                            "Error formatting property clause '{0}{1}'.",
                            _property,
                            _format == null ? string.Empty : string.Concat(":", _format)),
                        e);
                }
                catch (ArgumentException e)
                {
                    throw new FormatException(
                        string.Format(
                            "Error formatting property clause '{0}{1}'.",
                            _property,
                            _format == null ? string.Empty : string.Concat(":", _format)),
                        e);
                }
            }
            #endregion
        }
        #endregion


        #region methods
        private static IEnumerable<_IClause> GetClauseEnumerator(string format)
        {
            int beginClauseIndex, endClauseIndex = -1;
            do
            {
                beginClauseIndex = IndexOfOpenBrace(format, endClauseIndex + 1);
                if (beginClauseIndex < 0)
                {
                    //everything after last close brace index.
                    if (endClauseIndex + 1 < format.Length)
                        yield return new _LiteralClause(format.Substring(endClauseIndex + 1));
                    break;
                }
                if (beginClauseIndex - endClauseIndex - 1 > 0)
                    yield return new _LiteralClause(
                        format.Substring(endClauseIndex + 1, beginClauseIndex - endClauseIndex - 1));

                int endBraceIndex = IndexOfCloseBrace(format, beginClauseIndex + 1);
                if (endBraceIndex < 0)
                {
                    yield return new _LiteralClause(format.Substring(beginClauseIndex));
                    break;
                }
                else
                {
                    endClauseIndex = endBraceIndex;
                    //everything between open to close brace.
                    yield return new _PropertyClause(
                        format.Substring(beginClauseIndex + 1, endBraceIndex - beginClauseIndex - 1));
                }
            } while (beginClauseIndex > -1);
        }

        private static int IndexOfOpenBrace(string format, int startIndex)
        {
            int index = format.IndexOf('{', startIndex);
            if (index == -1)
                return index;
            if (index + 1 < format.Length && format[index + 1] == '{')
                return IndexOfOpenBrace(format, index + 2);
            return index;
        }

        private static int IndexOfCloseBrace(string format, int startIndex)
        {
            int index = format.IndexOf('}', startIndex);
            if (index == -1)
                return index;
            int braceCount = 0;
            for (int i = index + 1; i < format.Length; i++)
                if (format[i] == '}')
                    braceCount++;
                else
                    break;
            if (braceCount % 2 == 1)
                return IndexOfCloseBrace(format, index + braceCount + 1);
            return index;
        }
        #endregion
        #endregion

        #region Eval
        public static object Eval(this string expression, params object[] values)
        {
            _ExpressionParser expressionParser = new _ExpressionParser(expression, null, values);
            return Expression.Lambda(expressionParser.ParseExpression(null)).Compile().DynamicInvoke();
        }

        public static void AdjustTypeList(string key, Type type)
        {
            _ExpressionParser.AdjustTypeList(key, type);
        }

        #region classes
        public class ParseException : Exception
        {
            #region properties
            public int Position { get; private set; }
            #endregion


            #region constructors
            public ParseException(string message, int position)
                : base(message)
            {
                this.Position = position;
            }
            #endregion


            #region override method ToString
            public override string ToString()
            {
                return string.Format(_Resources.ParseExceptionFormat, this.Message, this.Position);
            }
            #endregion
        }

        private static class _Resources
        {
            #region fields
            public const string ParseExceptionFormat = "{0} (position {1})";
            public const string TheIdentifierWasDefinedMoreThanOnce = "The identifier '{0}' was defined more than once";
            public const string SyntaxError = "Syntax error";
            public const string UnrecognizedEscapeSequence = "Unrecognized escape sequence";
            public const string InvalidCharacter = "Invalid character '{0}'";
            public const string OpenBracketExpected = "'[' expected";
            public const string CloseBracketExpected = "']' expected";
            public const string OpenParenExpected = "'(' expected";
            public const string CloseParenExpected = "')' expected";
            public const string DotExpected = "'.' expected";
            public const string ColonExpected = "':' expected";
            public const string EqualExpected = "'=' expected";
            public const string ExpressionExpected = "Expression expected";
            public const string DigitExpected = "Digit expected";
            public const string ExpressionOfTypeExpected = "Expression of type '{0}' expected";
            public const string TypeIsUnknown = "Type '{0}' is unknown";
            public const string EmptyCharacterLiteral = "Empty character literal";
            public const string TooManyCharactersInCharacterLiteral = "Too many characters in character literal";
            public const string UnterminatedCharacterLiteral = "Unterminated character literal";
            public const string UnterminatedStringLiteral = "Unterminated string literal";
            public const string InvalidIntegerLiteral = "Invalid integer literal '{0}'";
            public const string InvalidRealLiteral = "Invalid real literal '{0}'";
            public const string UnknownIdentifier = "Unknown identifier '{0}'";
            public const string TypeHasNoNullableForm = "Type '{0}' has no nullable form";
            public const string OperatorIsIncompatibleWithOperandTypes = "Operator '{0}' is incompatible with operand types '{1}' and '{2}'";
            public const string ArgumentListIsIncompatableWithLambdaExpression = "Argument list is incompatable with lambda expression";
            public const string BothOfTheTypesConvertToTheOther = "Both of the types '{0}' and '{1}' converts to the other";
            public const string NeitherOfTheTypesConvertToTheOther = "Neither of the types '{0}' and '{1}' converts to the other";
            public const string TheFirstExpressionMustBeOfTypeBoolean = "The first expression must be of type 'Boolean'";
            public const string AmbiguousInvocationOfIndexerInType = "Ambiguous invocation of indexer in type '{0}'";
            public const string NoApplicableIndexerExistsInType = "No applicable indexer exists in type '{0}'";
            public const string AmbiguousInvocationOfConstructorInType = "Ambiguous invocation of constructor in type '{0}'";
            public const string NoApplicableConstructorExistsInType = "No applicable constructor exists in type '{0}'";
            public const string AmbiguousInvocationOfMethodInType = "Ambiguous invocation of method '{0}' in type '{1}'";
            public const string NoApplicableMethodExistsInType = "No applicable method '{0}' exists in type '{1}'";
            public const string MethodInTypeDoesNotReturnAValue = "Method '{0}' in type '{1}' does not return a value";
            public const string PropertyOrFieldDoesNotExistInType = "Property or field '{0}' does not exist in type '{1}'";
            public const string ArrayIndexMustBeAnIntegerExpression = "Array index must be an integer expression";
            public const string IndexingOfMultiDimensionalArraysIsNotSupported = "Indexing of multi-dimensional arrays is not supported";
            #endregion
        }

        private class _ExpressionParser
        {
            #region enums
            public enum _TokenType : byte
            {
                Unknown,
                End,

                Identifier,
                IntegerLiteral,
                RealLiteral,
                CharacterLiteral,
                StringLiteral,

                Exclamation,
                Amphersand,
                Bar,
                DoubleAmphersand,
                DoubleBar,

                LessThan,
                LessThanOrEqual,
                GreaterThanOrEqual,
                GreaterThan,

                DoubleEqual,
                ExclamationEqual,
                Question,

                As,

                Colon,
                Dot,
                Comma,

                Percent,
                Asterisk,
                Slash,
                Minus,
                Plus,

                OpenParen,
                CloseParen,
                OpenBracket,
                CloseBracket,
            }
            #endregion


            #region classes
            private class _TypeInfo
            {
                public string Alias;
                public Type Type;
            }

            private struct _Token
            {
                public _TokenType Type;
                public string Text;
                public int Position;
            }

            private class _MethodInfo
            {
                public MethodBase Method;
                public ParameterInfo[] Parameters;
                public Expression[] Arguments;
            }
            #endregion


            #region fields
            private static readonly _TypeInfo[] __predefinedTypes;
            private static readonly Expression __trueLiteral = Expression.Constant(true);
            private static readonly Expression __falseLiteral = Expression.Constant(false);
            private static readonly Expression __nullLiteral = Expression.Constant(null);
            private static readonly string __keywordTrue = "true";
            private static readonly string __keywordFalse = "false";
            private static readonly string __keywordNew = "new";
            private static readonly string __keywordNull = "null";

            private static readonly Dictionary<string, object> __keywordsDictionary;

            private Dictionary<Expression, string> _literalsDictionary;
            private Dictionary<string, object> _symbolsDictionary;
            private IDictionary<string, object> _externalsDictionary;
            private string _expression;
            private int _expressionLength;
            private int _position = -1;
            private char _char;
            private _Token _token;
            #endregion


            #region constructors
            static _ExpressionParser()
            {
                __predefinedTypes = CreatePredefinedTypes();
                __keywordsDictionary = CreateKeywordsDictionary();
            }

            public _ExpressionParser(string expression, ParameterExpression[] parameters, object[] values)
            {
                #region assertions
                Debug.Assert(expression != null, "Expression must be not null.");
                #endregion
                //
                _expression = expression;
                _expressionLength = expression.Length;
                _literalsDictionary = new Dictionary<Expression, string>();
                _symbolsDictionary = new Dictionary<string, object>(StringComparer.Ordinal);
                if (parameters != null) ProcessParameters(parameters);
                if (values != null) ProcessValues(values);
                NextChar();
                NextToken();
            }
            #endregion


            #region method ParseExpression
            public Expression ParseExpression(Type resultType)
            {
                var position = _token.Position;
                var expression = ParseCondition();
                if (resultType != null)
                    if ((expression = PromoteExpression(expression, resultType, true)) == null)
                        throw CreateParseException(position, _Resources.ExpressionOfTypeExpected, GetTypeName(resultType));
                ValidateToken(_TokenType.End, _Resources.SyntaxError);
                return expression;
            }
            #endregion

            #region private method ProcessParameters
            private void ProcessParameters(ParameterExpression[] parameters)
            {
                parameters.ForEach(x => { if (!x.Name.IsNullOrEmpty()) AddSymbol(x.Name, x); });
            }
            #endregion

            #region private method ProcessValues
            private void ProcessValues(object[] values)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    object value = values[i];
                    if (i == values.Length - 1 && value is IDictionary<string, object>)
                    {
                        _externalsDictionary = (IDictionary<string, object>)value;
                    }
                    else
                    {
                        AddSymbol(string.Concat("@", i.ToString(CultureInfo.InvariantCulture)), value);
                    }
                }
            }
            #endregion

            #region private method AddSymbol
            private void AddSymbol(string name, object value)
            {
                if (_symbolsDictionary.ContainsKey(name))
                    throw CreateParseException(_Resources.TheIdentifierWasDefinedMoreThanOnce, name);
                _symbolsDictionary.Add(name, value);
            }
            #endregion

            #region private method NextChar
            private void NextChar()
            {
                if (_position < _expressionLength) _position++;
                _char = _position < _expressionLength ? _expression[_position] : '\0';
            }
            #endregion

            #region private method NextToken
            private void NextToken()
            {
                while (Char.IsWhiteSpace(_char)) NextChar();
                _TokenType tokenType;
                int tokenPosition = _position;
                switch (_char)
                {
                    #region ! or !=
                    case '!':
                        NextChar();
                        if (_char == '=')
                        {
                            NextChar();
                            tokenType = _TokenType.ExclamationEqual;
                        }
                        else
                        {
                            tokenType = _TokenType.Exclamation;
                        }
                        break;
                    #endregion

                    #region %
                    case '%':
                        NextChar();
                        tokenType = _TokenType.Percent;
                        break;
                    #endregion

                    #region & or &&
                    case '&':
                        NextChar();
                        if (_char == '&')
                        {
                            NextChar();
                            tokenType = _TokenType.DoubleAmphersand;
                        }
                        else
                        {
                            tokenType = _TokenType.Amphersand;
                        }
                        break;
                    #endregion

                    #region *
                    case '*':
                        NextChar();
                        tokenType = _TokenType.Asterisk;
                        break;
                    #endregion

                    #region (
                    case '(':
                        NextChar();
                        tokenType = _TokenType.OpenParen;
                        break;
                    #endregion

                    #region )
                    case ')':
                        NextChar();
                        tokenType = _TokenType.CloseParen;
                        break;
                    #endregion

                    #region -
                    case '-':
                        NextChar();
                        tokenType = _TokenType.Minus;
                        break;
                    #endregion

                    #region ==
                    case '=':
                        NextChar();
                        if (_char == '=')
                        {
                            NextChar();
                            tokenType = _TokenType.DoubleEqual;
                        }
                        else
                        {
                            throw CreateParseException(_position, _Resources.EqualExpected);
                        }
                        break;
                    #endregion

                    #region +
                    case '+':
                        NextChar();
                        tokenType = _TokenType.Plus;
                        break;
                    #endregion

                    #region [
                    case '[':
                        NextChar();
                        tokenType = _TokenType.OpenBracket;
                        break;
                    #endregion

                    #region ]
                    case ']':
                        NextChar();
                        tokenType = _TokenType.CloseBracket;
                        break;
                    #endregion

                    #region | or ||
                    case '|':
                        NextChar();
                        if (_char == '|')
                        {
                            NextChar();
                            tokenType = _TokenType.DoubleBar;
                        }
                        else
                        {
                            tokenType = _TokenType.Bar;
                        }
                        break;
                    #endregion

                    #region :
                    case ':':
                        NextChar();
                        tokenType = _TokenType.Colon;
                        break;
                    #endregion

                    #region 'character literal'
                    case '\'':
                        NextChar();
                        if (_char == '\'')
                        {
                            throw CreateParseException(_position, _Resources.EmptyCharacterLiteral);
                        }
                        else if (_char == '\\')
                        {
                            NextChar();
                            if (_char == 'u') //uxxxx
                            {
                                var i = 0;
                                for (; i < 5; i++)
                                {
                                    NextChar();
                                    if (!_char.IsHex()) break;
                                }
                                if (i < 5)
                                {
                                    throw CreateParseException(_position, _Resources.UnrecognizedEscapeSequence);
                                }
                            }
                            else if (_char == 'x') //xn[n][n][n]
                            {
                                var i = 0;
                                for (; i < 5; i++)
                                {
                                    NextChar();
                                    if (!_char.IsHex()) break;
                                }
                                if (i == 0)
                                {
                                    throw CreateParseException(_position, _Resources.UnrecognizedEscapeSequence);
                                }
                            }
                            else if (_char == 'U') //Uxxxxxx
                            {
                                var i = 0;
                                for (; i < 7; i++)
                                {
                                    NextChar();
                                    if (!_char.IsHex()) break;
                                }
                                if (i < 7)
                                {
                                    throw CreateParseException(_position, _Resources.UnrecognizedEscapeSequence);
                                }
                            }
                            else if (new char[] { '\'', '"', '\\', '0', 'a', 'b', 'f', 'm', 'n', 'r', 't', 'v' }.Contains(_char)) // ', ", \, 0, a, b, f, n, r, t, v
                            {
                                NextChar();
                            }
                            else if (_position == _expressionLength)
                            {
                                throw CreateParseException(_position, _Resources.UnterminatedCharacterLiteral);
                            }
                            else
                            {
                                throw CreateParseException(_position, _Resources.UnrecognizedEscapeSequence);
                            }
                        }
                        else if (_position == _expressionLength)
                        {
                            throw CreateParseException(_position, _Resources.UnterminatedCharacterLiteral);
                        }
                        else
                        {
                            NextChar();
                        }
                        if (_char == '\'')
                        {
                            NextChar();
                            tokenType = _TokenType.CharacterLiteral;
                        }
                        else if (_position == _expressionLength)
                        {
                            throw CreateParseException(_position, _Resources.UnterminatedCharacterLiteral);
                        }
                        else
                        {
                            throw CreateParseException(_position, _Resources.TooManyCharactersInCharacterLiteral);
                        }
                        break;
                    #endregion

                    #region "string literal"
                    case '"':
                        while (true)
                        {
                            char previous = _char;
                            NextChar();
                            if (_position == _expressionLength)
                            {
                                throw CreateParseException(_position, _Resources.UnterminatedStringLiteral);
                            }
                            if (_char == '"' && previous != '\\')
                            {
                                break;
                            }
                        }
                        NextChar();
                        tokenType = _TokenType.StringLiteral;
                        break;
                    #endregion

                    #region ,
                    case ',':
                        NextChar();
                        tokenType = _TokenType.Comma;
                        break;
                    #endregion

                    #region < or <=
                    case '<':
                        NextChar();
                        if (_char == '=')
                        {
                            NextChar();
                            tokenType = _TokenType.LessThanOrEqual;
                        }
                        else
                        {
                            tokenType = _TokenType.LessThan;
                        }
                        break;
                    #endregion

                    #region .
                    case '.':
                        NextChar();
                        tokenType = _TokenType.Dot;
                        break;
                    #endregion

                    #region > or >=
                    case '>':
                        NextChar();
                        if (_char == '=')
                        {
                            NextChar();
                            tokenType = _TokenType.GreaterThanOrEqual;
                        }
                        else
                        {
                            tokenType = _TokenType.GreaterThan;
                        }
                        break;
                    #endregion

                    #region /
                    case '/':
                        NextChar();
                        tokenType = _TokenType.Slash;
                        break;
                    #endregion

                    #region ?
                    case '?':
                        NextChar();
                        tokenType = _TokenType.Question;
                        break;
                    #endregion

                    #region identifier or integer literal or real literal or end or error
                    default:
                        if (_char.IsLetter() || _char == '@' || _char == '_')
                        {
                            do
                            {
                                NextChar();
                            }
                            while (_char.IsLetterOrDigit() || _char == '_');
                            tokenType = _TokenType.Identifier;
                            break;
                        }
                        if (_char.IsDigit())
                        {
                            tokenType = _TokenType.IntegerLiteral;
                            do
                            {
                                NextChar();
                            }
                            while (_char.IsDigit());
                            if (_char == '.')
                            {
                                tokenType = _TokenType.RealLiteral;
                                NextChar();
                                if (!_char.IsDigit())
                                {
                                    throw CreateParseException(_position, _Resources.DigitExpected);
                                }
                                do
                                {
                                    NextChar();
                                }
                                while (_char.IsDigit());
                            }
                            if (_char == 'e' || _char == 'E')
                            {
                                tokenType = _TokenType.RealLiteral;
                                NextChar();
                                if (_char == '-' || _char == '+') NextChar();
                                if (!_char.IsDigit())
                                {
                                    throw CreateParseException(_position, _Resources.DigitExpected);
                                }
                                do
                                {
                                    NextChar();
                                }
                                while (_char.IsDigit());
                            }
                            if (_char == 'f' || _char == 'F' || _char == 'm' || _char == 'M') NextChar();
                            break;
                        }
                        if (_position == _expressionLength)
                        {
                            tokenType = _TokenType.End;
                            break;
                        }
                        throw CreateParseException(_position, _Resources.InvalidCharacter, _char);
                    #endregion
                }
                _token.Type = tokenType;
                _token.Text = _expression.Substring(tokenPosition, _position - tokenPosition);
                if (tokenType == _TokenType.StringLiteral)
                    _token.Text = ConvertExpressionToStringLiteral(_token.Text);
                _token.Position = tokenPosition;
                if (tokenType == _TokenType.Identifier)
                    switch (_token.Text)
                    {
                        case "as":
                            _token.Type = _TokenType.As;
                            break;
                    }
            }
            #endregion

            #region private static method ConvertToStringLiteralExpression
            private static string ConvertExpressionToStringLiteral(string value)
            {
                if (string.IsNullOrEmpty(value))
                    return value;
                //
                StringBuilder sb = new StringBuilder();
                int index = 0;
                while (index < value.Length)
                {
                    char c = value[index];
                    if (c == '\\' && index + 1 < value.Length)
                        switch (value[index + 1])
                        {
                            case '\'':
                                sb.Append('\'');
                                index += 2;
                                continue;
                            case '"':
                                sb.Append('"');
                                index += 2;
                                continue;
                            case '\\':
                                sb.Append('\\');
                                index += 2;
                                continue;
                            case '0':
                                sb.Append('\0');
                                index += 2;
                                continue;
                            case 'a':
                                sb.Append('\a');
                                index += 2;
                                continue;
                            case 'b':
                                sb.Append('\b');
                                index += 2;
                                continue;
                            case 'f':
                                sb.Append('\f');
                                index += 2;
                                continue;
                            case 'n':
                                sb.Append('\n');
                                index += 2;
                                continue;
                            case 'r':
                                sb.Append('\r');
                                index += 2;
                                continue;
                            case 't':
                                sb.Append('\t');
                                index += 2;
                                continue;
                            case 'v':
                                sb.Append('\v');
                                index += 2;
                                continue;
                        }
                    //
                    sb.Append(c);
                    index++;
                }
                //
                return sb.ToString();
            }
            #endregion

            #region private method ParseCondition
            // ? :
            private Expression ParseCondition()
            {
                var tokenPosition = _token.Position;
                Expression test = ParseOrElse();
                if (_token.Type == _TokenType.Question)
                {
                    NextToken();
                    Expression ifTrue = ParseCondition();
                    ValidateToken(_TokenType.Colon, _Resources.ColonExpected);
                    NextToken();
                    Expression ifFalse = ParseCondition();
                    test = CreateCondition(test, ifTrue, ifFalse, tokenPosition);
                }
                return test;
            }
            #endregion

            #region private method ParseOrElse
            // ||
            private Expression ParseOrElse()
            {
                Expression left = ParseAndAlso();
                while (_token.Type == _TokenType.DoubleBar)
                {
                    _Token operation = _token;
                    NextToken();
                    Expression right = ParseAndAlso();
                    left = CreateOrElse(left, right);
                }
                return left;
            }
            #endregion

            #region private method ParseAndAlso
            // &&
            private Expression ParseAndAlso()
            {
                Expression left = ParseOr();
                while (_token.Type == _TokenType.DoubleAmphersand)
                {
                    _Token operation = _token;
                    NextToken();
                    Expression right = ParseOr();
                    left = CreateAndAlso(left, right);
                }
                return left;
            }
            #endregion

            #region private method ParseOr
            // |
            private Expression ParseOr()
            {
                Expression left = ParseAnd();
                while (_token.Type == _TokenType.Bar)
                {
                    _Token operation = _token;
                    NextToken();
                    Expression right = ParseAnd();
                    left = CreateOr(left, right);
                }
                return left;
            }
            #endregion

            #region private method ParseAnd
            // &
            private Expression ParseAnd()
            {
                Expression left = ParseEquality();
                while (_token.Type == _TokenType.Amphersand)
                {
                    _Token operation = _token;
                    NextToken();
                    Expression right = ParseEquality();
                    left = CreateAnd(left, right);
                }
                return left;
            }
            #endregion

            #region private method ParseEquality
            // == != 
            private Expression ParseEquality()
            {
                Expression left = ParseTypeTesting();
                while (_token.Type == _TokenType.DoubleEqual || _token.Type == _TokenType.ExclamationEqual)
                {
                    _Token operation = _token;
                    var tokenPosition = _token.Position;
                    NextToken();
                    Expression right = ParseTypeTesting();
                    //
                    switch (operation.Type)
                    {
                        case _TokenType.DoubleEqual:
                            left = CreateEqual(left, right, tokenPosition);
                            break;
                        case _TokenType.ExclamationEqual:
                            left = CreateNotEqual(left, right, tokenPosition);
                            break;
                    }
                }
                return left;
            }
            #endregion

            #region private method ParseTypeTesting
            private Expression ParseTypeTesting()
            {
                Expression left = ParseRelational();
                while (_token.Type == _TokenType.As)
                {
                    _Token operation = _token;
                    NextToken();
                    ValidateToken(_TokenType.Identifier);
                    object value;
                    if (!__keywordsDictionary.TryGetValue(_token.Text, out value) || !(value is Type))
                    {
                        throw CreateParseException(_Resources.TypeIsUnknown, _token.Text);
                    }
                    Type type = (Type)value;
                    var tokenPosition = _token.Position;
                    NextToken();
                    if (_token.Type == _TokenType.Question)
                    {
                        if (!type.IsValueType || IsNullableType(type))
                            throw CreateParseException(tokenPosition, _Resources.TypeHasNoNullableForm, GetTypeName(type));
                        type = typeof(Nullable<>).MakeGenericType(type);
                        NextToken();
                    }
                    //
                    switch (operation.Type)
                    {
                        case _TokenType.As:
                            left = CreateConvert(left, type);
                            break;
                    }
                }
                return left;
            }
            #endregion

            #region private method ParseRelational
            // < <= > >=
            private Expression ParseRelational()
            {
                Expression left = ParseAdditive();
                while (
                    _token.Type == _TokenType.LessThan || _token.Type == _TokenType.LessThanOrEqual ||
                    _token.Type == _TokenType.GreaterThan || _token.Type == _TokenType.GreaterThanOrEqual)
                {
                    _Token operation = _token;
                    int tokenPosition = _token.Position;
                    NextToken();
                    Expression right = ParseAdditive();
                    //
                    switch (operation.Type)
                    {
                        case _TokenType.LessThan:
                            left = CreateLessThan(left, right, tokenPosition);
                            break;
                        case _TokenType.LessThanOrEqual:
                            left = CreateLessThanOrEqual(left, right, tokenPosition);
                            break;
                        case _TokenType.GreaterThan:
                            left = CreateGreaterThan(left, right, tokenPosition);
                            break;
                        case _TokenType.GreaterThanOrEqual:
                            left = CreateGreaterThanOrEqual(left, right, tokenPosition);
                            break;
                    }
                }
                return left;
            }
            #endregion

            #region private method ParseAdditive
            // + -
            private Expression ParseAdditive()
            {
                Expression left = ParseMultiplicative();
                while (_token.Type == _TokenType.Plus || _token.Type == _TokenType.Minus)
                {
                    _Token operation = _token;
                    int tokenPosition = _token.Position;
                    NextToken();
                    Expression right = ParseMultiplicative();
                    //
                    switch (operation.Type)
                    {
                        case _TokenType.Plus:
                            left = CreateAdd(left, right, tokenPosition);
                            break;
                        case _TokenType.Minus:
                            left = CreateSubtract(left, right, tokenPosition);
                            break;
                    }
                }
                return left;
            }
            #endregion

            #region private method ParseMultiplicative
            // * / %
            private Expression ParseMultiplicative()
            {
                Expression left = ParseUnary();
                while (_token.Type == _TokenType.Asterisk || _token.Type == _TokenType.Slash || _token.Type == _TokenType.Percent)
                {
                    _Token operation = _token;
                    int tokenPosition = _token.Position;
                    NextToken();
                    Expression right = ParseUnary();
                    //
                    switch (operation.Type)
                    {
                        case _TokenType.Asterisk:
                            left = CreateMultiply(left, right, tokenPosition);
                            break;
                        case _TokenType.Slash:
                            left = CreateDivide(left, right, tokenPosition);
                            break;
                        case _TokenType.Percent:
                            left = CreateModulo(left, right, tokenPosition);
                            break;
                    }
                }
                return left;
            }
            #endregion

            #region private method ParseUnary
            // -, !
            private Expression ParseUnary()
            {
                if (_token.Type == _TokenType.Minus || _token.Type == _TokenType.Exclamation)
                {
                    _Token operation = _token;
                    NextToken();
                    if (operation.Type == _TokenType.Minus &&
                        (_token.Type == _TokenType.IntegerLiteral || _token.Type == _TokenType.RealLiteral))
                    {
                        _token.Text = string.Concat("-", _token.Text);
                        _token.Position = operation.Position;
                        return ParsePrimary();
                    }
                    Expression expression = ParseUnary();
                    if (operation.Type == _TokenType.Minus)
                    {
                        expression = CreateNegate(expression);
                    }
                    else
                    {
                        expression = CreateNot(expression);
                    }
                    return expression;
                }
                return ParsePrimary();
            }
            #endregion

            #region private method ParsePrimary
            private Expression ParsePrimary()
            {
                Expression expression;
                switch (_token.Type)
                {
                    case _TokenType.Identifier:
                        expression = ParseIdentifier();
                        break;
                    case _TokenType.StringLiteral:
                        expression = ParseStringLiteral();
                        break;
                    case _TokenType.CharacterLiteral:
                        expression = ParseCharacterLiteral();
                        break;
                    case _TokenType.IntegerLiteral:
                        expression = ParseIntegerLiteral();
                        break;
                    case _TokenType.RealLiteral:
                        expression = ParseRealLiteral();
                        break;
                    case _TokenType.OpenParen:
                        expression = ParseParenExpression();
                        break;
                    default:
                        throw CreateParseException(_Resources.ExpressionExpected);
                }
                while (true)
                {
                    if (_token.Type == _TokenType.Dot)
                    {
                        NextToken();
                        expression = ParseMemberAccess(null, expression);
                    }
                    else if (_token.Type == _TokenType.OpenBracket)
                    {
                        expression = ParseElementAccess(expression);
                    }
                    else
                    {
                        break;
                    }
                }
                return expression;
            }
            #endregion

            #region private method ParseIdentifier
            private Expression ParseIdentifier()
            {
                ValidateToken(_TokenType.Identifier);
                object value;
                if (__keywordsDictionary.TryGetValue(_token.Text, out value))
                {
                    if (value == (object)__keywordNew) return ParseNew();
                    if (value is Type) return ParseTypeAccess((Type)value);
                    NextToken();
                    return (Expression)value;
                }
                if (_symbolsDictionary.TryGetValue(_token.Text, out value) ||
                    (_externalsDictionary != null && _externalsDictionary.TryGetValue(_token.Text, out value)))
                {
                    Expression expression = value as Expression;
                    if (expression == null)
                    {
                        expression = CreateConstant(value);
                    }
                    else
                    {
                        LambdaExpression lambdaExpression = expression as LambdaExpression;
                        if (lambdaExpression != null) return ParseLambdaInvocation(lambdaExpression);
                    }
                    NextToken();
                    return expression;
                }
                throw CreateParseException(_Resources.UnknownIdentifier, _token.Text);
            }
            #endregion

            #region private method ParseStringLiteral
            private Expression ParseStringLiteral()
            {
                ValidateToken(_TokenType.StringLiteral);
                string literal = _token.Text.Substring(1, _token.Text.Length - 2);
                NextToken();
                return CreateLiteral(literal, literal);
            }
            #endregion

            #region private method ParseCharacterLiteral
            private Expression ParseCharacterLiteral()
            {
                ValidateToken(_TokenType.CharacterLiteral);
                string literal = _token.Text.Substring(1, _token.Text.Length - 2);
                NextToken();
                return CreateLiteral(char.Parse(literal), literal);
            }
            #endregion

            #region private method ParseIntegerLiteral
            private Expression ParseIntegerLiteral()
            {
                ValidateToken(_TokenType.IntegerLiteral);
                string text = _token.Text;
                if (text[0] == '-')
                {
                    long value;
                    if (!long.TryParse(text, out value))
                        throw CreateParseException(_Resources.InvalidIntegerLiteral, text);
                    NextToken();
                    if (value >= int.MinValue && value <= int.MaxValue) return CreateLiteral((int)value, text);
                    return CreateLiteral(value, text);
                }
                else
                {
                    ulong value;
                    if (!ulong.TryParse(text, out value))
                        throw CreateParseException(_Resources.InvalidIntegerLiteral, text);
                    NextToken();
                    if (value <= int.MaxValue) return CreateLiteral((int)value, text);
                    if (value <= uint.MaxValue) return CreateLiteral((uint)value, text);
                    if (value <= Int64.MaxValue) return CreateLiteral((long)value, text);
                    return CreateLiteral(value, text);
                }
            }
            #endregion

            #region private method ParseRealLiteral
            private Expression ParseRealLiteral()
            {
                ValidateToken(_TokenType.RealLiteral);
                string text = _token.Text;
                //
                char ls = text[text.Length - 1];
                switch (ls)
                {
                    case 'f':
                    case 'F':
                        {
                            string val = text.Substring(0, text.Length - 1);
                            if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == "," && val.Contains('.'))
                                val = val.Replace('.', ',');
                            else if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == "." && val.Contains(','))
                                val = val.Replace(',', '.');
                            //
                            float value;
                            if (!float.TryParse(val, out value))
                                throw CreateParseException(_Resources.InvalidRealLiteral, text);
                            NextToken();
                            return CreateLiteral(value, text);
                        }
                    case 'm':
                    case 'M':
                        {
                            string val = text.Substring(0, text.Length - 1);
                            if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == "," && val.Contains('.'))
                                val = val.Replace('.', ',');
                            else if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == "." && val.Contains(','))
                                val = val.Replace(',', '.');
                            //
                            decimal value;
                            if (!decimal.TryParse(val, out value))
                                throw CreateParseException(_Resources.InvalidRealLiteral, text);
                            NextToken();
                            return CreateLiteral(value, text);
                        }
                    default:
                        {
                            string val = text;
                            if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == "," && val.Contains('.'))
                                val = val.Replace('.', ',');
                            else if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == "." && val.Contains(','))
                                val = val.Replace(',', '.');
                            //
                            double value;
                            if (!double.TryParse(val, out value))
                                throw CreateParseException(_Resources.InvalidRealLiteral, text);
                            NextToken();
                            return CreateLiteral(value, text);
                        }
                }
            }
            #endregion

            #region private method ParseParenExpression
            private Expression ParseParenExpression()
            {
                ValidateToken(_TokenType.OpenParen, _Resources.OpenParenExpected);
                NextToken();
                Expression expression = ParseCondition();
                ValidateToken(_TokenType.CloseParen, _Resources.CloseParenExpected);
                NextToken();
                return expression;
            }
            #endregion

            #region private method ParseNew
            private Expression ParseNew()
            {
                NextToken();
                ValidateToken(_TokenType.Identifier);
                object value;
                if (!__keywordsDictionary.TryGetValue(_token.Text, out value) || !(value is Type))
                {
                    throw CreateParseException(_Resources.TypeIsUnknown, _token.Text);
                }
                Type type = (Type)value;
                var tokenPosition = _token.Position;
                NextToken();
                if (_token.Type == _TokenType.Question)
                {
                    if (!type.IsValueType || IsNullableType(type))
                        throw CreateParseException(tokenPosition, _Resources.TypeHasNoNullableForm, GetTypeName(type));
                    type = typeof(Nullable<>).MakeGenericType(type);
                    NextToken();
                }
                ValidateToken(_TokenType.OpenParen, _Resources.OpenParenExpected);
                Expression[] arguments = ParseParenArguments();
                MethodBase method;
                switch (FindBestMethod(type.GetConstructors(), arguments, out method))
                {
                    case 0:
                        throw CreateParseException(tokenPosition, _Resources.NoApplicableConstructorExistsInType, GetTypeName(type));
                    case 1:
                        return CreateNew((ConstructorInfo)method, arguments);
                    default:
                        throw CreateParseException(tokenPosition, _Resources.AmbiguousInvocationOfConstructorInType, GetTypeName(type));
                }
            }
            #endregion

            #region private method ParseTypeAccess
            private Expression ParseTypeAccess(Type type)
            {
                var tokenPosition = _token.Position;
                NextToken();
                ValidateToken(_TokenType.Dot, _Resources.DotExpected);
                NextToken();
                return ParseMemberAccess(type, null);
            }
            #endregion

            #region private method ParseLambdaInvocation
            private Expression ParseLambdaInvocation(LambdaExpression lambdaExpression)
            {
                var tokenPosition = _token.Position;
                NextToken();
                Expression[] arguments = ParseParenArguments();
                MethodBase method;
                if (FindMethod(lambdaExpression.Type, "Invoke", false, arguments, out method) != 1)
                    throw CreateParseException(tokenPosition, _Resources.ArgumentListIsIncompatableWithLambdaExpression);
                return CreateInvoke(lambdaExpression, arguments);
            }
            #endregion

            #region private method ParseParenArguments
            private Expression[] ParseParenArguments()
            {
                ValidateToken(_TokenType.OpenParen, _Resources.OpenParenExpected);
                NextToken();
                Expression[] arguments = _token.Type == _TokenType.CloseParen ? new Expression[0] : ParseArguments();
                ValidateToken(_TokenType.CloseParen, _Resources.CloseParenExpected);
                NextToken();
                return arguments;
            }
            #endregion

            #region private method ParseBracketArguments
            private Expression[] ParseBracketArguments()
            {
                ValidateToken(_TokenType.OpenBracket, _Resources.OpenBracketExpected);
                NextToken();
                Expression[] arguments = ParseArguments();
                ValidateToken(_TokenType.CloseBracket, _Resources.CloseBracketExpected);
                NextToken();
                return arguments;
            }
            #endregion

            #region private method ParseArguments
            private Expression[] ParseArguments()
            {
                List<Expression> arguments = new List<Expression>();
                while (true)
                {
                    arguments.Add(ParseCondition());
                    if (_token.Type != _TokenType.Comma) break;
                    NextToken();
                }
                return arguments.ToArray();
            }
            #endregion

            #region private method ParseMemberAccess
            private Expression ParseMemberAccess(Type type, Expression instance)
            {
                if (instance != null) type = instance.Type;
                var member = _token;
                NextToken();
                if (_token.Type == _TokenType.OpenParen)
                {
                    Expression[] arguments = ParseParenArguments();
                    MethodBase method;
                    switch (FindMethod(type, member.Text, instance == null, arguments, out method))
                    {
                        case 0:
                            throw CreateParseException(member.Position, _Resources.NoApplicableMethodExistsInType, member.Text, GetTypeName(type));
                        case 1:
                            if (((MethodInfo)method).ReturnType == typeof(void))
                                throw CreateParseException(member.Position, _Resources.MethodInTypeDoesNotReturnAValue, member.Text, GetTypeName(type));
                            return CreateCall(instance, (MethodInfo)method, arguments);
                        default:
                            throw CreateParseException(member.Position, _Resources.AmbiguousInvocationOfMethodInType, member.Text, GetTypeName(type));
                    }
                }
                else
                {
                    MemberInfo memberInfo = FindPropertyOrField(type, member.Text, instance == null);
                    if (memberInfo == null)
                        throw CreateParseException(member.Position, _Resources.PropertyOrFieldDoesNotExistInType, member.Text, GetTypeName(type));
                    return memberInfo is PropertyInfo ?
                        CreateProperty(instance, (PropertyInfo)memberInfo) :
                        CreateField(instance, (FieldInfo)memberInfo);
                }
            }
            #endregion

            #region private method ParseElementAccess
            private Expression ParseElementAccess(Expression expression)
            {
                var tokenPosition = _token.Position;
                Expression[] arguments = ParseBracketArguments();
                if (expression.Type.IsArray)
                {
                    if (expression.Type.GetArrayRank() != 1 || arguments.Length != 1)
                        throw CreateParseException(tokenPosition, _Resources.IndexingOfMultiDimensionalArraysIsNotSupported);
                    Expression index = PromoteExpression(arguments[0], typeof(int), true);
                    if (index == null)
                        throw CreateParseException(tokenPosition, _Resources.ArrayIndexMustBeAnIntegerExpression);
                    return CreateArrayIndex(expression, index);
                }
                else
                {
                    MethodBase methodBase;
                    switch (FindIndexer(expression.Type, arguments, out methodBase))
                    {
                        case 0:
                            throw CreateParseException(tokenPosition, _Resources.NoApplicableConstructorExistsInType, GetTypeName(expression.Type));
                        case 1:
                            return CreateCall(expression, (MethodInfo)methodBase, arguments);
                        default:
                            throw CreateParseException(tokenPosition, _Resources.AmbiguousInvocationOfIndexerInType, GetTypeName(expression.Type));
                    }
                }
            }
            #endregion

            #region private method CreateCondition
            private Expression CreateCondition(Expression test, Expression expression1, Expression expression2, int tokenPosition)
            {
                if (test.Type != typeof(bool))
                    throw CreateParseException(tokenPosition, _Resources.TheFirstExpressionMustBeOfTypeBoolean);
                if (expression1.Type != expression2.Type)
                {
                    Expression expression1as2 = expression2 == __nullLiteral ? null : PromoteExpression(expression1, expression2.Type, true);
                    Expression expression2as1 = expression1 == __nullLiteral ? null : PromoteExpression(expression2, expression1.Type, true);
                    if (expression1as2 != null && expression2as1 == null)
                    {
                        expression1 = expression1as2;
                    }
                    else if (expression1as2 == null && expression2as1 != null)
                    {
                        expression2 = expression2as1;
                    }
                    else
                    {
                        string type1Name = expression1 == __nullLiteral ? "null" : GetTypeName(expression1.Type);
                        string type2Name = expression2 == __nullLiteral ? "null" : GetTypeName(expression2.Type);
                        if (expression1as2 != null && expression2as1 != null)
                            throw CreateParseException(tokenPosition, _Resources.BothOfTheTypesConvertToTheOther, type1Name, type2Name);
                        throw CreateParseException(tokenPosition, _Resources.NeitherOfTheTypesConvertToTheOther, type1Name, type2Name);
                    }
                }
                return Expression.Condition(test, expression1, expression2);
            }
            #endregion

            #region private method CreateOrElse
            private Expression CreateOrElse(Expression left, Expression right)
            {
                return Expression.OrElse(left, right);
            }
            #endregion

            #region private method CreateAndAlso
            private Expression CreateAndAlso(Expression left, Expression right)
            {
                return Expression.AndAlso(left, right);
            }
            #endregion

            #region private method CreateOr
            private Expression CreateOr(Expression left, Expression right)
            {
                return Expression.Or(left, right);
            }
            #endregion

            #region private method CreateAnd
            private Expression CreateAnd(Expression left, Expression right)
            {
                return Expression.And(left, right);
            }
            #endregion

            #region private method CreateEqual
            private Expression CreateEqual(Expression left, Expression right, int tokenPosition)
            {
                if (left.Type != right.Type)
                {
                    right = CreateConvert(right, left.Type);
                }
                return Expression.Equal(left, right);
            }
            #endregion

            #region private method CreateNotEqual
            private Expression CreateNotEqual(Expression left, Expression right, int tokenPosition)
            {
                if (left.Type != right.Type)
                {
                    right = CreateConvert(right, left.Type);
                }
                return Expression.NotEqual(left, right);
            }
            #endregion

            #region private method CreateLessThan
            private Expression CreateLessThan(Expression left, Expression right, int tokenPosition)
            {
                if (left.Type == typeof(string))
                    return Expression.LessThan(
                        CreateCall(null, left.Type.GetMethod("Compare", new Type[] { left.Type, right.Type }), left, right),
                        CreateConstant(0));
                if (left.Type != right.Type)
                {
                    right = CreateConvert(right, left.Type);
                }
                return Expression.LessThan(left, right);
            }
            #endregion

            #region private method CreateLessThanOrEqual
            private Expression CreateLessThanOrEqual(Expression left, Expression right, int tokenPosition)
            {
                if (left.Type == typeof(string))
                    return Expression.LessThanOrEqual(
                        CreateCall(null, left.Type.GetMethod("Compare", new Type[] { left.Type, right.Type }), left, right),
                        CreateConstant(0));
                if (left.Type != right.Type)
                {
                    right = CreateConvert(right, left.Type);
                }
                return Expression.LessThanOrEqual(left, right);
            }
            #endregion

            #region private method CreateGreaterThan
            private Expression CreateGreaterThan(Expression left, Expression right, int tokenPosition)
            {
                if (left.Type == typeof(string))
                    return Expression.GreaterThan(
                        CreateCall(null, left.Type.GetMethod("Compare", new Type[] { left.Type, right.Type }), left, right),
                        CreateConstant(0));
                if (left.Type != right.Type)
                {
                    right = CreateConvert(right, left.Type);
                }
                return Expression.GreaterThan(left, right);
            }
            #endregion

            #region private method CreateGreaterThanOrEqual
            private Expression CreateGreaterThanOrEqual(Expression left, Expression right, int tokenPosition)
            {
                if (left.Type == typeof(string))
                    return Expression.GreaterThanOrEqual(
                        CreateCall(null, left.Type.GetMethod("Compare", new Type[] { left.Type, right.Type }), left, right),
                        CreateConstant(0));
                if (left.Type != right.Type)
                {
                    right = CreateConvert(right, left.Type);
                }
                return Expression.GreaterThanOrEqual(left, right);
            }
            #endregion

            #region private method CreateAdd
            private Expression CreateAdd(Expression left, Expression right, int tokenPosition)
            {
                if (left.Type == typeof(string))
                    return CreateCall(null, left.Type.GetMethod("Concat", new Type[] { left.Type, right.Type }), left, right);
                if (left.Type != right.Type)
                {
                    right = CreateConvert(right, left.Type);
                }
                return Expression.Add(left, right);
            }
            #endregion

            #region private method CreateSubtract
            private Expression CreateSubtract(Expression left, Expression right, int tokenPosition)
            {
                if (left.Type != right.Type)
                {
                    right = CreateConvert(right, left.Type);
                }
                return Expression.Subtract(left, right);
            }
            #endregion

            #region private method CreateMultiply
            private Expression CreateMultiply(Expression left, Expression right, int tokenPosition)
            {
                if (left.Type != right.Type)
                {
                    right = CreateConvert(right, left.Type);
                }
                return Expression.Multiply(left, right);
            }
            #endregion

            #region private method CreateDivide
            private Expression CreateDivide(Expression left, Expression right, int tokenPosition)
            {
                if (left.Type != right.Type)
                {
                    right = CreateConvert(right, left.Type);
                }
                return Expression.Divide(left, right);
            }
            #endregion

            #region private method CreateModulo
            private Expression CreateModulo(Expression left, Expression right, int tokenPosition)
            {
                if (left.Type != right.Type)
                {
                    right = CreateConvert(right, left.Type);
                }
                return Expression.Modulo(left, right);
            }
            #endregion

            #region private method CreateNegate
            private Expression CreateNegate(Expression expression)
            {
                return Expression.Negate(expression);
            }
            #endregion

            #region private method CreateNot
            private Expression CreateNot(Expression expression)
            {
                return Expression.Not(expression);
            }
            #endregion

            #region private method CreateConstant
            private Expression CreateConstant(object value)
            {
                return Expression.Constant(value);
            }

            private Expression CreateConstant(object value, Type type)
            {
                return Expression.Constant(value, type);
            }
            #endregion

            #region private method CreateLiteral
            private Expression CreateLiteral(object value, string text)
            {
                ConstantExpression constantExpression = Expression.Constant(value);
                _literalsDictionary.Add(constantExpression, text);
                return constantExpression;
            }
            #endregion

            #region private method CreateNew
            private Expression CreateNew(ConstructorInfo constructor, params Expression[] arguments)
            {
                return Expression.New(constructor, arguments);
            }
            #endregion

            #region private method CreateProperty
            private Expression CreateProperty(Expression instance, PropertyInfo property)
            {
                return Expression.Property(instance, property);
            }
            #endregion

            #region private method CreateField
            private Expression CreateField(Expression instance, FieldInfo field)
            {
                return Expression.Field(instance, field);
            }
            #endregion

            #region private method CreateArrayIndex
            private Expression CreateArrayIndex(Expression array, Expression index)
            {
                return Expression.ArrayIndex(array, index);
            }
            #endregion

            #region private method CreateCall
            private Expression CreateCall(Expression instance, MethodInfo method, params Expression[] arguments)
            {
                return Expression.Call(instance, method, arguments);
            }
            #endregion

            #region private method CreateInvoke
            private Expression CreateInvoke(Expression instance, params Expression[] arguments)
            {
                return Expression.Invoke(instance, arguments);
            }
            #endregion

            #region private method CreateConvert
            private Expression CreateConvert(Expression instance, Type type)
            {
                return Expression.Convert(instance, type);
            }
            #endregion

            #region private method FindPropertyOrField
            private MemberInfo FindPropertyOrField(Type type, string memberName, bool isStatic)
            {
                BindingFlags bindingFlags = BindingFlags.Public | (isStatic ? BindingFlags.Static : BindingFlags.Instance);
                foreach (var t in SelfAndBaseTypes(type))
                {
                    if (t == null)
                        break;
                    MemberInfo[] memberInfos = t.FindMembers(
                        MemberTypes.Property | MemberTypes.Field,
                        bindingFlags,
                        Type.FilterName,
                        memberName);
                    if (memberInfos.Length > 0) return memberInfos[0];
                }
                return null;
            }
            #endregion

            #region private method FindIndexer
            private int FindIndexer(Type type, Expression[] arguments, out MethodBase method)
            {
                foreach (Type t in SelfAndBaseTypes(type))
                {
                    MemberInfo[] memberInfos = t.GetDefaultMembers();
                    if (memberInfos.Length > 0)
                    {
                        IEnumerable<MethodBase> methods = memberInfos.
                            OfType<PropertyInfo>().
                            Select(x => (MethodBase)x.GetGetMethod()).
                            Where(x => x != null);
                        var count = FindBestMethod(methods, arguments, out method);
                        if (count > 0) return count;
                    }
                }
                method = null;
                return 0;
            }
            #endregion

            #region private method FindMethod
            private int FindMethod(Type type, string methodName, bool isStatic, Expression[] arguments, out MethodBase method)
            {
                BindingFlags bindingFlags = BindingFlags.Public | (isStatic ? BindingFlags.Static : BindingFlags.Instance);
                foreach (var t in SelfAndBaseTypes(type))
                {
                    MemberInfo[] memberInfos = t.FindMembers(
                        MemberTypes.Method,
                        bindingFlags,
                        Type.FilterName,
                        methodName);
                    var count = FindBestMethod(memberInfos.Cast<MethodBase>(), arguments, out method);
                    if (count != 0) return count;
                }
                method = null;
                return 0;
            }
            #endregion

            #region private method FindBestMethod
            private int FindBestMethod(IEnumerable<MethodBase> methods, Expression[] arguments, out MethodBase method)
            {
                _MethodInfo[] methodInfos = methods.
                    Select(x => new _MethodInfo() { Method = x, Parameters = x.GetParameters() }).
                    Where(x => IsApplicable(x, arguments)).
                    ToArray();
                if (methodInfos.Length > 1)
                {
                    methodInfos = methodInfos.
                        Where(x => methodInfos.All(y => x == y || IsBetterThan(arguments, x, y))).
                        ToArray();
                }
                if (methodInfos.Length == 1)
                {
                    _MethodInfo methodInfo = methodInfos[0];
                    for (int i = 0; i < methodInfo.Parameters.Length; i++) arguments[i] = methodInfo.Arguments[i];
                    method = methodInfo.Method;
                }
                else
                {
                    method = null;
                }
                return methodInfos.Length;
            }
            #endregion

            #region private method IsApplicable
            private bool IsApplicable(_MethodInfo methodInfo, Expression[] arguments)
            {
                if (methodInfo.Parameters.Length != arguments.Length) return false;
                Expression[] promotedArguments = new Expression[arguments.Length];
                for (int i = 0; i < arguments.Length; i++)
                {
                    ParameterInfo parameterInfo = methodInfo.Parameters[i];
                    if (parameterInfo.IsOut) return false;
                    Expression promotedArgument = PromoteExpression(arguments[i], parameterInfo.ParameterType, false);
                    if (promotedArgument == null) return false;
                    promotedArguments[i] = promotedArgument;
                }
                methodInfo.Arguments = promotedArguments;
                return true;
            }
            #endregion

            #region private method PromoteExpression
            private Expression PromoteExpression(Expression expression, Type targetType, bool exact)
            {
                #region assertions
                Debug.Assert(expression != null, "Expression must be not null.");
                Debug.Assert(targetType != null, "Target type must be not null.");
                #endregion
                //
                if (expression.Type == targetType) return expression;
                if (expression is ConstantExpression)
                {
                    var constantExpression = expression as ConstantExpression;
                    if (constantExpression == __nullLiteral)
                    {
                        if (!targetType.IsValueType || IsNullableType(targetType))
                            return CreateConstant(null, targetType);
                    }
                    else
                    {
                        string text;
                        if (_literalsDictionary.TryGetValue(constantExpression, out text))
                        {
                            Type nonNullableTargetType = GetNonNullableType(targetType);
                            object value = null;
                            switch (Type.GetTypeCode(constantExpression.Type))
                            {
                                case TypeCode.Int32:
                                case TypeCode.UInt32:
                                case TypeCode.Int64:
                                case TypeCode.UInt64:
                                case TypeCode.Double:
                                    value = ParseNumber(text, nonNullableTargetType);
                                    break;
                            }
                            if (value != null)
                                return CreateConstant(value, targetType);
                        }
                    }
                }
                if (IsCompatibleWith(expression.Type, targetType))
                {
                    if (targetType.IsValueType || exact) return CreateConvert(expression, targetType);
                    return expression;
                }
                return null;
            }
            #endregion

            #region private static method CreatePredefinedTypes
            private static _TypeInfo[] CreatePredefinedTypes()
            {
                var result = 
                    new List<_TypeInfo>
					{
						new() {Alias = "object", Type = typeof(Object)}, new() {Alias = "Object", Type = typeof(Object)},
						new() {Alias = "Boolean", Type = typeof(Boolean)}, new() {Alias = "bool", Type = typeof(Boolean)},
						new() {Alias = "Char", Type = typeof(Char)}, new() {Alias = "char", Type = typeof(Char)},
						new() {Alias = "SByte", Type = typeof(SByte)}, new() {Alias = "sbyte", Type = typeof(SByte)},
						new() {Alias = "Byte", Type = typeof(Byte)}, new() {Alias = "byte", Type = typeof(Byte)},
						new() {Alias = "Int16", Type = typeof(Int16)}, new() {Alias = "short", Type = typeof(Int16)},
						new() {Alias = "UInt16", Type = typeof(UInt16)}, new() {Alias = "ushort", Type = typeof(UInt16)},
						new() {Alias = "Int32", Type = typeof(Int32)}, new() {Alias = "int", Type = typeof(Int32)},
						new() {Alias = "UInt32", Type = typeof(UInt32)}, new() {Alias = "uint", Type = typeof(UInt32)},
						new() {Alias = "Int64", Type = typeof(Int64)}, new() {Alias = "long", Type = typeof(Int64)},
						new() {Alias = "UInt64", Type = typeof(UInt64)}, new() {Alias = "ulong", Type = typeof(UInt64)},
						new() {Alias = "Single", Type = typeof(Single)}, new() {Alias = "float", Type = typeof(Single)},
						new() {Alias = "Double", Type = typeof(Double)}, new() {Alias = "double", Type = typeof(Double)},
						new() {Alias = "Decimal", Type = typeof(Decimal)}, new() {Alias = "decimal", Type = typeof(Decimal)},
						new() {Alias = "DateTime", Type = typeof(DateTime)},
						new() {Alias = "TimeSpan", Type = typeof(TimeSpan)},
						new() {Alias = "Guid", Type = typeof(Guid)},
						new() {Alias = "String", Type = typeof(String)}, new() {Alias = "string", Type = typeof(String)},
						new() {Alias = "Math", Type = typeof(Math)},
						new() {Alias = "Convert", Type = typeof(Convert)},
                        new() {Alias = "Regex", Type = typeof(System.Text.RegularExpressions.Regex)},
                        new() {Alias = "File", Type = typeof(System.IO.File)},
                        new() {Alias = "DataSet", Type = typeof(System.Data.DataSet)},
                        new() {Alias = "DataTable", Type=typeof(System.Data.DataTable)},
                        new() {Alias = "Array", Type=typeof(Array)},
                        new() {Alias = "ArrayList", Type=typeof(System.Collections.ArrayList)},
                        new() {Alias = "IList", Type=typeof(System.Collections.IList)},
                        new() {Alias = "Encoding", Type=typeof(System.Text.Encoding)},
                        
                        new() {Alias = "StringHelper", Type=typeof(InfraManager.Core.Helpers.StringHelper)},
                        new() {Alias = "BusinessRole", Type = Type.GetType(Types.BusinessRoleTypeName)},
#if Configuration
						new() {Alias = "Adapter", Type = Types.AdapterType},
                        new() {Alias = "ProductCatalogTemplate", Type = Types.ProductCatalogTemplateType},
						new() {Alias = "NetworkDevice", Type = Types.NetworkDeviceType},
						new() {Alias = "Peripheral", Type = Types.PeripheralType},
						new() {Alias = "TerminalDevice", Type = Types.TerminalDeviceType},
						new() {Alias = "AssetNumber", Type = Types.AssetNumberType},
                        new() {Alias = "AssetFields", Type = Types.AssetFieldsType},
						new() {Alias = "ConfigurationUnit", Type = Types.ConfigurationUnitType},
						new() {Alias = "DataEntity", Type = Types.DataEntityType},
#endif
						new() {Alias = "User", Type = Types.UserType},
						new() {Alias = "Role", Type = Types.RoleType},
                        new() {Alias = "Document", Type = Types.DocumentType},
                        new() {Alias = "DocumentList", Type = Types.DocumentListType},

                        new() {Alias = "Organization", Type = Types.OrganizationType},
                        new() {Alias = "Subdivision", Type = Types.SubdivisionType},

                        new() {Alias = "StorageLocation", Type = Types.StorageLocationType},
                        new() {Alias = "Workplace", Type = Types.WorkplaceType},
                        new() {Alias = "Building", Type = Types.BuildingType},
                        new() {Alias = "Floor", Type = Types.FloorType},
                        new() {Alias = "Rack", Type = Types.RackType},
                        new() {Alias = "Room", Type = Types.RoomType},

                        new() {Alias = "CalendarHelper", Type = Types.CalendarHelperType},
                        new() {Alias = "CalendarTimeZone", Type = Types.CalendarTimeZoneType},
#if Software
						new() {Alias = "SoftwareInstallation", Type = Types.SoftwareInstallationType},
                        new() {Alias = "SoftwareLicence", Type = Types.SoftwareLicenceType},
                        new() {Alias = "SoftwareModel", Type = Types.SoftwareModelType},
                        new() {Alias = "SoftwareModelSupportLineResponsible", Type = Types.SoftwareModelSupportLineResponsibleType},
#endif
                        new() {Alias = "MessageByEmail", Type = Types.MessageByEmailType},
#if Configuration
                        new() {Alias = "MessageByInquiry", Type = Types.MessageByInquiryType},
#endif
#if Monitoring
                        new() {Alias = "MessageByMonitoring", Type = Types.MessageByMonitoringType},
#endif

                        new() {Alias = "XmlElement", Type = typeof(Xml.XmlElement)},
                        new() {Alias = "XmlAttribute", Type = typeof(Xml.XmlAttribute)},
                        new() {Alias = "XmlElementCollection", Type = typeof(Xml.XmlElementCollection)},
                        new() {Alias = "XmlAttributeCollection",Type = typeof(Xml.XmlAttributeCollection)},

                        new() {Alias = "JsonElement", Type = typeof(Json.JsonElement)},
                        new() {Alias = "JsonElementCollection", Type = typeof(Json.JsonElementCollection)},

#if ServiceDesk
						new() {Alias = "DependencyObject", Type = Types.DependencyObjectType},
                        new() {Alias = "ExecutorAssignmentType", Type = Types.ExecutorAssignmentTypeType},
						new() {Alias = "Influence", Type = Types.InfluenceType},
						new() {Alias = "Negotiation", Type = Types.NegotiationType},
                        new() {Alias = "NegotiationStatus", Type = Types.NegotiationStatusType},
						new() {Alias = "Priority", Type = Types.PriorityType},
						new() {Alias = "PriorityMatrix", Type = Types.PriorityMatrixType},
                        new() {Alias = "Queue", Type = Types.QueueType},
						new() {Alias = "Urgency", Type = Types.UrgencyType},

						new() {Alias = "Call", Type = Types.CallType},
						new() {Alias = "CallReceiptType", Type = Types.CallReceiptTypeType},
						new() {Alias = "CallSummary", Type = Types.CallSummaryType},
						new() {Alias = "CallType", Type = Types.CallTypeType},
						new() {Alias = "IncidentResult", Type = Types.IncidentResultType},
						new() {Alias = "RFSResult", Type = Types.RFSResultType},

						new() {Alias = "Problem", Type = Types.ProblemType},
						new() {Alias = "ProblemCause", Type = Types.ProblemCauseType},
						new() {Alias = "ProblemType", Type = Types.ProblemTypeType},

                        new() {Alias = "RFC", Type = Types.RFCType},
                        new() {Alias = "RFCType", Type = Types.RFCTypeType},

                        new() {Alias = "AttendanceType", Type = Types.AttendanceTypeType},
						new() {Alias = "Service", Type = Types.ServiceType},
						new() {Alias = "ServiceAttendance", Type = Types.ServiceAttendanceType},
                        new() {Alias = "ServiceItem", Type = Types.ServiceItemType},
                        new() {Alias = "SLA", Type = Types.SLAType},

						new() {Alias = "WorkOrder", Type = Types.WorkOrderType},
                        new() {Alias = "WorkOrderPriority", Type = Types.WorkOrderPriorityType},
						new() {Alias = "WorkOrderTemplate", Type = Types.WorkOrderTemplateType},
						new() {Alias = "WorkOrderType", Type = Types.WorkOrderTypeType},

                        new() {Alias = "ActivesRequestSpecification", Type = Types.ActivesRequestSpecificationType},
                        new() {Alias = "GoodsInvoice", Type = Types.GoodsInvoiceType},
                        new() {Alias = "GoodsInvoiceSpecification", Type = Types.GoodsInvoiceSpecificationType},
                        new() {Alias = "GoodsInvoiceSpecificationDependencyPurchase", Type = Types.GoodsInvoiceSpecificationDependencyPurchaseType},
                        new() {Alias = "GoodsInvoiceSpecificationAssetReference", Type = Types.GoodsInvoiceSpecificationAssetReferenceType},
                        new() {Alias = "PurchaseSpecification", Type = Types.PurchaseSpecificationType},
                        new() {Alias = "PurchaseSpecificationDependencyActiveRequests", Type = Types.PurchaseSpecificationDependencyActiveRequestsType},
                        new() {Alias = "WorkOrderFinanceActivesRequest", Type = Types.WorkOrderFinanceActivesRequestType},
                        new() {Alias = "WorkOrderFinancePurchase", Type = Types.WorkOrderFinancePurchaseType},
                        new() {Alias = "PurchaseSpecificationDependencyFinanceBudgetRow", Type = Types.PurchaseSpecificationDependencyFinanceBudgetRowType},
                        new() {Alias = "FinanceBudgetRow", Type = Types.FinanceBudgetRowType},
                        new() {Alias = "FinanceBudgetRowDependencyActiveRequest", Type = Types.FinanceBudgetRowDependencyActiveRequestType},

                        new() {Alias = "ParameterValue", Type = Types.ParameterValueType},
                        new() {Alias = "BinaryValue", Type = Types.BinaryValueType},
                        new() {Alias = "ParameterEnum", Type = Types.ParameterEnumType},
                        new() {Alias = "ParameterEnumValue", Type = Types.ParameterEnumValueType},
                        new() {Alias = "ParameterData", Type = Types.ParameterDataType},

                        new() {Alias = "ParameterCustomDictionaryFilter", Type = Types.ParameterCustomDictionaryFilterType},
                        new() {Alias = "ParameterConfigurationObjectFilter", Type = Types.ParameterConfigurationObjectFilterType},
                        new() {Alias = "ParameterDateTimeFilter", Type = Types.ParameterDateTimeFilterType},
                        new() {Alias = "ParameterLocationFilter", Type = Types.ParameterLocationFilterType},
                        new() {Alias = "ParameterModelFilter", Type = Types.ParameterModelFilterType},
                        new() {Alias = "ParameterNumberFilter", Type = Types.ParameterNumberFilterType},
                        new() {Alias = "ParameterPositionFilter", Type = Types.ParameterPositionFilterType},                        
                        new() {Alias = "ParameterStringFilter", Type = Types.ParameterStringFilterType},
                        new() {Alias = "ParameterSubdivisionFilter", Type = Types.ParameterSubdivisionFilterType},
                        new() {Alias = "ParameterUserFilter", Type = Types.ParameterUserFilterType},
                        new() {Alias = "ParameterTableFilter", Type = Types.ParameterTableFilterType},
#endif
                        new _TypeInfo{Alias = "ServiceCenter", Type = Types.ServiceCenterType},
                        new _TypeInfo{Alias = "ServiceContract", Type = Types.ServiceContractType},
                        new _TypeInfo{Alias = "ServiceContractAgreement", Type = Types.ServiceContractAgreementType},
                        new _TypeInfo{Alias = "Supplier", Type = Types.SupplierType},
						new _TypeInfo{Alias = "Material", Type = Types.MaterialType},

                        new() {Alias = "ConfSys", Type = Types.ConfSysType },
                        new() {Alias = "TechnicalFailuresCategory", Type = Types.TechnicalFailuresCategoryType}
					};

                if (StringExtensions._additionalTypes != null)
                {
                    result.AddRange(_additionalTypes().Select(x => new _TypeInfo() { Alias = x.Key, Type = x.Value }));
                }

                return result.ToArray();

            }
            #endregion

            #region private static method CreateKeywordsDictionary
            private static Dictionary<string, object> CreateKeywordsDictionary()
            {
                var dictionary = new Dictionary<string, object>(StringComparer.Ordinal);
                dictionary.Add(__keywordTrue, __trueLiteral);
                dictionary.Add(__keywordFalse, __falseLiteral);
                dictionary.Add(__keywordNull, __nullLiteral);
                dictionary.Add(__keywordNew, __keywordNew);
                __predefinedTypes.ForEach(x => dictionary.Add(x.Alias, x.Type));
                return dictionary;
            }
            #endregion

            #region private static method SelfAndBaseTypes
            private static IEnumerable<Type> SelfAndBaseTypes(Type type)
            {
                return type.IsInterface ?
                    type.VisitDepthFirst(x => x.GetInterfaces()).Distinct() :
                    type.VisitDepthFirst(x => new Type[] { x.BaseType });
            }
            #endregion

            #region private static method IsBetterThan
            private static bool IsBetterThan(Expression[] arguments, _MethodInfo methodInfo1, _MethodInfo methodInfo2)
            {
                bool better = false;
                for (int i = 0; i < arguments.Length; i++)
                {
                    var result = CompareConversions(arguments[i].Type, methodInfo1.Parameters[i].ParameterType, methodInfo2.Parameters[i].ParameterType);
                    if (result < 0) return false;
                    if (result > 0) better = true;
                }
                return better;
            }
            #endregion

            #region private static method CompareConversions
            private static int CompareConversions(Type sourceType, Type targetType1, Type targetType2)
            {
                if (targetType1 == targetType2) return 0;
                if (sourceType == targetType1) return 1;
                if (sourceType == targetType2) return -1;
                bool t1t2 = IsCompatibleWith(targetType1, targetType2);
                bool t2t1 = IsCompatibleWith(targetType2, targetType1);
                if (t1t2 && !t2t1) return 1;
                if (t2t1 && !t1t2) return -1;
                if (IsSignedIntegralType(targetType1) && IsUnsignedIntegralType(targetType2)) return 1;
                if (IsSignedIntegralType(targetType2) && IsUnsignedIntegralType(targetType1)) return -1;
                return 0;
            }
            #endregion

            #region private static method IsNumericType
            private static bool IsNumericType(Type type)
            {
                return GetNumericTypeKind(type) != 0;
            }
            #endregion

            #region private static method IsSignedIntegralType
            private static bool IsSignedIntegralType(Type type)
            {
                return GetNumericTypeKind(type) == 2;
            }
            #endregion

            #region private static method IsUnsignedIntegralType
            private static bool IsUnsignedIntegralType(Type type)
            {
                return GetNumericTypeKind(type) == 3;
            }
            #endregion

            #region private static method GetNumericTypeKind
            private static int GetNumericTypeKind(Type type)
            {
                type = GetNonNullableType(type);
                if (type.IsEnum) return 0;
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Char:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        return 1;
                    case TypeCode.SByte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                        return 2;
                    case TypeCode.Byte:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        return 3;
                    default:
                        return 0;
                }
            }
            #endregion

            #region private static method IsEnumType
            private static bool IsEnumType(Type type)
            {
                return GetNonNullableType(type).IsEnum;
            }
            #endregion

            #region private static method ParseNumber
            private static object ParseNumber(string text, Type type)
            {
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.SByte:
                        sbyte @sbyte;
                        if (sbyte.TryParse(text, out @sbyte)) return @sbyte;
                        break;
                    case TypeCode.Byte:
                        byte @byte;
                        if (byte.TryParse(text, out @byte)) return @byte;
                        break;
                    case TypeCode.Int16:
                        short @short;
                        if (short.TryParse(text, out @short)) return @short;
                        break;
                    case TypeCode.UInt16:
                        ushort @ushort;
                        if (ushort.TryParse(text, out @ushort)) return @ushort;
                        break;
                    case TypeCode.Int32:
                        int @int;
                        if (int.TryParse(text, out @int)) return @int;
                        break;
                    case TypeCode.UInt32:
                        uint @uint;
                        if (uint.TryParse(text, out @uint)) return @uint;
                        break;
                    case TypeCode.Int64:
                        long @long;
                        if (long.TryParse(text, out @long)) return @long;
                        break;
                    case TypeCode.UInt64:
                        ulong @ulong;
                        if (ulong.TryParse(text, out @ulong)) return @ulong;
                        break;
                    case TypeCode.Single:
                        float @float;
                        if (float.TryParse(text, out @float)) return @float;
                        break;
                    case TypeCode.Double:
                        double @double;
                        if (double.TryParse(text, out @double)) return @double;
                        break;
                    case TypeCode.Decimal:
                        decimal @decimal;
                        if (decimal.TryParse(text, out @decimal)) return @decimal;
                        break;
                }
                return null;
            }
            #endregion

            #region private static method ParseEnum
            private static object ParseEnum(string text, Type type)
            {
                if (type.IsEnum)
                {
                    MemberInfo[] memberInfos = type.FindMembers(
                        MemberTypes.Field,
                        BindingFlags.Public | BindingFlags.Static,
                        Type.FilterName,
                        text);
                    if (memberInfos.Length > 0) return ((FieldInfo)memberInfos[0]).GetValue(null);
                }
                return null;
            }
            #endregion

            #region private static method IsCompatibleWith
            private static bool IsCompatibleWith(Type sourceType, Type targetType)
            {
                #region assertions
                Debug.Assert(sourceType != null, "Source type must be not null.");
                Debug.Assert(targetType != null, "Target type must be not null.");
                #endregion
                //
                if (sourceType == targetType) return true;
                if (!targetType.IsValueType) return targetType.IsAssignableFrom(sourceType);
                Type nonNullableSourceType = GetNonNullableType(sourceType);
                Type nonNullableTargetType = GetNonNullableType(targetType);
                if (nonNullableSourceType != sourceType && nonNullableTargetType == targetType) return false;
                TypeCode sourceTypeCode = nonNullableSourceType.IsEnum ? TypeCode.Object : Type.GetTypeCode(nonNullableSourceType);
                TypeCode targetTypeCode = nonNullableTargetType.IsEnum ? TypeCode.Object : Type.GetTypeCode(nonNullableTargetType);
                switch (sourceTypeCode)
                {
                    case TypeCode.SByte:
                        switch (targetTypeCode)
                        {
                            case TypeCode.Int16:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;
                    case TypeCode.Byte:
                        switch (targetTypeCode)
                        {
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;
                    case TypeCode.Int16:
                        switch (targetTypeCode)
                        {
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;
                    case TypeCode.UInt16:
                        switch (targetTypeCode)
                        {
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;
                    case TypeCode.Int32:
                        switch (targetTypeCode)
                        {
                            case TypeCode.Int64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;
                    case TypeCode.UInt32:
                        switch (targetTypeCode)
                        {
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;
                    case TypeCode.Int64:
                        switch (targetTypeCode)
                        {
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;
                    case TypeCode.UInt64:
                        switch (targetTypeCode)
                        {
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;
                    case TypeCode.Single:
                        switch (targetTypeCode)
                        {
                            case TypeCode.Double:
                                return true;
                        }
                        break;
                    case TypeCode.Char:
                        switch (targetTypeCode)
                        {
                            case TypeCode.UInt16:
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;
                    default:
                        if (sourceTypeCode == targetTypeCode) return true;
                        break;
                }
                return false;
            }
            #endregion

            #region private static method GetNonNullableType
            private static Type GetNonNullableType(Type type)
            {
                return IsNullableType(type) ? type.GetGenericArguments()[0] : type;
            }
            #endregion

            #region private static method IsNullableType
            private static bool IsNullableType(Type type)
            {
                return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
            }
            #endregion

            #region private static method GetTypeName
            private static string GetTypeName(Type type)
            {
                Type nonNullableType = GetNonNullableType(type);
                return type == nonNullableType ? type.Name : string.Concat(nonNullableType.Name, "?");
            }
            #endregion

            #region private method ValidateToken
            private void ValidateToken(_TokenType tokenType)
            {
                ValidateToken(tokenType, _Resources.SyntaxError);
            }

            private void ValidateToken(_TokenType tokenType, string message)
            {
                if (_token.Type != tokenType) throw CreateParseException(message);
            }
            #endregion

            #region private method CreateParseException
            private ParseException CreateParseException(string format, params object[] args)
            {
                return CreateParseException(_token.Position, format, args);
            }

            private ParseException CreateParseException(int position, string format, params object[] args)
            {
                return new ParseException(string.Format(CultureInfo.CurrentCulture, format, args), position);
            }

            internal static void AdjustTypeList(string key, Type type)
            {
                var item = __predefinedTypes.FirstOrDefault(x => x.Alias == key);
                if (item != null)
                    item.Type = type;
                if (__keywordsDictionary.ContainsKey(key))
                    __keywordsDictionary[key] = type;
            }
            #endregion
        }
        #endregion
        #endregion

        #region ToMultiline
        public static string ToMultiline(this string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            //
            return value.Replace(Environment.NewLine, "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
        }
        #endregion

        #region ToWebMultiline
        public static string ToWebMultiline(this string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            //
            return value.Replace(Environment.NewLine, "\n").Replace("\r", "\n").Replace("\n", "</br>");
        }
        #endregion

        #region Type init

        private static object _locker = new object();

        public static void InitAdditianlTypePRovider(Func<Dictionary<string, Type>> additionalTypes)
        {
            lock(_locker)
            {
                _additionalTypes = additionalTypes;
            }
        }

        private static Func<Dictionary<string, Type>> _additionalTypes;

        #endregion
    }

    public static class DataBinder
    {

        private static readonly char[] expressionPartSeparator = new char[] { '.' };
        private static readonly char[] indexExprStartChars = new char[] { '[', '(' };
        private static readonly char[] indexExprEndChars = new char[] { ']', ')' };

        public static object Eval(object container, string expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            expression = expression.Trim();

            if (expression.Length == 0)
            {
                throw new ArgumentNullException("expression");
            }

            if (container == null)
            {
                return null;
            }

            string[] expressionParts = expression.Split(expressionPartSeparator);

            return Eval(container, expressionParts);
        }

        private static object Eval(object container, string[] expressionParts)
        {
            Debug.Assert((expressionParts != null) && (expressionParts.Length != 0),
                            "invalid expressionParts parameter");

            object prop;
            int i;

            for (prop = container, i = 0; (i < expressionParts.Length) && (prop != null); i++)
            {
                string expr = expressionParts[i];
                bool indexedExpr = expr.IndexOfAny(indexExprStartChars) >= 0;

                if (indexedExpr == false)
                {
                    prop = GetPropertyValue(prop, expr);
                }
                else
                {
                    prop = GetIndexedPropertyValue(prop, expr);
                }
            }

            return prop;
        }

        public static object GetPropertyValue(object container, string propName)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            if (String.IsNullOrEmpty(propName))
            {
                throw new ArgumentNullException("propName");
            }

            object prop = null;

            // get a PropertyDescriptor using case-insensitive lookup 
            var pd = TypeDescriptor.GetProperties(container).Find(propName, true);
            if (pd != null)
            {
                prop = pd.GetValue(container);
            }
            else
            {
                throw new Exception($"SR.DataBinder_Prop_Not_Found {container.GetType()}, {propName}");
            }

            return prop;
        }

        public static object GetIndexedPropertyValue(object container, string expr)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            if (String.IsNullOrEmpty(expr))
            {
                throw new ArgumentNullException("expr");
            }

            object prop = null;
            bool intIndex = false;

            int indexExprStart = expr.IndexOfAny(indexExprStartChars);
            int indexExprEnd = expr.IndexOfAny(indexExprEndChars, indexExprStart + 1);

            if ((indexExprStart < 0) || (indexExprEnd < 0) ||
                (indexExprEnd == indexExprStart + 1))
            {
                throw new Exception($"SR.DataBinder_Invalid_Indexed_Expr, {expr}");
            }

            string propName = null;
            object indexValue = null;
            string index = expr.Substring(indexExprStart + 1, indexExprEnd - indexExprStart - 1).Trim();

            if (indexExprStart != 0)
                propName = expr.Substring(0, indexExprStart);

            if (index.Length != 0)
            {
                if (((index[0] == '"') && (index[index.Length - 1] == '"')) ||
                    ((index[0] == '\'') && (index[index.Length - 1] == '\'')))
                {
                    indexValue = index.Substring(1, index.Length - 2);
                }
                else
                {
                    if (Char.IsDigit(index[0]))
                    {
                        // treat it as a number
                        int parsedIndex;
                        intIndex = Int32.TryParse(index, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedIndex);
                        if (intIndex)
                        {
                            indexValue = parsedIndex;
                        }
                        else
                        {
                            indexValue = index;
                        }
                    }
                    else
                    {
                        // treat as a string 
                        indexValue = index;
                    }
                }
            }

            if (indexValue == null)
            {
                throw new ArgumentException($"SR.DataBinder_Invalid_Indexed_Expr {expr}");
            }

            object collectionProp = null;
            if ((propName != null) && (propName.Length != 0))
            {
                collectionProp = GetPropertyValue(container, propName);
            }
            else
            {
                collectionProp = container;
            }

            if (collectionProp != null)
            {
                Array arrayProp = collectionProp as Array;
                if (arrayProp != null && intIndex)
                {
                    prop = arrayProp.GetValue((int)indexValue);
                }
                else if ((collectionProp is IList) && intIndex)
                {
                    prop = ((IList)collectionProp)[(int)indexValue];
                }
                else
                {
                    PropertyInfo propInfo =
                        collectionProp.GetType().GetProperty("Item", BindingFlags.Public | BindingFlags.Instance, null, null, new Type[] { indexValue.GetType() }, null);
                    if (propInfo != null)
                    {
                        prop = propInfo.GetValue(collectionProp, new object[] { indexValue });
                    }
                    else
                    {
                        throw new ArgumentException($"SR.DataBinder_No_Indexed_Accessor, {collectionProp.GetType()}");
                    }
                }
            }

            return prop;
        }
    }
}