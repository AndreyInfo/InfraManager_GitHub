using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using AutoMapper;
using IM.Core.Import.BLL.Import.Array;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.View;
using IM.Core.Import.BLL.Interface.OrganizationStructure.Organizations;
using IM.Core.Import.BLL.Interface.OrganizationStructure.Subdivisions;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Import;
using InfraManager.DAL.Location;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.Extensions.Logging;
using IM.Core.Import.BLL.Import.Helpers;
using IM.Core.Import.BLL.Interface.Exceptions;
using IM.Core.Import.BLL.Interface.Import.Log;
using IM.Core.Import.BLL.Interface.Import.Models.Import;
using IM.Core.Import.BLL.OrganizationStructure;
using IM.Core.Import.BLL.UserImport.UserImport.CommonSaveBLL;
using Microsoft.EntityFrameworkCore;

namespace IM.Core.Import.BLL.Import
{
    internal class ImportAnalyzerBLL : IImportAnalyzerBLL, ISelfRegisteredService<IImportAnalyzerBLL>
    {
        private readonly IMapper _mapper;
        private readonly IEntityImportConnector<OrganizationDetails, Organization> _organizationsImportConnector;
        private readonly IImportConnectorBase<OrganizationDetails, Organization> _organizationImportConnectorBase;
        private readonly IEntityImportConnector<ISubdivisionDetails, Subdivision> _subdivisionImportConnector;
        private readonly IImportConnectorBase<ISubdivisionDetails, Subdivision> _subdivisionImportConnectorBase;
        private readonly IEntityImportConnector<IUserDetails, User> _userImportConnector;
        private readonly IImportConnectorBase<IUserDetails, User> _userImportConnectorBase;
        private readonly IOrganizationsBLL _organizationsBLL;
        private readonly ISubdivisionsBLL _subdivisionsBLL;
        private readonly IWorkplaceBLL _workplaceBLL;
        private readonly IRoomBLL _roomBLL;
        private readonly IPositionBLL _positionBLL;
        private readonly ILocalLogger<ImportAnalyzerBLL> _logger;
        private readonly IBaseImportMapper<IUserDetails, User> _importUserMapper;
        private readonly IBaseImportMapper<OrganizationDetails, Organization> _importOrganizationMapper;
        private readonly IBaseImportMapper<ISubdivisionDetails, Subdivision> _importSubdivisionMapper;
        private readonly IPolledObjectConverter _polledObjectConverter;
        private readonly IAdditionalParametersForSelect? _additional;
        private readonly IRepository<Organization> _organization;
        private readonly IRepository<Subdivision> _subdivisions;
        private readonly IRepository<User> _users;
        private readonly ISubdivisionParentIDQuery _subdivisionParentIDQuery;


        private readonly IReadonlyRepository<Workplace> _workplaceReadonlyRepository;

        public ImportAnalyzerBLL(IMapper mapper,
            IOrganizationsBLL organizationsBLL,
            ISubdivisionsBLL subdivisionsBLL,
            IWorkplaceBLL workplaceBLL,
            IRoomBLL roomBLL,
            IPositionBLL positionBLL,
            IReadonlyRepository<Workplace> workplaceReadonlyRepository,
            IBaseImportMapper<OrganizationDetails, Organization> importOrganizationMapper,
            IBaseImportMapper<IUserDetails, User> importUserMapper,
            IBaseImportMapper<ISubdivisionDetails, Subdivision> importSubdivisionMapper,
            IPolledObjectConverter polledObjectConverter,
            IAdditionalParametersForSelect? additional,
            IRepository<Subdivision> subdivisions,
            IRepository<Organization> organization, IRepository<User> users,
            ISubdivisionParentIDQuery subdivisionParentIDQuery,
            IEntityImportConnector<OrganizationDetails, Organization> organizationsImportConnector,
            IEntityImportConnector<ISubdivisionDetails, Subdivision> subdivisionImportConnector,
            IEntityImportConnector<IUserDetails, User> userImportConnector,
            IImportConnectorBase<ISubdivisionDetails, Subdivision> subdivisionImportConnectorBase,
            IImportConnectorBase<OrganizationDetails, Organization> organizationImportConnectorBase, 
            IImportConnectorBase<IUserDetails, User> userImportConnectorBase, 
            ILocalLogger<ImportAnalyzerBLL> logger)
        {
            _mapper = mapper;
            _organizationsBLL = organizationsBLL;
            _subdivisionsBLL = subdivisionsBLL;
            _workplaceBLL = workplaceBLL;
            _roomBLL = roomBLL;
            _positionBLL = positionBLL;
            _workplaceReadonlyRepository = workplaceReadonlyRepository;
            _importOrganizationMapper = importOrganizationMapper;
            _importUserMapper = importUserMapper;
            _importSubdivisionMapper = importSubdivisionMapper;
            _polledObjectConverter = polledObjectConverter;
            _additional = additional;
            _subdivisions = subdivisions;
            _organization = organization;
            _users = users;
            _subdivisionParentIDQuery = subdivisionParentIDQuery;
            _organizationsImportConnector = organizationsImportConnector;
            _subdivisionImportConnector = subdivisionImportConnector;
            _userImportConnector = userImportConnector;
            _subdivisionImportConnectorBase = subdivisionImportConnectorBase;
            _organizationImportConnectorBase = organizationImportConnectorBase;
            _userImportConnectorBase = userImportConnectorBase;
            _logger = logger;
        }

        public async Task SaveAsync(List<ImportModel?> importModels, UISetting uiSettings,
            CancellationToken cancellationToken)
        {
            _logger.Information("Получение информации об установленных флагах");
            var polledObject = _polledObjectConverter.GetPolledObjects(uiSettings.ObjectType);
            _logger.Information("Получена информация об установленных флагах");


            var polledObjectOrganization = polledObject.Organization;
            var polledObjectSubdivision = polledObject.Subdivision;
            var polledObjectUser = polledObject.User;
           
            Func<ImportModel, bool> isOrganization = x => x.ImportType is null or ImportTypeEnum.Organization;
            Func<ImportModel, bool> isSubdivision = x => x.ImportType is null or ImportTypeEnum.Subdivision;
            Func<ImportModel, bool> isUser = x => x.ImportType is null or ImportTypeEnum.User;

            var organizations = importModels.Where(isOrganization).ToList();
            var organizationImportData =
                await _importOrganizationMapper.Init(organizations, uiSettings, cancellationToken);

            var subdivisions = importModels.Where(isSubdivision).ToList();
            var subdivisionsImportData =
                await _importSubdivisionMapper.Init(subdivisions, uiSettings, cancellationToken);
            var users = importModels.Where(isUser).ToList();
            var importData =
                await _importUserMapper.Init(users, uiSettings, cancellationToken);
            var organizationsCount = 0;
            var subdivisionsCount = 0;
            var usersCount = 0;
            
            _logger.Information(
                $"Organisation {(polledObjectOrganization ? "+" : "-")}, Subdivision {(polledObjectSubdivision ? "+" : "-")}, User {(polledObjectUser ? "+" : "-")}");
            List<OrganizationDetails> organizationDetailsList = null;
            ErrorStatistics<OrganizationDetails>? errorOrganizations = null;
            if (polledObjectOrganization)
            {
                organizationDetailsList = GetOrganizationDetailsList(organizationImportData, out errorOrganizations);
                organizationsCount += organizationDetailsList.Count;
            }

            List<ISubdivisionDetails>? subdivisionDetailsList = null;
            ErrorStatistics<ISubdivisionDetails> errorSubdivisions = null;
            if (polledObjectSubdivision)
            {
                errorSubdivisions = GetErrorSubdivisions(subdivisionsImportData, out subdivisionDetailsList);
                subdivisionsCount += subdivisionDetailsList.Count;
            }

            List<IUserDetails>? userDetailsList = null;
            ErrorStatistics<IUserDetails> errorStatistics;
            if (polledObjectUser)
            {
                userDetailsList = GetUserDetailsList(importData, out errorStatistics);
                usersCount += userDetailsList.Count;
            }
            _logger.Information($"Найдено {organizationsCount} организаций, {subdivisionsCount} подразделений, {usersCount} пользователей");

            if (polledObjectOrganization)
            {
                _logger.Information("Добавление Организациий:");
                 Func<IGrouping<object, OrganizationDetails>, bool> predicate = groupping =>
                {
                    if (groupping.Count() <= 1) return false;
                    var name = LogHelper.ToOutputFormat(groupping.First().Name);
                    _logger.Information($"ERR Ошибка при импорте Организации с названием {name}");
                    return true;
                };
                await _organizationImportConnectorBase.CheckForDuplicatesAsync(organizationDetailsList, errorOrganizations, organizationImportData,
                    _additional, predicate, cancellationToken);
                
                await SaveOrganizationsAsync(organizationImportData, organizationDetailsList, errorOrganizations, cancellationToken);
            }

            if (polledObjectSubdivision)
            {
                _logger.Information("Добавление Подразделений:");

                Func<IGrouping<object, ISubdivisionDetails>, bool> predicate = groupping =>
                {
                    if (groupping.Count() <= 1) return false;
                    var name = LogHelper.ToOutputFormat(groupping.First().Name);
                    _logger.Information($"ERR Ошибка при импорте Подразделения с названием {name}");
                    return true;
                };

                await _subdivisionImportConnectorBase.CheckForDuplicatesAsync(subdivisionDetailsList, errorSubdivisions, subdivisionsImportData,
                    _additional, predicate, cancellationToken);
                
                await SaveSubdivisionsAsync(subdivisionsImportData, subdivisionDetailsList, errorSubdivisions, cancellationToken);
            }

            if (polledObjectUser)
            {
                _logger.Information("Добавление Пользователей:");
               
                errorStatistics = new ErrorStatistics<IUserDetails>();
                
                await _userImportConnectorBase.CheckForDuplicatesAsync(userDetailsList, errorStatistics, importData,
                _additional, null, cancellationToken);
                
                await SaveUsersAsync(importData, subdivisionsImportData, organizationImportData, _additional,
                    userDetailsList, errorStatistics, cancellationToken);
            }
        }

