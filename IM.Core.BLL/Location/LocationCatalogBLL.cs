using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.Location;
using System;
using InfraManager.CrossPlatform.WebApi.Contracts.Auth;
using InfraManager.DAL.OrganizationStructure;
using System.Threading;
using InfraManager.BLL.AccessManagement;
using System.Linq.Expressions;
using InfraManager.DAL.Asset;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using Microsoft.Extensions.Logging;
using Inframanager;

namespace InfraManager.BLL.Location
{
    internal class LocationCatalogBLL : ILocationBLL, ISelfRegisteredService<ILocationBLL>
    {
        private readonly IMapper _mapper;
        private readonly IClassIconBLL _classIconBLL;
        private readonly IUserContextProvider _userContextProvider;

        private readonly IReadonlyRepository<Owner> _ownerQuery;
        private readonly IReadonlyRepository<Organization> _organizations;
        private readonly IReadonlyRepository<Building> _buildingQuery;
        private readonly IReadonlyRepository<Floor> _floorQuery;
        private readonly IReadonlyRepository<Room> _roomQuery;
        private readonly IReadonlyRepository<Workplace> _workplaceQuery;
        private readonly IReadonlyRepository<Rack> _rackQuery;
        private readonly IObjectAccessBLL _objectAccessBLL;

        private readonly ILogger<LocationCatalogBLL> _logger;
        private readonly IValidatePermissions<Organization> _validatePermissionsOrganizations;
        private readonly IValidatePermissions<Building> _validatePermissionsBuildings;
        private readonly IValidatePermissions<Floor> _validatePermissionsFloors;
        private readonly IValidatePermissions<Room> _validatePermissionsRooms;
        private readonly IValidatePermissions<Workplace> _validatePermissionsWorkplaces;
        private readonly IValidatePermissions<Rack> _validatePermissionsRacks;
        private readonly IServiceMapper<ObjectClass, ILocationNodesQuery> _serviceMapper;
        private Guid _currentUserId => _userContextProvider.GetUserContext().UserID;

        /// <summary>
        /// все доступные классы для справочника местопложений
        /// </summary>
        private readonly ObjectClass[] availableObjectClasses;

        public LocationCatalogBLL(
            IMapper mapper,
            IReadonlyRepository<Owner> ownerQuery, 
            IClassIconBLL classIconBLL, 
            IUserContextProvider userContextProvider,
            IReadonlyRepository<Organization> organizations,
            IReadonlyRepository<Building> buildingQuery, 
            IReadonlyRepository<Floor> floorQuery, 
            IReadonlyRepository<Room> roomQuery, 
            IReadonlyRepository<Workplace> workplaceQuery,
            IReadonlyRepository<Rack> rackQuery,
            IObjectAccessBLL objectAccessBLL,
            ILogger<LocationCatalogBLL> logger,
            IValidatePermissions<Organization> validatePermissionsOrganizations,
            IValidatePermissions<Building> validatePermissionsBuildings,
            IValidatePermissions<Floor> validatePermissionsFloors,
            IValidatePermissions<Room> validatePermissionsRooms,
            IValidatePermissions<Workplace> validatePermissionsWorkplaces,
            IValidatePermissions<Rack> validatePermissionsRacks,
            IServiceMapper<ObjectClass, ILocationNodesQuery> serviceMapper)
        {
            _mapper = mapper;
            _ownerQuery = ownerQuery;
            _classIconBLL = classIconBLL;
            _userContextProvider = userContextProvider;

            _organizations = organizations;
            _buildingQuery = buildingQuery;
            _floorQuery = floorQuery;
            _roomQuery = roomQuery;
            _workplaceQuery = workplaceQuery;
            _rackQuery = rackQuery;

            availableObjectClasses = new ObjectClass[]
            {
                ObjectClass.Owner,
                ObjectClass.Organizaton,
                ObjectClass.Building,
                ObjectClass.Floor,
                ObjectClass.Room,
                ObjectClass.Rack,
                ObjectClass.Workplace,
            };
            _objectAccessBLL = objectAccessBLL;
            _logger = logger;
            _validatePermissionsOrganizations = validatePermissionsOrganizations;
            _validatePermissionsBuildings = validatePermissionsBuildings;
            _validatePermissionsFloors = validatePermissionsFloors;
            _validatePermissionsRooms = validatePermissionsRooms;
            _validatePermissionsWorkplaces = validatePermissionsWorkplaces;
            _validatePermissionsRacks = validatePermissionsRacks;
            _serviceMapper = serviceMapper;
        }

