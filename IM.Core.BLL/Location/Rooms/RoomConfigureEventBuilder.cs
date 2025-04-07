using Inframanager.BLL.EventsOld;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Location.Rooms;

//TODO передалать на Event.BLL
internal sealed class RoomConfigureEventBuilder : IConfigureEventBuilder<Room>
{
    public void Configure(IBuildEvent<Room> config)
    {
        config.HasId(x => x.IMObjID);
        config.HasEntityName(nameof(Room));
        config.HasInstanceName(x => nameof(Room));
    }

    public void WhenDeleted(IBuildEventOperation<Room> deleteConfig)
    {
        deleteConfig.HasOperation(OperationID.Room_Delete, room => $"Удалена [Комната] '{room.Name}'");
    }

    public void WhenInserted(IBuildEventOperation<Room> insertConfig)
    {
        insertConfig.HasOperation(OperationID.RoomType_Add, room => $"Добавлена [Комната] '{room.Name}'");
    }

    public void WhenUpdated(IBuildEventOperation<Room> updateConfig)
    {
        updateConfig.HasOperation(OperationID.Room_Update, room => $"Обновлена [Комната] '{room.Name}'");
    }
}
