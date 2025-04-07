using InfraManager.Core.Data;
using InfraManager.Core.Logging;
using InfraManager.Web.BLL;
using InfraManager.Web.BLL.Assets.AssetSearch;
using InfraManager.Web.BLL.Tables;
using InfraManager.Web.DTL.Tables;
using InfraManager.Web.Helpers;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.Web.Controllers.IM
{
    public partial class AssetApiController
    {
        #region method SearchByClientAndService
        public sealed class SearchByClientAndServiceInModel
        {
            public string Query { get; set; }
            public bool NeedClient { get; set; }
            public bool NeedService { get; set; }
            public int StartPosition { get; set; }
            public Guid? ServiceID { get; set; }
            public Guid? ClientID { get; set; }
        }

        public sealed class SearchByClientAndServiceOutModel
        {
            public IList<AssetLink> ClientList { get; set; }
            public IList<AssetLink> ServiceList { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [Route("imApi/SearchByClientAndService", Name = "SearchByClientAndService")]
        public SearchByClientAndServiceOutModel SearchByClientAndService([FromQuery]SearchByClientAndServiceInModel searchParams)
        {
            var user = base.CurrentUser;
            //
            if ((!searchParams.NeedClient && !searchParams.NeedService && searchParams.StartPosition > 0) ||
                (!searchParams.ServiceID.HasValue && !searchParams.ClientID.HasValue) ||
                (!searchParams.ServiceID.HasValue && searchParams.NeedService) ||
                (!searchParams.ClientID.HasValue && searchParams.NeedClient))
                return new SearchByClientAndServiceOutModel() { ClientList = null, ServiceList = null, Result = RequestResponceType.NullParamsError };
            //
            searchParams.Query = System.Web.HttpUtility.UrlDecode(searchParams.Query);
            if (searchParams.StartPosition == 0 && !searchParams.NeedClient && !searchParams.NeedService)
            {
                searchParams.NeedClient = true;
                searchParams.NeedService = true;
            }
            //
            Logger.Trace("AssetApiController.SearchByClientAndService SearchText={0}, NeedClient={1}, NeedService={2}, StartPosition={3}, UserID={4}, UserName={5}",
                            searchParams.Query, searchParams.NeedClient, searchParams.NeedService, searchParams.StartPosition, user.User.ID, user.User.Name);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = AssetLink.GetListByClientAndService(searchParams.Query, searchParams.NeedClient ? searchParams.ClientID : null, searchParams.NeedService ? searchParams.ServiceID : null, searchParams.StartPosition, user.User.ID, dataSource);
                    //
                    return new SearchByClientAndServiceOutModel() { ClientList = retval.Item1, ServiceList = retval.Item2, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new SearchByClientAndServiceOutModel() { ClientList = null, ServiceList = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region (Table methods)
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("imApi/GetAssetSearchListForTable", Name = "GetAssetSearchListForTable")]
        public TableHelper.GetTableOutModel GetAssetSearchForTable([FromForm] AssetSearchTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForTable(requestInfo, user, TableHelper.TableType.AssetSearch);
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("imApi/GetAssetModelSearchListForTable", Name = "GetAssetModelSearchForTable")]
        public ResultData<List<BaseForTable>> GetAssetModelSearchForTable([FromForm] AssetModelSearchTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.AssetModelSearch);
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("imApi/GetSoftwareModelSearchForTable", Name = "GetSoftwareModelSearchForTable")]
        public ResultData<List<BaseForTable>> GetSoftwareModelSearchForTable([FromForm] AssetModelSearchTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.SoftwareLicenceModel);
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("finApi/GetContractAndAgreementSearchListForTable", Name = "GetContractAndAgreementSearchListForTable")]
        public ResultData<List<BaseForTable>> GetContractAndAgreementSearchListForTable([FromForm] AssetModelSearchTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.ContractAndAgreementSearch);
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("imApi/GetProcessorModelSearchForTable", Name = "GetProcessorModelSearchForTable")]
        public ResultData<List<BaseForTable>> GetProcessorModelSearchForTable([FromForm] AssetModelSearchTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.ProcessorSimpleSearch);
        }
        #endregion

        #region method GetSearchedObjectByID
        public sealed class GetSearchedObjectByIDInModel
        {
            public List<Guid> AssetIDList { get; set; }
            public bool ShowKE { get; set; }
            public bool IsHostClusterForm { get; set; }
            public bool IsConfigurationUnitAgentForm { get; set; }
            public bool IsVMClusterForm { get; set; }
            public bool IsInventory { get; set; }

        }

        public sealed class GetSearchedObjectByIDOutModel
        {
            public List<AssetLink> AssetLinkList { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [Route("imApi/GetSearchedObjectByID", Name = "GetSearchedObjectByID")]
        public GetSearchedObjectByIDOutModel GetSearchedObjectByID([FromForm]GetSearchedObjectByIDInModel model)
        {
            var user = base.CurrentUser;
            //
            if (model == null || model.AssetIDList == null || model.AssetIDList.Count == 0)
                return new GetSearchedObjectByIDOutModel() { AssetLinkList = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.GetSearchedObjectByID UserID={0}, UserName={1}", user.User.ID, user.User.Name);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = AssetLink.GetListByID(model.AssetIDList, model.ShowKE, model.IsConfigurationUnitAgentForm, model.IsHostClusterForm, model.IsVMClusterForm, model.IsInventory, user.User, dataSource);
                    //
                    return new GetSearchedObjectByIDOutModel() { AssetLinkList = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetSearchedObjectByIDOutModel() { AssetLinkList = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetTableFiltersData
        public sealed class GetAssetLinkDataWithClassOutModel
        {
            public List<ListInfoWithClass> List { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetAssetLinkDataOutModel
        {
            public List<ListInfo> List { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [Route("imApi/GetAssetLinkTypes", Name = "GetAssetLinkTypes")]
        public GetAssetLinkDataWithClassOutModel GetAssetLinkTypes()
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("AssetApiController.GetAssetLinkTypes UserID={0}, UserName={1}", user.User.ID, user.User.Name);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = AssetSearchAsTable.GetTypesList(dataSource);
                    //
                    return new GetAssetLinkDataWithClassOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetAssetLinkDataWithClassOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }

        [HttpGet]
        [Route("imApi/GetAssetLinkModels", Name = "GetAssetLinkModels")]
        public GetAssetLinkDataOutModel GetAssetLinkModels([FromQuery] Guid typeID)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("AssetApiController.GetAssetLinkModels typeID={0} UserID={1}, UserName={2}", typeID, user.User.ID, user.User.Name);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = AssetSearchAsTable.GetModelsList(typeID, dataSource);
                    //
                    return new GetAssetLinkDataOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetAssetLinkDataOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }

        public sealed class GetAssetLinkForTypesInModel
        {
            public List<Guid> Types { get; set; }
        }

        [HttpPost]
        [Route("imApi/GetAssetLinkModelsForTypes", Name = "GetAssetLinkModelsForTypes")]
        public GetAssetLinkDataOutModel GetAssetLinkModelsForTypes([FromForm] GetAssetLinkForTypesInModel model)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("AssetApiController.GetAssetLinkModelsForTypes UserID={0}, UserName={1}", user.User.ID, user.User.Name);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = AssetSearchAsTable.GetModelsList(model.Types, dataSource);
                    //
                    return new GetAssetLinkDataOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetAssetLinkDataOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }

        [HttpPost]
        [Route("imApi/GetAssetLinkVendorsForTypes", Name = "GetAssetLinkVendorsForTypes")]
        public GetAssetLinkDataOutModel GetAssetLinkVendors([FromForm] GetAssetLinkForTypesInModel model)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("AssetApiController.GetAssetLinkVendors UserID={0}, UserName={1}", user.User.ID, user.User.Name);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = AssetSearchAsTable.GetVendorsList(model.Types, dataSource);
                    //
                    return new GetAssetLinkDataOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetAssetLinkDataOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }

        [HttpGet]
        [Route("imApi/GetAssetLinkVendors", Name = "GetAssetLinkVendors")]
        public GetAssetLinkDataOutModel GetAssetLinkVendors([FromQuery] Guid typeID)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("AssetApiController.GetAssetLinkVendors typeID={0} UserID={1}, UserName={2}", typeID, user.User.ID, user.User.Name);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = AssetSearchAsTable.GetVendorsList(typeID, dataSource);
                    //
                    return new GetAssetLinkDataOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetAssetLinkDataOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion
    }
}