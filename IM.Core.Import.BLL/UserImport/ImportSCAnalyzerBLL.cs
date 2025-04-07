using AutoMapper;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.ServiceCatalogue;
using InfraManager;
using InfraManager.Core.Extensions;
using InfraManager.Core.Helpers;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.ResourcesArea;
using InfraManager.ServiceBase.ImportService;
using Microsoft.Extensions.Logging;

namespace IM.Core.Import.BLL.Import
{
    internal class ImportSCAnalyzerBLL : IImportSCAnalyzerBLL, ISelfRegisteredService<IImportSCAnalyzerBLL>
    {
        private readonly IMapper _mapper;
        private readonly IServicesBLL _servicesBLL;
        private readonly IServiceCategoriesBLL _serviceCategoriesBLL;


        public ImportSCAnalyzerBLL(IMapper mapper,
            IServicesBLL servicesBLL,
            IServiceCategoriesBLL serviceCategoriesBLL)
        {
            _mapper = mapper;
            _servicesBLL = servicesBLL;
            _serviceCategoriesBLL = serviceCategoriesBLL;
        }
        public async Task SaveAsync(SCImportDetail[] importModels, IProtocolLogger protocolLogger, CancellationToken cancellationToken)
        {
            Dictionary<SCImportDetail, List<string>> errorModels = new Dictionary<SCImportDetail, List<string>>();
            List<SCImportDetail> validModels = new List<SCImportDetail>();
            List<SCImportDetail> modelsForCreate = new List<SCImportDetail>();
            Dictionary<ServiceData, Service> modelsForUpdate = new Dictionary<ServiceData, Service>();

            ValidateModels(importModels, validModels, errorModels);

            protocolLogger.Information($"Моделей для импортирования: {validModels.Count}");

            var importServiceCatalogueData = new ImportSCData();
            await FindAllModelsInDBAsync(importServiceCatalogueData, validModels, cancellationToken);
           
            modelsForCreate = GetModelsForCreate(validModels, importServiceCatalogueData);
            modelsForUpdate = await GetModelsForUpdateAsync(importServiceCatalogueData, cancellationToken);

            await CreateServicesAsync(modelsForCreate, errorModels, protocolLogger, cancellationToken);
            await UpdateServicesAsync(modelsForUpdate, validModels, errorModels, protocolLogger, cancellationToken);

            PrintAllErrors(errorModels, protocolLogger);
        }

        private async Task FindAllModelsInDBAsync(ImportSCData importServiceCatalogueData, List<SCImportDetail> validModels, CancellationToken cancellationToken)
        {
            importServiceCatalogueData.ValidModelsWithoutExternalID = validModels.Where(x => string.IsNullOrEmpty(x.ExternalIdentifier)).ToList();
            importServiceCatalogueData.ValidModelsWithExternalID = validModels.Where(x => !string.IsNullOrEmpty(x.ExternalIdentifier)).ToList();

            importServiceCatalogueData.FindServicesWithExternalIDByExternalID = await _servicesBLL.GetAllServicesByExternalIDAsync(importServiceCatalogueData.ValidModelsWithExternalID, cancellationToken);
            importServiceCatalogueData.FindServicesWithoutExternalIDByCategoryAndNameWithExternalID = await GetServicesByCategoryAndNameButEmptyExternalIDInDB(validModels, importServiceCatalogueData, cancellationToken);
            importServiceCatalogueData.FindServicesWithoutExternalIDByCategoryAndNameWithoutExternalID = await _servicesBLL.GetAllServicesByNameAndCategoryAndEmptyIDAsync(importServiceCatalogueData.ValidModelsWithoutExternalID, cancellationToken);
            importServiceCatalogueData.FindServicesWithExternalIDByCategoryAndNameWithoutExternalID = await _servicesBLL.GetAllServicesByNameAndCategoryWithExternalIDAsync(importServiceCatalogueData.ValidModelsWithoutExternalID, cancellationToken);
        }