        private List<IUserDetails>? GetUserDetailsList(ImportData<IUserDetails, User> importData, out ErrorStatistics<IUserDetails> errorStatistics)
        {
            var importDataImportModels = importData.ImportModels as List<ImportModel> ??
                                         importData.ImportModels.ToList();
            var userDetailsList = (List<IUserDetails>?) _mapper
                .Map<List<UserDetails>>(importDataImportModels).Cast<IUserDetails>().ToList();
            errorStatistics = new ErrorStatistics<IUserDetails>();
            _userImportConnectorBase.CheckUpdateModelsForError(userDetailsList,errorStatistics,importData,(x,y)=>y.PreValidate(x),"Не достаточно данных");
            errorStatistics.ErrorDetails.Clear();
            return userDetailsList;
        }

        private ErrorStatistics<ISubdivisionDetails> GetErrorSubdivisions(ImportData<ISubdivisionDetails, Subdivision> subdivisionsImportData, out List<ISubdivisionDetails>? subdivisionDetailsList)
        {
            var models = subdivisionsImportData.ImportModels as List<ImportModel> ??
                         subdivisionsImportData.ImportModels.ToList();
            var errorSubdivisions = new ErrorStatistics<ISubdivisionDetails>();
            
            //todo:отрефакторить
            subdivisionDetailsList = _mapper.Map<List<SubdivisionDetails>>(models).Cast<ISubdivisionDetails>().ToList();
            _subdivisionImportConnectorBase.CheckUpdateModelsForError(
                    subdivisionDetailsList, errorSubdivisions,
                subdivisionsImportData,
                (details, data) => data.PreValidate(details), "Недостаточно данных");
            errorSubdivisions.ErrorDetails.Clear();
            return errorSubdivisions;
        }

        private List<OrganizationDetails> GetOrganizationDetailsList(ImportData<OrganizationDetails, Organization> organizationImportData, out ErrorStatistics<OrganizationDetails> errorOrganizations)
        {
            var organizationDetailsList =
                _mapper.Map<List<OrganizationDetails>>(organizationImportData.ImportModels);


            errorOrganizations = new ErrorStatistics<OrganizationDetails>();

            _organizationImportConnectorBase.CheckUpdateModelsForError(organizationDetailsList, errorOrganizations,
                organizationImportData, (details, data) => data.PreValidate(details), "Недостаточно данных");
            errorOrganizations.ErrorDetails.Clear();
            return organizationDetailsList;
        }

        public async Task ModifyAdditionalTab(AdditionalTabDetails additionalTab, UISetting settings,
            CancellationToken cancellationToken)
        {
            additionalTab.SubdivisionDefaultOrganizationItemID = await GetSubdivisionDefaultOrganizationItemID(
                additionalTab.SubdivisionDefaultOrganizationItemClassID,
                additionalTab.SubdivisionDefaultOrganizationItemID, cancellationToken);
            var locationTypeEnum = (LocationTypeEnum) settings.LocationMode;
            additionalTab.LocationItemNumberID =
                await CheckSetRoomOrWorkplaceID(locationTypeEnum, settings.LocationItemID, cancellationToken);
        }

