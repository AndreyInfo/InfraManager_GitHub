using System.Data;
using System.Text;
using IM.Core.Import.BLL.Interface.Import.Log;

namespace IM.Core.Import.BLL.Import;

public class PythonDictionaryBuilder
{
    private readonly Dictionary<string, string> _data = new();
    private readonly PythonDataConverter _converter;
    private readonly ILocalLogger<PythonDictionaryBuilder> _logger;

    public PythonDictionaryBuilder(ILocalLogger<PythonDictionaryBuilder> logger)
    {
        _converter = new PythonDataConverter(logger);
        _logger = logger;
    }

    public void AddRecord(string? name, string? value, Encoding? encoding)
    {
        var validName = _converter.GetFieldName(name);
        if (validName == null)
            throw new DataException($"{validName} не может быть названием поля в Python. Пропуск");
        var validData = _converter.ConvertString(value, encoding);
        _data[validName] = validData ?? string.Empty;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append('{');
        foreach (var keyValue in _data)
        {
            builder.Append(keyValue.Key).Append(" : ").Append(keyValue.Value).Append(", ");
        }

        builder.Append('}');
        return builder.ToString();
    }
}