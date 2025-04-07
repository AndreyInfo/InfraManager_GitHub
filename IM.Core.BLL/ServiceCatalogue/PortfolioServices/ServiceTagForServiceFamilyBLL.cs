using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

internal class ServiceTagForServiceFamilyBLL : IServiceTagForServiceFamilyBLL, ISelfRegisteredService<IServiceTagForServiceFamilyBLL>
{
    private readonly IRepository<ServiceTag> _repository;
    public ServiceTagForServiceFamilyBLL(IRepository<ServiceTag> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceTag[]> GetByIDAndClassIdAsync(Guid id, ObjectClass classID, CancellationToken cancellationToken)
    {
        return await _repository.ToArrayAsync(c => c.ObjectId == id && c.ClassId == classID, cancellationToken);
    }

    public async Task SaveAsync(IEnumerable<ServiceTag> saveModels, Guid id, ObjectClass classID, CancellationToken cancellationToken = default)
    {
        saveModels.ForEach(c => c.ObjectId = id);
        if (saveModels is null)
            return;

        var existsModels = await _repository.ToArrayAsync(c => c.ObjectId == id && c.ClassId == classID, cancellationToken);

        var notChange = saveModels.Intersect(existsModels);

        var removingModels = existsModels.Except(notChange).ToArray();
        removingModels.ForEach(model => _repository.Delete(model));

        var addingModels = saveModels.Except(notChange).ToArray();
        addingModels.ForEach(model => _repository.Insert(model));
    }
}
