using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.Software.SoftwareModels;
using InfraManager.BLL.Software.SoftwareModels.ForTable;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Software;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Software.SoftwareModels.CommonDetails;
using InfraManager.BLL.Software.SoftwareLicenseModelAdditionFields;
using InfraManager.BLL.Software.SoftwareModelTabs.Recognitions;
using InfraManager.BLL.ColumnMapper;

namespace InfraManager.BLL.Software;

internal class SoftwareModelBLL : ISoftwareModelBLL, ISelfRegisteredService<ISoftwareModelBLL>
{
    private readonly ILogger<SoftwareModelBLL> _logger;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;
    private readonly IRepository<SoftwareModel> _softwareModelsRepository;
    private readonly IRepository<Manufacturer> _manufacturersRepository;
    private readonly IValidatePermissions<SoftwareModel> _validatePermissions;
    private readonly IUnitOfWork _saveChangesCommand;
    private readonly IServiceMapper<SoftwareModelTemplate, ISoftwareModelProvider> _serviceMapper;
    private readonly IInsertEntityBLL<SoftwareModel, SoftwareModelData> _insertEntityBLL;
    private readonly IModifyEntityBLL<Guid, SoftwareModel, SoftwareModelData, SoftwareModelDetailsBase> _modifyEntityBLL;
    private readonly IRemoveEntityBLL<Guid, SoftwareModel> _removeEntityBLL;
    private readonly ILicenseModelAdditionFieldsBLL _licenseModelAdditionFieldsBLL;
    private readonly ISupportGroupBLL _supportGroupBLL;
    private readonly ISoftwareModelRecognitionBLL _softwareModelRecognitionBLL;
    private readonly ISoftwareModelQuery _softwareModelQuery;
    private readonly IOrderedColumnQueryBuilder<SoftwareModel, SoftwareModelForTable> _orderedColumnQueryBuilder;

    public SoftwareModelBLL(
        ILogger<SoftwareModelBLL> logger,
        IMapper mapper,
        ICurrentUser currentUser,
        IRepository<SoftwareModel> softwareModelsRepository,
        IRepository<Manufacturer> manufacturersRepository,
        IValidatePermissions<SoftwareModel> validatePermissions,
        IUnitOfWork saveChangesCommand,
        IServiceMapper<SoftwareModelTemplate, ISoftwareModelProvider> serviceMapper,
        IInsertEntityBLL<SoftwareModel, SoftwareModelData> insertEntityBLL,
        IModifyEntityBLL<Guid, SoftwareModel, SoftwareModelData, SoftwareModelDetailsBase> modifyEntityBLL,
        IRemoveEntityBLL<Guid, SoftwareModel> removeEntityBLL,
        ILicenseModelAdditionFieldsBLL licenseModelAdditionFieldsBLL,
        ISupportGroupBLL supportGroupBLL,
        ISoftwareModelRecognitionBLL softwareModelRecognitionBLL,
        ISoftwareModelQuery softwareModelQuery,
        IOrderedColumnQueryBuilder<SoftwareModel, SoftwareModelForTable> orderedColumnQueryBuilder
        )
    {
        _logger = logger;
        _mapper = mapper;
        _currentUser = currentUser;
        _softwareModelsRepository = softwareModelsRepository;
        _manufacturersRepository = manufacturersRepository;
        _validatePermissions = validatePermissions;
        _saveChangesCommand = saveChangesCommand;
        _serviceMapper = serviceMapper;
        _insertEntityBLL = insertEntityBLL;
        _modifyEntityBLL = modifyEntityBLL;
        _removeEntityBLL = removeEntityBLL;
        _licenseModelAdditionFieldsBLL = licenseModelAdditionFieldsBLL;
        _supportGroupBLL = supportGroupBLL;
        _softwareModelRecognitionBLL = softwareModelRecognitionBLL;
        _softwareModelQuery = softwareModelQuery;
        _orderedColumnQueryBuilder = orderedColumnQueryBuilder;
    }

    public async Task<SoftwareModelListItemDetails[]> GetListAsync(BaseFilter filter, CancellationToken cancellationToken = default)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var query = _softwareModelsRepository.Query();

        var orderedQuery = await _orderedColumnQueryBuilder.BuildQueryAsync(filter.ViewName, query, cancellationToken);

        var softwareModelItems = await _softwareModelQuery.ExecuteAsync(_mapper.Map<PaggingFilter>(filter), orderedQuery, cancellationToken);

        return _mapper.Map<SoftwareModelListItemDetails[]>(softwareModelItems);
    }

    public async Task<SoftwareModelDetailsBase> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);

        var softwareModel = await _softwareModelsRepository
            .FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.SoftwareModel);

        var manufacturer = await _manufacturersRepository
            .FirstOrDefaultAsync(x => x.ImObjID == softwareModel.ManufacturerID, cancellationToken);

        var details = await _serviceMapper.Map(softwareModel.Template).GetAsync(softwareModel, softwareModel.Template);

        if (manufacturer is not null)
        {
            details.Manufacturer = new ManufacturerDetails
            {
                ID = (Guid)manufacturer.ImObjID,
                Name = manufacturer.Name
            };
        }

        return details;
    }

    public async Task<SoftwareModelDetailsBase> AddAsync(SoftwareModelData data, CancellationToken cancellationToken = default)
    {
        var saveModel = await _insertEntityBLL.CreateAsync(data, cancellationToken);
        
        await _licenseModelAdditionFieldsBLL.SaveAdditionalFieldsForSoftwareModelAsync(saveModel.ID, data, cancellationToken);
        _supportGroupBLL.SaveSupportGroupForSoftwareModel(saveModel.ID, data);
        _softwareModelRecognitionBLL.SaveModelRecognitionForSoftwareModel(saveModel.ID, data);

        await _saveChangesCommand.SaveAsync(cancellationToken);

        return _mapper.Map<SoftwareModelDetailsBase>(saveModel);
    }

    public async Task<SoftwareModelDetailsBase> UpdateAsync(Guid id, SoftwareModelData data, CancellationToken cancellationToken = default)
    {
        var modifyModel = await _modifyEntityBLL.ModifyAsync(id, data, cancellationToken);

        await _licenseModelAdditionFieldsBLL.UpdateAdditionalFieldsForSoftwareModelAsync(id, data, cancellationToken);
        await _supportGroupBLL.UpdateSupportGroupForSoftwareModelAsync(id, data, cancellationToken);
        await _softwareModelRecognitionBLL.UpdateModelRecognitionForSoftwareModelAsync(id, data, cancellationToken);

        await _saveChangesCommand.SaveAsync(cancellationToken);

        return _mapper.Map<SoftwareModelDetailsBase>(modifyModel);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _removeEntityBLL.RemoveAsync(id, cancellationToken);

        await _saveChangesCommand.SaveAsync(cancellationToken);
    }
}
