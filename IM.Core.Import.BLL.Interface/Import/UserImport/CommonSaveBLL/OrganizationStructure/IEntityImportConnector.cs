using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.OrganizationStructure;

public interface IEntityImportConnector<TDetails, TEntity>
{
    Task SaveOrUpdateEntitiesAsync(ICollection<TDetails> users,
        IAdditionalParametersForSelect? additional,
        ImportData<TDetails, TEntity> data,
        ErrorStatistics<TDetails> errorEntries,
        CancellationToken cancellationToken = default);

    void LogStatistics(ErrorStatistics<TDetails> errorEntities, ImportData<TDetails, TEntity> importData);
}