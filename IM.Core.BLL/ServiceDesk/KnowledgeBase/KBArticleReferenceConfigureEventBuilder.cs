using Inframanager.BLL.EventsOld;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.KB
{
    public class KBArticleReferenceConfigureEventBuilder : IConfigureEventBuilder<KBArticleReference>
    {
        public void Configure(IBuildEvent<KBArticleReference> config)
        {
            config.HasEntityName(nameof(KBArticleReference));
            config.HasInstanceName(x => "KBArticleReference");
        }

        public void WhenDeleted(IBuildEventOperation<KBArticleReference> deleteConfig)
        {
            deleteConfig.HasOperation(OperationID.None, "Связь со статьей БЗ '{0}' удалена.");
        }

        public void WhenInserted(IBuildEventOperation<KBArticleReference> insertConfig)
        {
            insertConfig.HasOperation(OperationID.None, "Связь со статьей БЗ '{0}' добавлена.");
        }

        public void WhenUpdated(IBuildEventOperation<KBArticleReference> updateConfig)
        {
            throw new System.NotImplementedException();
        }
    }
}