        private async Task<int?> CheckSetRoomOrWorkplaceID(LocationTypeEnum typeOfLocationMode,
            Guid? locationModeItemID, CancellationToken cancellationToken)
        {
            try
            {
                switch (typeOfLocationMode)
                {
                    case LocationTypeEnum.Room:
                    {
                        var room = await _roomBLL.GetAsync(locationModeItemID, cancellationToken);
                        return room?.ID;
                    }
                    case LocationTypeEnum.Workplace:
                    {
                        var workplace = await _workplaceBLL.GetAsync(locationModeItemID, cancellationToken);
                        return workplace?.ID;
                    }
                    case LocationTypeEnum.None:
                    case LocationTypeEnum.User:
                    default:
                        return null;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error when get roomId or workplaceID ");
                throw;
            }
        }

        private async Task<Guid?> GetSubdivisionDefaultOrganizationItemID(
            int? subdivisionDefaultOrganizationItemClassID, Guid? subdivisionDefaultOrganizationItemID,
            CancellationToken cancellationToken)
        {
            if (subdivisionDefaultOrganizationItemClassID != null && subdivisionDefaultOrganizationItemID != null)
            {
                if (subdivisionDefaultOrganizationItemClassID == (int?) ObjectClass.Organizaton)
                {
                    return await GetOrganizationInternalID(subdivisionDefaultOrganizationItemID, cancellationToken);
                }
                else if (subdivisionDefaultOrganizationItemClassID == (int?) ObjectClass.Division)
                {
                    return await GetSubdivisionInternalID(subdivisionDefaultOrganizationItemID, cancellationToken);
                }
            }

            return null;
        }

        private async Task<Guid?> GetSubdivisionInternalID(Guid? subdivisionDefaultOrganizationItemID,
            CancellationToken cancellationToken)
        {
            var findSubdivision =
                await _subdivisionsBLL.GetSubdivisionByIDAsync((Guid) subdivisionDefaultOrganizationItemID,
                    cancellationToken);
            if (findSubdivision != null)
            {
                return subdivisionDefaultOrganizationItemID;
            }
            else
            {
                return null;
            }
        }

        private async Task<Guid?> GetOrganizationInternalID(Guid? subdivisionDefaultOrganizationItemID,
            CancellationToken cancellationToken)
        {
            var findOrganization =
                await _organizationsBLL.GetOrganizationByIDAsync((Guid) subdivisionDefaultOrganizationItemID,
                    cancellationToken);
            if (findOrganization != null)
            {
                return subdivisionDefaultOrganizationItemID;
            }
            else
            {
                return null;
            }
        }

        private async Task SaveUsersAsync(ImportData<IUserDetails, User> userImportData,
            ImportData<ISubdivisionDetails, Subdivision> subdivisionsImportData,
            ImportData<OrganizationDetails, Organization> organizationImportData,
            IAdditionalParametersForSelect? additional,
            List<IUserDetails> userDetailsList,
            ErrorStatistics<IUserDetails> errorStatistics,
            CancellationToken cancellationToken)
        {
            //todo: валидация на каждый этап 1)тип 2) после загрузки полей 3) при сохранении 4)при обновлении
            //todo: отрефакторить -
            var users = new List<IUserDetails>();
            var errorOrganizations = new ErrorStatistics<OrganizationDetails>();
            var subdivisionDetailsSet = new HashSet<ISubdivisionDetails>();
            CheckUsers(userImportData, errorStatistics, userDetailsList);
            var additionalTabDetails = userImportData.AdditionalDetails;
            var sortUserHierarchy = SortUserHierarchy(userDetailsList, additionalTabDetails);
            var userDetailsEnumerable = userDetailsList.Except(sortUserHierarchy);
            foreach (var duplicate in userDetailsEnumerable)
            {
                var userName = LogHelper.ToOutputFormat(duplicate.FullName);
                _logger.Information($"Дубликат пользователя {userName}.");
                errorStatistics.ErrorDetails.Add(duplicate,"Дубликат пользователя в источнике");
            }
            _logger.Information($"В источнике обнаружено количество пользователей: {userDetailsList.Count}");
            foreach (var entity in sortUserHierarchy.ToList())
            {
                var singleUserEntity = new List<IUserDetails> {entity};
                await InitUser(userImportData, subdivisionsImportData, organizationImportData,
                    subdivisionDetailsSet,
                    errorOrganizations, errorStatistics, entity, cancellationToken);
                if (singleUserEntity.Any())
                    await _userImportConnector.SaveOrUpdateEntitiesAsync(singleUserEntity, additional, userImportData, errorStatistics,
                        cancellationToken);
            }

            _userImportConnector.LogStatistics(errorStatistics, userImportData);
        }

        public async Task InitUser(ImportData<IUserDetails, User> userImportData,
            ImportData<ISubdivisionDetails, Subdivision> subdivisionsImportData,
            ImportData<OrganizationDetails, Organization> organizationImportData,
            HashSet<ISubdivisionDetails> errorSubdivisions,
            ErrorStatistics<OrganizationDetails> errorOrganizations,
            ErrorStatistics<IUserDetails> errorUsers,
            IUserDetails users,
            CancellationToken cancellationToken)
        {
                var additionalParameters = userImportData.AdditionalDetails;
                await MakeUsersAsync(users,
                    additionalParameters, cancellationToken);

                var userDetailsCollection = new List<IUserDetails>{users};
                await InitUserData(userImportData,
                    subdivisionsImportData,
                    organizationImportData,
                    errorSubdivisions,
                    errorOrganizations,
                    errorUsers,
                    userDetailsCollection,
                    additionalParameters, cancellationToken);


                if (additionalParameters.UpdateLocation)
                {
                    switch ((LocationTypeEnum) additionalParameters.LocationMode)
                    {
                        case LocationTypeEnum.None:
                            SetUserLocation(userDetailsCollection, ImportConstants.DefaultUserWorkplaceID,
                                ImportConstants.DefaultUserRoomID);
                            break;
                        case LocationTypeEnum.Workplace:
                            await SetUserLocationByIdAsync(userDetailsCollection, additionalParameters.LocationItemID,
                                cancellationToken);
                            break;
                        case LocationTypeEnum.Room:
                            await CreateWorkplaceAutomaticallyAsync(userDetailsCollection, additionalParameters.LocationItemID,
                                cancellationToken);
                            break;
                    }
                }
        }

        private void CheckUsers(ImportData<IUserDetails, User> userImportData, ErrorStatistics<IUserDetails> errorUsers, List<IUserDetails> userDetailsList)
        {
            foreach (var entity in userDetailsList.ToList())
            {
                if (userImportData.PreValidate(entity))
                {
                    errorUsers.ErrorDetails.Add(entity,"Запись не может быть записью пользователя тк недостаточно данных.");
                    userDetailsList.Remove(entity);
                    _logger.Information(
                        $"Запись пользователя {LogHelper.ToOutputFormat(userImportData.DetailsKey(entity)?.ToString())} содердит ошибку в данных.");
                }
            }
        }

        private async Task InitUserData(ImportData<IUserDetails, User> userImportData,
            ImportData<ISubdivisionDetails, Subdivision> subdivisionsImportData,
            ImportData<OrganizationDetails, Organization> organizationImportData,
            HashSet<ISubdivisionDetails> errorSubdivisions,
            ErrorStatistics<OrganizationDetails> errorOrganizations,
            ErrorStatistics<IUserDetails> errorUsers,
            ICollection<IUserDetails> users,
            AdditionalTabDetails additionalParameters,
            CancellationToken cancellationToken)
        {
            DefaultSubdivisionData defaultSubdivisionData;
            var departmentID = additionalParameters.UserDefaultOrganizationItemID;
            if (departmentID != null)
            {
                var resultSubdivision = await _subdivisionsBLL.GetSubdivisionByIDAsync(
                    departmentID.Value, cancellationToken);
                if (resultSubdivision == null)
                {
                    _logger.Information($"Заданное в настройках подразделение не найдено в базе.");

                    var additionalParametersSubdivisionDefaultOrganizationItemID =
                        LogHelper.ToOutputFormat(additionalParameters?.SubdivisionDefaultOrganizationItemID
                            ?.ToString());
                    throw new ObjectNotFoundException(
                        $"Подразделение с ID={additionalParametersSubdivisionDefaultOrganizationItemID} не найдено");
                }

                defaultSubdivisionData = new (resultSubdivision.ID, resultSubdivision.FullName,
                    resultSubdivision.OrganizationID);
            }
            else
            {
                defaultSubdivisionData = new (ImportConstants.DefaultSubdivisionID, "",
                    ImportConstants.DefaultOrganizationID);
            }

            var defaultSubdivisionID = await GetDefaultOrgDataAsync(additionalParameters.SubdivisionDefaultOrganizationItemID,
                additionalParameters.SubdivisionDefaultOrganizationItemClassID, cancellationToken);

            foreach (var userDetails in users.ToList())
            {
                try
                {
                    if (additionalParameters.UpdateSubdivision && organizationImportData != null)
                    {
                        await SetOrganization(userDetails, organizationImportData, errorOrganizations,
                            cancellationToken);
                    }


                    var importDataAdditionalDetails = subdivisionsImportData.AdditionalDetails; 
                    var (organizationId, skipUser) = await SetUserSubdivision(userDetails,
                        defaultSubdivisionData,
                        defaultSubdivisionID,
                        importDataAdditionalDetails, cancellationToken);
                    if (skipUser)
                    {
                        var message = $"Не удалось найти подразделение для пользователя {LogHelper.ToOutputFormat(userImportData.DetailsKey(userDetails)?.ToString())}";
                        _logger.Information(message);
                        errorUsers.ErrorDetails.Add(userDetails, message);
                        users.Remove(userDetails);
                    }
                    await SetWorkplace(userDetails, additionalParameters, organizationId, cancellationToken);
                    await SetPosition(userDetails, cancellationToken);
                    userDetails.WorkPlaceID ??= ImportConstants.DefaultUserWorkplaceID;
                    userDetails.RoomID ??= ImportConstants.DefaultUserRoomID;
                }
                catch (Exception e)
                {
                    var modelName = LogHelper.ToOutputFormat(userDetails?.Name);
                    var modelSurname = LogHelper.ToOutputFormat(userDetails.Surname);
                    _logger.Information(
                        $"Ошибка инициализации пользователея {modelName} {modelSurname}");
                    _logger.Error(e, e.Message);
                }
            }
        }

        private async Task SetPosition(IUserDetails userDetails, CancellationToken cancellationToken)
        {
            var model = userDetails.ImportModel;
            if (!string.IsNullOrWhiteSpace(model.UserPosition))
            {
                var position = _mapper.Map<PositionModel>(model);
                var positionName = LogHelper.ToOutputFormat(position.Name);
                _logger.Information($"Поиск должности {positionName} в базе ");
                var entity = await _positionBLL.GetByNameAsync(position, cancellationToken);

                if (entity == null)
                {
                    _logger.Information("Должность не найдена в базе, добавление");
                    entity = await _positionBLL.CreateAsync(position, cancellationToken);
                }

                if (entity != null)
                {
                    var userDetailsName = LogHelper.ToOutputFormat(userDetails.Name);
                    var userDetailsPatronymic = LogHelper.ToOutputFormat(userDetails.Patronymic);
                    var userDetailsSurname = LogHelper.ToOutputFormat(userDetails.Surname);
                    _logger.Information(
                        $"Пользователю {userDetailsName} {userDetailsPatronymic} {userDetailsSurname} установлена должность {positionName}");
                }

                userDetails.PositionID = entity?.ID ?? ImportConstants.DefaultJobTitle;

            }
            else
            {
                userDetails.PositionID = ImportConstants.DefaultJobTitle;
            }
        }

        private async Task SetWorkplace(IUserDetails userDetails, AdditionalTabDetails additionalParameters,
            Guid? organizationId, CancellationToken cancellationToken)
        {
            if (additionalParameters.LocationMode != (byte) LocationTypeEnum.User)
                return;
            var model = userDetails.ImportModel;
            if (model.UserWorkplace != null && additionalParameters.UpdateLocation)
            {
                var userDetailsSubdivisionID = userDetails.SubdivisionID == ImportConstants.DefaultSubdivisionID
                    ? null
                    : userDetails.SubdivisionID;

                var workplaceModel =
                    new WorkplaceModel(model.UserWorkplace, userDetailsSubdivisionID, null)
                    {
                        OrganizationID = organizationId
                    };
                var workplace = await _workplaceBLL.GetWorkplaceByModelAsync(workplaceModel, cancellationToken);

                var userDetailsName = LogHelper.ToOutputFormat(userDetails.Name);
                var userDetailsSurname = LogHelper.ToOutputFormat(userDetails.Surname);
                if (workplace == null)
                {
                    _logger.Information(
                        $"Рабочее место {model.UserWorkplace} для пользователя {userDetailsName} {userDetailsSurname}  не найдено");
                    userDetails.WorkPlaceID = ImportConstants.DefaultUserWorkplaceID;
                    userDetails.RoomID = ImportConstants.DefaultUserRoomID;
                    return;
                }

                var userDetailsPatronymic = LogHelper.ToOutputFormat(userDetails.Patronymic);
                var workplaceName = LogHelper.ToOutputFormat(workplace.Name);
                _logger.Information(
                    $"Пользователю {userDetailsName} {userDetailsPatronymic} {userDetailsSurname} присвоено рабочее место {workplaceName}");
                userDetails.WorkPlaceID = workplace.ID;
                userDetails.RoomID = workplace.RoomID;
            }
        }

        private async Task<(Guid? organizationID, bool skipUser)> SetUserSubdivision(IUserDetails userDetails,
            DefaultSubdivisionData defaultSubdivisionData,
            SubdivisionData defaultOrganizationData,
            AdditionalTabDetails importDataAdditionalDetails,
            CancellationToken cancellationToken)
        {
            Subdivision? resultSubdivision = null;
            //todo:использовать поля пользователя -

            var model = userDetails;
            var userDetailsName = LogHelper.ToOutputFormat(userDetails.Name);
            var userDetailsPatronymic = LogHelper.ToOutputFormat(userDetails.Patronymic);
            var userDetailsSurname = LogHelper.ToOutputFormat(userDetails.Surname);
            if (!string.IsNullOrWhiteSpace(model.SubdivisionExternalID) ||
                new ArrayKey<string>(model.SubdivisionName).IsSet()) //есть данные подразделения
            {
                var organizationId = await GetOrganizationId(model, importDataAdditionalDetails, cancellationToken);
                if (organizationId != null || defaultOrganizationData.OrganizationID != null)
                {
                    Guid? subdivisionID = null;
                    try
                    {
                        subdivisionID = await GetSubdivisionIDAsync(model, importDataAdditionalDetails,
                            organizationId,defaultOrganizationData, cancellationToken);
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e, e.Message);
                        _logger.Information(
                            "Данные для подразделения не получены. Будет установлено подразделение по умолчанию.");
                    }

                    if (subdivisionID != null)
                    {
                        resultSubdivision =
                            await _subdivisions.FirstOrDefaultAsync(x => x.ID == subdivisionID, cancellationToken);
                    }
                }

                if (resultSubdivision == null)
                {
                    var modelSubdivisionName = LogHelper.ToOutputFormat(model.SubdivisionName);
                    var modelSubdivisionExternalID = LogHelper.ToOutputFormat(model.SubdivisionExternalID);
                    var defaultSubdivision = LogHelper.ToOutputFormat(defaultSubdivisionData.SubdivisionFullName);
                    //todo:заполнить username или разделить данные

                    _logger.Information(
                        $"Подразделение {modelSubdivisionName} с id {modelSubdivisionExternalID} не найдено в базе. Используется подразделение по умолчанию {defaultSubdivision}.");
                    if (defaultSubdivisionData.SubdivisionID == ImportConstants.DefaultSubdivisionID)
                        return (defaultSubdivisionData.OrganizationId, true);
                    userDetails.SubdivisionID = defaultSubdivisionData.SubdivisionID;
                    _logger.Information(
                        $"Пользователю {userDetailsName} {userDetailsPatronymic} {userDetailsSurname} присвоено подразделение {defaultSubdivision}");

                    return (defaultSubdivisionData.OrganizationId, !userDetails.SubdivisionID.HasValue);
                }

            }
            else
            {
                var departmentID = importDataAdditionalDetails.UserDefaultOrganizationItemID;
                if (departmentID != null)
                {
                    resultSubdivision = await _subdivisionsBLL.GetSubdivisionByIDAsync(
                        departmentID.Value, cancellationToken);
                    if (resultSubdivision == null)
                    {
                        _logger.Information($"Заданное в настройках подразделение не найдено в базе.");

                        var additionalDetailsSubdivisionDefaultOrganizationItemID =
                            LogHelper.ToOutputFormat(importDataAdditionalDetails.SubdivisionDefaultOrganizationItemID
                                ?.ToString());
                        throw new ObjectNotFoundException(
                            $"Подразделение с ID={additionalDetailsSubdivisionDefaultOrganizationItemID} не найдено");

                    }
                }
            }

            if (resultSubdivision != null)
            {
                var resultSubdivisionName = LogHelper.ToOutputFormat(resultSubdivision.Name);
                _logger.Information(
                    $"Пользователю {userDetailsName} {userDetailsPatronymic} {userDetailsSurname} присвоено подразделение {resultSubdivisionName}");
                userDetails.SubdivisionID = resultSubdivision.ID;
                //todo:заполнить username или разделить данные
            }

            return (resultSubdivision?.OrganizationID,false);
        }

