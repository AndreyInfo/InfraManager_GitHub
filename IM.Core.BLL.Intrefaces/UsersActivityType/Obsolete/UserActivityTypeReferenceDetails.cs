using System;

namespace InfraManager.BLL.UsersActivityType.Obsolete
{
    [Obsolete("Use InfraManager.BLL.UsersActivityType.UserActivityTypeReferenceDetails instead")]
    public class UserActivityTypeReferenceDetails
    {
        protected UserActivityTypeReferenceDetails()
        {
            
        }

        public UserActivityTypeReferenceDetails(Guid userActivityTypeID, ObjectClass objectClass, Guid objectID)
        {
            Id = Guid.NewGuid();
            UserActivityTypeId = userActivityTypeID;
            ObjectClassId = objectClass;
            ObjectId = objectID;
        }
        
        public Guid Id { get; init; }
        public Guid UserActivityTypeId { get; init; }
        public ObjectClass ObjectClassId { get; init; }
        public Guid ObjectId { get; init; }
    }
}
