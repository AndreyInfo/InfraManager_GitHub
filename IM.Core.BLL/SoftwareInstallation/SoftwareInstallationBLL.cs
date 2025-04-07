using AutoMapper;
using IM.Core.WebApi.Contracts.Common.Models;
using Inframanager.BLL;
using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.Events;
using InfraManager.BLL.FieldEdit;
using InfraManager.BLL.Localization;
using InfraManager.CrossPlatform.WebApi.Contracts;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Attributes;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;
using InfraManager.CrossPlatform.WebApi.Contracts.SoftwareInstallation;
using InfraManager.CrossPlatform.WebApi.Contracts.SoftwareInstallation.Models;
using InfraManager.DAL;
using InfraManager.DAL.Settings;
using InfraManager.DAL.Software;
using InfraManager.DAL.Software.Installation;
using InfraManager.ResourcesArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ISoftwareInstallationDataProviderDAL = InfraManager.DAL.Software.ISoftwareInstallationDataProvider;

namespace InfraManager.BLL.Software
{
    [ObjectClassMapping(ObjectClass.SoftwareInstallation)]
    /// <inheritdoc cref="ISoftwareInstallationBLL" />
    internal class SoftwareInstallationBLL : ISoftwareInstallationBLL, IEntityEditor, ISelfRegisteredService<ISoftwareInstallationBLL>
    {
        /// <summary>
        /// Автомаппер
        /// </summary>
        private readonly IMapper _mapper;
        private readonly ISoftwareInstallationDataProviderDAL _installationDataProviderDAL;
        private readonly IEventBuilder _eventMaker;
        private readonly IEventBLL _eventDataProvider;
        private readonly IServiceMapper<ObjectClass, IFindNameByGlobalID> _nameFinderByGuidResolver;
        private readonly IServiceMapper<ObjectClass, IFindName> _nameFinderResolver;
        private readonly ISoftwareModelDataProvider _softwareModelDataProvider;
        private readonly IFieldManager _fieldSetter;
        private readonly ISoftwareLicenceReferencesDataProvider _softwareLicenceReferencesDataProvider;
        private readonly IServiceMapper<ObjectClass, ISoftwareInstalationListQuery> _listQueryMapper;
        private readonly ICustomFilterListQuery _customFilterListQuery;
        private readonly IPagingQueryCreator _pagingQueryCreator;
        private readonly IFinder<WebFilter> _webFilderFinder;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly ICurrentUser _currentUser;
        private readonly IUserAccessBLL _userAccessBLL;
        private readonly ILocalizeText _localizeText;

        /// <summary>
        /// Инициализирует экземпляр <see cref="SoftwareInstallationBLL"/>.
        /// </summary>       
        /// <param name="mapper"> автомаппер </param>
        /// <param name="builderQueryListSoftwareInstallationFactory">  Фабрика постороения запроса, для получения списка инсталляций </param>
        public SoftwareInstallationBLL(
            IMapper mapper,
            IServiceMapper<ObjectClass, ISoftwareInstalationListQuery> listQueryMapper,
            ICustomFilterListQuery customFilterListQuery,
            ISoftwareInstallationDataProviderDAL installationDataProviderDAL,
            IEventBuilder eventMaker,
            IEventBLL eventDataProvider,
            IServiceMapper<ObjectClass, IFindNameByGlobalID> nameFinderByGuidResolver,
            IServiceMapper<ObjectClass, IFindName> nameFinderResolver,
            IFieldManager fieldManager,
            ISoftwareModelDataProvider softwareModelDataProvider,
            ISoftwareLicenceReferencesDataProvider softwareLicenceReferencesDataProvider,
            IPagingQueryCreator pagingQueryCreator,
            IFinder<WebFilter> webFilterFinder,
            IUnitOfWork saveChangesCommand,
            ICurrentUser currentUser,
            IUserAccessBLL userAccessBLL,
            ILocalizeText localizeText)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _installationDataProviderDAL = installationDataProviderDAL ?? throw new ArgumentNullException(nameof(installationDataProviderDAL));
            _eventMaker = eventMaker ?? throw new ArgumentNullException(nameof(eventMaker));
            _eventDataProvider = eventDataProvider ?? throw new ArgumentNullException(nameof(eventDataProvider));
            _nameFinderResolver = nameFinderResolver ?? throw new ArgumentNullException(nameof(nameFinderResolver));
            _nameFinderByGuidResolver = nameFinderByGuidResolver ?? throw new ArgumentNullException(nameof(nameFinderByGuidResolver));
            _softwareModelDataProvider = softwareModelDataProvider ?? throw new ArgumentNullException(nameof(softwareModelDataProvider));
            _fieldSetter = fieldManager ?? throw new ArgumentNullException(nameof(fieldManager));
            _softwareLicenceReferencesDataProvider = softwareLicenceReferencesDataProvider ?? throw new ArgumentNullException(nameof(softwareLicenceReferencesDataProvider));
            _listQueryMapper = listQueryMapper ?? throw new ArgumentNullException(nameof(listQueryMapper));
            _customFilterListQuery = customFilterListQuery ?? throw new ArgumentNullException(nameof(customFilterListQuery));
            _pagingQueryCreator = pagingQueryCreator ?? throw new ArgumentNullException(nameof(pagingQueryCreator));
            _webFilderFinder = webFilterFinder ?? throw new ArgumentNullException(nameof(webFilterFinder));
            _saveChangesCommand = saveChangesCommand;
            _currentUser = currentUser;
            _userAccessBLL = userAccessBLL;
            _localizeText = localizeText;
        }

