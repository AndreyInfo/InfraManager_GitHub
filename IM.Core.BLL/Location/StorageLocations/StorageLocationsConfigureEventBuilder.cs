using Inframanager.BLL.EventsOld;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Location.StorageLocations;

internal class StorageLocationsConfigureEventBuilder : IConfigureEventBuilder<StorageLocation>
{
    public void Configure(IBuildEvent<StorageLocation> config)
    {
        config.HasId(x => x.ID);
        config.HasEntityName(nameof(StorageLocation));
        config.HasInstanceName(x => nameof(StorageLocation));
    }

    public void WhenInserted(IBuildEventOperation<StorageLocation> insertConfig)
    {
        insertConfig.HasOperation(OperationID.StorageLocation_Add, storageLocation => $"Добавлен [Склад] '{storageLocation.Name}");
    }

    public void WhenUpdated(IBuildEventOperation<StorageLocation> updateConfig)
    {
        updateConfig.HasOperation(OperationID.StorageLocation_Update, storageLocation => $"Сохранен [Склад] '{storageLocation.Name}");
    }

    public void WhenDeleted(IBuildEventOperation<StorageLocation> deleteConfig)
    {
        deleteConfig.HasOperation(OperationID.StorageLocation_Delete, storageLocation => $"Удален [Склад] '{storageLocation.Name}");
    }
}
