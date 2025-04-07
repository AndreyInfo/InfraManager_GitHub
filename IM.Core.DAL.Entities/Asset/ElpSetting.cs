using System;
using Inframanager;

namespace InfraManager.DAL.Asset
{
    [ObjectClassMapping(ObjectClass.ELPSetting)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.ELPSetting_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.ELPSetting_Properties)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.ELPSetting_Delete)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.ELPSetting_Insert)]
    [OperationIdMapping(ObjectAction.Update, OperationID.ELPSetting_Update)]
    public class ElpSetting
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public Guid? VendorId { get; set; }
        public virtual Manufacturer Vendor { get; set; }
    }
}