        /// <inheritdoc />
        public async Task<SoftwareInstallationListItem[]> GetListAsync(SoftwareInstallationListFilter filter, CancellationToken cancellationToken = default)
        {
            if (!await _userAccessBLL.UserHasOperationAsync(_currentUser.UserId, OperationID.SoftwareInstallation_List, cancellationToken))
                throw new AccessDeniedException("Access denied");

            IQueryable<ViewSoftwareInstallation> query = null;

            var objectClass = filter.ParentClassID ?? filter.TreeSettings?.FiltrationObjectClassID;

            if (objectClass.HasValue && _listQueryMapper.HasKey(objectClass.Value))
                query = await _listQueryMapper.Map(objectClass.Value).QueryAsync(filter, cancellationToken);
            else if (filter.CurrentFilterID.HasValue)
                query = await GetCustomFilterQuery(filter.CurrentFilterID);
            else
                query = await _installationDataProviderDAL.GetViewListAsync(cancellationToken);

            if (query == null)
                throw new InvalidObjectException(string.Format(_localizeText.Localize(nameof(Resources.SoftwareInstallationSearchError)), ObjectClass.SoftwareInstallation));

            if (!string.IsNullOrEmpty(filter.SoftwareModelName))
                query = query.Where(x => x.SoftwareModelName.ToLower() == filter.SoftwareModelName.ToLower());

            if (!string.IsNullOrEmpty(filter.CommercialModelName))
                query = query.Where(x => x.CommercialModelName.ToLower() == filter.CommercialModelName.ToLower());

            if (!string.IsNullOrEmpty(filter.DeviceName))
                query = query.Where(x => x.DeviceName.ToLower() == filter.DeviceName.ToLower());

            if (!string.IsNullOrEmpty(filter.InstallPath))
                query = query.Where(x => x.InstallPath.ToLower() == filter.InstallPath.ToLower());

            if (filter.InstallDate.HasValue)
                query = query.Where(x => x.InstallDate == filter.InstallDate.Value);

            if (filter.DateLastSurvey.HasValue)
                query = query.Where(x => x.UtcDateLastDetected == filter.DateLastSurvey.Value);

            // TODO: поле DateLastRightsCheck пока не реализовано
            //if (filter.DateLastRightsCheck.HasValue)
            //    query = query.Where(x => x.DateLastRightsCheck == filter.DateLastRightsCheck.Value);

            if (filter.CreateDate.HasValue)
                query = query.Where(x => x.UtcDateCreated == filter.CreateDate.Value);

            if (filter.Status.HasValue)
                query = query.Where(x => x.State == filter.Status.Value);

            var pagingQuery = _pagingQueryCreator.Create(query.OrderBy(x => x.Id));

            var data = await pagingQuery.PageAsync(filter.StartRecordIndex, filter.CountRecords, cancellationToken);

            return _mapper.Map<SoftwareInstallationListItem[]>(data);
        }

        private async Task<IQueryable<ViewSoftwareInstallation>> GetCustomFilterQuery(Guid? filterId)
        {
            FilterElementData[] filterElements;

            if (filterId.HasValue)
            {
                var webFilter = await _webFilderFinder.FindAsync(new object[] { filterId.Value });
                filterElements = webFilter.Elements.Select(x => x.Parse()).ToArray();
            }
            else
                filterElements = new FilterElementData[0];

            return _customFilterListQuery.Query<ViewSoftwareInstallation>(filterElements);
        }

        public bool CanHandle(ObjectClassModel objectClass)
        {
            return objectClass.ObjClassID == Constants.Installation;
        }

