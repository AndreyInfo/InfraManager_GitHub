namespace IM.Core.Import.BLL.Interface.Import.Models;

public interface IModelDataContains
{
    bool ContainsKey(string key);
    
    string this[string value] { get; }
}