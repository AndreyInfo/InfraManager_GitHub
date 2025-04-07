using System;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL.Settings.TableFilters.Attributes;
using InfraManager.BLL.Settings.UserFields;
using InfraManager.DAL;
using InfraManager.DAL.Settings.UserFields;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.AssetsManagement.Hardware;

/// <summary>
/// Этот класс описывает выходной контракт для списка оборудования.
/// </summary>
[ListViewItem(ListView.HardwareList, OperationID.ApplicationModule_ConfigurationManagment_View)]
public class HardwareListItem : IHardwareListItem
{
    public Guid ID { get; init; }
    public ObjectClass ClassID { get; init; }

    [LikeFilter]
    [ColumnSettings(0, 100)]
    [Label(nameof(Resources.AssetNumber_Name))]
    public string Name { get; init; }

    [LikeFilter]
    [ColumnSettings(1)]
    [Label(nameof(Resources.AssetNumber_SerialNumber))]
    public string SerialNumber { get; init; }

    [LikeFilter]
    [ColumnSettings(2)]
    [Label(nameof(Resources.AssetNumber_Code))]
    public string Code { get; init; }

    [LikeFilter]
    [ColumnSettings(3)]
    [Label(nameof(Resources.AssetNumber_Note))]
    public string Note { get; init; }

    [MultiSelectFilter(LookupQueries.ProductCatalogTypes)]
    [ColumnSettings(4)]
    [Label(nameof(Resources.AssetNumber_TypeName))]
    public string TypeName { get; init; }

    [MultiSelectFilter(LookupQueries.HardwareModelNames)]
    [ColumnSettings(5)]
    [Label(nameof(Resources.AssetNumber_ModelName))]
    public string ModelName { get; init; }

    [MultiSelectFilter(LookupQueries.Manufacturers)]
    [ColumnSettings(6)]
    [Label(nameof(Resources.AssetNumber_VendorName))]
    public string VendorName { get; init; }

    [SimpleValueFilter("RoomSearcher", false)]
    [ColumnSettings(7)]
    [Label(nameof(Resources.AssetNumber_RoomName))]
    public string RoomName { get; init; }

    [SimpleValueFilter("RackSearcher", false)]
    [ColumnSettings(8)]
    [Label(nameof(Resources.AssetNumber_RackName))]
    public string RackName { get; init; }

    [SimpleValueFilter("WorkplaceSearcher", false)]
    [ColumnSettings(9)]
    [Label(nameof(Resources.AssetNumber_WorkplaceName))]
    public string WorkplaceName { get; init; }

    [ColumnSettings(10)]
    [Label(nameof(Resources.AssetNumber_FloorName))]
    public string FloorName { get; init; }

    [ColumnSettings(11)]
    [Label(nameof(Resources.AssetNumber_BuildingName))]
    public string BuildingName { get; init; }

    [ColumnSettings(12)]
    [Label(nameof(Resources.AssetNumber_OrganizationName))]
    public string OrganizationName { get; init; }

    [MultiSelectFilter(LookupQueries.HardwareStates)]
    [ColumnSettings(13)]
    [Label(nameof(Resources.AssetNumber_AssetStateName))]
    public string LifeCycleStateName { get; init; }
    public string LifeCycleStateID { get; init; }

    [LikeFilter]
    [ColumnSettings(15)]
    [Label(nameof(Resources.AssetNumber_InvNumber))]
    public string InvNumber { get; init; }

    [LikeFilter]
    [ColumnSettings(16)]
    [Label(nameof(Resources.AssetNumber_Agreement))]
    public string Agreement { get; init; }

    [SimpleValueFilter("WebUserSearcher", true)]
    [ColumnSettings(17)]
    [Label(nameof(Resources.AssetNumber_UserName))]
    public string UserName { get; init; }

    [LikeFilter]
    [ColumnSettings(18)]
    [Label(nameof(Resources.AssetNumber_Founding))]
    public string Founding { get; init; }

    // todo: Возможно, когда будет понятно почему legacy ищет только по организациям, придется мигрировать OwnerSearcher с исправлением.
    [SimpleValueFilter("OrganizationSearcher", false)]
    [ColumnSettings(19)]
    [Label(nameof(Resources.AssetNumber_OwnerName))]
    public string OwnerName { get; init; }