        private async Task<Guid?> GetSubdivisionIDAsync(IUserDetails model,
            AdditionalTabDetails importSettings,
            Guid? organizationID,
            SubdivisionData defaultOrganizationData,
            CancellationToken token)
        {
            Guid? subdivisionID;
            switch ((SubdivisionComparisonEnum) importSettings.SubdivisionComparison)
            {
                case SubdivisionComparisonEnum.Name:
                    subdivisionID =
                        await _subdivisionParentIDQuery.ExecuteAsync(organizationID,
                            model.SubdivisionName,
                            defaultOrganizationData.OrganizationID,
                            defaultOrganizationData.SubdivisionID,
                            token);
                    break;
                case SubdivisionComparisonEnum.ExternalID:
                    if (string.IsNullOrWhiteSpace(model.SubdivisionExternalID))
                        return null;
                    subdivisionID = await _subdivisions.Query()
                        .Where(x => x.ExternalID == model.SubdivisionExternalID)
                        .Select(x => (Guid?) x.ID)
                        .SingleOrDefaultAsync(token);
                    break;
                case SubdivisionComparisonEnum.NameOrExternalID:
                    subdivisionID =
                        await _subdivisionParentIDQuery.ExecuteAsync(organizationID,
                            model.SubdivisionName,
                            defaultOrganizationData.OrganizationID,
                            defaultOrganizationData.SubdivisionID,
                            token);
                    if (subdivisionID != null) return subdivisionID;
                    if (string.IsNullOrWhiteSpace(model.SubdivisionExternalID))
                        return null;
                    subdivisionID = await _subdivisions.Query()
                        .Where(x => x.ExternalID == model.SubdivisionExternalID)
                        .Select(x => (Guid?) x.ID)
                        .SingleOrDefaultAsync(token);

                    return subdivisionID;
                default:
                    throw new NotSupportedException();
            }

            return subdivisionID;
        }

