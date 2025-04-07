using System;
using InfraManager;

namespace Inframanager.DAL.ActiveDirectory.Import
{
     [ObjectClassMapping(ObjectClass.UIADPath)]
     [OperationIdMapping(ObjectAction.Insert, OperationID.UIADPath_Insert)]
     [OperationIdMapping(ObjectAction.Update, OperationID.UIADPath_Update)]
     [OperationIdMapping(ObjectAction.Delete, OperationID.UIADPath_Delete)]
     [OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
     [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
    public class UIADPath
    {
        public Guid ID { get; init; }

        public Guid? ADPathID { get; init; }

        public Guid ADSettingID { get; init; }

        public string Path { get; init; }
    }
}