using InfraManager.Core.Data;
using InfraManager.Core.Exceptions;
using InfraManager.Core.Logging;
using InfraManager.Web.BLL.Assets;
using InfraManager.Web.BLL.Assets.AssetNumber;
using InfraManager.Web.BLL.Assets.AssetSearch;
using InfraManager.Web.BLL.Assets.Repair;
using InfraManager.Web.BLL.Assets.Utilizer;
using InfraManager.Web.BLL.Assets.WrittenOff;
using InfraManager.Web.BLL.Contracts;
using InfraManager.Web.BLL.Finance.ActivesRequest;
using InfraManager.Web.BLL.Finance.Budget;
using InfraManager.Web.BLL.Finance.GoodsInvoice;
using InfraManager.Web.BLL.Finance.LocatedActives;
using InfraManager.Web.BLL.Finance.Purchase;
using InfraManager.Web.BLL.Finance.Specification;
using InfraManager.Web.BLL.Finance.Specification.Search;
using InfraManager.Web.BLL.Inventories;
using InfraManager.Web.BLL.SD.Calls;
using InfraManager.Web.BLL.SD.MyWorkplace;
using InfraManager.Web.BLL.SD.Problems;
using InfraManager.Web.BLL.SD.WorkOrders;
using InfraManager.Web.BLL.Software;
using InfraManager.Web.BLL.Tables;
using InfraManager.Web.Controllers;
using InfraManager.Web.DTL.Tables;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using InfraManager.Web.BLL.Assets.LicenceConsumption;
using InfraManager.Web.BLL.Assets.LicenceReturning;
using InfraManager.Web.DTL.Software;
using RequestResponceType = InfraManager.Web.Controllers.RequestResponceType;
using SoftwareLicence = InfraManager.Web.BLL.Software.SoftwareLicence;
using InfraManager.Web.BLL.SD.RFC;
using InfraManager.Web.BLL.Catalogs;
using InfraManager.Web.BLL.Assets.DataEntity;
using InfraManager.Web.BLL.Synonyms;
using InfraManager.Web.BLL.Finance.Unit;
using InfraManager.Web.BLL.Workflow;
using InfraManager.Web.BLL.Costs;
using InfraManager.BLL.Web.SD.KB;
using InfraManager.ResourcesArea;
using Global = IMSystem.Global;