        private async Task<Guid?> GetOrganizationId(IUserDetails model, AdditionalTabDetails importSettings,
            CancellationToken token)
        {
            switch ((OrganisationComparisonEnum) importSettings.OrganizationComparison)
            {
                case OrganisationComparisonEnum.Name:
                    var organizationByName =
                        await _organization.SingleOrDefaultAsync(x => x.Name == model.ParentOrganizationName, token);
                    return organizationByName?.ID;
                case OrganisationComparisonEnum.ExternalID:
                    var organizationByExternalID =
                        await _organization.SingleOrDefaultAsync(
                            x => x.ExternalId == model.ParentOrganizationExternalID,
                            token);
                    return organizationByExternalID?.ID;
                case OrganisationComparisonEnum.NameOrExternalID:
                    var organizationByNameOrExternalID =
                        await _organization.SingleOrDefaultAsync(x => x.Name == model.ParentOrganizationName, token)
                        ??
                        await _organization.SingleOrDefaultAsync(
                            x => x.ExternalId == model.ParentOrganizationExternalID,
                            token);
                    return organizationByNameOrExternalID?.ID;
                default:
                    throw new NotSupportedException();
            }
        }

        private async Task SetOrganization(IUserDetails user,
            ImportData<OrganizationDetails, Organization> importData,
            ErrorStatistics<OrganizationDetails> errorOrganizations,
            CancellationToken cancellationToken)
        {
            var model = user.ImportModel;
            if (importData.AdditionalDetails.UpdateSubdivision &&
                (model.OrganizationExternalID != null || model.OrganizationName != null))
            {
                var organizationDetails = new OrganizationDetails
                    {Name = model.OrganizationName, ExternalId = model.OrganizationExternalID};
                var resultOrganization =
                    await _organizationsBLL.GetOrganizationByIDOrNameAsync(organizationDetails, cancellationToken);
                if (resultOrganization == null && importData.AdditionalDetails.UpdateSubdivision)
                {
                    await _organizationsImportConnector.SaveOrUpdateEntitiesAsync(
                        new List<OrganizationDetails> {organizationDetails},_additional, importData, errorOrganizations,
                         cancellationToken);
                    await _organizationsBLL.GetOrganizationByIDOrNameAsync(organizationDetails, cancellationToken);
                }
            }
        }


        private async Task CreateWorkplaceAutomaticallyAsync(List<IUserDetails> users, Guid? locationItemID,
            CancellationToken cancellationToken)
        {
            if (locationItemID is null)
            {
                var message = "Не указана комната для автоматического создания рабочих мест";
                _logger.Information(message);
                throw new InfraManager.BLL.ObjectNotFoundException(message);
            }

            var room = await _roomBLL.GetAsync(locationItemID, cancellationToken);

            if (room is null)
            {
                var message = $"Не найдена комната для автоматического создания рабочих мест";
                _logger.Information(message);
                throw new ObjectNotFoundException(message);
            }

            foreach (var user in users)
            {
                try
                {
                    var workplaceName = (!string.IsNullOrEmpty(user.LoginName))
                        ? user.LoginName
                        : user.FullName;

                    var workplaceModel = new WorkplaceModel(
                        name: $"Рабочее место {workplaceName}",
                        subdivisionID: user.SubdivisionID,
                        roomID: room.ID
                    );

                    var workplace = await _workplaceBLL.GetOrCreateByModelAsync(workplaceModel, cancellationToken);
                    if (workplace is null)
                    {
                        _logger.Information("Не удалось автоматически создать рабочее место");
                        throw new ObjectNotFoundException("Не удалось автоматически создать рабочее место");
                    }

                    user.RoomID = workplace.RoomID;
                    user.WorkPlaceID = workplace.ID;
                }
                catch (Exception e)
                {
                    var userName = LogHelper.ToOutputFormat(user.Name);
                    var userSurname = LogHelper.ToOutputFormat(user.Surname);
                    _logger.Information($"Ошибка при установке рабочего места для {userName} {userSurname}");
                    _logger.Error(e, e.Message);
                }
            }
        }

        private async Task SetUserLocationByIdAsync(List<IUserDetails> users, Guid? locationItemID,
            CancellationToken cancellationToken)
        {
            var workplace =
                await _workplaceReadonlyRepository.FirstOrDefaultAsync(x => x.IMObjID == locationItemID,
                    cancellationToken);
            if (workplace == null)
            {
                _logger.Information("Не найдено указанное рабочее место");
                throw new ObjectNotFoundException($"Не найдено указанне рабочее место с ID {locationItemID}");
            }

            SetUserLocation(users, workplace.ID, workplace.RoomID);
        }

        private void SetUserLocation(List<IUserDetails> users, int? workplaceNumber, int? roomID)
        {
            foreach (var user in users)
            {
                user.WorkPlaceID = workplaceNumber;
                user.RoomID = roomID;
            }
        }

        private async Task SaveSubdivisionsAsync(ImportData<ISubdivisionDetails, Subdivision> importData,
            List<ISubdivisionDetails> details,
            ErrorStatistics<ISubdivisionDetails> errorSubdivisions,
            CancellationToken cancellationToken)
        {
            _logger.Information(
                $"В источнике обнаружено количество подразделений: {details.Count}");

            //фильтр некорректных данных
            foreach (var entity in details.ToList())
            {
                if (importData.PreValidate(entity))
                {
                    errorSubdivisions.ErrorDetails.Add(entity, "Недостаточно данных");
                    details.Remove(entity);
                    _logger.Information(
                        $"{LogHelper.ToOutputFormat(importData.DetailsKey(entity)?.ToString())} недостаточно данных");
                }
            }
            
            var importDataAdditionalDetails = importData.AdditionalDetails;

            //получение подразделения или организации по умолчанию
            SubdivisionData organizationData = new SubdivisionData(null,null);
            if (importDataAdditionalDetails.UpdateSubdivision)
            {
                organizationData = await GetDefaultOrgDataAsync(importDataAdditionalDetails.SubdivisionDefaultOrganizationItemID,
                    importDataAdditionalDetails.SubdivisionDefaultOrganizationItemClassID,
                    cancellationToken);
            }

            //установка идентификатора подразделения
            foreach (var detail in details.ToList())
            {
                var organization = await GetOrganizationAsync(detail, importDataAdditionalDetails, cancellationToken);
                detail.OrganizationID = organization?.ID;
                var organizationId = organization?.ID ?? organizationData.OrganizationID;
                if (!organizationId.HasValue)
                {
                    
                    detail.SubdivisionID = null;
                    details.Remove(detail);
                    var message = $"Для подразделение {LogHelper.ToOutputFormat(importData.DetailsKey(detail)?.ToString())} не найдена ни указанная организация, ни организация по умолчанию";
                    _logger.Information(message);
                    errorSubdivisions.ErrorDetails.Add(detail, message);

                }
            }
            
            //перестановка для последовательной вставки подразделений
            var parentsFirst = MakeParentsFirst(details, importDataAdditionalDetails);
            var duplicates = details.Except(parentsFirst);
            foreach (var duplicate in duplicates)
            {
                var duplicateSubdivisionName = LogHelper.ToOutputFormat(duplicate.Name);
                var message = $"Дубликат подразделения {duplicateSubdivisionName}.";
                _logger.Information(message);
                var entity = _mapper.Map<SubdivisionDetails>(duplicate);
                errorSubdivisions.ErrorDetails.Add(entity, message);
            }
            details = parentsFirst;
            foreach (var detail in details)
            {
                var inner = new List<ISubdivisionDetails> {detail};

                //восстановление иерархии
                inner = await BuildSubdivisionHierarchy(inner,
                    importDataAdditionalDetails,
                    importData,
                    errorSubdivisions,
                    organizationData,
                    cancellationToken);

                //установка полей по умолчанию
                foreach (var current in inner)
                {
                    if (current is ISubdivisionHierarchyDetails {ParentSubdivision: { }} currentElement)
                        current.OrganizationID = currentElement.ParentSubdivision.OrganizationID;
                }


                if (importDataAdditionalDetails.UpdateSubdivision)
                {
                    await SetOrganizationToSubdivision(inner,
                        importDataAdditionalDetails.SubdivisionDefaultOrganizationItemID,
                        importDataAdditionalDetails.SubdivisionDefaultOrganizationItemClassID,
                        errorSubdivisions,
                        organizationData,
                        cancellationToken);
                }

                //импорт подразделений
                await _subdivisionImportConnector.SaveOrUpdateEntitiesAsync(inner,
                    _additional,
                    importData,
                    errorSubdivisions,
                    cancellationToken);
            }

            _subdivisionImportConnector.LogStatistics(errorSubdivisions, importData);
        }

