using InfraManager.Core.Data;
using InfraManager.Core.Exceptions;
using InfraManager.Core.Logging;
using InfraManager.Web.BLL;
using InfraManager.Web.BLL.Finance.GoodsInvoice;
using InfraManager.Web.BLL.Finance.LocatedActives;
using InfraManager.Web.BLL.Finance.Specification;
using InfraManager.Web.BLL.Finance.Specification.Search;
using InfraManager.Web.BLL.Tables;
using InfraManager.Web.DTL.Tables;
using InfraManager.Web.Helpers;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.Web.Controllers.Finance
{
    public partial class FinanceApiController
    {
        #region (Table methods)
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("finApi/GetARSSearchListForTable", Name = "GetARSSearchListForTable")]
        public TableHelper.GetTableOutModel GetARSSearchForTable([FromForm] FinanceSearchTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForTable(requestInfo, user, TableHelper.TableType.ARSSearch);
        }       
        #endregion

        #region method GetTableFiltersData
        public sealed class GetARSLinkDataOutModel
        {
            public List<ListInfo> List { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [Route("finApi/GetARSLinkTypes", Name = "GetARSLinkTypes")]
        public GetARSLinkDataOutModel GetARSLinkTypes()
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("FinanceApiController.GetARSLinkTypes UserID={0}, UserName={1}", user.User.ID, user.User.Name);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = ActivesRequestSpecificationSearchAsTable.GetTypesList(dataSource);
                    //
                    return new GetARSLinkDataOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetARSLinkDataOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }

        public sealed class GetARSModelLinkDataOutModel
        {
            public List<ListInfoWithClass> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [Route("finApi/GetARSLinkModels", Name = "GetARSLinkModels")]
        public GetARSModelLinkDataOutModel GetARSLinkModels([FromQuery] Guid typeID)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("FinanceApiController.GetARSLinkModels typeID={0} UserID={1}, UserName={2}", typeID, user.User.ID, user.User.Name);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = ActivesRequestSpecificationSearchAsTable.GetModelsList(typeID, dataSource);
                    //
                    return new GetARSModelLinkDataOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetARSModelLinkDataOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }

        [HttpGet]
        [Route("finApi/GetARSLinkVendors", Name = "GetARSLinkVendors")]
        public GetARSLinkDataOutModel GetARSLinkVendors([FromQuery] Guid typeID)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("FinanceApiController.GetARSLinkVendors typeID={0} UserID={1}, UserName={2}", typeID, user.User.ID, user.User.Name);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = ActivesRequestSpecificationSearchAsTable.GetVendorsList(typeID, dataSource);
                    //
                    return new GetARSLinkDataOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetARSLinkDataOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }

        [HttpGet]
        [Route("finApi/GetARSLinkCustomers", Name = "GetARSLinkCustomers")]
        public GetARSLinkDataOutModel GetARSLinkCustomers()
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("FinanceApiController.GetARSLinkCustomers UserID={0}, UserName={1}", user.User.ID, user.User.Name);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = ActivesRequestSpecificationSearchAsTable.GetCustomersList(user.User.ID, dataSource);
                    //
                    return new GetARSLinkDataOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetARSLinkDataOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }

        [HttpGet]
        [Route("finApi/GetARSLinkReference", Name = "GetARSLinkReference")]
        public GetARSLinkDataOutModel GetARSLinkReference()
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("FinanceApiController.GetARSLinkReference UserID={0}, UserName={1}", user.User.ID, user.User.Name);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = ActivesRequestSpecificationSearchAsTable.GetReferenceList(user.User.ID, dataSource);
                    //
                    return new GetARSLinkDataOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetARSLinkDataOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }

        [HttpGet]
        [Route("finApi/GetARSLinkNumbers", Name = "GetARSLinkNumbers")]
        public GetARSLinkDataOutModel GetARSLinkNumbers()
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("FinanceApiController.GetARSLinkNumbers UserID={0}, UserName={1}", user.User.ID, user.User.Name);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = ActivesRequestSpecificationSearchAsTable.GetNumbersList(user.User.ID, dataSource);
                    //
                    return new GetARSLinkDataOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetARSLinkDataOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }

        [HttpGet]
        [Route("finApi/GetGoodsInvoiceNumbers", Name = "GetGoodsInvoiceNumbers")]
        public GetARSLinkDataOutModel GetGoodsInvoiceNumbers([FromQuery] Guid workOrderID)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("FinanceApiController.GetGoodsInvoiceNumbers UserID={0}, UserName={1}", user.User.ID, user.User.Name);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = GoodsInvoice.GetNumbersList(workOrderID, user.User.ID, dataSource);
                    //
                    return new GetARSLinkDataOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetARSLinkDataOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }

        [HttpGet]
        [Route("finApi/GetLifeCycleStates", Name = "GetLifeCycleStates")]
        public GetARSLinkDataOutModel GetLifeCycleStates([FromQuery] Guid workOrderID)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("FinanceApiController.GetLifeCycleStates UserID={0}, UserName={1}", user.User.ID, user.User.Name);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = GoodsInvoice.GetLifeCycleStateList(workOrderID, user.User.ID, dataSource);
                    //
                    return new GetARSLinkDataOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetARSLinkDataOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetSearchedARSByID
        public sealed class GetSearchedARSByIDIncomingModel
        {
            public List<Guid> ArsIDList { get; set; }
            public bool PurchaseMode { get; set; }
        }

        public sealed class GetSearchedARSByIDOutModel
        {
            public List<ActivesRequestSpecificationSearch> ARSList { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [Route("finApi/GetSearchedARSByID", Name = "GetSearchedARSByID")]
        public GetSearchedARSByIDOutModel GetSearchedARSByID([FromForm]GetSearchedARSByIDIncomingModel model)
        {
            var user = base.CurrentUser;
            //
            if (model == null || model.ArsIDList == null || model.ArsIDList.Count == 0)
                return new GetSearchedARSByIDOutModel() { ARSList = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("FinanceApiController.GetSearchedARSByID UserID={0}, UserName={1}", user.User.ID, user.User.Name);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = ActivesRequestSpecificationSearch.GetListByID(model.ArsIDList, model.PurchaseMode, user.User, dataSource);
                    //
                    return new GetSearchedARSByIDOutModel() { ARSList = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetSearchedARSByIDOutModel() { ARSList = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetARSFromSearch
        public sealed class GetARSFromSearchIncomingModel
        {
            public Guid SpecificationID { get; set; }
        }
        public sealed class GetARSFromSearchOutModel
        {
            public ActivesRequestSpecification Elem { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetARSFromSearch", Name = "GetARSFromSearch")]
        public GetARSFromSearchOutModel GetARSFromSearch([FromQuery] GetARSFromSearchIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetARSFromSearchOutModel() { Elem = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("FinanceApiController.GetARSFromSearch userID={0}, userName={1}, specificationID={2}", user.Id, user.UserName, model.SpecificationID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = ActivesRequestSpecification.Get(model.SpecificationID, dataSource);
                    return new GetARSFromSearchOutModel() { Elem = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException)
            {
                return new GetARSFromSearchOutModel() { Elem = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetARSFromSearch, model: {0}.", model);
                return new GetARSFromSearchOutModel() { Elem = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetLocationList
        public sealed class GetLocationListOutModel
        {
            public List<ListInfoWithClass> List { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [Route("finApi/GetLocationList", Name = "GetLocationList")]
        public GetLocationListOutModel GetLocationList()
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("FinanceApiController.GetLocationList UserID={0}, UserName={1}", user.User.ID, user.User.Name);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = LocatedActivesSearchAsTable.GetLocationList(dataSource);
                    //
                    return new GetLocationListOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetLocationListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion
    }
}