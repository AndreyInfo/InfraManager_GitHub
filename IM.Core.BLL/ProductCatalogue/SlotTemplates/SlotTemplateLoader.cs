using Inframanager.BLL;
using InfraManager.BLL.ProductCatalogue.Slots;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.SlotTemplates;
internal sealed class SlotTemplateLoader : ILoadEntity<SlotBaseKey, SlotTemplate, SlotTemplateDetails>
    , ISelfRegisteredService<ILoadEntity<SlotBaseKey, SlotTemplate, SlotTemplateDetails>>
{
    private readonly IReadonlyRepository<SlotTemplate> _repository;

    public SlotTemplateLoader(IReadonlyRepository<SlotTemplate> repository)
    {
        _repository = repository;
    }

    public Task<SlotTemplate> LoadAsync(SlotBaseKey key, CancellationToken cancellationToken)
    {
        return _repository.With(x => x.SlotType)
            .FirstOrDefaultAsync(x => x.ObjectID == key.ObjectID && x.Number == key.Number, cancellationToken);
    }
}
