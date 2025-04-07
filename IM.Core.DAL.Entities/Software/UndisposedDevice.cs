using System;

namespace InfraManager.DAL.Software
{
    [ObjectClassMapping(ObjectClass.UndisposedDevice)]
    public partial class UndisposedDevice : INamedEntity, IGloballyIdentifiedEntity
    {
        public int DeviceId { get; set; }
        public string ComputerName { get; set; }
        public string Ipaddress { get; set; }
        public string SubNetMask { get; set; }
        public string Macaddress { get; set; }
        public string BiosName { get; set; }
        public string BiosVersion { get; set; }
        public string DefaultPrinter { get; set; }
        public int? CsVendorId { get; set; }
        public string CsModel { get; set; }
        public string CsSize { get; set; }
        public string CsFormFactor { get; set; }
        public string SerlNumber { get; set; }
        public string InvNumber { get; set; }
        public Guid IMObjID { get; set; }
        public Guid? ReportId { get; set; }
        public DateTime? DateInquiry { get; set; }
        public string SmbiosassetTag { get; set; }
        public string UserName { get; set; }
        public string LogonDomain { get; set; }
        public string ModelName { get; set; }
        public string ManufacturerName { get; set; }
        public string Description { get; set; }
        public DateTime? DateReceived { get; set; }
        public decimal? Cost { get; set; }
        public DateTime? Warranty { get; set; }
        public string Agreement { get; set; }
        public string Founding { get; set; }
        public string ResponsibleUser { get; set; }
        public string OwningOrganization { get; set; }
        public string UtilizerName { get; set; }
        public string Code { get; set; }
        public string SupplierName { get; set; }
        public string UserField1 { get; set; }
        public string UserField2 { get; set; }
        public string UserField3 { get; set; }
        public string UserField4 { get; set; }
        public string UserField5 { get; set; }
        public string BuildingName { get; set; }
        public string RoomName { get; set; }
        public string LocationString { get; set; }
        public int? ClassId { get; set; }
        public string TypeName { get; set; }
        public bool? IsLogical { get; set; }
        public int? ProductCatalogTemplateId { get; set; }
        public string Note { get; set; }
        public byte? ProviderType { get; set; }
        public string SourceName { get; set; }
        public string ExternalId { get; set; }
        public string SupplierExternalId { get; set; }
        public string ModelExternalId { get; set; }

        public string GetName()
        {
            return ComputerName;
        }
    }
}