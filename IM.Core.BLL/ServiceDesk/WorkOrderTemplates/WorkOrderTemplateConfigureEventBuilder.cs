using Inframanager.BLL.EventsOld;
using InfraManager.DAL.ServiceDesk.WorkOrders;

namespace InfraManager.BLL.ServiceDesk.WorkOrderTemplates;

public class WorkOrderTemplateConfigureEventBuilder : IConfigureEventBuilder<WorkOrderTemplate>
{
    public void Configure(IBuildEvent<WorkOrderTemplate> config)
    {
        config.HasId(x => x.ID);
        config.HasInstanceName(x => nameof(x.Name));
        config.HasEntityName(nameof(WorkOrderTemplate));
        config.HasProperty(x => x.UserField1);
        config.HasProperty(x => x.UserField2);
        config.HasProperty(x => x.UserField3);
        config.HasProperty(x => x.UserField4);
        config.HasProperty(x => x.UserField5);
        config.HasProperty(x => x.Description);
        config.HasProperty(x => x.ManhoursNormInMinutes);
    }

    public void WhenInserted(IBuildEventOperation<WorkOrderTemplate> insertConfig)
    {
        insertConfig.HasOperation(
            OperationID.WorkOrderTemplate_Add,
            WorkOrderTemplate => $"Добавлен [Шаблон работы] '{WorkOrderTemplate.Name}'");
    }

    public void WhenUpdated(IBuildEventOperation<WorkOrderTemplate> updateConfig)
    {
        updateConfig.HasOperation(
            OperationID.WorkOrderTemplate_Update,
            WorkOrderTemplate => $"Сохранен [Шаблон работы] '{WorkOrderTemplate.Name}'");
    }

    public void WhenDeleted(IBuildEventOperation<WorkOrderTemplate> deleteConfig)
    {
        deleteConfig.HasOperation(
           OperationID.WorkOrderTemplate_Delete,
           WorkOrderTemplate => $"Удален [Шаблон работы] '{WorkOrderTemplate.Name}'");
    }
}