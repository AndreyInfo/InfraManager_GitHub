using AutoMapper;
using IM.Core.Import.BLL.Interface.Import.ServiceCatalogue;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.Extensions.Logging;

namespace IM.Core.Import.BLL.Import.ServiceCatalogue
{
    internal class ServicesBLL : IServicesBLL, ISelfRegisteredService<IServicesBLL>
    {
        private readonly IUnitOfWork _saveChanges;
        private readonly ILogger<ServicesBLL> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<Service> _repository;
        private readonly IReadonlyRepository<ServiceCategory> _serviceCategoriesReadonlyRepository;
        public ServicesBLL(IUnitOfWork saveChanges,
            ILogger<ServicesBLL> logger,
            IMapper mapper,
            IRepository<Service> repository,
            IReadonlyRepository<ServiceCategory> serviceCategoriesReadonlyRepository)
        {
            _logger = logger;
            _saveChanges = saveChanges;
            _mapper = mapper;
            _repository = repository;
            _serviceCategoriesReadonlyRepository = serviceCategoriesReadonlyRepository;
        }
        public async Task CreateAsync(Service[] services, CancellationToken cancellationToken = default)
        {
            try
            {
                foreach (var service in services)
                {
                    _repository.Insert(service);
                }

                await _saveChanges.SaveAsync(cancellationToken);

            }
            catch (Exception e)
            {
                _logger.LogInformation($"ERR Ошибка добавления сервисов");
                _logger.LogError(e, $"Error Create Services");
                throw;
            }
        }

        public async Task UpdateAsync(Dictionary<ServiceData, Service> updateServices, CancellationToken cancellationToken = default)
        {
            try
            {
                foreach (var service in updateServices)
                {
                    _mapper.Map(service.Key, service.Value);
                }

                await _saveChanges.SaveAsync(cancellationToken);

            }
            catch (Exception e)
            {
                _logger.LogInformation($"ERR Ошибка обновления сервисов");
                _logger.LogError(e, $"Error Update Services");
                throw;
            }
        }

        public async Task<Service[]> GetAllServicesByExternalIDAsync(List<SCImportDetail> services, CancellationToken cancellationToken)
        {
            var servicesExternalIDs = services.Select(x => x.ExternalIdentifier);
            return await _repository.ToArrayAsync(x => servicesExternalIDs.Contains(x.ExternalID), cancellationToken);
        }

        public async Task<Service[]> GetAllServicesByNameAndCategoryAndEmptyIDAsync(List<SCImportDetail> services, CancellationToken cancellationToken)
        {
            var servicesByName = await GetServicesByNameAndEmptyExternalIDAsync(services, cancellationToken);
            return await GetServicesAsync(services, servicesByName,cancellationToken);
        }

        public async Task<Service[]> GetAllServicesByNameAndCategoryWithExternalIDAsync(List<SCImportDetail> services, CancellationToken cancellationToken)
        {
            var servicesByName = await GetServicesByNameWithExternalIDAsync(services, cancellationToken);
            return await GetServicesAsync(services, servicesByName, cancellationToken);
        }

        private async Task<Service[]> GetServicesAsync(List<SCImportDetail> services, Service[] servicesByName, CancellationToken cancellationToken)
        {
            var servicesByCategory = await GetServicesByServiceCategoryAndEmptyExternalIDAsync(services, cancellationToken);
            List<Service> resultServices = new List<Service>();
            foreach (var service in servicesByName)
            {
                if (servicesByCategory.Contains(service))
                {
                    resultServices.Add(service);
                }
            }
            return resultServices.ToArray();
        }


        private async Task<Service[]> GetServicesByServiceCategoryAndEmptyExternalIDAsync(List<SCImportDetail> services, CancellationToken cancellationToken)
        {
            var categoryNames = services.Select(x => x.Category_Name);
            var categories = await _serviceCategoriesReadonlyRepository.ToArrayAsync(x => categoryNames.Contains(x.Name), cancellationToken);
            var categoryIDs = categories.Select(x => x.ID);
            return await _repository.ToArrayAsync(x => categoryIDs.Contains((Guid)x.CategoryID));
        }

        private async Task<Service[]> GetServicesByNameWithExternalIDAsync(List<SCImportDetail> services, CancellationToken cancellationToken)
        {
            var servicesNames = services.Select(x => x.Service_Name);
            return await _repository.ToArrayAsync(x => servicesNames.Contains(x.Name) && !string.IsNullOrEmpty(x.ExternalID), cancellationToken);
        }

        private async Task<Service[]> GetServicesByNameAndEmptyExternalIDAsync(List<SCImportDetail> services, CancellationToken cancellationToken)
        {
            var servicesNames = services.Select(x => x.Service_Name);
            return await _repository.ToArrayAsync(x => servicesNames.Contains(x.Name) && string.IsNullOrEmpty(x.ExternalID), cancellationToken);
        }
    }
}
