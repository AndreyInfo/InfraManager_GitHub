using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.KnowledgeBase
{
    internal class ReferenceEventWriterConfigurer : IConfigureEventWriter<KBArticleReference, KBArticleReference>
    {
        public void Configure(IEventWriter<KBArticleReference, KBArticleReference> writer)
        {
            writer.HasParentObject((x, subj) => new InframanagerObject(x.ObjectId, x.ObjectClassID));
        }
    }
}
