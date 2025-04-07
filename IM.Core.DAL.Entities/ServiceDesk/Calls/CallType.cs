using Inframanager;
using InfraManager.DAL.ServiceCatalogue;
using System;

namespace InfraManager.DAL.ServiceDesk;

[ObjectClassMapping(ObjectClass.CallType)]
[OperationIdMapping(ObjectAction.Insert, OperationID.CallType_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.CallType_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.CallType_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.CallType_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.CallType_Properties)]
public class CallType : Catalog<Guid>, IMarkableForDelete
    {

        public CallType(string name)
        {
            Name = name;
            ID = Guid.NewGuid();
        }

        public static Guid CallID = Guid.Empty;
        private static Guid IncidentID = new("00000000-0000-0000-0000-000000000001");
        public static Guid ChangeRequestID = new("00000000-0000-0000-0000-000000000002");
        private static Guid ErrorID = new("00000000-0000-0000-0000-000000000003");
        private static Guid QuestionID = new("00000000-0000-0000-0000-000000000004");

        public bool VisibleInWeb { get; set; }

        public Guid? EventHandlerCallTypeID { get; set; }

        public string EventHandlerName { get; set; }

        public byte[] Icon { get; set; }

        public bool Removed { get; private set; }

        public byte[] RowVersion { get; set; }

        public string WorkflowSchemeIdentifier { get; set; }

        public bool UseWorkflowSchemeFromAttendance { get; set; }
        
        public string IconName { get; set; }
        
        public bool IsFixed { get; set; }

        public Guid? ParentCallTypeID { get; set; }

        public string FullName => Parent == null ? Name : $"{Parent.FullName} / {Name}";

        public virtual CallType Parent { get; set; }
        
        public void MarkForDelete() => Removed = true;       

        public string GetWorkflowSchemeIdentifier(ServiceAttendance serviceAttendance)
        {
            return (serviceAttendance != null
                        && UseWorkflowSchemeFromAttendance
                        && (ID == ChangeRequestID
                            || ID == CallID
                            || EventHandlerCallTypeID == ChangeRequestID)
                    ? serviceAttendance.WorkflowSchemeIdentifier
                    : Parent == null ? null : WorkflowSchemeIdentifier)
                ?? Parent?.GetWorkflowSchemeIdentifier(serviceAttendance);
        }

        public bool IsIncident => ID == IncidentID
            || (ParentCallTypeID == CallID && EventHandlerCallTypeID == IncidentID)
            || (Parent != null && Parent.IsIncident);

        public bool IsChangeRequest => ID == ChangeRequestID
            || (ParentCallTypeID == CallID && EventHandlerCallTypeID == ChangeRequestID)
            || (Parent != null && Parent.IsChangeRequest);

        public static string GetFullCallTypeName(Guid? id) => throw new NotSupportedException();
        
        public static Guid GetRootId(Guid? id) => throw new NotSupportedException();
    }
