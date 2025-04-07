using Inframanager.BLL.EventsOld;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.ServiceUnits;

internal class ServiceUnitsConfigureEventBuilder : IConfigureEventBuilder<ServiceUnit>
{
    public void Configure(IBuildEvent<ServiceUnit> config)
    {
        config.HasId(x => x.ID);
        config.HasEntityName(nameof(ServiceUnit));
        config.HasInstanceName(x => nameof(ServiceUnit));
    }

    public void WhenInserted(IBuildEventOperation<ServiceUnit> insertConfig)
    {
        insertConfig.HasOperation(OperationID.ServiceUnit_Add, serviceUnit => $"Добавлен [Сервисный блок] '{serviceUnit.Name}");
    }

    public void WhenUpdated(IBuildEventOperation<ServiceUnit> updateConfig)
    {
        updateConfig.HasOperation(OperationID.ServiceUnit_Update, serviceUnit => $"Сохранен [Сервисный блок] '{serviceUnit.Name}");
    }

    public void WhenDeleted(IBuildEventOperation<ServiceUnit> deleteConfig)
    {
        deleteConfig.HasOperation(OperationID.ServiceUnit_Delete, serviceUnit => $"Удален [Сервисный блок] '{serviceUnit.Name}");
    }
}
