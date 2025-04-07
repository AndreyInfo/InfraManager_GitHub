namespace IM.Core.Import.BLL.Interface.Import.Models;

public interface IModelDataTryGet
{
    bool TryGetValue(string key, out string value);
    //
    
}