        #region Получение дерева
        public async Task<LocationTreeNodeDetails[]> GetLocationNodesByParentIdAndRightsUserAsync(LocationTreeFilter model, CancellationToken cancellationToken = default)
        {
            ValidateClassID(model.ClassID);

            var result = await ExecuteGetTreeAsync(model, cancellationToken);

            return result;
        }
        //TODO использовать методы получения ниже, где реализована проверка доступа
        private async Task<LocationTreeNodeDetails[]> ExecuteGetTreeAsync(LocationTreeFilter filter, CancellationToken cancellationToken)
        {
            var result = new List<LocationTreeNodeDetails>();

            switch (filter.ClassID)
            {
                case ObjectClass.Owner:
                    result.Add(await GetOwnerAsync(cancellationToken));
                    break;

                case ObjectClass.Organizaton:
                    var organizations = await GetOrganizationNodesAsync(cancellationToken);
                    result.AddRange(_mapper.Map<List<LocationTreeNodeDetails>>(organizations));
                    break;

                case ObjectClass.Building:
                    if (!await _organizations.AnyAsync(c=> c.ID == filter.OrganizationID, cancellationToken))
                        throw new ObjectNotFoundException<Guid>(filter.OrganizationID.Value, ObjectClass.Organizaton);

                    var building = await _buildingQuery.ToArrayAsync(c => c.OrganizationID == filter.OrganizationID, cancellationToken);
                    result.AddRange(_mapper.Map<List<LocationTreeNodeDetails>>(building));
                    break;

                case ObjectClass.Floor:
                    await ThrowIfNotExistsParent(_buildingQuery, c => c.ID == filter.ParentID,
                                                 ObjectClass.Building, filter.ParentID, cancellationToken);

                    var floors = await _floorQuery.ToArrayAsync(c => c.BuildingID == filter.ParentID, cancellationToken);
                    result.AddRange(_mapper.Map<List<LocationTreeNodeDetails>>(floors));
                    break;

                case ObjectClass.Room:
                    await ThrowIfNotExistsParent(_floorQuery, c => c.ID == filter.ParentID,
                                                 ObjectClass.Floor, filter.ParentID, cancellationToken);

                    var rooms = await _roomQuery.ToArrayAsync(c => c.FloorID == filter.ParentID, cancellationToken);
                    result.AddRange(_mapper.Map<List<LocationTreeNodeDetails>>(rooms));
                    break;

                case ObjectClass.Rack:
                    await ThrowIfNotExistsParent(_roomQuery, c => c.ID == filter.ParentID,
                                                 ObjectClass.Room, filter.ParentID, cancellationToken);

                    var racks = await _rackQuery.ToArrayAsync(c => c.RoomID == filter.ParentID, cancellationToken);
                    result.AddRange(_mapper.Map<List<LocationTreeNodeDetails>>(racks));
                    break;
                case ObjectClass.Workplace:
                    await ThrowIfNotExistsParent(_roomQuery, c => c.ID == filter.ParentID, 
                                                 ObjectClass.Room, filter.ParentID, cancellationToken);

                    var workplaces = await _workplaceQuery.ToArrayAsync(c => c.RoomID == filter.ParentID, cancellationToken);
                    result = _mapper.Map<List<LocationTreeNodeDetails>>(workplaces);
                    break;
            }

            // иконки статичные, поэтому через ClassId
            result.ForEach(c => c.IconName = _classIconBLL.GetIconNameByClassID(c.ClassID));

            return result.ToArray();
        }

