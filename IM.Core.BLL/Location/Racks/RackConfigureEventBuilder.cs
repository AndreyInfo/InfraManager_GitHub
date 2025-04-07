using Inframanager.BLL.EventsOld;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.Location.Racks;

//TODO передалать на Event.BLL
internal sealed class RackConfigureEventBuilder : IConfigureEventBuilder<Rack>
{
    public void Configure(IBuildEvent<Rack> config)
    {
        config.HasId(x => x.IMObjID);
        config.HasEntityName(nameof(Rack));
        config.HasInstanceName(x => nameof(Rack));
    }

    public void WhenDeleted(IBuildEventOperation<Rack> deleteConfig)
    {
        deleteConfig.HasOperation(OperationID.Rack_Delete, rack => $"Удален [Шкаф] '{rack.Name}");
    }

    public void WhenInserted(IBuildEventOperation<Rack> insertConfig)
    {
        insertConfig.HasOperation(OperationID.Rack_Add, rack => $"Добавлен [Шкаф] '{rack.Name}");
    }

    public void WhenUpdated(IBuildEventOperation<Rack> updateConfig)
    {
        updateConfig.HasOperation(OperationID.Rack_Update, rack => $"Обновлен [Шкаф] '{rack.Name}");
    }
}
