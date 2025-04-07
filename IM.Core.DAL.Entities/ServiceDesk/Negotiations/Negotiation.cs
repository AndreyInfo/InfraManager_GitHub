using Inframanager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk.Negotiations
{
    [ObjectClassMapping(ObjectClass.Negotiation)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.None)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
    public abstract class Negotiation : IGloballyIdentifiedEntity
    {
        public const int NameMaxLength = 500;

        protected Negotiation()
        {
        }

        public Negotiation(Guid objectId, ObjectClass objectClassId)
        {
            IMObjID = Guid.NewGuid();
            ObjectID = objectId;
            ObjectClassID = objectClassId;
            Status = NegotiationStatus.Created;
            NegotiationUsers = new HashSet<NegotiationUser>();
        }

        public Guid IMObjID { get; }
        public Guid ObjectID { get; }
        public ObjectClass ObjectClassID { get; }
        public string Name { get; set; }
        public NegotiationMode Mode { get; set; }
        public NegotiationStatus Status { get; set; }
        public DateTime? UtcDateVoteStart { get; set; }
        public void Start()
        {
            Status = NegotiationStatus.Voting;
            UtcDateVoteStart = DateTime.UtcNow;
        }

        public DateTime? UtcDateVoteEnd { get; set; }
        public byte[] RowVersion { get; set; }
        public virtual ICollection<NegotiationUser> NegotiationUsers { get; }

        public NegotiationUser AddUser(User user)
        {
            var negotiationUser = new NegotiationUser(IMObjID, user);
            NegotiationUsers.Add(negotiationUser);

            return negotiationUser;
        }

        public bool IsNotStarted => Status == NegotiationStatus.Created;
        public bool IsFinished => (!NotFinished).IsSatisfiedBy(this);
        
        public static Specification<Negotiation> NotFinished =>
            new Specification<Negotiation>(negotiation => negotiation.Status == NegotiationStatus.Created || negotiation.Status == NegotiationStatus.Voting);

        public static Specification<Negotiation> UserIsNegotiator(Guid userID) =>
            new Specification<Negotiation>(negotiation => negotiation.NegotiationUsers.Any(u => u.UserID == userID));

        public static Specification<Negotiation> AnyNegotiator(Specification<IEnumerable<User>> spec) =>
            spec.Convert<Negotiation>(negotiation => negotiation.NegotiationUsers.Select(x => x.User));
    }
}