        private static IEnumerable<ISubdivisionDetails> ReorderAndFilterImportModels(
            HashSet<ISubdivisionDetails> allModels,
            Func<ISubdivisionDetails, ISubdivisionDetails, bool> isParent,
            Func<ISubdivisionDetails, ISubdivisionDetails, bool> isEqual,
            Func<ISubdivisionDetails, ISubdivisionDetails, bool> isChild)
        {
            HashSet<ISubdivisionDetails> passed = new();
            var current = allModels.ToList();


            var childs = current
                .Where(x => current.Any(y => isChild(x, y)))
                .ToList();
            current = current.Where(x => !childs.Contains(x)).ToList();

            var currents = new Stack<IEnumerator<ISubdivisionDetails>>();
            IEnumerator<ISubdivisionDetails> c = current.GetEnumerator();
            while (true)
            {
                if (c.MoveNext())
                {
                    var element = c.Current;
                    if (!passed.Any(x => isEqual(x, element)))
                    {
                        yield return element;
                        passed.Add(element);
                    }

                    var currentChilds = allModels.Where(x => isParent(element, x) && !passed.Any(y => isEqual(y, x)))
                        .ToList();
                    if (currentChilds.Any())
                    {
                        currents.Push(c);
                        c = currentChilds.GetEnumerator();
                    }
                }
                else if (currents.Any())
                {
                    c = currents.Pop();
                }
                else
                {
                    break;
                }

            }
        }

        private IEnumerable<IUserDetails> SortUserImportModels(IEnumerable<IUserDetails> allModels,
            HashSet<string> externalId,
            Func<IUserDetails, string> idGetter,
            Func<IUserDetails, string> parentIdGetter)
        {
            var current = allModels;
            var stack = new Stack<IEnumerable<IUserDetails>>();
            IEnumerable<IUserDetails> first;
            HashSet<string> ids = new();

            var keys = current.Select(x => idGetter(x))
                .Where(x => !(string.IsNullOrWhiteSpace(x))).ToHashSet();
            var childs = current.Select(x => new
                {
                    Model = x,
                    ParentID = parentIdGetter(x)
                }).Where(x => !string.IsNullOrWhiteSpace(x.ParentID) && keys.Contains(x.ParentID))
                .Select(x => x.Model)
                .ToList();
            current = current.Where(x => !childs.Contains(x)).ToList();

            var currents = new Stack<IEnumerator<IUserDetails>>();
            var c = current.GetEnumerator();
            while (true)
            {
                if (c.MoveNext())
                {
                    var element = c.Current;
                    var id = idGetter(element);
                    if (!ids.Contains(idGetter(element)))
                    {
                        yield return element;
                        ids.Add(id);
                    }

                    var currentChilds = allModels.Where(x => parentIdGetter(x) == id && !ids.Contains(idGetter(x)))
                        .ToList();
                    if (currentChilds.Any())
                    {
                        currents.Push(c);
                        c = currentChilds.GetEnumerator();
                    }
                }
                else if (currents.Any())
                {
                    c = currents.Pop();
                }
                else
                {
                    break;
                }

            }
        }

        private async Task<List<ISubdivisionDetails>> BuildSubdivisionHierarchy(List<ISubdivisionDetails> subdivisions,
            AdditionalTabDetails importDataAdditionalDetails, ImportData<ISubdivisionDetails, Subdivision> importData,
            ErrorStatistics<ISubdivisionDetails> errorSubdivisions,
            SubdivisionData defaultOrganizationData,
            CancellationToken cancellationToken)
        {
            var entities = new List<ISubdivisionDetails>();
            var subdivisionComparisonEnum = (SubdivisionComparisonEnum) importDataAdditionalDetails.SubdivisionComparison;
            var defaultSubdivisionID = defaultOrganizationData.SubdivisionID;
            foreach (var entity in subdivisions.ToList())
            {
                var (subdivision, isInnerSubdivision) =
                    await GetParentSubdivision(entity,
                        importDataAdditionalDetails, defaultOrganizationData, cancellationToken);

                if (!entity.OrganizationID.HasValue)
                {
                    entity.OrganizationID = defaultOrganizationData.OrganizationID;
                    entity.SubdivisionID = defaultOrganizationData.SubdivisionID;
                }
                else if (isInnerSubdivision)
                {
                    if (subdivision != null)
                    {
                        entity.SubdivisionID = subdivision.ID;
                        entity.OrganizationID = subdivision.OrganizationID;
                    }
                    else
                    {
                        var subdivisionWithParent = new SubdivisionDetailsWithParentNames(entity);
                        subdivisionWithParent.SubdivisionParent = entity.ParentFullName;

                        var existing = SearchNotSavedSubdivision(subdivisions,
                            entity,
                            subdivisionComparisonEnum);

                        if (existing != null)
                        {
                            entity.ParentSubdivision = existing;
                            entities.Add(entity);
                            continue;
                        }

                        if (defaultSubdivisionID.HasValue)
                        {
                            entity.OrganizationID = defaultOrganizationData.OrganizationID;
                            entity.SubdivisionID = defaultSubdivisionID;
                        }
                        else
                        {
                            var message = $"Для подразделения {LogHelper.ToOutputFormat(importData.DetailsKey(entity)?.ToString())} не найдено родительское подразделение";

                            errorSubdivisions.ErrorDetails.Add(entity, message);
                            _logger.Information(
                                message);
                            continue;
                        }
                    }
                }
                else // корневая
                {
                    entity.SubdivisionID = null;
                }

                entities.Add(entity);
            }

            return entities;
        }

        private async Task MakeUsersAsync(IUserDetails model,
            AdditionalTabDetails dataAdditionalDetails,
            CancellationToken cancellationToken)
        {
            
                var (manager, isSet) =
                    await GetUserAsync(model, cancellationToken, dataAdditionalDetails.UserComparison);

                if (isSet && manager != null)
                {
                    model.ManagerID = manager.IMObjID;
                }
                else
                {
                    model.ManagerID = null;
                }
        }

        private async Task<(User? manager, bool isSet)> GetUserAsync(IUserDetails model,
            CancellationToken cancellationToken, byte additionalDetailsUserComparison)
        {
            if (string.IsNullOrWhiteSpace(model.ManagerIdentifier))
                return (null, false);
            var predicate = GetPredicate(model, (UserComparisonEnum) additionalDetailsUserComparison);
            var manager = await _users.Query()
                .Where(predicate)
                .SingleOrDefaultAsync(cancellationToken);

            return (manager, true);
        }

