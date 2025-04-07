using System.Text;
using IM.Core.Import.BLL.Interface.Import.Log;

namespace IM.Core.Import.BLL.Import;

public class PythonScriptBuilder
{
   
    
    private readonly PivottedStringBuilder _pivottedBuilder;
    private readonly PythonDataConverter _converter;

    public PythonScriptBuilder(ILogAdapter logger)
    {
        var stringBuilder = new StringBuilder();
        _pivottedBuilder = new PivottedStringBuilder(stringBuilder);
        _converter = new PythonDataConverter(logger);
    }

    public bool BeginFunction(string functionName, params string[] parameters)
    {
        if (!_converter.IsValidName(functionName))
            return false;
        
        _pivottedBuilder.Append("def").Append(' ').Append(functionName);
        
        _pivottedBuilder.Append('(');
        if (parameters.Any())
            _pivottedBuilder.Append(parameters.First());
        
        foreach (var parameterName in parameters.Skip(1))
        {
            _pivottedBuilder.Append(", ").Append(parameterName);
        }

        _pivottedBuilder.Append(')');
        
        _pivottedBuilder.AppendLine(':');
        
        _pivottedBuilder.AddPivot();
        return true;
    }

    public void AddLine(string line)
    {
        _pivottedBuilder.AppendLine(line);
    }

    public void EndFunction()
    {
        _pivottedBuilder.SubPivot();
        _pivottedBuilder.AppendLine();
    }

    public bool AddVariable(string? name, object? value, Encoding? encoding)
    {
        var validName = _converter.GetFieldName(name);
        if (validName == null)
            return false;
        var data = value?.ToString()?.Trim();
        if (string.IsNullOrWhiteSpace(data))
            data = "''";
        _pivottedBuilder.Append(validName).Append(" = ").AppendLine(data);
        return true;
    }

    public override string ToString()
    {
        return _pivottedBuilder.ToString();
    }
}