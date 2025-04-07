using Inframanager;
using InfraManager.DAL.ServiceCatalogue;
using System;

namespace InfraManager.DAL.Calendar
{
    [OperationIdMapping(ObjectAction.Insert, OperationID.CalendarExclusion_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.CalendarExclusion_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.CalendarExclusion_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.CalendarExclusion_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.CalendarExclusion_Properties)]
    public class CalendarExclusion
    {
        public CalendarExclusion()
        { }

        public CalendarExclusion(ObjectClass objectClassID, Guid objectID
                                , Guid exclusionID
                                , DateTime utcPeriodStart, DateTime utcPeriodEnd
                                , bool isWorkPeriod
                                , ObjectClass relatedObjectClassID, Guid relatedObjectID
            )
        {
            ID = Guid.NewGuid();
            ObjectClassID = objectClassID;
            ObjectID = objectID;
            ExclusionID = exclusionID;
            UtcPeriodStart = utcPeriodStart;
            UtcPeriodEnd = utcPeriodEnd;
            IsWorkPeriod = isWorkPeriod;
            ServiceReference = new ServiceReference(relatedObjectClassID, ID, relatedObjectID);
            ServiceReferenceID = ServiceReference.ID;
        }

        public Guid ID { get; init; }

        public ObjectClass ObjectClassID { get; set; }

        public Guid ObjectID { get; set; }

        public Guid ExclusionID { get; set; }

        public DateTime UtcPeriodStart { get; set; }

        public DateTime UtcPeriodEnd { get; set; }

        public bool IsWorkPeriod { get; set; }

        public Guid? ServiceReferenceID { get; set; }

        public virtual Exclusion Exclusion { get; init; }

        public virtual ServiceReference ServiceReference { get; init; }
    }
}
