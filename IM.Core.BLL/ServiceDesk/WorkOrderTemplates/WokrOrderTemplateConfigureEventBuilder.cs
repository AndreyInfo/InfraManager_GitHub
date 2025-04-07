using Inframanager.BLL.EventsOld;
using InfraManager.DAL.ServiceDesk.WorkOrders;

namespace InfraManager.BLL.ServiceDesk.WorkOrderTemplates;

//TODO переделать на Event.BLL
internal sealed class WokrOrderTemplateConfigureEventBuilder : IConfigureEventBuilder<WorkOrderTemplate>
{
    public void Configure(IBuildEvent<WorkOrderTemplate> config)
    {
        config.HasId(x => x.ID);
        config.HasEntityName(nameof(WorkOrderTemplate));
        config.HasInstanceName(x => nameof(WorkOrderTemplate));
    }

    public void WhenDeleted(IBuildEventOperation<WorkOrderTemplate> deleteConfig)
    {
        deleteConfig.HasOperation(OperationID.WorkOrderTemplate_Delete, workOrderTemplate => $"Удален [Шаблон Задания] '{workOrderTemplate.Name}");
    }

    public void WhenInserted(IBuildEventOperation<WorkOrderTemplate> insertConfig)
    {
        insertConfig.HasOperation(OperationID.WorkOrderTemplate_Add, workOrderTemplate => $"Добавлен [Шаблон Задания] '{workOrderTemplate.Name}");
    }

    public void WhenUpdated(IBuildEventOperation<WorkOrderTemplate> updateConfig)
    {
        updateConfig.HasOperation(OperationID.WorkOrderTemplate_Update, workOrderTemplate => $"Сохранен [Шаблон Задания] '{workOrderTemplate.Name}");
    }
}