        public async Task<BaseResult<SetFieldResult, BaseError>> HandleAsync(SetFieldRequest model, CancellationToken cancellationToken)
        {
            var dbItem = await _installationDataProviderDAL.GetAsync(model.ID, cancellationToken);

            var changeItem = _mapper.Map<SoftwareInstallationItem>(dbItem);

            var setResult = _fieldSetter.SetFieldValue(changeItem, model.FieldValue);
            if (setResult != null)
                return new BaseResult<SetFieldResult, BaseError>(null, setResult);

            var result = await SaveInternal(changeItem, dbItem, cancellationToken);
            if (result.Success)
            {
                await _saveChangesCommand.SaveAsync();
                return new BaseResult<SetFieldResult, BaseError>(new SetFieldResult(model) { CurrentObjectValue = _fieldSetter.GetFieldValue(changeItem, model.FieldValue.Field) }, null);
            }

            return new BaseResult<SetFieldResult, BaseError>(null, BaseError.BadParamsError);
        }

        public async Task<BaseResult<SoftwareInstallationItem, BaseError>> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var installationItem = await _installationDataProviderDAL.GetAsync(id, cancellationToken);

            if (installationItem != null)
            {
                var instViewQuery = await GetCustomFilterQuery(null);
                var instViewItem = instViewQuery.Where(x => x.Id == id).FirstOrDefault();
                return new BaseResult<SoftwareInstallationItem, BaseError>(
                    new SoftwareInstallationItem()
                    {
                        CommercialModelName = installationItem.SoftwareModel.CommercialModel?.Name,
                        CreateDate = installationItem.UtcDateCreated.ToString("dd.MM.yyyy"),
                        DateLastSurvey = installationItem.UtcDateLastDetected?.ToString("dd.MM.yyyy HH:mm"),
                        DeviceID = installationItem.DeviceID,
                        DeviceClassID = (int)installationItem.DeviceClassID,
                        DeviceName = instViewItem.DeviceName,
                        ID = installationItem.ID,
                        InstallDate = installationItem.InstallDate?.ToString("dd.MM.yyyy"),
                        InstallPath = installationItem.InstallPath,
                        SoftwareInstallationName = installationItem.UniqueNumber,
                        SoftwareModelID = installationItem.SoftwareModelID,
                        SoftwareModelName = installationItem.SoftwareModel?.Name,
                        Status = installationItem.State,
                    }, null);
            }

