using InfraManager;
using System.Text;
using IM.Core.Import.BLL.Import;

namespace IM.Core.Import.BLL.Interface.Import.Models.Settings;

public interface IScriptDataParser<T>
{
    ParserData GetParserData(IEnumerable<ScriptDataDetails<T>> scripts,
        Encoding? encoding = null);

    Dictionary<string, string?> ParseToDictionary(
        ParserData parser, 
        string[] fieldName,
        string[] data, 
        IEnumerable<IReadOnlyDictionary<string,string>> parents = null);

    Dictionary<string, object?> ParseToObjectDictionary(
        ParserData parser,
        string[] fieldName,
        string[] data,
        IEnumerable<IReadOnlyDictionary<string, string>> parents = null);

    bool TryParseObjectDictionary(ParserData parser,
        IReadOnlyDictionary<string, string> source,
        List<Dictionary<string, string>> parents,
        List<string> parentPassedAttributes,
        List<string> passedAttributes,
        out Dictionary<string, object?> result,
        out string notFoundField, out CheckerTypeEnum typeEnum);
}