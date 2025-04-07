using Inframanager.BLL.EventsOld;
using InfraManager.DAL.Location;



namespace InfraManager.BLL.Location.Buildings;

//TODO передалать на Event.BLL
internal class BuildingConfigureEventBuilder : IConfigureEventBuilder<Building>
{
    public void Configure(IBuildEvent<Building> config)
    {
        config.HasId(x => x.IMObjID);
        config.HasEntityName(nameof(Building));
        config.HasInstanceName(x => nameof(Building));
    }

    public void WhenDeleted(IBuildEventOperation<Building> deleteConfig)
    {
        deleteConfig.HasOperation(OperationID.Building_Delete, building => $"Удалено [Здание] '{building.Name}");
    }

    public void WhenInserted(IBuildEventOperation<Building> insertConfig)
    {
        insertConfig.HasOperation(OperationID.Building_Add, building => $"Добавлено [Здание] '{building.Name}");
    }

    public void WhenUpdated(IBuildEventOperation<Building> updateConfig)
    {
        updateConfig.HasOperation(OperationID.Building_Update, building => $"Обновлено [Здание] '{building.Name}");
    }
}