        private static Expression<Func<User, bool>> GetPredicate(IUserDetails model,
            UserComparisonEnum additionalDetailsUserComparison)
        {
            //todo:рефакторинг 
            //проверить ключи и userparameterlogic
            var modelManagerIdentifier = model.ManagerIdentifier.Trim();
            Expression<Func<User, bool>> predicate =
                additionalDetailsUserComparison switch
                {
                    UserComparisonEnum.ByLogin => x => x.LoginName == modelManagerIdentifier,
                    UserComparisonEnum.ByFullName => x =>
                        ((x.Surname + " " + x.Name).Trim() + " " + x.Patronymic).Trim() == modelManagerIdentifier,
                    UserComparisonEnum.ByNumber => x => x.Number == modelManagerIdentifier,
                    UserComparisonEnum.ByExternalID => x => x.ExternalID == modelManagerIdentifier,
                    UserComparisonEnum.BySID => x => x.SID == modelManagerIdentifier,
                    UserComparisonEnum.ByFirstNameLastName => x =>
                        (x.Surname + " " + x.Name).Trim() == modelManagerIdentifier,
                    _ => throw new ObjectNotFoundException(
                        "Выбранное сопоставление по типу для поиска пользователя не поддерживается")
                };

            return predicate;
        }


        private List<ISubdivisionDetails> MakeParentsFirst(IEnumerable<ISubdivisionDetails> models,
            AdditionalTabDetails importDataAdditionalDetails)
        {
            //todo:проверить правильную конвертицию ??
            Func<ISubdivisionDetails, ISubdivisionDetails, bool> areEqualSubdivision;
            Func<ISubdivisionDetails, ISubdivisionDetails, bool> isParentSubdivision;
            Func<ISubdivisionDetails, ISubdivisionDetails, bool> isChildSubdivision;
            
            switch ((SubdivisionComparisonEnum) importDataAdditionalDetails.SubdivisionComparison)
            {
                case SubdivisionComparisonEnum.Name:
                    areEqualSubdivision = (x,
                        y) => new ArrayKey<string>(x.ParentFullName, x.Name)
                        .Equals(new ArrayKey<string>(y.ParentFullName, y.Name));
                    isParentSubdivision = (x, y) => new ArrayKey<string>(x.ParentFullName, x.Name) ==
                                                    new ArrayKey<string>(y.ParentFullName);
                    isChildSubdivision = (y, x) => new ArrayKey<string>(x.ParentFullName, x.Name) ==
                                                   new ArrayKey<string>(y.ParentFullName);
                    break;
                case SubdivisionComparisonEnum.ExternalID:
                    areEqualSubdivision = (x, y) => x.ExternalID == y.ExternalID;
                    isParentSubdivision = (y, x) => x.ParentExternalID == y.ExternalID;
                    isChildSubdivision = (x, y) => x.ParentExternalID == y.ExternalID;
                    break;
                case SubdivisionComparisonEnum.NameOrExternalID:
                    areEqualSubdivision = (x, y) => new ArrayKey<string>(x.ParentFullName, x.Name)
                                                        .Equals(new ArrayKey<string>(y.ParentFullName, y.Name))
                                                    || x.ExternalID == y.ExternalID;
                    isParentSubdivision = (x, y) => new ArrayKey<string>(x.ParentFullName, x.Name)
                                                        .Equals(new ArrayKey<string>(y.ParentFullName))
                                                    || y.ParentExternalID == x.ExternalID;
                    isChildSubdivision = (y, x) => new ArrayKey<string>(x.ParentFullName, x.Name)
                                                       .Equals(new ArrayKey<string>(y.ParentFullName))
                                                   || y.ParentExternalID == x.ExternalID;
                    break;
                default:
                    throw new InfraManager.BLL.ObjectNotFoundException(
                        "Указанный тип сопоставления подразделений не поддерживается");
            }

            Func<ISubdivisionDetails, ISubdivisionDetails, bool> areEqualOrganizations = (x,y)=>x.OrganizationID == y.OrganizationID;

            Func<ISubdivisionDetails, ISubdivisionDetails, bool> areEqual = (x, y) =>
                areEqualOrganizations(x, y) && areEqualSubdivision(x, y); 
            Func<ISubdivisionDetails, ISubdivisionDetails, bool> isParent = (x,y) => areEqualOrganizations(x,y) && isParentSubdivision(x,y);
            Func<ISubdivisionDetails, ISubdivisionDetails, bool> isChild = (x, y) =>
                areEqualOrganizations(x, y) && isChildSubdivision(x, y);

            var sorted = ReorderAndFilterImportModels(models.ToHashSet(), isParent, areEqual, isChild).ToList();

            return sorted;
        }

        private List<IUserDetails> SortUserHierarchy(IEnumerable<IUserDetails> models, AdditionalTabDetails additional)
        {
            var externalId = new HashSet<string>();
            Func<IUserDetails, string> idGetter;
            Func<IUserDetails, string> parentIdGetter = x => x.ManagerIdentifier;
            switch ((UserComparisonEnum) additional.UserComparison)
            {
                case UserComparisonEnum.ByLogin:
                    idGetter = x => x.LoginName;
                    break;
                case UserComparisonEnum.ByNumber:
                    idGetter = x => x.Number;
                    break;
                case UserComparisonEnum.ByFullName:
                    idGetter = x => x.FullName;
                    break;
                case UserComparisonEnum.ByExternalID:
                    idGetter = x => x.ExternalID;
                    break;
                case UserComparisonEnum.BySID:
                    idGetter = x => x.SID;
                    break;
                case UserComparisonEnum.ByFirstNameLastName:
                    idGetter = x => x.Surname + " " + x.Name;
                    break;
                default:
                    throw new ObjectNotFoundException(
                        "Выбраное сопоставление пользователей для поиска менеджера не поддерживается");
            }

            var sorted = SortUserImportModels(models.ToHashSet(), externalId, idGetter, parentIdGetter).ToList();

            return sorted;
        }

        private static ISubdivisionDetails? SearchNotSavedSubdivision(List<ISubdivisionDetails> subdivisions,
            ISubdivisionDetails model,  SubdivisionComparisonEnum subdivisionComparison)
        {
            var organizationID = model.OrganizationID;
            Func<ISubdivisionDetails, bool> predicate;
            switch (subdivisionComparison)
            {
                case SubdivisionComparisonEnum.Name:
                    predicate = x =>
                        new ArrayKey<string>(x.ParentFullName, x.Name) == new ArrayKey<string>(model.ParentFullName);
                    break;
                case SubdivisionComparisonEnum.ExternalID:
                    predicate = x => x.ExternalID == model.ParentExternalID;
                    break;
                case SubdivisionComparisonEnum.NameOrExternalID:
                    var modelKey = new OrKeys<ArrayKey<string>, StringKey>(
                        new ArrayKey<string>(model.ParentFullName),
                        new StringKey(model.ParentExternalID));
                    predicate = x =>
                    {
                        var key = new OrKeys<ArrayKey<string>, StringKey>(
                            new ArrayKey<string>(x.ParentFullName, x.Name),
                            new StringKey(x.ExternalID));
                        return key == modelKey;
                    };
                    break;
                default:
                    throw new ObjectNotFoundException("Выбранное сопоставление подразделения не поддерживается");
            }

            var fromPredicate = subdivisions.Where(x => x.OrganizationID == organizationID)
                .Where(predicate).ToList();
            var fromPassed =
                fromPredicate.SingleOrDefault();
            return fromPassed;
        }

        private static IUserDetails? SearchNotSavedUser(List<IUserDetails> users, IUserDetails model,
            AdditionalTabDetails details)
        {
            Func<IUserDetails, bool> predicate;
            switch ((UserComparisonEnum) details.UserComparison)
            {
                case UserComparisonEnum.ByLogin:
                    predicate = x => x.LoginName == model.ManagerIdentifier;
                    break;
                case UserComparisonEnum.ByNumber:
                    predicate = x => x.Number == model.ManagerIdentifier;
                    break;
                case UserComparisonEnum.ByFullName:
                    predicate = x => x.FullName == model.ManagerIdentifier;
                    break;
                case UserComparisonEnum.ByExternalID:
                    predicate = x => x.ExternalID == model.ManagerIdentifier;
                    break;
                case UserComparisonEnum.BySID:
                    predicate = x => x.SID == model.ManagerIdentifier;
                    break;
                case UserComparisonEnum.ByFirstNameLastName:
                    predicate = x => string.Join(" ", model.Surname, model.Name) == model.ManagerIdentifier;
                    break;
                default:
                    throw new ObjectNotFoundException(
                        "Выбраное собоставление пользователей для поиска иерархии не проддерживвается");
            }

            var fromPredicate = users
                .Where(predicate).ToList();
            var fromPassed =
                fromPredicate.SingleOrDefault();
            return fromPassed;
        }


