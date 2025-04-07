using Inframanager.BLL.EventsOld;
using InfraManager.DAL.Software;

namespace InfraManager.BLL.Software;

// TODO: Переписать с использованием BLL.Events 
public class SoftwareModelConfigureEventBuilder : IConfigureEventBuilder<SoftwareModel>
{
    public void Configure(IBuildEvent<SoftwareModel> config)
    {
        config.HasId(x => x.ID);
        config.HasEntityName(nameof(SoftwareModel));
        config.HasInstanceName(x => nameof(SoftwareModel));
    }

    public void WhenDeleted(IBuildEventOperation<SoftwareModel> deleteConfig)
    {
        deleteConfig.HasOperation(OperationID.SoftwareModel_Delete, model => $"Удалена [Модель ПО] {model.Name}");
    }

    public void WhenInserted(IBuildEventOperation<SoftwareModel> insertConfig)
    {
        insertConfig.HasOperation(OperationID.SoftwareModel_Add, model => $"Добавлена [Модель ПО] {model.Name}");
    }

    public void WhenUpdated(IBuildEventOperation<SoftwareModel> updateConfig)
    {
        updateConfig.HasOperation(OperationID.SoftwareModel_Update, model => $"Сохранена [Модель ПО] {model.Name}");
    }
}
