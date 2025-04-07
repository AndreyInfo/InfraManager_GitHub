using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Search;
using InfraManager.DAL.Settings;
using System;

namespace InfraManager.BLL.Search
{
    internal class ExecutorUserSearcher : UserSearcherBase
    {
        public static OperationID[] Operations =>
            new[]
            {
                OperationID.SD_General_Administrator,
                OperationID.SD_General_Executor
            };

        public ExecutorUserSearcher(
            IUserSearchQuery query,
            IFinder<Setting> settingsFinder,
            IConvertSettingValue<int> valueConverter,
            IConvertSettingValue<bool> boolConverter,
            IPagingQueryCreator paging,
            ICurrentUser currentUser)
            : base(query, settingsFinder, valueConverter, boolConverter, paging, currentUser)
        {
        }

        protected override bool HasAnyNonAdministrativeRole => true;

        protected override OperationID[] OperationIds => Operations;

        protected override bool NoTOZ => false;

        protected override Guid[] ExceptUserIDs => Array.Empty<Guid>();
    }
}
