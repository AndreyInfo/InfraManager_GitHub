using InfraManager.BLL.Settings;
using InfraManager.DAL.Search;
using InfraManager.DAL;
using System;
using InfraManager.DAL.Settings;
using System.Threading.Tasks;
using System.Threading;

namespace InfraManager.BLL.Search
{
    internal class UserSearcherForFormParameter : UserSearcherBase //TODO: выпилить (а если я заиспользовать захочу не For form parameters - мне переименовывать или у нас BL уже с PL местами поменялись)
    {
        private  int LimitUsers = int.MaxValue;

        protected override bool HasAnyNonAdministrativeRole => false;

        protected override OperationID[] OperationIds => Array.Empty<OperationID>();

        protected override bool NoTOZ => true;

        protected override Guid[] ExceptUserIDs => Array.Empty<Guid>();

        public UserSearcherForFormParameter(
            IUserSearchQuery query,
            IFinder<Setting> settingsFinder,
            IConvertSettingValue<int> valueConverter,
            IConvertSettingValue<bool> boolConverter,
            IPagingQueryCreator paging,
            ICurrentUser currentUser)
            : base(query, settingsFinder, valueConverter, boolConverter, paging, currentUser)
        {
        }

        protected override Task<int> TakeLimitUsersAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(LimitUsers);
    }
}
