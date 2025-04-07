using InfraManager.Core.Data;
using InfraManager.Core.Logging;
using InfraManager.Web.BLL.SD.Calls;
using InfraManager.Web.BLL.SD.Problems;
using InfraManager.Web.BLL.Tables;
using InfraManager.Web.DTL;
using InfraManager.Web.DTL.Tables;
using InfraManager.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.Web.Controllers.SD
{
    public partial class SDApiController
    {
        #region (Table methods)
        //don't touch, used int customers mobile application
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/GetListForTable", Name = "GetListForTable")]
        public TableHelper.GetTableOutModel GetListForTable([FromForm] TableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForTable(requestInfo, user, TableHelper.TableType.SD);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/GetListForObject", Name = "GetListForObject")]
        public ResultData<List<BaseForTable>> GetListForObject([FromForm] TableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            var result = TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.SD);
            return result;
        }
        //for ko.listView
        //[HttpPost]
        //[AcceptVerbs("POST")]
        //[Route("sdApi/GetListForObjectWithoutFilter", Name = "GetListForObjectWithoutFilter")]
        //public ResultData<List<BaseForTable>> GetListForObjectWithoutFilter([FromForm] TableLoadRequestInfo requestInfo)
        //{
        //    var user = base.CurrentUser;
        //    return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.SDWithoutFilter);
        //}
        #endregion

        #region (Operations)
        #region  RemoveObjectList
        public sealed class ObjectListModel
        {
            public List<ObjectInfo> ObjectList { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/RemoveObjectList", Name = "RemoveObjectList")]
        public ResultWithMessage RemoveObjectList(ObjectListModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultWithMessage.Create(RequestResponceType.NullParamsError);
            //
            Logger.Trace("SDApiController.RemoveObjectList userID={0}, userName={1}, ObjectListCount={2}", user.Id, user.UserName, model.ObjectList.Count);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    OperationsHelper.DeleteObjectList(model.ObjectList, dataSource, user.User);
                    //
                    return ResultWithMessage.Create(RequestResponceType.Success);
                }
            }
            catch (ConstraintException ex)
            {
                return ResultWithMessage.Create(RequestResponceType.ValidationError, ex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при удалении.");
                return ResultWithMessage.Create(RequestResponceType.GlobalError, ex.Message);
            }
        }

        #endregion

        #region method SetObjectNoteState
        public sealed class ObjectListWithNoteStateModel
        {
            public bool NoteIsReaded { get; set; }
            public List<ObjectInfo> ObjectList { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/SetObjectNoteState", Name = "SetObjectNoteState")]
        public ResultWithMessage SetObjectNoteState(ObjectListWithNoteStateModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultWithMessage.Create(RequestResponceType.NullParamsError);
            //
            Logger.Trace("SDApiController.SetObjectNoteState userID={0}, userName={1}, ObjectListCount={2}", user.Id, user.UserName, model.ObjectList.Count);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    InfraManager.Web.BLL.SD.SDNote.SDNote.SetNoteReadedState(model.NoteIsReaded, model.ObjectList, user.User.ID, dataSource);
                    //
                    return ResultWithMessage.Create(RequestResponceType.Success);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при изменении состояния сообщения.");
                return ResultWithMessage.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method SetCustomControlToList
        public sealed class ObjectListWithCustomControlModel
        {
            public bool CustomControl { get; set; }
            public List<ObjectInfo> ObjectList { get; set; }
            public List<ObjectInfo> UserList { get; set; }
            public bool IsEngineerView { get; set; }//запрос приходит из инженерного списка или из клиентского
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/SetCustomControlToList", Name = "SetCustomControlToList")]
        [Obsolete("POST to api/{resource (calls, problems, changeRequests, workOrders)/{id}/customcontrols}")]
        public ResultWithMessage SetCustomControlToList(ObjectListWithCustomControlModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultWithMessage.Create(RequestResponceType.NullParamsError);
            //
            Logger.Trace("SDApiController.SetCustomControlToList userID={0}, userName={1}, ObjectListCount={2}", user.Id, user.UserName, model.ObjectList.Count);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.IsEngineerView)
                        OperationsHelper.SetCustomControlToList(model.CustomControl, model.ObjectList, model.UserList, dataSource);
                    else
                        OperationsHelper.SetCustomControl(model.CustomControl, model.ObjectList, user.User.ID, dataSource);
                    //
                    return ResultWithMessage.Create(RequestResponceType.Success);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при установке/снятии личного контроля.");
                return ResultWithMessage.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method PickObjects
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/PickObjects", Name = "PickObjects")]
        public ResultWithMessage PickObjects(ObjectListModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultWithMessage.Create(RequestResponceType.NullParamsError);
            //
            Logger.Trace("SDApiController.PickObjects userID={0}, userName={1}, ObjectListCount={2}", user.Id, user.UserName, model.ObjectList.Count);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    OperationsHelper.PickObjects(user.User, model.ObjectList, dataSource);
                    //
                    return ResultWithMessage.Create(RequestResponceType.Success);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при взятии из очереди.");
                return ResultWithMessage.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method CreateProblemByCall
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/CreateProblemByCall", Name = "CreateProblemByCall")]
        public GetProblemOutModel CreateProblemByCall(ObjectListModel model)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.CreateProblemByCall userID={0}, userName={1}, ObjectListCount={2}", user.Id, user.UserName, model.ObjectList.Count);
            try
            {
                GetProblemOutModel retval = null;
                //
                Guid callID = model.ObjectList[0].ID;
                var res = GetCall(callID);
                var call = res.Call;
                //
                var problem = new Problem(new DTL.SD.Problems.Problem()
                {
                    Description = call.Description,
                    Summary = call.CallSummaryName
                });
                problem.CallList = new List<Guid>(model.ObjectList.Select(x => x.ID));
                //
                retval = new GetProblemOutModel()
                {
                    Problem = problem,
                    Result = res.Result
                };
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при создании проблемы по заявке.");
                return null;
            }
        }
        #endregion

        #region method GetObject
        public sealed class GetObjectOutModel
        {
            public ComponentModel.IObject Object { get; set; }
            public ResultWithMessage Result { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/GetObject", Name = "GetObject")]
        public GetObjectOutModel GetObject(ObjectInfo model)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetObject userID={0}, userName={1}, ClassID={2}, ObjectID={3}", user.Id, user.UserName, model.ClassID, model.ID);
            try
            {
                GetObjectOutModel retval = null;
                //
                if (model.ClassID == IMSystem.Global.OBJ_CALL)
                {
                    var res = GetCall(model.ID);
                    retval = new GetObjectOutModel()
                    {
                        Object = res.Call,
                        Result = ResultWithMessage.Create(res.Result)
                    };
                }
                else if (model.ClassID == IMSystem.Global.OBJ_PROBLEM)
                {
                    var res = GetProblem(model.ID);
                    retval = new GetObjectOutModel()
                    {
                        Object = res.Problem,
                        Result = ResultWithMessage.Create(res.Result)
                    };
                }
                else if (model.ClassID == IMSystem.Global.OBJ_WORKORDER)
                {
                    var res = GetWorkOrder(model.ID);
                    retval = new GetObjectOutModel()
                    {
                        Object = res.WorkOrder,
                        Result = ResultWithMessage.Create(res.Result)
                    };
                }
                else if (model.ClassID == IMSystem.Global.OBJ_RFC)
                {
                    var res = GetRFC(model.ID);
                    retval = new GetObjectOutModel()
                    {
                        Object = res.RFC,
                        Result = ResultWithMessage.Create(res.Result)
                    };
                }
                else
                    throw new NotSupportedException("model.ClassID");
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при получении объекта.");
                return null;
            }
        }
        #endregion
        #endregion



        #region method SetCallsOwner
        public sealed class ObjectCallsOwnerModel
        {
            public List<ObjectInfo> ObjectList { get; set; }
            public Guid UserID { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/SetCallsOwner", Name = "SetCallsOwner")]
        public ResultWithMessage SetCallsOwner(ObjectCallsOwnerModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultWithMessage.Create(RequestResponceType.NullParamsError);
            //
            Logger.Trace("SDApiController.SetCallsOwner userID={0}, userName={1}, ObjectListCount={2}", user.Id, user.UserName, model.ObjectList.Count);
            try
            {
                var message = Call.SetCallsOwner(model.ObjectList.Select(x => x.ID).ToList(), model.UserID, user.User);
                //
                bool isResult = false;
                if (message == string.Empty) {
                    message = "Заявки переданы";
                    isResult = true;
                }
                return ResultWithMessage.Create(RequestResponceType.Success, message, isResult);
            }
            catch (Exception ex) 
            {
                Logger.Error(ex, "Ошибка.");
                return ResultWithMessage.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method GetKBAUserList
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/GetKBAUserList", Name = "GetKBAUserList")]
        public ResultData<List<BaseForTable>> GetContractAndAgreementSearchListForTable(TableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.KBAUserList);
        }
        #endregion

    }
}