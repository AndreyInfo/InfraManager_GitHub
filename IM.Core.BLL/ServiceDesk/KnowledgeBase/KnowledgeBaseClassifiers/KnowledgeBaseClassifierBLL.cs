using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.BLL.KnowledgeBase.KnowledgeBaseClassifiers;
using InfraManager.BLL.Localization;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.ResourcesArea;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ServiceDesk.KnowledgeBase.KnowledgeBaseClassifiers;

public class KnowledgeBaseClassifierBLL : StandardBLL<Guid, KBArticleFolder, KnowledgeBaseClassifierData,
        KnowledgeBaseClassifierDetails, KnowledgeBaseClassifierFilter>,
    IKnowledgeBaseClassifier,
    ISelfRegisteredService<IKnowledgeBaseClassifier>
{

    private readonly IRemoveEntityBLL<Guid, KBArticleFolder> _removeEntityBLL;
    private readonly IReadonlyRepository<KBArticleReference> _articlesReferences;
    private readonly IInsertEntityBLL<KBArticleFolder, KnowledgeBaseClassifierData> _insertEntity;
    private readonly IGetEntityBLL<Guid, KBArticleFolder, KnowledgeBaseClassifierDetails> _getEntityBLL;
    private readonly ILocalizeText _localize;
    private readonly ILogger<KnowledgeBaseClassifierBLL> _logger;
    #region ctor

    public KnowledgeBaseClassifierBLL(
        IRepository<KBArticleFolder> repository,
        ILogger<KnowledgeBaseClassifierBLL> logger,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        IBuildObject<KnowledgeBaseClassifierDetails, KBArticleFolder> detailsBuilder,
        IInsertEntityBLL<KBArticleFolder, KnowledgeBaseClassifierData> insertEntityBLL,
        IModifyEntityBLL<Guid, KBArticleFolder, KnowledgeBaseClassifierData, KnowledgeBaseClassifierDetails>
            modifyEntityBLL,
        IRemoveEntityBLL<Guid, KBArticleFolder> removeEntityBLL,
        IGetEntityBLL<Guid, KBArticleFolder, KnowledgeBaseClassifierDetails> detailsBLL,
        IGetEntityArrayBLL<Guid, KBArticleFolder, KnowledgeBaseClassifierDetails, KnowledgeBaseClassifierFilter>
            detailsArrayBLL,
        IReadonlyRepository<KBArticleReference> articlesReferences,
        ILocalizeText localize)
        : base(repository,
            logger,
            unitOfWork,
            currentUser,
            detailsBuilder,
            insertEntityBLL,
            modifyEntityBLL,
            removeEntityBLL,
            detailsBLL,
            detailsArrayBLL)
    {
        _removeEntityBLL = removeEntityBLL;
        _articlesReferences = articlesReferences;
        _localize = localize;
        _insertEntity = insertEntityBLL;
        _getEntityBLL = detailsBLL;
        _logger = logger;
    }


    public async Task<KnowledgeBaseClassifierDetails> AddAsync(KnowledgeBaseClassifierData data,
        CancellationToken cancellationToken = default)
    {
        var entity = await _insertEntity.CreateAsync(data, cancellationToken);

        await UnitOfWork.SaveAsync(cancellationToken);

        _logger.LogInformation(
            "User with ID = {UserID} inserted new KnowledgeBaseClassifier with ID = {KnowledgeBaseClassifierID}",
            CurrentUser.UserId, entity.ID);

        return await _getEntityBLL.DetailsAsync(entity.ID, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var articlesCount =
            await _articlesReferences.CountAsync(
                x => x.ObjectClassID == ObjectClass.KBArticleFolder && x.ObjectId == id, cancellationToken);

        if (articlesCount != 0)
        {
            throw new InvalidObjectException(
                await _localize.LocalizeAsync(nameof(Resources.CantDeleteKnowledgeBaseClassifier), cancellationToken));
        }

        await _removeEntityBLL.RemoveAsync(id, cancellationToken);
        await UnitOfWork.SaveAsync(cancellationToken);
    }

    #endregion
}