            return new BaseResult<SoftwareInstallationItem, BaseError>(null, BaseError.ObjectDeleted);
        }

        public async Task<BaseResult<Guid, SoftwareInstallationRules>> SaveAsync(SoftwareInstallationItem item, CancellationToken cancellationToken)
        {
            var sameItem = await _installationDataProviderDAL.GetAsync(item.ID, cancellationToken);
            var result = await SaveInternal(item, sameItem, cancellationToken);
            await _saveChangesCommand.SaveAsync();
            return result;
        }

        private async Task<BaseResult<Guid, SoftwareInstallationRules>> SaveInternal(SoftwareInstallationItem valueFromRequest,
            DAL.Software.SoftwareInstallation valueFromDB = null,
            CancellationToken cancellationToken = default)
        {
            var valid = Validate(valueFromRequest);
            if (valid != null)
                return new BaseResult<Guid, SoftwareInstallationRules>(Guid.Empty, valid);

            bool isNew = valueFromDB == null;
            SoftwareInstallationItem oldValue = null;
            if (isNew)
            {
                if (valueFromRequest.ID == Guid.Empty)
                    valueFromRequest.ID = Guid.NewGuid();
                valueFromDB = _mapper.Map<DAL.Software.SoftwareInstallation>(valueFromRequest);
                valueFromDB.UtcDateCreated = DateTime.UtcNow;
                valueFromDB.RegistryID = string.Empty;
            }
            else
            {
                oldValue = _mapper.Map<SoftwareInstallationItem>(valueFromDB);
                valid = Validate(oldValue);
                if (valid != null)
                    return new BaseResult<Guid, SoftwareInstallationRules>(Guid.Empty, valid);
                _mapper.Map(valueFromRequest, valueFromDB, typeof(SoftwareInstallationItem), typeof(DAL.Software.SoftwareInstallation));
            }
            if (!string.IsNullOrEmpty(valueFromRequest.SoftwareInstallationName))
            {
                var check = await _installationDataProviderDAL.GetByNameAsync(valueFromRequest.SoftwareInstallationName, cancellationToken);
                if (isNew && check != null)
                    return new BaseResult<Guid, SoftwareInstallationRules>(Guid.Empty, SoftwareInstallationRules.UniqueName);
                if (!isNew && !(check == null || check.ID == valueFromRequest.ID))
                    return new BaseResult<Guid, SoftwareInstallationRules>(Guid.Empty, SoftwareInstallationRules.UniqueName);
            }

            if (isNew)
            {
                _installationDataProviderDAL.Add(valueFromDB);
            }
            var eventResult = await _eventMaker.CreateEvent(oldValue, valueFromRequest, nameResolveFactory);
            if (!eventResult.Success)
                return new BaseResult<Guid, SoftwareInstallationRules>(Guid.Empty, SoftwareInstallationRules.EventFault);
            _eventDataProvider.AddEvent(eventResult.Result);

            return new BaseResult<Guid, SoftwareInstallationRules>(valueFromDB.ID, null);
        }

        private Func<object, Task<string>> nameResolveFactory(FieldCompareAttribute attribute, string fieldName)
        {
            if (fieldName == nameof(SoftwareInstallationItem.DeviceID))
                return deviceNameResolver;
            if (fieldName == nameof(SoftwareInstallationItem.SoftwareModelID))
                return modelNameResolver;

            return null;
        }

        private async Task<string> deviceNameResolver(object arg)
        {
            SoftwareInstallationItem item = arg as SoftwareInstallationItem;
            if (item != null)
            {
                var nameFinderByGuid = _nameFinderByGuidResolver.Map((ObjectClass)item.DeviceClassID);
                return await nameFinderByGuid.FindAsync(item.DeviceID);
            }
            return null;
        }

        private async Task<string> modelNameResolver(object arg)
        {
            SoftwareInstallationItem item = arg as SoftwareInstallationItem;
            if (item != null)
            {
                var model = await _softwareModelDataProvider.GetAsync(item.SoftwareModelID);
                return model?.Name;
            }
            return null;
        }

        private SoftwareInstallationRules? Validate(SoftwareInstallationItem item)
        {
            if (item.SoftwareModelID == Guid.Empty)
                return SoftwareInstallationRules.ModelObligatory;
            if (item.DeviceID == Guid.Empty)
                return SoftwareInstallationRules.DeviceObligatory;
            return null;
        }

        public async Task<BaseResult<bool, BaseError>> DeleteDependantAsync(Guid installationID, GuidList list, CancellationToken cancellationToken)
        {
            var installs = await _installationDataProviderDAL.GetDependancesAsync(installationID, cancellationToken);
            installs.Where(x => list.IDList.Contains(x.DependantInstallationId)).ToList()
            .ForEach(d => { _installationDataProviderDAL.RemoveDependant(d); });
            await _saveChangesCommand.SaveAsync();
            return new BaseResult<bool, BaseError>(true, null);
        }

        public async Task<BaseResult<bool, BaseError>> AddDependantAsync(Guid installationID, GuidList list, CancellationToken cancellationToken)
        {

            var installs = await _installationDataProviderDAL.GetDependancesAsync(installationID, cancellationToken);
            list.IDList.Where(x => !installs.Any(y => y.DependantInstallationId == x)).ToList()
            .ForEach(d => { _installationDataProviderDAL.AddDependant(new DAL.Software.SoftwareInstallationDependances() { DependantInstallationId = d, InstallationId = installationID }); });
            await _saveChangesCommand.SaveAsync();
            return new BaseResult<bool, BaseError>(true, null);

        }

        public async Task<BaseResult<List<SoftwareLicenceUseListItem>, BaseError>> GetLicenceUseAsync(Guid id, CancellationToken cancellationToken)
        {

            var inst = await _installationDataProviderDAL.GetAsync(id, cancellationToken);
            var refs = await _softwareLicenceReferencesDataProvider.GetListForObjectAsync((int)inst.DeviceClassID, inst.DeviceID, cancellationToken);
            List<SoftwareLicenceUseListItem> res = new List<SoftwareLicenceUseListItem>();
            foreach (var r in refs)
            {
                var nameFinder = _nameFinderResolver.Map((ObjectClass)r.ClassId);
                var dev = await nameFinder.FindAsync(new object[] { r.ObjectId });
                res.Add(new SoftwareLicenceUseListItem() { ObjectName = dev, SoftwareLicenceName = r.SoftwareLicence.Name, RightCount = r.SoftwareExecutionCount, ID = r.SoftwareLicenceId });
            }
            await _saveChangesCommand.SaveAsync();
            return new BaseResult<List<SoftwareLicenceUseListItem>, BaseError>(res, null);

        }
    }
}
