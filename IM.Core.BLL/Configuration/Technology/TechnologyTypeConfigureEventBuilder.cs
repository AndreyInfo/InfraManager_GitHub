using Inframanager.BLL.EventsOld;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Configuration;

namespace InfraManager.BLL.Configuration.Technology;

public class TechnologyTypeConfigureEventBuilder : IConfigureEventBuilder<TechnologyType>
{
    public void Configure(IBuildEvent<TechnologyType> config)
    {
        config.HasInstanceName(x => x.Name);
        config.HasEntityName(nameof(TechnologyType));
    }

    public void WhenInserted(IBuildEventOperation<TechnologyType> insertConfig)
    {
        insertConfig.HasOperation(OperationID.TelephoneType_Add, TechnologyType => $"Добавлен [Тип технологий] '{TechnologyType.Name}");
    }

    public void WhenUpdated(IBuildEventOperation<TechnologyType> updateConfig)
    {
        updateConfig.HasOperation(OperationID.TelephoneType_Update, TechnologyType => $"Сохранен [Тип технологий] '{TechnologyType.Name}");
    }

    public void WhenDeleted(IBuildEventOperation<TechnologyType> deleteConfig)
    {
    }
}