        private async Task<Organization?> GetOrganizationAsync(ISubdivisionDetails model,
            AdditionalTabDetails importDataAdditionalDetails,
            CancellationToken cancellationToken)
        {
            Expression<Func<Organization, bool>> predicate;

            switch ((OrganisationComparisonEnum) importDataAdditionalDetails.OrganizationComparison)
            {
                case OrganisationComparisonEnum.Name:
                    if (string.IsNullOrWhiteSpace(model.OrganisationName))
                        return null;
                    predicate = z => !string.IsNullOrWhiteSpace(z.Name) && z.Name == model.OrganisationName;
                    break;
                case OrganisationComparisonEnum.ExternalID:
                    if (string.IsNullOrWhiteSpace(model.OrganisationExternalId))
                        return null;
                    predicate = z =>
                        !string.IsNullOrWhiteSpace(z.ExternalId) && z.ExternalId == model.OrganisationExternalId;
                    break;
                case OrganisationComparisonEnum.NameOrExternalID:
                    if (!(new ArrayKey<string>(model.ParentFullName)).IsSet() &&
                        string.IsNullOrWhiteSpace(model.ExternalID))
                        return null;
                    predicate = z => (!string.IsNullOrWhiteSpace(z.Name) && z.Name == model.OrganisationName)
                                     || (!string.IsNullOrWhiteSpace(z.ExternalId) &&
                                         z.ExternalId == model.OrganisationExternalId);
                    break;
                default:
                    throw new ObjectNotFoundException("Выбранное сопоставление организации не поддерживается");
            }

            var executableQuery = _organization.Query().Where(predicate);
            var organization = await GetOnlyOneAsync(executableQuery, cancellationToken);
            return organization;
        }

        private async Task<(Subdivision? id, bool isInnerSubdivision)> GetParentSubdivision(ISubdivisionDetails model,
            AdditionalTabDetails importDataAdditionalDetails,
            SubdivisionData defaultOrganizationData,
            CancellationToken cancellationToken)
        {
            //todo:по всему проекту заменить ключи на сравнения ??
            //todo:поставить try catch +
            //todo: переделать ArrayKey в хелпер
            var organizationID = model.OrganizationID;
            var resultOrganizationID = organizationID ?? defaultOrganizationData.OrganizationID;
            if (resultOrganizationID == null)
                return (null, false);
            Expression<Func<Subdivision, bool>> predicate;

            switch ((SubdivisionComparisonEnum) importDataAdditionalDetails.SubdivisionComparison)
            {
                case SubdivisionComparisonEnum.Name:
                    return await GetParentSubdivisionAndCheckAsync(organizationID, model.ParentFullName, defaultOrganizationData);

                case SubdivisionComparisonEnum.ExternalID:
                    if (string.IsNullOrWhiteSpace(model.ParentExternalID))
                        return (null, false);
                    predicate = z =>
                        !string.IsNullOrWhiteSpace(z.ExternalID) && z.ExternalID == model.ParentExternalID;
                    break;

                case SubdivisionComparisonEnum.NameOrExternalID:
                    if (string.IsNullOrWhiteSpace(model.ParentExternalID))
                        return (null, false);
                    var (subdivisionByName, isSet) =
                        await GetParentSubdivisionAndCheckAsync(organizationID, model.ParentFullName, defaultOrganizationData);
                    if (!isSet)
                        return (subdivisionByName, false);
                    predicate = z => (!string.IsNullOrWhiteSpace(z.ExternalID) &&
                                      z.ExternalID == model.ParentExternalID);
                    break;
                default:
                    throw new ObjectNotFoundException("Указанное сопоставление подразделений не поддерживается");
            }

            var executableQuery = _subdivisions.Query().Where(predicate);
            var subdivision = await GetOnlyOneAsync(executableQuery, cancellationToken);
            return (subdivision, true);
        }

        private async Task<(Subdivision?, bool)> GetParentSubdivisionAndCheckAsync([DisallowNull] Guid? organizationID,
            IEnumerable<string> subdivisionParent, SubdivisionData defaultOrganizationData)
        {
            var key = new ArrayKey<string>(subdivisionParent);
            if (!key.IsSet())
                return (null, false);
            var parentSubdivisionID =
                await _subdivisionParentIDQuery.ExecuteAsync(organizationID,
                    subdivisionParent,
                    defaultOrganizationData.OrganizationID,
                    defaultOrganizationData.SubdivisionID);
            if (parentSubdivisionID == null)
                return (null, true);
            var parentSubdivision = await _subdivisions.FirstOrDefaultAsync(x => x.ID == parentSubdivisionID);
            return (parentSubdivision, true);
        }



        //todo:перенести в  extension-
        private async Task<T?> GetOnlyOneAsync<T>(IQueryable<T> query, CancellationToken token)
            where T : class
        {
            var count = await query.CountAsync(token);
            if (count != 1)
            {
                if (count > 1)
                //todo:написать расширенный лог
                    _logger.Information($"В базе найдено более одного подразделения");
                return null;
            }
            return await query.SingleOrDefaultAsync(token);
        }

        private async Task<SubdivisionData> GetDefaultOrgDataAsync(
            Guid? subdivisionDefaultOrganizationItemID, 
            int? subdivisionDefaultOrganizationItemClassID,
            CancellationToken cancellationToken)
        {
            SubdivisionData defaultOrgData = new SubdivisionData(null,null);

            if (subdivisionDefaultOrganizationItemClassID != null &&
                subdivisionDefaultOrganizationItemID != null)
            {
                switch ((ObjectClass) subdivisionDefaultOrganizationItemClassID)
                {
                    case ObjectClass.Organizaton:
                        defaultOrgData = new SubdivisionData(subdivisionDefaultOrganizationItemID, null);
                        break;
                    case ObjectClass.Division:

                        var subDivision =
                            await _subdivisionsBLL.GetSubdivisionByIDAsync((Guid) subdivisionDefaultOrganizationItemID,
                                cancellationToken);
                        if (subDivision != null)
                        {
                            defaultOrgData = new SubdivisionData(subDivision.OrganizationID, subDivision.ID);
                        }
                        else
                        {
                            defaultOrgData = new SubdivisionData(null, null);
                            //todo:выход из импорта
                        }

                        break;
                }
            }

            return defaultOrgData;
        }
        
        private async Task SetOrganizationToSubdivision(List<ISubdivisionDetails> subdivisions,
            Guid? subdivisionDefaultOrganizationItemID, int? subdivisionDefaultOrganizationItemClassID,
            ErrorStatistics<ISubdivisionDetails> errorSubdivisions,
            SubdivisionData defaultOrganizationData,
            CancellationToken cancellationToken)
        {
            SubdivisionData defaultOrgData = null;

                var subdivisionDetailsEnumerable = subdivisions.Where(x => !x.OrganizationID.HasValue);
                        subdivisionDetailsEnumerable.ForEach(x =>
                        {
                            x.OrganizationID = defaultOrganizationData.OrganizationID;
                            x.SubdivisionID = defaultOrganizationData.SubdivisionID;
                        });
        }

        private async Task SaveOrganizationsAsync(ImportData<OrganizationDetails, Organization> importData,
            List<OrganizationDetails> organizationDetailsList, ErrorStatistics<OrganizationDetails> errorOrganizations,
            CancellationToken cancellationToken)
        {
            _logger.Information($"В источнике обнаружено количество организаций: {organizationDetailsList.Count}");
            
            await _organizationsImportConnector.SaveOrUpdateEntitiesAsync(organizationDetailsList,
                _additional,
                importData,
                errorOrganizations,
                cancellationToken);
            _organizationsImportConnector.LogStatistics(errorOrganizations, importData);
        }
    }
}
