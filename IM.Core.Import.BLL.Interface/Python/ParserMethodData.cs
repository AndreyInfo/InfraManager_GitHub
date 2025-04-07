
namespace IM.Core.Import.BLL.Interface.Import;

public record  ParserMethodData(string Name,string SourceScript, Func<dynamic,IEnumerable<dynamic>, dynamic> Function)
{
}