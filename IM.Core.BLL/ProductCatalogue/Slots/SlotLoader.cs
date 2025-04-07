using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.Slots;

public class SlotLoader : ILoadEntity<SlotBaseKey, Slot, SlotData>,
    ISelfRegisteredService<ILoadEntity<SlotBaseKey, Slot, SlotData>>
{
    private readonly IRepository<Slot> _repository;

    public SlotLoader(IRepository<Slot> repository)
    {
        _repository = repository;
    }

    public Task<Slot> LoadAsync(SlotBaseKey key, CancellationToken cancellationToken = default)
    {
        return _repository.With(x => x.SlotType)
            .With(x => x.Adapter)
            .FirstOrDefaultAsync(x => x.ObjectID == key.ObjectID && x.Number == key.Number, cancellationToken);
    }
}