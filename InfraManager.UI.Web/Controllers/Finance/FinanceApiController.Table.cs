using InfraManager.Web.BLL.Tables;
using InfraManager.Web.DTL.Tables;
using InfraManager.Web.Helpers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.Web.Controllers.Finance
{
    public sealed partial class FinanceApiController : BaseApiController
    {
        #region (Table methods)
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("finApi/GetListForTable", Name = "FinGetListForTable")]
        public TableHelper.GetTableOutModel GetListForTable([FromForm] TableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForTable(requestInfo, user, TableHelper.TableType.Finance);
        }
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("finApi/GetListForObject", Name = "FinGetListForObject")]
        public ResultData<List<BaseForTable>> GetListForObject([FromForm] TableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.Finance);
        }

        //старый метод для получения того, что есть в составе у закупок для формирования накладной в закупках
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("finApi/GetPurchaseSpecificationSearchListForTable", Name = "GetPurchaseSpecificationSearchListForTable")]
        public TableHelper.GetTableOutModel GetPurchaseSpecificationSearchListForTable([FromForm] AssetSearchTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForTable(requestInfo, user, TableHelper.TableType.PurchaseSpecificationSearch);
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("finApi/GetGoodsInvoiceSpecificationSearchListForTable", Name = "GetGoodsInvoiceSpecificationSearchListForTable")]
        public TableHelper.GetTableOutModel GetGoodsInvoiceSpecificationSearchListForTable([FromForm] AssetSearchTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForTable(requestInfo, user, TableHelper.TableType.GoodsInvoiceSpecificationSearch);
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("finApi/GetActivesRequestSpecificationListForTable", Name = "GetActivesRequestSpecificationListForTable")]
        public ResultData<List<BaseForTable>> GetActivesRequestSpecificationListForTable([FromForm] AssetSearchTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.ARS);
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("finApi/GetPurchaseSpecificationListForTable", Name = "GetPurchaseSpecificationListForTable")]
        public ResultData<List<BaseForTable>> GetPurchaseSpecificationListForTable([FromForm] AssetSearchTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.PurchaseSpecificationSearch);
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("finApi/GetLocatedActivesSearchListForTable", Name = "GetLocatedActivesSearchListForTable")]
        public TableHelper.GetTableOutModel GetLocatedActivesSearchListForTable([FromForm] AssetSearchTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForTable(requestInfo, user, TableHelper.TableType.LocatedActivesSearch);
        }
        #endregion

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("finApi/GetLocatedActivesSearchListForObject", Name = "GetLocatedActivesSearchListForObject")]
        public ResultData<List<BaseForTable>> GetLocatedActivesSearchListForObject([FromForm] AssetSearchTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.LocatedActivesSearch);
        }
    }
}