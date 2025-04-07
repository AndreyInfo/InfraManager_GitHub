using System;
using System.Collections.Generic;
using Inframanager;

namespace InfraManager.DAL.ServiceDesk.Manhours
{
    [ObjectClassMapping(ObjectClass.ManhoursWork)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.ManhoursWork_Properties)]
    [OperationIdMapping(ObjectAction.Update, OperationID.ManhoursWork_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.ManhoursWork_Delete)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.ManhoursWork_Add)]
    public class ManhoursWork : IGloballyIdentifiedEntity
    {
        public const int MaxDescriptionLength = 250;

        public Guid IMObjID { get; }
        public Guid ObjectID { get; }
        public ObjectClass ObjectClassID { get; }
        public string Description { get; set; }
        public virtual User Executor { get; }
        public virtual User Initiator { get; }
        public Guid? UserActivityTypeID { get; set; }
        public virtual UserActivityType UserActivityType { get; }
        public int Number { get; }
        public Guid? ExecutorID { get; set; }
        public Guid? InitiatorID { get; set; }
        public virtual ICollection<ManhoursEntry> Entries { get; }

        public ManhoursWork()
        {
        }

        public ManhoursWork(Guid objectId, ObjectClass objectClassId)
        {
            IMObjID = Guid.NewGuid();
            ObjectID = objectId;
            ObjectClassID = objectClassId;
            Entries = new List<ManhoursEntry>();
        }

        public void AddManhour(int value, DateTime startDate)
        {
            Entries.Add(new ManhoursEntry(IMObjID, value, startDate)
            {
                UtcDate = startDate,
                Value = value
            });
        }
    }
}