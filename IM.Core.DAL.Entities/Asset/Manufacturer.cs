using System;
using System.Collections.Generic;
using IM.Core.Import.BLL.Interface.Import.Models.SaveOrUpdateData;
using Inframanager;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.DAL.Asset
{
    [ObjectClassMapping(ObjectClass.Manufacturer)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.Manufacturer_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.Manufacturer_Update )]
    [OperationIdMapping(ObjectAction.Delete, OperationID.Manufacturer_Delete )]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.Manufacturer_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Manufacturer_Properties)]
    public class Manufacturer : IImportEntity
    {
        public static int EmptyID = 0;

        public int ID { get; init; }

        [ImportField(Name ="Производитель-Название", Required = false)]
        public string Name { get; set; }

        public bool IsCable { get; init; }

        public bool IsRack { get; init; }

        public bool IsPanel { get; init; }

        public bool IsNetworkDevice { get; init; }
        
        public bool IsComputer { get; init; }

        public bool IsOutlet { get; init; }
        
        public bool IsCableCanal { get; init; }
        
        public bool IsSoftware { get; init; }

        public bool IsMaterials { get; init; }

        public Guid? ImObjID { get; init; }

        public string ExternalID { get; set; }

        public int? ComplementaryID { get; init; }

        public Guid? ComplementaryGuidID { get; init; }

        public virtual ICollection<AdapterType> AdapterTypes { get; init; }
        public virtual ICollection<ElpSetting> Elpsetting { get; init; }
        public virtual ICollection<NetworkDeviceModel> NetworkDeviceModel { get; init; }
        public virtual ICollection<TerminalDeviceModel> TerminalDeviceModel { get; init; }
        public virtual IEnumerable<PeripheralType> PeripheralType { get; init; }
        
        public virtual ICollection<CabinetType> RackTypes { get; init; }
    }
}