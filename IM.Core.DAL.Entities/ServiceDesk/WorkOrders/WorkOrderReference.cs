using System;
using System.Linq.Expressions;

namespace InfraManager.DAL.ServiceDesk.WorkOrders
{
    [ObjectClassMapping(ObjectClass.WorkOrderReference)]
    public abstract class WorkOrderReference
    {
        public const long NullID = 0;

        protected WorkOrderReference()
        {
        }

        protected internal WorkOrderReference(Guid objectID, ObjectClass classID, string referenceName)
        {
            ObjectID = objectID;
            ObjectClassID = classID;
            ReferenceName = referenceName; // пока эти референсы на практике не могут поменяться
        }

        public long ID { get; }
        public ObjectClass ObjectClassID { get; }
        public Guid ObjectID { get; }
        public string ReferenceName { get; }

        public static Expression<Func<WorkOrderReference, bool>> NullReference =>
            x => x.ID == NullID;

        public static Expression<Func<WorkOrderReference, bool>> ByReferencedObject(InframanagerObject referencedObject) =>
            x => x.ObjectID == referencedObject.Id && x.ObjectClassID == referencedObject.ClassId;

        /// <summary>
        /// Возвращает <c>true</c> если этот объект является null-записью; иначе - <c>false</c>.
        /// </summary>
        public bool IsDefault => ID == NullID && ObjectClassID == ObjectClass.Unknown;
    }

    public class WorkOrderReference<TReference> : WorkOrderReference
        where TReference : IHaveWorkOrderReferences
    {
        protected WorkOrderReference() : base() 
        {
        }

        internal WorkOrderReference(TReference reference)
            : base(reference.IMObjID, GetReferenceClassID(), reference.ReferenceName)
        {
        }

        private static ObjectClass GetReferenceClassID() =>
            typeof(TReference).GetObjectClassOrDefault() 
                ?? throw new NotSupportedException($"Type {typeof(TReference)} should have {nameof(ObjectClassMappingAttribute)} in order to reference work orders.");
    }
}
