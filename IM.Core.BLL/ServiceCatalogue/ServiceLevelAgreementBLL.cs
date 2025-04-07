using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using InfraManager;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.OrganizationStructure;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices;
using InfraManager.Core.Extensions;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ServiceCatalog;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceCatalogue.SLA;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ServiceCatalogue
{
    public class ServiceLevelAgreementBLL : IServiceLevelAgreementBLL, ISelfRegisteredService<IServiceLevelAgreementBLL>
    {
        private readonly IMapper _mapper;

        private readonly IRepository<ServiceLevelAgreement> _slaRepository;
        private readonly IReadonlyRepository<ServiceLevelAgreement> _slaReadonlyRepository;
        private readonly IReadonlyRepository<Organization> _organizationReadonlyRepository;
        private readonly IReadonlyRepository<OrganizationItemGroup> _organizationItemGroupRepository;
        private readonly IFindEntityByGlobalIdentifier<User> _userFinder;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IPortfolioServiceBLL _portfolioServiceBll;
        private readonly IRepository<SLAServiceReference> _slaServiceReferencesRepository;
        private readonly ISLAInfrastructureQuery _slaInfrastructure;
        private readonly ISLAConcludedWithQuery _concludedWithQuery;
        private readonly ILogger<ServiceLevelAgreementBLL> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IValidatePermissions<ServiceLevelAgreement> _slaPermissions;
        private readonly IRepository<Subdivision> _subdivisionRepository;
        private readonly IGuidePaggingFacade<ServiceLevelAgreement, SLAForTable> _paggingFacade;


        public ServiceLevelAgreementBLL(IMapper mapper,
            IUnitOfWork saveChangesCommand,
            IRepository<ServiceLevelAgreement> slaRepository,
            IReadonlyRepository<ServiceLevelAgreement> slaReadonlyRepository,
            IFindEntityByGlobalIdentifier<User> userFinder,
            IReadonlyRepository<OrganizationItemGroup> organizationItemGroupRepository,
            IReadonlyRepository<Organization> organizationReadonlyRepository,
            IPortfolioServiceBLL portfolioServiceBll,
            IRepository<SLAServiceReference> slaServiceReferencesRepository,
            ISLAInfrastructureQuery slaInfrastructure,
            ISLAConcludedWithQuery concludedWithQuery,
            ILogger<ServiceLevelAgreementBLL> logger,
            ICurrentUser currentUser,
            IValidatePermissions<ServiceLevelAgreement> slaPermissions,
            IRepository<Subdivision> subdivisionRepository,
            IGuidePaggingFacade<ServiceLevelAgreement, SLAForTable> paggingFacade)
        {
            _mapper = mapper;
            _saveChangesCommand = saveChangesCommand;
            _slaRepository = slaRepository;
            _slaReadonlyRepository = slaReadonlyRepository;
            _userFinder = userFinder;
            _organizationItemGroupRepository = organizationItemGroupRepository;
            _organizationReadonlyRepository = organizationReadonlyRepository;
            _portfolioServiceBll = portfolioServiceBll;
            _slaServiceReferencesRepository = slaServiceReferencesRepository;
            _slaInfrastructure = slaInfrastructure;
            _concludedWithQuery = concludedWithQuery;
            _logger = logger;
            _currentUser = currentUser;
            _slaPermissions = slaPermissions;
            _subdivisionRepository = subdivisionRepository;
            _paggingFacade = paggingFacade;
        }

        private Guid _currentUserID => _currentUser.UserId;

        public async Task DeleteAsync(Guid slaID,
            CancellationToken cancellationToken = default)
        {
            if (!await _slaPermissions.UserHasPermissionAsync(_currentUserID, ObjectAction.Delete, cancellationToken))
            {
                _logger.LogTrace($"User with ID = {_currentUserID} SLA DELETE access Denied");
                throw new AccessDeniedException(slaID, ObjectClass.SLA);
            }

            var sla = await _slaReadonlyRepository.WithMany(x => x.References).WithMany(x => x.ServiceReferences)
                          .WithMany(x => x.Rules).FirstOrDefaultAsync(x => x.ID == slaID, cancellationToken)
                      ?? throw new ObjectNotFoundException<Guid>(slaID, "Sla Not Found");

            _slaRepository.Delete(sla);

            await _saveChangesCommand.SaveAsync(cancellationToken);
            _logger.LogInformation($"{_currentUserID} deleted SLA with ID = {slaID}");
        }

        public async Task<ServiceLevelAgreementDetails[]> ListAsync(SLAFilter filter,
            CancellationToken cancellationToken = default)
        {
            if (!await _slaPermissions.UserHasPermissionAsync(_currentUserID, ObjectAction.ViewDetailsArray, cancellationToken))
            {
                _logger.LogTrace($"User with ID = {_currentUserID} SLA LIST VIEW access Denied");
                throw new AccessDeniedException($"SLA List View");
            }

            if (filter == null || filter.SelectedItem == null)
                throw new ArgumentNullException(nameof(filter));

            var listOfGroupIDs = await GetGroupsIDsForSLAAsync(filter, cancellationToken);

            if (filter.ObjectID == Owner.DefaultOwnerID && !filter.UserID.HasValue)
            {
                listOfGroupIDs.AddRange(
                    (await _slaReadonlyRepository.WithMany(x => x.OrganizationItemGroups)
                        .ToArrayAsync(x => x.OrganizationItemGroups.Count == 0, cancellationToken)).Select(x => x.ID));
                
                listOfGroupIDs = listOfGroupIDs.Distinct().ToList();
            }

            var slaQuery = _slaReadonlyRepository.WithMany(x => x.OrganizationItemGroups)
                .With(x => x.CalendarWorkSchedule).With(x => x.TimeZone).Query()
                .Where(x => listOfGroupIDs.Contains(x.ID));

            if (!string.IsNullOrWhiteSpace(filter.SearchString))
                slaQuery = slaQuery.Where(x => x.Name.ToLower().Contains(filter.SearchString.ToLower()));

            var result = await _paggingFacade.GetPaggingAsync(filter, slaQuery, null, cancellationToken);

            _logger.LogInformation($"User with ID = {_currentUser.UserId} get SLA List; take={filter.CountRecords}; skip = {filter.StartRecordIndex}");
            return _mapper.Map<ServiceLevelAgreementDetails[]>(result);
        }

        public async Task<Guid> InsertAsync(ServiceLevelAgreementData data, CancellationToken cancellationToken = default)
        {
            if (!await _slaPermissions.UserHasPermissionAsync(_currentUserID, ObjectAction.Insert, cancellationToken))
            {
                _logger.LogTrace($"User ID = {_currentUser.UserId} no access to insert SLA");
                throw new AccessDeniedException($"User ID = {_currentUser.UserId} Insert");
            }

            var entity = _mapper.Map<ServiceLevelAgreement>(data);

            _slaRepository.Insert(entity);

            entity.OrganizationItemGroups.Clear(); // TODO: копипаста (дублирует автомаппер)
            foreach(var item in GetConcludes(entity.ID, data))
            {
                entity.OrganizationItemGroups.Add(item);
            }

            await _saveChangesCommand.SaveAsync(cancellationToken);

            _logger.LogInformation($"User ID = {_currentUserID} insert SLA");
            return entity.ID;
        }

        public async Task UpdateAsync(Guid id, ServiceLevelAgreementData data,
            CancellationToken cancellationToken = default)
        {
            if (!await _slaPermissions.UserHasPermissionAsync(_currentUserID, ObjectAction.Update, cancellationToken))
            {
                _logger.LogTrace($"User ID = {_currentUser.UserId} no access to update SLA");
                throw new AccessDeniedException($"User ID = {_currentUser.UserId} Update");
            }


            var foundEntity = await _slaReadonlyRepository.With(x => x.OrganizationItemGroups)
                                  .FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
                              ?? throw new ObjectNotFoundException($"SLA not found; ID = {id}");

            _mapper.Map(data, foundEntity);

            foundEntity.OrganizationItemGroups.Clear();
            foreach (var item in GetConcludes(id, data)) // TODO: Дублирует автомаппер
            {
                foundEntity.OrganizationItemGroups.Add(item);
            }
            
            _logger.LogInformation($"User ID = {_currentUserID} Updated SLA");

            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        private List<OrganizationItemGroup> GetConcludes(Guid id, ServiceLevelAgreementData data)
        {
            var items = new List<OrganizationItemGroup>();
            foreach (var el in data.Concludeds)
            {
                items.Add(new OrganizationItemGroup(id, el.ObjectID, el.ClassID));
            }

            return items;
        }

        public async Task<ServiceLevelAgreementDetails> GetAsync(Guid slaID, CancellationToken cancellationToken = default)
        {
            if (!await _slaPermissions.UserHasPermissionAsync(_currentUserID, ObjectAction.ViewDetails, cancellationToken))
            {
                _logger.LogTrace($"User ID = {_currentUser.UserId} no access to see Details SLA");
                throw new AccessDeniedException($"User ID = {_currentUser.UserId} Details");
            }

            _slaReadonlyRepository.With(x => x.OrganizationItemGroups);

            var foundEntity = await _slaReadonlyRepository.With(x => x.TimeZone).With(x => x.CalendarWorkSchedule)
                                  .FirstOrDefaultAsync(x => x.ID == slaID, cancellationToken)
                              ?? throw new ObjectNotFoundException($"SLA not found; ID = {slaID}");

            _logger.LogInformation($"User ID = {_currentUserID} got SLA with ID = {slaID}");

            return _mapper.Map<ServiceLevelAgreementDetails>(foundEntity);
        }

        public async Task CloneAsync(Guid slaID, ServiceLevelAgreementData data,
            CancellationToken cancellationToken = default)
        {
            await _slaPermissions.ValidateOrRaiseErrorAsync(_logger, _currentUserID, ObjectAction.Insert,
                cancellationToken);
            
            var foundEntity = await _slaReadonlyRepository.DisableTrackingForQuery().With(x => x.OrganizationItemGroups)
                                  .With(x => x.TimeZone).With(x => x.CalendarWorkSchedule).WithMany(x => x.Rules)
                                  .WithMany(x => x.References).WithMany(x => x.ServiceReferences)
                                  .FirstOrDefaultAsync(x => x.ID == slaID, cancellationToken)
                              ?? throw new ObjectNotFoundException($"SLA not found; ID = {slaID}");

            var cloned = foundEntity.Clone(data.Name, data.FormID, data.Note, data.Number, data.UtcStartDate,
                data.UtcFinishDate, data.TimeZoneID, data.CalendarWorkScheduleID);

            _slaRepository.Insert(cloned);
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        #region ListHelpers

        private async Task<HashSet<Guid>> GetTopLevelIDsAsync(ConcludedDetails selectedItem,
            CancellationToken cancellationToken = default)
        {
            var identifiers = new HashSet<Guid>();

            if (selectedItem.ClassID == ObjectClass.Division)
            {
                var subdivision = await GetSubdivisionWithChildsAsync(selectedItem.ObjectID, cancellationToken);

                foreach (var id in GetTopLevelIDs(subdivision, false))
                {
                    identifiers.Add(id);
                }
            }

            if (selectedItem.ClassID == ObjectClass.User)
            {
                _userFinder.With(x => x.Position)
                    .With(x => x.Subdivision);

                var user = await _userFinder.FindAsync(selectedItem.ObjectID, cancellationToken);

                foreach (var id in GetTopLevelIDs(user.Subdivision, true))
                {
                    identifiers.Add(id);
                }
            }

            return identifiers;
        }

        private IEnumerable<Guid> GetTopLevelIDs(Subdivision subdivision, bool includeSelf)
        {
            if (subdivision == null) throw new ArgumentNullException(nameof(subdivision));

            if (includeSelf)
                yield return subdivision.ID;

            yield return subdivision.OrganizationID;
        }

        private async Task<HashSet<Guid>> GetNestedLevelIDsAsync(ConcludedDetails selectedItem,
            CancellationToken cancellationToken = default)
        {
            var identifiers = new List<Guid>();

            if (selectedItem.ClassID == ObjectClass.Organizaton)
            {
                var organization =
                    await _organizationReadonlyRepository.WithMany(x => x.Subdivisions).FirstOrDefaultAsync(
                        x => x.ID == selectedItem.ObjectID, cancellationToken);

                identifiers = GetAllOrganizationNestedIds(organization);
            }

            if (selectedItem.ClassID == ObjectClass.Division)
            {
                var subdivision = await GetSubdivisionWithChildsAsync(selectedItem.ObjectID, cancellationToken);

                identifiers = GetAllSubdivisionNestedIds(subdivision, false).ToList();
            }

            if (selectedItem.ClassID == ObjectClass.Owner)
            {
                identifiers = (await _organizationItemGroupRepository.ToArrayAsync(cancellationToken)).Select(x => x.ItemID).ToList();
            }

            return identifiers.Distinct().ToHashSet();
        }

        private List<Guid> GetAllOrganizationNestedIds(Organization organization)
        {
            var identifiers = new List<Guid>();

            foreach (var subdivision in organization.Subdivisions)
                foreach (var id in GetAllSubdivisionNestedIds(subdivision, true))
                    identifiers.Add(id);

            return identifiers;
        }

        private IEnumerable<Guid> GetAllSubdivisionNestedIds(Subdivision subdivision, bool includeSelf)
        {
            if (includeSelf)
                yield return subdivision.ID;

            if (subdivision.Users != null)
                foreach (var user in subdivision.Users)
                    yield return user.IMObjID;
        }

        private async Task<Guid[]> GetUsersIDs(Guid userID, CancellationToken cancellationToken = default)
        {
            var user = await _userFinder.FindAsync(userID, cancellationToken) ??
                       throw new ObjectNotFoundException($"User with id = {userID} not found for sla find");

            var existingIDs = new List<Guid> { user.IMObjID };

            if (user.SubdivisionID.HasValue)
            {
                existingIDs.Add(user.SubdivisionID.Value);
            }
            
            if (user.OrganizationId.HasValue)
            {
                existingIDs.Add(user.OrganizationId.Value);
            }

            return existingIDs.ToArray();
        }

        private async Task<List<Guid>> GetGroupsIDsForSLAAsync(SLAFilter filter,
            CancellationToken cancellationToken = default)
        {
            var identifiers = new List<Guid>();

            if (filter.IncludeOwn)
            {
                identifiers.Add(filter.SelectedItem.ObjectID);
            }

            if (filter.IncludeTopLevels)
            {
                var topLevels = await GetTopLevelIDsAsync(filter.SelectedItem, cancellationToken);

                foreach (var id in topLevels)
                    identifiers.Add(id);
            }

            if (filter.IncludeNestedLevels)
            {
                var nestedLevels = await GetNestedLevelIDsAsync(filter.SelectedItem, cancellationToken);

                foreach (var id in nestedLevels)
                    identifiers.Add(id);
            }

            if (filter.UserID.HasValue)
            {
                identifiers.AddRange(await GetUsersIDs(filter.UserID.Value, cancellationToken));
            }
            
            var organizationItems = await _organizationItemGroupRepository
                .ToArrayAsync(x => identifiers.Contains(x.ItemID), cancellationToken);

            return organizationItems.Select(x => x.ID).ToList();
        }


        private async Task<Subdivision> GetSubdivisionWithChildsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _subdivisionRepository.WithMany(x => x.ChildSubdivisions)
                .FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
        }
        #endregion

        public async Task<SLAConcludedWithItem[]> GetConcludedWithAsync(Guid slaID, CancellationToken cancellationToken = default)
        {
            return await _concludedWithQuery.ExecuteAsync(slaID, cancellationToken);
        }

        public async Task<OrganizationItemGroupData[]> GetOrganizationItemGroupsAsync(Guid slaID, CancellationToken cancellationToken = default)
        {
            var foundSLA = await _slaReadonlyRepository.WithMany(x => x.OrganizationItemGroups)
                                  .FirstOrDefaultAsync(x => x.ID == slaID, cancellationToken)
                              ?? throw new ObjectNotFoundException($"SLA not found; ID = {slaID}");

            var items = foundSLA.OrganizationItemGroups.ToArray();

            return _mapper.Map<OrganizationItemGroupData[]>(items);
        }

        #region Infrastructure

        public async Task<PortfolioServiceInfrastructureItem[]> FreeInfrastructureAsync(Guid portfolioServiceID,
            Guid slaID, CancellationToken cancellationToken = default)
        {
            var serviceInfrastructure =
                await _portfolioServiceBll.GetInfrastructureAsync(portfolioServiceID, cancellationToken);

            var slaInfrastructure =
                await _slaServiceReferencesRepository.ToArrayAsync(x => x.SLAID == slaID, cancellationToken);

            if (!slaInfrastructure.Any())
            {
                return serviceInfrastructure;
            }

            return serviceInfrastructure
                .Where(x => !slaInfrastructure.Any(z => z.ServiceReferenceID == x.ID)).ToArray();
        }

        public async Task<PortfolioServiceInfrastructureItem[]> InfrastructureAsync(Guid slaID, Guid serviceID, int? skip = null,
            int? take = null, string searchString = null, CancellationToken cancellationToken = default)
        {
            return await _slaInfrastructure.ExecuteAsync(slaID, serviceID, skip, take, searchString, cancellationToken);
        }

        public async Task InsertInfrastructureAsync(Guid slaID, Guid serviceReferenceID,
            CancellationToken cancellationToken = default)
        {
            _slaServiceReferencesRepository.Insert(new SLAServiceReference(slaID, serviceReferenceID));
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        public async Task DeleteInfrastructureAsync(Guid slaID, Guid serviceReferenceID,
            CancellationToken cancellationToken = default)
        {
            var slaInfrastructure =
                await _slaServiceReferencesRepository.FirstOrDefaultAsync(
                    x => x.SLAID == slaID && x.ServiceReferenceID == serviceReferenceID, cancellationToken)
                ?? throw new ObjectNotFoundException(
                    $"SLA Infrastructure not found; SLAID = {slaID}, serviceReferenceID = {serviceReferenceID}");

            _slaServiceReferencesRepository.Delete(slaInfrastructure);

            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        #endregion
    }
}
