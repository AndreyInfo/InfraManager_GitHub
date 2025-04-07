using Inframanager;
using System;

namespace InfraManager.DAL
{
    [ObjectClassMapping(ObjectClass.Report)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.Report_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.Report_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.Report_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.Report_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Report_Properties)]
    public class Reports
    {
        public Reports()
        {
            ID = Guid.NewGuid();
            DateCreated = DateTime.Now;
            DateModified = DateCreated;
        }
        public Guid ID { get; init; }
        public string Name { get; init; }
        public string Note { get; init; }
        public DateTime DateCreated { get; init; }
        public DateTime DateModified { get; set; }
        public string Data { get; set; }
        public Guid ReportFolderID { get; init; }
        public byte SecurityLevel { get; init; }
        public byte[] RowVersion { get; set; }

        public virtual ReportFolder Folder { get; init; }
    }
}
