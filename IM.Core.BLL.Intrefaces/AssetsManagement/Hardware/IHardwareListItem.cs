namespace InfraManager.BLL.AssetsManagement.Hardware;

public interface IHardwareListItem
{
    string TypeName { get; }
    string ProductCatalogTemplateName { get; }
    string InvNumber { get; }
    string ServiceContractLifeCycleStateName { get; }
    string VendorName { get; }
    string RoomName { get; }
    string RackName { get; }
    string WorkplaceName { get; }
    string OwnerName { get; }
    string UserName { get; }
    string UtilizerName { get; }
    string ServiceCenterName { get; }
    string ServiceContractNumber { get; }
    string SupplierName { get; }
    decimal? Cost { get; }
    string AppointmentDate { get; }
    string Warranty { get; }
    string DateReceived { get; }
    string DateInquiry { get; }
    string DateAnnuled { get; }
    string ServiceContractUtcFinishDate { get; }
    string ModelName { get; }
    string LifeCycleStateName { get; }
}