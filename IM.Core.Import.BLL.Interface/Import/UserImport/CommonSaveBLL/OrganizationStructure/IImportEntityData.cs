using System.Diagnostics.CodeAnalysis;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using InfraManager;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Import;

public interface IImportEntityData<TDetails, TEntity, TEnum>
where TEnum:Enum
{
    Func<ICollection<TDetails>, IAdditionalParametersForSelect, CancellationToken, Task<IEnumerable<TEntity>>> GetComparerFunction(TEnum parameter, bool getRemoved);
    Task<IEnumerable<IDuplicateKeyData<TDetails,TEntity>>> GetUniqueKeys(ObjectType flags, bool getRemoved, CancellationToken token);
}