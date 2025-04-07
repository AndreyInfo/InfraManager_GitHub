using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.BLL.KnowledgeBase.KnowledgeBaseClassifiers;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.KnowledgeBase.KnowledgeBaseClassifiers;

public class KnowledgeBaseClassifierLoader : ILoadEntity<Guid, KBArticleFolder, KnowledgeBaseClassifierDetails>,
    ISelfRegisteredService<ILoadEntity<Guid, KBArticleFolder, KnowledgeBaseClassifierDetails>>
{
    private readonly IReadonlyRepository<KBArticleFolder> _finder; 

    public KnowledgeBaseClassifierLoader(IReadonlyRepository<KBArticleFolder> finder)
    {
        _finder = finder;
    }
    
    public Task<KBArticleFolder> LoadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _finder.With(x => x.Parent).With(x => x.Expert) .FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
               ?? throw new ObjectNotFoundException($"Knowledge Base Classifier was not found with id = {id}");
    }
}