        private List<SCImportDetail> GetModelsForCreate(List<SCImportDetail> validModels, ImportSCData importServiceCatalogueData)
        {
            return validModels.Where(x =>
                        !importServiceCatalogueData.FindServicesWithExternalIDByExternalID.Any(y => y.ExternalID == x.ExternalIdentifier)
                        && !importServiceCatalogueData.FindServicesWithoutExternalIDByCategoryAndNameWithExternalID.Any(y => y.Name == x.Service_Name && y.Category.Name.Equals(x.Category_Name))
                        && !importServiceCatalogueData.FindServicesWithoutExternalIDByCategoryAndNameWithoutExternalID.Any(y => y.Name == x.Service_Name && y.Category.Name.Equals(x.Category_Name))
                        && !importServiceCatalogueData.FindServicesWithExternalIDByCategoryAndNameWithoutExternalID.Any(y => y.Name == x.Service_Name && y.Category.Name.Equals(x.Category_Name))).ToList();
        }

        private async Task<Service[]> GetServicesByCategoryAndNameButEmptyExternalIDInDB(List<SCImportDetail> validModels, ImportSCData importServiceCatalogueData, CancellationToken cancellationToken)
        {
            var servicesFromValidModels = validModels.Where(x => !string.IsNullOrEmpty(x.ExternalIdentifier) && !importServiceCatalogueData.FindServicesWithExternalIDByExternalID.Any(y => y.ExternalID.Equals(x.ExternalIdentifier))).ToList();
            return await _servicesBLL.GetAllServicesByNameAndCategoryAndEmptyIDAsync(servicesFromValidModels, cancellationToken);
        }

        private void PrintAllErrors(Dictionary<SCImportDetail, List<string>> errorModels, IProtocolLogger protocolLogger)
        {
            int errorCount = 0;
            foreach (var model in errorModels)
            {
                if (model.Value.Count > 0)
                {
                    foreach (var error in model.Value)
                    {
                        protocolLogger.Information(error);
                    }
                    errorCount++;
                }
            }
            protocolLogger.Information($"Моделей импорта сервисов с ошибками: {errorCount}");
        }

        private async Task UpdateServicesAsync(Dictionary<ServiceData, Service> modelsForUpdate, List<SCImportDetail> validModels, Dictionary<SCImportDetail, List<string>> errorModels, IProtocolLogger protocolLogger, CancellationToken cancellationToken)
        {
            var removedModels = new Dictionary<ServiceData, Service>();
            if (modelsForUpdate.Count > 0)
            {
                foreach (var model in modelsForUpdate)
                {
                    if (await IsNeedCreateNewCategoryAsync(model.Key.CategoryName, cancellationToken))
                    {
                        var newServiceCategory = new ServiceCategory(model.Key.CategoryName);
                        await _serviceCategoriesBLL.CreateAsync(newServiceCategory, cancellationToken);
                        model.Key.CategoryID = newServiceCategory.ID;
                    }

                    if (IsModelChangeCategory(model) || IsModelChangeName(model))
                    {
                        var removedModel = await CheckMultipleNameInCategoryAsync(modelsForUpdate, model, validModels, errorModels, cancellationToken);
                        if (removedModel is not null)
                        {
                            removedModels.Add(removedModel, model.Value);
                        }

                    }
                }

                modelsForUpdate = DeleteUnusedModels(removedModels, modelsForUpdate);
                modelsForUpdate = GetServicesWithDifferenceFromDB(modelsForUpdate, validModels, errorModels);
                await _servicesBLL.UpdateAsync(modelsForUpdate, cancellationToken);
            }
            protocolLogger.Information($"Обновлено сервисов: {modelsForUpdate.Count}");
        }

        private Dictionary<ServiceData, Service> DeleteUnusedModels(Dictionary<ServiceData, Service> removedModels, Dictionary<ServiceData, Service> modelsForUpdate)
        {
            foreach (var removeModel in removedModels)
            {
                modelsForUpdate.Remove(removeModel.Key);
            }
            return modelsForUpdate;
        }

