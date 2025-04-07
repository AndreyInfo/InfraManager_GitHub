using Inframanager.BLL.EventsOld;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Calls;

public class CallTypeConfigureEventBuilder : IConfigureEventBuilder<CallType>
{
    private string BoolConverter(bool value) => value ? "Да" : "Нет";
    
    public void Configure(IBuildEvent<CallType> config)
    {
        config.HasId(x => x.ID);
        config.HasInstanceName(x => nameof(x.Name));
        config.HasEntityName(nameof(CallType));

        config.HasProperty(x => x.EventHandlerName);
        config.HasProperty(x => x.Name);
        config.HasProperty(x => x.VisibleInWeb).HasConverter(BoolConverter);
        config.HasProperty(x=>x.UseWorkflowSchemeFromAttendance).HasConverter(BoolConverter);
    }

    public void WhenInserted(IBuildEventOperation<CallType> insertConfig)
    {
        insertConfig.HasOperation(OperationID.CallType_Add, CallType => $"Добавлен [Тип заявки] '{CallType.Name}");
    }

    public void WhenUpdated(IBuildEventOperation<CallType> updateConfig)
    {
        updateConfig.HasOperation(OperationID.CallType_Update, CallType => $"Сохранен [Тип заявки] '{CallType.Name}");
    }

    public void WhenDeleted(IBuildEventOperation<CallType> deleteConfig)
    {
    }
}