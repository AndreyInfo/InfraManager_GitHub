using Inframanager.BLL.EventsOld;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Location.Floors;

//TODO передалать на Event.BLL
internal sealed class FloorConfigureEventBuilder : IConfigureEventBuilder<Floor>
{
    public void Configure(IBuildEvent<Floor> config)
    {
        config.HasId(x => x.IMObjID);
        config.HasEntityName(nameof(Floor));
        config.HasInstanceName(x => nameof(Floor));
    }

    public void WhenDeleted(IBuildEventOperation<Floor> deleteConfig)
    {
        deleteConfig.HasOperation(OperationID.Floor_Delete, floor => $"Удален [Этаж] '{floor.Name}'");
    }

    public void WhenInserted(IBuildEventOperation<Floor> insertConfig)
    {
        insertConfig.HasOperation(OperationID.Floor_Add, floor => $"Добавлен [Этаж] '{floor.Name}'");
    }

    public void WhenUpdated(IBuildEventOperation<Floor> updateConfig)
    {
        updateConfig.HasOperation(OperationID.Floor_Update, floor => $"Обновлен [Этаж] '{floor.Name}'");
    }
}