        private async Task<ServiceData> CheckMultipleNameInCategoryAsync(Dictionary<ServiceData, Service> modelsForUpdate, KeyValuePair<ServiceData, Service> model, List<SCImportDetail> validModels, Dictionary<SCImportDetail, List<string>> errorModels, CancellationToken cancellationToken)
        {
            ServiceData removedModel = null;
            var category = await _serviceCategoriesBLL.GetServiceCategoryByNameAsync(model.Key.CategoryName, cancellationToken);
            if (IsMoreThanOneServiceInDBCategory(model.Key.Name, category))
            {
                var validModel = validModels.FirstOrDefault(x => x.Service_Name.Equals(model.Key.Name));
                errorModels[validModel].Add($"В категории {category.Name} уже содержится сервис с именем {validModel.Service_Name}");
                removedModel = model.Key;
            }

            if (IsMoreThanOneServiceInNotCommitedServices(modelsForUpdate, model.Key))
            {
                var validModel = validModels.FirstOrDefault(x => x.Service_Name.Equals(model.Key.Name));
                errorModels[validModel].Add($"В категории {category.Name} не может содержаться несколько сервисов с именем {validModel.Service_Name} при исходном имени {model.Value.Name}");
                removedModel = model.Key;
            }

            return removedModel;
        }

        private bool IsMoreThanOneServiceInNotCommitedServices(Dictionary<ServiceData, Service> modelsForUpdate, ServiceData key)
        {
            var models = modelsForUpdate.Where(x => x.Key.Name.Equals(key.Name) && x.Key.CategoryID.Equals(key.CategoryID) && x.Key.State.Equals(key.State) && x.Key.ExternalID.Equals(key.ExternalID) && x.Key.CategoryName.Equals(key.CategoryName));
            return models.Count() > 1;
        }

        private bool IsModelChangeName(KeyValuePair<ServiceData, Service> model)
        {
            return !model.Key.Name.Equals(model.Value.Name);
        }

        private bool IsModelChangeCategory(KeyValuePair<ServiceData, Service> model)
        {
            if (model.Key.CategoryID is not null)
            {
                return !model.Key.CategoryID.Equals(model.Value.CategoryID);
            }
            else
            {
                return true;
            }
        }

        private bool IsMoreThanOneServiceInDBCategory(string name, ServiceCategory? category)
        {
            return category.Services.Where(x => x.Name.Equals(name)).Count() > 1;
        }

        private Dictionary<ServiceData, Service> GetServicesWithDifferenceFromDB(Dictionary<ServiceData, Service> updateModels, List<SCImportDetail> validModels, Dictionary<SCImportDetail, List<string>> errorModels)
        {
            Dictionary<ServiceData, Service> servicesForUpdate = new Dictionary<ServiceData, Service>();
            foreach (var service in updateModels)
            {
                if (service.Key.State != service.Value.State
                    || service.Key.Name != service.Value.Name
                    || service.Key.ExternalID != service.Value.ExternalID
                    || service.Key.CategoryID != service.Value.CategoryID)
                {
                    servicesForUpdate.Add(service.Key, service.Value);
                }
                else
                {
                    var validModel = validModels.First(x => x.Category_Name.Equals(service.Value.Category.Name) && x.Service_Name.Equals(service.Value.Name));
                    errorModels[validModel].Add($"Для модели {validModel.Service_Name} в категории {validModel.Category_Name} данные не изменились");
                }
            }

            return servicesForUpdate;
        }

        private async Task<bool> IsNeedCreateNewCategoryAsync(string categoryName, CancellationToken cancellationToken)
        {
            var category = await _serviceCategoriesBLL.GetServiceCategoryByNameAsync(categoryName, cancellationToken);
            return category is null;
        }

