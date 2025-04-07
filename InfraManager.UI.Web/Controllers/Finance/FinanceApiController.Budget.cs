using InfraManager.Core.Data;
using InfraManager.Core.Exceptions;
using InfraManager.Core.Logging;
using InfraManager.Web.BLL.Finance.Budget;
using InfraManager.Web.BLL.Finance.Specification;
using InfraManager.Web.DTL.Tables;
using InfraManager.Web.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Resources = InfraManager.ResourcesArea.Resources;
using Microsoft.AspNetCore.Hosting;

namespace InfraManager.Web.Controllers.Finance
{
    public partial class FinanceApiController
    {
        private readonly IWebHostEnvironment _environment;
        
        public FinanceApiController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        #region method GetFinanceBudget
        public sealed class GetFinanceBudgetIncomingModel
        {
            public Guid FinanceBudgetID { get; set; }
        }
        public sealed class GetFinanceBudgetOutModel
        {
            public FinanceBudget Data { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetFinanceBudget", Name = "GetFinanceBudget")]
        public GetFinanceBudgetOutModel GetFinanceBudget([FromQuery] GetFinanceBudgetIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetFinanceBudgetOutModel() { Data = null, Result = RequestResponceType.NullParamsError };
                else if (!BLL.Global.IsBudgetEnabled)
                    return new GetFinanceBudgetOutModel() { Data = null, Result = RequestResponceType.OperationError };
                //
                Logger.Trace("FinanceApiController.GetFinanceBudgetOutModel userID={0}, userName={1}, id={2}", user.Id, user.UserName, model.FinanceBudgetID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = FinanceBudget.Get(model.FinanceBudgetID, dataSource);
                    //
                    return new GetFinanceBudgetOutModel() { Data = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetFinanceBudgetList, id={0}.", model.FinanceBudgetID);
                return new GetFinanceBudgetOutModel() { Data = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method SaveFinanceBudget        
        public sealed class SaveFinanceBudgetIn
        {
            public Guid? ID { get; set; }
            public string Name { get; set; }
            public string Note { get; set; }
            public int Year { get; set; }
            public string UtcDateCreatedJS { get; set; }
            public string RowVersion { get; set; }
        }
        public sealed class SaveFinanceBudgetOut : ResultWithMessage
        {
            private SaveFinanceBudgetOut() : base() { }

            public Guid? ID { get; private set; }

            public static SaveFinanceBudgetOut Create(RequestResponceType type, Guid id)
            {
                var retval = new SaveFinanceBudgetOut();
                retval.Result = type;
                retval.Message = string.Empty;
                retval.ID = id;
                retval.IsResult = false;
                //
                return retval;
            }

            public static new SaveFinanceBudgetOut Create(RequestResponceType type, string message, bool result = false)
            {
                var retval = new SaveFinanceBudgetOut();
                retval.Result = type;
                retval.Message = message;
                retval.IsResult = result;
                //
                return retval;
            }
        }

        [HttpPost]
        [Route("finApi/saveFinanceBudget", Name = "SaveFinanceBudget")]
        public SaveFinanceBudgetOut SaveFinanceBudget(SaveFinanceBudgetIn info)
        {
            var user = base.CurrentUser;
            if (info == null || user == null)
                return SaveFinanceBudgetOut.Create(RequestResponceType.NullParamsError, Resources.ErrorCaption);
            else if (!BLL.Global.IsBudgetEnabled)
                return SaveFinanceBudgetOut.Create(RequestResponceType.OperationError, Resources.OperationError);
            //
            Logger.Trace("FinanceApiController.SaveFinanceBudgetIn userID={0}, userName={1}", user.Id, user.UserName);
            if (!user.User.HasRoles ||
                (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_FinanceBudget_Add) && !info.ID.HasValue) ||
                (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_FinanceBudget_Update) && info.ID.HasValue))
                return SaveFinanceBudgetOut.Create(RequestResponceType.OperationError, Resources.AccessError);
            //            
            try
            {
                FinanceBudget obj;
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction();
                    if (!info.ID.HasValue)
                        obj = FinanceBudget.Insert(info.Name, info.Note, info.Year, info.UtcDateCreatedJS, dataSource);
                    else
                        obj = FinanceBudget.Update(info.ID.Value, info.Name, info.Note, info.Year, info.UtcDateCreatedJS, info.RowVersion, dataSource);
                    dataSource.CommitTransaction();
                }
                return SaveFinanceBudgetOut.Create(RequestResponceType.Success, obj.ID);
            }
            catch (ObjectConcurrencyException ex)
            {
                return SaveFinanceBudgetOut.Create(RequestResponceType.ConcurrencyError, string.Format(Resources.ConcurrencyError, ex.Message));
            }
            catch (ArgumentValidationException ex)
            {
                return SaveFinanceBudgetOut.Create(RequestResponceType.ValidationError, string.Format(Resources.ArgumentValidationException, ex.Message));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return SaveFinanceBudgetOut.Create(RequestResponceType.GlobalError, Resources.ErrorCaption);
            }
        }
        #endregion

        #region method DeleteFinanceBudget        
        public sealed class DeleteFinanceBudgetIn
        {
            public List<Guid> IDList { get; set; }
        }

        [HttpPost]
        [Route("finApi/deleteFinanceBudget", Name = "DeleteFinanceBudget")]
        public ResultWithMessage DeleteFinanceBudget(DeleteFinanceBudgetIn info)
        {
            var user = base.CurrentUser;
            if (info == null || user == null)
                return ResultWithMessage.Create(RequestResponceType.NullParamsError, Resources.ErrorCaption);
            else if (!BLL.Global.IsBudgetEnabled)
                return ResultWithMessage.Create(RequestResponceType.OperationError, Resources.OperationError);
            //
            Logger.Trace("FinanceApiController.DeleteFinanceBudget userID={0}, userName={1}", user.Id, user.UserName);
            if (!user.User.HasRoles ||
                !user.User.OperationIsGranted(IMSystem.Global.OPERATION_FinanceBudget_Delete))
                return ResultWithMessage.Create(RequestResponceType.OperationError, Resources.AccessError);
            //            
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction();
                    foreach (var id in info.IDList)
                        FinanceBudget.Delete(id, dataSource, user.User);
                    dataSource.CommitTransaction();
                }
                return ResultWithMessage.Create(RequestResponceType.Success);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ResultWithMessage.Create(RequestResponceType.GlobalError, Resources.ErrorCaption);
            }
        }
        #endregion

