using InfraManager.Web.BLL.Tables;
using InfraManager.Web.DTL.Tables;
using InfraManager.Web.Helpers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.Web.Controllers.Mobile
{
    public partial class MobileApiController : BaseApiController
    {
        #region GetListForObjectMobile
        //for ko.listView
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("mobileApi/GetListForObjectMobile", Name = "GetListForObjectMobile")]
        public ResultData<List<BaseForTable>> GetListForObjectMobile([FromQuery] TableLoadRequestInfoMobile requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.SD);
        }
        #endregion

        #region GetListForObjectMobile
        //for ko.listView
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("mobileApi/Search_Get", Name = "Search_Get")]
        public ResultData<List<BaseForTable>> Search_Get([FromQuery] TableLoadRequestInfoMobileSearch requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.SDMobileSearch);
        }
        #endregion

        #region GetListForObjectMobile
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("mobileApi/Search", Name = "Search")]
        public ResultData<List<BaseForTable>> Search(TableLoadRequestInfoMobileSearch requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.SDMobileSearch);
        }
        #endregion


        #region GetListForObjectMobile
        //for ko.listView
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("mobileApi/CommonSearch_Get", Name = "CommonSearch_Get")]
        public ResultData<List<BaseForTable>> CommonSearch_Get([FromQuery] TableLoadRequestInfoMobileSearch requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.SDMobileCommonSearch);
        }
        #endregion

        #region GetListForObjectMobile
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("mobileApi/CommonSearch", Name = "CommonSearch")]
        public ResultData<List<BaseForTable>> CommonSearch(TableLoadRequestInfoMobileSearch requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.SDMobileCommonSearch);
        }
        #endregion
    }
}