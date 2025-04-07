using Inframanager;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfraManager.DAL.OrganizationStructure
{
    [ObjectClassMapping(ObjectClass.Substitution)]
    public class DeputyUser : IMarkableForDelete, IGloballyIdentifiedEntity
    {
        public Guid IMObjID { get; init; }
        public Guid ParentUserId { get; init; }
        public virtual User Parent { get; }
        public Guid ChildUserId { get; init; }
        public virtual User Child { get; }
        public DateTime UtcDataDeputyWith { get; init; }
        public DateTime UtcDataDeputyBy { get; init; }
        public bool Removed { get; private set; }

        public void MarkForDelete()
        {
            Removed = true;
        }

        public static Specification<DeputyUser> IsActive =>
            new Specification<DeputyUser>(
                x => x.UtcDataDeputyWith <= DateTime.UtcNow 
                    && x.UtcDataDeputyBy > DateTime.UtcNow)
            && IsAlive;

        public static Specification<DeputyUser> IsAlive => new Specification<DeputyUser>(x => !x.Removed);

        public static Specification<DeputyUser> UserIsDeputy(Guid userID, Guid deputyID) =>
            new Specification<DeputyUser>(x => x.ParentUserId == userID)
                && UserIsDeputy(deputyID);
        public static Specification<DeputyUser> UserIsDeputy(Guid deputyID) =>
            new Specification<DeputyUser>(x => x.ChildUserId == deputyID)
                && IsActive;
    }
}
