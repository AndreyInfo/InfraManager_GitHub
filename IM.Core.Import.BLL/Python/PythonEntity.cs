using System.Dynamic;

namespace IM.Core.Import.BLL.Import;

public class PythonEntity:DynamicObject
{
 
    private readonly IReadOnlyDictionary<string, string?>? _data;
    private readonly Action<string>? _logInformation;
    private ILookup<string, string?>? _ignoreCaseData;

    public PythonEntity(IReadOnlyDictionary<string, string?>? data, Action<string>? logInformation)
    {
        _data = data;
        _logInformation = logInformation;
    }

    private static string GetFormattedString(string value)
    {
        return string.Format("Поле {0} не найдено. В выражении будет использовано значение \"\"",value);
    }

    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        var binderName = binder.Name;
        if (_data?.ContainsKey(binderName) ?? false)
        {
            result = _data[binderName];
        }
        else
        {
            result = string.Empty;
            _logInformation?.Invoke(GetFormattedString(binderName));
        }

        return true;
    }
}