using Inframanager.BLL;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL;
using System;

namespace InfraManager.BLL.OrganizationStructure
{
    internal class DeputyUserQueryBuilder :
        IBuildEntityQuery<DeputyUser, DeputyUserDetails, DeputyUserListFilter>,
        ISelfRegisteredService<IBuildEntityQuery<DeputyUser, DeputyUserDetails, DeputyUserListFilter>>
    {

        private readonly IReadonlyRepository<DeputyUser> _repository;

        public DeputyUserQueryBuilder(IReadonlyRepository<DeputyUser> repository)
        {
            _repository = repository;
        }

        public IExecutableQuery<DeputyUser> Query(DeputyUserListFilter filter)
        {
            var query = _repository
                .With(x => x.Child)
                .With(x => x.Parent)
                .Query();

            if(!filter.ShowFinished)
            {
                query = query.Where(x => x.UtcDataDeputyBy > DateTime.UtcNow);
            }

            if (filter.Active)
            {
                query = query.Where(DeputyUser.IsActive);
            }

            if(filter.DeputyMode == DeputyMode.Deputy)
            {
                query = query.Where(x => x.ParentUserId == filter.UserID);
            }
            else
            {
                query = query.Where(x => x.ChildUserId == filter.UserID);
            }

            return query;
        }
    }
}
