using AutoMapper;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.ConfigurationUnit;
using IM.Core.Import.BLL.Interface.Import.ITAsset;
using IM.Core.Import.BLL.Interface.Import.Port;
using IM.Core.Import.BLL.Interface.Import.ProductCatalog;
using IM.Core.Import.BLL.Interface.Import.Slot;
using InfraManager;
using InfraManager.Core.Extensions;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ConfigurationUnits;
using InfraManager.DAL.Finance;
using InfraManager.DAL.Import.ITAsset;
using InfraManager.DAL.ITAsset;
using InfraManager.DAL.Location;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.ServiceBase.ImportService.WebAPIModels.ITAsset;
using System.Linq.Expressions;
using AdapterEntity = InfraManager.DAL.Asset.Adapter;
using AssetEntity = InfraManager.DAL.Asset.Asset;
using EnumBase = System.Enum;
using SlotEntity = InfraManager.DAL.Asset.Slot;

namespace IM.Core.Import.BLL.Import;
internal class ImportITAssetAnalyzerBLL : IImportITAssetAnalyzerBLL
    , ISelfRegisteredService<IImportITAssetAnalyzerBLL>
{
    private readonly IMapper _mapper;
    private readonly IAssetsBLL _assetsBLL;
    private readonly IAdaptersBLL _adaptersBLL;
    private readonly IPeripheralsBLL _peripheralsBLL;
    private readonly INetworkDevicesBLL _networkDevicesBLL;
    private readonly ITerminalDevicesBLL _terminalDevicesBLL;
    private readonly IReadonlyRepository<ProductCatalogType> _pcTypeRepository;
    private readonly IReadonlyRepository<AdapterEntity> _adapterReadonlyRepository;
    private readonly IReadonlyRepository<AdapterType> _adapterTypeRepository;
    private readonly IReadonlyRepository<Peripheral> _peripheralRepository;
    private readonly IReadonlyRepository<PeripheralType> _peripheralTypeRepository;
    private readonly IReadonlyRepository<TerminalDevice> _terminalDeviceRepository;
    private readonly IReadonlyRepository<TerminalDeviceModel> _terminalDeviceModelRepository;
    private readonly IReadonlyRepository<NetworkDevice> _networkDeviceRepository;
    private readonly IReadonlyRepository<NetworkDeviceModel> _networkDeviceModelRepository;
    private readonly IReadonlyRepository<Room> _roomRepository;
    private readonly IReadonlyRepository<Rack> _rackRepository;
    private readonly IReadonlyRepository<Workplace> _workplaceRepository;
    private readonly IReadonlyRepository<NetworkNode> _networkNodeRepository;
    private readonly IReadonlyRepository<Manufacturer> _manufacturerRepository;
    private readonly IReadonlyRepository<Organization> _organizationRepository;
    private readonly IReadonlyRepository<Supplier> _supplierRepository;
    private readonly IConfigurationUnitsBLL _configurationUnitsBLL;
    private readonly IITAssetUndisposedBLL _iTAssetUndisposedBLL;
    private readonly ISlotBLL _slotBLL;
    private readonly IPortBLL _portBLL;
    private readonly IUnitOfWork _saveChanges;

    List<ITAssetImportParsedDetails> _adapterImportList = new();
    List<ITAssetImportParsedDetails> _peripheralImportList = new();
    List<ITAssetImportParsedDetails> _networkDeviceImportList = new();
    List<ITAssetImportParsedDetails> _terminalDeviceImportList = new();

    List<AssetEntity> _assetsForCreate = new();
    List<AssetEntity> _assetsForUpdate = new();
    List<NetworkNode> _cuForCreate = new();
    List<NetworkNode> _cuForUpdate = new();
    List<ITAssetUndisposed> _iTAssetUndisposed = new();
    int _slotCreatedCount = 0;
    int _portCreatedCount = 0;
    private readonly Dictionary<string, ITAssetUndisposedReasonCodeEnum> _undisposedReasonValues = new()
    {
        {"ExternalID",  ITAssetUndisposedReasonCodeEnum.ExternalIDNotUniq},
        {"InvNumber",  ITAssetUndisposedReasonCodeEnum.InvNumberNotUniq},
        {"SerialNumber",  ITAssetUndisposedReasonCodeEnum.SerialNumberIDNotUniq},
        {"Code",  ITAssetUndisposedReasonCodeEnum.CodeNotUniq},
        {"AssetTag",  ITAssetUndisposedReasonCodeEnum.AssetTagNotUniq}
    };
    private Dictionary<ITAssetImportParsedDetails, List<string>> _errorObjects = new();

    private const bool _isControlUniqSerialNumberAndInvNumber = false; // TODO: глобальная настройка, пока не реализована
    private const string _invNumberString = "invNumber";
    private const string _serialNumberString = "SerialNumber";

    public ImportITAssetAnalyzerBLL(IMapper mapper,
        IAssetsBLL assetsBLL,
        IAdaptersBLL adaptersBLL,
        IPeripheralsBLL peripheralsBLL,
        INetworkDevicesBLL networkDevicesBLL,
        ITerminalDevicesBLL terminalDevicesBLL,
        IReadonlyRepository<ProductCatalogType> pcTypeRepository,
        IReadonlyRepository<AdapterEntity> adapterRepository,
        IReadonlyRepository<AdapterType> adapterTypeRepository,
        IReadonlyRepository<Peripheral> peripheralRepository,
        IReadonlyRepository<PeripheralType> peripheralTypeRepository,
        IReadonlyRepository<TerminalDevice> terminalDeviceRepository,
        IReadonlyRepository<TerminalDeviceModel> terminalDeviceModelRepository,
        IReadonlyRepository<NetworkDevice> networkDeviceRepository,
        IReadonlyRepository<NetworkDeviceModel> networkDeviceModelRepository,
        IReadonlyRepository<Room> roomRepository,
        IReadonlyRepository<Rack> rackRepository,
        IReadonlyRepository<Workplace> workplaceRepository,
        IReadonlyRepository<NetworkNode> networkNodeRepository,
        IReadonlyRepository<Manufacturer> manufacturerRepository,
        IReadonlyRepository<Organization> organizationRepository,
        IReadonlyRepository<Supplier> supplierRepository,
        IConfigurationUnitsBLL configurationUnitsBLL,
        IITAssetUndisposedBLL iTAssetUndisposedBLL,
        ISlotBLL slotBLL,
        IPortBLL portBLL,
        IUnitOfWork saveChanges)
    {
        _mapper = mapper;
        _assetsBLL = assetsBLL;
        _adaptersBLL = adaptersBLL;
        _peripheralsBLL = peripheralsBLL;
        _networkDevicesBLL = networkDevicesBLL;
        _terminalDevicesBLL = terminalDevicesBLL;
        _pcTypeRepository = pcTypeRepository;
        _adapterReadonlyRepository = adapterRepository;
        _adapterTypeRepository = adapterTypeRepository;
        _peripheralRepository = peripheralRepository;
        _peripheralTypeRepository = peripheralTypeRepository;
        _terminalDeviceRepository = terminalDeviceRepository;
        _terminalDeviceModelRepository = terminalDeviceModelRepository;
        _networkDeviceRepository = networkDeviceRepository;
        _networkDeviceModelRepository = networkDeviceModelRepository;
        _roomRepository = roomRepository;
        _rackRepository = rackRepository;
        _workplaceRepository = workplaceRepository;
        _networkNodeRepository = networkNodeRepository;
        _manufacturerRepository = manufacturerRepository;
        _organizationRepository = organizationRepository;
        _supplierRepository = supplierRepository;
        _configurationUnitsBLL = configurationUnitsBLL;
        _iTAssetUndisposedBLL = iTAssetUndisposedBLL;
        _slotBLL = slotBLL;
        _portBLL = portBLL;
        _saveChanges = saveChanges;
    }
    public async Task SaveAsync(ITAssetImportSettingDetails settings, ITAssetImportDetails[] importModels, IProtocolLogger protocolLogger, CancellationToken cancellationToken)
    {
        var parsedImportModels = _mapper.Map<ITAssetImportParsedDetails[]>(importModels);

        await SortByModelAsync(parsedImportModels, cancellationToken);
        PrintAllErrors(protocolLogger);

        if (_adapterImportList.Any())
            await SaveOrUpdateAdapterAsync(_adapterImportList, settings, protocolLogger, cancellationToken);

        if (_peripheralImportList.Any())
            await SaveOrUpdatePeripheralAsync(_peripheralImportList, settings, protocolLogger, cancellationToken);

        if (_networkDeviceImportList.Any())
            await SaveOrUpdateNetworkDeviceAsync(_networkDeviceImportList, settings, protocolLogger, cancellationToken);

        if (_terminalDeviceImportList.Any())
            await SaveOrUpdateTerminalDeviceAsync(_terminalDeviceImportList, settings, protocolLogger, cancellationToken);

        await _iTAssetUndisposedBLL.CreateAsync(_iTAssetUndisposed.ToArray(), protocolLogger, cancellationToken);
    }

    private async Task SortByModelAsync(ITAssetImportParsedDetails[] parsedImportModels, CancellationToken cancellationToken)
    {
        foreach (var obj in parsedImportModels)
        {
            _errorObjects.Add(obj, new List<string>());
            var endString = string.IsNullOrEmpty(obj?.Name) ? $"с внешним идентификатором {obj?.ExternalID}" : $"с именем {obj?.Name}";

            if (obj.TypeExternalID == null && obj.TypeName == null)
            {
                _errorObjects[obj].Add($"Тип объекта отсутствует в данных импорта для объекта {endString}");

                var undisposed = _mapper.Map<ITAssetUndisposed>(obj);
                undisposed.ReasonCode = ITAssetUndisposedReasonCodeEnum.TypeNotSpecified;
                _iTAssetUndisposed.Add(undisposed);
            }

            var objectClass = await GetProductCatalogTypeAsync(obj.TypeExternalID, obj.TypeName, cancellationToken);

            if (objectClass == null)
            {
                _errorObjects[obj].Add($"Тип объекта не найден для объекта {endString}");

                var undisposed = _mapper.Map<ITAssetUndisposed>(obj);
                undisposed.ReasonCode = ITAssetUndisposedReasonCodeEnum.TypeNotFound;
                _iTAssetUndisposed.Add(undisposed);
            }

            if (string.IsNullOrEmpty(obj.ModelName))
            {
                _errorObjects[obj].Add($"Наименование модели отсутствует в данных импорта для объекта {endString}");

                var undisposed = _mapper.Map<ITAssetUndisposed>(obj);
                undisposed.ReasonCode = ITAssetUndisposedReasonCodeEnum.ModelNotSpecified;
                _iTAssetUndisposed.Add(undisposed);
            }

            if (string.IsNullOrEmpty(obj.EquipmentExternalID) && string.IsNullOrEmpty(obj.EquipmentName)
                && string.IsNullOrEmpty(obj.RackExternalID) && string.IsNullOrEmpty(obj.RackName)
                && string.IsNullOrEmpty(obj.WorkplaceExternalID) && string.IsNullOrEmpty(obj.WorkplaceName)
                && string.IsNullOrEmpty(obj.RoomExternalID) && string.IsNullOrEmpty(obj.RoomName)
                && string.IsNullOrEmpty(obj.Location))
            {
                _errorObjects[obj].Add($"Наименование рабочего места отсутствует в данных импорта для объекта {endString}");

                var undisposed = _mapper.Map<ITAssetUndisposed>(obj);
                undisposed.ReasonCode = ITAssetUndisposedReasonCodeEnum.LocationNotSpecified;
                _iTAssetUndisposed.Add(undisposed);
            }

            if (_errorObjects[obj].Any())
                continue;

            switch (objectClass.ProductCatalogTemplate.ClassID)
            {
                case ObjectClass.TerminalDevice:
                    _terminalDeviceImportList.Add(obj);
                    break;
                case ObjectClass.ActiveDevice:
                    _networkDeviceImportList.Add(obj);
                    break;
                case ObjectClass.Adapter:
                    _adapterImportList.Add(obj);
                    break;
                case ObjectClass.Peripherial:
                    _peripheralImportList.Add(obj);
                    break;
                default:
                    _errorObjects[obj].Add($"Не удалось определить тип для объекта {endString}");
                    break;
            }
        }
    }

    private async Task SaveOrUpdateAdapterAsync(List<ITAssetImportParsedDetails> importParsedModels, ITAssetImportSettingDetails settings, IProtocolLogger protocolLogger, CancellationToken cancellationToken)
    {
        List<AdapterEntity> objectsForCreate = new List<AdapterEntity>();
        List<AdapterEntity> objectsForUpdate = new List<AdapterEntity>();
        var preparedValues = _adapterReadonlyRepository.Select(x => new { x.Code, x.SerialNumber, x.ExternalID });
        string[] codeValues = preparedValues.Select(x => x.Code).ToArray();
        string[] serialNumberValues = preparedValues.Select(x => x.SerialNumber).ToArray();
        string[] externalIDValues = preparedValues.Select(x => x.ExternalID).ToArray();

        var idenParamPropertyName = GetIdenParamPropertyName(settings.AdapterAndPeripheralIdenParam);

        if (idenParamPropertyName is null)
        {
            protocolLogger.Information($"Не удалось определить правило идентификации объектов");
            return;
        }

        foreach (var modelInstance in importParsedModels)
        {
            var endString = "для объекта " + (string.IsNullOrEmpty(modelInstance?.Name) ? $"с внешним идентификатором {modelInstance?.ExternalID}" : $"с именем {modelInstance?.Name}");

            if (!string.IsNullOrEmpty(modelInstance.RackExternalID) || !string.IsNullOrEmpty(modelInstance.RackName)
                || !string.IsNullOrEmpty(modelInstance.WorkplaceExternalID) || !string.IsNullOrEmpty(modelInstance.WorkplaceName))
            {
                _errorObjects[modelInstance].Add($"Указан неверный тип местоположения в данных импорта {endString}");

                var undisposed = _mapper.Map<ITAssetUndisposed>(modelInstance);
                undisposed.ReasonCode = ITAssetUndisposedReasonCodeEnum.LocationNotCorrect;
                _iTAssetUndisposed.Add(undisposed);

                continue;
            }

            var dynamicExpression = GetDynamicExpression<AdapterEntity>(modelInstance, idenParamPropertyName, endString);
            if (dynamicExpression == null)
            {
                _errorObjects[modelInstance].Add($"Не удалось найти значение по заданным правилам идентификации {endString}");
                continue;
            }

            var modelInstanceFound = _adapterReadonlyRepository.Where(dynamicExpression).ToList();

            Room? defaultLocation = null;
            if (settings.DefaultLocationNotSpecifiedID
                && string.IsNullOrEmpty(modelInstance.EquipmentExternalID) && string.IsNullOrEmpty(modelInstance.EquipmentName)
                && string.IsNullOrEmpty(modelInstance.RoomExternalID) && string.IsNullOrEmpty(modelInstance.RoomName))
                defaultLocation = await _roomRepository.FirstOrDefaultAsync(x => x.ExternalID == settings.DefaultModelID, cancellationToken);

            var activeEquipmentLocation = await GetActiveEquipmentAsync(modelInstance.EquipmentExternalID, modelInstance.EquipmentName, cancellationToken);
            var terminalEquipmentLocation = await GetTerminalEquipmentAsync(modelInstance.EquipmentExternalID, modelInstance.EquipmentName, cancellationToken);
            var roomLocation = await GetRoomAsync(modelInstance.RoomExternalID, modelInstance.RoomName, cancellationToken);

            if (settings.DefaultLocationNotFoundID
                && activeEquipmentLocation == null && terminalEquipmentLocation == null && roomLocation == null)
                defaultLocation = await _roomRepository.FirstOrDefaultAsync(x => x.ExternalID == settings.DefaultModelID, cancellationToken);

            if (activeEquipmentLocation == null && terminalEquipmentLocation == null && (roomLocation == null || defaultLocation == null))
            {
                _errorObjects[modelInstance].Add($"Не найдено местоположение {endString}");

                var undisposed = _mapper.Map<ITAssetUndisposed>(modelInstance);
                undisposed.ReasonCode = ITAssetUndisposedReasonCodeEnum.LocationNotFound;
                _iTAssetUndisposed.Add(undisposed);

                continue;
            }

            var model = await _adapterTypeRepository.FirstOrDefaultAsync(x => x.Name == modelInstance.ModelName, cancellationToken);

            if (!modelInstanceFound.Any())
            {
                _errorObjects[modelInstance].Add($"Не найден объект {endString}");

                var newObject = _mapper.Map<AdapterEntity>(modelInstance);

                if (UniqControl(
                    modelInstance.Code,
                    modelInstance,
                    nameof(modelInstance.Code),
                    codeValues,
                    null,
                    endString))
                    newObject.Code = modelInstance.Code;

                if (UniqControl(modelInstance.SerialNumber, modelInstance, nameof(modelInstance.SerialNumber), serialNumberValues, null, endString))
                    newObject.SerialNumber = modelInstance.SerialNumber;

                if (UniqControl(modelInstance.ExternalID, modelInstance, nameof(modelInstance.ExternalID), externalIDValues, null, endString))
                    newObject.ExternalID = modelInstance.ExternalID;

                if (activeEquipmentLocation != null)
                    newObject.NetworkDeviceID = activeEquipmentLocation.ID;

                if (terminalEquipmentLocation != null)
                    newObject.TerminalDeviceID = terminalEquipmentLocation.ID;

                newObject.RoomID = roomLocation != null ? roomLocation.ID : defaultLocation.ID;

                if (model == null)
                {
                    if (settings.CreateModelAutomatically)
                        model = await CreateAdapterType(modelInstance, protocolLogger, endString, cancellationToken);
                    else
                    {
                        _errorObjects[modelInstance].Add($"Не найдено поле {nameof(modelInstance.ModelName)} {endString}");

                        var undisposed = _mapper.Map<ITAssetUndisposed>(modelInstance);
                        undisposed.ReasonCode = ITAssetUndisposedReasonCodeEnum.ModelNotFound;
                        _iTAssetUndisposed.Add(undisposed);

                        continue;
                    }
                }

                var createdObject = await _adaptersBLL.CreateAsync(newObject, protocolLogger, cancellationToken);
                objectsForCreate.Add(createdObject);

                await CreateOrUpdateAssetAsync(modelInstance, createdObject.ID, protocolLogger, endString, cancellationToken);

                continue;
            }

            if (modelInstanceFound.Count() > 1)
            {
                _errorObjects[modelInstance].Add($"Найдено несколько объектов {endString}");
                continue;
            }

            var adapter = modelInstanceFound.FirstOrDefault();

            if (UniqControl(modelInstance.Code, modelInstance, nameof(modelInstance.Code), codeValues, adapter.Code, endString))
                adapter.Code = modelInstance.Code;

            if (UniqControl(modelInstance.SerialNumber, modelInstance, nameof(modelInstance.SerialNumber), serialNumberValues, adapter.SerialNumber, endString))
                adapter.SerialNumber = modelInstance.SerialNumber;

            if (UniqControl(modelInstance.ExternalID, modelInstance, nameof(modelInstance.ExternalID), externalIDValues, adapter.ExternalID, endString))
                adapter.ExternalID = modelInstance.ExternalID;

            if (model == null)
            {
                if (settings.CreateModelAutomatically)
                    model = await CreateAdapterType(modelInstance, protocolLogger, endString, cancellationToken);

                else
                {
                    _errorObjects[modelInstance].Add($"Не найдено поле {nameof(modelInstance.ModelName)} {endString}");

                    var undisposed = _mapper.Map<ITAssetUndisposed>(modelInstance);
                    undisposed.ReasonCode = ITAssetUndisposedReasonCodeEnum.ModelNotFound;
                    _iTAssetUndisposed.Add(undisposed);

                    continue;
                }
            }

            _mapper.Map(modelInstance, adapter);

            adapter.NetworkDeviceID = activeEquipmentLocation?.ID;
            adapter.TerminalDeviceID = terminalEquipmentLocation?.ID;
            adapter.RoomID = roomLocation != null ? roomLocation.ID : defaultLocation.ID;

            if (model != null)
                adapter.AdapterTypeID = model.IMObjID;

            objectsForUpdate.Add(adapter);

            await CreateOrUpdateAssetAsync(modelInstance, adapter.ID, protocolLogger, endString, cancellationToken);
        }

        PrintAllErrors(protocolLogger);

        await CreateUpdateAsync(objectsForCreate, objectsForUpdate, protocolLogger, cancellationToken);
    }

    private async Task SaveOrUpdatePeripheralAsync(List<ITAssetImportParsedDetails> importParsedModels, ITAssetImportSettingDetails settings, IProtocolLogger protocolLogger, CancellationToken cancellationToken)
    {
        List<Peripheral> objectsForCreate = new List<Peripheral>();
        List<Peripheral> objectsForUpdate = new List<Peripheral>();
        var preparedValues = _peripheralRepository.Select(x => new { x.Code, x.SerialNumber, x.ExternalID });
        string[] codeValues = preparedValues.Select(x => x.Code).ToArray();
        string[] serialNumberValues = preparedValues.Select(x => x.SerialNumber).ToArray();
        string[] externalIDValues = preparedValues.Select(x => x.ExternalID).ToArray();

        var idenParamPropertyName = GetIdenParamPropertyName(settings.AdapterAndPeripheralIdenParam);
        if (idenParamPropertyName is null)
        {
            protocolLogger.Information($"Не удалось определить правило идентификации объектов");
            return;
        }

        foreach (var modelInstance in importParsedModels)
        {
            var endString = "для объекта " + (string.IsNullOrEmpty(modelInstance?.Name) ? $"с внешним идентификатором {modelInstance?.ExternalID}" : $"с именем {modelInstance?.Name}");

            if (!string.IsNullOrEmpty(modelInstance.RackExternalID) || !string.IsNullOrEmpty(modelInstance.RackName)
                || !string.IsNullOrEmpty(modelInstance.WorkplaceExternalID) || !string.IsNullOrEmpty(modelInstance.WorkplaceName))
            {
                _errorObjects[modelInstance].Add($"Указан неверный тип местоположения в данных импорта {endString}");

                var undisposed = _mapper.Map<ITAssetUndisposed>(modelInstance);
                undisposed.ReasonCode = ITAssetUndisposedReasonCodeEnum.LocationNotCorrect;
                _iTAssetUndisposed.Add(undisposed);

                continue;
            }

            var dynamicExpression = GetDynamicExpression<Peripheral>(modelInstance, idenParamPropertyName, endString);
            if (dynamicExpression == null)
            {
                _errorObjects[modelInstance].Add($"Не удалось найти значение по заданным правилам идентификации {endString}");
                continue;
            }

            var modelInstanceFound = _peripheralRepository.Where(dynamicExpression).ToList();

            Room? defaultLocation = null;
            if (settings.DefaultLocationNotSpecifiedID
                && string.IsNullOrEmpty(modelInstance.EquipmentExternalID) && string.IsNullOrEmpty(modelInstance.EquipmentName)
                && string.IsNullOrEmpty(modelInstance.RoomExternalID) && string.IsNullOrEmpty(modelInstance.RoomName))
                defaultLocation = await _roomRepository.FirstOrDefaultAsync(x => x.ExternalID == settings.DefaultModelID, cancellationToken);

            var activeEquipmentLocation = await GetActiveEquipmentAsync(modelInstance.EquipmentExternalID, modelInstance.EquipmentName, cancellationToken);
            var terminalEquipmentLocation = await GetTerminalEquipmentAsync(modelInstance.EquipmentExternalID, modelInstance.EquipmentName, cancellationToken);
            var roomLocation = await GetRoomAsync(modelInstance.RoomExternalID, modelInstance.RoomName, cancellationToken);

            if (settings.DefaultLocationNotFoundID
                && activeEquipmentLocation == null && terminalEquipmentLocation == null && roomLocation == null)
                defaultLocation = await _roomRepository.FirstOrDefaultAsync(x => x.ExternalID == settings.DefaultModelID, cancellationToken);

            if (activeEquipmentLocation == null && terminalEquipmentLocation == null && (roomLocation == null || defaultLocation == null))
            {
                _errorObjects[modelInstance].Add($"Не найдено местоположение {endString}");

                var undisposed = _mapper.Map<ITAssetUndisposed>(modelInstance);
                undisposed.ReasonCode = ITAssetUndisposedReasonCodeEnum.LocationNotFound;
                _iTAssetUndisposed.Add(undisposed);

                continue;
            }

            var model = await _peripheralTypeRepository.FirstOrDefaultAsync(x => x.Name == modelInstance.ModelName, cancellationToken);

            if (!modelInstanceFound.Any())
            {
                _errorObjects[modelInstance].Add($"Не найден объект {endString}");

                var newObject = _mapper.Map<Peripheral>(modelInstance);

                if (UniqControl(modelInstance.Code, modelInstance, nameof(modelInstance.Code), codeValues, null, endString))
                    newObject.Code = modelInstance.Code;

                if (UniqControl(modelInstance.SerialNumber, modelInstance, nameof(modelInstance.SerialNumber), serialNumberValues, null, endString))
                    newObject.SerialNumber = modelInstance.SerialNumber;

                if (UniqControl(modelInstance.ExternalID, modelInstance, nameof(modelInstance.ExternalID), externalIDValues, null, endString))
                    newObject.ExternalID = modelInstance.ExternalID;

                if (activeEquipmentLocation != null)
                    newObject.NetworkDeviceID = activeEquipmentLocation.ID;

                if (terminalEquipmentLocation != null)
                    newObject.TerminalDeviceID = terminalEquipmentLocation.ID;

                newObject.RoomID = roomLocation != null ? roomLocation.ID : defaultLocation.ID;

                if (model == null)
                {
                    if (settings.CreateModelAutomatically)
                        model = await CreatePeripheralType(modelInstance, protocolLogger, endString, cancellationToken);
                    else
                    {
                        _errorObjects[modelInstance].Add($"Не найдено поле {nameof(modelInstance.ModelName)} {endString}");

                        var undisposed = _mapper.Map<ITAssetUndisposed>(modelInstance);
                        undisposed.ReasonCode = ITAssetUndisposedReasonCodeEnum.ModelNotFound;
                        _iTAssetUndisposed.Add(undisposed);

                        continue;
                    }
                }

                var createdObject = await _peripheralsBLL.CreateAsync(newObject, protocolLogger, cancellationToken);
                objectsForCreate.Add(createdObject);

                await CreateOrUpdateAssetAsync(modelInstance, createdObject.ID, protocolLogger, endString, cancellationToken);

                continue;
            }

            if (modelInstanceFound.Count() > 1)
            {
                _errorObjects[modelInstance].Add($"Найдено несколько объектов {endString}");
                continue;
            }

            var peripheral = modelInstanceFound.FirstOrDefault();

            if (UniqControl(modelInstance.Code, modelInstance, nameof(modelInstance.Code), codeValues, peripheral.Code, endString))
                peripheral.Code = modelInstance.Code;

            if (UniqControl(modelInstance.SerialNumber, modelInstance, nameof(modelInstance.SerialNumber), serialNumberValues, peripheral.SerialNumber, endString))
                peripheral.SerialNumber = modelInstance.SerialNumber;

            if (UniqControl(modelInstance.ExternalID, modelInstance, nameof(modelInstance.ExternalID), externalIDValues, peripheral.ExternalID, endString))
                peripheral.ExternalID = modelInstance.ExternalID;

            if (model == null)
            {
                if (settings.CreateModelAutomatically)
                    model = await CreatePeripheralType(modelInstance, protocolLogger, endString, cancellationToken);

                else
                {
                    _errorObjects[modelInstance].Add($"Не найдено поле {nameof(modelInstance.ModelName)} {endString}");

                    var undisposed = _mapper.Map<ITAssetUndisposed>(modelInstance);
                    undisposed.ReasonCode = ITAssetUndisposedReasonCodeEnum.ModelNotFound;
                    _iTAssetUndisposed.Add(undisposed);

                    continue;
                }
            }

            _mapper.Map(modelInstance, peripheral);

            peripheral.NetworkDeviceID = activeEquipmentLocation?.ID;
            peripheral.TerminalDeviceID = terminalEquipmentLocation?.ID;
            peripheral.RoomID = roomLocation != null ? roomLocation.ID : defaultLocation.ID;

            if (model != null)
                peripheral.PeripheralTypeID = model.IMObjID;

            objectsForUpdate.Add(peripheral);

            await CreateOrUpdateAssetAsync(modelInstance, peripheral.ID, protocolLogger, endString, cancellationToken);
        }

        PrintAllErrors(protocolLogger);

        await CreateUpdateAsync(objectsForCreate, objectsForUpdate, protocolLogger, cancellationToken);
    }

    private async Task SaveOrUpdateNetworkDeviceAsync(List<ITAssetImportParsedDetails> importParsedModels, ITAssetImportSettingDetails settings, IProtocolLogger protocolLogger, CancellationToken cancellationToken)
    {
        List<NetworkDevice> objectsForCreate = new List<NetworkDevice>();
        List<NetworkDevice> objectsForUpdate = new List<NetworkDevice>();
        var preparedValues = _networkDeviceRepository.Where(x => !x.Removed)
            .Select(x => new { x.Code, x.SerialNumber, x.InvNumber, x.ExternalID, x.AssetTag });
        string[] codeValues = preparedValues.Select(x => x.Code).ToArray();
        string[] serialNumberValues = preparedValues.Select(x => x.SerialNumber).ToArray();
        string[] invNumberValues = preparedValues.Select(x => x.InvNumber).ToArray();
        string[] externalIDValues = preparedValues.Select(x => x.ExternalID).ToArray();
        string[] assetTagValues = preparedValues.Select(x => x.AssetTag).ToArray();

        var idenParamPropertyName = GetIdenParamPropertyName(settings.NetworkAndTerminalIdenParam);
        if (idenParamPropertyName is null)
        {
            protocolLogger.Information($"Не удалось определить правило идентификации объектов");
            return;
        }

        var cuIdenParamPropertyName = GetIdenParamPropertyName(settings.CUIdenParam);
        if (cuIdenParamPropertyName is null)
        {
            protocolLogger.Information($"Не удалось определить правило идентификации объектов КЕ");
            return;
        }

        foreach (var modelInstance in importParsedModels)
        {
            var endString = "для объекта " + (string.IsNullOrEmpty(modelInstance?.Name) ? $"с внешним идентификатором {modelInstance?.ExternalID}" : $"с именем {modelInstance?.Name}");

            if (!string.IsNullOrEmpty(modelInstance.EquipmentExternalID) || !string.IsNullOrEmpty(modelInstance.EquipmentName)
                || !string.IsNullOrEmpty(modelInstance.WorkplaceExternalID) || !string.IsNullOrEmpty(modelInstance.WorkplaceName))
            {
                _errorObjects[modelInstance].Add($"Указан неверный тип местоположения в данных импорта {endString}");

                var undisposed = _mapper.Map<ITAssetUndisposed>(modelInstance);
                undisposed.ReasonCode = ITAssetUndisposedReasonCodeEnum.LocationNotCorrect;
                _iTAssetUndisposed.Add(undisposed);

                continue;
            }

            var dynamicExpression = GetDynamicExpression<NetworkDevice>(modelInstance, idenParamPropertyName, endString);
            if (dynamicExpression == null)
            {
                _errorObjects[modelInstance].Add($"Не удалось найти значение по заданным правилам идентификации {endString}");
                continue;
            }

            var modelInstanceFound = _networkDeviceRepository
                .Where(dynamicExpression)
                .Where(x => !x.Removed)
                .ToList();

            Room? defaultLocation = null;
            if (settings.DefaultLocationNotSpecifiedID
                && string.IsNullOrEmpty(modelInstance.RackExternalID) && string.IsNullOrEmpty(modelInstance.RackName)
                && string.IsNullOrEmpty(modelInstance.RoomExternalID) && string.IsNullOrEmpty(modelInstance.RoomName))
                defaultLocation = await _roomRepository.FirstOrDefaultAsync(x => x.ExternalID == settings.DefaultModelID, cancellationToken);

            var rackLocation = await GetRackAsync(modelInstance.RackExternalID, modelInstance.RackName, cancellationToken);
            var roomLocation = await GetRoomAsync(modelInstance.RoomExternalID, modelInstance.RoomName, cancellationToken);

            if (settings.DefaultLocationNotFoundID && rackLocation == null && roomLocation == null)
                defaultLocation = await _roomRepository.FirstOrDefaultAsync(x => x.ExternalID == settings.DefaultModelID, cancellationToken);

            if (rackLocation == null && (roomLocation == null || defaultLocation == null))
            {
                _errorObjects[modelInstance].Add($"Не найдено местоположение {endString}");

                var undisposed = _mapper.Map<ITAssetUndisposed>(modelInstance);
                undisposed.ReasonCode = ITAssetUndisposedReasonCodeEnum.LocationNotFound;
                _iTAssetUndisposed.Add(undisposed);

                continue;
            }

            var model = await _networkDeviceModelRepository.FirstOrDefaultAsync(x => x.Name == modelInstance.ModelName, cancellationToken);

            var cuDynamicExpression = GetDynamicExpression<NetworkNode>(modelInstance, cuIdenParamPropertyName, endString);
            if (cuDynamicExpression == null)
            {
                _errorObjects[modelInstance].Add($"Не удалось найти значение по заданным правилам идентификации КЕ {endString}");
                continue;
            }

            var cuFound = _networkNodeRepository.Where(cuDynamicExpression).ToList();

            if (!modelInstanceFound.Any())
            {
                _errorObjects[modelInstance].Add($"Не найден объект {endString}");

                var newObject = _mapper.Map<NetworkDevice>(modelInstance);

                if (UniqControl(modelInstance.Code, modelInstance, nameof(modelInstance.Code), codeValues, null, endString))
                    newObject.Code = modelInstance.Code;

                if (UniqControl(modelInstance.SerialNumber, modelInstance, nameof(modelInstance.SerialNumber), serialNumberValues, null, endString))
                    newObject.SerialNumber = modelInstance.SerialNumber;

                if (UniqControl(modelInstance.InvNumber, modelInstance, nameof(modelInstance.InvNumber), invNumberValues, null, endString))
                    newObject.InvNumber = modelInstance.InvNumber;

                if (UniqControl(modelInstance.ExternalID, modelInstance, nameof(modelInstance.ExternalID), externalIDValues, null, endString))
                    newObject.ExternalID = modelInstance.ExternalID;

                if (UniqControl(modelInstance.AssetTag, modelInstance, nameof(modelInstance.AssetTag), assetTagValues, null, endString))
                    newObject.AssetTag = modelInstance.AssetTag;

                if (rackLocation != null)
                    newObject.RackID = rackLocation.ID;

                newObject.RoomID = roomLocation != null ? roomLocation.ID : defaultLocation.ID;

                if (model == null)
                {
                    _errorObjects[modelInstance].Add($"Не найдено поле {nameof(modelInstance.ModelName)} {endString}");

                    var undisposed = _mapper.Map<ITAssetUndisposed>(modelInstance);
                    undisposed.ReasonCode = ITAssetUndisposedReasonCodeEnum.ModelNotFound;
                    _iTAssetUndisposed.Add(undisposed);

                    continue;
                }

                var createdObject = await _networkDevicesBLL.CreateAsync(newObject, protocolLogger, cancellationToken);
                objectsForCreate.Add(createdObject);

                await CreateSlotsAsync(createdObject.IMObjID, model.IMObjID, protocolLogger, cancellationToken);
                await CreatePortsAsync(createdObject, model.IMObjID, protocolLogger, cancellationToken);

                await CreateOrUpdateAssetAsync(modelInstance, createdObject.ID, protocolLogger, endString, cancellationToken);

                await SaveOrUpdateCUAsync(modelInstance, cuFound, createdObject.ID, null, endString);

                continue;
            }

            if (modelInstanceFound.Count() > 1)
            {
                _errorObjects[modelInstance].Add($"Найдено несколько объектов {endString}");
                continue;
            }

            var networkDevice = modelInstanceFound.FirstOrDefault();

            if (UniqControl(modelInstance.Code, modelInstance, nameof(modelInstance.Code), codeValues, networkDevice.Code, endString))
                networkDevice.Code = modelInstance.Code;

            if (UniqControl(modelInstance.SerialNumber, modelInstance, nameof(modelInstance.SerialNumber), serialNumberValues, networkDevice.SerialNumber, endString))
                networkDevice.SerialNumber = modelInstance.SerialNumber;

            if (UniqControl(modelInstance.InvNumber, modelInstance, nameof(modelInstance.InvNumber), invNumberValues, networkDevice.InvNumber, endString))
                networkDevice.InvNumber = modelInstance.InvNumber;

            if (UniqControl(modelInstance.ExternalID, modelInstance, nameof(modelInstance.ExternalID), externalIDValues, networkDevice.ExternalID, endString))
                networkDevice.ExternalID = modelInstance.ExternalID;

            if (UniqControl(modelInstance.AssetTag, modelInstance, nameof(modelInstance.AssetTag), assetTagValues, networkDevice.AssetTag, endString))
                networkDevice.AssetTag = modelInstance.AssetTag;

            if (model == null)
            {
                _errorObjects[modelInstance].Add($"Не найдено поле {nameof(modelInstance.ModelName)} {endString}");

                var undisposed = _mapper.Map<ITAssetUndisposed>(modelInstance);
                undisposed.ReasonCode = ITAssetUndisposedReasonCodeEnum.ModelNotFound;
                _iTAssetUndisposed.Add(undisposed);

                continue;
            }

            _mapper.Map(modelInstance, networkDevice);

            networkDevice.RackID = rackLocation?.ID;
            networkDevice.RoomID = roomLocation != null ? roomLocation.ID : defaultLocation.ID;

            if (model != null)
                networkDevice.Model = model;

            objectsForUpdate.Add(networkDevice);

            await CreateOrUpdateAssetAsync(modelInstance, networkDevice.ID, protocolLogger, endString, cancellationToken);

            await SaveOrUpdateCUAsync(modelInstance, cuFound, networkDevice.ID, null, endString);

            continue;
        }

        PrintAllErrors(protocolLogger);

        await CreateUpdateAsync(objectsForCreate, objectsForUpdate, protocolLogger, cancellationToken);
    }

    private async Task SaveOrUpdateTerminalDeviceAsync(List<ITAssetImportParsedDetails> importParsedModels, ITAssetImportSettingDetails settings, IProtocolLogger protocolLogger, CancellationToken cancellationToken)
    {
        List<TerminalDevice> objectsForCreate = new List<TerminalDevice>();
        List<TerminalDevice> objectsForUpdate = new List<TerminalDevice>();
        var preparedValues = _terminalDeviceRepository.Where(x => !x.Removed)
            .Select(x => new { x.Code, x.SerialNumber, x.InvNumber, x.ExternalID, x.AssetTag });
        string[] codeValues = preparedValues.Select(x => x.Code).ToArray();
        string[] serialNumberValues = preparedValues.Select(x => x.SerialNumber).ToArray();
        string[] invNumberValues = preparedValues.Select(x => x.InvNumber).ToArray();
        string[] externalIDValues = preparedValues.Select(x => x.ExternalID).ToArray();
        string[] assetTagValues = preparedValues.Select(x => x.AssetTag).ToArray();

        var idenParamPropertyName = GetIdenParamPropertyName(settings.NetworkAndTerminalIdenParam);
        if (idenParamPropertyName is null)
        {
            protocolLogger.Information($"Не удалось определить правило идентификации объектов");
            return;
        }

        var cuIdenParamPropertyName = GetIdenParamPropertyName(settings.CUIdenParam);
        if (cuIdenParamPropertyName is null)
        {
            protocolLogger.Information($"Не удалось определить правило идентификации объектов КЕ");
            return;
        }

        foreach (var modelInstance in importParsedModels)
        {
            var endString = "для объекта " + (string.IsNullOrEmpty(modelInstance?.Name) ? $"с внешним идентификатором {modelInstance?.ExternalID}" : $"с именем {modelInstance?.Name}");

            if (!string.IsNullOrEmpty(modelInstance.RackName))
            {
                _errorObjects[modelInstance].Add($"Указан неверный тип местоположения в данных импорта {endString}");

                var undisposed = _mapper.Map<ITAssetUndisposed>(modelInstance);
                undisposed.ReasonCode = ITAssetUndisposedReasonCodeEnum.LocationNotCorrect;
                _iTAssetUndisposed.Add(undisposed);

                continue;
            }

            var dynamicExpression = GetDynamicExpression<TerminalDevice>(modelInstance, idenParamPropertyName, endString);
            if (dynamicExpression == null)
            {
                _errorObjects[modelInstance].Add($"Не удалось найти значение по заданным правилам идентификации {endString}");
                continue;
            }

            var modelInstanceFound = _terminalDeviceRepository
                .Where(dynamicExpression)
                .Where(x => !x.Removed)
                .ToList();

            Room? defaultLocation = null;
            if (settings.DefaultLocationNotSpecifiedID
                && string.IsNullOrEmpty(modelInstance.WorkplaceExternalID) && string.IsNullOrEmpty(modelInstance.WorkplaceName)
                && string.IsNullOrEmpty(modelInstance.RoomExternalID) && string.IsNullOrEmpty(modelInstance.RoomName))
                defaultLocation = await _roomRepository.FirstOrDefaultAsync(x => x.ExternalID == settings.DefaultModelID, cancellationToken);

            var workplaceLocation = await GetWorkplaceAsync(modelInstance.WorkplaceExternalID, modelInstance.WorkplaceName, cancellationToken);
            var roomLocation = await GetRoomAsync(modelInstance.RoomExternalID, modelInstance.RoomName, cancellationToken);

            if (settings.DefaultLocationNotFoundID && workplaceLocation == null && roomLocation == null)
                defaultLocation = await _roomRepository.FirstOrDefaultAsync(x => x.ExternalID == settings.DefaultModelID, cancellationToken);

            if (workplaceLocation == null && (roomLocation == null || defaultLocation == null))
            {
                _errorObjects[modelInstance].Add($"Не найдено местоположение {endString}");

                var undisposed = _mapper.Map<ITAssetUndisposed>(modelInstance);
                undisposed.ReasonCode = ITAssetUndisposedReasonCodeEnum.LocationNotFound;
                _iTAssetUndisposed.Add(undisposed);

                continue;
            }

            var model = await _terminalDeviceModelRepository.FirstOrDefaultAsync(x => x.Name == modelInstance.ModelName, cancellationToken);

            var cuDynamicExpression = GetDynamicExpression<NetworkNode>(modelInstance, cuIdenParamPropertyName, endString);
            if (cuDynamicExpression == null)
            {
                _errorObjects[modelInstance].Add($"Не удалось найти значение по заданным правилам идентификации КЕ {endString}");
                continue;
            }

            var cuFound = _networkNodeRepository.Where(cuDynamicExpression).ToList();

            if (!modelInstanceFound.Any())
            {
                _errorObjects[modelInstance].Add($"Не найден объект {endString}");

                var newObject = _mapper.Map<TerminalDevice>(modelInstance);

                if (UniqControl(modelInstance.Code, modelInstance, nameof(modelInstance.Code), codeValues, null, endString))
                    newObject.Code = modelInstance.Code;

                if (UniqControl(modelInstance.SerialNumber, modelInstance, nameof(modelInstance.SerialNumber), serialNumberValues, null, endString))
                    newObject.SerialNumber = modelInstance.SerialNumber;

                if (UniqControl(modelInstance.InvNumber, modelInstance, nameof(modelInstance.InvNumber), invNumberValues, null, endString))
                    newObject.InvNumber = modelInstance.InvNumber;

                if (UniqControl(modelInstance.ExternalID, modelInstance, nameof(modelInstance.ExternalID), externalIDValues, null, endString))
                    newObject.ExternalID = modelInstance.ExternalID;

                if (UniqControl(modelInstance.AssetTag, modelInstance, nameof(modelInstance.AssetTag), assetTagValues, null, endString))
                    newObject.AssetTag = modelInstance.AssetTag;

                if (workplaceLocation != null)
                    newObject.Workplace = workplaceLocation;

                newObject.RoomID = roomLocation != null ? roomLocation.ID : defaultLocation.ID;

                if (model == null)
                {
                    if (settings.CreateModelAutomatically)
                        model = await CreateTerminalDeviceType(modelInstance, protocolLogger, endString, cancellationToken);
                    else
                    {
                        _errorObjects[modelInstance].Add($"Не найдено поле {nameof(modelInstance.ModelName)} {endString}");

                        var undisposed = _mapper.Map<ITAssetUndisposed>(modelInstance);
                        undisposed.ReasonCode = ITAssetUndisposedReasonCodeEnum.ModelNotFound;
                        _iTAssetUndisposed.Add(undisposed);

                        continue;
                    }
                }

                var createdObject = await _terminalDevicesBLL.CreateAsync(newObject, protocolLogger, cancellationToken);
                objectsForCreate.Add(createdObject);

                await CreateOrUpdateAssetAsync(modelInstance, createdObject.ID, protocolLogger, endString, cancellationToken);

                await SaveOrUpdateCUAsync(modelInstance, cuFound, null, createdObject.ID, endString);

                continue;
            }

            if (modelInstanceFound.Count() > 1)
            {
                _errorObjects[modelInstance].Add($"Найдено несколько объектов {endString}");
                continue;
            }

            var terminalDevice = modelInstanceFound.FirstOrDefault();

            if (UniqControl(modelInstance.Code, modelInstance, nameof(modelInstance.Code), codeValues, terminalDevice.Code, endString))
                terminalDevice.Code = modelInstance.Code;

            if (UniqControl(modelInstance.SerialNumber, modelInstance, nameof(modelInstance.SerialNumber), serialNumberValues, terminalDevice.SerialNumber, endString))
                terminalDevice.SerialNumber = modelInstance.SerialNumber;

            if (UniqControl(modelInstance.InvNumber, modelInstance, nameof(modelInstance.InvNumber), invNumberValues, terminalDevice.InvNumber, endString))
                terminalDevice.InvNumber = modelInstance.InvNumber;

            if (UniqControl(modelInstance.ExternalID, modelInstance, nameof(modelInstance.ExternalID), externalIDValues, terminalDevice.ExternalID, endString))
                terminalDevice.ExternalID = modelInstance.ExternalID;

            if (UniqControl(modelInstance.AssetTag, modelInstance, nameof(modelInstance.AssetTag), assetTagValues, terminalDevice.AssetTag, endString))
                terminalDevice.AssetTag = modelInstance.AssetTag;

            if (model == null)
            {
                if (settings.CreateModelAutomatically)
                    model = await CreateTerminalDeviceType(modelInstance, protocolLogger, endString, cancellationToken);

                else if (terminalDevice.TypeID == null)
                {
                    _errorObjects[modelInstance].Add($"Не найдено поле {nameof(modelInstance.ModelName)} {endString}");

                    var undisposed = _mapper.Map<ITAssetUndisposed>(modelInstance);
                    undisposed.ReasonCode = ITAssetUndisposedReasonCodeEnum.ModelNotFound;
                    _iTAssetUndisposed.Add(undisposed);

                    continue;
                }
            }

            _mapper.Map(modelInstance, terminalDevice);

            terminalDevice.Workplace = workplaceLocation;
            terminalDevice.RoomID = roomLocation != null ? roomLocation.ID : defaultLocation.ID;

            if (model != null)
                terminalDevice.Model = model;

            objectsForUpdate.Add(terminalDevice);

            await CreateOrUpdateAssetAsync(modelInstance, terminalDevice.ID, protocolLogger, endString, cancellationToken);

            await SaveOrUpdateCUAsync(modelInstance, cuFound, null, terminalDevice.ID, endString);

            continue;
        }

        PrintAllErrors(protocolLogger);

        await CreateUpdateAsync(objectsForCreate, objectsForUpdate, protocolLogger, cancellationToken);
    }

    private async Task CreateUpdateAsync<TEntity>(List<TEntity> objectsForCreate
        , List<TEntity> objectsForUpdate
        , IProtocolLogger protocolLogger
        , CancellationToken cancellationToken)
    {
        if (objectsForCreate.Any())
            protocolLogger.Information($"Обновлено объектов: {objectsForUpdate.Count}");

        if (_assetsForCreate.Any())
        {
            await _assetsBLL.CreateAsync(_assetsForCreate.ToArray(), protocolLogger, cancellationToken);
            protocolLogger.Information($"Создано объектов имущества: {_assetsForCreate.Count}");
            _assetsForCreate.Clear();
        }

        if (_cuForCreate != null && _cuForCreate.Any())
        {
            await _configurationUnitsBLL.CreateAsync(_cuForCreate.ToArray(), protocolLogger, cancellationToken);
            protocolLogger.Information($"Создано КЕ: {_cuForCreate.Count}");
            _cuForCreate.Clear();
        }

        if (objectsForUpdate.Any())
            protocolLogger.Information($"Обновлено объектов: {objectsForUpdate.Count}");

        if (_assetsForUpdate.Any())
        {
            protocolLogger.Information($"Обновлено объектов имущества: {objectsForUpdate.Count}");
            _assetsForUpdate.Clear();
        }

        if (_cuForUpdate != null && _cuForUpdate.Any())
        {
            protocolLogger.Information($"Обновлено КЕ: {_cuForUpdate.Count}");
            _cuForUpdate.Clear();
        }

        if (_slotCreatedCount > 0)
            protocolLogger.Information($"Создано слотов оборудования: {_slotCreatedCount}");

        if (_portCreatedCount > 0)
            protocolLogger.Information($"Создано портов оборудования: {_portCreatedCount}");

        await _saveChanges.SaveAsync(cancellationToken);
    }

    private static Func<TEntity, bool> GetParamEqualPredicate<TEntity>(string propertyName, string propertyValue)
    {
        var param = Expression.Parameter(typeof(TEntity), "x");
        var value = Expression.Constant(propertyValue, typeof(string));
        var member = Expression.Property(param, propertyName);
        Expression body = Expression.Equal(member, value);
        var final = Expression.Lambda<Func<TEntity, bool>>(body: body, parameters: param);
        return final.Compile();
    }

    private string GetIdenParamPropertyName(byte idenParam)
    => EnumBase
        .GetValues(typeof(ITAssetIdentificationParameterEnum))
        .Cast<ITAssetIdentificationParameterEnum>()
        .FirstOrDefault(x => (byte)x == idenParam)
        .ToString();

    private string GetPropertyValue(ITAssetImportParsedDetails modelInstance, string idenParamPropertyName)
        => modelInstance.GetType().GetProperty(idenParamPropertyName).GetValue(modelInstance, null).ToString();

    private async Task SaveOrUpdateCUAsync(ITAssetImportParsedDetails modelInstance
        , List<NetworkNode> cuFound
        , int? networkDeviceID
        , int? terminalDeviceID
        , string endString)
    {
        if (!cuFound.Any())
        {
            //Создаем КЕ и связь с объектом
            var pcType = await _pcTypeRepository.FirstOrDefaultAsync(x => x.ExternalID == modelInstance.TypeExternalID || x.Name == modelInstance.TypeName);
            _cuForCreate.Add(
                new NetworkNode
                {
                    Name = modelInstance.Name,
                    ProductCatalogTypeID = pcType.IMObjID,
                    NetworkDeviceID = networkDeviceID,
                    TerminalDeviceID = terminalDeviceID,
                    IPAddress = modelInstance.IpAddress,
                    IPMask = modelInstance.IpMask,
                });
            _errorObjects[modelInstance].Add($"Создана связь с КЕ {endString}");

            return;
        }

        if (cuFound.Count() > 1)
        {
            _errorObjects[modelInstance].Add($"Найдено несколько КЕ {endString}");
            return;
        }

        var configurationUnit = cuFound.FirstOrDefault();

        // Создаем связь с объектом
        if (networkDeviceID != null)
            configurationUnit.NetworkDeviceID = networkDeviceID;

        if (terminalDeviceID != null)
            configurationUnit.TerminalDeviceID = terminalDeviceID;

        configurationUnit.IPMask = modelInstance.IpMask;
        _cuForUpdate.Add(configurationUnit);
        _errorObjects[modelInstance].Add($"Обновлена связь с КЕ {endString}");
    }

    private async Task CreateOrUpdateAssetAsync(ITAssetImportParsedDetails modelInstance, int objectID, IProtocolLogger protocolLogger, string endString, CancellationToken cancellationToken)
    {
        var owner = await GetOrganizationAsync(modelInstance.OwnerExternalID, modelInstance.OwnerName, cancellationToken);
        if (owner == null)
            _errorObjects[modelInstance].Add($"Не найдено поле {nameof(modelInstance.OwnerName)} {endString}");

        var utilizer = await GetOrganizationAsync(modelInstance.UtilizerExternalID, modelInstance.UtilizerName, cancellationToken);
        if (utilizer == null)
            _errorObjects[modelInstance].Add($"Не найдено поле {nameof(modelInstance.UtilizerName)} {endString}");

        var supplier = await GetSupplierAsync(modelInstance.SupplierExternalID, modelInstance.SupplierName, cancellationToken);
        if (supplier == null)
            _errorObjects[modelInstance].Add($"Не найдено поле {nameof(modelInstance.SupplierName)} {endString}");

        var assetsFound = await _assetsBLL.GetAsync(objectID, protocolLogger, cancellationToken);
        if (!assetsFound.Any())
        {
            var newAsset = _mapper.Map<AssetEntity>(modelInstance);
            newAsset.DeviceID = objectID;

            if (owner != null)
                newAsset.OwnerID = owner.ID;

            if (utilizer != null)
                newAsset.UtilizerID = utilizer.ID;

            if (supplier != null)
                newAsset.SupplierID = supplier.ID;

            _assetsForCreate.Add(newAsset);
            return;
        }

        else if (assetsFound.Count() > 1)
        {
            _errorObjects[modelInstance].Add($"Найдено несколько объектов имущества {endString}");
            return;
        }

        else
        {
            var asset = assetsFound.FirstOrDefault();

            asset.OwnerID = owner?.ID;
            asset.UtilizerID = utilizer?.ID;
            asset.SupplierID = supplier?.ID;

            _mapper.Map(modelInstance, asset);
            _assetsForUpdate.Add(asset);
        }
    }

    private Func<TEntity, bool>? GetDynamicExpression<TEntity>(ITAssetImportParsedDetails modelInstance, string propertyName, string endString)
    {
        var propertyValue = GetPropertyValue(modelInstance, propertyName);

        if (string.IsNullOrEmpty(propertyValue))
        {
            _errorObjects[modelInstance].Add($"Не удалось определить значение правила идентификации {propertyName} {endString}");
            return null;
        }

        return GetParamEqualPredicate<TEntity>(propertyName, propertyValue);
    }

    private async Task<AdapterType> CreateAdapterType(ITAssetImportParsedDetails modelInstance, IProtocolLogger protocolLogger, string endString, CancellationToken cancellationToken)
    {
        var vendor = await GetVendorAsync(modelInstance.VendorExternalID, modelInstance.VendorName, cancellationToken);
        if (vendor == null)
            _errorObjects[modelInstance].Add($"Не найдено поле {nameof(modelInstance.VendorName)} {endString}");

        var pcType = await GetProductCatalogTypeAsync(modelInstance.TypeExternalID, modelInstance.TypeName, cancellationToken);
        if (pcType == null)
            _errorObjects[modelInstance].Add($"Не найдено поле {nameof(modelInstance.TypeName)} {endString}");

        return await _adaptersBLL.CreateTypeAsync(new AdapterType()
        {
            Name = modelInstance.ModelName,
            ManufacturerID = vendor.ID,
            ProductCatalogTypeID = pcType.IMObjID,
            CanBuy = true
        }, protocolLogger, cancellationToken);
    }

    private async Task<PeripheralType> CreatePeripheralType(ITAssetImportParsedDetails modelInstance, IProtocolLogger protocolLogger, string endString, CancellationToken cancellationToken)
    {
        var vendor = await GetVendorAsync(modelInstance.VendorExternalID, modelInstance.VendorName, cancellationToken);
        if (vendor == null)
            _errorObjects[modelInstance].Add($"Не найдено поле {nameof(modelInstance.VendorName)} {endString}");

        var pcType = await GetProductCatalogTypeAsync(modelInstance.TypeExternalID, modelInstance.TypeName, cancellationToken);
        if (pcType == null)
            _errorObjects[modelInstance].Add($"Не найдено поле {nameof(modelInstance.TypeName)} {endString}");

        return await _peripheralsBLL.CreateTypeAsync(new PeripheralType()
        {
            Name = modelInstance.ModelName,
            ManufacturerID = vendor.ID,
            ProductCatalogTypeID = pcType.IMObjID,
            CanBuy = true
        }, protocolLogger, cancellationToken);
    }

    private async Task<TerminalDeviceModel> CreateTerminalDeviceType(ITAssetImportParsedDetails modelInstance, IProtocolLogger protocolLogger, string endString, CancellationToken cancellationToken)
    {
        var vendor = await GetVendorAsync(modelInstance.VendorExternalID, modelInstance.VendorName, cancellationToken);
        if (vendor == null)
            _errorObjects[modelInstance].Add($"Не найдено поле {nameof(modelInstance.VendorName)} {endString}");

        var pcType = await GetProductCatalogTypeAsync(modelInstance.TypeExternalID, modelInstance.TypeName, cancellationToken);
        if (pcType == null)
            _errorObjects[modelInstance].Add($"Не найдено поле {nameof(modelInstance.TypeName)} {endString}");

        return await _terminalDevicesBLL.CreateTypeAsync(new TerminalDeviceModel()
        {
            Name = modelInstance.ModelName,
            ManufacturerID = vendor.ID,
            ProductCatalogTypeID = pcType.IMObjID,
            CanBuy = true
        }, protocolLogger, cancellationToken);
    }

    private bool UniqControl(string? importValue, ITAssetImportParsedDetails modelInstance, string valueName, string[] values, string? value, string endString)
    {
        if (string.IsNullOrEmpty(importValue))
            return false;

        if (values.Where(x => x.Contains(importValue)).Count() == 1 && value == importValue)
            return false;

        if (_isControlUniqSerialNumberAndInvNumber && (valueName == _invNumberString || valueName == _serialNumberString))
            return false;

        if (values.Contains(importValue))
        {
            _errorObjects[modelInstance].Add($"Для поля {valueName} не пройден контроль уникальности {endString}");

            var undisposed = _mapper.Map<ITAssetUndisposed>(modelInstance);
            undisposed.ReasonCode = _undisposedReasonValues.GetValueOrDefault(valueName);
            _iTAssetUndisposed.Add(undisposed);

            return true;
        }
        else
            return false;
    }

    private async Task<Manufacturer?> GetVendorAsync(string? externalID, string? name, CancellationToken cancellationToken)
        => await _manufacturerRepository.FirstOrDefaultAsync(x => x.ExternalID == externalID || x.Name == name, cancellationToken);

    private async Task<Organization?> GetOrganizationAsync(string? externalID, string? name, CancellationToken cancellationToken)
        => await _organizationRepository.FirstOrDefaultAsync(x => x.ExternalId == externalID || x.Name == name, cancellationToken);

    private async Task<Supplier?> GetSupplierAsync(string? externalID, string? name, CancellationToken cancellationToken)
        => await _supplierRepository.FirstOrDefaultAsync(x => x.ExternalID == externalID || x.Name == name, cancellationToken);

    private async Task<NetworkDevice?> GetActiveEquipmentAsync(string? externalID, string? name, CancellationToken cancellationToken)
        => await _networkDeviceRepository.FirstOrDefaultAsync(x => x.ExternalID == externalID || x.Name == name, cancellationToken);

    private async Task<TerminalDevice?> GetTerminalEquipmentAsync(string? externalID, string? name, CancellationToken cancellationToken)
        => await _terminalDeviceRepository.FirstOrDefaultAsync(x => x.ExternalID == externalID || x.Name == name, cancellationToken);

    private async Task<Rack?> GetRackAsync(string? externalID, string? name, CancellationToken cancellationToken)
        => await _rackRepository.FirstOrDefaultAsync(x => x.ExternalID == externalID || x.Name == name, cancellationToken);

    private async Task<Workplace?> GetWorkplaceAsync(string? externalID, string? name, CancellationToken cancellationToken)
        => await _workplaceRepository.FirstOrDefaultAsync(x => x.ExternalID == externalID || x.Name == name, cancellationToken);

    private async Task<Room?> GetRoomAsync(string? externalID, string? name, CancellationToken cancellationToken)
        => await _roomRepository.FirstOrDefaultAsync(x => x.ExternalID == externalID || x.Name == name, cancellationToken);

    private async Task<ProductCatalogType?> GetProductCatalogTypeAsync(string? externalID, string? name, CancellationToken cancellationToken)
        => await _pcTypeRepository.FirstOrDefaultAsync(x => x.ExternalID == externalID || x.Name == name, cancellationToken);

    private void PrintAllErrors(IProtocolLogger protocolLogger)
    {
        int errorCount = 0;
        foreach (var obj in _errorObjects)
            if (obj.Value.Count > 0)
            {
                foreach (var error in obj.Value)
                    protocolLogger.Information(error);

                errorCount++;
            }

        protocolLogger.Information($"Объектов импорта ит-активов с ошибками: {errorCount}");
        _errorObjects.Clear();
    }

    private async Task CreateSlotsAsync(Guid objectID, Guid modelID, IProtocolLogger protocolLogger, CancellationToken cancellationToken)
    {
        List<SlotEntity> slotList = new();
        foreach (var slotByTemplate in await _slotBLL.GetSlotTemplateAsync(modelID, cancellationToken))
            slotList.Add(new SlotEntity(objectID, (int)ObjectClass.Adapter, slotByTemplate.Number, slotByTemplate.SlotTypeID));

        await _slotBLL.CreateAsync(slotList.ToArray(), protocolLogger, cancellationToken);
        _slotCreatedCount += slotList.Count;
    }

    private async Task CreatePortsAsync(NetworkDevice obj, Guid modelID, IProtocolLogger protocolLogger, CancellationToken cancellationToken)
    {
        List<ActivePort> portList = new();
        foreach (var portByTemplate in await _portBLL.GetPortTemplateAsync(modelID, cancellationToken))
            portList.Add(new ActivePort(obj.ID, obj.IMObjID, portByTemplate.PortNumber, portByTemplate.JackTypeID, portByTemplate.TechnologyID));

        await _portBLL.CreateAsync(portList.ToArray(), protocolLogger, cancellationToken);
        _portCreatedCount += portList.Count;
    }
}
