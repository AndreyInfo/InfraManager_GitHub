using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using InfraManager.DAL;
using InfraManager.DAL.Location;
using InfraManager.DAL.OrganizationStructure;
using AutoMapper;
using System.Linq;
using System.Text;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using Microsoft.Extensions.Logging;
using Inframanager;
using InfraManager.BLL.Localization;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Location.StorageLocations;

internal sealed class StorageLocationBLL : IStorageLocationBLL
    , ISelfRegisteredService<IStorageLocationBLL>
{
    private readonly ILocationBLL _locationBll;

    private readonly IReadonlyRepository<StorageLocationReference> _storageLocationReferencesRepository;
    private readonly IReadonlyRepository<Organization> _organizationsRepository;
    private readonly IReadonlyRepository<Building> _buildingsRepository;
    private readonly IReadonlyRepository<Floor> _floorsRepository;
    private readonly IReadonlyRepository<Room> _roomsRepository;
    private readonly IReadonlyRepository<Workplace> _workplacesRepository;
    private readonly IRepository<StorageLocation> _storageLocationRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly IValidatePermissions<StorageLocation> _validatePermissions;
    private readonly ILogger<StorageLocationBLL> _logger;
    private readonly ILocalizeText _localizeText;
    public StorageLocationBLL(ILocationBLL locationBll
                             , IReadonlyRepository<StorageLocationReference> storageLocationReferencesRepository
                             , IReadonlyRepository<Organization> organizationsRepository
                             , IReadonlyRepository<Building> buildingsRepository
                             , IReadonlyRepository<Floor> floorsRepository
                             , IReadonlyRepository<Room> roomsRepository
                             , IReadonlyRepository<Workplace> workplacesRepository
                             , IRepository<StorageLocation> storageLocationRepository
                             , IMapper mapper
                             , IUnitOfWork unitOfWork
                             , ICurrentUser currentUser
                             , IValidatePermissions<StorageLocation> validatePermissions
                             , ILogger<StorageLocationBLL> logger
                             , ILocalizeText localizeText)
    {
        _locationBll = locationBll;
        _storageLocationReferencesRepository = storageLocationReferencesRepository;
        _organizationsRepository = organizationsRepository;
        _buildingsRepository = buildingsRepository;
        _floorsRepository = floorsRepository;
        _roomsRepository = roomsRepository;
        _workplacesRepository = workplacesRepository;
        _storageLocationRepository = storageLocationRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _validatePermissions = validatePermissions;
        _logger = logger;
        _localizeText = localizeText;
    }
    #region Получение списка локаций
    /// TODO селать сортировку
    public async Task<LocationListItem[]> GetTableLocationAsync(StorageFilterLocation filter, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);
        await ThrowIfNotExistsByIDAsync(filter.StorageLocationID, cancellationToken);

        var references = await _storageLocationReferencesRepository.ToArrayAsync(c => c.StorageLocationID == filter.StorageLocationID, cancellationToken);
        var result = new List<LocationListItem>();
        foreach (var item in references)
            result.Add(new LocationListItem
            {
                Location = await ExecuteGetLocationAsync(item.ObjectClassID, item.ObjectID, cancellationToken),
                ClassID = item.ObjectClassID,
                ID = item.ObjectID
            });

        if (!string.IsNullOrEmpty(filter.SearchString))
            result = result.Where(c => c.Location.Contains(filter.SearchString)).ToList();

        return result.Skip(filter.StartRecordIndex).Take(filter.CountRecords).ToArray();
    }

    private async Task<string> ExecuteGetLocationAsync(ObjectClass classID, Guid id, CancellationToken cancellationToken) =>
        classID switch
        {
            ObjectClass.Workplace => await GetNameWorkplaceAsync(id, cancellationToken),
            ObjectClass.Room => await GetNameRoomAsync(id, cancellationToken),
            ObjectClass.Floor => await GetNameFloorAsync(id, cancellationToken),
            ObjectClass.Building => await GetNameBuidldingAsync(id, cancellationToken),
            ObjectClass.Organizaton => await GetNameOrganizationAsync(id, cancellationToken),
            _ => throw new Exception($"Not correct ClassID:{classID}")
        };

    private async Task<string> GetNameOrganizationAsync(Guid organizationID, CancellationToken cancellationToken)
    {
        var organization = await _organizationsRepository.FirstOrDefaultAsync(c => c.ID == organizationID, cancellationToken);

        return organization.Name;
    }

    private async Task<string> GetNameBuidldingAsync(Guid buildingID, CancellationToken cancellationToken)
    {
        var building = await _buildingsRepository.With(c => c.Organization).FirstOrDefaultAsync(c => c.IMObjID == buildingID, cancellationToken);

        var nameOfFloorAndParentsLocation = new List<string>
        {
            building.Organization.Name,
            building.Name
        };

        return BuildLocationString(nameOfFloorAndParentsLocation);
    }

    private async Task<string> GetNameFloorAsync(Guid buildingID, CancellationToken cancellationToken)
    {
        var floor = await _floorsRepository.With(c => c.Building).ThenWith(c => c.Organization).FirstOrDefaultAsync(c => c.IMObjID == buildingID, cancellationToken);

        var nameOfFloorAndParentsLocation = new List<string>
        {
            floor.Building.Organization.Name,
            floor.Building.Name,
            floor.Name
        };

        return BuildLocationString(nameOfFloorAndParentsLocation);
    }

    private async Task<string> GetNameRoomAsync(Guid roomID, CancellationToken cancellationToken)
    {
        var room = await _roomsRepository.With(c => c.Floor)
                                          .ThenWith(c => c.Building)
                                          .ThenWith(c => c.Organization)
                                          .FirstOrDefaultAsync(c => c.IMObjID == roomID, cancellationToken);

        var nameOfFloorAndParentsLocation = new List<string>
        {
            room.Floor.Building.Organization.Name,
            room.Floor.Building.Name,
            room.Floor.Name,
            room.Name
        };

        return BuildLocationString(nameOfFloorAndParentsLocation);
    }

    private async Task<string> GetNameWorkplaceAsync(Guid workplacesID, CancellationToken cancellationToken)
    {
        var room = await _workplacesRepository.With(c => c.Room)
                                          .ThenWith(c => c.Floor)
                                          .ThenWith(c => c.Building)
                                          .ThenWith(c => c.Organization)
                                          .FirstOrDefaultAsync(c => c.IMObjID == workplacesID, cancellationToken);

        var nameOfFloorAndParentsLocation = new List<string>
        {
            room.Room.Floor.Building.Organization.Name,
            room.Room.Floor.Building.Name,
            room.Room.Floor.Name,
            room.Room.Name,
            room.Name
        };

        return BuildLocationString(nameOfFloorAndParentsLocation);
    }

    private string BuildLocationString(List<string> names)
    {
        if (names is null || !names.Any())
            return string.Empty;

        var stringBuilder = new StringBuilder(names[0]);
        names.Remove(names[0]);
        foreach (var item in names)
        {
            stringBuilder.Append(" / ");
            stringBuilder.Append(item);
        }

        return stringBuilder.ToString();
    }
    #endregion

    #region Получение дерева
    public async Task<LocationTreeNodeDetails[]> GetTreeNodesByParentIDAsync(LocationTreeFilter filter, Guid storageID, CancellationToken cancellationToken)
    {
        await ThrowIfNotExistsByIDAsync(storageID, cancellationToken);
        var nodes = await _locationBll.GetLocationNodesByParentIdAndRightsUserAsync(filter, cancellationToken);
        return await InitializateLocationStorage(storageID, nodes, cancellationToken);
    }

    private async Task<LocationTreeNodeDetails[]> InitializateLocationStorage(Guid storageID, LocationTreeNodeDetails[] models, CancellationToken cancellationToken)
    {
        var result = new List<LocationTreeNodeDetails>(models);
        foreach (var node in models)
        {
            var isInitializateLocation = await _storageLocationReferencesRepository.AnyAsync(c => c.StorageLocationID == storageID
                                                                                            && c.ObjectClassID == node.ClassID
                                                                                            && c.ObjectID == node.UID, cancellationToken);
            // если полностью является местоположением справочника
            if (isInitializateLocation)
            {
                node.HasChild = false;
                node.IsSelectPart = false;
                node.IsSelectFull = true;
            }
            // если не явл расположением справочника, но есть элементы, ниже уровнем в дереве, являющиеся расположением скалада
            else if (await HasChildLocationStorage(node.ClassID, storageID, node.UID, cancellationToken))
            {
                node.HasChild = true;
                node.IsSelectPart = true;
                node.IsSelectFull = false;
            }
            // если не явялется сам и его дети тоже не избранные
            else
                result.Remove(node);

        }

        return result.ToArray();
    }

    private async Task<bool> HasChildLocationStorage(ObjectClass classID, Guid storageID, Guid locationID, CancellationToken cancellationToken) =>
        classID switch
        {
            ObjectClass.Owner => await OwnerHasChildLocationStorageAsync(storageID, cancellationToken),
            ObjectClass.Organizaton => await OrganizationHasChildLocationStorageAsync(locationID, storageID, cancellationToken),
            ObjectClass.Building => await BuildingHasChildLocationStorageAsync(locationID, storageID, cancellationToken),
            ObjectClass.Floor => await FloorHasChildLocationStorageAsync(locationID, storageID, cancellationToken),
            ObjectClass.Room => await RoomHasChildLocationStorageAsync(locationID, storageID, cancellationToken),
            ObjectClass.Workplace => false, // ибо последний уровень дерева
            _ => throw new Exception($"Not corrected classID : {classID}"),
        };


    private async Task<bool> OwnerHasChildLocationStorageAsync(Guid storageID, CancellationToken cancellationToken)
    {
        var organizationsID = (await _organizationsRepository.ToArrayAsync(cancellationToken)).Select(x => x.ID);

        foreach (var id in organizationsID)
        {
            var isStoralocation = await IsLocationOfStorageLocationAsync(id, storageID, ObjectClass.Organizaton, cancellationToken);
            var hasChild = await OrganizationHasChildLocationStorageAsync(id, storageID, cancellationToken);
            if (isStoralocation || hasChild)
                return true;
        }

        return false;
    }


    private async Task<bool> OrganizationHasChildLocationStorageAsync(Guid id, Guid storageID, CancellationToken cancellationToken)
    {
        var organization = await _organizationsRepository.WithMany(c => c.Buildings)
                                                 .FirstOrDefaultAsync(c => c.ID == id, cancellationToken);

        foreach (var building in organization.Buildings)
        {
            var isExistsIntoBuilding = await IsLocationOfStorageLocationAsync(building.IMObjID, storageID, ObjectClass.Building, cancellationToken);
            var isExistsBuildingChildren = await BuildingHasChildLocationStorageAsync(building.IMObjID, storageID, cancellationToken);
            if (isExistsIntoBuilding || isExistsBuildingChildren)
                return true;
        }

        return false;
    }


    private async Task<bool> BuildingHasChildLocationStorageAsync(Guid id, Guid storageID, CancellationToken cancellationToken)
    {
        var building = await _buildingsRepository.WithMany(c => c.Floors)
                                                 .FirstOrDefaultAsync(c => c.IMObjID == id, cancellationToken);

        foreach (var floor in building.Floors)
        {
            var isExistsIntoFloor = await IsLocationOfStorageLocationAsync(floor.IMObjID, storageID, ObjectClass.Floor, cancellationToken);
            var isExistsFloorChildren = await FloorHasChildLocationStorageAsync(floor.IMObjID, storageID, cancellationToken);
            if (isExistsIntoFloor || isExistsFloorChildren)
                return true;
        }

        return false;
    }

    private async Task<bool> FloorHasChildLocationStorageAsync(Guid id, Guid storageID, CancellationToken cancellationToken)
    {
        var floor = await _floorsRepository.WithMany(c => c.Rooms)
                                              .FirstOrDefaultAsync(c => c.IMObjID == id, cancellationToken);

        foreach (var room in floor.Rooms)
        {
            var isExistsIntoRoom = await IsLocationOfStorageLocationAsync(room.IMObjID, storageID, ObjectClass.Room, cancellationToken);
            var isExistsRoomChildren = await RoomHasChildLocationStorageAsync(room.IMObjID, storageID, cancellationToken);
            if (isExistsIntoRoom || isExistsRoomChildren)
                return true;
        }

        return false;
    }


    private async Task<bool> RoomHasChildLocationStorageAsync(Guid id, Guid storageID, CancellationToken cancellationToken)
    {
        var floor = await _roomsRepository.WithMany(c => c.Workplaces)
                                              .FirstOrDefaultAsync(c => c.IMObjID == id, cancellationToken);

        foreach (var workplace in floor.Workplaces)
        {
            var isExistsIntoWorkplace = await IsLocationOfStorageLocationAsync(workplace.IMObjID, storageID, ObjectClass.Workplace, cancellationToken);
            if (isExistsIntoWorkplace)
                return true;
        }

        return false;
    }

    private async Task<bool> IsLocationOfStorageLocationAsync(Guid locationID, Guid storageLocationID, ObjectClass classID, CancellationToken cancellationToken) =>
        await _storageLocationReferencesRepository.AnyAsync(c => c.ObjectClassID == classID
                                                            && c.StorageLocationID == storageLocationID
                                                            && c.ObjectID == locationID,
                                                            cancellationToken);
    #endregion

    public async Task<StorageLocationDetails> UpdateAsync(StorageLocationDetails model, Guid id, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update, cancellationToken);
        
        var storageLocation = await _storageLocationRepository.FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.StorageLocation);

        storageLocation = _mapper.Map(model, storageLocation);
        await _unitOfWork.SaveAsync(cancellationToken);

        return model;
    }

    public async Task<StorageLocationDetails> AddAsync(StorageLocationInsertDetails model, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert, cancellationToken);
        var entites = _mapper.Map<StorageLocation>(model);
        entites.StorageLocationReferences.ForEach(c => c.StorageLocationID = entites.ID);
        _storageLocationRepository.Insert(entites);
        await _unitOfWork.SaveAsync(cancellationToken);

        return _mapper.Map<StorageLocationDetails>(entites);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancellationToken);

        var storageLocation = await _storageLocationRepository.FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.StorageLocation);

        _storageLocationRepository.Delete(storageLocation);

        await _unitOfWork.SaveAsync(cancellationToken);
    }

    private async Task ThrowIfNotExistsByIDAsync(Guid id, CancellationToken cancellationToken)
    {
        var isExistsStorageLocation = await _storageLocationRepository.AnyAsync(c => c.ID == id, cancellationToken);
        if (!isExistsStorageLocation)
            throw new ObjectNotFoundException<Guid>(id, ObjectClass.StorageLocation);
    }
}
