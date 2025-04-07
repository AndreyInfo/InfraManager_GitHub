using System.Text;
using IM.Core.Import.BLL.Interface.Import.Log;

namespace IM.Core.Import.BLL.Import;

public class PythonDataConverter
{
    private ILogAdapter _logger;
    private const int HexLength = 2;
    
    private readonly HashSet<char> _letters =
        ("abcdefghijklmnopqrstuvwxywzABCDEFGHIJKLMNOPQRSTUVWXYZабвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛММНОПРСТУФХЦЧШЩЪЫЬЭЮЯ").ToHashSet();

    private readonly HashSet<char> _digits = ("0123456789").ToHashSet();
    private readonly HashSet<char> _signs = ("_=+.,;/- ").ToHashSet();

    private HashSet<char> special = "\"\\\'\"".ToHashSet();

    private readonly HashSet<string> _keywords = new()
    {
        "False", "None", "True", "and", "as", "assert", "async", "await", "break", "class", "continue",
        "def", "del", "elif", "else", "except", "finally", "for", "from", "global", "if", "import", "in", "is",
        "lambda",
        "nonlocal", "not", "or", "pass", "raise", "return", "try", "while", "with", "yield"
    };

    public PythonDataConverter(ILogAdapter logger)
    {
        _logger = logger;
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public string? GetFieldName(string? name)
    {
        var trimmedName = name.Trim();
        if (!IsValidName(trimmedName))
            return null;
        return trimmedName;
    }

    public string GetSequence(string name, string?[] values, Encoding? encoding)
    {
        var builder = new StringBuilder();
        foreach (var value in values)
        {
            builder.Append($"'{ConvertString(value,encoding)}'").Append(',');
        }
        
        return $"{name} = [{builder.ToString().TrimEnd(',')}]";
    }

    private string GetFullHex8(string hexValue)
    {
        var length = hexValue.Length;
        var feedLength = HexLength - length;
        
        var stringBuilder = new StringBuilder();
        
        for (int i = 0; i < feedLength; i++)
        {
            stringBuilder.Append('0');
        }

        stringBuilder.Append(hexValue);
        
        return stringBuilder.ToString();
    }
    public string? ConvertString(string? source, Encoding? encoding = null)
    {
        encoding ??= Encoding.GetEncoding("windows-1251");
        var stringBuilder = new StringBuilder();
        
        if (source == null)
            return null;

        var trimmedSource = source.Trim();
        foreach (var c in trimmedSource)
        {
            if (_letters.Contains(c) || _digits.Contains(c) || _signs.Contains(c))
                stringBuilder.Append(c);
            
            else if (special.Contains(c))
            {
                stringBuilder.Append($"\\{c}");
            }
            else
            {
                
                var ordChar =  encoding.GetBytes(new String(new []{c})).Single();
                var shortHexString = ordChar.ToString("X");
                var fullHexString = GetFullHex8(shortHexString);
                stringBuilder.Append($"\\x{fullHexString}");
            }
        }

        return stringBuilder.ToString();
    }
    
    public bool IsValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            _logger.Information("Пустое название");
            return false;
        }

        if (_keywords.Contains(name.Trim()))
        {
            _logger.Information("В качестве названия использовано ключевое слово Python");
            return false;
        }
        var firstLetter = name.First();
        if (!(firstLetter == '_' || _letters.Contains(firstLetter)))
        {
            _logger.Information("Название начинается не с _ или буквы");
            return false;
        }

        if (name.Skip(1).All(x => x == '_' || _digits.Contains(x) || _letters.Contains(x))) 
            return true;
        
        _logger.Information("Название содежит символы не являющиеся буквами или цифрами или _");
        return false;

    }
}