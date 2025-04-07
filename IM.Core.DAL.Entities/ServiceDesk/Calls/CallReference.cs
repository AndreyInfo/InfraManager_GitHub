using System;

namespace InfraManager.DAL.ServiceDesk
{
    public abstract class CallReference
    {
        protected CallReference()
        {
        }

        protected CallReference(Guid callID, Guid objectID, ObjectClass objectClassID)
        {
            CallID = callID;
            ObjectID = objectID;
            ObjectClassID = objectClassID;
        }

        public Guid CallID { get; }
        public Guid ObjectID { get; }
        public ObjectClass ObjectClassID { get; }
    }

    public class CallReference<TReference> : CallReference where TReference : IHaveUtcModifiedDate, IGloballyIdentifiedEntity
    {
        public CallReference(Guid callID, Guid objectID)
            : base(callID, objectID, typeof(TReference).GetObjectClassOrRaiseError())
        {
        }

        public override string ToString() => 
            $"reference between Call (ID = {CallID}) and {typeof(TReference).Name} (ID = {ObjectID})";
    }
}
