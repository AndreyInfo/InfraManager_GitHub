using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Import;

public interface IImportMapper<TDetails, TEntity>
{
    IEnumerable<TEntity> CreateMap(ImportData<TDetails, TEntity> data, IEnumerable<TDetails> details);
    void UpdateMap(ImportData<TDetails, TEntity> data, IEnumerable<(TDetails, TEntity)> updatePairs);
}