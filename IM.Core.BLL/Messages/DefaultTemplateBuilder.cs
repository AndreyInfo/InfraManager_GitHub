using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.DAL;

namespace InfraManager.BLL.Messages;

internal class DefaultTemplateBuilder<TEntity, TTemplate> : IBuildEntityTemplate<TEntity, TTemplate>
    where TEntity : class, IGloballyIdentifiedEntity
{
    private readonly IMapper _mapper;
    private readonly IFindEntityByGlobalIdentifier<TEntity> _finder;

    public DefaultTemplateBuilder(IMapper mapper, IFindEntityByGlobalIdentifier<TEntity> finder)
    {
        _mapper = mapper;
        _finder = finder;
    }

    public async Task<TTemplate> BuildAsync(Guid id, CancellationToken cancellationToken = default, Guid? userID = null)
    {
        var entity = await _finder.FindAsync(id, cancellationToken)
            ?? throw new ObjectNotFoundException($"Объект уведомления {id} не найден");
        return _mapper.Map<TTemplate>(entity);
    }
}