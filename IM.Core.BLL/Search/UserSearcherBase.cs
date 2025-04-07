using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Search;
using InfraManager.DAL.Settings;
using System;
using System.Linq;

namespace InfraManager.BLL.Search
{
    internal abstract class UserSearcherBase : JsonCriteriaObjectSearcher<UserSearchClientCriteria>
    {
        private readonly IUserSearchQuery _query;
        private readonly IFinder<Setting> _settingsFinder;
        private readonly IConvertSettingValue<bool> _converter;

        public UserSearcherBase(
            IUserSearchQuery query,
            IFinder<Setting> settingsFinder, 
            IConvertSettingValue<int> valueConverter,
            IConvertSettingValue<bool> boolConverter,
            IPagingQueryCreator paging,
            ICurrentUser currentUser) 
            : base(settingsFinder, valueConverter, paging, currentUser)
        {
            _query = query;
            _settingsFinder = settingsFinder;
            _converter = boolConverter;
        }

        protected override IQueryable<ObjectSearchResult> Query(Guid userId, UserSearchClientCriteria searchBy)
        {
            return _query.Query(
                new UserSearchCriteria
                {
                    UserId = searchBy.UserId,
                    QueueId = searchBy.QueueId,
                    HasAnyNonAdministrativeRole = HasAnyNonAdministrativeRole,
                    Operations = searchBy.OperationIds ?? OperationIds,
                    Text = searchBy.Text,
                    NoTOZ = searchBy.NoTOZ ?? NoTOZ,
                    ExceptUserIDs = searchBy.ExceptUserIDs ?? ExceptUserIDs,
                    SubdivisionID = searchBy.SubdivisionID,
                    MOL = searchBy.MOL ?? false,
                    OrganizationID = searchBy.OrganizationID,
                    ControlsObjectID = searchBy.ControlsObjectID,
                    ControlsObjectClassID = searchBy.ControlsObjectClassID,
                    ControlsObjectValue = searchBy.ControlsObjectValue,
                    UseTTZ = UseTTZ,
                },
                userId);
        }

        protected virtual bool UseTTZ
        {
            get
            {
                var setting = _settingsFinder.Find(SystemSettings.UseTTZ);
                return _converter.Convert(setting.Value);
            }
        }

        protected abstract bool HasAnyNonAdministrativeRole { get; }
        protected abstract OperationID[] OperationIds { get; }
        protected abstract bool NoTOZ { get; }
        protected abstract Guid[] ExceptUserIDs { get; }
    }
}