        private async Task CreateServicesAsync(List<SCImportDetail> modelsForCreate, Dictionary<SCImportDetail, List<string>> errorModels, IProtocolLogger protocolLogger, CancellationToken cancellationToken)
        {
            List<Service> servicesForCreate = new List<Service>();
            Dictionary<ServiceCategory, List<Service>> serviceCategoriesFroCheckDuplicates = new Dictionary<ServiceCategory, List<Service>>();
            foreach (var model in modelsForCreate)
            {
                var serviceCategory = await _serviceCategoriesBLL.GetServiceCategoryByNameAsync(model.Category_Name, cancellationToken);
                if (serviceCategory is null)
                {
                    serviceCategory = new ServiceCategory(model.Category_Name);
                    await _serviceCategoriesBLL.CreateAsync(serviceCategory, cancellationToken);
                }
                var service = _mapper.Map<Service>(model);

                service.CategoryID = serviceCategory.ID;
                if ((serviceCategoriesFroCheckDuplicates.ContainsKey(serviceCategory) && (serviceCategoriesFroCheckDuplicates[serviceCategory].Any(x => x.Name.Equals(service.Name)))) || serviceCategory.Services.Any(x => x.Name == service.Name) || servicesForCreate.Any(x => x.Name == service.Name && x.CategoryID == service.CategoryID))
                {
                    errorModels[model].Add($"В категории {serviceCategory.Name} уже содержится сервис с именем {service.Name}");
                }
                else
                {
                    servicesForCreate.Add(service);
                }

                if (serviceCategoriesFroCheckDuplicates.ContainsKey(serviceCategory))
                {
                    serviceCategoriesFroCheckDuplicates[serviceCategory].Add(service);
                }
                else
                {
                    serviceCategoriesFroCheckDuplicates.Add(serviceCategory, new List<Service> { service });
                }
            }
            if (servicesForCreate.Count > 0)
            {
                await _servicesBLL.CreateAsync(servicesForCreate.ToArray(), cancellationToken);
            }
            protocolLogger.Information($"Создано сервисов: {servicesForCreate.Count}");
        }


