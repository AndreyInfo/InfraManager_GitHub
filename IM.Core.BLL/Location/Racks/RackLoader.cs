using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.Location.Racks;

internal class RackLoader : ILoadEntity<int, Rack>, ISelfRegisteredService<ILoadEntity<int, Rack>>
{
    private readonly IReadonlyRepository<Rack> _repository;

    public RackLoader(IReadonlyRepository<Rack> repository) => _repository = repository;

    public Task<Rack> LoadAsync(int id, CancellationToken cancellationToken = default)
    {
        return _repository
            .With(rack => rack.Room)
            .ThenWith(rack => rack.Floor)
            .ThenWith(rack => rack.Building)
            .ThenWith(rack => rack.Organization)
            .SingleOrDefaultAsync(
                rack => rack.ID == id,
                cancellationToken);
    }
}