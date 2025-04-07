using InfraManager.Core.Data;
using InfraManager.Core.Exceptions;
using InfraManager.Core.Logging;
using InfraManager.UI.Web.Models.Filter;
using InfraManager.Web.BLL.Assets;
using InfraManager.Web.BLL.Assets.AssetNumber;
using InfraManager.Web.BLL.Assets.Repair;
using InfraManager.Web.BLL.Assets.Utilizer;
using InfraManager.Web.BLL.Assets.WrittenOff;
using InfraManager.Web.BLL.Catalogs;
using InfraManager.Web.BLL.Contracts;
using InfraManager.Web.BLL.Finance.ActivesRequest;
using InfraManager.Web.BLL.Finance.Budget;
using InfraManager.Web.BLL.Finance.Purchase;
using InfraManager.Web.BLL.Inventories;
using InfraManager.Web.BLL.SD.Calls;
using InfraManager.Web.BLL.SD.MyWorkplace;
using InfraManager.Web.BLL.SD.Problems;
using InfraManager.Web.BLL.SD.RFC;
using InfraManager.Web.BLL.SD.WorkOrders;
using InfraManager.Web.BLL.Software;
using InfraManager.Web.BLL.Tables.Filters;
using InfraManager.Web.BLL.Tables.Filters.Elements;
using InfraManager.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Resources = InfraManager.ResourcesArea.Resources;

namespace InfraManager.Web.Controllers.Utility
{
    [ApiController]
    public class FilterApiController : BaseApiController
    {
        #region method GetFilters
        [HttpGet]
        [Obsolete("Use api/filters instead")]
        [Route("filterApi/GetFilters", Name = "GetFilters")]
        public IList<BLL.Tables.Filters.Filter> GetFilters([FromQuery] String viewName, [FromQuery] BLL.Modules module)
        {
            var user = base.CurrentUser;
            if (!user.ActionIsGranted(module, viewName))
            {
                Logger.Warning("FilterApiController.GetFilters userID={0}, userName={1}, viewName={2} failed (user is client)", user.Id, user.UserName, viewName);
                return null;
            }
            Logger.Trace("FilterApiController.GetFilters userID={0}, userName={1}, viewName={2}", user.Id, user.UserName, viewName);
            //
            var retval = new List<BLL.Tables.Filters.Filter>();
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    retval.AddRange(BLL.Tables.Filters.Filter.GetList(user.User.ID, viewName, dataSource));
                }
                retval = retval.Distinct(new InfraManager.Core.FEqualityComparer<BLL.Tables.Filters.Filter>((x, y) => x.ID == y.ID)).ToList();
                //
                foreach (var el in retval)
                    if (el.Name.StartsWith("_") && el.Name.EndsWith("_"))
                    {
                        var newName = Resources.ResourceManager.GetString(el.Name, Thread.CurrentThread.CurrentUICulture);
                        if (!String.IsNullOrWhiteSpace(newName))
                            el.Name = newName;
                        else
                            el.Name = el.Name.Trim('_');
                    }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения фильтров для представления списка.");
                return null;
            }
            //
            return retval;
        }
        #endregion

        #region method GetSDFilters
        [HttpGet]
        [Obsolete("Use api/filters instead")]
        [Route("filterApi/GetSDFilters", Name = "GetSDFilters")]
        public IList<BLL.Tables.Filters.Filter> GetSDFilters([FromQuery] String viewName, [FromQuery] BLL.Modules module)
        {
            var user = base.CurrentUser;
            if (!user.ActionIsGranted(module, viewName))
            {
                Logger.Warning("FilterApiController.GetFilters userID={0}, userName={1}, viewName={2} failed (user is client)", user.Id, user.UserName, viewName);
                return null;
            }
            Logger.Trace("FilterApiController.GetFilters userID={0}, userName={1}, viewName={2}", user.Id, user.UserName, viewName);
            //
            var retval = new List<BLL.Tables.Filters.Filter>();
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    retval.AddRange(BLL.Tables.Filters.Filter.GetSDList(user.User.ID, viewName, dataSource));
                }
                retval = retval.Distinct(new InfraManager.Core.FEqualityComparer<BLL.Tables.Filters.Filter>((x, y) => x.ID == y.ID)).ToList();
                //
                foreach (var el in retval)
                    if (el.Name.StartsWith("_") && el.Name.EndsWith("_"))
                    {
                        var newName = Resources.ResourceManager.GetString(el.Name, Thread.CurrentThread.CurrentUICulture);
                        if (!String.IsNullOrWhiteSpace(newName))
                            el.Name = newName;
                        else
                            el.Name = el.Name.Trim('_');
                    }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения фильтров для представления списка.");
                return null;
            }
            //
            return retval;
        }