        private async Task<Dictionary<ServiceData, Service>> GetModelsForUpdateAsync(ImportSCData importSCData, CancellationToken cancellationToken)
        {
            Dictionary<ServiceData, Service> serviceDetailsForUpdate = new Dictionary<ServiceData, Service>();

            foreach (var modelForUpdate in importSCData.FindServicesWithoutExternalIDByCategoryAndNameWithoutExternalID)
            {
                var modelFromImport = importSCData.ValidModelsWithoutExternalID.FirstOrDefault(x => x.Service_Name.Equals(modelForUpdate.Name) && x.Category_Name.Equals(modelForUpdate.Category.Name));
                var category = await _serviceCategoriesBLL.GetServiceCategoryByNameAsync(modelFromImport.Category_Name, cancellationToken);
                ServiceData serviceDetails = new ServiceData(modelFromImport.Service_Name, category?.ID, TypeHelper.GetEnumByAnotherEnumWithName<CatalogItemState>(typeof(ServiceState), modelFromImport.State), modelFromImport.ExternalIdentifier, modelFromImport.Category_Name);
                serviceDetailsForUpdate.Add(serviceDetails, modelForUpdate);
            }

            foreach (var modelForUpdate in importSCData.FindServicesWithExternalIDByExternalID)
            {
                var modelFromImport = importSCData.ValidModelsWithExternalID.FirstOrDefault(x => x.ExternalIdentifier.Equals(modelForUpdate.ExternalID));
                var category = await _serviceCategoriesBLL.GetServiceCategoryByNameAsync(modelFromImport.Category_Name, cancellationToken);
                ServiceData serviceDetails = new ServiceData(modelFromImport.Service_Name, category?.ID, TypeHelper.GetEnumByAnotherEnumWithName<CatalogItemState>(typeof(ServiceState), modelFromImport.State), modelFromImport.ExternalIdentifier, modelFromImport.Category_Name);
                serviceDetailsForUpdate.Add(serviceDetails, modelForUpdate);
            }

            foreach (var modelForUpdate in importSCData.FindServicesWithoutExternalIDByCategoryAndNameWithExternalID)
            {
                var modelFromImport = importSCData.ValidModelsWithExternalID.FirstOrDefault(x => x.Service_Name.Equals(modelForUpdate.Name) && x.Category_Name.Equals(modelForUpdate.Category.Name));
                var category = await _serviceCategoriesBLL.GetServiceCategoryByNameAsync(modelFromImport.Category_Name, cancellationToken);
                ServiceData serviceDetails = new ServiceData(modelFromImport.Service_Name, category?.ID, TypeHelper.GetEnumByAnotherEnumWithName<CatalogItemState>(typeof(ServiceState), modelFromImport.State), modelFromImport.ExternalIdentifier, modelFromImport.Category_Name);
                serviceDetailsForUpdate.Add(serviceDetails, modelForUpdate);
            }

            foreach (var modelForUpdate in importSCData.FindServicesWithExternalIDByCategoryAndNameWithoutExternalID)
            {
                var modelFromImport = importSCData.ValidModelsWithoutExternalID.FirstOrDefault(x => x.Service_Name.Equals(modelForUpdate.Name) && x.Category_Name.Equals(modelForUpdate.Category.Name));
                var category = await _serviceCategoriesBLL.GetServiceCategoryByNameAsync(modelFromImport.Category_Name, cancellationToken);
                ServiceData serviceDetails = new ServiceData(modelFromImport.Service_Name, category?.ID, TypeHelper.GetEnumByAnotherEnumWithName<CatalogItemState>(typeof(ServiceState), modelFromImport.State), modelForUpdate.ExternalID, modelFromImport.Category_Name);
                serviceDetailsForUpdate.Add(serviceDetails, modelForUpdate);
            }

            return serviceDetailsForUpdate;
        }


        private void ValidateModels(SCImportDetail[] importModels, List<SCImportDetail> validModels, Dictionary<SCImportDetail, List<string>> errorModels)
        {
            foreach (var model in importModels)
            {
                errorModels.Add(model, new List<string>());
                var endString = string.IsNullOrEmpty(model?.Service_Name) ? $"с внешним идентификатором {model?.ExternalIdentifier}" : $"с именем {model?.Service_Name}";

                if (string.IsNullOrEmpty(model.Category_Name))
                {
                    errorModels[model].Add($"Не задано имя категории для записи сервиса {endString}");
                }
                if (!string.IsNullOrEmpty(model.Category_Name) && model.Category_Name.Length > 250)
                {
                    errorModels[model].Add($"Слишком длинное имя для категории сервиса {endString}");
                }
                if (string.IsNullOrEmpty(model.Service_Name))
                {
                    errorModels[model].Add($"Не задано имя для записи сервиса {endString}");
                }
                if (!string.IsNullOrEmpty(model.Service_Name) && model.Service_Name.Length > 250)
                {
                    errorModels[model].Add($"Слишком длинное имя для сервиса {endString}");
                }
                if (!string.IsNullOrEmpty(model.ExternalIdentifier) && model.ExternalIdentifier.Length > 250)
                {
                    errorModels[model].Add($"Слишком длинный внешний идентификатор для сервиса {endString}");
                }
                if (IsInvalidState(model.State))
                {
                    errorModels[model].Add($"Задано некорректное состояние для записи сервиса {endString}");
                }
                if (errorModels[model].Count == 0)
                {
                    validModels.Add(model);
                }
            }
        }

        private bool IsInvalidState(string state)
        {
            string[] validStates = new string[] { Resources.CatalogItemState_Blocked, Resources.CatalogItemState_Excluded, Resources.CatalogItemState_Projected, Resources.CatalogItemState_Worked };
            return !validStates.Contains(state);
        }
    }
}
