using InfraManager.Core.Data;
using InfraManager.Core.Exceptions;
using InfraManager.Core.Logging;
using InfraManager.Web.BLL;
using InfraManager.Web.BLL.Fields;
using InfraManager.Web.BLL.Finance;
using InfraManager.Web.BLL.Finance.GoodsInvoice;
using InfraManager.Web.BLL.Finance.Specification;
using InfraManager.Web.BLL.Helpers;
using InfraManager.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Resources = InfraManager.ResourcesArea.Resources;
using System.Globalization;

namespace InfraManager.Web.Controllers.Finance
{
    public sealed partial class FinanceApiController
    {
        #region ActivesRequestSpecification

        #region method GetActivesRequestSpecification
        public sealed class GetActivesRequestSpecificationIncomingModel
        {
            public Guid WorkOrderID { get; set; }
            public Guid SpecificationID { get; set; }
        }
        public sealed class GetActivesRequestSpecificationOutModel
        {
            public ActivesRequestSpecification Elem { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetActivesRequestSpecification", Name = "GetActivesRequestSpecification")]
        public GetActivesRequestSpecificationOutModel GetActivesRequestSpecification([FromQuery] GetActivesRequestSpecificationIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetActivesRequestSpecificationOutModel() { Elem = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("FinanceApiController.GetActivesRequestSpecification userID={0}, userName={1}, objID={2}, specificationID={3}", user.Id, user.UserName, model.WorkOrderID, model.SpecificationID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!BLL.SD.WorkOrders.WorkOrder.AccessIsGranted(model.WorkOrderID, user.User, true, dataSource))
                    {
                        Logger.Error("FinanceApiController.GetActivesRequestSpecification userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.WorkOrderID);
                        return new GetActivesRequestSpecificationOutModel() { Elem = null, Result = RequestResponceType.AccessError };
                    }
                    //
                    var retval = ActivesRequestSpecification.Get(model.SpecificationID, dataSource);
                    return new GetActivesRequestSpecificationOutModel() { Elem = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException)
            {
                return new GetActivesRequestSpecificationOutModel() { Elem = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetActivesRequestSpecification, model: {0}.", model);
                return new GetActivesRequestSpecificationOutModel() { Elem = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetActivesRequestSpecificationList
        public sealed class GetActivesRequestSpecificationListIncomingModel
        {
            public Guid WorkOrderID { get; set; }
            public bool WithDeclined { get; set; }
        }
        public sealed class GetActivesRequestSpecificationListOutModel
        {
            public IList<ActivesRequestSpecification> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        //[HttpGet]
        //[AcceptVerbs("GET")]
        //[Route("finApi/GetActivesRequestSpecificationList", Name = "GetActivesRequestSpecificationList")]
        //public GetActivesRequestSpecificationListOutModel GetActivesRequestSpecificationList([FromQuery] GetActivesRequestSpecificationListIncomingModel model)
        //{
        //    try
        //    {
        //        var user = base.CurrentUser;
        //        if (user == null)
        //            return new GetActivesRequestSpecificationListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
        //        //
        //        Logger.Trace("FinanceApiController.GetActivesRequestSpecificationList userID={0}, userName={1}, objID={2}, withDeclined={3}", user.Id, user.UserName, model.WorkOrderID, model.WithDeclined);
        //        //
        //        using (var dataSource = DataSource.GetDataSource())
        //        {
        //            if (!BLL.SD.WorkOrders.WorkOrder.AccessIsGranted(model.WorkOrderID, user.User, true, dataSource))
        //            {
        //                Logger.Error("FinanceApiController.GetActivesRequestSpecificationList userID={0}, userName={1}, objID={2}, withDeclined={3}  failed (access denied)", user.Id, user.UserName, model.WorkOrderID, model.WithDeclined);
        //                return new GetActivesRequestSpecificationListOutModel() { List = null, Result = RequestResponceType.AccessError };
        //            }
        //            var retval = ActivesRequestSpecification.GetList(model.WorkOrderID, model.WithDeclined, null, dataSource);
        //            //
        //            return new GetActivesRequestSpecificationListOutModel() { List = retval, Result = RequestResponceType.Success };
        //        }
        //    }
        //    catch (NotSupportedException ex)
        //    {
        //        Logger.Error(ex, String.Format(@"GetActivesRequestSpecificationList not supported, model: '{0}'", model));
        //        return new GetActivesRequestSpecificationListOutModel() { List = null, Result = RequestResponceType.BadParamsError };
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex, "GetActivesRequestSpecificationList, model: {0}.", model);
        //        return new GetActivesRequestSpecificationListOutModel() { List = null, Result = RequestResponceType.GlobalError };
        //    }
        //}
        #endregion

        #region method GetActivesRequestSpecificationListForNegotiation
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetActivesRequestSpecificationListForNegotiation", Name = "GetActivesRequestSpecificationListForNegotiation")]
        public GetActivesRequestSpecificationListOutModel GetActivesRequestSpecificationListForNegotiation([FromQuery] GetActivesRequestSpecificationListIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetActivesRequestSpecificationListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("FinanceApiController.GetActivesRequestSpecificationListForNegotiation userID={0}, userName={1}, objID={2}", user.Id, user.UserName, model.WorkOrderID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!BLL.SD.WorkOrders.WorkOrder.AccessIsGranted(model.WorkOrderID, user.User, true, dataSource))
                    {
                        Logger.Error("FinanceApiController.GetActivesRequestSpecificationListForNegotiation userID={0}, userName={1}, objID={2}  failed (access denied)", user.Id, user.UserName, model.WorkOrderID);
                        return new GetActivesRequestSpecificationListOutModel() { List = null, Result = RequestResponceType.AccessError };
                    }
                    var retval = ActivesRequestSpecification.GetListForNegotiation(model.WorkOrderID, null, dataSource);
                    //
                    return new GetActivesRequestSpecificationListOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetActivesRequestSpecificationListForNegotiation not supported, model: '{0}'", model));
                return new GetActivesRequestSpecificationListOutModel() { List = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetActivesRequestSpecificationListForNegotiation, model: {0}.", model);
                return new GetActivesRequestSpecificationListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetActivesRequestSpecificationsEnums
        public sealed class GetActivesRequestSpecificationsEnumsOutModel
        {
            public IList<ListInfo> StateList { get; set; }
            public IList<ListInfo> NDSTypeList { get; set; }
            public IList<ListInfo> NDSPercentList { get; set; }
            public IList<ListInfo> UnitList { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetActivesRequestSpecificationsEnums", Name = "GetActivesRequestSpecificationsEnums")]
        public GetActivesRequestSpecificationsEnumsOutModel GetActivesRequestSpecificationsEnums()
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetActivesRequestSpecificationsEnumsOutModel() { StateList = null, NDSTypeList = null, NDSPercentList = null, UnitList = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("FinanceApiController.GetActivesRequestSpecificationsEnums userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var stateList = ActivesRequestSpecification.GetStateList(HttpContext?.GetCurrentCulture() ?? CultureInfo.CurrentCulture.Name);
                    var typeList = ActivesRequestSpecification.GetNDSTypeList(HttpContext?.GetCurrentCulture() ?? CultureInfo.CurrentCulture.Name);
                    var percentList = ActivesRequestSpecification.GetNDSPercentList(HttpContext?.GetCurrentCulture() ?? CultureInfo.CurrentCulture.Name);
                    var unitList = PurchaseSpecification.GetUnitList(dataSource);
                    return new GetActivesRequestSpecificationsEnumsOutModel() { StateList = stateList, NDSTypeList = typeList, NDSPercentList = percentList, UnitList = unitList, Result = RequestResponceType.Success };

                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetActivesRequestSpecificationsEnums");
                return new GetActivesRequestSpecificationsEnumsOutModel() { StateList = null, NDSTypeList = null, NDSPercentList = null, UnitList = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method EditActivesRequestSpecification
        public sealed class EditActivesRequestSpecificationOutModel
        {
            public ActivesRequestSpecification NewModel { get; set; }
            public ResultWithMessage Response { get; set; }
        }
        [HttpPost]
        [Route("finApi/EditActivesRequestSpecification", Name = "EditActivesRequestSpecification")]
        public EditActivesRequestSpecificationOutModel EditActivesRequestSpecification(ActivesRequestSpecificationInfo model)
        {
            var user = base.CurrentUser;
            if (model != null && user != null)
            {
                try
                {
                    int operationID = 0;
                    switch (model.Operation)
                    {
                        case FieldOperation.Create:
                            operationID = IMSystem.Global.OPERATION_ActivesRequestSpecification_Add;
                            break;
                        case FieldOperation.Edit:
                            operationID = IMSystem.Global.OPERATION_ActivesRequestSpecification_Update;
                            break;
                        case FieldOperation.Remove:
                            operationID = IMSystem.Global.OPERATION_ActivesRequestSpecification_Delete;
                            break;
                    }
                    if (!user.User.OperationIsGranted(operationID))
                    {
                        Logger.Trace("FinanceApiController.EditActivesRequestSpecification userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                        return new EditActivesRequestSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.OperationError) };
                    }
                    //
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        if (!BLL.SD.WorkOrders.WorkOrder.AccessIsGranted(model.WorkOrderID, user.User, true, dataSource))
                        {
                            Logger.Error("FinanceApiController.EditActivesRequestSpecification userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.WorkOrderID);
                            return new EditActivesRequestSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.AccessError) };
                        }
                        var result = ActivesRequestSpecification.Edit(model, dataSource, user.User);
                        WorkflowWrapper.MakeOnSaveReaction(model.WorkOrderID, IMSystem.Global.OBJ_WORKORDER, dataSource, user.User);
                        //
                        return new EditActivesRequestSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.Success), NewModel = result };
                    }
                }
                catch (NotSupportedException e)
                {
                    Logger.Error(e, String.Format(@"EditActivesRequestSpecification not supported, model: '{0}'", model));
                    return new EditActivesRequestSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.BadParamsError) };
                }
                catch (ObjectDeletedException)
                {
                    Logger.Trace(String.Format(@"EditActivesRequestSpecification object deleted, model: '{0}'", model));
                    return new EditActivesRequestSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.ObjectDeleted) };
                }
                catch (ArgumentValidationException e)
                {
                    Logger.Trace(String.Format(@"EditActivesRequestSpecification validation error, model: '{0}'", model));
                    return new EditActivesRequestSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.ValidationError, ValidatorHelper.CreateErrorMessage(e)) };
                }
                catch (Exception e)
                {
                    Logger.Error(e, String.Format(@"EditActivesRequestSpecification, model: '{0}'", model));
                    return new EditActivesRequestSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.GlobalError) };
                }
            }
            else return new EditActivesRequestSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.NullParamsError) };
        }
        #endregion

        #region method RemoveSpecification
        public sealed class RemoveActivesRequestSpecificationIncomingModel
        {
            public List<Guid> IDList { get; set; }
            public Guid WorkOrderID { get; set; }
        }
        //
        [HttpPost]
        [Route("finApi/RemoveActivesRequestSpecification", Name = "RemoveActivesRequestSpecification")]
        public RequestResponceType RemoveActivesRequestSpecification(RemoveActivesRequestSpecificationIncomingModel model)
        {
            var user = base.CurrentUser;
            if (model != null && user != null)
            {
                try
                {
                    if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_ActivesRequestSpecification_Delete))
                    {
                        Logger.Trace("FinanceApiController.RemoveActivesRequestSpecification userID={0}, userName={1} failed (operation denied)", user.Id, user.UserName);
                        return RequestResponceType.OperationError;
                    }
                    //
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        if (!BLL.SD.WorkOrders.WorkOrder.AccessIsGranted(model.WorkOrderID, user.User, true, dataSource))
                        {
                            Logger.Error("FinanceApiController.RemoveActivesRequestSpecification userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.WorkOrderID);
                            return RequestResponceType.AccessError;
                        }
                        foreach (var id in model.IDList)
                        {
                            var info = new ActivesRequestSpecificationInfo() { ID = id, WorkOrderID = model.WorkOrderID, Operation = FieldOperation.Remove };
                            ActivesRequestSpecification.Edit(info, dataSource, user.User);
                        }
                        WorkflowWrapper.MakeOnSaveReaction(model.WorkOrderID, IMSystem.Global.OBJ_WORKORDER, dataSource, user.User);
                        //
                        return RequestResponceType.Success;
                    }
                }
                catch (NotSupportedException e)
                {
                    Logger.Error(e, String.Format(@"RemoveActivesRequestSpecification not supported, model: '{0}'", model));
                    return RequestResponceType.BadParamsError;
                }
                catch (ObjectDeletedException)
                {
                    Logger.Trace(String.Format(@"RemoveActivesRequestSpecification object deleted, model: '{0}'", model));
                    return RequestResponceType.ObjectDeleted;
                }
                catch (ArgumentValidationException)
                {
                    Logger.Trace(String.Format(@"RemoveActivesRequestSpecification validation error, model: '{0}'", model));
                    return RequestResponceType.ValidationError;
                }
                catch (Exception e)
                {
                    Logger.Error(e, String.Format(@"RemoveActivesRequestSpecification, model: '{0}'", model));
                    return RequestResponceType.GlobalError;
                }
            }
            else return RequestResponceType.NullParamsError;
        }
        #endregion

