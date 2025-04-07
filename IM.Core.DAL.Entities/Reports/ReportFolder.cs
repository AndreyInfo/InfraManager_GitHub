using Inframanager;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL
{
    [ObjectClassMapping(ObjectClass.ReportFolder)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.ReportFolder_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.ReportFolder_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.ReportFolder_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.ReportFolder_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.ReportFolder_Properties)]
    public class ReportFolder
    {
        public ReportFolder()
        {
            ID = Guid.NewGuid();
            ReportFolderID = Guid.Empty;
        }
        public Guid ID { get; init; }
        public string Name { get; init; }
        public string Note { get; init; }
        public Guid ReportFolderID { get; init; }
        public byte SecurityLevel { get; set; }
        /// <summary>
        /// Возвращает или задает идентификатор версии
        /// </summary>
        public byte[] RowVersion { get; set; }
        public virtual ICollection<ReportFolder> Childs { get; init; }
    }
}
