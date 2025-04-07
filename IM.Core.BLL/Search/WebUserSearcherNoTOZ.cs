using InfraManager.BLL.Settings;
using InfraManager.DAL.Search;
using InfraManager.DAL;
using System;
using InfraManager.DAL.Settings;

namespace InfraManager.BLL.Search
{
    internal class WebUserSearcherNoTOZ : UserSearcherBase
    {
        protected override bool HasAnyNonAdministrativeRole => false;

        protected override OperationID[] OperationIds => Array.Empty<OperationID>();

        protected override bool NoTOZ => true;

        protected override Guid[] ExceptUserIDs => Array.Empty<Guid>();

        public WebUserSearcherNoTOZ(
            IUserSearchQuery query,
            IFinder<Setting> settingsFinder,
            IConvertSettingValue<int> valueConverter,
            IConvertSettingValue<bool> boolConverter,
            IPagingQueryCreator paging,
            ICurrentUser currentUser)
            : base(query, settingsFinder, valueConverter, boolConverter, paging, currentUser)
        {
        }
    }
}
