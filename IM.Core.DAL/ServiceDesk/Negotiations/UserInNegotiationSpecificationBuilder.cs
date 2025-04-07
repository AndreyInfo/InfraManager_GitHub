using Inframanager;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk.Negotiations
{
    internal class UserInNegotiationSpecificationBuilder<T> : IBuildUserInNegotiationSpecification<T>
        where T : class, IGloballyIdentifiedEntity
    {
        private readonly DbSet<Negotiation> _negotiations;
        private readonly DbSet<DeputyUser> _deputyUsers;

        public UserInNegotiationSpecificationBuilder(
            DbSet<Negotiation> negotiations,
            DbSet<DeputyUser> deputyUsers)
        {
            _negotiations = negotiations;
            _deputyUsers = deputyUsers;
        }

        public Specification<T> Build(User user)
        {
            var classID = typeof(T).GetObjectClassOrRaiseError();
            var userIsDeputy = DeputyUser.UserIsDeputy(user.IMObjID);
            var userIsDeputyOfNegotiator = new Specification<Negotiation>(
                n => n.NegotiationUsers.Any(
                    x => _deputyUsers
                        .Where(userIsDeputy)                        
                        .Any(dp => dp.ParentUserId == x.UserID)));

            return new Specification<T>(
                entity =>
                    _negotiations
                        .Where(
                            negotiation => negotiation.ObjectClassID == classID
                                && negotiation.ObjectID == entity.IMObjID)
                        .Any(Negotiation.UserIsNegotiator(user.IMObjID)
                                || userIsDeputyOfNegotiator));
        }
    }
}
