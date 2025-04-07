using System.Data;
using System.Dynamic;
using System.Text;
using IM.Core.Import.BLL.Import.CheckNotFoundFields;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Log;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using IronPython.Hosting;
using Microsoft.Extensions.Logging;
using Mono.Unix.Native;

namespace IM.Core.Import.BLL.Import;

public class PythonScriptDataParser<T> : IScriptDataParser<T>
{
    private readonly ILocalLogger<PythonScriptDataParser<T>> _logger;
    private volatile int _i = 0;

    public PythonScriptDataParser(ILocalLogger<PythonScriptDataParser<T>> logger)
    {
        _logger = logger;
      
    }
    
    public ParserData GetParserData(IEnumerable<ScriptDataDetails<T>> scripts, Encoding? encoding=null)
    {
        var result = new ParserData();
        foreach (var script in scripts)
        {
            var currentFieldName = script.FieldName;
            
            var pythonScriptBuilder = new PythonScriptBuilder(_logger);

            pythonScriptBuilder.BeginFunction(currentFieldName, "record","parents");
        
            if (!pythonScriptBuilder.AddVariable(currentFieldName, script.Script, encoding))
            {
                _logger.Information($"{currentFieldName} не может быть названием в Python. Пропуск.");
                continue;
            }
            pythonScriptBuilder.AddLine($"return {currentFieldName}");
            pythonScriptBuilder.EndFunction();
            
            var engine = Python.CreateEngine();
            var scope = engine.CreateScope();
            var expression = pythonScriptBuilder.ToString();

            try
            {
                engine.Execute(expression, scope);
                var resultFunc = (Func<dynamic, IEnumerable<dynamic>, dynamic>)scope.GetVariable(currentFieldName);
                result.Add(currentFieldName, expression, resultFunc);
            }
            catch (Microsoft.Scripting.SyntaxErrorException e)
            {
                _logger.Information($"Возникла ошибка при парсинге скрипта {script.Script} для поля {currentFieldName}: {e.Message}");
                throw;
            }
            catch (Exception e)
            {
                _logger.Error(e,"Ошибка при инерпритации.");
                throw;
            }
           
        }

        return result;
    }

    public Dictionary<string, string?> ParseToDictionary(
        ParserData parser,
        string[] fieldName,
        string[] data,
        IEnumerable<IReadOnlyDictionary<string, string>> parents = null)
    {
        Action<string>? logError = y => _logger.Information(y);
        var convertToDictionary = GetScriptResults(parser, fieldName, data, parents, x=> GetExpando(x, logError));
        var toDictionary = convertToDictionary.ToDictionary(x => x.Key, x => x.Value?.ToString() ?? string.Empty);
        return toDictionary;
    }
    
    public Dictionary<string, object?> ParseToObjectDictionary(
        ParserData parser,
        string[] fieldName,
        string[] data,
        IEnumerable<IReadOnlyDictionary<string, string>> parents = null)
    {
        Action<string>? logError = y => _logger.Information(y);
        return GetScriptResults(parser, fieldName, data, parents, x=> GetExpando(x, logError));
    }

    private Dictionary<string, object?> GetScriptResults(ParserData parser, string[] fieldName, string[] data, IEnumerable<IReadOnlyDictionary<string, string>> parents, Func<IReadOnlyDictionary<string, string>, dynamic> getDynamic)
    {
        var length = fieldName.Length;

        if (length != data.Length)
            throw new DataException("Размер заголовка не соответствует размеру данных.");
        var record = fieldName.Zip(data).ToDictionary(x => x.First, x => x.Second);

        dynamic RecordGetDynamic(IReadOnlyDictionary<string, string> x, CheckerTypeEnum y) => getDynamic(x);
        return ConvertToDictionary(parser, parents, RecordGetDynamic, RecordGetDynamic, record);
    }

    private Dictionary<string, object?> ConvertToDictionary(ParserData parser,
        IEnumerable<IReadOnlyDictionary<string, string>> parents,
        Func<IReadOnlyDictionary<string, string>, CheckerTypeEnum, dynamic> recordGetDynamic,
        Func<IReadOnlyDictionary<string, string>, CheckerTypeEnum, dynamic> parentGetDynamic,
        IReadOnlyDictionary<string, string> record)
    {
        var resultObject = recordGetDynamic(record, CheckerTypeEnum.Record);

        var result = new Dictionary<string, object?>();
        var resultParent = parents?.Select(x => parentGetDynamic(x, CheckerTypeEnum.Parent)).ToList() ?? Enumerable.Empty<dynamic>();
        var parserMethodDatas = parser.GetScripts().ToList();
        foreach (var script in parserMethodDatas)
        {
            object? scriptValue;
            try
            {
                scriptValue = (object?) script.Function(resultObject, resultParent);
            }
            catch (MissingMemberException e)
            {
                _logger.Information(e.Message);
                _logger.Information($"Значение поля {script.Name} установлено в ''");
                scriptValue = string.Empty;
            }
            catch (IndexOutOfRangeException e)
            {
                _logger.Information(e.Message);
                _logger.Information(
                    $"В поле {script.Name} индекс вышел за границы массива. Значение поля установлено в ''");
                scriptValue = string.Empty;
            }

            result[script.Name] = scriptValue;
        }

        return result;
    }

    public bool TryParseObjectDictionary(ParserData parser,
        IReadOnlyDictionary<string, string> source,
        List<Dictionary<string, string>> parents,
        List<string> parentPassedAttributes,
        List<string> passedAttributes,
        out Dictionary<string, object?> result,
        out string notFoundField,
        out CheckerTypeEnum typeEnum)
    {
        try
        {
            dynamic GetDynamic(IReadOnlyDictionary<string, string> x, CheckerTypeEnum y) => GetDynamicObject(x, passedAttributes, y);
            dynamic ParentGetDynamic(IReadOnlyDictionary<string, string> x, CheckerTypeEnum y) => GetDynamicObject(x, parentPassedAttributes, y);
            result = ConvertToDictionary(parser, parents, GetDynamic, ParentGetDynamic, source);
            notFoundField = null;
            typeEnum = default;
            return true;
        }
        catch (FieldNotFoundException e)
        {
            notFoundField = e.FieldName;
            result = null;
            typeEnum = e.CheckerTypeEnum;
        }
        return false;
    }

    private static dynamic GetDynamicObject(IReadOnlyDictionary<string, string> data,
        List<string> passedAttributes,
        CheckerTypeEnum typeEnum)
    {
        return new FieldsChecker(passedAttributes, data, typeEnum);
    }


    private static dynamic GetExpando(IReadOnlyDictionary<string, string?>? record, Action<string>? logError)
    {
        dynamic resultObject = new PythonEntity(record, logError);
        return resultObject;
    }
}