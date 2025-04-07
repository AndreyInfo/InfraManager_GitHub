using IM.Core.Import.BLL.Import.Array;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.View;

namespace IM.Core.Import.BLL.Import;

public interface IImportParameterLogic<TDetails, TEntity, TEnum>
where TEnum:Enum
{
    
    Func<TDetails, IIsSet> GetDetailsKey(TEnum parameter);
    Func<TDetails, bool> ValidateBeforeInitFunc(AdditionalTabDetails parameter);
    ImportKeyData<TDetails,TEntity> GetModelKey(TEnum parameter);
    Func<TDetails, bool> ValidateAfterInitFunc();
    Func<TEntity,bool> ValidateBeforeCreate();
}