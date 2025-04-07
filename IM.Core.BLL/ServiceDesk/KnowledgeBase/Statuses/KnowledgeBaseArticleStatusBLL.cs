using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.BLL.KnowledgeBase;
using InfraManager.BLL.Localization;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.ResourcesArea;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ServiceDesk.KB.Statuses;

internal class KnowledgeBaseArticleStatusBLL :
    StandardBLL<Guid, KnowledgeBaseArticleStatus, LookupData, KBArticleStatusDetails, LookupListFilter>,
    IKnowledgeBaseArticleStatusBLL,
    ISelfRegisteredService<IKnowledgeBaseArticleStatusBLL>
{
    private readonly IRepository<KBArticle> _articleRepository;
    private readonly IRemoveEntityBLL<Guid, KnowledgeBaseArticleStatus> _removeBLL;
    private readonly IUnitOfWork _saveChanges;
    private readonly ILocalizeText _localizer;
        
    public KnowledgeBaseArticleStatusBLL(
        IRepository<KnowledgeBaseArticleStatus> repository,
        ILogger<KnowledgeBaseArticleStatusBLL> logger,
        IRepository<KBArticle> articleRepository,
        IGetEntityArrayBLL<Guid, KnowledgeBaseArticleStatus, KBArticleStatusDetails, LookupListFilter> getEntityArray,
        IModifyEntityBLL<Guid,KnowledgeBaseArticleStatus,LookupData,KBArticleStatusDetails> modifyEntityBLL,
        IRemoveEntityBLL<Guid, KnowledgeBaseArticleStatus> removeBLL,
        IGetEntityBLL<Guid,KnowledgeBaseArticleStatus,KBArticleStatusDetails> getEntityBLL,
        IInsertEntityBLL<KnowledgeBaseArticleStatus, LookupData> insertEntity,
        IUnitOfWork saveChanges,
        ICurrentUser currentUser,
        IBuildObject<KBArticleStatusDetails,KnowledgeBaseArticleStatus> buildObject,
        ILocalizeText localizer)
        : base(
            repository,
            logger,
            saveChanges,
            currentUser,
            buildObject,
            insertEntity,
            modifyEntityBLL,
            removeBLL,
            getEntityBLL,
            getEntityArray)
    {
        _articleRepository = articleRepository;
        _removeBLL = removeBLL;
        _saveChanges = saveChanges;
        _localizer = localizer;
    }
    
    public async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var isUsing = await _articleRepository.AnyAsync(x => x.ArticleStatusID == id, cancellationToken);
        if (isUsing)
        {
            throw new InvalidObjectException(
                await _localizer.LocalizeAsync(nameof(Resources.Cant_Delete_System_KnowledgeArticleStatus),
                    cancellationToken));
        }

        await _removeBLL.RemoveAsync(id, cancellationToken);
        await _saveChanges.SaveAsync(cancellationToken);
    }
}