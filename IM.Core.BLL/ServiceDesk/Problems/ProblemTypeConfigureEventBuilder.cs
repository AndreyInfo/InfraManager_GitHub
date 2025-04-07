using Inframanager.BLL.EventsOld;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Problems;

public class ProblemTypeConfigureEventBuilder : IConfigureEventBuilder<ProblemType>
{
    public void Configure(IBuildEvent<ProblemType> config)
    {
        config.HasEntityName(nameof(ProblemType));
        config.HasInstanceName(_ => nameof(ProblemType));
        config.HasId(x => x.ID);
    }

    public void WhenInserted(IBuildEventOperation<ProblemType> insertConfig)
    {
        insertConfig.HasOperation(OperationID.ProblemType_Add,
            problemType => $"Добавлен [Тип проблемы] '{problemType.Name}'");
    }

    public void WhenUpdated(IBuildEventOperation<ProblemType> updateConfig)
    {
        updateConfig.HasOperation(OperationID.ProblemType_Add,
            problemType => $"Сохранен [Тип проблемы] '{problemType.Name}'");
    }

    public void WhenDeleted(IBuildEventOperation<ProblemType> deleteConfig)
    {
        throw new System.NotImplementedException();
    }
}