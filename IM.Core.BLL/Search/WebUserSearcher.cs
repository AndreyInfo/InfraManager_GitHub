using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Search;
using InfraManager.DAL.Settings;
using System;

namespace InfraManager.BLL.Search
{
    internal class WebUserSearcher : UserSearcherBase
    {
        public WebUserSearcher(
            IUserSearchQuery query, 
            IFinder<Setting> settingsFinder, 
            IConvertSettingValue<int> valueConverter,
            IConvertSettingValue<bool> boolConverter,
            IPagingQueryCreator paging, 
            ICurrentUser currentUser) 
            : base(query, settingsFinder, valueConverter, boolConverter, paging, currentUser)
        {
        }

        protected override bool HasAnyNonAdministrativeRole => false;

        protected override OperationID[] OperationIds => Array.Empty<OperationID>();

        protected override bool NoTOZ => false;

        protected override Guid[] ExceptUserIDs => Array.Empty<Guid>();
    }
}
