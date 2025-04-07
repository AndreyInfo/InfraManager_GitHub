using AutoMapper;
using IM.Core.Import.BLL.Interface.Import.ServiceCatalogue;
using InfraManager;
using InfraManager.DAL.Import;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraManager.Services;

namespace IM.Core.Import.BLL.Import.ServiceCatalogue
{
    internal class ServiceCategoriesBLL : IServiceCategoriesBLL, ISelfRegisteredService<IServiceCategoriesBLL>
    {
        private readonly IUnitOfWork _saveChanges;
        private readonly IRepository<ServiceCategory> _repository;
        public ServiceCategoriesBLL(IUnitOfWork saveChanges,
            IRepository<ServiceCategory> repository)
        {
            _saveChanges = saveChanges;
            _repository = repository;
        }

        public async Task CreateAsync(ServiceCategory serviceCategory, CancellationToken cancellationToken = default)
        {
            _repository.Insert(serviceCategory);

            await _saveChanges.SaveAsync(cancellationToken);
        }
        public async Task<ServiceCategory?> GetServiceCategoryByNameAsync(string categoryName, CancellationToken cancellationToken)
        {
            return await _repository.FirstOrDefaultAsync(x => x.Name == categoryName, cancellationToken);
        }
        public async Task<ServiceCategory> GetServiceCategoryByIDAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _repository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
        }
        public async Task<IEnumerable<ServiceCategory>> GetAllServiceCategoriesByNameAsync(List<string> serviceCategoriesName, CancellationToken cancellationToken)
        {
            return await _repository.ToArrayAsync(x => serviceCategoriesName.Contains(x.Name), cancellationToken);
        }
    }
}
