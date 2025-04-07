using Inframanager.BLL;
using InfraManager.BLL.KnowledgeBase;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using Enumerable = System.Linq.Enumerable;

namespace InfraManager.BLL.ServiceDesk.KB.Statuses;

internal class KBArticleStatusQueryBuilder : IBuildEntityQuery<KnowledgeBaseArticleStatus, KBArticleStatusDetails, LookupListFilter>,
    ISelfRegisteredService<IBuildEntityQuery<KnowledgeBaseArticleStatus, KBArticleStatusDetails, LookupListFilter>>
{
    private readonly IRepository<KnowledgeBaseArticleStatus> _repository;
    
    public KBArticleStatusQueryBuilder(IRepository<KnowledgeBaseArticleStatus> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<KnowledgeBaseArticleStatus> Query(LookupListFilter filterBy)
    {
        var query = _repository.Query();

        if (!string.IsNullOrEmpty(filterBy.SearchName))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterBy.SearchName.ToLower()));
        }

        return query;
    }
}