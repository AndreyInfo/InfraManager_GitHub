using System;
using System.Collections.Generic;
using Inframanager;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.DAL.Asset
{
    [ObjectClassMapping(ObjectClass.NetworkDeviceModel)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.NetworkDeviceModel_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.NetworkDeviceModel_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.NetworkDeviceModel_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails,OperationID.NetworkDeviceModel_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
    public class NetworkDeviceModel : IGloballyIdentifiedEntity
        , IProductModel<int>
        , IImportEntity
    {
        public int ID { get; init; }
        
        public string Name { get; set; }

        public int ManufacturerID { get; set; }
        
        public int? PortCount { get; set; }
        
        public string ImageCyrillic { get; set; } 

        public decimal Width { get; set; }

        public decimal Height { get; set; }
        public decimal? HeightInUnits { get; set; }
        public string ProductNumberCyrillic { get; set; } 
        public string Oid { get; set; }
        public int SlotCount { get; set; }

        public string ProductNumber { get; set; }
        public string ExternalID { get; set; }
        public string Code { get; set; }

        public string Note { get; set; }

        public bool Removed { get; set; }
        
        public Guid IMObjID { get; init; }
        
        public int? ComplementaryID { get; set; }
        
        public decimal? Depth { get; set; }
        
        public bool IsRackmount { get; set; }
        
        public Guid? HypervisorModelID { get; set; }
        
        public int? MaxLoad { get; set; }
        
        public int? NominalLoad { get; set; }
        
        public bool? ColorPrint { get; set; }
        
        public bool? PhotoPrint { get; set; }
        
        public bool? DuplexMode { get; set; }
        
        public int? PrintTechnology { get; set; }
       
        public int? MaxPrintFormat { get; set; }
        
        public int? PrintSpeedUnit { get; set; }
        
        public int? RollNumber { get; set; }
        
        public decimal? Speed { get; set; }
        
        public Guid ProductCatalogTypeID { get; set; }
        
        public byte[] RowVersion { get; init; }
        
        public bool CanBuy { get; set; }

        public virtual ProductCatalogType ProductCatalogType { get; init; }
        
        public virtual Manufacturer Manufacturer { get; init; }
        
        public virtual ICollection<NetworkDevice> NetworkDevice { get; init; }
    }
}