        #region method ApproveFinanceBudget        
        [HttpPost]
        [Route("finApi/approveFinanceBudget", Name = "ApproveFinanceBudget")]
        public ResultWithMessage ApproveFinanceBudget(Guid id)
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultWithMessage.Create(RequestResponceType.NullParamsError, Resources.ErrorCaption);
            else if (!BLL.Global.IsBudgetEnabled)
                return ResultWithMessage.Create(RequestResponceType.OperationError, Resources.OperationError);
            //
            Logger.Trace("FinanceApiController.ApproveFinanceBudget userID={0}, userName={1}", user.Id, user.UserName);
            if (!user.User.HasRoles ||
                !user.User.OperationIsGranted(IMSystem.Global.OPERATION_FinanceBudget_Update))
                return ResultWithMessage.Create(RequestResponceType.OperationError, Resources.AccessError);
            //            
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction();
                    FinanceBudget.Approve(id, dataSource);
                    dataSource.CommitTransaction();
                }
                return ResultWithMessage.Create(RequestResponceType.Success);
            }
            catch (ArgumentValidationException ex)
            {
                return SaveFinanceBudgetOut.Create(RequestResponceType.ValidationError, string.Format(Resources.ArgumentValidationException, ex.Message));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ResultWithMessage.Create(RequestResponceType.GlobalError, Resources.ErrorCaption);
            }
        }
        #endregion

        #region method GetFinanceBudgetRowDependencyListByPurchaseSpecification
        public sealed class GetFinanceBudgetRowDependencyListByPurchaseSpecificationIncomingModel
        {
            public Guid PurchaseSpecificationID { get; set; }
        }
        public sealed class GetFinanceBudgetRowDependencyListByPurchaseSpecificationOutModel
        {
            public IList<FinanceBudgetRowDependency> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetFinanceBudgetRowDependencyListByPurchaseSpecification", Name = "GetFinanceBudgetRowDependencyListByPurchaseSpecification")]
        public GetFinanceBudgetRowDependencyListByPurchaseSpecificationOutModel GetFinanceBudgetRowDependencyListByPurchaseSpecification([FromQuery] GetFinanceBudgetRowDependencyListByPurchaseSpecificationIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetFinanceBudgetRowDependencyListByPurchaseSpecificationOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                else if (!BLL.Global.IsBudgetEnabled)
                    return new GetFinanceBudgetRowDependencyListByPurchaseSpecificationOutModel() { List = null, Result = RequestResponceType.OperationError };
                //
                Logger.Trace("FinanceApiController.GetFinanceBudgetRowDependencyListByPurchaseSpecification userID={0}, userName={1}, purchaseSpecificaitonID={2}", user.Id, user.UserName, model.PurchaseSpecificationID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = FinanceBudgetRowDependency.GetListByPurchaseSpecification(model.PurchaseSpecificationID, dataSource);
                    //
                    return new GetFinanceBudgetRowDependencyListByPurchaseSpecificationOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetFinanceBudgetRowDependencyListByPurchaseSpecification, purchaseSpecificaitonID={0}.", model.PurchaseSpecificationID);
                return new GetFinanceBudgetRowDependencyListByPurchaseSpecificationOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetFinanceBudgetRowDependencyListByWorkOrder
        public sealed class GetFinanceBudgetRowDependencyListByWorkOrderIncomingModel
        {
            public Guid WorkOrderID { get; set; }
        }
        public sealed class GetFinanceBudgetRowDependencyListByWorkOrderOutModel
        {
            public IList<FinanceBudgetRowDependency> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetFinanceBudgetRowDependencyListByWorkOrder", Name = "GetFinanceBudgetRowDependencyListByWorkOrder")]
        public GetFinanceBudgetRowDependencyListByWorkOrderOutModel GetFinanceBudgetRowDependencyListByWorkOrder([FromQuery] GetFinanceBudgetRowDependencyListByWorkOrderIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetFinanceBudgetRowDependencyListByWorkOrderOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                else if (!BLL.Global.IsBudgetEnabled)
                    return new GetFinanceBudgetRowDependencyListByWorkOrderOutModel() { List = null, Result = RequestResponceType.OperationError };
                //
                Logger.Trace("FinanceApiController.GetFinanceBudgetRowDependencyListByWorkOrder userID={0}, userName={1}, workOrderID={2}", user.Id, user.UserName, model.WorkOrderID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = FinanceBudgetRowDependency.GetListByWorkOrder(model.WorkOrderID, dataSource);
                    //
                    return new GetFinanceBudgetRowDependencyListByWorkOrderOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetFinanceBudgetRowDependencyListByPurchaseSpecification, workOrderID={0}.", model.WorkOrderID);
                return new GetFinanceBudgetRowDependencyListByWorkOrderOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetFinanceBudgetRowDependencyByWorkOrder
        public sealed class GetFinanceBudgetRowDependencyByWorkOrderIncomingModel
        {
            public Guid WorkOrderID { get; set; }
            public Guid FinanceBudgetRowID { get; set; }
        }
        public sealed class GetFinanceBudgetRowDependencyByWorkOrderOutModel
        {
            public FinanceBudgetRowDependency Data { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetFinanceBudgetRowDependencyByWorkOrder", Name = "GetFinanceBudgetRowDependencyByWorkOrder")]
        public GetFinanceBudgetRowDependencyByWorkOrderOutModel GetFinanceBudgetRowDependencyByWorkOrder([FromQuery] GetFinanceBudgetRowDependencyByWorkOrderIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetFinanceBudgetRowDependencyByWorkOrderOutModel() { Data = null, Result = RequestResponceType.NullParamsError };
                else if (!BLL.Global.IsBudgetEnabled)
                    return new GetFinanceBudgetRowDependencyByWorkOrderOutModel() { Data = null, Result = RequestResponceType.OperationError };
                //
                Logger.Trace("FinanceApiController.GetFinanceBudgetRowDependencyListByWorkOrder userID={0}, userName={1}, workOrderID={2}, financeBudgetRowID={3}", user.Id, user.UserName, model.WorkOrderID, model.FinanceBudgetRowID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = FinanceBudgetRowDependency.GetByWorkOrder(model.WorkOrderID, model.FinanceBudgetRowID, dataSource);
                    //
                    return new GetFinanceBudgetRowDependencyByWorkOrderOutModel() { Data = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetFinanceBudgetRowDependencyByPurchaseSpecification, workOrderID={0}, financeBudgetRowID={1}", model.WorkOrderID, model.FinanceBudgetRowID);
                return new GetFinanceBudgetRowDependencyByWorkOrderOutModel() { Data = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method InsertFinanceBudgetRowDependency
        public sealed class InsertFinanceBudgetRowDependencyIncomingModel
        {
            public List<InsertFinanceBudgetRowDependencyItemIncomingModel> List { get; set; }
        }
        public sealed class InsertFinanceBudgetRowDependencyItemIncomingModel
        {
            public Guid WorkOrderID { get; set; }
            public Guid FinanceBudgetRowID { get; set; }
            public decimal Sum { get; set; }
        }
        public sealed class InsertFinanceBudgetRowDependencyOutModel
        {
            public List<FinanceBudgetRowDependency> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [Route("finApi/InsertFinanceBudgetRowDependency", Name = "InsertFinanceBudgetRowDependency")]
        public InsertFinanceBudgetRowDependencyOutModel InsertFinanceBudgetRowDependency(InsertFinanceBudgetRowDependencyIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new InsertFinanceBudgetRowDependencyOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                else if (!BLL.Global.IsBudgetEnabled)
                    return new InsertFinanceBudgetRowDependencyOutModel() { List = null, Result = RequestResponceType.OperationError };
                //
                Logger.Trace("FinanceApiController.InsertFinanceBudgetRowDependency userID={0}, userName={1}, count={2}", user.Id, user.UserName, model.List.Count);
                //
                var woList = model.List.Select(x => x.WorkOrderID).Distinct().ToArray();
                if (woList.Length != 1)
                    return new InsertFinanceBudgetRowDependencyOutModel() { List = null, Result = RequestResponceType.BadParamsError };
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var wo = BLL.SD.WorkOrders.WorkOrder.Get(woList[0], user.User.ID, dataSource);
                    if (!wo.AccessIsGranted(user.User, dataSource))
                    {
                        Logger.Trace("FinanceApiController.InsertFinanceBudgetRowDependency userID={0}, userName={1}, ID={2} failed (access denied)", user.Id, user.UserName, wo.ID);
                        return new InsertFinanceBudgetRowDependencyOutModel() { List = null, Result = RequestResponceType.AccessError };
                    }
                    //
                    var retval = new List<FinanceBudgetRowDependency>();
                    dataSource.BeginTransaction();
                    foreach (var item in model.List)
                        retval.Add(FinanceBudgetRowDependency.Insert(item.WorkOrderID, item.FinanceBudgetRowID, item.Sum, dataSource, user.User));
                    dataSource.CommitTransaction();
                    //
                    return new InsertFinanceBudgetRowDependencyOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "InsertFinanceBudgetRowDependency");
                return new InsertFinanceBudgetRowDependencyOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method DeleteFinanceBudgetRowDependency
        public sealed class DeleteFinanceBudgetRowDependencyIncomingModel
        {
            public Guid WorkOrderID { get; set; }
            public List<Guid> FinanceBudgetRowIDList { get; set; }
        }
        [HttpPost]
        [Route("finApi/DeleteFinanceBudgetRowDependency", Name = "DeleteFinanceBudgetRowDependency")]
        public RequestResponceType DeleteFinanceBudgetRowDependency(DeleteFinanceBudgetRowDependencyIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return RequestResponceType.NullParamsError;
                else if (!BLL.Global.IsBudgetEnabled)
                    return RequestResponceType.OperationError;
                //
                Logger.Trace("FinanceApiController.DeleteFinanceBudgetRowDependency userID={0}, userName={1}, workOrderID={2}", user.Id, user.UserName, model.WorkOrderID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var wo = BLL.SD.WorkOrders.WorkOrder.Get(model.WorkOrderID, user.User.ID, dataSource);
                    if (!wo.AccessIsGranted(user.User, dataSource))
                    {
                        Logger.Trace("FinanceApiController.DeleteFinanceBudgetRowDependency userID={0}, userName={1}, ID={2} failed (access denied)", user.Id, user.UserName, model.WorkOrderID);
                        return RequestResponceType.AccessError;
                    }
                    //
                    dataSource.BeginTransaction();
                    foreach (Guid id in model.FinanceBudgetRowIDList)
                        FinanceBudgetRowDependency.Delete(model.WorkOrderID, id, dataSource, user.User);
                    dataSource.CommitTransaction();
                    //
                    return RequestResponceType.Success;
                }
            }
            catch (NotSupportedException)
            {
                return RequestResponceType.BadParamsError;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "DeleteFinanceBudgetRowDependency, workOrderID={0}", model.WorkOrderID);
                return RequestResponceType.GlobalError;
            }
        }
        #endregion


        #region method GetFinanceBudgetRow
        public sealed class GetFinanceBudgetRowIncomingModel
        {
            public Guid FinanceBudgetRowID { get; set; }
        }
        public sealed class GetFinanceBudgetRowOutModel
        {
            public FinanceBudgetRow Data { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetFinanceBudgetRow", Name = "GetFinanceBudgetRow")]
        public GetFinanceBudgetRowOutModel GetFinanceBudgetRow([FromQuery] GetFinanceBudgetRowIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetFinanceBudgetRowOutModel() { Data = null, Result = RequestResponceType.NullParamsError };
                else if (!BLL.Global.IsBudgetEnabled)
                    return new GetFinanceBudgetRowOutModel() { Data = null, Result = RequestResponceType.OperationError };
                //
                Logger.Trace("FinanceApiController.GetFinanceBudgetRowOutModel userID={0}, userName={1}, id={2}", user.Id, user.UserName, model.FinanceBudgetRowID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = FinanceBudgetRow.Get(model.FinanceBudgetRowID, dataSource);
                    if (!FinanceBudgetRow.CheckAccess(user.User.ID, retval.FinanceCenterID, dataSource))
                        return new GetFinanceBudgetRowOutModel() { Data = null, Result = RequestResponceType.AccessError };
                    //
                    return new GetFinanceBudgetRowOutModel() { Data = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetFinanceBudgetRowList, id={0}.", model.FinanceBudgetRowID);
                return new GetFinanceBudgetRowOutModel() { Data = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method SaveFinanceBudgetRow               
        public sealed class SaveFinanceBudgetRowOut : ResultWithMessage
        {
            private SaveFinanceBudgetRowOut() : base() { }

            public Guid? ID { get; private set; }

            public static SaveFinanceBudgetRowOut Create(RequestResponceType type, Guid id)
            {
                var retval = new SaveFinanceBudgetRowOut();
                retval.Result = type;
                retval.Message = string.Empty;
                retval.ID = id;
                //
                return retval;
            }

            public static new SaveFinanceBudgetRowOut Create(RequestResponceType type, string message, bool result = false)
            {
                var retval = new SaveFinanceBudgetRowOut();
                retval.Result = type;
                retval.Message = message;
                retval.IsResult = result;
                //
                return retval;
            }
        }

        [HttpPost]
        [Route("finApi/saveFinanceBudgetRow", Name = "SaveFinanceBudgetRow")]
        public SaveFinanceBudgetRowOut SaveFinanceBudgetRow(DTL.Finance.FinanceBudgetRow info)
        {
            var user = base.CurrentUser;
            if (info == null || user == null)
                return SaveFinanceBudgetRowOut.Create(RequestResponceType.NullParamsError, Resources.ErrorCaption);
            else if (!BLL.Global.IsBudgetEnabled)
                return SaveFinanceBudgetRowOut.Create(RequestResponceType.OperationError, Resources.OperationError);
            //
            Logger.Trace("FinanceApiController.SaveFinanceBudgetRowIn userID={0}, userName={1}", user.Id, user.UserName);
            if (!user.User.HasRoles ||
                (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_FinanceBudgetRow_Add) && !info.ID.HasValue) ||
                (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_FinanceBudgetRow_Update) && info.ID.HasValue))
                return SaveFinanceBudgetRowOut.Create(RequestResponceType.OperationError, Resources.AccessError);
            //         
            List<object> documentList = null;
            List<string> paths = null;
            if (info.AdjustmentContext != null && info.AdjustmentContext.Files != null)
            {
                var api = new FileApiController(_environment);
                if (!api.GetDocumentFromFiles(info.AdjustmentContext.Files, out documentList, out paths, CurrentUser))
                    return SaveFinanceBudgetRowOut.Create(RequestResponceType.OperationError, Resources.UploadedFileNotFoundAtServerSide);
            }
            //   
            try
            {
                FinanceBudgetRow obj;
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!FinanceBudgetRow.CheckAccess(user.User.ID, info.FinanceCenterID, dataSource))
                        return SaveFinanceBudgetRowOut.Create(RequestResponceType.AccessError, Resources.AccessError);
                    //
                    if (info.AdjustmentContext != null && info.AdjustmentContext.InitiatorID == Guid.Empty)
                        info.AdjustmentContext.InitiatorID = user.User.ID;
                    //
                    dataSource.BeginTransaction();
                    if (!info.ID.HasValue)
                        obj = FinanceBudgetRow.Insert(info, documentList, dataSource, user.User);
                    else
                        obj = FinanceBudgetRow.Update(info, documentList, dataSource, user.User);
                    dataSource.CommitTransaction();
                }
                return SaveFinanceBudgetRowOut.Create(RequestResponceType.Success, obj.ID);
            }
            catch (ArgumentValidationException ex)
            {
                return SaveFinanceBudgetRowOut.Create(RequestResponceType.ValidationError, string.Format(Resources.ArgumentValidationException, ex.Message));
            }
            catch (NotSupportedException ex)
            {
                return SaveFinanceBudgetRowOut.Create(RequestResponceType.OperationError, string.Format(Resources.OperationError, ex.Message));
            }
            catch (ObjectConcurrencyException ex)
            {
                return SaveFinanceBudgetRowOut.Create(RequestResponceType.ConcurrencyError, string.Format(Resources.ConcurrencyError, ex.Message));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return SaveFinanceBudgetRowOut.Create(RequestResponceType.GlobalError, Resources.ErrorCaption);
            }
        }
        #endregion

        #region method DeleteFinanceBudgetRow        
        public sealed class DeleteFinanceBudgetRowIn
        {
            public List<Guid> IDList { get; set; }
            public DTL.Finance.FinanceBudgetRowAdjustmentContext AdjustmentContext { get; set; }
        }

        [HttpPost]
        [Route("finApi/deleteFinanceBudgetRow", Name = "DeleteFinanceBudgetRow")]
        public ResultWithMessage DeleteFinanceBudgetRow(DeleteFinanceBudgetRowIn info)
        {
            var user = base.CurrentUser;
            if (info == null || user == null)
                return ResultWithMessage.Create(RequestResponceType.NullParamsError, Resources.ErrorCaption);
            else if (!BLL.Global.IsBudgetEnabled)
                return ResultWithMessage.Create(RequestResponceType.OperationError, Resources.OperationError);
            //
            Logger.Trace("FinanceApiController.DeleteFinanceBudgetRow userID={0}, userName={1}", user.Id, user.UserName);
            if (!user.User.HasRoles ||
                !user.User.OperationIsGranted(IMSystem.Global.OPERATION_FinanceBudgetRow_Delete))
                return ResultWithMessage.Create(RequestResponceType.OperationError, Resources.AccessError);
            //         
            List<object> documentList = null;
            List<string> paths = null;
            if (info.AdjustmentContext != null && info.AdjustmentContext.Files != null)
            {
                var api = new FileApiController(_environment);
                    if (!api.GetDocumentFromFiles(info.AdjustmentContext.Files, out documentList, out paths, user))
                        return SaveFinanceBudgetRowOut.Create(RequestResponceType.OperationError, Resources.UploadedFileNotFoundAtServerSide);
            }
            //            
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    foreach (var id in info.IDList)
                    {
                        var row = FinanceBudgetRow.Get(id, dataSource);
                        if (!FinanceBudgetRow.CheckAccess(user.User.ID, row.FinanceCenterID, dataSource))
                            return ResultWithMessage.Create(RequestResponceType.AccessError, Resources.AccessError);
                    }
                    //
                    if (info.AdjustmentContext != null && info.AdjustmentContext.InitiatorID == Guid.Empty)
                        info.AdjustmentContext.InitiatorID = user.User.ID;
                    //
                    dataSource.BeginTransaction();
                    foreach (var id in info.IDList)
                        FinanceBudgetRow.Delete(id, info.AdjustmentContext, documentList, dataSource, user.User);
                    dataSource.CommitTransaction();
                }
                return ResultWithMessage.Create(RequestResponceType.Success);
            }
            catch (NotSupportedException ex)
            {
                return SaveFinanceBudgetRowOut.Create(RequestResponceType.OperationError, string.Format(Resources.OperationError, ex.Message));
            }
            catch (ArgumentValidationException ex)
            {
                return SaveFinanceBudgetRowOut.Create(RequestResponceType.ValidationError, ex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ResultWithMessage.Create(RequestResponceType.GlobalError, Resources.ErrorCaption);
            }
        }
        #endregion


        #region method GetFinanceBudgetRowTable
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("finApi/GetFinanceBudgetRowTable", Name = "GetFinanceBudgetRowTable")]
        public TableHelper.GetTableOutModel GetFinanceBudgetRowTable([FromForm] frmFinanceBudgetRowTableRequestInfo requestInfo)
        {
            if (!BLL.Global.IsBudgetEnabled)
                return null;
            //
            var user = base.CurrentUser;
            return TableHelper.GetListForTable(requestInfo, user, TableHelper.TableType.frmFinanceBudgetRow_Table);
        }
        #endregion

        #region method GetFinanceBudgetRowSearchList
        public sealed class GetFinanceBudgetRowSearchListIncomingModel
        {
            public List<Guid> IDList { get; set; }
            public Guid PurchaseSpecificationOrWorkOrderID { get; set; }//TODO можно было был и обойтись, но тогда дублировать запрос еще раз
        }

        public sealed class GetFinanceBudgetRowSearchListOutModel
        {
            public List<FinanceBudgetRowSearchForTable> List { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [Route("finApi/GetFinanceBudgetRowSearchList", Name = "GetFinanceBudgetRowSearchList")]
        public GetFinanceBudgetRowSearchListOutModel GetFinanceBudgetRowSearchList([FromForm]GetFinanceBudgetRowSearchListIncomingModel model)
        {
            var user = base.CurrentUser;
            //
            if (model == null || model.IDList == null || model.IDList.Count == 0)
                return new GetFinanceBudgetRowSearchListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
            else if (!BLL.Global.IsBudgetEnabled)
                return new GetFinanceBudgetRowSearchListOutModel() { List = null, Result = RequestResponceType.OperationError };
            //
            Logger.Trace("FinanceApiController.GetSearchedARSByID UserID={0}, UserName={1}", user.User.ID, user.User.Name);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var requestInfo = new frmFinanceBudgetRowTableRequestInfo()
                    {
                        CountRecords = model.IDList.Count,
                        IDList = model.IDList.ToArray(),
                        StartRecordIndex = 0,
                        ViewName = FinanceBudgetRowSearchForTable.VIEW_NAME,
                        PurchaseSpecificationOrWorkOrderID = model.PurchaseSpecificationOrWorkOrderID
                    };
                    var retval = FinanceBudgetRowSearchForTable.GetList(null, requestInfo, user.User, dataSource);
                    //
                    return new GetFinanceBudgetRowSearchListOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetFinanceBudgetRowSearchListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion


        #region method GetFinanceBudgetRowPurchaseSpecificationDependency
        public sealed class GetFinanceBudgetRowPurchaseSpecificationDependencyListIncomingModel
        {
            public Guid FinanceBudgetRowID { get; set; }
        }
        public sealed class GetFinanceBudgetRowPurchaseSpecificationDependencyListOutModel
        {
            public IList<FinanceBudgetRowPurchaseSpecificationDependency> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetFinanceBudgetRowPurchaseSpecificationDependencyList", Name = "GetFinanceBudgetRowPurchaseSpecificationDependencyList")]
        public GetFinanceBudgetRowPurchaseSpecificationDependencyListOutModel GetFinanceBudgetRowPurchaseSpecificationDependencyList([FromQuery] GetFinanceBudgetRowPurchaseSpecificationDependencyListIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetFinanceBudgetRowPurchaseSpecificationDependencyListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                else if (!BLL.Global.IsBudgetEnabled)
                    return new GetFinanceBudgetRowPurchaseSpecificationDependencyListOutModel() { List = null, Result = RequestResponceType.OperationError };
                //
                Logger.Trace("FinanceApiController.GetFinanceBudgetRowPurchaseSpecificationDependencyList userID={0}, userName={1}, financeBudgetRowID={2}", user.Id, user.UserName, model.FinanceBudgetRowID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = FinanceBudgetRowPurchaseSpecificationDependency.GetListByFinanceBudgetRow(model.FinanceBudgetRowID, dataSource);
                    return new GetFinanceBudgetRowPurchaseSpecificationDependencyListOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetFinanceBudgetRowPurchaseSpecificationDependencyListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion


        #region method GetFinanceBudgetRowDependencyList
        public sealed class GetFinanceBudgetRowDependencyListIncomingModel
        {
            public Guid FinanceBudgetRowID { get; set; }
        }
        public sealed class GetFinanceBudgetRowDependencyListOutModel
        {
            public IList<ActivesRequestSpecificationDependency> ARSList { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetFinanceBudgetRowDependencyList", Name = "GetFinanceBudgetRowDependencyList")]
        public GetFinanceBudgetRowDependencyListOutModel GetFinanceBudgetRowDependencyList([FromQuery] GetFinanceBudgetRowDependencyListIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetFinanceBudgetRowDependencyListOutModel() { ARSList = null, Result = RequestResponceType.NullParamsError };
                else if (!BLL.Global.IsBudgetEnabled)
                    return new GetFinanceBudgetRowDependencyListOutModel() { ARSList = null, Result = RequestResponceType.OperationError };
                //
                Logger.Trace("FinanceApiController.GetFinanceBudgetRowDependencyList userID={0}, userName={1}, financeBudgetRowID={2}", user.Id, user.UserName, model.FinanceBudgetRowID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = ActivesRequestSpecificationDependency.GetListByFinanceBudgetRow(model.FinanceBudgetRowID, dataSource);
                    //
                    return new GetFinanceBudgetRowDependencyListOutModel() { ARSList = retval, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetFinanceBudgetRowDependencyList not supported, model: '{0}'", model));
                return new GetFinanceBudgetRowDependencyListOutModel() { ARSList = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetFinanceBudgetRowDependencyList, model: {0}.", model);
                return new GetFinanceBudgetRowDependencyListOutModel() { ARSList = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetFinanceBudgetRowDependencyItem
        public sealed class GetFinanceBudgetRowDependencyInfo
        {
            public Guid ActiveRequstSpecificationID { get; set; }
            public int Count { get; set; }
        }
        public sealed class GetFinanceBudgetRowDependencyItemIncomingModel
        {
            public Guid FinanceBudgetRowID { get; set; }
            public List<GetFinanceBudgetRowDependencyInfo> List { get; set; }
        }
        public sealed class GetFinanceBudgetRowDependencyItemOutModel
        {
            public IList<ActivesRequestSpecificationDependency> ARSList { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("finApi/GetFinanceBudgetRowDependencyItem", Name = "GetFinanceBudgetRowDependencyItem")]
        public GetFinanceBudgetRowDependencyItemOutModel GetFinanceBudgetRowDependencyItem(GetFinanceBudgetRowDependencyItemIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetFinanceBudgetRowDependencyItemOutModel() { ARSList = null, Result = RequestResponceType.NullParamsError };
                else if (!BLL.Global.IsBudgetEnabled)
                    return new GetFinanceBudgetRowDependencyItemOutModel() { ARSList = null, Result = RequestResponceType.OperationError };
                //
                Logger.Trace("FinanceApiController.GetFinanceBudgetRowDependencyItem userID={0}, userName={1}, financeBudgetRowID={2}", user.Id, user.UserName, model.FinanceBudgetRowID);
                //
                var retval = new List<ActivesRequestSpecificationDependency>();
                using (var dataSource = DataSource.GetDataSource())
                {
                    foreach (var item in model.List)
                    {
                        var tmp = ActivesRequestSpecificationDependency.GetByFinanceBudgetRow(model.FinanceBudgetRowID, item.ActiveRequstSpecificationID, item.Count, dataSource);
                        if (tmp == null)
                            throw new NotSupportedException();
                        retval.Add(tmp);
                    }
                }
                return new GetFinanceBudgetRowDependencyItemOutModel() { ARSList = retval, Result = RequestResponceType.Success };
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetFinanceBudgetRowDependencyItem not supported, model: '{0}'", model));
                return new GetFinanceBudgetRowDependencyItemOutModel() { ARSList = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetFinanceBudgetRowDependencyItem, model: {0}.", model);
                return new GetFinanceBudgetRowDependencyItemOutModel() { ARSList = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion


        //#region method GetFinanceBudgetRowAdjustmentInfo
        //public sealed class GetFinanceBudgetRowAdjustmentInfoIncomingModel
        //{
        //    public Guid ID { get; set; }
        //}
        //public sealed class GetFinanceBudgetRowAdjustmentInfoOutModel
        //{
        //    public FinanceBudgetRowAdjustmentInfo Data { get; set; }
        //    public RequestResponceType Result { get; set; }
        //}
        //[HttpGet]
        //[AcceptVerbs("GET")]
        //[Route("finApi/GetFinanceBudgetRowAdjustmentInfo", Name = "GetFinanceBudgetRowAdjustmentInfo")]
        //public GetFinanceBudgetRowAdjustmentInfoOutModel GetFinanceBudgetRowAdjustmentInfo([FromQuery] GetFinanceBudgetRowAdjustmentInfoIncomingModel model)
        //{
        //    try
        //    {
        //        var user = base.CurrentUser;
        //        if (user == null)
        //            return new GetFinanceBudgetRowAdjustmentInfoOutModel() { Data = null, Result = RequestResponceType.NullParamsError };
        //        //
        //        Logger.Trace("FinanceApiController.GetFinanceBudgetRowAdjustmentInfo userID={0}, userName={1}, id={2}", user.Id, user.UserName, model.ID);
        //        //
        //        using (var dataSource = DataSource.GetDataSource())
        //        {
        //            var retval = FinanceBudgetRowAdjustmentInfo.Get(model.ID, dataSource);
        //            //
        //            return new GetFinanceBudgetRowAdjustmentInfoOutModel() { Data = retval, Result = RequestResponceType.Success };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex, "GetFinanceBudgetRowAdjustmentInfo, id={0}.", model.ID);
        //        return new GetFinanceBudgetRowAdjustmentInfoOutModel() { Data = null, Result = RequestResponceType.GlobalError };
        //    }
        //}
        //#endregion

        #region method GetFinanceBudgetRowAdjustment
        public sealed class GetFinanceBudgetRowAdjustmentIncomingModel
        {
            public Guid ID { get; set; }
        }
        public sealed class GetFinanceBudgetRowAdjustmentOutModel
        {
            public FinanceBudgetRowAdjustment Data { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetFinanceBudgetRowAdjustment", Name = "GetFinanceBudgetRowAdjustment")]
        public GetFinanceBudgetRowAdjustmentOutModel GetFinanceBudgetRowAdjustment([FromQuery] GetFinanceBudgetRowAdjustmentIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetFinanceBudgetRowAdjustmentOutModel() { Data = null, Result = RequestResponceType.NullParamsError };
                else if (!BLL.Global.IsBudgetEnabled)
                    return new GetFinanceBudgetRowAdjustmentOutModel() { Data = null, Result = RequestResponceType.OperationError };
                //
                Logger.Trace("FinanceApiController.GetFinanceBudgetRowAdjustment userID={0}, userName={1}, id={2}", user.Id, user.UserName, model.ID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = FinanceBudgetRowAdjustment.Get(model.ID, dataSource);
                    //
                    return new GetFinanceBudgetRowAdjustmentOutModel() { Data = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetFinanceBudgetRowAdjustment, id={0}.", model.ID);
                return new GetFinanceBudgetRowAdjustmentOutModel() { Data = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetFinanceBudgetRowAdjustmentInfoList     
        public sealed class GetFinanceBudgetRowAdjustmentInfoListIncomingModel
        {
            public Guid FinanceBudgetID { get; set; }
        }
        public sealed class GetFinanceBudgetRowAdjustmentInfoListOutModel
        {
            public IList<FinanceBudgetRowAdjustmentInfo> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("finApi/GetFinanceBudgetRowAdjustmentInfoList", Name = "GetFinanceBudgetRowAdjustmentInfoList")]
        public GetFinanceBudgetRowAdjustmentInfoListOutModel GetFinanceBudgetRowAdjustmentInfoList([FromQuery] GetFinanceBudgetRowAdjustmentInfoListIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetFinanceBudgetRowAdjustmentInfoListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                else if (!BLL.Global.IsBudgetEnabled)
                    return new GetFinanceBudgetRowAdjustmentInfoListOutModel() { List = null, Result = RequestResponceType.OperationError };
                //
                Logger.Trace("FinanceApiController.GetFinanceBudgetRowAdjustmentInfoList userID={0}, userName={1}, objID={2}", user.Id, user.UserName, model.FinanceBudgetID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = FinanceBudgetRowAdjustmentInfo.GetList(model.FinanceBudgetID, dataSource).OrderBy(x => x.UtcDate).ThenBy(x => x.RowIdentifier).ThenBy(x => x.RowName).ToList();
                    //
                    return new GetFinanceBudgetRowAdjustmentInfoListOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetFinanceBudgetRowAdjustmentInfoList");
                return new GetFinanceBudgetRowAdjustmentInfoListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion
    }
}