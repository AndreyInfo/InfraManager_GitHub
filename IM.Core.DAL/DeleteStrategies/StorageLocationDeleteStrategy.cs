using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Location;

namespace InfraManager.DAL.DeleteStrategies;

internal class StorageLocationDeleteStrategy : IDeleteStrategy<StorageLocation>
                                               , ISelfRegisteredService<IDeleteStrategy<StorageLocation>>
{
    private readonly IRepository<StorageLocationReference> _storageLocationReferences;
    private readonly DbSet<StorageLocation> _storageLocations;

    public StorageLocationDeleteStrategy(IRepository<StorageLocationReference> storageLocationReferences,
        DbSet<StorageLocation> storageLocations)
    {
        _storageLocationReferences = storageLocationReferences;
        _storageLocations = storageLocations;
    }

    public void Delete(StorageLocation entity)
    {
        entity.StorageLocationReferences.ForEach(c=> _storageLocationReferences.Delete(c));
        _storageLocations.Remove(entity);
    }
}