#endregion

        #region method GetFilterOperationList
        [HttpGet]
        [Route("filterApi/GetFilterOperationList", Name = "GetFilterOperationList")]
        [Obsolete("Use api/filteroperations instead")]
        public IList GetFilterOperationList([FromQuery] FilterTypes filterType)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("FilterApiController.GetFilterOperationList userID={0}, userName={1}, filterType={2}", user.Id, user.UserName, filterType);
            //
            try
            {
                var culture = HttpContext?.GetCurrentCulture() ?? CultureInfo.CurrentUICulture.Name;
                var retval = BLL.Tables.Filters.Filter.GetOperationList(culture, filterType);
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка операций фильтров.");
                return null;
            }
        }
        #endregion

        #region method GetFullFilterElementsList
        [HttpGet]
        [Route("filterApi/GetFilterElementsList", Name = "GetFullFilterElementsList")]
        public IList<FilterElement> GetFullFilterElementsList([FromQuery] String viewName, [FromQuery] BLL.Modules module)
        {
            var user = base.CurrentUser;
            if (!user.ActionIsGranted(module, viewName))
            {
                Logger.Warning("FilterApiController.GetFullFilterElementsList userID={0}, userName={1}, viewName={2} failed (user is client)", user.Id, user.UserName, viewName);
                return null;
            }
            Logger.Trace("FilterApiController.GetFullFilterElementsList userID={0}, userName={1}, viewName={2}", user.Id, user.UserName, viewName);
            //
            try
            {
                var culture = HttpContext?.GetCurrentCulture() ?? CultureInfo.CurrentUICulture.Name;
                Type type;
                //
                switch (viewName)
                {
                    case ClientCallForTable.VIEW_NAME:
                        type = typeof(ClientCallForTable); break;
                    case CommonWithNegotiationsForTable.VIEW_NAME:
                        type = typeof(CommonWithNegotiationsForTable); break;
                    //
                    case CommonForTable.VIEW_NAME:
                        type = typeof(CommonForTable); break;
                    case CallForTable.VIEW_NAME:
                        type = typeof(CallForTable); break;
                    case ProblemForTable.VIEW_NAME:
                        type = typeof(ProblemForTable); break;
                    case WorkOrderForTable.VIEW_NAME:
                        type = typeof(WorkOrderForTable); break;
                    case CustomControlForTable.VIEW_NAME:
                        type = typeof(CustomControlForTable); break;
                    //
                    case AssetNumberForTable.VIEW_NAME:
                        type = typeof(AssetNumberForTable); break;
                    case RepairForTable.VIEW_NAME:
                        type = typeof(RepairForTable); break;
                    case WrittenOffForTable.VIEW_NAME:
                        type = typeof(WrittenOffForTable); break;
                    case UtilizerCompleteForTable.VIEW_NAME:
                        type = typeof(UtilizerCompleteForTable); break;
                    case SoftwareLicenceForTable.VIEW_NAME:
                        type = typeof(SoftwareLicenceForTable); break;
                    //
                    case FinanceActivesRequestForTable.VIEW_NAME:
                        type = typeof(FinanceActivesRequestForTable); break;
                    case FinancePurchaseForTable.VIEW_NAME:
                        type = typeof(FinancePurchaseForTable); break;
                    case FinanceBudgetRowForTable.VIEW_NAME:
                        type = typeof(FinanceBudgetRowForTable); break;
                    case FinanceBudgetRowSearchForTable.VIEW_NAME:
                        type = typeof(FinanceBudgetRowSearchForTable); break;
                    case FinanceBudgetForTable.VIEW_NAME:
                        type = typeof(FinanceBudgetForTable); break;
                    //
                    case ContractForTable.VIEW_NAME:
                        type = typeof(ContractForTable); break;
                    case InventoryForTable.VIEW_NAME:
                        type = typeof(InventoryForTable); break;
                    case InventorySpecificationForTable.VIEW_NAME:
                        type = typeof(InventorySpecificationForTable); break;
                    case ConfigurationUnitForTable.VIEW_NAME:
                        type = typeof(ConfigurationUnitForTable); break;
                    case LogicalObjectForTable.VIEW_NAME:
                        type = typeof(LogicalObjectForTable); break;
                    case DataEntityForTable.VIEW_NAME:
                        type = typeof(DataEntityForTable); break;
                    case RFCForTable.VIEW_NAME:
                        type = typeof(RFCForTable); break;
                    //
                    case SoftwareLicenseDistributionForTable.VIEW_NAME:
                        type = typeof(SoftwareLicenseDistributionForTable); break;
                    case SubSoftwareLicenseForTable.VIEW_NAME:
                        type = typeof(SubSoftwareLicenseForTable); break;
                    case SoftwareInstallationForTable.VIEW_NAME:
                        type = typeof(SoftwareInstallationForTable); break;
                    //
                    case SoftwareModelCatalogForTable.VIEW_NAME:
                        type = typeof(SoftwareModelCatalogForTable); break;
                    case ClusterForTable.VIEW_NAME:
                        type = typeof(ClusterForTable); break;
                    //
                    default:
                        throw new NotSupportedException();
                }
                //
                var method = typeof(FilterElement).GetMethod(nameof(FilterElement.GetDefaultFilterElements), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                var generic = method.MakeGenericMethod(type);
                var result = generic.Invoke(null, new object[] { culture, viewName, user.User });
                //
                return result as IList<FilterElement>;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка доступынх элементов фильтра.");
                return null;
            }
        }
        #endregion

        #region method GetFilterElementsListByFilter
        [HttpGet]
        [Route("filterApi/GetFilterElementsListByFilter", Name = "GetFilterElementsListByFilter")]
        [Obsolete("Use api/filters/{id} instead")]
        public IList<FilterElement> GetFilterElementsListByFilter([FromQuery] String filterId, [FromQuery] BLL.Modules module)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("FilterApiController.GetFilterElementsListByFilter userID={0}, userName={1}, filterID={2}", user.Id, user.UserName, filterId);
            //
            Guid id;
            if (String.IsNullOrWhiteSpace(filterId) || !Guid.TryParse(filterId, out id))
                return null;
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var filter = BLL.Tables.Filters.Filter.Get(id, user.User.ID, dataSource);
                    if (filter.UserID.HasValue && filter.UserID.Value != user.User.ID)
                    {
                        Logger.Warning("FilterApiController.GetFilterElementsListByFilter userID={0}, userName={1}, filterID={2} failed (filter don't belong to user)", user.Id, user.UserName, filterId);
                        return null;
                    }
                    else if (!user.ActionIsGranted(module, filter.ViewName))
                    {
                        Logger.Warning("FilterApiController.GetFilterElementsListByFilter userID={0}, userName={1}, filterID={2} failed (user is client)", user.Id, user.UserName, filterId);
                        return null;
                    }
                    //
                    var retval = filter.Elements;
                    return retval;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "Ошибка получения элементов фильтра.");
                return null;
            }
        }
        #endregion

        #region method PostFilter
        public sealed class PostFilterReturnModel
        {
            public BLL.Tables.Filters.Filter NewFilter { get; set; }
            public ResultWithMessage Response { get; set; }
        }
        [HttpPost]
        [Route("filterApi/PostFilter", Name = "PostFilter")]
        [Obsolete("Post to api/filters/{view} to submit new filter or Put to api/filters/{id} to update existing filter")]
        public PostFilterReturnModel PostFilter(
            [FromBody] FilterElementModel[] data,
            [FromQuery] string filterId, 
            [FromQuery] string filterName, 
            [FromQuery] string viewName, 
            [FromQuery] BLL.Modules module, 
            [FromQuery] bool isTemp, 
            [FromQuery] bool others = false)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new PostFilterReturnModel() { Response = ResultWithMessage.Create(RequestResponceType.NullParamsError, Resources.SaveError) };
            //
            Logger.Trace("FilterApiController.PostFilter userID={0}, userName={1}, filterID={2}, filterName={3}, viewName={4}", user.Id, user.UserName, filterId, filterName, viewName);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                    try
                    {
                        dataSource.BeginTransaction();
                        //
                        BLL.Tables.Filters.Filter filter = null;
                        try
                        {
                            Guid id;
                            if (Guid.TryParse(filterId, out id))
                            {
                                filter = BLL.Tables.Filters.Filter.Get(id, user.User.ID, dataSource);
                                //
                                if ((filter.UserID.HasValue && filter.UserID.Value != user.User.ID) || filter.Standart != false)
                                {
                                    Logger.Warning("FilterApiController.PostFilter userID={0}, userName={1}, filterID={2} failed (filter don't belong to user)", user.Id, user.UserName, filterId);
                                    return new PostFilterReturnModel() { Response = ResultWithMessage.Create(RequestResponceType.ObjectDeleted, Resources.SaveError) };
                                }
                                else if (!user.ActionIsGranted(module, filter.ViewName))
                                {
                                    Logger.Warning("FilterApiController.PostFilter userID={0}, userName={1}, filterID={2} failed (user is client)", user.Id, user.UserName, filterId);
                                    return new PostFilterReturnModel() { Response = ResultWithMessage.Create(RequestResponceType.AccessError, Resources.SaveError) };
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.Error(e, "Cannot get filter");
                            return new PostFilterReturnModel() { Response = ResultWithMessage.Create(RequestResponceType.GlobalError, Resources.SaveError) };
                        }
                        //
                        if (filter == null)
                        {
                            if (String.IsNullOrEmpty(filterName) || String.IsNullOrEmpty(viewName))
                                throw new ArgumentNullException("Cannot create new filter");
                            if (!user.ActionIsGranted(module, viewName))
                            {
                                Logger.Warning("FilterApiController.PostFilter userID={0}, userName={1}, filterID={2} failed to create new filter (user is client)", user.Id, user.UserName, filterId);
                                return new PostFilterReturnModel() { Response = ResultWithMessage.Create(RequestResponceType.AccessError, Resources.SaveError) };
                            }
                            //
                            filter = new BLL.Tables.Filters.Filter();
                            filter.Name = filterName;
                            filter.ViewName = viewName;
                            filter.Standart = false;
                            filter.Others = others;
                            filter.UserID = user.User.ID;
                        }                        
                        //
                        var feList = new List<FilterElement>();
                        //
                        foreach (var elem in data
                            .Where(x => x.IsEmpty.HasValue 
                                && x.Type.HasValue
                                && !string.IsNullOrEmpty(x.LocaleName)
                                && !string.IsNullOrEmpty(x.PropertyName)))
                        {
                            Type type;
                            var baseFilterElem = new FilterElement(
                                elem.PropertyName, 
                                elem.LocaleName, 
                                (FilterTypes)elem.Type.Value,
                                (FilterEmptyState)elem.IsEmpty.Value);
                            if (baseFilterElem == null)
                                continue;
                            //
                            switch (baseFilterElem.Type)
                            {
                                case FilterTypes.DatePick:
                                    type = typeof(DatePickElement); break;
                                case FilterTypes.SelectorMultiple:
                                    type = typeof(SelectorMultipleElement); break;
                                case FilterTypes.SliderRange:
                                    type = typeof(RangeSliderElement); break;
                                case FilterTypes.SimpleValue:
                                    type = typeof(SimpleValueElement); break;
                                case FilterTypes.LikeValue:
                                    type = typeof(LikeValueElement); break;
                                //
                                default:
                                    throw new NotSupportedException("FilterTypes");
                            }
                            //

                            object result = null;
                            try
                            {
                                var method = type.GetMethod(nameof(FilterElement.ParseFilterData), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                                result = method.Invoke(null, new object[] { elem, baseFilterElem });
                            }
                            catch(Exception ex)
                            {

                            }

                            if (result == null)
                                continue;
                            //
                            feList.Add(result as FilterElement);

                        }
                        //
                        if (feList.Count <= 0)
                        {
                            BLL.Settings.FilterSettings.DeleteTempSettings(user.User.ID, viewName, dataSource);
                            dataSource.CommitTransaction();
                            return new PostFilterReturnModel() { Response = ResultWithMessage.Create(RequestResponceType.BadParamsError, Resources.NoFilterElementsError) };
                        }
                        //
                        var oldElements = filter.Elements as List<FilterElement>;
                        feList.ForEach(fe => fe.FilterID = filter.ID);
                        filter.Elements = feList;
                        filter.Others = others;
                        //
                        if(filter.UserID != null && filter.Standart == false)
                            filter.Save(dataSource);
                        else
                            return new PostFilterReturnModel() { Response = ResultWithMessage.Create(RequestResponceType.BadParamsError, Resources.SaveError) };
                        if (oldElements != null)
                            oldElements.ForEach(o => o.Delete(dataSource));
                        //
                        if (!isTemp)
                            BLL.Settings.FilterSettings.DeleteTempSettings(user.User.ID, viewName, dataSource);
                        //
                        dataSource.CommitTransaction();
                        return new PostFilterReturnModel() { Response = ResultWithMessage.Create(RequestResponceType.Success), NewFilter = filter };
                    }
                    finally
                    {//for all return
                        if (dataSource.TransactionIsActive)
                            dataSource.RollbackTransaction();
                    }
            }
            catch (ArgumentValidationException e)
            {
                Logger.Error(e, String.Format(@"Save Filter validation error '{3}', filterID='{0}', filterName='{1}', viewName='{2}'", filterId, filterName, viewName, e.Message));
                return new PostFilterReturnModel { Response = ResultWithMessage.Create(RequestResponceType.ValidationError, ValidatorHelper.CreateErrorMessage(e)) };
            }
            catch (Exception e)
            {//for dataSource
                Logger.Error(e, "Save filter error");
                return new PostFilterReturnModel() { Response = ResultWithMessage.Create(RequestResponceType.GlobalError, Resources.SaveError) };
            }
        }
        #endregion

        #region method DeleteFilter
        [HttpPost]
        [Route("filterApi/DeleteFilter", Name = "DeleteFilter")]
        [Obsolete("Delete to api/filters/{id}")]
        public bool DeleteFilter([FromQuery] string filterId, [FromQuery] BLL.Modules module, [FromQuery]string viewName)
        {
            var user = base.CurrentUser;
            if (user == null)
                return false;
            //
            Logger.Trace("FilterApiController.DeleteFilter userID={0}, userName={1}, filterID={2}", user.Id, user.UserName, filterId);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                    try
                    {
                        dataSource.BeginTransaction();
                        BLL.Tables.Filters.Filter filter = null;
                        try
                        {
                            Guid id;
                            if (Guid.TryParse(filterId, out id))
                            {
                                filter = BLL.Tables.Filters.Filter.Get(id, user.User.ID, dataSource);
                                //
                                if ((filter.UserID.HasValue && filter.UserID.Value != user.User.ID) || filter.Standart != false)
                                {
                                    Logger.Warning("FilterApiController.DeleteFilter userID={0}, userName={1}, filterID={2} failed (filter don't belong to user)", user.Id, user.UserName, filterId);
                                    return false;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.Error(e, "Cannot get filter");
                            return false;
                        }
                        if (filter == null)
                        {
                            Logger.Trace("FilterApiController.DeleteFilter userID={0}, userName={1}, filterID={2} failed (filter not exists)", user.Id, user.UserName, filterId);
                            return false;
                        }
                        //
                        filter.Delete(dataSource, user.User.ID);
                        dataSource.CommitTransaction();
                        return true;
                    }
                    finally
                    {
                        if (dataSource.TransactionIsActive)
                            dataSource.RollbackTransaction();
                    }
            }
            catch (Exception e)
            {
                Logger.Error(e, "Delete filter error");
                return false;
            }
        }
        #endregion

        #region method Rename
        [HttpPost]
        [Route("filterApi/RenameFilter", Name = "RenameFilter")]
        [Obsolete("Put to api/filters/{id} to update existing filter")]
        public ResultWithMessage RenameFilter([FromQuery] string filterId, [FromQuery] String newName)
        {
            var user = base.CurrentUser;
            if (user == null || String.IsNullOrWhiteSpace(newName))
                return ResultWithMessage.Create(RequestResponceType.NullParamsError, Resources.SaveError); ;
            //
            Logger.Trace("FilterApiController.RenameFilter userID={0}, userName={1}, filterID={2}, newName={3}", user.Id, user.UserName, filterId, newName);
            Guid id;
            if (!Guid.TryParse(filterId, out id))
                return ResultWithMessage.Create(RequestResponceType.NullParamsError, Resources.SaveError);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var filter = BLL.Tables.Filters.Filter.Get(id, user.User.ID, dataSource);
                    if (filter.UserID.HasValue && filter.UserID.Value != user.User.ID)
                    {
                        Logger.Warning("FilterApiController.RenameFilter userID={0}, userName={1}, filterID={2} failed (filter don't belong to user)", user.Id, user.UserName, filterId);
                        return ResultWithMessage.Create(RequestResponceType.AccessError, Resources.SaveError);
                    }
                    InfraManager.Web.BLL.Tables.Filters.Filter.Rename(id, newName, dataSource);
                    return ResultWithMessage.Create(RequestResponceType.Success);
                }
            }
            catch (ArgumentValidationException e)
            {
                Logger.Error(e, String.Format(@"Rename Filter validation error '{2}', filterID='{0}', filterName='{1}'", filterId, newName, e.Message));
                return ResultWithMessage.Create(RequestResponceType.ValidationError, ValidatorHelper.CreateErrorMessage(e));
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error rename filter");
                return ResultWithMessage.Create(RequestResponceType.GlobalError, Resources.SaveError);
            }
        }
        #endregion

        #region method GetFilterElement
        [HttpGet]
        [Route("filterApi/GetFilterElement", Name = "GetFilterElement")]
        public FilterElement GetFilterElement([FromQuery] String viewName, [FromQuery] BLL.Modules module, [FromQuery] String propertyName)
        {
            var user = base.CurrentUser;
            if (!user.ActionIsGranted(module, viewName))
            {
                Logger.Warning("FilterApiController.GetFilterElement userID={0}, userName={1}, viewName={2}, propertyName={3} failed (user is client)", user.Id, user.UserName, viewName, propertyName);
                return null;
            }
            Logger.Trace("FilterApiController.GetFilterElement userID={0}, userName={1}, viewName={2}, propertyName={3}", user.Id, user.UserName, viewName, propertyName);
            //
            try
            {
                var culture = HttpContext?.GetCurrentCulture() ?? CultureInfo.CurrentUICulture.Name;
                Type type;
                //
                switch (viewName)
                {
                    case ClientCallForTable.VIEW_NAME:
                        type = typeof(ClientCallForTable); break;
                    case CommonWithNegotiationsForTable.VIEW_NAME:
                        type = typeof(CommonWithNegotiationsForTable); break;
                    //
                    case CommonForTable.VIEW_NAME:
                        type = typeof(CommonForTable); break;
                    case CallForTable.VIEW_NAME:
                        type = typeof(CallForTable); break;
                    case ProblemForTable.VIEW_NAME:
                        type = typeof(ProblemForTable); break;
                    case WorkOrderForTable.VIEW_NAME:
                        type = typeof(WorkOrderForTable); break;
                    case CustomControlForTable.VIEW_NAME:
                        type = typeof(CustomControlForTable); break;
                    //
                    case AssetNumberForTable.VIEW_NAME:
                        type = typeof(AssetNumberForTable); break;
                    case RepairForTable.VIEW_NAME:
                        type = typeof(RepairForTable); break;
                    case WrittenOffForTable.VIEW_NAME:
                        type = typeof(WrittenOffForTable); break;
                    case UtilizerCompleteForTable.VIEW_NAME:
                        type = typeof(UtilizerCompleteForTable); break;
                    case SoftwareLicenceForTable.VIEW_NAME:
                        type = typeof(SoftwareLicenceForTable); break;
                    //
                    case FinanceActivesRequestForTable.VIEW_NAME:
                        type = typeof(FinanceActivesRequestForTable); break;
                    case FinancePurchaseForTable.VIEW_NAME:
                        type = typeof(FinancePurchaseForTable); break;
                    case FinanceBudgetRowForTable.VIEW_NAME:
                        type = typeof(FinanceBudgetRowForTable); break;
                    case FinanceBudgetRowSearchForTable.VIEW_NAME:
                        type = typeof(FinanceBudgetRowSearchForTable); break;
                    case FinanceBudgetForTable.VIEW_NAME:
                        type = typeof(FinanceBudgetForTable); break;
                    //
                    case ContractForTable.VIEW_NAME:
                        type = typeof(ContractForTable); break;
                    case InventoryForTable.VIEW_NAME:
                        type = typeof(InventoryForTable); break;
                    case InventorySpecificationForTable.VIEW_NAME:
                        type = typeof(InventorySpecificationForTable); break;
                    case ConfigurationUnitForTable.VIEW_NAME:
                        type = typeof(ConfigurationUnitForTable); break;
                    case ClusterForTable.VIEW_NAME:
                        type = typeof(ClusterForTable); break;
                    case LogicalObjectForTable.VIEW_NAME:
                        type = typeof(LogicalObjectForTable); break;
                    case DataEntityForTable.VIEW_NAME:
                        type = typeof(DataEntityForTable); break;
                    //
                    case SoftwareLicenseDistributionForTable.VIEW_NAME:
                        type = typeof(SoftwareLicenseDistributionForTable); break;
                    case SubSoftwareLicenseForTable.VIEW_NAME:
                        type = typeof(SubSoftwareLicenseForTable); break;
                    case RFCForTable.VIEW_NAME:
                        type = typeof(RFCForTable); break;
                    case SoftwareInstallationForTable.VIEW_NAME:
                        type = typeof(SoftwareInstallationForTable); break;
                    case SoftwareModelCatalogForTable.VIEW_NAME:
                        type = typeof(SoftwareModelCatalogForTable); break;
                    //
                    default:
                        throw new NotSupportedException();
                }
                //
                var method = typeof(FilterElement).GetMethod(nameof(FilterElement.GetFilterElementByProperty), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                var generic = method.MakeGenericMethod(type);
                var result = generic.Invoke(null, new object[] { propertyName, culture, viewName, user.User });
                //
                return result as FilterElement;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения данных элемента фильтра.");
                return null;
            }
        }
        #endregion
    }
}
