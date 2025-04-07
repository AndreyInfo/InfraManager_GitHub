using AutoMapper;
using Inframanager;
using InfraManager.BLL.ServiceDesk.MassIncidents;
using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Settings;
using System;
using System.Linq;

namespace InfraManager.BLL.Search
{
    /// <summary>
    /// TODO: Поиск пользователя должен делаться по запросу на api/users?massIncidentOwner=true&hasNonAdminRole=true, а не через
    /// сбоку приделанный search
    /// </summary>
    internal class MassIncidentOwnerSearcher : JsonCriteriaObjectSearcher<SearchCriteria>
    {
        private readonly IReadonlyRepository<User> _users;
        private readonly IBuildSearchSpecification<User> _userSearchSpecBuilder;
        private readonly Specification<User> _canBeMassIncidentOwner;
        private readonly IMapper _mapper;

        public MassIncidentOwnerSearcher(
            IReadonlyRepository<User> users,
            IBuildSearchSpecification<User> userSearchSpecBuilder,
            MassIncidentOwnerSpecification canBeMassIncidentOwner,
            IMapper mapper,
            IFinder<Setting> settingsFinder, 
            IConvertSettingValue<int> valueConverter, 
            IPagingQueryCreator paging, 
            ICurrentUser currentUser) 
            : base(settingsFinder, valueConverter, paging, currentUser)
        {
            _users = users;
            _userSearchSpecBuilder = userSearchSpecBuilder;
            _canBeMassIncidentOwner = canBeMassIncidentOwner;
            _mapper = mapper;
        }

        protected override IQueryable<ObjectSearchResult> Query(Guid userID, SearchCriteria searchBy)
        {
            var where = User.ExceptSystemUsers && User.HasNonAdminRole && _canBeMassIncidentOwner;

            if (!string.IsNullOrWhiteSpace(searchBy.Text))
            {
                where = where && _userSearchSpecBuilder.Build(searchBy.Text);
            }

            return _mapper.ProjectTo<ObjectSearchResult>(_users.Query().Where(where));
        }
    }
}
