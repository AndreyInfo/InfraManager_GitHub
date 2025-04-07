using InfraManager;

namespace IM.Core.Import.BLL.Import;

public interface IImportMapperComparer<TDetails, TEntity>
{
    Func<TDetails, TEntity, bool> IsModelChanged(ObjectType flags, bool withRemoved);
}