        private async Task ThrowIfNotExistsParent<T, TID>(IReadonlyRepository<T> repository, Expression<Func<T,bool>> expression
                                                          , ObjectClass classID, TID id, CancellationToken cancellationToken) 
            where T : class
            where TID : struct
        {
            if (!await repository.AnyAsync(expression, cancellationToken))
                throw new ObjectNotFoundException<TID>(id, classID);
        }
        private async Task<LocationTreeNodeDetails> GetOwnerAsync(CancellationToken cancellationToken = default)
        {
            var owner = await _ownerQuery.FirstOrDefaultAsync(cancellationToken);
            var dto = _mapper.Map<LocationTreeNodeDetails>(owner);
            dto.HasChild = await _organizations.AnyAsync(cancellationToken);
            return dto;
        }
        #endregion

        private void ValidateClassID(ObjectClass classID)
        {
            if (!availableObjectClasses.Contains(classID))  
                throw new Exception($"Объект c ClassID {classID} не используется в дереве местоположений");
        }

        public async Task<LocationTreeNodeDetails[]> GetBranchTreeAsync(ObjectClass classID, Guid id, CancellationToken cancellationToken = default)
        {
            ValidateClassID(classID);
            // флаг чтобы недобавить лишний элемент, и если уже один добавлен
            // то все элементы уровнем выше добавлются автоматом
            bool alreadyRead = false;
            var result = new List<LocationTreeNodeDetails>();


            /*Код хранит информацию о биекции*/
            if (classID == ObjectClass.Workplace)
            {
                var workplace = await GetLocationByIMObjIDAsync(id, _workplaceQuery, cancellationToken)
                    ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Workplace);

                result.Add(_mapper.Map<LocationTreeNodeDetails>(workplace));
                alreadyRead = true;
                id = workplace.Room.IMObjID;
            }

            if (classID == ObjectClass.Room || alreadyRead)
            {
                var room = await GetLocationByIMObjIDAsync(id, _roomQuery, cancellationToken)
                    ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Room);

                result.Add(_mapper.Map<LocationTreeNodeDetails>(room));
                alreadyRead = true;
                id = room.Floor.IMObjID;
            }

            if (classID == ObjectClass.Floor || alreadyRead)
            {
                var floor = await GetLocationByIMObjIDAsync(id, _floorQuery, cancellationToken)
                    ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Floor);