namespace InfraManager.Web.Helpers
{
    public static class TableHelper
    {
        public enum TableType : byte
        {
            SD = 0,
            Asset = 1,
            AssetSearch = 2,
            Finance = 3,
            AssetModelSearch = 4,
            PurchaseSpecificationSearch = 5,
            ARSSearch = 6,
            GoodsInvoiceSpecificationSearch = 7,
            ARS = 8,
            LocatedActivesSearch = 9,
            frmFinanceBudgetRow_Table = 10,
            Contract = 11,
            ContractAgreement = 12,
            ContractAssetMaintenance = 13,
            ContractLicenceMaintenance = 14,
            Supplier = 15,
            SupplierContactPerson = 16,
            SoftwareLicence = 17,
            ContractAndAgreementSearch = 18,
            AdapterReference = 19,
            PeripheralReference = 20,
            SubdeviceList = 21,
            ContractLicence = 22,
            Inventory = 23,
            InventorySpecification = 24,
            SoftwareLicenceSerialNumber = 25,
            SoftwareLicenceReference = 26,
            AdminTools = 27,
            Users = 28,
            Equip = 29,
            SoftwareLicenceReferenceReturning = 30,
            OrganizationList = 31,
            SubdivisionList = 32,
            CallUsersList = 33,
            AssetUsersList = 34,
            SoftwareLicenceUpdate = 35,
            AgreementAssetMaintenance = 36,
            AgreementLicenceMaintenance = 37,
            ContractLicenceAgreement = 38,
            SoftwareLicenceModel = 39,
            SDMobileSearch = 40,
            SoftModel = 41,
            //SDWithoutFilter = 42,
            RFCCategory = 43,
            Deputy = 44,
            DataEntityDependency = 45,
            ClusterVM = 46,
            ClusterHosts = 47,
            SDMobileCommonSearch = 48,
            SoftwareModelUsingType = 49,
            Manufacturer = 50,
            Synonym = 51,
            Positions = 52,
            Units = 53,
            Criticality = 54,
            InfrastructureSegment = 55,
            // TODO: после перехода на получение данных по дефолтным значениям для грида из нового webApi, класс удалить
            LicensingScheme = 56,
            SoftwareLicenceLocation = 57,
            ProcessorSimpleSearch = 58,
            Workflow = 59,
            FileSystem = 60,
            CostCategory = 61,
            TelephoneType = 62,
            TechnologyType = 63,
            CartridgeType = 64,
            ConnectorType = 65,
            SlotType = 66,
            Notification = 67,
            SoftwareModelUpdate = 68,
            SoftwareModelLicenses = 69,
            LogicObjectComponents = 70,
            ProblemTypeNew = 71,
            SoftwareModelСomponent = 72,
            CalendarWeekend = 73,
            CalendarHoliday = 74,
            FormBuilder = 75,
            SoftwareModelCatalog = 76,
            KBAUserList = 77,
            Exclusion = 78,
            ParameterEnum = 79,
            CalendarWorkSchedule = 80,
            SoftwareModelPackageContents = 81,
            SoftwareModelInstallation = 82,
            SoftwareModelProcessNameList = 83,
            SoftwareModelCatalogDependency = 84,
            SoftwareModelRelated = 85,
            SynonymsSoftwareModelForTable = 86
        }
        //
        public sealed class GetTableOutModel
        {
            public List<Guid> ObjectIDList { get; set; } //если список строится не по заявко-заданиям-проблемам, сюда айдишник соответствующей по номеру
            public List<Guid> IDList { get; set; }
            public List<int> ClassIDList { get; set; }
            public List<ColumnWithData> DataList { get; set; }
            public List<OperationInfo> OperationInfoList { get; set; }
            public RequestResponceType Result { get; set; }
        }
        //
        private class FilterParams
        {
            public BLL.Tables.Filters.Filter Filter;
            public bool WithFinishedWorkflow;
            public DateTime? AfterUtcDateModified;
            public DTL.Settings.UserTreeSettings TreeSettings;
            //
            private FilterParams() { }
            //
            public static FilterParams Create(BLL.Tables.Filters.Filter filter, bool withFinishedWorkflow, DateTime? afterUtcDateModified, DTL.Settings.UserTreeSettings settings)
            {
                var retval = new FilterParams();
                //
                retval.Filter = filter;
                retval.WithFinishedWorkflow = withFinishedWorkflow;
                retval.AfterUtcDateModified = afterUtcDateModified;
                retval.TreeSettings = settings;
                //
                return retval;
            }
        }
        //
        private class ColumnParams
        {
            public DTL.Settings.ColumnSettings Column;
            public BLL.Settings.ColumnSettings ColumnSettings;
            //
            private ColumnParams() { }
            //
            public static ColumnParams Create(DTL.Settings.ColumnSettings column, BLL.Settings.ColumnSettings settings)
            {
                var retval = new ColumnParams();
                //
                retval.Column = column;
                retval.ColumnSettings = settings;
                //
                return retval;
            }
        }
        //
        public static GetTableOutModel GetListForTable(TableLoadRequestInfo requestInfo, ApplicationUser user, TableType type)
        {
            if (requestInfo == null || user == null)
                return new GetTableOutModel() { Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("GetListForTable userID={0}, userName={1}, viewName={2}, filterID={3}, timezoneOffsetInMinutes={4}, startRecordIndex={5}, countRecords={6}",
                user.Id,
                user.UserName,
                requestInfo.ViewName,
                requestInfo.CurrentFilterID,
                requestInfo.TimezoneOffsetInMinutes,
                requestInfo.StartRecordIndex,
                requestInfo.CountRecords);
            //
            int tryCount = 3;
        tryAgain:
            try
            {
                var actionIdentifier = string.Format("GetListForTable_userID={0},listType={1},viewName={2}", user.Id, type, requestInfo.ViewName);
                if (requestInfo.IDList != null)
                    actionIdentifier += string.Format(", ids='{0}'", string.Join(",", requestInfo.IDList.Select(x => x.ToString())));
                using (var dataSource = DataSource.GetDataSource(actionIdentifier))
                {
                    var hasErrors = false;
                    try
                    {
                        dataSource.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                        //
                        if ((type == TableType.SD && CheckAccessForSD(requestInfo.ViewName, user, dataSource) == false)
                            || (type == TableType.Asset && CheckAccessForAsset(requestInfo.ViewName, user, dataSource) == false)
                            || (type == TableType.Finance && CheckAccessForFinance(requestInfo.ViewName, user, dataSource) == false))
                        {
                            Logger.Warning("GetListForTable userID={0}, userName={1}, viewName={2} failed (user access denied)", user.Id, user.UserName, requestInfo.ViewName);
                            return new GetTableOutModel() { Result = RequestResponceType.AccessError };
                        }
                        //
                        var columnSettings = BLL.Settings.ColumnSettings.TryGetOrCreate(user.User, requestInfo.ViewName, dataSource);
                        if (columnSettings == null)
                        {
                            Logger.Warning("GetListForTable userID={0}, userName={1}, viewName={2} failed (columnSettings absent)", user.Id, user.UserName, requestInfo.ViewName);
                            return new GetTableOutModel() { Result = RequestResponceType.BadParamsError };
                        }
                        //
                        columnSettings.ColumnsDTL.Sort((x, y) => x.Order.CompareTo(y.Order));
                        var sortColumn = columnSettings.ColumnsDTL.Where(x => x.SortAsc.HasValue).FirstOrDefault();
                        //Filtration settings
                        BLL.Settings.FilterSettings filterSettings = null;
                        BLL.Tables.Filters.Filter filter = null;
                        DTL.Settings.UserTreeSettings treeSettings = requestInfo.TreeSettings;
                        //
                        if (type != TableType.AssetSearch &&
                            type != TableType.ARSSearch && type != TableType.PurchaseSpecificationSearch &&
                            type != TableType.GoodsInvoiceSpecificationSearch &&
                            type != TableType.LocatedActivesSearch && type != TableType.frmFinanceBudgetRow_Table)
                        {
                            if (!requestInfo.CurrentFilterID.HasValue)//возможно остался мусор с прошлого редактирования, удаляем!
                                BLL.Settings.FilterSettings.DeleteTempSettings(user.User.ID, requestInfo.ViewName, dataSource);
                            else
                                filter = BLL.Tables.Filters.Filter.Get(requestInfo.CurrentFilterID.Value, user.User.ID, dataSource);
                            //
                            if (filter == null)
                            {
                                filterSettings = BLL.Settings.FilterSettings.Get(user.User.ID, requestInfo.ViewName, dataSource);// try get main current filter
                                                                                                                                 //                                                                                             
                                if (filterSettings != null)
                                {
                                    if (filterSettings.FilterID.HasValue)
                                        filter = BLL.Tables.Filters.Filter.Get(filterSettings.FilterID.Value, user.User.ID, dataSource);
                                }
                                else
                                {
                                    filterSettings = new BLL.Settings.FilterSettings(user.User.ID, null, requestInfo.ViewName, false, false, null);
                                    //filterSettings.Save(dataSource); needn't save with null FilterID!
                                }
                            }
                        }
                        //
                        if (type == TableType.SD)
                        {
                            var retval = GetDataForSD(requestInfo, user, ColumnParams.Create(sortColumn, columnSettings),
                                                      FilterParams.Create(filter,
                                                                          filterSettings != null ? filterSettings.WithFinishedWorkflow : requestInfo.WithFinishedWorkflow,
                                                                          filterSettings != null ? filterSettings.AfterUtcDateModified : BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(requestInfo.AfterModifiedMilliseconds),
                                                                          treeSettings), dataSource);
                            if (retval.ObjectIDList.Count == 0)
                                retval.ObjectIDList = null;
                            //
                            return retval;
                        }
                        else if (type == TableType.Asset)
                        {
                            var retval = GetDataForAsset(requestInfo, user, ColumnParams.Create(sortColumn, columnSettings),
                                                      FilterParams.Create(filter,
                                                                          filterSettings != null ? filterSettings.WithFinishedWorkflow : requestInfo.WithFinishedWorkflow,
                                                                          filterSettings != null ? filterSettings.AfterUtcDateModified : BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(requestInfo.AfterModifiedMilliseconds),
                                                                          treeSettings), dataSource);
                            if (retval.ObjectIDList.Count == 0)
                                retval.ObjectIDList = null;
                            if (retval.OperationInfoList.Count == 0)
                                retval.OperationInfoList = null;
                            //
                            return retval;
                        }
                        else if (type == TableType.Finance)
                        {
                            var retval = GetDataForFinance(requestInfo, user, ColumnParams.Create(sortColumn, columnSettings),
                                                      FilterParams.Create(filter,
                                                                          filterSettings != null ? filterSettings.WithFinishedWorkflow : requestInfo.WithFinishedWorkflow,
                                                                          filterSettings != null ? filterSettings.AfterUtcDateModified : BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(requestInfo.AfterModifiedMilliseconds),
                                                                          treeSettings), dataSource);
                            if (retval.ObjectIDList.Count == 0)
                                retval.ObjectIDList = null;
                            if (retval.OperationInfoList.Count == 0)
                                retval.OperationInfoList = null;
                            //
                            return retval;
                        }
                        else if (type == TableType.ARSSearch && requestInfo is FinanceSearchTableLoadRequestInfo)
                        {
                            var retval = GetDataForARSSearch(requestInfo as FinanceSearchTableLoadRequestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            if (retval.ObjectIDList.Count == 0)
                                retval.ObjectIDList = null;
                            //
                            return retval;
                        }
                        else if (type == TableType.GoodsInvoiceSpecificationSearch && requestInfo is AssetSearchTableLoadRequestInfo)
                        {
                            var retval = GetDataForGoodsInvoiceSpecificationSearch(requestInfo as AssetSearchTableLoadRequestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            if (retval.ObjectIDList.Count == 0)
                                retval.ObjectIDList = null;
                            //
                            return retval;
                        }
                        else if (type == TableType.frmFinanceBudgetRow_Table && requestInfo is frmFinanceBudgetRowTableRequestInfo)
                        {
                            var retval = GetDataForFrmFinanceBudgetRowTable(requestInfo as frmFinanceBudgetRowTableRequestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            if (retval.ObjectIDList.Count == 0)
                                retval.ObjectIDList = null;
                            //
                            return retval;
                        }
                        else if (type == TableType.PurchaseSpecificationSearch && requestInfo is AssetSearchTableLoadRequestInfo)
                        {
                            var retval = GetTableForPurchaseSpecification(requestInfo as AssetSearchTableLoadRequestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            if (retval.ObjectIDList.Count == 0)
                                retval.ObjectIDList = null;
                            return retval;
                        }
                        else return new GetTableOutModel() { Result = RequestResponceType.GlobalError };
                    }
                    catch (SqlException e)
                    {
                        hasErrors = true;
                        Logger.Trace("Ошибка получения данных таблицы. {0}", e.ToString());
                        //
                        if (e.Number == 0 || e.Number == 3980)//without error (ok, aborted)
                            return new GetTableOutModel() { Result = RequestResponceType.Success };
                        else if (e.Number == -2)// timeout
                            return new GetTableOutModel() { Result = RequestResponceType.Timeout };
                        throw;
                    }
                    catch (InvalidOperationException e)
                    {
                        Logger.Trace("Ошибка получения данных таблицы. {0}", e.ToString());
                        if (--tryCount > 0)
                            goto tryAgain;
                        else
                            throw;
                    }
                    catch (Exception ex)
                    {
                        var x = ex.Message;
                        hasErrors = true;
                        dataSource.RollbackTransaction();
                        throw;
                    }
                    finally
                    {
                        if (!hasErrors)
                            dataSource.CommitTransaction();
                    }
                }
            }
            catch (FiltrationException ex)
            {
                Logger.Error(ex, "Ошибка получения данных таблицы. Фильтры полетели.");
                return new GetTableOutModel() { Result = RequestResponceType.FiltrationError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения данных таблицы.");
                return new GetTableOutModel() { Result = RequestResponceType.GlobalError };
            }
        }

        public static ResultData<List<BaseForTable>> GetListForObject(TableLoadRequestInfo requestInfo, ApplicationUser user, TableType type)
        {
            if (requestInfo == null || user == null)
                return ResultData<List<BaseForTable>>.Create(RequestResponceType.NullParamsError);
            //
            Logger.Trace("GetListForObject userID={0}, userName={1}, viewName={2}, filterID={3}, timezoneOffsetInMinutes={4}, startRecordIndex={5}, countRecords={6}",
                user.Id,
                user.UserName,
                requestInfo.ViewName,
                requestInfo.CurrentFilterID,
                requestInfo.TimezoneOffsetInMinutes,
                requestInfo.StartRecordIndex,
                requestInfo.CountRecords); 
            //
            int tryCount = 3;
        tryAgain:
            try
            {
                var actionIdentifier = string.Format("GetListForObject_userID={0},listType={1},viewName={2}", user.Id, type, requestInfo.ViewName);
                if (requestInfo.IDList != null)
                    actionIdentifier += string.Format(", ids='{0}'", string.Join(",", requestInfo.IDList.Select(x => x.ToString())));
                using (var dataSource = DataSource.GetDataSource(actionIdentifier))
                {
                    var hasErrors = false;
                    try
                    {
                        dataSource.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                        //
                        if ((type == TableType.SD && CheckAccessForSD(requestInfo.ViewName, user, dataSource) == false)
                            || (type == TableType.Asset && CheckAccessForAsset(requestInfo.ViewName, user, dataSource) == false)
                            || (type == TableType.Finance && CheckAccessForFinance(requestInfo.ViewName, user, dataSource) == false))
                        {
                            Logger.Warning("GetListForObject userID={0}, userName={1}, viewName={2} failed (user access denied)", user.Id, user.UserName, requestInfo.ViewName);
                            return ResultData<List<BaseForTable>>.Create(RequestResponceType.AccessError);
                        }
                        //
                        var columnSettings = BLL.Settings.ColumnSettings.TryGetOrCreate(user.User, requestInfo.ViewName, dataSource);
                        if (columnSettings == null)
                        {
                            Logger.Warning("GetListForObject userID={0}, userName={1}, viewName={2} failed (columnSettings absent)", user.Id, user.UserName, requestInfo.ViewName);
                            return ResultData<List<BaseForTable>>.Create(RequestResponceType.BadParamsError);
                        }
                        //
                        columnSettings.ColumnsDTL.Sort((x, y) => x.Order.CompareTo(y.Order));
                        var sortColumn = columnSettings.ColumnsDTL.Where(x => x.SortAsc.HasValue).FirstOrDefault();
                        if (!(requestInfo.ViewName == CommonForTable.VIEW_NAME || requestInfo.ViewName == CallForTable.VIEW_NAME || requestInfo.ViewName == WorkOrderForTable.VIEW_NAME))
                        {
                            var ctrlSortColumn = columnSettings.ColumnsDTL.Where(x => x.CtrlSortAsc.HasValue).FirstOrDefault();
                            if (ctrlSortColumn != null && sortColumn != null)
                            {
                                if (sortColumn == null || sortColumn == ctrlSortColumn)
                                    sortColumn = ctrlSortColumn;
                                else
                                {
                                    string tmp = (bool)sortColumn.SortAsc ? "ASC" : "DESC";
                                    string tmpName = BLL.Settings.ColumnSettings.GetDatabaseColumnName(sortColumn.MemberName, requestInfo.ViewName) + "] " + tmp + " , [" + BLL.Settings.ColumnSettings.GetDatabaseColumnName(ctrlSortColumn.MemberName, requestInfo.ViewName);
                                    sortColumn.SortAsc = ctrlSortColumn.CtrlSortAsc;
                                    sortColumn.MemberName = tmpName;
                                    BLL.Settings.ColumnSettings.RemoveColumnSettingFromCache(user.User, requestInfo.ViewName);
                                }
                            }
                        }
                        //Filtration settings
                        BLL.Settings.FilterSettings filterSettings = null;
                        BLL.Tables.Filters.Filter filter = null;
                        DTL.Settings.UserTreeSettings treeSettings = requestInfo.TreeSettings;
                        //
                        if (type != TableType.AssetSearch && type != TableType.AssetModelSearch && type != TableType.ProcessorSimpleSearch &&
                            type != TableType.ContractAndAgreementSearch && type != TableType.PurchaseSpecificationSearch &&
                            type != TableType.GoodsInvoiceSpecificationSearch && type != TableType.ARS &&
                            type != TableType.LocatedActivesSearch && type != TableType.frmFinanceBudgetRow_Table &&
                            type != TableType.CallUsersList && type != TableType.AssetUsersList &&
                            type != TableType.RFCCategory && type != TableType.Deputy &&
                            type != TableType.DataEntityDependency && type != TableType.KBAUserList
                            /*&&*//*type != TableType.SDWithoutFilter*/)
                        {
                            if (!requestInfo.CurrentFilterID.HasValue)//возможно остался мусор с прошлого редактирования, удаляем!
                                BLL.Settings.FilterSettings.DeleteTempSettings(user.User.ID, requestInfo.ViewName, dataSource);
                            else
                                filter = BLL.Tables.Filters.Filter.Get(requestInfo.CurrentFilterID.Value, user.User.ID, dataSource);
                            //
                            if (filter == null)
                            {
                                filterSettings = BLL.Settings.FilterSettings.Get(user.User.ID, requestInfo.ViewName, dataSource);// try get main current filter
                                                                                                                                 //                                                                                             
                                if (filterSettings != null)
                                {
                                    if (filterSettings.FilterID.HasValue)
                                        filter = BLL.Tables.Filters.Filter.Get(filterSettings.FilterID.Value, user.User.ID, dataSource);
                                }
                                else
                                {
                                    filterSettings = new BLL.Settings.FilterSettings(user.User.ID, null, requestInfo.ViewName, false, false, null);
                                    //filterSettings.Save(dataSource); needn't save with null FilterID!
                                }
                            }
                        }
                        //
                        if (type == TableType.SD /*|| type == TableType.SDWithoutFilter*/)
                        {
                            var retval = GetSDList(requestInfo, user, ColumnParams.Create(sortColumn, columnSettings),
                                                      FilterParams.Create(filter,
                                                                          filterSettings != null ? filterSettings.WithFinishedWorkflow : requestInfo.WithFinishedWorkflow,
                                                                          filterSettings != null ? filterSettings.AfterUtcDateModified : BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(requestInfo.AfterModifiedMilliseconds),
                                                                          treeSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.Asset)
                        {
                            var retval = GetAssetList(requestInfo, user, ColumnParams.Create(sortColumn, columnSettings),
                                                      FilterParams.Create(filter,
                                                                          filterSettings != null ? filterSettings.WithFinishedWorkflow : requestInfo.WithFinishedWorkflow,
                                                                          filterSettings != null ? filterSettings.AfterUtcDateModified : BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(requestInfo.AfterModifiedMilliseconds),
                                                                          treeSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.CallUsersList)
                        {
                            var retval = GetCallFoUserSerach(requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.AssetUsersList)
                        {
                            var retval = GetAssetFoUserSerach(requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.ContractAgreement && requestInfo is frmTableLoadRequestInfo info)
                        {
                            var retval = GetDataForContractAgreement(info, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.ContractLicence && requestInfo is frmTableLoadRequestInfo)
                        {
                            var retval = GetDataForContractLicence((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.ContractLicenceAgreement && requestInfo is frmTableLoadRequestInfo)
                        {
                            var retval = GetDataForContractLicenceAgreement((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.ContractAssetMaintenance && requestInfo is frmTableLoadRequestInfo)
                        {
                            var retval = GetDataForAssetMaintenance((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.AgreementAssetMaintenance && requestInfo is frmTableLoadRequestInfo)
                        {
                            var retval = GetDataForAssetAgreementMaintenance((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.ContractLicenceMaintenance && requestInfo is frmTableLoadRequestInfo)
                        {
                            var retval = GetDataForLicenceMaintenance((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.AgreementLicenceMaintenance && requestInfo is frmTableLoadRequestInfo)
                        {
                            var retval = GetDataForLicenceAgreementMaintenance((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.LocatedActivesSearch && requestInfo is AssetSearchTableLoadRequestInfo)
                        {
                            var retval = GetDataForLocatedActivesSearch(requestInfo as AssetSearchTableLoadRequestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.Supplier)
                        {
                            var retval = GetDataForSupplier(requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.Deputy)
                        {
                            var retval = GetDataForDeputy((TableLoadRequestInfoDeputy)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.SoftwareModelCatalog)
                        {
                            var retval = GetSoftwareModelCatalog((TableLoadRequestInfoSoftwareModelCatalog)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), FilterParams.Create(filter,
                                                                          filterSettings != null ? filterSettings.WithFinishedWorkflow : requestInfo.WithFinishedWorkflow,
                                                                          filterSettings != null ? filterSettings.AfterUtcDateModified : BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(requestInfo.AfterModifiedMilliseconds),
                                                                          treeSettings), dataSource);
                            return retval;
                        }           
                        else if (type == TableType.SoftwareModelCatalogDependency)
                        {
                            var retval = GetSoftwareModelCatalogDependency((TableLoadRequestInfoSoftwareModelCatalog)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), FilterParams.Create(filter,
                                                                          filterSettings != null ? filterSettings.WithFinishedWorkflow : requestInfo.WithFinishedWorkflow,
                                                                          filterSettings != null ? filterSettings.AfterUtcDateModified : BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(requestInfo.AfterModifiedMilliseconds),
                                                                          treeSettings), dataSource);
                            return retval;
                        }     
                        else if (type == TableType.SoftwareModelRelated)
                        {
                            var retval = GetSoftwareModelRelated((TableLoadRequestInfoSoftwareModelCatalog)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), FilterParams.Create(filter,
                                                                          filterSettings != null ? filterSettings.WithFinishedWorkflow : requestInfo.WithFinishedWorkflow,
                                                                          filterSettings != null ? filterSettings.AfterUtcDateModified : BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(requestInfo.AfterModifiedMilliseconds),
                                                                          treeSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.SupplierContactPerson && requestInfo is frmTableLoadRequestInfo)
                        {
                            var retval = GetDataForSupplierContactPerson((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.SoftwareLicence)
                        {
                            var retval = GetDataForSoftwareLicence(requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.SoftModel)
                        {
                            var retval = GetDataForSoftModel(requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.AdapterReference)
                        {
                            var retval = GetDataForAdapterReference((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.PeripheralReference)
                        {
                            var retval = GetDataForPeripheralReference((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.Finance)
                        {
                            var retval = GetFinanceList(requestInfo, user, ColumnParams.Create(sortColumn, columnSettings),
                                                      FilterParams.Create(filter,
                                                                          filterSettings != null ? filterSettings.WithFinishedWorkflow : requestInfo.WithFinishedWorkflow,
                                                                          filterSettings != null ? filterSettings.AfterUtcDateModified : BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(requestInfo.AfterModifiedMilliseconds),
                                                                          treeSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.AssetModelSearch && requestInfo is AssetModelSearchTableLoadRequestInfo)
                        {
                            var retval = GetDataForAssetModelSearch(requestInfo as AssetModelSearchTableLoadRequestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.ARS && requestInfo is AssetSearchTableLoadRequestInfo)
                        {
                            var retval = GetDataForARS(requestInfo as AssetSearchTableLoadRequestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.ContractAndAgreementSearch && requestInfo is AssetModelSearchTableLoadRequestInfo)
                        {
                            var retval = GetContractAndAgreementSearch(requestInfo as AssetModelSearchTableLoadRequestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.PurchaseSpecificationSearch && requestInfo is AssetSearchTableLoadRequestInfo)
                        {
                            var retval = GetDataForPurchaseSpecification(requestInfo as AssetSearchTableLoadRequestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.SubdeviceList && requestInfo is SimpleRequestWithSearchAndNavigatorInfo)
                        {
                            var retval = GetDataForSubdeviceList(requestInfo as SimpleRequestWithSearchAndNavigatorInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.Users && requestInfo is SimpleRequestWithSearchAndNavigatorInfoAndSoftwareSubLicence)
                        {
                            var retval = GetDataForUsersList(requestInfo as SimpleRequestWithSearchAndNavigatorInfoAndSoftwareSubLicence, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.Equip && requestInfo is SimpleRequestWithSearchAndNavigatorInfoAndSoftwareSubLicence)
                        {
                            var retval = GetDataForEquipList(requestInfo as SimpleRequestWithSearchAndNavigatorInfoAndSoftwareSubLicence, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.AssetSearch && requestInfo is AssetSearchTableLoadRequestInfo)
                        {
                            var retval = GetDataForAssetSearch(requestInfo as AssetSearchTableLoadRequestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.DataEntityDependency)
                        {
                            var retval = GetDataForDataEntityDependency((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.ClusterVM)
                        {
                            var retval = GetClusterVMDependency((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.ClusterHosts)
                        {
                            var retval = GetClusterHostsDependency((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.InventorySpecification)
                        {
                            var retval = GetDataForInventorySpecification(requestInfo as AssetSearchTableLoadRequestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.SoftwareLicenceSerialNumber)
                        {
                            var retval = GetDataForSoftwareLicenceSerialNumber((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.SoftwareLicenceReference)
                        {
                            var retval = GetDataForSoftwareLicenceReference((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.SoftwareLicenceReferenceReturning)
                        {
                            var retval = GetDataForSoftwareLicenceReferenceReturning((frmTableLoadRequestInfoForPool)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.AdminTools)
                        {
                            var retval = GetDataForAdminTools(requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.OrganizationList && requestInfo is OrgStructureSearchTableLoadRequestInfo)
                        {
                            var retval = GetDataForOrganizationList((OrgStructureSearchTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.SubdivisionList && requestInfo is OrgStructureSearchTableLoadRequestInfo)
                        {
                            var retval = GetDataForSubdivisionList((OrgStructureSearchTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.SoftwareLicenceUpdate)
                        {
                            var retval = GetDataForSoftwareLicenceUpdate((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.SoftwareLicenceLocation)
                        {
                            var retval = GetDataForSoftwareLicenceLocation((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.SoftwareLicenceModel)
                        {
                            var retval = GetDataForSoftwareLicenceModel((AssetModelSearchTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.SDMobileSearch)
                        {
                            var retval = GetDataForMobileSearch((TableLoadRequestInfoMobileSearch)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.RFCCategory)
                        {
                            var retval = GetDataForRFCCategoryList((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.SDMobileCommonSearch)
                        {
                            var retval = GetDataForMobileCommonSearch((TableLoadRequestInfoMobileSearch)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.SoftwareModelUsingType)
                        {
                            var retval = GetDataForSoftwareModelUsingType((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.Manufacturer)
                        {
                            return GetDataForManufacturer((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                        }
                        else if (type == TableType.Synonym)
                        {
                            return GetDataForSynonym((SimpleRequestWithSearchInfoAndFilterSynonym)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                        }
                        else if (type == TableType.Positions)
                        {
                            return GetDataForPositions((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                        }
                        else if (type == TableType.Units)
                        {
                            return GetDataForUnits((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                        }
                        else if (type == TableType.Criticality)
                        {
                            return GetDataForCriticality((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                        }
                        else if (type == TableType.LicensingScheme)
                        {
                            var retval = GetDataForRFCCategoryList((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.Workflow)
                        {
                            var retval = GetDataWorkflowScheme((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.InfrastructureSegment)
                        {
                            var retval = GetDataForInfrastructureSegment((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.FileSystem)
                        {
                            var retval = GetDataFileSystem((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.CostCategory)
                        {
                            var retval = GetDataCostCategory((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.TelephoneType)
                        {
                            var retval = GetTelephoneType((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.TechnologyType)
                        {
                            var retval = GetTechnologyType((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.CartridgeType)
                        {
                            var retval = GetCartridgeType((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.ConnectorType)
                        {
                            var retval = GetConnectorType((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.SlotType)
                        {
                            var retval = GetSlotType((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }

                        else if (type == TableType.SoftwareModelUpdate)
                        {
                            var retval = GetSoftwareModelUpdateForTable((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.SoftwareModelInstallation)
                        {
                            var retval = GetSoftwareModelInstallationForTable((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }   
                        else if (type == TableType.SoftwareModelPackageContents)
                        {
                            var retval = GetSoftwareModelPackageContentsForTable((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }       
                        else if (type == TableType.SynonymsSoftwareModelForTable)
                        {
                            var retval = GetSoftwareSynonymsModelForTable((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.SoftwareModelProcessNameList)
                        {
                            var retval = GetSoftwareModelProcessNameList((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.SoftwareModelСomponent)
                        {
                            var retval = GetSoftwareModelСomponentForTable((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.SoftwareModelLicenses)
                        {
                            var retval = GetSoftwareModelLicensesForTable((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.LogicObjectComponents)
                        {
                            var retval = GetLogicObjectComponentsForTable((frmTableLoadRequestInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.Notification)
                        {
                            var retval = GetNotificationForTable((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.ProblemTypeNew)
                        {
                            var retval = GetProblemTypeNewForTable((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.CalendarHoliday)
                        {
                            var retval = GetDataCalendarHoliday((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.CalendarWeekend)
                        {
                            var retval = GetDataCalendarWeekend((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if(type == TableType.FormBuilder)
                        {
                            var retval = GetDataForlBuilderForm((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.KBAUserList)
                        {
                            var retval = GetDataForKBAUserLst(requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if(type == TableType.Exclusion)
                        {
                            var retval = GetDataForExclusuion((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.ParameterEnum)
                        {
                            var retval = GetDataParameterEnum((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else if (type == TableType.CalendarWorkSchedule)
                        {
                            var retval = GetDataCalendarWorkSchedule((SimpleRequestWithSearchInfo)requestInfo, user, ColumnParams.Create(sortColumn, columnSettings), dataSource);
                            return retval;
                        }
                        else return ResultData<List<BaseForTable>>.Create(RequestResponceType.GlobalError);
                    }
                    catch (SqlException e)
                    {
                        hasErrors = true;
                        Logger.Trace("Ошибка получения данных таблицы. {0}", e.ToString());
                        //
                        if (e.Number == 0 || e.Number == 3980)//without error (ok, aborted)
                            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success);
                        else if (e.Number == -2)// timeout
                            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Timeout);
                        throw;
                    }
                    catch (InvalidOperationException e)
                    {
                        Logger.Trace("Ошибка получения данных таблицы. {0}", e.ToString());
                        if (--tryCount > 0)
                            goto tryAgain;
                        else
                            throw;
                    }
                    catch (Exception ex)
                    {
                        var x = ex.Message;
                        hasErrors = true;
                        dataSource.RollbackTransaction();
                        throw;
                    }
                    finally
                    {
                        if (!hasErrors)
                            dataSource.CommitTransaction();
                    }
                }
            }
            catch (FiltrationException ex)
            {
                Logger.Error(ex, "Ошибка получения данных таблицы. Фильтры полетели.");
                return ResultData<List<BaseForTable>>.Create(RequestResponceType.FiltrationError);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения данных таблицы.");
                return ResultData<List<BaseForTable>>.Create(RequestResponceType.GlobalError);
            }
        }

        private static bool CheckAccessForSD(String viewName, ApplicationUser user, DataSource dataSource)
        {
            var userSettings = BLL.Settings.UserSettings.TryGetOrCreate(user.User, user.DefaultUserCulture, dataSource);
            //!
            if (userSettings == null || (!user.User.HasRoles &&
                (viewName == CommonForTable.VIEW_NAME ||
                viewName == CallForTable.VIEW_NAME ||
                viewName == ProblemForTable.VIEW_NAME ||
                viewName == WorkOrderForTable.VIEW_NAME)))//guard
                return false;
            else return true;
        }

        private static bool CheckAccessForAsset(String viewName, ApplicationUser user, DataSource dataSource)
        {
            //TODO
            if (viewName == SoftwareLicenceForTable.VIEW_NAME)
            {
                return user.User.OperationIsGranted(IMSystem.Global.OPERATION_SOFTWARELICENCE_PROPERTIES);
            }
            return true;
        }

        private static bool CheckAccessForFinance(String viewName, ApplicationUser user, DataSource dataSource)
        {
            //TODO
            return true;
        }

        private static ResultData<List<BaseForTable>> GetDataForAdminTools(TableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            List<BaseForTable> objectList = null;
            //
            var sortColumn = columnParams.Column;
            var columnSettings = columnParams.ColumnSettings;
            //
            String sqlQueryFilter = String.Empty;
            //
            switch (requestInfo.ViewName)
            {
                case BLL.Users.Session.VIEW_NAME:
                    {
                        var list = BLL.Users.Session.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case BLL.Users.SessionHistory.VIEW_NAME:
                    {
                        var list = BLL.Users.SessionHistory.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                //
                default:
                    throw new NotSupportedException();
            }
            //
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        //don't touch, used int customers mobile application
        private static GetTableOutModel GetDataForSD(TableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, FilterParams filterParams, DataSource dataSource)
        {
            List<ColumnWithData> columnWithDataList = null;
            List<Guid> idList = new List<Guid>();
            List<Guid> objectIdList = new List<Guid>(); //смотри описание в модели
            List<int> classIdList = new List<int>();
            List<OperationInfo> operationInfoList = new List<OperationInfo>();
            //
            var sortColumn = columnParams.Column;
            var columnSettings = columnParams.ColumnSettings;
            //
            String sqlQueryFilter = String.Empty;
            //
            switch (requestInfo.ViewName)
            {
                case ClientCallForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<ClientCallForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<ClientCallForTable>(user.User.ID, filterParams.Filter.Elements, dataSource);
                        var list = ClientCallForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, filterParams.WithFinishedWorkflow, filterParams.AfterUtcDateModified, dataSource);
                        foreach (var e in list)
                        {
                            idList.Add(e.ID);
                            classIdList.Add(e.ClassID);
                            operationInfoList.Add(new OperationInfo()
                            {
                                ObjectName = OperationInfo.GetObjectName(e.ClassID, e.Number),
                                InControl = e.InControl,
                                OwnerID = e.OwnerID,
                                CanBePicked = e.CanBePicked,
                                NoteInfo = new NoteInfo(e.UnreadMessageCount, e.NoteCount, e.MessageCount),
                            });
                        }
                        columnWithDataList = GetListForTablePrivate<ClientCallForTable>(columnSettings.ColumnsDTL, list, requestInfo.TimezoneOffsetInMinutes);
                    }
                    break;
                case CommonWithNegotiationsForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<CommonWithNegotiationsForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<CommonWithNegotiationsForTable>(user.User.ID, filterParams.Filter.Elements, dataSource);
                        var list = CommonWithNegotiationsForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, filterParams.WithFinishedWorkflow, filterParams.AfterUtcDateModified, dataSource);
                        foreach (var e in list)
                        {
                            idList.Add(e.ID);
                            objectIdList.Add(e.ObjectID);
                            classIdList.Add(e.ClassID);
                        }
                        columnWithDataList = GetListForTablePrivate<CommonWithNegotiationsForTable>(columnSettings.ColumnsDTL, list, requestInfo.TimezoneOffsetInMinutes);
                    }
                    break;
                //
                case CommonForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<CommonForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<CommonForTable>(user.User.ID, filterParams.Filter.Elements, dataSource);
                        var list = CommonForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, filterParams.WithFinishedWorkflow, filterParams.AfterUtcDateModified, dataSource);
                        foreach (var e in list)
                        {
                            idList.Add(e.ID);
                            classIdList.Add(e.ClassID);
                        }
                        columnWithDataList = GetListForTablePrivate<CommonForTable>(columnSettings.ColumnsDTL, list, requestInfo.TimezoneOffsetInMinutes);
                    }
                    break;
                case CallForTable.VIEW_NAME:
                    {
                        if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_SD_General_Calls_View))
                            return new GetTableOutModel() { Result = RequestResponceType.OperationError };
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<CallForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<CallForTable>(user.User.ID, filterParams.Filter.Elements, dataSource);
                        var list = CallForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, filterParams.WithFinishedWorkflow, filterParams.AfterUtcDateModified, dataSource);
                        foreach (var e in list)
                        {
                            idList.Add(e.ID);
                            classIdList.Add(e.ClassID);
                        }
                        columnWithDataList = GetListForTablePrivate<CallForTable>(columnSettings.ColumnsDTL, list, requestInfo.TimezoneOffsetInMinutes);
                    }
                    break;
                case WorkOrderForTable.VIEW_NAME:
                    {
                        if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_SD_General_WorkOrders_View))
                            return new GetTableOutModel() { Result = RequestResponceType.OperationError };
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<WorkOrderForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<WorkOrderForTable>(user.User.ID, filterParams.Filter.Elements, dataSource);
                        var list = WorkOrderForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, filterParams.WithFinishedWorkflow, filterParams.AfterUtcDateModified, dataSource);
                        foreach (var e in list)
                        {
                            idList.Add(e.ID);
                            classIdList.Add(e.ClassID);
                        }
                        columnWithDataList = GetListForTablePrivate<WorkOrderForTable>(columnSettings.ColumnsDTL, list, requestInfo.TimezoneOffsetInMinutes);
                    }
                    break;
                case ProblemForTable.VIEW_NAME:
                    {
                        if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_SD_General_Problems_View))
                            return new GetTableOutModel() { Result = RequestResponceType.OperationError };
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<ProblemForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<ProblemForTable>(user.User.ID, filterParams.Filter.Elements, dataSource);
                        var list = ProblemForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, filterParams.WithFinishedWorkflow, filterParams.AfterUtcDateModified, dataSource);
                        foreach (var e in list)
                        {
                            idList.Add(e.ID);
                            classIdList.Add(e.ClassID);
                            operationInfoList.Add(new OperationInfo()
                            {
                                ObjectName = OperationInfo.GetObjectName(e.ClassID, e.Number),
                                InControl = e.InControl,
                                OwnerID = e.OwnerID,
                                CanBePicked = false,
                                NoteInfo = new NoteInfo(e.UnreadMessageCount, e.NoteCount, e.MessageCount),
                            });
                        }
                        columnWithDataList = GetListForTablePrivate<ProblemForTable>(columnSettings.ColumnsDTL, list, requestInfo.TimezoneOffsetInMinutes);
                    }
                    break;
                case CustomControlForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<CustomControlForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<CustomControlForTable>(user.User.ID, filterParams.Filter.Elements, dataSource);
                        var list = CustomControlForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, filterParams.WithFinishedWorkflow, filterParams.AfterUtcDateModified, dataSource);
                        foreach (var e in list)
                        {
                            idList.Add(e.ID);
                            classIdList.Add(e.ClassID);
                        }
                        columnWithDataList = GetListForTablePrivate<CustomControlForTable>(columnSettings.ColumnsDTL, list, requestInfo.TimezoneOffsetInMinutes);
                    }
                    break;

                //
                default:
                    throw new NotSupportedException();
            }
            //
            return new GetTableOutModel() { IDList = idList, ObjectIDList = objectIdList, ClassIDList = classIdList, DataList = columnWithDataList, OperationInfoList = operationInfoList, Result = RequestResponceType.Success };
        }
        private static ResultData<List<BaseForTable>> GetSDList(TableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, FilterParams filterParams, DataSource dataSource)
        {
            List<BaseForTable> objectList = null;
            //
            var sortColumn = columnParams.Column;
            var columnSettings = columnParams.ColumnSettings;
            //
            String sqlQueryFilter = String.Empty;
            //
            switch (requestInfo.ViewName)
            {
                case ClientCallForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<ClientCallForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<ClientCallForTable>(user.User.ID, filterParams.Filter.Elements, dataSource);
                        var list = ClientCallForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, filterParams.WithFinishedWorkflow, filterParams.AfterUtcDateModified, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case CommonWithNegotiationsForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<CommonWithNegotiationsForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<CommonWithNegotiationsForTable>(user.User.ID, filterParams.Filter.Elements, dataSource);
                        var list = CommonWithNegotiationsForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, filterParams.WithFinishedWorkflow, filterParams.AfterUtcDateModified, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                //
                case CommonForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<CommonForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<CommonForTable>(user.User.ID, filterParams.Filter.Elements, dataSource);
                        var list = CommonForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, filterParams.WithFinishedWorkflow, filterParams.AfterUtcDateModified, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case CallForTable.VIEW_NAME:
                    {
                        if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_SD_General_Calls_View))
                            return ResultData<List<BaseForTable>>.Create(RequestResponceType.OperationError);
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<CallForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<CallForTable>(user.User.ID, filterParams.Filter.Elements, dataSource);
                        var list = CallForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, filterParams.WithFinishedWorkflow, filterParams.AfterUtcDateModified, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case LiteCallForTable.VIEW_NAME:
                    {
                        //if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_SD_General_Calls_View))
                        //    return ResultData<List<BaseForTable>>.Create(RequestResponceType.OperationError);
                        //if (filterParams.Filter != null)
                        //    if (filterParams.Filter.Standart)
                        //        sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<CallForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName);
                        //    else
                        //        sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<CallForTable>(user.User.ID, filterParams.Filter.Elements, dataSource);
                        var list = LiteCallForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, filterParams.WithFinishedWorkflow, filterParams.AfterUtcDateModified, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case WorkOrderForTable.VIEW_NAME:
                    {
                        if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_SD_General_WorkOrders_View))
                            return ResultData<List<BaseForTable>>.Create(RequestResponceType.OperationError);
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<WorkOrderForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<WorkOrderForTable>(user.User.ID, filterParams.Filter.Elements, dataSource);
                        var list = WorkOrderForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, filterParams.WithFinishedWorkflow, filterParams.AfterUtcDateModified, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case ProblemForTable.VIEW_NAME:
                    {
                        if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_SD_General_Problems_View))
                            return ResultData<List<BaseForTable>>.Create(RequestResponceType.OperationError);
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<ProblemForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<ProblemForTable>(user.User.ID, filterParams.Filter.Elements, dataSource);
                        var list = ProblemForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, filterParams.WithFinishedWorkflow, filterParams.AfterUtcDateModified, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case CustomControlForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<CustomControlForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<CustomControlForTable>(user.User.ID, filterParams.Filter.Elements, dataSource);
                        var list = CustomControlForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, filterParams.WithFinishedWorkflow, filterParams.AfterUtcDateModified, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case RFCForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<RFCForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<RFCForTable>(user.User.ID, filterParams.Filter.Elements, dataSource);
                        var list = RFCForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, filterParams.WithFinishedWorkflow, filterParams.AfterUtcDateModified, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;

                //RFCForTable
                default:
                    throw new NotSupportedException();
            }
            //
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }
        //
        private static ResultData<List<BaseForTable>> GetAssetList(TableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, FilterParams filterParams, DataSource dataSource)
        {
            List<BaseForTable> objectList = null;
            //
            var sortColumn = columnParams.Column;
            var columnSettings = columnParams.ColumnSettings;
            var treeSettings = filterParams.TreeSettings;
            //
            if (treeSettings == null)
            {
                var userSettings = BLL.Settings.UserSettings.TryGetOrCreate(user.User, user.DefaultUserCulture, dataSource);
                if (userSettings != null)
                    treeSettings = userSettings.DTLTree;
            }
            //
            String sqlQueryFilter = String.Empty;
            //
            switch (requestInfo.ViewName)
            {
                case AssetNumberForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<AssetNumberForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<AssetNumberForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = AssetNumberForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case RepairForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<RepairForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<RepairForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = RepairForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case WrittenOffForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<WrittenOffForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<WrittenOffForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = WrittenOffForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case UtilizerCompleteForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<UtilizerCompleteForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<UtilizerCompleteForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = UtilizerCompleteForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case ContractForTable.VIEW_NAME:
                    {
                        int[] ContractsClassID = { 0, 374, 378, 182 };//ClassID для контрактов
                        if (!ContractsClassID.Contains(filterParams.TreeSettings.FiltrationObjectClassID) && filterParams.TreeSettings.FiltrationTreeType == 2)
                        {
                            break;
                        }
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<ContractForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<ContractForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = ContractForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case InventoryForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<InventoryForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<InventoryForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = InventoryForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, requestInfo.WithFinishedWorkflow, BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(requestInfo.AfterModifiedMilliseconds), dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case ConfigurationUnitForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<ConfigurationUnitForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<ConfigurationUnitForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = ConfigurationUnitForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, requestInfo.WithFinishedWorkflow, BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(requestInfo.AfterModifiedMilliseconds), dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case ClusterForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<ClusterForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<ClusterForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = ClusterForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, requestInfo.WithFinishedWorkflow, BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(requestInfo.AfterModifiedMilliseconds), dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case LogicalObjectForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<LogicalObjectForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<LogicalObjectForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = LogicalObjectForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, requestInfo.WithFinishedWorkflow, BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(requestInfo.AfterModifiedMilliseconds), dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case DataEntityForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<DataEntityForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<DataEntityForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = DataEntityForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, requestInfo.WithFinishedWorkflow, BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(requestInfo.AfterModifiedMilliseconds), dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case SoftwareLicenceForTable.VIEW_NAME:
                    {
                        int[] SoftwareLicenceClassID = { 0, 374, 378 };//ClassID для лицензий
                        if (filterParams.TreeSettings.FiltrationTreeType == 2)
                            if (!SoftwareLicenceClassID.Contains(filterParams.TreeSettings.FiltrationObjectClassID))
                                break;
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<SoftwareLicenceForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<SoftwareLicenceForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = SoftwareLicenceForTable.GetList(sortColumn, new AssetModelSearchTableLoadRequestInfo(requestInfo), user.User, sqlQueryFilter, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case SoftwareLicenseDistributionForTable.VIEW_NAME:
                    {
                        int[] SoftwareLicenceClassID =
                        {
                        0,
                        Global.OBJ_SOFTWARE_DISTRIBUTION_CENTRE,
                        374,
                        378
                    };//ClassID для лицензий
                        if (filterParams.TreeSettings.FiltrationTreeType == 2)
                            if (!SoftwareLicenceClassID.Contains(filterParams.TreeSettings.FiltrationObjectClassID))
                                break;
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<SoftwareLicenseDistributionForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<SoftwareLicenseDistributionForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = SoftwareLicenseDistributionForTable.GetList(
                        sortColumn,
                        new AssetModelSearchTableLoadRequestInfo(requestInfo),
                        user.User,
                        sqlQueryFilter,
                        dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case SubSoftwareLicenseForTable.VIEW_NAME:
                    {
                        int[] SoftwareLicenceClassID = { 0, 374, 378 };//ClassID для лицензий
                        if (filterParams.TreeSettings.FiltrationTreeType == 2)
                            if (!SoftwareLicenceClassID.Contains(filterParams.TreeSettings.FiltrationObjectClassID))
                                break;
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<SubSoftwareLicenseForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<SubSoftwareLicenseForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = SubSoftwareLicenseForTable.GetList(sortColumn, new AssetModelSearchTableLoadRequestInfo(requestInfo), user.User,
                            sqlQueryFilter, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case SoftwareSublicenseReferenceForTable.VIEW_NAME:
                    {
                        objectList = requestInfo.ParentID.HasValue
                                ? SoftwareSublicenseReferenceForTable
                                    .GetSublicenseReferences(
                                        IM.BusinessLayer.User.Get(user.User.ID),
                                        requestInfo.ParentID.Value,
                                        sortColumn?.MemberName,
                                        sortColumn?.SortAsc ?? true)
                                    .Cast<BaseForTable>()
                                    .ToList()
                                : SoftwareSublicenseReferenceForTable
                                    .GetSublicenseReferences(
                                        user.User.ID,
                                        requestInfo.SoftwarePoolSettings.SoftwareDistributionCentreID,
                                        requestInfo.SoftwarePoolSettings.SoftwareModelID,
                                        requestInfo.SoftwarePoolSettings.SoftwareLicenceScheme,
                                        requestInfo.SoftwarePoolSettings.Type,
                                        sortColumn?.MemberName,
                                        sortColumn?.SortAsc ?? true)
                                    .Cast<BaseForTable>()
                                    .ToList();
                    }
                    break;
                case SoftwareSublicenceForTable.VIEW_NAME:
                    {
                        var tableRequest = new AssetModelSearchTableLoadRequestInfo(requestInfo);
                        objectList =
                            SoftwareSublicenceForTable
                                .GetList(
                                    requestInfo.ParentID.Value,
                                    user.User.ID,
                                    tableRequest.HasLifeCycle,
                                    null,
                                    requestInfo.StartRecordIndex,
                                    requestInfo.CountRecords,
                                    sortColumn?.MemberName,
                                    sortColumn?.SortAsc ?? true
                                    )
                                .Cast<BaseForTable>()
                                .ToList();
                    }
                    break;
                case SoftwareSublicenseReferencePoolForTable.VIEW_NAME:
                    {
                        objectList = SoftwareSublicenseReferencePoolForTable
                            .GetSublicenseReferencesByPool(
                                requestInfo.SoftwarePoolSettings.SoftwareDistributionCentreID,
                                requestInfo.SoftwarePoolSettings.SoftwareLicenceScheme,
                                requestInfo.SoftwarePoolSettings.SoftwareModelID,
                                requestInfo.SoftwarePoolSettings.Type,
                                sortColumn?.MemberName,
                                sortColumn?.SortAsc ?? true)
                            .Cast<BaseForTable>()
                            .ToList();
                    }
                    break;
                //
                default:
                    throw new NotSupportedException();
            }
            //
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }
        //используется в мобильном приложении?
        private static GetTableOutModel GetDataForAsset(TableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, FilterParams filterParams, DataSource dataSource)
        {
            List<ColumnWithData> columnWithDataList = null;
            List<Guid> idList = new List<Guid>();
            List<Guid> objectIdList = new List<Guid>(); //смотри описание в модели
            List<int> classIdList = new List<int>();
            List<OperationInfo> operationInfoList = new List<OperationInfo>();
            //
            var sortColumn = columnParams.Column;
            var columnSettings = columnParams.ColumnSettings;
            var treeSettings = filterParams.TreeSettings;
            //
            if (treeSettings == null)
            {
                var userSettings = BLL.Settings.UserSettings.TryGetOrCreate(user.User, user.DefaultUserCulture, dataSource);
                if (userSettings != null)
                    treeSettings = userSettings.DTLTree;
            }
            //
            String sqlQueryFilter = String.Empty;
            //
            switch (requestInfo.ViewName)
            {
                case AssetNumberForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<AssetNumberForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<AssetNumberForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = AssetNumberForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
                        foreach (var e in list)
                        {
                            idList.Add(e.ID);
                            classIdList.Add(e.ClassID);
                            operationInfoList.Add(new OperationInfo()
                            {
                                ObjectName = Asset.GetObjectName(e.TypeName, e.ModelName, e.Name, e.InvNumber),
                                OwnerClassID = e.OwnerClassID,
                                OwnerID = e.OwnerID,
                                UserID = e.UserID,
                                LifeCycleStateID = e.LifeCycleStateID,
                            });
                        }
                        columnWithDataList = GetListForTablePrivate<AssetNumberForTable>(columnSettings.ColumnsDTL, list, requestInfo.TimezoneOffsetInMinutes);
                    }
                    break;
                case RepairForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<RepairForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<RepairForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = RepairForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
                        foreach (var e in list)
                        {
                            idList.Add(e.DeviceID);
                            classIdList.Add(e.ClassID);
                            operationInfoList.Add(new OperationInfo()
                            {
                                ObjectName = Asset.GetObjectName(e.TypeName, e.ModelName, e.Name, e.InventoryNumber),
                                OwnerClassID = e.OwnerClassID,
                                OwnerID = e.OwnerID,
                                UserID = e.UserID,
                                LifeCycleStateID = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                            });
                        }
                        columnWithDataList = GetListForTablePrivate<RepairForTable>(columnSettings.ColumnsDTL, list, requestInfo.TimezoneOffsetInMinutes);
                    }
                    break;
                case WrittenOffForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<WrittenOffForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<WrittenOffForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = WrittenOffForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
                        foreach (var e in list)
                        {
                            idList.Add(e.ID);
                            classIdList.Add(e.ClassID);
                            operationInfoList.Add(new OperationInfo()
                            {
                                ObjectName = Asset.GetObjectName(e.TypeName, e.ModelName, e.DeviceName, e.InvNumber),
                                OwnerClassID = e.OwnerClassID,
                                OwnerID = e.OwnerID,
                                UserID = e.UserID,
                                LifeCycleStateID = e.LifeCycleStateID,
                            });
                        }
                        columnWithDataList = GetListForTablePrivate<WrittenOffForTable>(columnSettings.ColumnsDTL, list, requestInfo.TimezoneOffsetInMinutes);
                    }
                    break;
                //case UtilizerForTable.VIEW_NAME:
                //    {
                //        if (filterParams.Filter != null)
                //            if (filterParams.Filter.Standart)
                //                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<UtilizerForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                //            else
                //                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<UtilizerForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                //        var list = UtilizerForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
                //        foreach (var e in list)
                //        {
                //            idList.Add(e.ID);
                //            classIdList.Add(e.ClassID);
                //            operationInfoList.Add(new OperationInfo()
                //            {
                //                ObjectName = GetObjectName(e.TypeName, e.ModelName, e.Name, e.InvNumber),
                //                OwnerClassID = e.OwnerClassID,
                //                OwnerID = e.OwnerID,
                //                UserID = e.UserID,
                //                LifeCycleStateID = e.LifeCycleStateID,
                //            });
                //        }
                //        columnWithDataList = GetListForTablePrivate<UtilizerForTable>(columnSettings.ColumnsDTL, list, requestInfo.TimezoneOffsetInMinutes);
                //    }
                //    break;
                case UtilizerCompleteForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<UtilizerCompleteForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<UtilizerCompleteForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = UtilizerCompleteForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
                        foreach (var e in list)
                        {
                            idList.Add(e.ID);
                            classIdList.Add(e.ClassID);
                            operationInfoList.Add(new OperationInfo()
                            {
                                ObjectName = Asset.GetObjectName(e.TypeName, e.ModelName, e.Name, e.InventoryNumber),
                                OwnerClassID = e.OwnerClassID,
                                OwnerID = e.OwnerID,
                                UserID = e.UserID,
                                LifeCycleStateID = e.LifeCycleStateID,
                            });
                        }
                        columnWithDataList = GetListForTablePrivate<UtilizerCompleteForTable>(columnSettings.ColumnsDTL, list, requestInfo.TimezoneOffsetInMinutes);
                    }
                    break;
                //
                default:
                    throw new NotSupportedException();
            }
            //
            return new GetTableOutModel() { IDList = idList, ObjectIDList = objectIdList, ClassIDList = classIdList, DataList = columnWithDataList, OperationInfoList = operationInfoList, Result = RequestResponceType.Success };
        }

        private static GetTableOutModel GetDataForFinance(TableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, FilterParams filterParams, DataSource dataSource)
        {
            List<ColumnWithData> columnWithDataList = null;
            List<Guid> idList = new List<Guid>();
            List<Guid> objectIdList = new List<Guid>(); //смотри описание в модели
            List<int> classIdList = new List<int>();
            List<OperationInfo> operationInfoList = new List<OperationInfo>();
            //
            var sortColumn = columnParams.Column;
            var columnSettings = columnParams.ColumnSettings;
            var treeSettings = filterParams.TreeSettings;
            //
            if (treeSettings == null)
            {
                var userSettings = BLL.Settings.UserSettings.TryGetOrCreate(user.User, user.DefaultUserCulture, dataSource);
                if (userSettings != null)
                    treeSettings = userSettings.DTLTree;
            }
            //
            String sqlQueryFilter = String.Empty;
            //
            switch (requestInfo.ViewName)
            {
                case FinanceActivesRequestForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<FinanceActivesRequestForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<FinanceActivesRequestForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = FinanceActivesRequestForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
                        foreach (var e in list)
                        {
                            idList.Add(e.ID);
                            classIdList.Add(e.ClassID);
                            operationInfoList.Add(new OperationInfo()
                            {
                                ObjectName = OperationInfo.GetObjectName(e.ClassID, e.Number),
                                OwnerID = e.CustomerID,

                            });
                        }
                        columnWithDataList = GetListForTablePrivate<FinanceActivesRequestForTable>(columnSettings.ColumnsDTL, list, requestInfo.TimezoneOffsetInMinutes);
                    }
                    break;
                case FinancePurchaseForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<FinancePurchaseForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<FinancePurchaseForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = FinancePurchaseForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
                        foreach (var e in list)
                        {
                            idList.Add(e.ID);
                            classIdList.Add(e.ClassID);
                            operationInfoList.Add(new OperationInfo()
                            {
                                ObjectName = OperationInfo.GetObjectName(e.ClassID, e.Number),
                                OwnerID = e.AssignorID,
                                NoteInfo = new NoteInfo(e.UnreadMessageCount, e.NoteCount, e.MessageCount)
                            });
                        }
                        columnWithDataList = GetListForTablePrivate<FinancePurchaseForTable>(columnSettings.ColumnsDTL, list, requestInfo.TimezoneOffsetInMinutes);
                    }
                    break;
                case FinanceBudgetRowForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<FinanceBudgetRowForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<FinanceBudgetRowForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = FinanceBudgetRowForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
                        foreach (var e in list)
                        {
                            idList.Add(e.ID);
                            classIdList.Add(e.ClassID);
                            operationInfoList.Add(new OperationInfo()
                            {
                                ObjectName = string.Format("{0}: {1}, {3} [{2}]", Resources.FinanceBudgetRow, e.Identifier, e.ReasonObjectFullName, e.ProductFullName)
                            });
                        }
                        columnWithDataList = GetListForTablePrivate<FinanceBudgetRowForTable>(columnSettings.ColumnsDTL, list, requestInfo.TimezoneOffsetInMinutes);
                    }
                    break;
                case FinanceBudgetForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<FinanceBudgetForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<FinanceBudgetForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = FinanceBudgetForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
                        foreach (var e in list)
                        {
                            idList.Add(e.ID);
                            classIdList.Add(e.ClassID);
                            operationInfoList.Add(new OperationInfo()
                            {
                                ObjectName = string.Format("{0}: {1}", Resources.FinanceBudget, e.FullName)
                            });
                        }
                        columnWithDataList = GetListForTablePrivate<FinanceBudgetForTable>(columnSettings.ColumnsDTL, list, requestInfo.TimezoneOffsetInMinutes);
                    }
                    break;
                //
                default:
                    throw new NotSupportedException();
            }
            //
            return new GetTableOutModel() { IDList = idList, ObjectIDList = objectIdList, ClassIDList = classIdList, DataList = columnWithDataList, OperationInfoList = operationInfoList, Result = RequestResponceType.Success };
        }
        private static ResultData<List<BaseForTable>> GetFinanceList(TableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, FilterParams filterParams, DataSource dataSource)
        {
            List<BaseForTable> objectList = null;
            //
            var sortColumn = columnParams.Column;
            var columnSettings = columnParams.ColumnSettings;
            //
            String sqlQueryFilter = String.Empty;
            //           
            var treeSettings = filterParams.TreeSettings;
            if (treeSettings == null)
            {
                var userSettings = BLL.Settings.UserSettings.TryGetOrCreate(user.User, user.DefaultUserCulture, dataSource);
                if (userSettings != null)
                    treeSettings = userSettings.DTLTree;
            }
            //
            switch (requestInfo.ViewName)
            {
                case FinanceActivesRequestForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<FinanceActivesRequestForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<FinanceActivesRequestForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = FinanceActivesRequestForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case FinancePurchaseForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<FinancePurchaseForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<FinancePurchaseForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = FinancePurchaseForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case FinanceBudgetRowForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<FinanceBudgetRowForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<FinanceBudgetRowForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = FinanceBudgetRowForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                case FinanceBudgetForTable.VIEW_NAME:
                    {
                        if (filterParams.Filter != null)
                            if (filterParams.Filter.Standart)
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<FinanceBudgetForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                            else
                                sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<FinanceBudgetForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
                        var list = FinanceBudgetForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
                        objectList = list.Cast<BaseForTable>().ToList();
                    }
                    break;
                //
                default:
                    throw new NotSupportedException();
            }
            //
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForAssetModelSearch(AssetModelSearchTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = AssetModelSearchAsTable.GetList(sortColumn, requestInfo, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }
        private static ResultData<List<BaseForTable>> GetContractAndAgreementSearch(AssetModelSearchTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = AssetModelSearchAsTable.GetContractList(sortColumn, requestInfo, user.User, requestInfo.ActiveRequest == true ? true : (bool?)null, requestInfo.ActiveRequest == false ? true : (bool?)null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }


        private static GetTableOutModel GetDataForARSSearch(FinanceSearchTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            List<ColumnWithData> columnWithDataList = null;
            List<Guid> idList = new List<Guid>();
            List<Guid> objectIdList = new List<Guid>(); //смотри описание в модели
            List<int> classIdList = new List<int>();
            List<OperationInfo> operationInfoList = new List<OperationInfo>();
            //
            var sortColumn = columnParams.Column;
            var columnSettings = columnParams.ColumnSettings;
            //
            var list = ActivesRequestSpecificationSearchAsTable.GetList(sortColumn, requestInfo, user.User, dataSource);
            foreach (var e in list)
            {
                idList.Add(e.ID);
                classIdList.Add(e.ClassID);
                operationInfoList.Add(new OperationInfo()
                {
                    ObjectName = e.ProductCatalogModelFullName,
                    OwnerID = user.User.ID
                });
            }
            columnWithDataList = GetListForTablePrivate<ActivesRequestSpecificationSearchAsTable>(columnSettings.ColumnsDTL, list, requestInfo.TimezoneOffsetInMinutes);
            //
            return new GetTableOutModel() { IDList = idList, ObjectIDList = objectIdList, ClassIDList = classIdList, DataList = columnWithDataList, OperationInfoList = operationInfoList, Result = RequestResponceType.Success };
        }

        private static GetTableOutModel GetDataForFrmFinanceBudgetRowTable(frmFinanceBudgetRowTableRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            List<ColumnWithData> columnWithDataList = null;
            List<Guid> idList = new List<Guid>();
            List<Guid> objectIdList = new List<Guid>(); //смотри описание в модели
            List<int> classIdList = new List<int>();
            List<OperationInfo> operationInfoList = new List<OperationInfo>();
            //
            var sortColumn = columnParams.Column;
            var columnSettings = columnParams.ColumnSettings;
            //
            var list = FinanceBudgetRowSearchForTable.GetList(sortColumn, requestInfo, user.User, dataSource);
            foreach (var e in list)
            {
                idList.Add(e.ID);
                classIdList.Add(e.ClassID);
                operationInfoList.Add(new OperationInfo()
                {
                    ObjectName = e.Identifier,
                    OwnerID = user.User.ID
                });
            }
            columnWithDataList = GetListForTablePrivate<FinanceBudgetRowSearchForTable>(columnSettings.ColumnsDTL, list, requestInfo.TimezoneOffsetInMinutes);
            //
            return new GetTableOutModel() { IDList = idList, ObjectIDList = objectIdList, ClassIDList = classIdList, DataList = columnWithDataList, OperationInfoList = operationInfoList, Result = RequestResponceType.Success };
        }

        private static GetTableOutModel GetTableForPurchaseSpecification(AssetSearchTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            List<ColumnWithData> columnWithDataList = null;
            List<Guid> idList = new List<Guid>();
            List<Guid> objectIdList = new List<Guid>(); //смотри описание в модели
            List<int> classIdList = new List<int>();
            List<OperationInfo> operationInfoList = new List<OperationInfo>();
            //
            var sortColumn = columnParams.Column;
            var columnSettings = columnParams.ColumnSettings;
            //
            var list = PurchaseSpecificationSearchAsTable.GetList(sortColumn, requestInfo, user.User, dataSource);
            foreach (var e in list)
            {
                idList.Add(e.ID);
                classIdList.Add(e.ClassID);
                operationInfoList.Add(new OperationInfo()
                {
                    RowObject = e.BLL
                });
            }
            columnWithDataList = GetListForTablePrivate<PurchaseSpecificationSearchAsTable>(columnSettings.ColumnsDTL, list, requestInfo.TimezoneOffsetInMinutes);
            //
            return new GetTableOutModel() { IDList = idList, ObjectIDList = objectIdList, ClassIDList = classIdList, DataList = columnWithDataList, OperationInfoList = operationInfoList, Result = RequestResponceType.Success };
        }
        private static ResultData<List<BaseForTable>> GetDataForPurchaseSpecification(AssetSearchTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = PurchaseSpecificationSearchAsTable.GetList(sortColumn, requestInfo, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForARS(AssetSearchTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = ActivesRequestSpecificationAsTable.GetList(sortColumn, requestInfo, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static GetTableOutModel GetDataForGoodsInvoiceSpecificationSearch(AssetSearchTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            List<ColumnWithData> columnWithDataList = null;
            List<Guid> idList = new List<Guid>();
            List<Guid> objectIdList = new List<Guid>(); //смотри описание в модели
            List<int> classIdList = new List<int>();
            List<OperationInfo> operationInfoList = new List<OperationInfo>();
            //
            var sortColumn = columnParams.Column;
            var columnSettings = columnParams.ColumnSettings;
            //
            var list = GoodsInvoiceSpecificationSearchAsTable.GetList(sortColumn, requestInfo, user.User, dataSource);
            foreach (var e in list)
            {
                idList.Add(e.ID);
                classIdList.Add(e.ClassID);
                operationInfoList.Add(new OperationInfo()
                {
                    RowObject = e.DTL
                });
            }
            columnWithDataList = GetListForTablePrivate<GoodsInvoiceSpecificationSearchAsTable>(columnSettings.ColumnsDTL, list, requestInfo.TimezoneOffsetInMinutes);
            //
            return new GetTableOutModel() { IDList = idList, ObjectIDList = objectIdList, ClassIDList = classIdList, DataList = columnWithDataList, OperationInfoList = operationInfoList, Result = RequestResponceType.Success };
        }

        private static ResultData<List<BaseForTable>> GetDataForLocatedActivesSearch(AssetSearchTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = LocatedActivesSearchAsTable.GetList(sortColumn, requestInfo, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForAssetMaintenance(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = AssetMaintenanceForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForAssetAgreementMaintenance(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = AssetMaintenanceForTable.GetListForAgreement(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForLicenceMaintenance(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = LicenceMaintenanceForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }
        private static ResultData<List<BaseForTable>> GetDataForLicenceAgreementMaintenance(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = LicenceMaintenanceForTable.GetListForAgreement(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }
        private static ResultData<List<BaseForTable>> GetDataForSupplier(TableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = SupplierForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForSoftwareModelUsingType(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = SoftwareModelUsingTypeForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForDeputy(TableLoadRequestInfoDeputy requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            if (requestInfo.DeputyTableType == "ideputyfor")
            {
                var list = IDeputyForForTable.GetList(sortColumn, requestInfo, user.User, dataSource);
                var objectList = list.Cast<BaseForTable>().ToList();
                return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
            }
            else if (requestInfo.DeputyTableType == "mydeputies")
            {
                var list = MyDeputyForTable.GetList(sortColumn, requestInfo, user.User, dataSource);
                var objectList = list.Cast<BaseForTable>().ToList();
                return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
            }
            else
                return ResultData<List<BaseForTable>>.Create(RequestResponceType.GlobalError, null);
        }
        private static ResultData<List<BaseForTable>> GetSoftwareModelCatalogDependency(TableLoadRequestInfoSoftwareModelCatalog requestInfo, ApplicationUser user, ColumnParams columnParams, FilterParams filterParams, DataSource dataSource)
        {
            List<BaseForTable> objectList = null;
            //
            var sortColumn = columnParams.Column;
            var treeSettings = filterParams.TreeSettings;
            //
            if (treeSettings == null)
            {
                var userSettings = BLL.Settings.UserSettings.TryGetOrCreate(user.User, user.DefaultUserCulture, dataSource);
                if (userSettings != null)
                    treeSettings = userSettings.DTLTree;
            }
            //
            String sqlQueryFilter = String.Empty;

            if (filterParams.Filter != null)
                if (filterParams.Filter.Standart)
                    sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<SoftwareModelCatalogForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                else
                    sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<SoftwareModelCatalogForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
            var list = SoftwareModelCatalogForTable.GetListDependency(sortColumn, requestInfo, user.User, sqlQueryFilter, BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(requestInfo.AfterModifiedMilliseconds), dataSource);
            objectList = list.Cast<BaseForTable>().ToList();

            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetSoftwareModelRelated(TableLoadRequestInfoSoftwareModelCatalog requestInfo, ApplicationUser user, ColumnParams columnParams, FilterParams filterParams, DataSource dataSource)
        {
            List<BaseForTable> objectList = null;
            //
            var sortColumn = columnParams.Column;
            var treeSettings = filterParams.TreeSettings;
            //
            if (treeSettings == null)
            {
                var userSettings = BLL.Settings.UserSettings.TryGetOrCreate(user.User, user.DefaultUserCulture, dataSource);
                if (userSettings != null)
                    treeSettings = userSettings.DTLTree;
            }
            //
            String sqlQueryFilter = String.Empty;

            if (filterParams.Filter != null)
                if (filterParams.Filter.Standart)
                    sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<SoftwareModelUpdateForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                else
                    sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<SoftwareModelUpdateForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
            var list = InfraManager.Web.BLL.ProductCatalog.Models.SoftwareModelUpdateForTable.GetListRelated(sortColumn, requestInfo, user.User, sqlQueryFilter, BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(requestInfo.AfterModifiedMilliseconds), dataSource);
            objectList = list.Cast<BaseForTable>().ToList();

            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetSoftwareModelCatalog(TableLoadRequestInfoSoftwareModelCatalog requestInfo, ApplicationUser user, ColumnParams columnParams, FilterParams filterParams, DataSource dataSource)
        {
            List<BaseForTable> objectList = null;
            //
            var sortColumn = columnParams.Column;
            var treeSettings = filterParams.TreeSettings;
            //
            if (treeSettings == null)
            {
                var userSettings = BLL.Settings.UserSettings.TryGetOrCreate(user.User, user.DefaultUserCulture, dataSource);
                if (userSettings != null)
                    treeSettings = userSettings.DTLTree;
            }
            //
            String sqlQueryFilter = String.Empty;

            if (filterParams.Filter != null)
                if (filterParams.Filter.Standart)
                    sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommandForStandartFilter<SoftwareModelCatalogForTable>(user.User.ID, filterParams.Filter, requestInfo.ViewName, treeSettings);
                else
                    sqlQueryFilter = BLL.Tables.Filters.Filter.CreateSqlFilterCommand<SoftwareModelCatalogForTable>(user.User.ID, filterParams.Filter.Elements, dataSource, treeSettings);
            var list = SoftwareModelCatalogForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(requestInfo.AfterModifiedMilliseconds), dataSource);
            objectList = list.Cast<BaseForTable>().ToList();

            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }
        //
        private static ResultData<List<BaseForTable>> GetDataForInventorySpecification(TableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = InventorySpecificationForTable.GetList(sortColumn, (AssetSearchTableLoadRequestInfo)requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForMobileSearch(TableLoadRequestInfoMobileSearch requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = CallForTable.GetListForMobileSearch(sortColumn, requestInfo, user.User, null, true, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            //
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }


        private static ResultData<List<BaseForTable>> GetDataForMobileCommonSearch(TableLoadRequestInfoMobileSearch requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = CommonForTable.GetListForMobileSearch(sortColumn, requestInfo, user.User, null, true, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            //
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForRFCCategoryList(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = RFCCategoryForTable.GetRFCCategoryList(sortColumn, requestInfo, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            //
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }


        private static ResultData<List<BaseForTable>> GetDataForSoftwareLicence(TableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = SoftwareLicenceForTable.GetList(sortColumn, (AssetModelSearchTableLoadRequestInfo)requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForSoftModel(TableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list =
                requestInfo is SoftwareModelSearchTableLoadRequestInfo
                    ? SoftModelForTable.GetList(sortColumn, (SoftwareModelSearchTableLoadRequestInfo)requestInfo, user.User, null, dataSource)
                    : SoftModelForTable.GetList(sortColumn, (AssetModelSearchTableLoadRequestInfo)requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForAdapterReference(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = AdapterReferenceForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForPeripheralReference(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = PeripheralReferenceForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForSubdeviceList(SimpleRequestWithSearchAndNavigatorInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = SubdeviceForTable.GetSubdeviceListOnStore(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForUsersList(SimpleRequestWithSearchAndNavigatorInfoAndSoftwareSubLicence requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = AssertUserForTable.GetUsersList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForEquipList(SimpleRequestWithSearchAndNavigatorInfoAndSoftwareSubLicence requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = AssertEquipForTable.GetEquipList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }


        private static ResultData<List<BaseForTable>> GetDataForSupplierContactPerson(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = ContractPersonForTable.GetListBySupplier(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForContractAgreement(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = ContractAgreementForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForContractLicence(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = ContractLicenceForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForContractLicenceAgreement(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = ContractLicenceForTable.GetListForAgreement(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetCallFoUserSerach(TableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            String sqlQueryFilter = String.Empty;
            List<BaseForTable> objectList = null;
            var sortColumn = columnParams.Column;
            //в ADList прописан ID для поиска клиента
            var IdUser = requestInfo.CurrentFilterID.ToString();
            //Прописываем фильтр для формы поиска клиента без создания нового фильтра
            if (requestInfo.IDList == null)
                sqlQueryFilter = "WHERE\r\n(c.[ClientID] = '" + IdUser + "')";
            requestInfo.CurrentFilterID = null;
            var list = CallForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, false, null, dataSource);
            objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }
        private static ResultData<List<BaseForTable>> GetAssetFoUserSerach(TableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            List<BaseForTable> objectList = null;
            var sortColumn = columnParams.Column;
            //в ADList прописан ID для поиска клиента
            var IdUser = requestInfo.IDList[0].ToString();
            //Прописываем фильтр для формы поиска клиента без создания нового фильтра
            String sqlQueryFilter = "WHERE " +
                "([dbo].func_ItemInOrganizationItem('9','" +
                IdUser +
                "', UtilizerClassID, UtilizerID) = '1') AND ClassID IN(6, 34)";
            requestInfo.IDList = null;
            var list = AssetNumberForTable.GetList(sortColumn, requestInfo, user.User, sqlQueryFilter, dataSource);
            objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }
        private static ResultData<List<BaseForTable>> GetDataForSoftwareLicenceSerialNumber(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = SoftwareLicenceSerialNumberForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForSoftwareLicenceReference(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = SoftwareLicenceReferenceForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForSoftwareLicenceReferenceReturning(frmTableLoadRequestInfoForPool requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = SoftwareLicenceReferenceReturningForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForSoftwareLicenceUpdate(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = SoftwareLicenceUpdateForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForSoftwareLicenceLocation(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = SoftwareLicenceLocationForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForSoftwareLicenceModel(AssetModelSearchTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = SoftwareLicenceModelForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForAssetSearch(AssetSearchTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = AssetSearchAsTable.GetList(sortColumn, requestInfo, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForDataEntityDependency(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = DataEntityDependencyListForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetClusterVMDependency(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = ClusterVMForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }
        private static ResultData<List<BaseForTable>> GetClusterHostsDependency(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = ClusterHostsForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }
        private static ResultData<List<BaseForTable>> GetSoftwareModelUpdateForTable(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = InfraManager.Web.BLL.ProductCatalog.Models.SoftwareModelUpdateForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }     
        
        private static ResultData<List<BaseForTable>> GetSoftwareModelСomponentForTable(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = InfraManager.Web.BLL.ProductCatalog.Models.SoftwareModelСomponentForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetSoftwareModelInstallationForTable(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = InfraManager.Web.BLL.ProductCatalog.Models.SoftwareModelInstallationForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetSoftwareModelProcessNameList(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;

            var list = InfraManager.BLL.Web.ProductCatalog.Models.SoftwareModelProcessNameList.GetList(requestInfo.ParentObjectID, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetSoftwareModelPackageContentsForTable(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = InfraManager.Web.BLL.ProductCatalog.Models.SoftwareModelPackageContentsForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }
               
        private static ResultData<List<BaseForTable>> GetSoftwareSynonymsModelForTable(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
           
            var list = InfraManager.BLL.Web.Synonyms.SynonymsSoftwareModelForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetSoftwareModelLicensesForTable(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = InfraManager.Web.BLL.ProductCatalog.Models.SoftwareModelLicensesForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }
        private static ResultData<List<BaseForTable>> GetLogicObjectComponentsForTable(frmTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            var list = LogicObjectComponents.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        internal static List<ColumnWithData> GetListForTablePrivate<T>(List<DTL.Settings.ColumnSettings> columnSettingsDTL, List<T> list, int timezoneOffsetInMinutes)
        {
            var retval = new List<ColumnWithData>();
            //
            foreach (var column in columnSettingsDTL)
            {
                var propertyInfo = (typeof(T)).GetProperty(column.MemberName);
                var cellList = new List<Cell>();
                foreach (var obj in list)
                {
                    string valueStringRepresentation = string.Empty;
                    //
                    var value = propertyInfo.GetValue(obj);
                    if (value != null)
                    {
                        if (propertyInfo.Name.ToLower().StartsWith("utc"))
                        {//convert dateTime UTC -> browser time
                            if (propertyInfo.PropertyType == typeof(DateTime))
                                valueStringRepresentation = ((DateTime)value).AddMinutes(-timezoneOffsetInMinutes).ToString(InfraManager.Core.Global.DateTimeWithoutSecondsFormat);
                            else if (propertyInfo.PropertyType == typeof(DateTime?))
                                valueStringRepresentation = ((DateTime?)value).Value.AddMinutes(-timezoneOffsetInMinutes).ToString(InfraManager.Core.Global.DateTimeWithoutSecondsFormat);
                            else
                                valueStringRepresentation = value.ToString();
                        }
                        else if (propertyInfo.Name == "TotalCostWithNDS")
                        {
                            if (propertyInfo.PropertyType == typeof(decimal?))
                                valueStringRepresentation = ((decimal?)value).Value.ToString("N2", CultureInfo.CreateSpecificCulture("ru-RU"));
                        }
                        else if (propertyInfo.Name == "PurchasedAndPlaced" || propertyInfo.Name == "WaitPurchase")
                        {
                            if (propertyInfo.PropertyType == typeof(decimal?))
                            {
                                var val = (decimal?)value;
                                valueStringRepresentation = val.HasValue ? string.Concat(val.Value, "%") : string.Empty;
                            }
                        }
                        else
                        {
                            valueStringRepresentation = value.ToString();
                            //коррекция по ширине ради оптимизации
                            if (valueStringRepresentation.Length > 50)
                            {
                                var preferredLength = (int)(Math.Max((double)column.Width / 5d, 50d) * 1.5d);
                                //
                                valueStringRepresentation =
                                       valueStringRepresentation.Length > preferredLength ?
                                       valueStringRepresentation.Substring(0, preferredLength) + "..." :
                                       valueStringRepresentation;
                            }
                        }
                    }
                    //
                    var cell = new Cell(valueStringRepresentation, null);
                    cellList.Add(cell);
                };
                retval.Add(new ColumnWithData(column, cellList));
            }
            //
            return retval;
        }

        private static ResultData<List<BaseForTable>> GetDataForOrganizationList(OrgStructureSearchTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = OrganizationForTable.GetList(sortColumn, requestInfo, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForSubdivisionList(OrgStructureSearchTableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = SubdivisionForTable.GetList(sortColumn, requestInfo, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForManufacturer(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = ManufacturerForTable.GetList(sortColumn, requestInfo, null, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForSynonym(SimpleRequestWithSearchInfoAndFilterSynonym requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = SynonymForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForPositions(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = PositionForTable.GetList(sortColumn, requestInfo, user.User, null, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForUnits(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = UnitForTable.GetList(sortColumn, requestInfo, null, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForCriticality(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = CriticalityForTable.GetList(sortColumn, requestInfo, null, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForInfrastructureSegment(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = BLL.Assets.InfrastructureSegmentForTable.GetList(sortColumn, requestInfo, null, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataWorkflowScheme(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = WorkflowSchemeForTable.GetList(sortColumn, requestInfo, null, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataFileSystem(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = FileSystemForTable.GetList(sortColumn, requestInfo, null, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataCostCategory(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = CostCategoryForTable.GetList(sortColumn, requestInfo, null, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }


        private static ResultData<List<BaseForTable>> GetTelephoneType(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = TelephoneTypeForTable.GetList(sortColumn, requestInfo, null, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        //=======
        private static ResultData<List<BaseForTable>> GetSlotType(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = SlotTypeForTable.GetList(sortColumn, requestInfo, null, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetTechnologyType(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = TechnologyTypeForTable.GetList(sortColumn, requestInfo, null, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetCartridgeType(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = CartridgeTypeForTable.GetList(sortColumn, requestInfo, null, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetConnectorType(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = ConnectorTypeForTable.GetList(sortColumn, requestInfo, null, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetNotificationForTable(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = InfraManager.Web.BLL.SD.NotificationForTable.GetList(sortColumn, requestInfo, null, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetProblemTypeNewForTable(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = ProblemTypeNewForTable.GetList(sortColumn, requestInfo, null, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataCalendarHoliday(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = BLL.Calendar.CalendarHolidayForTable.GetList(sortColumn, requestInfo, null, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataCalendarWeekend(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = BLL.Calendar.CalendarWeekendForTable.GetList(sortColumn, requestInfo, null, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForlBuilderForm(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = BLL.FormBuilder.FormForTable.GetList(sortColumn, requestInfo, null, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }
        private static ResultData<List<BaseForTable>> GetDataForKBAUserLst(TableLoadRequestInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = KBAAdmittedPersons.GetList(sortColumn, requestInfo, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataForExclusuion(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = BLL.Calendar.ExclusionForTable.GetList(sortColumn, requestInfo, null, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataParameterEnum(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = BLL.Parameters.ParameterEnumForTable.GetList(sortColumn, requestInfo, null, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }

        private static ResultData<List<BaseForTable>> GetDataCalendarWorkSchedule(SimpleRequestWithSearchInfo requestInfo, ApplicationUser user, ColumnParams columnParams, DataSource dataSource)
        {
            var sortColumn = columnParams.Column;
            //
            var list = BLL.Calendar.CalendarWorkScheduleForTable.GetList(sortColumn, requestInfo, null, user.User, dataSource);
            var objectList = list.Cast<BaseForTable>().ToList();
            return ResultData<List<BaseForTable>>.Create(RequestResponceType.Success, objectList);
        }
    }
}