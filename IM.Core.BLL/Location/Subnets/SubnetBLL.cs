using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using InfraManager.BLL.CrudWeb;
using InfraManager.DAL;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Location.Subnets;

internal class SubnetBLL : ISubnetBLL, ISelfRegisteredService<ISubnetBLL>
{
    private readonly IMapper _mapper;
    private readonly IRepository<BuildingSubnet> _repositoryBuildingSubnets;
    private readonly IRepository<Building> _repositoryBuilding;
    private readonly IUnitOfWork _saveChangesCommand;

    public SubnetBLL(IMapper mapper,
                     IRepository<BuildingSubnet> repositoryBuildingSubnets,
                     IRepository<Building> repositoryBuilding,
                     IUnitOfWork saveChangesCommand)
    {
        _mapper = mapper;
        _repositoryBuilding = repositoryBuilding;
        _repositoryBuildingSubnets = repositoryBuildingSubnets;
        _saveChangesCommand = saveChangesCommand;
    }

    public async Task<int> AddAsync(SubnetDetails model, CancellationToken cancellationToken)
    {
        await ThrowIfBuildingNotFoundAsync(model.BuildingID, cancellationToken);
        await ThrowIfExistsSubnetIntoBuildingAsync(model.BuildingID, model.Subnet, cancellationToken);

        var saveModel = _mapper.Map<BuildingSubnet>(model);
        _repositoryBuildingSubnets.Insert(saveModel);
        await _saveChangesCommand.SaveAsync(cancellationToken);

        return saveModel.ID;
    }
    public async Task<int> UpdateAsync(SubnetDetails model, CancellationToken cancellationToken)
    {
        await ThrowIfBuildingNotFoundAsync(model.BuildingID, cancellationToken);
        await ThrowIfExistsSubnetIntoBuildingAsync(model.BuildingID, model.Subnet, cancellationToken);

        var saveModel = await _repositoryBuildingSubnets.FirstOrDefaultAsync(c => c.ID == model.ID, cancellationToken);
        ThrowIfNull(saveModel, model.ID);

        _mapper.Map(model, saveModel);
        await _saveChangesCommand.SaveAsync(cancellationToken);
        return saveModel.ID;
    }
    public async Task DeleteAsync(IEnumerable<DeleteModel<int>> deleteModels, CancellationToken cancellationToken)
    {
        var models = _mapper.Map<BuildingSubnet[]>(deleteModels);

        foreach (var item in models)
            _repositoryBuildingSubnets.Delete(item);

        await _saveChangesCommand.SaveAsync(cancellationToken);
    }

    public async Task<SubnetDetails[]> GetSubnetsByBuildingIDAsync(int buildingID, string search, CancellationToken cancellationToken)
    {
        await ThrowIfBuildingNotFoundAsync(buildingID, cancellationToken);
        BuildingSubnet[] entities;

        if (string.IsNullOrEmpty(search))
            entities = await _repositoryBuildingSubnets.ToArrayAsync(c => c.BuildingID == buildingID, cancellationToken);
        else
            entities = await _repositoryBuildingSubnets.ToArrayAsync(c => c.BuildingID == buildingID && c.Subnet.Contains(search), cancellationToken);

        return _mapper.Map<SubnetDetails[]>(entities);
    }

    private async Task ThrowIfExistsSubnetIntoBuildingAsync(int buildingID, string subnet, CancellationToken cancellationToken)
    {
        var isExistsWithSameName = await _repositoryBuildingSubnets.AnyAsync(c => c.BuildingID == buildingID && c.Subnet.Equals(subnet), cancellationToken);
        if (isExistsWithSameName)
            throw new InvalidObjectException($"Уже существует подсеть {subnet} в этом здании");
    }

    private async Task ThrowIfBuildingNotFoundAsync(int id, CancellationToken cancellationToken)
    {
        var isExists = await _repositoryBuilding.AnyAsync(c => c.ID == id, cancellationToken);
        if (!isExists)
            throw new ObjectNotFoundException<int>(id, ObjectClass.Building);
    }

    public async Task<SubnetDetails> GetByIDAsync(int id, CancellationToken cancellationToken)
    {
        var enitity = await _repositoryBuildingSubnets.FirstOrDefaultAsync(c => c.ID == id, cancellationToken);
        ThrowIfNull(enitity, id);
        return _mapper.Map<SubnetDetails>(enitity);
    }

    private void ThrowIfNull(BuildingSubnet model, int id)
    {
        if (model is null)
            throw new ObjectNotFoundException<int>(id, "Не была найдена подсеть");
    }
}