                result.Add(_mapper.Map<LocationTreeNodeDetails>(floor));
                alreadyRead = true;
                id = floor.Building.IMObjID;
            }

            if (classID == ObjectClass.Building || alreadyRead)
            {
                var building = await GetLocationByIMObjIDAsync(id, _buildingQuery, cancellationToken)
                    ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Building);

                result.Add(_mapper.Map<LocationTreeNodeDetails>(building));
                alreadyRead = true;
                id = building.OrganizationID.Value;
            }

            if (classID == ObjectClass.Organizaton || alreadyRead)
            {
                var organization = await _organizations.FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
                    ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Organizaton);

                result.Add(_mapper.Map<LocationTreeNodeDetails>(organization));
            }

            result.Add(_mapper.Map<LocationTreeNodeDetails>(await _ownerQuery.FirstOrDefaultAsync(cancellationToken)));
            result.Reverse();
            return result.ToArray();
        }

        private async Task<T> GetLocationByIMObjIDAsync<T>(Guid id, IReadonlyRepository<T> repository, CancellationToken cancellationToken) where T : class, ILocationObject
        {
            if (!await repository.AnyAsync(c => c.IMObjID == id, cancellationToken))
                return null;

            var location = await repository.FirstOrDefaultAsync(c => c.IMObjID == id, cancellationToken);
            return location;
        }


        //TODO сделать корневой элемента на фронте
        public async Task<LocationTreeNodeDetails[]> GetOwnerNodesAsync(CancellationToken cancellationToken)
        {
            var owner = await _ownerQuery.FirstOrDefaultAsync(cancellationToken)
                ?? throw new ObjectNotFoundException($"Не найден {ObjectClass.Owner}");
            
            var dto = _mapper.Map<LocationTreeNodeDetails>(owner);
            dto.HasChild = await _organizations.AnyAsync(cancellationToken);
            return new LocationTreeNodeDetails[] { dto };
        }

        public async Task<LocationTreeNodeDetails[]> GetOrganizationNodesAsync(CancellationToken cancellationToken)
        {
            await _validatePermissionsOrganizations.ValidateOrRaiseErrorAsync(_logger, _currentUserId, ObjectAction.ViewDetailsArray, cancellationToken);
            var organizations = await _organizations.ToArrayAsync(cancellationToken);
            return _mapper.Map<LocationTreeNodeDetails[]>(organizations);
        }

        public async Task<LocationTreeNodeDetails[]> GetBuildingNodesAsync(Guid organizationID, CancellationToken cancellationToken)
        {
            await _validatePermissionsBuildings.ValidateOrRaiseErrorAsync(_logger, _currentUserId, ObjectAction.ViewDetailsArray, cancellationToken);

            if (!await _organizations.AnyAsync(c => c.ID == organizationID, cancellationToken)) 
                throw new ObjectNotFoundException<Guid>(organizationID, ObjectClass.Organizaton);

            var buildings = await _buildingQuery.ToArrayAsync(c => c.OrganizationID == organizationID, cancellationToken);
            return _mapper.Map<LocationTreeNodeDetails[]>(buildings);
        }


        
        public async Task<LocationTreeNodeDetails[]> GetFloorNodesAsync(int buildingID, CancellationToken cancellationToken)
        {
            await _validatePermissionsFloors.ValidateOrRaiseErrorAsync(_logger, _currentUserId, ObjectAction.ViewDetailsArray, cancellationToken);

            _logger.LogTrace($"Getting nodes type \"{ObjectClass.Floor}\" from Location tree");

            var nodes = await _serviceMapper.Map(ObjectClass.Floor).GetNodesAsync(buildingID, cancellationToken: cancellationToken);
            return _mapper.Map<LocationTreeNodeDetails[]>(nodes);
        }

        public async Task<LocationTreeNodeDetails[]> GetRoomNodesAsync(int floorID, ObjectClass? childClassID, CancellationToken cancellationToken)
        {
            await _validatePermissionsFloors.ValidateOrRaiseErrorAsync(_logger, _currentUserId, ObjectAction.ViewDetailsArray, cancellationToken);

            _logger.LogTrace($"Getting nodes type \"{ObjectClass.Room}\" from Location tree");

            var nodes = await _serviceMapper.Map(ObjectClass.Room).GetNodesAsync(floorID, cancellationToken: cancellationToken);
            return _mapper.Map<LocationTreeNodeDetails[]>(nodes);
        }

        public async Task<LocationTreeNodeDetails[]> GetWorkplaceNodesAsync(int roomID, CancellationToken cancellationToken)
        {
            await _validatePermissionsWorkplaces.ValidateOrRaiseErrorAsync(_logger, _currentUserId, ObjectAction.ViewDetailsArray, cancellationToken);

            _logger.LogTrace($"Getting nodes type \"{ObjectClass.Workplace}\" from Location tree");

            var nodes = await _serviceMapper.Map(ObjectClass.Workplace).GetNodesAsync(roomID, cancellationToken: cancellationToken);
            return _mapper.Map<LocationTreeNodeDetails[]>(nodes);
        }

        public async Task<LocationTreeNodeDetails[]> GetRackNodesAsync(int roomID, CancellationToken cancellationToken)
        {
            await _validatePermissionsRacks.ValidateOrRaiseErrorAsync(_logger, _currentUserId, ObjectAction.ViewDetailsArray, cancellationToken);

            _logger.LogTrace($"Getting nodes type \"{ObjectClass.Rack}\" from Location tree");

            var nodes = await _serviceMapper.Map(ObjectClass.Rack).GetNodesAsync(roomID, cancellationToken: cancellationToken);
            return _mapper.Map<LocationTreeNodeDetails[]>(nodes);
        }
    }
}
