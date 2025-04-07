using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.KnowledgeBase.KnowledgeBaseClassifiers;
using InfraManager.BLL.Localization;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ServiceDesk.KnowledgeBase.KnowledgeBaseClassifiers;

public class KnowledgeBaseClassifierBuilder :
    IBuildObject<KBArticleFolder, KnowledgeBaseClassifierData>,
    IModifyObject<KBArticleFolder, KnowledgeBaseClassifierData>,
    ISelfRegisteredService<IBuildObject<KBArticleFolder, KnowledgeBaseClassifierData>>,
    ISelfRegisteredService<IModifyObject<KBArticleFolder, KnowledgeBaseClassifierData>>
{
    private readonly IRepository<User> _userFinder;
    private readonly IMapper _mapper;
    private readonly ILocalizeText _localize;

    public KnowledgeBaseClassifierBuilder(IMapper mapper,
        IRepository<User> userFinder,
        ILocalizeText localize)
    {
        _mapper = mapper;
        _userFinder = userFinder;
        _localize = localize;
    }

    public Task<KBArticleFolder> BuildAsync(KnowledgeBaseClassifierData data,
        CancellationToken cancellationToken = default)
    {
        var newKnowledgeBaseClassifier = _mapper.Map<KBArticleFolder>(data);

        return BuildEntityAsync(newKnowledgeBaseClassifier, data, cancellationToken);
    }

    private async Task<KBArticleFolder> BuildEntityAsync(KBArticleFolder entity, KnowledgeBaseClassifierData data,
        CancellationToken cancellationToken = default)
    {
        if (entity.UpdatePeriod < 0)
        {
            entity.UpdatePeriod = 0;
        }

        var expertUser = await _userFinder.With(x => x.UserRoles)
            .FirstOrDefaultAsync(x => x.IMObjID == data.ExpertID, cancellationToken);

        if (expertUser == null || !User.HasOperation(OperationID.BeKnowledgeBaseExpert).IsSatisfiedBy(expertUser))
        {
            throw new InvalidObjectException(
                await _localize.LocalizeAsync(nameof(Resources.UserCantBeKnowledgeBaseExpert), cancellationToken));
        }

        entity.Expert = expertUser;
        
        return entity;
    }

    public Task ModifyAsync(KBArticleFolder entity, KnowledgeBaseClassifierData data,
        CancellationToken cancellationToken = default)
    {
        _mapper.Map(data, entity);

        return BuildEntityAsync(entity, data, cancellationToken);
    }

    public void SetModifiedDate(KBArticleFolder entity)
    {
    }


    public Task<IEnumerable<KBArticleFolder>> BuildManyAsync(IEnumerable<KnowledgeBaseClassifierData> dataItems,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}