using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Inframanager.BLL;
using InfraManager.DAL.ProductCatalogue.LifeCycles;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles;

public class LifeCycleModifier:  IModifyObject<LifeCycle, LifeCycleData>,
    ISelfRegisteredService<IModifyObject<LifeCycle, LifeCycleData>>
{
    private readonly IMapper _mapper;
    
    public LifeCycleModifier(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Task ModifyAsync(LifeCycle entity, LifeCycleData data, CancellationToken cancellationToken = default)
    {
        if (entity.IsFixed)
        {
            throw new ObjectReadonlyException(new InframanagerObject(entity.ID, ObjectClass.LifeCycle));
        }
        
        _mapper.Map(data, entity);
        
        return Task.CompletedTask;
    }

    public void SetModifiedDate(LifeCycle entity)
    {
    }
}