    [SimpleValueFilter("UtilizerSearcher", true)]
    [ColumnSettings(20)]
    [Label(nameof(Resources.AssetNumber_UtilizerName))]
    public string UtilizerName { get; init; }

    [DatePickFilter(true)]
    [ColumnSettings(21)]
    [Label(nameof(Resources.AssetNumber_AppointmentDate))]
    public string AppointmentDate { get; init; }

    [RangeSliderFilter(true)]
    [ColumnSettings(22)]
    [Label(nameof(Resources.AssetNumber_Cost))]
    public decimal? Cost { get; init; }

    [MultiSelectFilter(LookupQueries.Suppliers)]
    [ColumnSettings(23)]
    [Label(nameof(Resources.AssetNumber_ServiceCenterName))]
    public string ServiceCenterName { get; init; }

    [MultiSelectFilter(LookupQueries.ServiceContracts)]
    [ColumnSettings(24)]
    [Label(nameof(Resources.AssetNumber_ServiceContractNumber))]
    public string ServiceContractNumber { get; init; }

    [DatePickFilter(true)]
    [ColumnSettings(25)]
    [Label(nameof(Resources.AssetNumber_Warranty))]
    public string Warranty { get; init; }

    [MultiSelectFilter(LookupQueries.Suppliers)]
    [ColumnSettings(26)]
    [Label(nameof(Resources.AssetNumber_SupplierName))]
    public string SupplierName { get; init; }

    [DatePickFilter(true)]
    [ColumnSettings(27)]
    [Label(nameof(Resources.AssetNumber_DateReceived))]
    public string DateReceived { get; init; }

    [DatePickFilter(true)]
    [ColumnSettings(28)]
    [Label(nameof(Resources.AssetNumber_DateInquiry))]
    public string DateInquiry { get; init; }

    [DatePickFilter(true)]
    [ColumnSettings(29)]
    [Label(nameof(Resources.AssetNumber_DateAnnuled))]
    public string DateAnnuled { get; init; }

    [ColumnSettings(30, false)]
    [UserFieldDisplay(UserFieldType.Asset, FieldNumber.UserField1)]
    public string UserField1 { get; init; }

    [ColumnSettings(31, false)]
    [UserFieldDisplay(UserFieldType.Asset, FieldNumber.UserField2)]
    public string UserField2 { get; init; }

    [ColumnSettings(32, false)]
    [UserFieldDisplay(UserFieldType.Asset, FieldNumber.UserField3)]
    public string UserField3 { get; init; }

    [ColumnSettings(33, false)]
    [UserFieldDisplay(UserFieldType.Asset, FieldNumber.UserField4)]
    public string UserField4 { get; init; }

    [ColumnSettings(34, false)]
    [UserFieldDisplay(UserFieldType.Asset, FieldNumber.UserField5)]
    public string UserField5 { get; init; }

    [MultiSelectFilter(LookupQueries.IsWorkingLookup)]
    [ColumnSettings(35)]
    [Label(nameof(Resources.AssetNumber_IsWorking))]
    public string IsWorking { get; init; }

    [MultiSelectFilter(LookupQueries.ProductCatalogTemplates)]
    [ColumnSettings(36)]
    [Label(nameof(Resources.AssetNumber_ClassName))]
    public string ProductCatalogTemplateName { get; init; }

    [MultiSelectFilter(LookupQueries.LocationOnStoreLookup)]
    [ColumnSettings(37)]
    [Label(nameof(Resources.AssetNumber_LocationOnStore))]
    public string LocationOnStore { get; init; }

    [MultiSelectFilter(LookupQueries.ServiceContractStates)]
    [ColumnSettings(38)]
    [Label(nameof(Resources.ServiceContractState))]
    public string ServiceContractLifeCycleStateName { get; init; }

    [DatePickFilter(true)]
    [ColumnSettings(39)]
    [Label(nameof(Resources.ServiceContractFinishDate))]
    public string ServiceContractUtcFinishDate { get; init; }
}