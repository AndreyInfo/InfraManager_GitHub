namespace IM.Core.Import.BLL.Interface.Import.Models;

public interface IModelDataKeys
{
    IEnumerable<string> Keys { get; }
    
    string this[string value] { get; }
    
}