using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    internal class NegotiationQueryBuilder : 
        IBuildEntityQuery<Negotiation, NegotiationDetails, NegotiationListFilter>,
        ISelfRegisteredService<IBuildEntityQuery<Negotiation, NegotiationDetails, NegotiationListFilter>>
    {
        private readonly IReadonlyRepository<Negotiation> _repository;

        public NegotiationQueryBuilder(IReadonlyRepository<Negotiation> repository)
        {
            _repository = repository;
        }

        public IExecutableQuery<Negotiation> Query(NegotiationListFilter filterBy)
        {
            var query = _repository
                .WithMany(x => x.NegotiationUsers)
                    .ThenWith(x => x.User)
                        .ThenWith(x => x.Subdivision)
                .WithMany(x => x.NegotiationUsers)
                    .ThenWith(x => x.User)
                        .ThenWith(x => x.Position)
                .Query();

            if (filterBy.Parent.HasValue)
            {
                query = query
                    .Where(
                        x => x.ObjectID == filterBy.Parent.Value.Id
                            && x.ObjectClassID == filterBy.Parent.Value.ClassId);
            }

            if (filterBy.IDList != null && filterBy.IDList.Any())
            {
                query = query.Where(x => filterBy.IDList.Contains(x.IMObjID));
            }

            if (filterBy.UserID.HasValue)
            {
                query = query.Where(x => x.NegotiationUsers.Any(user => user.UserID == filterBy.UserID));
            }

            return query;
        }
    }
}
