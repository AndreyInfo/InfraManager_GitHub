using System;
using System.Collections.Generic;
using Inframanager;
using InfraManager.DAL.Configuration;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.DAL.Asset
{
    [ObjectClassMapping(ObjectClass.TerminalDeviceModel)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.TerminalDeviceModel_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.TerminalDeviceModel_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.TerminalDeviceModel_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.TerminalDeviceModel_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
    public partial class TerminalDeviceModel : 
        IProductModel<int>
        , IImportModel
    {
        public int ID { get; init; }
        public string Name { get; set; }

        public int ManufacturerID { get; set; }
        public int? ConnectorTypeID { get; set; }
        public int? TechnologyTypeID { get; set; }
        public string ImageCyrillic { get; set; }
        public string ProductNumberCyrillic { get; set; }
        public string ProductNumber { get; set; }
        public string ExternalID { get; set; }
        public string Code { get; set; }

        public string Note { get; set; }
        public bool Removed { get; set; }
        public Guid IMObjID { get; init; }
        public int? ComplementaryID { get; set; }
        public Guid? HypervisorModelID { get; set; }
        public Guid ProductCatalogTypeID { get; set; }
        public byte[] RowVersion { get; init; }
        public bool CanBuy { get; set; }

        public virtual TechnologyType TechnologyType { get; init; }
        public virtual ConnectorType ConnectorType { get; init; }
        public virtual ProductCatalogType ProductCatalogType { get; init; }
        public virtual Manufacturer Manufacturer { get; init; }
        public virtual ICollection<TerminalDevice> TerminalDevice { get; init; }
    }
}