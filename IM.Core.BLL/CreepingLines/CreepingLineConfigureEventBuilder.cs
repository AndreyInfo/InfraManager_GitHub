using Inframanager.BLL.EventsOld;
using InfraManager.DAL;

namespace InfraManager.BLL
{
    public class CreepingLineConfigureEventBuilder : 
        IConfigureEventBuilder<CreepingLine> 
    {
        public void Configure(IBuildEvent<CreepingLine> config)
        {
            config.HasEntityName(nameof(CreepingLine));
            config.HasId(x => x.ID);
            config.HasInstanceName(x => x.Name);
            config.HasProperty(x => x.Name);
            config.HasProperty(x => x.Visible);
        }

        public void WhenDeleted(IBuildEventOperation<CreepingLine> deleteConfig)
        {
            deleteConfig.HasOperation(
                            OperationID.CreepingLine_Delete, // TODO: Это легаси поведение надо пофиксить
                            creepingLine => $"Удалено [Сообщение бегущей строки] '{creepingLine.Name}'");
        }

        public void WhenInserted(IBuildEventOperation<CreepingLine> insertConfig)
        {
            insertConfig.HasOperation(
                                        OperationID.CreepingLine_Add,
                                        creepingLine => $"Добавлено [Сообщение бегущей строки] '{creepingLine.Name}'");
        }

        public void WhenUpdated(IBuildEventOperation<CreepingLine> updateConfig)
        {
            updateConfig.HasOperation(
                            OperationID.CreepingLine_Update,
                            creepingLine => $"Изменено [Сообщение бегущей строки] '{creepingLine.Name}'");
        }
    }
}
