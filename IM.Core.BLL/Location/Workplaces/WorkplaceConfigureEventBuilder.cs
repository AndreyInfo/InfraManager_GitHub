using Inframanager.BLL.EventsOld;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Location.Buildings;

internal sealed class WorkplaceConfigureEventBuilder : IConfigureEventBuilder<Workplace>
{
    public void Configure(IBuildEvent<Workplace> config)
    {
        config.HasId(x => x.IMObjID);
        config.HasEntityName(nameof(Workplace));
        config.HasInstanceName(x => nameof(Workplace));
    }

    public void WhenDeleted(IBuildEventOperation<Workplace> deleteConfig)
    {
        deleteConfig.HasOperation(OperationID.Workplace_Delete, workplace => $"Удалено [Рабочее место] '{workplace.Name}'");
    }

    public void WhenInserted(IBuildEventOperation<Workplace> insertConfig)
    {
        insertConfig.HasOperation(OperationID.Workplace_Add, workplace => $"Добавлено [Рабочее место] '{workplace.Name}'");
    }

    public void WhenUpdated(IBuildEventOperation<Workplace> updateConfig)
    {
        updateConfig.HasOperation(OperationID.Workplace_Update, workplace => $"Обновлено [Рабочее место] '{workplace.Name}'");
    }
}