        #region method AddSpecificationList
        public sealed class AddActivesRequestSpecificationListInputModel
        {
            public List<DTL.Finance.AddFromCatalogueSpecificationInfo> ModelsList { get; set; }
            public int ObjectClassID { get; set; }
            public Guid ObjectID { get; set; }
        }
        public sealed class AddActivesRequestSpecificationListOutModel
        {
            public IList<ActivesRequestSpecification> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("finApi/AddActivesRequestSpecificationList", Name = "AddActivesRequestSpecificationList")]
        public AddActivesRequestSpecificationListOutModel AddActivesRequestSpecificationList(AddActivesRequestSpecificationListInputModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new AddActivesRequestSpecificationListOutModel() { List = null, Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace("FinanceApiController.AddActivesRequestSpecificationList userID={0}, userName={1}, ModelsListCount={2}, ObjectClassID={3}, ObjectID={4}",
            user.Id, user.UserName, model.ModelsList.Count, model.ObjectClassID, model.ObjectID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    IList<ActivesRequestSpecification> retval = null;
                    //
                    retval = BLL.SD.WorkOrders.WorkOrder.AddActivesRequestSpecificationsModels(model.ObjectID, model.ModelsList, dataSource, user.User);
                    //
                    WorkflowWrapper.MakeOnSaveReaction(model.ObjectID, model.ObjectClassID, dataSource, user.User);
                    return new AddActivesRequestSpecificationListOutModel() { List = retval, Result = (RequestResponceType.Success) };
                }
            }
            catch (FieldConcurrencyException e)
            {
                Logger.Error(e, String.Format(@"AddActivesRequestSpecificationList concurency error. ObjClassID: '{2}'. Id: '{0}'. Models: '{1}'",
                        model.ObjectID.ToString(),
                        String.Join(@"\n, ", model.ModelsList.Select(d => d.ModelID.ToString())),
                        model.ObjectClassID));
                return new AddActivesRequestSpecificationListOutModel() { List = null, Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectConcurrencyException e)
            {
                Logger.Error(e, String.Format(@"AddActivesRequestSpecificationList OBJECT concurency error. ObjClassID: '{2}'. Id: '{0}'. Models: '{1}'",
                         model.ObjectID.ToString(),
                        String.Join(@"\n, ", model.ModelsList.Select(d => d.ModelID.ToString())),
                        model.ObjectClassID));
                return new AddActivesRequestSpecificationListOutModel() { List = null, Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectDeletedException e)
            {
                Logger.Error(e, String.Format(@"AddActivesRequestSpecificationList object deleted error. ObjClassID: '{2}'. Id: '{0}'. Models: '{1}'",
                            model.ObjectID.ToString(),
                        String.Join(@"\n, ", model.ModelsList.Select(d => d.ModelID.ToString())),
                        model.ObjectClassID));
                return new AddActivesRequestSpecificationListOutModel() { List = null, Result = (RequestResponceType.ObjectDeleted) };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при добавлении спецификаций.");
                return new AddActivesRequestSpecificationListOutModel() { List = null, Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion

        #endregion

        #region PurchaseSpecification

        #region method GetPurchaseSpecification
        public sealed class GetPurchaseSpecificationIncomingModel
        {
            public Guid WorkOrderID { get; set; }
            public Guid SpecificationID { get; set; }
        }
        public sealed class GetPurchaseSpecificationOutModel
        {
            public PurchaseSpecification Elem { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetPurchaseSpecification", Name = "GetPurchaseSpecification")]
        public GetPurchaseSpecificationOutModel GetPurchaseSpecification([FromQuery] GetPurchaseSpecificationIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetPurchaseSpecificationOutModel() { Elem = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("FinanceApiController.GetPurchaseSpecification userID={0}, userName={1}, objID={2}, specificationID={3}", user.Id, user.UserName, model.WorkOrderID, model.SpecificationID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!BLL.SD.WorkOrders.WorkOrder.AccessIsGranted(model.WorkOrderID, user.User, true, dataSource))
                    {
                        Logger.Error("FinanceApiController.GetPurchaseSpecification userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.WorkOrderID);
                        return new GetPurchaseSpecificationOutModel() { Elem = null, Result = RequestResponceType.AccessError };
                    }
                    //
                    var retval = PurchaseSpecification.Get(model.SpecificationID, dataSource);
                    return new GetPurchaseSpecificationOutModel() { Elem = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException)
            {
                return new GetPurchaseSpecificationOutModel() { Elem = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetPurchaseSpecification, model: {0}.", model);
                return new GetPurchaseSpecificationOutModel() { Elem = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetPurchaseSpecificationList
        public sealed class GetPurchaseSpecificationListIncomingModel
        {
            public Guid WorkOrderID { get; set; }
            public bool WithDeclined { get; set; }
        }
        public sealed class GetPurchaseSpecificationListOutModel
        {
            public IList<PurchaseSpecification> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        //[HttpGet]
        //[AcceptVerbs("GET")]
        //[Route("finApi/GetPurchaseSpecificationList", Name = "GetPurchaseSpecificationList")]
        //public GetPurchaseSpecificationListOutModel GetPurchaseSpecificationList([FromQuery] GetPurchaseSpecificationListIncomingModel model)
        //{
        //    try
        //    {
        //        var user = base.CurrentUser;
        //        if (user == null)
        //            return new GetPurchaseSpecificationListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
        //        //
        //        Logger.Trace("FinanceApiController.GetPurchaseSpecificationList userID={0}, userName={1}, objID={2}, withDeclined={3}", user.Id, user.UserName, model.WorkOrderID, model.WithDeclined);
        //        //
        //        using (var dataSource = DataSource.GetDataSource())
        //        {
        //            if (!BLL.SD.WorkOrders.WorkOrder.AccessIsGranted(model.WorkOrderID, user.User, true, dataSource))
        //            {
        //                Logger.Error("FinanceApiController.GetPurchaseSpecificationList userID={0}, userName={1}, objID={2}, withDeclined={3}  failed (access denied)", user.Id, user.UserName, model.WorkOrderID, model.WithDeclined);
        //                return new GetPurchaseSpecificationListOutModel() { List = null, Result = RequestResponceType.AccessError };
        //            }
        //            var retval = PurchaseSpecification.GetList(model.WorkOrderID, model.WithDeclined, null, dataSource);
        //            //
        //            return new GetPurchaseSpecificationListOutModel() { List = retval, Result = RequestResponceType.Success };
        //        }
        //    }
        //    catch (NotSupportedException ex)
        //    {
        //        Logger.Error(ex, String.Format(@"GetPurchaseSpecificationList not supported, model: '{0}'", model));
        //        return new GetPurchaseSpecificationListOutModel() { List = null, Result = RequestResponceType.BadParamsError };
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex, "GetPurchaseSpecificationList, model: {0}.", model);
        //        return new GetPurchaseSpecificationListOutModel() { List = null, Result = RequestResponceType.GlobalError };
        //    }
        //}
        #endregion

        #region method GetPurchaseSpecificationListForNegotiation
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetPurchaseSpecificationListForNegotiation", Name = "GetPurchaseSpecificationListForNegotiation")]
        public GetPurchaseSpecificationListOutModel GetPurchaseSpecificationListForNegotiation([FromQuery] GetPurchaseSpecificationListIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetPurchaseSpecificationListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("FinanceApiController.GetPurchaseSpecificationListForNegotiation userID={0}, userName={1}, objID={2}", user.Id, user.UserName, model.WorkOrderID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!BLL.SD.WorkOrders.WorkOrder.AccessIsGranted(model.WorkOrderID, user.User, true, dataSource))
                    {
                        Logger.Error("FinanceApiController.GetPurchaseSpecificationListForNegotiation userID={0}, userName={1}, objID={2}  failed (access denied)", user.Id, user.UserName, model.WorkOrderID);
                        return new GetPurchaseSpecificationListOutModel() { List = null, Result = RequestResponceType.AccessError };
                    }
                    var retval = PurchaseSpecification.GetListForNegotiation(model.WorkOrderID, null, dataSource);
                    //
                    return new GetPurchaseSpecificationListOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetPurchaseSpecificationListForNegotiation not supported, model: '{0}'", model));
                return new GetPurchaseSpecificationListOutModel() { List = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetPurchaseSpecificationListForNegotiation, model: {0}.", model);
                return new GetPurchaseSpecificationListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetPurchaseSpecificationsEnums
        public sealed class GetPurchaseSpecificationsEnumsOutModel
        {
            public IList<ListInfo> StateList { get; set; }
            public IList<ListInfo> NDSTypeList { get; set; }
            public IList<ListInfo> NDSPercentList { get; set; }
            public IList<ListInfo> UnitList { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetPurchaseSpecificationsEnums", Name = "GetPurchaseSpecificationsEnums")]
        public GetPurchaseSpecificationsEnumsOutModel GetPurchaseSpecificationsEnums()
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetPurchaseSpecificationsEnumsOutModel() { UnitList = null, StateList = null, NDSTypeList = null, NDSPercentList = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("FinanceApiController.GetPurchaseSpecificationsEnums userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var stateList = PurchaseSpecification.GetStateList(HttpContext?.GetCurrentCulture() ?? CultureInfo.CurrentCulture.Name);
                    var typeList = ActivesRequestSpecification.GetNDSTypeList(HttpContext?.GetCurrentCulture() ?? CultureInfo.CurrentCulture.Name);
                    var percentList = ActivesRequestSpecification.GetNDSPercentList(HttpContext?.GetCurrentCulture() ?? CultureInfo.CurrentCulture.Name);
                    var unitList = PurchaseSpecification.GetUnitList(dataSource);
                    return new GetPurchaseSpecificationsEnumsOutModel() { UnitList = unitList, StateList = stateList, NDSTypeList = typeList, NDSPercentList = percentList, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetPurchaseSpecificationsEnums");
                return new GetPurchaseSpecificationsEnumsOutModel() { UnitList = null, StateList = null, NDSTypeList = null, NDSPercentList = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method EditPurchaseSpecification
        public sealed class EditPurchaseSpecificationOutModel
        {
            public PurchaseSpecification NewModel { get; set; }
            public ResultWithMessage Response { get; set; }
        }
        [HttpPost]
        [Route("finApi/EditPurchaseSpecification", Name = "EditPurchaseSpecification")]
        public EditPurchaseSpecificationOutModel EditPurchaseSpecification(PurchaseSpecificationInfo model)
        {
            var user = base.CurrentUser;
            if (model != null && user != null)
            {
                try
                {
                    int operationID = 0;
                    switch (model.Operation)
                    {
                        case FieldOperation.Create:
                            operationID = IMSystem.Global.OPERATION_PurchaseSpecification_Add;
                            break;
                        case FieldOperation.Edit:
                            operationID = IMSystem.Global.OPERATION_PurchaseSpecification_Update;
                            break;
                        case FieldOperation.Remove:
                            operationID = IMSystem.Global.OPERATION_PurchaseSpecification_Delete;
                            break;
                    }
                    if (!user.User.OperationIsGranted(operationID))
                    {
                        Logger.Trace("FinanceApiController.EditPurchaseSpecification userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                        return new EditPurchaseSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.OperationError) };
                    }
                    //
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        if (!BLL.SD.WorkOrders.WorkOrder.AccessIsGranted(model.WorkOrderID, user.User, true, dataSource))
                        {
                            Logger.Error("FinanceApiController.EditPurchaseSpecification userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.WorkOrderID);
                            return new EditPurchaseSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.AccessError) };
                        }
                        var result = PurchaseSpecification.Edit(model, dataSource, user.User);
                        WorkflowWrapper.MakeOnSaveReaction(model.WorkOrderID, IMSystem.Global.OBJ_WORKORDER, dataSource, user.User);
                        //
                        return new EditPurchaseSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.Success), NewModel = result };
                    }
                }
                catch (NotSupportedException e)
                {
                    Logger.Error(e, String.Format(@"EditPurchaseSpecification not supported, model: '{0}'", model));
                    return new EditPurchaseSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.BadParamsError) };
                }
                catch (ObjectDeletedException)
                {
                    Logger.Trace(String.Format(@"EditPurchaseSpecification object deleted, model: '{0}'", model));
                    return new EditPurchaseSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.ObjectDeleted) };
                }
                catch (ArgumentValidationException e)
                {
                    Logger.Trace(String.Format(@"EditPurchaseSpecification validation error, model: '{0}'", model));
                    return new EditPurchaseSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.ValidationError, ValidatorHelper.CreateErrorMessage(e)) };
                }
                catch (Exception e)
                {
                    Logger.Error(e, String.Format(@"EditPurchaseSpecification, model: '{0}'", model));
                    return new EditPurchaseSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.GlobalError) };
                }
            }
            else return new EditPurchaseSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.NullParamsError) };
        }
        #endregion

        #region method GetContractOrAgreementInfoForMarkAsExecuted
        public sealed class GetContractOrAgreementInfoForMarkAsExecutedOutModel
        {
            public List<ListInfo> StateList { get; set; }
            public string CurrentState { get; set; }
            public Guid WorkOrderID { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [Route("finApi/GetContractOrAgreementInfoForMarkAsExecuted", Name = "GetContractOrAgreementInfoForMarkAsExecuted")]
        public GetContractOrAgreementInfoForMarkAsExecutedOutModel GetContractOrAgreementInfoForMarkAsExecuted([FromQuery] Guid purchaseSpecificationID)
        {
            var user = base.CurrentUser;
            if (user != null)
            {
                try
                {
                    if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PurchaseSpecification_Properties))
                    {
                        Logger.Trace("FinanceApiController.GetContractOrAgreementInfoForMarkAsExecuted userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, purchaseSpecificationID);
                        return new GetContractOrAgreementInfoForMarkAsExecutedOutModel() { Result = RequestResponceType.OperationError };
                    }
                    //
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        var ps = PurchaseSpecification.Get(purchaseSpecificationID, dataSource);
                        if (ps.State != ActivesRequestSpecificationState.Purchasing)
                        {
                            Logger.Error("FinanceApiController.GetContractOrAgreementInfoForMarkAsExecuted userID={0}, userName={1}, purchaseSpecificationID={2} failed (state of specification isn't purchsing)", user.Id, user.UserName, purchaseSpecificationID);
                            return new GetContractOrAgreementInfoForMarkAsExecutedOutModel() { Result = RequestResponceType.BadParamsError };
                        }
                        if (!BLL.SD.WorkOrders.WorkOrder.AccessIsGranted(ps.WorkOrderID, user.User, true, dataSource))
                        {
                            Logger.Error("FinanceApiController.GetContractOrAgreementInfoForMarkAsExecuted userID={0}, userName={1}, workOrderID={2} failed (access denied)", user.Id, user.UserName, ps.WorkOrderID);
                            return new GetContractOrAgreementInfoForMarkAsExecutedOutModel() { Result = RequestResponceType.AccessError };
                        }
                        //
                        var retval = new GetContractOrAgreementInfoForMarkAsExecutedOutModel();
                        retval.WorkOrderID = ps.WorkOrderID;
                        switch (ps.ProductCatalogModelClassID)
                        {
                            case IMSystem.Global.OBJ_ServiceContract:
                                {
                                    var serviceContract = BLL.Contracts.Contract.Get(ps.ProductCatalogModelID.Value, user.User.ID, dataSource);
                                    retval.CurrentState = serviceContract.LifeCycleStateName;
                                    var menu = BLL.Assets.AssetContextMenu
                                        .Get(
                                            InfraManager.IM.BusinessLayer.User.Get(new Guid(user.Id), dataSource),
                                            new List<DTL.ObjectInfo>
                                            {
                                                new DTL.ObjectInfo
                                                {
                                                    ClassID = ps.ProductCatalogModelClassID.Value,
                                                    ID = ps.ProductCatalogModelID.Value
                                                },
                                            })
                                        .ToList();
                                    menu.RemoveAll(delegate (BLL.Assets.AssetContextMenu v) {
                                        if (!v.Enabled)
                                            return true;
                                        return v.ObjectClassID != null;
                                    });

                                    retval.StateList = menu.
                                         Select(x => new ListInfo() { ID = x.LifeCycleStateOperationID.ToString(), Name = x.Name }).
                                         ToList();

                                }
                                break;
                            case IMSystem.Global.OBJ_ServiceContractAgreement:
                                {
                                    var serviceContractAgreement = BLL.Contracts.ContractAgreement.Get(ps.ProductCatalogModelID.Value, dataSource);
                                    retval.CurrentState = serviceContractAgreement.LifeCycleStateName;

                                    var menu = BLL.Assets.AssetContextMenu.Get(
                                        InfraManager.IM.BusinessLayer.User.Get(new Guid(user.Id), dataSource),
                                        new List<DTL.ObjectInfo> 
                                        { 
                                            new DTL.ObjectInfo 
                                            { 
                                                ClassID = ps.ProductCatalogModelClassID.Value, 
                                                ID = ps.ProductCatalogModelID.Value 
                                            }, 
                                        });
                                    retval.StateList = menu.
                                        Select(x => new ListInfo() { ID = x.LifeCycleStateOperationID.ToString(), Name = x.Name }).
                                        ToList();
                                }
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                        //
                        retval.Result = RequestResponceType.Success;
                        return retval;
                    }
                }
                catch (NotSupportedException e)
                {
                    Logger.Error(e, String.Format(@"GetContractOrAgreementInfoForMarkAsExecuted not supported, purchaseSpecificationID: '{0}'", purchaseSpecificationID));
                    return new GetContractOrAgreementInfoForMarkAsExecutedOutModel() { Result = RequestResponceType.BadParamsError };
                }
                catch (ObjectDeletedException)
                {
                    Logger.Trace(String.Format(@"GetContractOrAgreementInfoForMarkAsExecuted object deleted, purchaseSpecificationID: '{0}'", purchaseSpecificationID));
                    return new GetContractOrAgreementInfoForMarkAsExecutedOutModel() { Result = RequestResponceType.ObjectDeleted };
                }
                catch (Exception e)
                {
                    Logger.Error(e, String.Format(@"GetContractOrAgreementInfoForMarkAsExecuted, purchaseSpecificationID: '{0}'", purchaseSpecificationID));
                    return new GetContractOrAgreementInfoForMarkAsExecutedOutModel() { Result = RequestResponceType.GlobalError };
                }
            }
            else return new GetContractOrAgreementInfoForMarkAsExecutedOutModel() { Result = RequestResponceType.NullParamsError };
        }
        #endregion

        #region method MarkAsExecutedOnContractOrAgreement
        public sealed class MarkAsExecutedOnContractOrAgreementInModel
        {
            public Guid PurchaseSpecificationID { get; set; }
            public string NewStateID { get; set; }          
        }
        [HttpPost]
        [Route("finApi/MarkAsExecutedOnContractOrAgreement", Name = "MarkAsExecutedOnContractOrAgreement")]
        public ResultWithMessage MarkAsExecutedOnContractOrAgreement(MarkAsExecutedOnContractOrAgreementInModel model)
        {
            var user = base.CurrentUser;
            if (user != null)
            {
                try
                {
                    int operationID = IMSystem.Global.OPERATION_PurchaseSpecification_Update;
                    if (!user.User.OperationIsGranted(operationID))
                    {
                        Logger.Trace("FinanceApiController.MarkAsExecutedOnContractOrAgreement userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.PurchaseSpecificationID);
                        return ResultWithMessage.Create(RequestResponceType.OperationError);
                    }
                    //
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        dataSource.BeginTransaction();
                        PurchaseSpecification.MarkAsExecutedOnContractOrAgreement(user.User.ID, model.PurchaseSpecificationID, model.NewStateID,out string msg, dataSource, user.User);
                        dataSource.CommitTransaction();
                        //
                        if(msg == string.Empty)
                            return ResultWithMessage.Create(RequestResponceType.Success);
                        else
                            return ResultWithMessage.Create(RequestResponceType.Success,msg);
                    }
                }
                catch (NotSupportedException e)
                {
                    Logger.Error(e, String.Format(@"MarkAsExecutedOnContractOrAgreement not supported, purchaseSpecificationID: '{0}'", model.PurchaseSpecificationID));
                    return ResultWithMessage.Create(RequestResponceType.GlobalError);
                }
                catch (ObjectDeletedException)
                {
                    Logger.Trace(String.Format(@"MarkAsExecutedOnContractOrAgreement object deleted, purchaseSpecificationID: '{0}'", model.PurchaseSpecificationID));
                    return ResultWithMessage.Create(RequestResponceType.ObjectDeleted);
                }
                catch (ArgumentValidationException e)
                {
                    Logger.Trace(String.Format(@"MarkAsExecutedOnContractOrAgreement validation error, purchaseSpecificationID: '{0}'", model.PurchaseSpecificationID));
                    return ResultWithMessage.Create(RequestResponceType.ValidationError, e.Message);
                }
                catch (Exception e)
                {
                    Logger.Error(e, String.Format(@"MarkAsExecutedOnContractOrAgreement, purchaseSpecificationID: '{0}'", model.PurchaseSpecificationID));
                    return ResultWithMessage.Create(RequestResponceType.GlobalError);
                }
            }
            else return ResultWithMessage.Create(RequestResponceType.NullParamsError);
        }
        #endregion

        #region method RemoveSpecification
        public sealed class RemovePurchaseSpecificationIncomingModel
        {
            public List<Guid> IDList { get; set; }
            public Guid WorkOrderID { get; set; }
        }
        //
        [HttpPost]
        [Route("finApi/RemovePurchaseSpecification", Name = "RemovePurchaseSpecification")]
        public RequestResponceType RemovePurchaseSpecification(RemovePurchaseSpecificationIncomingModel model)
        {
            var user = base.CurrentUser;
            if (model != null && user != null)
            {
                try
                {
                    if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PurchaseSpecification_Delete))
                    {
                        Logger.Trace("FinanceApiController.RemovePurchaseSpecification userID={0}, userName={1} failed (operation denied)", user.Id, user.UserName);
                        return RequestResponceType.OperationError;
                    }
                    //
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        if (!BLL.SD.WorkOrders.WorkOrder.AccessIsGranted(model.WorkOrderID, user.User, true, dataSource))
                        {
                            Logger.Error("FinanceApiController.RemovePurchaseSpecification userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.WorkOrderID);
                            return RequestResponceType.AccessError;
                        }
                        foreach (var id in model.IDList)
                        {
                            var info = new PurchaseSpecificationInfo() { ID = id, WorkOrderID = model.WorkOrderID, Operation = FieldOperation.Remove };
                            PurchaseSpecification.Edit(info, dataSource, user.User);
                        }
                        WorkflowWrapper.MakeOnSaveReaction(model.WorkOrderID, IMSystem.Global.OBJ_WORKORDER, dataSource, user.User);
                        //
                        return RequestResponceType.Success;
                    }
                }
                catch (NotSupportedException e)
                {
                    Logger.Error(e, String.Format(@"RemovePurchaseSpecification not supported, model: '{0}'", model));
                    return RequestResponceType.BadParamsError;
                }
                catch (ObjectDeletedException)
                {
                    Logger.Trace(String.Format(@"RemovePurchaseSpecification object deleted, model: '{0}'", model));
                    return RequestResponceType.ObjectDeleted;
                }
                catch (ArgumentValidationException)
                {
                    Logger.Trace(String.Format(@"RemovePurchaseSpecification validation error, model: '{0}'", model));
                    return RequestResponceType.ValidationError;
                }
                catch (Exception e)
                {
                    Logger.Error(e, String.Format(@"RemovePurchaseSpecification, model: '{0}'", model));
                    return RequestResponceType.GlobalError;
                }
            }
            else return RequestResponceType.NullParamsError;
        }
        #endregion

        #region method AddPurchaseSpecificationListFromCatalogue
        public sealed class AddPurchaseSpecificationListFromCatalogueInputModel
        {
            public List<DTL.Finance.AddFromCatalogueSpecificationInfo> ModelsList { get; set; }
            public int ObjectClassID { get; set; }
            public Guid ObjectID { get; set; }
        }
        public sealed class AddPurchaseSpecificationListFromCatalogueOutModel
        {
            public IList<PurchaseSpecification> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("finApi/AddPurchaseSpecificationListFromCatalogue", Name = "AddPurchaseSpecificationListFromCatalogue")]
        public AddPurchaseSpecificationListFromCatalogueOutModel AddPurchaseSpecificationListFromCatalogue(AddPurchaseSpecificationListFromCatalogueInputModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new AddPurchaseSpecificationListFromCatalogueOutModel() { List = null, Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace("FinanceApiController.AddPurchaseSpecificationListFromCatalogue userID={0}, userName={1}, ModelsListCount={2}, ObjectClassID={3}, ObjectID={4}",
            user.Id, user.UserName, model.ModelsList.Count, model.ObjectClassID, model.ObjectID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    IList<PurchaseSpecification> retval = null;
                    //
                    retval = BLL.SD.WorkOrders.WorkOrder.AddPurchaseSpecificationsModelsFromCatalogue(model.ObjectID, model.ModelsList, dataSource, user.User);
                    //
                    WorkflowWrapper.MakeOnSaveReaction(model.ObjectID, model.ObjectClassID, dataSource, user.User);
                    return new AddPurchaseSpecificationListFromCatalogueOutModel() { List = retval, Result = (RequestResponceType.Success) };
                }
            }
            catch (FieldConcurrencyException e)
            {
                Logger.Error(e, String.Format(@"AddPurchaseSpecificationListFromCatalogue concurency error. ObjClassID: '{2}'. Id: '{0}'. Models: '{1}'",
                        model.ObjectID.ToString(),
                        String.Join(@"\n, ", model.ModelsList.Select(d => d.ModelID.ToString())),
                        model.ObjectClassID));
                return new AddPurchaseSpecificationListFromCatalogueOutModel() { List = null, Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectConcurrencyException e)
            {
                Logger.Error(e, String.Format(@"AddPurchaseSpecificationListFromCatalogue OBJECT concurency error. ObjClassID: '{2}'. Id: '{0}'. Models: '{1}'",
                         model.ObjectID.ToString(),
                        String.Join(@"\n, ", model.ModelsList.Select(d => d.ModelID.ToString())),
                        model.ObjectClassID));
                return new AddPurchaseSpecificationListFromCatalogueOutModel() { List = null, Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectDeletedException e)
            {
                Logger.Error(e, String.Format(@"AddPurchaseSpecificationListFromCatalogue object deleted error. ObjClassID: '{2}'. Id: '{0}'. Models: '{1}'",
                            model.ObjectID.ToString(),
                        String.Join(@"\n, ", model.ModelsList.Select(d => d.ModelID.ToString())),
                        model.ObjectClassID));
                return new AddPurchaseSpecificationListFromCatalogueOutModel() { List = null, Result = (RequestResponceType.ObjectDeleted) };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при добавлении спецификаций.");
                return new AddPurchaseSpecificationListFromCatalogueOutModel() { List = null, Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion

        #region method AddPurchaseSpecificationListFromObject
        public sealed class AddPurchaseSpecificationListFromObjectInputModel
        {
            public List<DTL.Finance.AddFromAnotherObjectSpecificationInfo> ModelsList { get; set; }
            public DTL.Finance.AddFromAnotherObjectSpecificationOptions Options { get; set; }
            public int ObjectClassID { get; set; }
            public Guid ObjectID { get; set; }
        }
        public sealed class AddPurchaseSpecificationListFromObjectOutModel
        {
            public IList<PurchaseSpecification> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("finApi/AddPurchaseSpecificationListFromObject", Name = "AddPurchaseSpecificationListFromObject")]
        public AddPurchaseSpecificationListFromObjectOutModel AddPurchaseSpecificationListFromObject(AddPurchaseSpecificationListFromObjectInputModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new AddPurchaseSpecificationListFromObjectOutModel() { List = null, Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace("FinanceApiController.AddPurchaseSpecificationListFromObject userID={0}, userName={1}, ModelsListCount={2}, ObjectClassID={3}, ObjectID={4}",
            user.Id, user.UserName, model.ModelsList.Count, model.ObjectClassID, model.ObjectID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    IList<PurchaseSpecification> retval = null;
                    //
                    retval = BLL.SD.WorkOrders.WorkOrder.AddPurchaseSpecificationsModelsFromARS(model.ObjectID, model.Options, model.ModelsList, dataSource, user.User);
                    //
                    WorkflowWrapper.MakeOnSaveReaction(model.ObjectID, model.ObjectClassID, dataSource, user.User);
                    return new AddPurchaseSpecificationListFromObjectOutModel() { List = retval, Result = (RequestResponceType.Success) };
                }
            }
            catch (FieldConcurrencyException e)
            {
                Logger.Error(e, String.Format(@"AddPurchaseSpecificationListFromObject concurency error. ObjClassID: '{2}'. Id: '{0}'. Models: '{1}'",
                        model.ObjectID.ToString(),
                        String.Join(@"\n, ", model.ModelsList.Select(d => d.ModelID.ToString())),
                        model.ObjectClassID));
                return new AddPurchaseSpecificationListFromObjectOutModel() { List = null, Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectConcurrencyException e)
            {
                Logger.Error(e, String.Format(@"AddPurchaseSpecificationListFromObject OBJECT concurency error. ObjClassID: '{2}'. Id: '{0}'. Models: '{1}'",
                         model.ObjectID.ToString(),
                        String.Join(@"\n, ", model.ModelsList.Select(d => d.ModelID.ToString())),
                        model.ObjectClassID));
                return new AddPurchaseSpecificationListFromObjectOutModel() { List = null, Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectDeletedException e)
            {
                Logger.Error(e, String.Format(@"AddPurchaseSpecificationListFromObject object deleted error. ObjClassID: '{2}'. Id: '{0}'. Models: '{1}'",
                            model.ObjectID.ToString(),
                        String.Join(@"\n, ", model.ModelsList.Select(d => d.ModelID.ToString())),
                        model.ObjectClassID));
                return new AddPurchaseSpecificationListFromObjectOutModel() { List = null, Result = (RequestResponceType.ObjectDeleted) };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при добавлении спецификаций.");
                return new AddPurchaseSpecificationListFromObjectOutModel() { List = null, Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion

        #endregion

        #region method ActivesRequestSpecificationDependency

        #region method GetActivesRequestSpecificationList
        public sealed class GetPurchaseSpecificationDependencyListIncomingModel
        {
            public Guid PurchaseSpecificationID { get; set; }
        }
        public sealed class GetPurchaseSpecificationDependencyListOutModel
        {
            public IList<ActivesRequestSpecificationDependency> ARSList { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetPurchaseSpecificationDependencyList", Name = "GetPurchaseSpecificationDependencyList")]
        public GetPurchaseSpecificationDependencyListOutModel GetPurchaseSpecificationDependencyList([FromQuery] GetPurchaseSpecificationDependencyListIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetPurchaseSpecificationDependencyListOutModel() { ARSList = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("FinanceApiController.GetPurchaseSpecificationDependencyList userID={0}, userName={1}, objID={2}", user.Id, user.UserName, model.PurchaseSpecificationID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = ActivesRequestSpecificationDependency.GetList(model.PurchaseSpecificationID, true, dataSource);
                    //
                    return new GetPurchaseSpecificationDependencyListOutModel() { ARSList = retval, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetPurchaseSpecificationDependencyList not supported, model: '{0}'", model));
                return new GetPurchaseSpecificationDependencyListOutModel() { ARSList = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetPurchaseSpecificationDependencyList, model: {0}.", model);
                return new GetPurchaseSpecificationDependencyListOutModel() { ARSList = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #endregion

        #region method GetGoodsInvoiceSpecificationList
        public sealed class GetGoodsInvoiceSpecificationListIncomingModel
        {
            public Guid ObjectID { get; set; }
            public int ObjectClassID { get; set; }
        }
        public sealed class GetGoodsInvoiceSpecificationListOutModel
        {
            public IList<GoodsInvoiceSpecification> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetGoodsInvoiceSpecificationList", Name = "GetGoodsInvoiceSpecificationList")]
        public GetGoodsInvoiceSpecificationListOutModel GetGoodsInvoiceSpecificationList([FromQuery] GetGoodsInvoiceSpecificationListIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetGoodsInvoiceSpecificationListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("FinanceApiController.GetGoodsInvoiceSpecificationList userID={0}, userName={1}, objID={2}", user.Id, user.UserName, model.ObjectID);
                //
                List<GoodsInvoiceSpecification> retval = null;
                using (var dataSource = DataSource.GetDataSource())
                {
                    switch (model.ObjectClassID)
                    {
                        case IMSystem.Global.OBJ_GoodsInvoice:
                            retval = GoodsInvoiceSpecification.GetList(model.ObjectID, null, dataSource);
                            break;
                        case IMSystem.Global.OBJ_PurchaseSpecification:
                            retval = GoodsInvoiceSpecification.GetListByPURS(model.ObjectID, null, dataSource);
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                    //
                    return new GetGoodsInvoiceSpecificationListOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetGoodsInvoiceSpecificationList not supported, model: '{0}'", model));
                return new GetGoodsInvoiceSpecificationListOutModel() { List = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetGoodsInvoiceSpecificationList, model: {0}.", model);
                return new GetGoodsInvoiceSpecificationListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetGoodsInvoiceList
        public sealed class GetGoodsInvoiceListIncomingModel
        {
            public int ObjectClassID { get; set; }
            public Guid ObjectID { get; set; }
        }
        public sealed class GetGoodsInvoiceListOutModel
        {
            public IList<GoodsInvoice> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetGoodsInvoiceList", Name = "GetGoodsInvoiceList")]
        public GetGoodsInvoiceListOutModel GetGoodsInvoiceList([FromQuery] GetGoodsInvoiceListIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetGoodsInvoiceListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("FinanceApiController.GetGoodsInvoiceListOutModel userID={0}, userName={1}, objID={2}, objClassID={3}", user.Id, user.UserName, model.ObjectID, model.ObjectClassID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.ObjectClassID == IMSystem.Global.OBJ_WORKORDER)
                    {
                        if (!BLL.SD.WorkOrders.WorkOrder.AccessIsGranted(model.ObjectID, user.User, true, dataSource))
                        {
                            Logger.Error("FinanceApiController.GetGoodsInvoiceListOutModel userID={0}, userName={1}, objID={2}, objClassID={3}  failed (access denied)", user.Id, user.UserName, model.ObjectID, model.ObjectClassID);
                            return new GetGoodsInvoiceListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.ObjectClassID == IMSystem.Global.OBJ_PurchaseSpecification)
                    {
                        //TODO: access
                    }
                    else
                        throw new NotSupportedException("ObjectClassID");
                    //
                    var retval = GoodsInvoice.GetList(model.ObjectID, model.ObjectClassID, null, dataSource);
                    //
                    return new GetGoodsInvoiceListOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetGoodsInvoiceList not supported, model: '{0}'", model));
                return new GetGoodsInvoiceListOutModel() { List = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetGoodsInvoiceList, model: {0}.", model);
                return new GetGoodsInvoiceListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetGoodsInvoice
        public sealed class GetGoodsInvoiceIncomingModel
        {
            public Guid GoodInvoiceID { get; set; }
        }
        public sealed class GetGoodsInvoiceOutModel
        {
            public GoodsInvoice Data { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetGoodsInvoice", Name = "GetGoodsInvoice")]
        public GetGoodsInvoiceOutModel GetGoodsInvoice([FromQuery] GetGoodsInvoiceIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetGoodsInvoiceOutModel() { Data = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("FinanceApiController.GetGoodsInvoiceOutModel userID={0}, userName={1}, id={2}", user.Id, user.UserName, model.GoodInvoiceID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = GoodsInvoice.Get(model.GoodInvoiceID, dataSource);
                    //
                    return new GetGoodsInvoiceOutModel() { Data = retval, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetGoodsInvoice not supported, model: '{0}'", model));
                return new GetGoodsInvoiceOutModel() { Data = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetGoodsInvoiceList, model: {0}.", model);
                return new GetGoodsInvoiceOutModel() { Data = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion



        #region method EditGoodsInvoice
        public sealed class EditGoodsInvoiceOutModel
        {
            public GoodsInvoice GoodsInvoice { get; set; }
            public ResultWithMessage Response { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("finApi/EditGoodsInvoice", Name = "EditGoodsInvoice")]
        public EditGoodsInvoiceOutModel EditGoodsInvoice(GoodsInvoiceInfo model)
        {
            var user = base.CurrentUser;
            //
            if (model == null || user == null)
                return new EditGoodsInvoiceOutModel() { GoodsInvoice = null, Response = ResultWithMessage.Create(RequestResponceType.NullParamsError) };
            //
            try
            {
                int operationID = 0;
                switch (model.Operation)
                {
                    case FieldOperation.Create:
                        operationID = IMSystem.Global.OPERATION_GoodsInvoice_Add;
                        break;
                    case FieldOperation.Edit:
                        operationID = IMSystem.Global.OPERATION_GoodsInvoice_Update;
                        break;
                    case FieldOperation.Remove:
                        operationID = IMSystem.Global.OPERATION_GoodsInvoice_Delete;
                        break;

                }
                //
                if (!user.User.OperationIsGranted(operationID))
                {
                    Logger.Trace("FinanceApiController.EditGoodsInvoice userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return new EditGoodsInvoiceOutModel() { Response = ResultWithMessage.Create(RequestResponceType.OperationError) };
                }
                //
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!BLL.SD.WorkOrders.WorkOrder.AccessIsGranted(model.WorkOrderID, user.User, true, dataSource))
                    {
                        Logger.Error("FinanceApiController.EditGoodsInvoice userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.WorkOrderID);
                        return new EditGoodsInvoiceOutModel() { Response = ResultWithMessage.Create(RequestResponceType.AccessError) };
                    }
                    var result = GoodsInvoice.Edit(model, dataSource, user.User);
                    WorkflowWrapper.MakeOnSaveReaction(model.WorkOrderID, IMSystem.Global.OBJ_WORKORDER, dataSource, user.User);
                    //
                    return new EditGoodsInvoiceOutModel() { Response = ResultWithMessage.Create(RequestResponceType.Success), GoodsInvoice = result };
                }
            }
            catch (FieldConcurrencyException e)
            {
                Logger.Error(e, String.Format(@"AddGoodsInvoice concurency error.  Id: '{0}'", model.WorkOrderID.ToString()));
                return new EditGoodsInvoiceOutModel() { GoodsInvoice = null, Response = ResultWithMessage.Create(RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectConcurrencyException e)
            {
                Logger.Error(e, String.Format(@"AddGoodsInvoice OBJECT concurency error.  Id: '{0}'", model.WorkOrderID.ToString()));
                return new EditGoodsInvoiceOutModel() { GoodsInvoice = null, Response = ResultWithMessage.Create(RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectDeletedException e)
            {
                Logger.Error(e, String.Format(@"AddGoodsInvoice object deleted error.  Id: '{0}'", model.WorkOrderID.ToString()));
                return new EditGoodsInvoiceOutModel() { GoodsInvoice = null, Response = ResultWithMessage.Create(RequestResponceType.ObjectDeleted) };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при добавлении спецификаций.");
                return new EditGoodsInvoiceOutModel() { GoodsInvoice = null, Response = ResultWithMessage.Create(RequestResponceType.GlobalError) };
            }
        }
        #endregion

        #region method EditGoodsInvoiceSpecification
        public sealed class EditGoodsInvoiceSpecificationOutModel
        {
            public GoodsInvoiceSpecification NewModel { get; set; }
            public ResultWithMessage Response { get; set; }
        }
        [HttpPost]
        [Route("finApi/EditGoodsInvoiceSpecification", Name = "EditGoodsInvoiceSpecification")]
        public EditGoodsInvoiceSpecificationOutModel EditGoodsInvoiceSpecification(GoodsInvoiceSpecificationInfo model)
        {
            var user = base.CurrentUser;
            if (model != null && user != null)
            {
                try
                {
                    int operationID = 0;
                    switch (model.Operation)
                    {
                        case FieldOperation.Create:
                            operationID = IMSystem.Global.OPERATION_GoodsInvoiceSpecification_Add;
                            break;
                        case FieldOperation.Edit:
                            operationID = IMSystem.Global.OPERATION_GoodsInvoiceSpecification_Update;
                            break;
                        case FieldOperation.Remove:
                            operationID = IMSystem.Global.OPERATION_GoodsInvoiceSpecification_Delete;
                            break;
                    }
                    if (!user.User.OperationIsGranted(operationID))
                    {
                        Logger.Trace("FinanceApiController.EditGoodsInvoiceSpecification userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                        return new EditGoodsInvoiceSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.OperationError) };
                    }
                    //
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        var result = GoodsInvoiceSpecification.Edit(model, dataSource, user.User);
                        //
                        return new EditGoodsInvoiceSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.Success), NewModel = result };
                    }
                }
                catch (NotSupportedException e)
                {
                    Logger.Error(e, String.Format(@"EditGoodsInvoiceSpecification not supported, model: '{0}'", model));
                    return new EditGoodsInvoiceSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.BadParamsError) };
                }
                catch (ObjectDeletedException)
                {
                    Logger.Trace(String.Format(@"EditGoodsInvoiceSpecification object deleted, model: '{0}'", model));
                    return new EditGoodsInvoiceSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.ObjectDeleted) };
                }
                catch (ArgumentValidationException e)
                {
                    Logger.Trace(String.Format(@"EditGoodsInvoiceSpecification validation error, model: '{0}'", model));
                    return new EditGoodsInvoiceSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.ValidationError, ValidatorHelper.CreateErrorMessage(e)) };
                }
                catch (Exception e)
                {
                    Logger.Error(e, String.Format(@"EditGoodsInvoiceSpecification, model: '{0}'", model));
                    return new EditGoodsInvoiceSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.GlobalError) };
                }
            }
            else return new EditGoodsInvoiceSpecificationOutModel() { Response = ResultWithMessage.Create(RequestResponceType.NullParamsError) };
        }
        #endregion

        #region method AddGoodsInvoiceSpecification
        public sealed class AddGoodsInvoiceSpecificationInputModel
        {
            public DTL.Finance.GoodsInvoiceSpecification GoodsInvoiceSpecification { get; set; }
        }
        public sealed class AddGoodsInvoiceSpecificationOutModel
        {
            public GoodsInvoiceSpecification GoodsInvoiceSpecification { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("finApi/GetGoodsInvoiceSpecificationNDSFields", Name = "GetGoodsInvoiceSpecificationNDSFields")]
        public AddGoodsInvoiceSpecificationOutModel GetGoodsInvoiceSpecificationNDSFields(AddGoodsInvoiceSpecificationInputModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new AddGoodsInvoiceSpecificationOutModel() { GoodsInvoiceSpecification = null, Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace("FinanceApiController.AddGoodsInvoiceSpecification userID={0}, userName={1}, Number={2} CargoName={3}",
            user.Id, user.UserName, model.GoodsInvoiceSpecification.SpecificationNumber, model.GoodsInvoiceSpecification.CargoName);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    GoodsInvoiceSpecification retval = GoodsInvoiceSpecification.GetNDSFields(model.GoodsInvoiceSpecification, dataSource);
                    //
                    return new AddGoodsInvoiceSpecificationOutModel() { GoodsInvoiceSpecification = retval, Result = (RequestResponceType.Success) };
                }
            }
            catch (FieldConcurrencyException e)
            {
                Logger.Error(e, String.Format(@"AddGoodsInvoiceSpecification concurency error.  Id: '{0}'", model.GoodsInvoiceSpecification.ID.ToString()));
                return new AddGoodsInvoiceSpecificationOutModel() { GoodsInvoiceSpecification = null, Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectConcurrencyException e)
            {
                Logger.Error(e, String.Format(@"AddGoodsInvoiceSpecification OBJECT concurency error.  Id: '{0}'", model.GoodsInvoiceSpecification.ID.ToString()));
                return new AddGoodsInvoiceSpecificationOutModel() { GoodsInvoiceSpecification = null, Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectDeletedException e)
            {
                Logger.Error(e, String.Format(@"AddGoodsInvoiceSpecification object deleted error.  Id: '{0}'", model.GoodsInvoiceSpecification.ID.ToString()));
                return new AddGoodsInvoiceSpecificationOutModel() { GoodsInvoiceSpecification = null, Result = (RequestResponceType.ObjectDeleted) };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при добавлении спецификаций.");
                return new AddGoodsInvoiceSpecificationOutModel() { GoodsInvoiceSpecification = null, Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion

        #region method RemoveGoodsInvoiceSpecification
        public sealed class RemoveGoodsInvoiceSpecificationIncomingModel
        {
            public List<Guid> IDList { get; set; }
            public Guid GoodsInvoiceID { get; set; }
        }
        //
        [HttpPost]
        [Route("finApi/RemoveGoodsInvoiceSpecification", Name = "RemoveGoodsInvoiceSpecification")]
        public RequestResponceType RemoveGoodsInvoiceSpecification(RemoveGoodsInvoiceSpecificationIncomingModel model)
        {
            var user = base.CurrentUser;
            if (model != null && user != null)
            {
                try
                {
                    if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_GoodsInvoiceSpecification_Delete))
                    {
                        Logger.Trace("FinanceApiController.RemoveGoodsInvoiceSpecification userID={0}, userName={1} failed (operation denied)", user.Id, user.UserName);
                        return RequestResponceType.OperationError;
                    }
                    //
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        foreach (var id in model.IDList)
                        {
                            var info = new GoodsInvoiceSpecificationInfo() { ID = id, GoodsInvoiceID = model.GoodsInvoiceID, Operation = FieldOperation.Remove };
                            GoodsInvoiceSpecification.Edit(info, dataSource, user.User);
                        }
                        //
                        return RequestResponceType.Success;
                    }
                }
                catch (NotSupportedException e)
                {
                    Logger.Error(e, String.Format(@"RemoveGoodsInvoiceSpecification not supported, model: '{0}'", model));
                    return RequestResponceType.BadParamsError;
                }
                catch (ObjectDeletedException)
                {
                    Logger.Trace(String.Format(@"RemoveGoodsInvoiceSpecification object deleted, model: '{0}'", model));
                    return RequestResponceType.ObjectDeleted;
                }
                catch (ArgumentValidationException)
                {
                    Logger.Trace(String.Format(@"RemoveGoodsInvoiceSpecification validation error, model: '{0}'", model));
                    return RequestResponceType.ValidationError;
                }
                catch (Exception e)
                {
                    Logger.Error(e, String.Format(@"RemoveGoodsInvoiceSpecification, model: '{0}'", model));
                    return RequestResponceType.GlobalError;
                }
            }
            else return RequestResponceType.NullParamsError;
        }
        #endregion


        #region method GetInvoiceSpecificationDependencyList
        public sealed class GetInvoiceSpecificationDependencyListIncomingModel
        {
            public Guid? InvoiceSpecificationID { get; set; }
            public List<DTL.Finance.GoodsInvoiceSpecificationPurchaseDependencyInfo> PurchaseSpecificationIDList { get; set; }
        }
        public sealed class GetInvoiceSpecificationDependencyListOutModel
        {
            public IList<PurchaseSpecificationDependency> PURSList { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetInvoiceSpecificationDependencyList", Name = "GetInvoiceSpecificationDependencyList")]
        public GetInvoiceSpecificationDependencyListOutModel GetInvoiceSpecificationDependencyList([FromQuery] GetInvoiceSpecificationDependencyListIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetInvoiceSpecificationDependencyListOutModel() { PURSList = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("FinanceApiController.GetInvoiceSpecificationDependencyList userID={0}, userName={1}, objID={2}", user.Id, user.UserName, model.InvoiceSpecificationID);
                //
                IList<PurchaseSpecificationDependency> retval = null;
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.InvoiceSpecificationID.HasValue && model.InvoiceSpecificationID.Value != Guid.Empty)
                    {
                        retval = PurchaseSpecificationDependency.GetList(model.InvoiceSpecificationID.Value, dataSource);
                    }
                    else if (model.PurchaseSpecificationIDList != null)
                    {
                        retval = PurchaseSpecificationDependency.GetList(model.PurchaseSpecificationIDList.Select(x => x.PurchaseSpecificationID).ToList(), dataSource);
                    }
                    //
                    return new GetInvoiceSpecificationDependencyListOutModel() { PURSList = retval, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetInvoiceSpecificationDependencyList not supported, model: '{0}'", model));
                return new GetInvoiceSpecificationDependencyListOutModel() { PURSList = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetInvoiceSpecificationDependencyList, model: {0}.", model);
                return new GetInvoiceSpecificationDependencyListOutModel() { PURSList = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method RemoveGoodsInvoice
        public sealed class RemoveGoodsInvoiceIncomingModel
        {
            public List<Guid> IDList { get; set; }
            public Guid WorkOrderID { get; set; }
        }
        //
        [HttpPost]
        [Route("finApi/RemoveGoodsInvoice", Name = "RemoveGoodsInvoice")]
        public RequestResponceType RemoveGoodsInvoice(RemoveGoodsInvoiceIncomingModel model)
        {
            var user = base.CurrentUser;
            if (model == null || user == null)
                return RequestResponceType.NullParamsError;
            //
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PurchaseSpecification_Delete))
                {
                    Logger.Trace("FinanceApiController.RemoveGoodsInvoice userID={0}, userName={1} failed (operation denied)", user.Id, user.UserName);
                    return RequestResponceType.OperationError;
                }
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!BLL.SD.WorkOrders.WorkOrder.AccessIsGranted(model.WorkOrderID, user.User, true, dataSource))
                    {
                        Logger.Error("FinanceApiController.RemoveGoodsInvoice userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.WorkOrderID);
                        return RequestResponceType.AccessError;
                    }
                    dataSource.BeginTransaction();
                    foreach (var id in model.IDList)
                        GoodsInvoice.Delete(id, dataSource);
                    dataSource.CommitTransaction();
                    //
                    WorkflowWrapper.MakeOnSaveReaction(model.WorkOrderID, IMSystem.Global.OBJ_WORKORDER, dataSource, user.User);
                    //
                    return RequestResponceType.Success;
                }
            }
            catch (NotSupportedException e)
            {
                Logger.Error(e, String.Format(@"RemoveGoodsInvoice not supported, model: '{0}'", model));
                return RequestResponceType.BadParamsError;
            }
            catch (ObjectDeletedException)
            {
                Logger.Trace(String.Format(@"RemoveGoodsInvoice object deleted, model: '{0}'", model));
                return RequestResponceType.ObjectDeleted;
            }
            catch (ArgumentValidationException)
            {
                Logger.Trace(String.Format(@"RemoveGoodsInvoice validation error, model: '{0}'", model));
                return RequestResponceType.ValidationError;
            }
            catch (Exception e)
            {
                Logger.Error(e, String.Format(@"RemoveGoodsInvoice, model: '{0}'", model));
                return RequestResponceType.GlobalError;
            }
        }
        #endregion


        #region method FinanceNegotiationVote
        public sealed class FinanceNegotiationVoteInputModel
        {
            public Guid NegotiationID { get; set; }
            public String Comment { get; set; }
            public BLL.SD.Negotiations.VotingType Type { get; set; }
            public Guid ObjectID { get; set; }
            public List<SpecificationInNegotiationInfo> SpecificationsList { get; set; }
        }
        public sealed class FinanceNegotiationVoteOutputModel
        {
            public RequestResponceType Result { get; set; }
            public String UtcDateTimeVote { get; set; }
        }
        [HttpPost]
        [Route("finApi/FinanceNegotiationVote", Name = "FinanceNegotiationVote")]
        public FinanceNegotiationVoteOutputModel FinanceNegotiationVote(FinanceNegotiationVoteInputModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                {
                    Logger.Error(String.Format(@"FinanceNegotiationVote null user, model: '{0}'", model));
                    return new FinanceNegotiationVoteOutputModel() { Result = RequestResponceType.NullParamsError, UtcDateTimeVote = String.Empty };
                }
                //
                if (model.Type == BLL.SD.Negotiations.VotingType.None)
                {
                    Logger.Error(String.Format(@"FinanceNegotiationVote bad params, model: '{0}'", model));
                    return new FinanceNegotiationVoteOutputModel() { Result = RequestResponceType.BadParamsError, UtcDateTimeVote = String.Empty };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var dt = BLL.SD.Negotiations.FinanceNegotiation.Vote(model.NegotiationID, model.ObjectID, user.User.ID, model.Comment, model.Type, model.SpecificationsList, dataSource, user.User);
                    return new FinanceNegotiationVoteOutputModel() { Result = RequestResponceType.Success, UtcDateTimeVote = JSDateTimeHelper.ToJS(dt) };
                }
            }
            catch (NotSupportedException e)
            {
                Logger.Error(e, String.Format(@"FinanceNegotiationVote not supported, model: '{0}'", model));
                return new FinanceNegotiationVoteOutputModel() { Result = RequestResponceType.BadParamsError, UtcDateTimeVote = String.Empty };
            }
            catch (ObjectDeletedException e)
            {
                Logger.Error(e, String.Format(@"FinanceNegotiationVote object deleted, model: '{0}'", model));
                return new FinanceNegotiationVoteOutputModel() { Result = RequestResponceType.ObjectDeleted };
            }
            catch (FieldConcurrencyException e)
            {
                Logger.Error(e, String.Format(@"FinanceNegotiationVote field concurrency, model: '{0}'", model));
                return new FinanceNegotiationVoteOutputModel() { Result = RequestResponceType.ConcurrencyError };
            }
            catch (ObjectConcurrencyException e)
            {
                Logger.Error(e, String.Format(@"Negotiation already ended, model: '{0}'", model));
                return new FinanceNegotiationVoteOutputModel() { Result = RequestResponceType.NegotiationEnded };
            }
            catch (ArgumentValidationException e)
            {
                Logger.Error(e, String.Format(@"FinanceNegotiationVote validation error, model: '{0}'", model));
                return new FinanceNegotiationVoteOutputModel() { Result = RequestResponceType.ValidationError, UtcDateTimeVote = String.Empty };
            }
            catch (Exception e)
            {
                Logger.Error(e, String.Format(@"FinanceNegotiationVote, model: '{0}'", model));
                return new FinanceNegotiationVoteOutputModel() { Result = RequestResponceType.GlobalError, UtcDateTimeVote = String.Empty };
            }
        }
        #endregion

        #region method GetSupplierList
        public sealed class GetSupplierListOutModel
        {
            public InvoiceHelper[] List { get; set; }
            //public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetSupplierList", Name = "GetSupplierList")]
        [Obsolete("Use api/suppliers instead")]
        public List<InvoiceHelper> GetSupplierList()
        {
            try
            {
                var user = base.CurrentUser;
                //
                Logger.Trace("FinanceApiController.GetSupplierList userID={0}, userName={1}", user.Id, user.UserName);
                //
                if (!user.User.HasRoles)
                {
                    Logger.Error("FinanceApiController.GetSupplierList userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                    return null;
                }
                //
                var retval = InvoiceHelper.GetSupplierList();
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка поставщиков.");
                return null;
            }
        }
        #endregion

        #region method GetOrganizationList
        public sealed class GetOrganizationListOutModel
        {
            public InvoiceHelper[] List { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetOrganizationList", Name = "GetOrganizationList")]
        public GetOrganizationListOutModel GetOrganizationList()
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetOrganizationListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("FinanceApiController.GetOrganizationList userID={0}, userName={1}", user.Id, user.UserName);
                //
                if (!user.User.HasRoles)
                {
                    Logger.Error("FinanceApiController.GetOrganizationList userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                    return new GetOrganizationListOutModel() { List = null, Result = RequestResponceType.AccessError };
                }
                //
                var retval = InvoiceHelper.GetOrganizationList();
                return new GetOrganizationListOutModel() { List = retval.ToArray(), Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка организаций.");
                return new GetOrganizationListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method LocateAsset
        public sealed class LocateAssetOutModel
        {
            public bool NoDuplicates { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class LocateAssetInputModel
        {
            public List<DTL.Assets.AssetFieldsInfo> AssetFieldsList { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("finApi/LocateAsset", Name = "LocateAsset")]
        public LocateAssetOutModel LocateAsset(LocateAssetInputModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new LocateAssetOutModel() { Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("FinanceApiController.LocateAsset userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                bool res;
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction();
                    try
                    {
                        res = BLL.Assets.Asset.LocateAsset(model.AssetFieldsList, dataSource);
                        if (res || (model.AssetFieldsList != null && model.AssetFieldsList.All(x => x.GenerateCode && x.SetCodeIfDuplicate || x.GenerateInvNumber && x.SetInvNumberIfDuplicate)))
                            dataSource.CommitTransaction();
                        else
                            dataSource.RollbackTransaction();
                    }
                    catch
                    {
                        dataSource.RollbackTransaction();
                        throw;
                    }
                    //
                    return new LocateAssetOutModel() { Result = RequestResponceType.Success, NoDuplicates = res };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при попытке размещения оборудования спецификаций.");
                return new LocateAssetOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetPurchaseSpecificationNDSTotalValues
        public sealed class GetNDSTotalValuesOutModel
        {
            public string CostWithoutNDS { get; set; }
            public string CostWithNDS { get; set; }
            public string SumNDS { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetPurchaseSpecificationNDSTotalValues", Name = "GetPurchaseSpecificationNDSTotalValues")]
        public GetNDSTotalValuesOutModel GetPurchaseSpecificationNDSTotalValues([FromQuery] Guid workOrderID)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetNDSTotalValuesOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("FinanceApiController.GetPurchaseSpecificationTotalValues userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = PurchaseSpecification.GetNDSTotalValues(workOrderID, dataSource);
                    return new GetNDSTotalValuesOutModel() { CostWithoutNDS = retval.Item1.HasValue ? retval.Item1.ToString() : null, CostWithNDS = retval.Item2.HasValue ? retval.Item2.ToString() : null, SumNDS = retval.Item3.HasValue ? retval.Item3.ToString() : null, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetNDSTotalValuesOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetActivesRequestSpecificationNDSTotalValues
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetActivesRequestSpecificationNDSTotalValues", Name = "GetActivesRequestSpecificationNDSTotalValues")]
        public GetNDSTotalValuesOutModel GetActivesRequestSpecificationNDSTotalValues([FromQuery] Guid workOrderID)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetNDSTotalValuesOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("FinanceApiController.GetActivesRequestSpecificationNDSTotalValues userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = ActivesRequestSpecification.GetNDSTotalValues(workOrderID, dataSource);
                    return new GetNDSTotalValuesOutModel() { CostWithoutNDS = retval.Item1.HasValue ? retval.Item1.ToString() : null, CostWithNDS = retval.Item2.HasValue ? retval.Item2.ToString() : null, SumNDS = retval.Item3.HasValue ? retval.Item3.ToString() : null, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetNDSTotalValuesOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion       
    }
}
