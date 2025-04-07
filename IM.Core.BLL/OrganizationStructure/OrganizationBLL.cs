using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.OrganizationStructure
{
    internal class OrganizationBLL : IOrganizationBLL, ISelfRegisteredService<IOrganizationBLL>
    {
        private readonly IFinder<Organization> _finder;
        private readonly IRepository<Organization> _organizationsRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrganizationBLL> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IValidatePermissions<Organization> _permissionsValidator;
        private readonly IGetEntityBLL<Guid, Organization, OrganizationDetails> _detailsBll;
        private readonly IGuidePaggingFacade<Organization, OrganizationsList> _guidePaggingFacade;

        public OrganizationBLL(
            IFinder<Organization> finder,
            IRepository<Organization> organizationsRepository,
            IMapper mapper, 
            IUnitOfWork unitOfWork, 
            ILogger<OrganizationBLL> logger, 
            ICurrentUser currentUser,
            IValidatePermissions<Organization> permissionsValidator,
            IGetEntityBLL<Guid, Organization, OrganizationDetails> detailsBll,
            IGuidePaggingFacade<Organization, OrganizationsList> guidePaggingFacade)
        {
            _finder = finder;
            _organizationsRepository = organizationsRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _currentUser = currentUser;
            _permissionsValidator = permissionsValidator;
            _detailsBll = detailsBll;
            _guidePaggingFacade = guidePaggingFacade;
        }

        public async Task DeleteByIdAsync(Guid organizationID,
            CancellationToken cancellationToken = default)
        {
            await _permissionsValidator.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancellationToken);
            var foundEntity = await _finder.FindAsync(organizationID, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(organizationID, ObjectClass.Organizaton);

            _organizationsRepository.Delete(foundEntity);

            await _unitOfWork.SaveAsync(cancellationToken);
        }

        public async Task<Organization> GetAsync(Guid subdivisionID,
            CancellationToken cancellationToken = default)
        {
            return await _finder.FindAsync(new object[] { subdivisionID }, cancellationToken);
        }

        public async Task<OrganizationDetails> GetOrganizationDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            var organization = await _organizationsRepository.FirstOrDefaultAsync(x=>x.ID == id,cancellationToken) ??
                               throw new ObjectNotFoundException<Guid>(id, "Организация не найдена");
            
            return _mapper.Map<OrganizationDetails>(organization);
        }
        public async Task<Guid> AddOrganizationAsync(OrganizationData organization, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Organization>(organization);

            _organizationsRepository.Insert(entity);

            await _unitOfWork.SaveAsync(cancellationToken);

            return entity.ID;
        }
        public async Task UpdateOrganizationAsync(Guid id,OrganizationData organizationDetails, CancellationToken cancellationToken)
        {
            await _permissionsValidator.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update, cancellationToken);

            _logger.LogTrace($"User (ID = {_currentUser.UserId} is update organization)");
            var foundEntity = await _organizationsRepository.FirstOrDefaultAsync(x=>x.ID == id, cancellationToken) ?? 
                              throw new ObjectNotFoundException<Guid>(id, ObjectClass.Organizaton);
            
            _mapper.Map(organizationDetails,foundEntity);
            
            await _unitOfWork.SaveAsync(cancellationToken);

            _logger.LogTrace($"User (ID = {_currentUser.UserId} is finish update organization)");
        }

        public async Task<OrganizationDetails[]> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace($"User (ID = {_currentUser.UserId} is attempting to view organizations)");

            if (!await _permissionsValidator.UserHasPermissionAsync(_currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken))
            {
                throw new AccessDeniedException("View organizations array.");
            }

            return _mapper.Map<OrganizationDetails[]>(await _organizationsRepository.ToArrayAsync(cancellationToken));
        }

        public Task<OrganizationDetails> DetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return _detailsBll.DetailsAsync(id, cancellationToken);
        }

        public async Task<OrganizationDetails[]> GetListAsync(BaseFilter filter, CancellationToken cancellationToken)
        {
            var organizations = await _guidePaggingFacade.GetPaggingAsync(filter
                , _organizationsRepository.Query()
                , x => x.Name.ToLower().Contains(filter.SearchString.ToLower())
                , cancellationToken);

            return _mapper.Map<OrganizationDetails[]>(organizations);
        }
    }
}
