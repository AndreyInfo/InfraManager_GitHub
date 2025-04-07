using InfraManager.Core.Data;
using InfraManager.Core.Logging;
using InfraManager.Web.BLL.SD.Calls;
using InfraManager.Web.BLL.History;
using ProblemBll = InfraManager.Web.BLL.SD.Problems.Problem;
using InfraManager.Web.BLL.SD.SDNote;
using InfraManager.Web.BLL.SD.WorkOrders;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using InfraManager.Web.BLL.SD.Negotiations;
using InfraManager.Web.BLL.SD.RFC;

namespace InfraManager.Web.Controllers.SD
{
    public partial class SDApiController
    {
        #region method GetHistoryList
        public sealed class GetHistoryListIncomingModel
        {
            public Guid ID { get; set; }
            public int EntityClassId { get; set; }
            public string ViewName { get; set; }
            public int StartIdx { get; set; }
            public int Count { get; set; }
        }
        public sealed class GetHistoryListOutModel
        {
            public IList<HistoryObject> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetHistory", Name = "GetHistory")]
        public GetHistoryListOutModel GetHistoryList([FromQuery] GetHistoryListIncomingModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetHistoryListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetHistoryList userID={0}, userName={1}, objectID={2}, entityClassID={3}", user.Id, user.UserName, model.ID, model.EntityClassId);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.EntityClassId == IMSystem.Global.OBJ_CALL)
                    {
                        if (!Call.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Trace("SDApiController.GetCallHistory userID={0}, userName={1}, objectID={2} failed (access denied)", user.Id, user.UserName, model.ID);
                            return new GetHistoryListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                        var asEngineer = (user.User.HasRoles && user.IsEngineerView(model.ViewName));
                        //
                        var retval = HistoryObject.GetList(model.ID, !asEngineer, dataSource, model.StartIdx, model.Count);
                        return new GetHistoryListOutModel() { List = retval, Result = RequestResponceType.Success };
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_WORKORDER)
                    {
                        if (!WorkOrder.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Trace("SDApiController.GetCallHistory userID={0}, userName={1}, objectID={2} failed (access denied)", user.Id, user.UserName, model.ID);
                            return new GetHistoryListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                        //
                        var retval = HistoryObject.GetList(model.ID, false, dataSource, model.StartIdx, model.Count);
                        return new GetHistoryListOutModel() { List = retval, Result = RequestResponceType.Success };
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_PROBLEM)
                    {
                        if (!ProblemBll.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Trace("SDApiController.GetCallHistory userID={0}, userName={1}, objectID={2} failed (access denied)", user.Id, user.UserName, model.ID);
                            return new GetHistoryListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                        //
                        var retval = HistoryObject.GetList(model.ID, false, dataSource, model.StartIdx, model.Count);
                        return new GetHistoryListOutModel() { List = retval, Result = RequestResponceType.Success };
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_RFC)
                    {
                        if (!RFC.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Trace("SDApiController.GetCallHistory userID={0}, userName={1}, objectID={2} failed (access denied)", user.Id, user.UserName, model.ID);
                            return new GetHistoryListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                        //
                        var retval = HistoryObject.GetList(model.ID, false, dataSource, model.StartIdx, model.Count);
                        return new GetHistoryListOutModel() { List = retval, Result = RequestResponceType.Success };
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_Negotiation)
                    {                        
                        var retval = HistoryObject.GetList(model.ID, false, dataSource);
                        return new GetHistoryListOutModel() { List = retval, Result = RequestResponceType.Success };
                    }
                    else throw new NotSupportedException("entityClassID not valid");
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetHistoryList not supported, model: '{0}'", model));
                return new GetHistoryListOutModel() { List = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetHistoryList, model: {0}.", model);
                return new GetHistoryListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetNote
        public sealed class GetNoteIncomingModel
        {
            public Guid NoteID { get; set; }
            public Guid EntityID { get; set; }
            public int EntityClassId { get; set; }
        }
        public sealed class GetNoteOutModel
        {
            public SDNote Elem { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetNote", Name = "GetNote")]
        public GetNoteOutModel GetNote([FromQuery] GetNoteIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetNoteOutModel() { Elem = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetNote userID={0}, userName={1}, objID={2}, nodeID={3}, objClassId={4}", user.Id, user.UserName, model.EntityID, model.NoteID, model.EntityClassId);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    SDNote retval = null;
                    if (model.EntityClassId == IMSystem.Global.OBJ_CALL)
                    {
                        if (!Call.AccessIsGranted(model.EntityID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetNote userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.EntityID, model.EntityClassId);
                            return new GetNoteOutModel() { Elem = null, Result = RequestResponceType.AccessError };
                        }
                        retval = SDNote.Get(model.EntityClassId, model.NoteID, user.User.ID, dataSource);
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_WORKORDER)
                    {
                        if (!WorkOrder.AccessIsGranted(model.EntityID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetNote userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.EntityID, model.EntityClassId);
                            return new GetNoteOutModel() { Elem = null, Result = RequestResponceType.AccessError };
                        }
                        retval = SDNote.Get(model.EntityClassId, model.NoteID, user.User.ID, dataSource);
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_PROBLEM)
                    {
                        if (!ProblemBll.AccessIsGranted(model.EntityID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetNote userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.EntityID, model.EntityClassId);
                            return new GetNoteOutModel() { Elem = null, Result = RequestResponceType.AccessError };
                        }
                        retval = SDNote.Get(model.EntityClassId, model.NoteID, user.User.ID, dataSource);
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_RFC)
                    {
                        if (!RFC.AccessIsGranted(model.EntityID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetNote userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.EntityID, model.EntityClassId);
                            return new GetNoteOutModel() { Elem = null, Result = RequestResponceType.AccessError };
                        }
                        retval = SDNote.Get(model.EntityClassId, model.NoteID, user.User.ID, dataSource);
                    }
                    else
                        throw new NotSupportedException("entityClassID not valid");
                    //
                    if (retval != null && retval.IsNote && !user.User.HasRoles && !IsPrivilegeAccess(model.EntityID, dataSource))
                    {
                        Logger.Error("SDApiController.GetNote userID={0}, userName={1}, objID={2}, objClassId={3} failed (user is client)", user.Id, user.UserName, model.EntityID, model.EntityClassId);
                        return new GetNoteOutModel() { Elem = null, Result = RequestResponceType.AccessError };
                    }
                    return new GetNoteOutModel() { Elem = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetNoteOutModel() { Elem = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetNoteList
        public sealed class GetNoteListIncomingModel
        {
            public Guid ID { get; set; }
            public bool OnlyMessages { get; set; }
            public int EntityClassId { get; set; }
        }
        public sealed class GetNoteListOutModel
        {
            public IList<SDNote> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetNoteList", Name = "GetMessages")]
        [Obsolete("Ресурс не поддерживает EF и выводится из работы. Взаме использовать ресурсы по типам объектов типа /api/calls/{id}/notes")]
        public GetNoteListOutModel GetNoteList([FromQuery] GetNoteListIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetNoteListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetNoteList userID={0}, userName={1}, objID={2}, onlyMessages={3}, objClassId={4}", user.Id, user.UserName, model.ID, model.OnlyMessages, model.EntityClassId);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!model.OnlyMessages && !user.User.HasRoles && !IsPrivilegeAccess(model.ID, dataSource))
                    {
                        Logger.Error("SDApiController.GetNoteList userID={0}, userName={1}, objID={2}, objClassId={3} failed (user is client)", user.Id, user.UserName, model.ID, model.EntityClassId);
                        return new GetNoteListOutModel() { List = null, Result = RequestResponceType.AccessError };
                    }
                    //
                    if (model.EntityClassId == IMSystem.Global.OBJ_CALL)
                    {
                        if (!Call.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetNoteList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetNoteListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                        var retval = SDNote.GetList(model.EntityClassId, model.ID, user.User.ID, model.OnlyMessages ? SDNoteType.Message : (SDNoteType?)null, dataSource);
                        return new GetNoteListOutModel() { List = retval, Result = RequestResponceType.Success };
                    }
                    if (model.EntityClassId == IMSystem.Global.OBJ_WORKORDER)
                    {
                        if (!WorkOrder.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetNoteList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetNoteListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                        var retval = SDNote.GetList(model.EntityClassId, model.ID, user.User.ID, SDNoteType.Note, dataSource);
                        return new GetNoteListOutModel() { List = retval, Result = RequestResponceType.Success };
                    }
                    if (model.EntityClassId == IMSystem.Global.OBJ_PROBLEM)
                    {
                        if (!ProblemBll.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetNoteList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetNoteListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                        var retval = SDNote.GetList(model.EntityClassId, model.ID, user.User.ID, SDNoteType.Note, dataSource);
                        return new GetNoteListOutModel() { List = retval, Result = RequestResponceType.Success };
                    }
                    if (model.EntityClassId == IMSystem.Global.OBJ_RFC)
                    {
                        if (!RFC.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetNoteList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetNoteListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                        var retval = SDNote.GetList(model.EntityClassId, model.ID, user.User.ID, SDNoteType.Note, dataSource);
                        return new GetNoteListOutModel() { List = retval, Result = RequestResponceType.Success };
                    }
                    else throw new NotSupportedException("entityClassID not valid");
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetNoteList not supported, model: '{0}'", model));
                return new GetNoteListOutModel() { List = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetNoteList, model: {0}.", model);
                return new GetNoteListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetTimeline
        public sealed class GetTimelineIncomingModel
        {
            public Guid ObjectID { get; set; }
            public int ObjectClassId { get; set; }
        }
        public sealed class GetTimelineOutModel
        {
            public TimeLine TimeLine { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetTimeline", Name = "GetTimeLine")]
        [Obsolete("Ресурс не поддерживает EF и выводится из работы. Для данных по timeline использовать информацию соответсвующих классо типа /api/calls/{id}")]
        public GetTimelineOutModel GetTimeline([FromQuery] GetTimelineIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetTimelineOutModel() { TimeLine = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetTimeLine userID={0}, userName={1}, objID={2}, objClassId={3}", user.Id, user.UserName, model.ObjectID, model.ObjectClassId);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.ObjectClassId == IMSystem.Global.OBJ_CALL)
                    {
                        if (!Call.AccessIsGranted(model.ObjectID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetTimeLine userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ObjectID, model.ObjectClassId);
                            return new GetTimelineOutModel() { TimeLine = null, Result = RequestResponceType.AccessError };
                        }
                        var retval = CallTimeLine.Get(model.ObjectID, dataSource);
                        return new GetTimelineOutModel() { TimeLine = retval, Result = RequestResponceType.Success };
                    }
                    if (model.ObjectClassId == IMSystem.Global.OBJ_WORKORDER)
                    {
                        if (!WorkOrder.AccessIsGranted(model.ObjectID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetTimeLine userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ObjectID, model.ObjectClassId);
                            return new GetTimelineOutModel() { TimeLine = null, Result = RequestResponceType.AccessError };
                        }
                        var retval = WorkOrderTimeLine.Get(model.ObjectID, dataSource);
                        return new GetTimelineOutModel() { TimeLine = retval, Result = RequestResponceType.Success };
                    }
                    if (model.ObjectClassId == IMSystem.Global.OBJ_PROBLEM)
                    {
                        if (!ProblemBll.AccessIsGranted(model.ObjectID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetTimeLine userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ObjectID, model.ObjectClassId);
                            return new GetTimelineOutModel() { TimeLine = null, Result = RequestResponceType.AccessError };
                        }
                        var retval = ProblemTimeLine.Get(model.ObjectID, dataSource);
                        return new GetTimelineOutModel() { TimeLine = retval, Result = RequestResponceType.Success };
                    }
                    if (model.ObjectClassId == IMSystem.Global.OBJ_RFC)
                    {
                        if (!RFC.AccessIsGranted(model.ObjectID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetTimeLine userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ObjectID, model.ObjectClassId);
                            return new GetTimelineOutModel() { TimeLine = null, Result = RequestResponceType.AccessError };
                        }
                        var retval = RFCTimeLine.Get(model.ObjectID, dataSource);
                        return new GetTimelineOutModel() { TimeLine = retval, Result = RequestResponceType.Success };
                    }
                    else throw new NotSupportedException("ObjectClassID not valid");
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetTimeLine not supported, model: '{0}'", model));
                return new GetTimelineOutModel() { TimeLine = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetTimeLine, model: {0}.", model);
                return new GetTimelineOutModel() { TimeLine = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion
    }
}