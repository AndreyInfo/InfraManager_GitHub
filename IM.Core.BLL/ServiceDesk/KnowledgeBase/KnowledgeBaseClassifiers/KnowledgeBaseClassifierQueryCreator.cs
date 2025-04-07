using Inframanager.BLL;
using InfraManager.BLL.KnowledgeBase.KnowledgeBaseClassifiers;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.KnowledgeBase.KnowledgeBaseClassifiers;

public class KnowledgeBaseClassifierQueryCreator : IBuildEntityQuery<KBArticleFolder, KnowledgeBaseClassifierDetails,
        KnowledgeBaseClassifierFilter>,
    ISelfRegisteredService<IBuildEntityQuery<KBArticleFolder, KnowledgeBaseClassifierDetails,
        KnowledgeBaseClassifierFilter>>
{
    private readonly IReadonlyRepository<KBArticleFolder> _repository;

    public KnowledgeBaseClassifierQueryCreator(IReadonlyRepository<KBArticleFolder> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<KBArticleFolder> Query(KnowledgeBaseClassifierFilter filterBy)
    {
        return _repository.With(x => x.Parent).With(x => x.Expert).Query(x => x.ParentID == filterBy.ParentID);
    }
}