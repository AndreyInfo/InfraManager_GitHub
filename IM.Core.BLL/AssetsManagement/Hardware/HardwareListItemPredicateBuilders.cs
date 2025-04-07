using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.AssetsManagement.Hardware;

namespace InfraManager.BLL.AssetsManagement.Hardware;

internal class HardwareListItemPredicateBuilders<TQueryResultItem, TListItem> :
    FilterBuildersAggregate<TQueryResultItem, TListItem>
    where TQueryResultItem : HardwareListQueryResultItemBase
    where TListItem : IHardwareListItem
{
    public HardwareListItemPredicateBuilders()
    {
        AddPredicateBuilder(queryItem => queryItem.Name);
        AddPredicateBuilder(queryItem => queryItem.SerialNumber);
        AddPredicateBuilder(queryItem => queryItem.Code);
        AddPredicateBuilder(queryItem => queryItem.Note);
        AddPredicateBuilder(
            reportItem => reportItem.RoomName, 
            queryItem => queryItem.RoomID);
        AddPredicateBuilder(
            reportItem => reportItem.RackName, 
            queryItem => queryItem.RackID);
        AddPredicateBuilder(
            reportItem => reportItem.WorkplaceName, 
            queryItem => queryItem.WorkplaceID);
        AddPredicateBuilder(
            reportItem => reportItem.LifeCycleStateName,
            queryItem => queryItem.AssetItem.LifeCycleStateID);
        AddPredicateBuilder(queryItem => queryItem.Agreement);
        AddPredicateBuilder(
            reportItem => reportItem.UserName,
            queryItem => queryItem.AssetItem.UserID);
        AddPredicateBuilder(queryItem => queryItem.Founding);
        AddPredicateBuilder(
            reportItem => reportItem.OwnerName,
            queryItem => queryItem.AssetItem.OwnerID);
        AddPredicateBuilder(
            reportItem => reportItem.UtilizerName,
            queryItem => queryItem.AssetItem.UtilizerID);
        AddPredicateBuilder(
            reportItem => reportItem.AppointmentDate,
            queryItem => queryItem.AssetItem.AppointmentDate);
        AddPredicateBuilder(
            reportItem => reportItem.Cost,
            queryItem => queryItem.AssetItem.Cost);
        AddPredicateBuilder(
            reportItem => reportItem.ServiceCenterName,
            queryItem => queryItem.AssetItem.ServiceCenterID);
        AddPredicateBuilder(
            reportItem => reportItem.ServiceContractNumber,
            queryItem => queryItem.AssetItem.ServiceContractID);
        AddPredicateBuilder(
            reportItem => reportItem.Warranty,
            queryItem => queryItem.AssetItem.Warranty);
        AddPredicateBuilder(
            reportItem => reportItem.SupplierName,
            queryItem => queryItem.AssetItem.SupplierID);
        AddPredicateBuilder(
            reportItem => reportItem.DateReceived,
            queryItem => queryItem.AssetItem.DateReceived);
        AddPredicateBuilder(
            reportItem => reportItem.DateInquiry,
            queryItem => queryItem.AssetItem.DateInquiry);
        AddPredicateBuilder(
            reportItem => reportItem.DateAnnuled,
            queryItem => queryItem.AssetItem.DateAnnuled);
        AddPredicateBuilder(queryItem => queryItem.IsWorking);
        AddPredicateBuilder(queryItem => queryItem.LocationOnStore); 
        AddPredicateBuilder(
            reportItem => reportItem.ServiceContractLifeCycleStateName,
            queryItem => queryItem.AssetItem.ServiceContractLifeCycleStateID);
        AddPredicateBuilder(
            reportItem => reportItem.ServiceContractUtcFinishDate,
            queryItem => queryItem.AssetItem.ServiceContractUtcFinishDate);
    }
}