using InfraManager.Core.Data;
using InfraManager.Core.Exceptions;
using InfraManager.Core.Helpers;
using InfraManager.Core.Logging;
using InfraManager.Web.BLL.TimeManagement;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Resources = InfraManager.ResourcesArea.Resources;

namespace InfraManager.Web.Controllers.SD
{
    public partial class SDApiController
    {
        #region helper class TimeSheetOutModel
        public sealed class TimeSheetOutModel
        {
            public TimeSheet Value { get; set; }
            public RequestResponceType Result { get; set; }
        }
        #endregion

        #region method GetActiveTimeSheet      
        [HttpGet]
        [Route("sdApi/GetActiveTimeSheet", Name = "GetActiveTimeSheet")]
        public TimeSheetOutModel GetActiveTimeSheet(int year, int dayOfYear, int timezoneOffsetInMinutes)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new TimeSheetOutModel() { Value = null, Result = RequestResponceType.NullParamsError };
            //
            bool firstStep = true;
        start:
            Logger.Trace("SDPaiController.GetActiveTimeSheet userID={0}, userName={1}, year={2}, dayOfYear={3}, timezoneOffsetInMinutes={4}", user.Id, user.UserName, year, dayOfYear, timezoneOffsetInMinutes);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!user.User.HasRoles)
                    {
                        Logger.Error("SDApiController.GetActiveTimeSheet userID={0}, userName={1}, year={2}, dayOfYear={3} failed (access denied)", user.Id, user.UserName, year, dayOfYear);
                        return new TimeSheetOutModel { Value = null, Result = RequestResponceType.AccessError };
                    }
                    //
                    var retval = TimeSheet.GetActiveTimeSheet(user.User, year, dayOfYear, timezoneOffsetInMinutes, dataSource);
                    return new TimeSheetOutModel { Value = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectConcurrencyException)
            {
                if (!firstStep)
                    return new TimeSheetOutModel { Value = null, Result = RequestResponceType.FiltrationError };
                //
                firstStep = false;
                goto start;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения активного табеля");
                return new TimeSheetOutModel { Value = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetActiveTimeSheet      
        [HttpGet]
        [Route("sdApi/GetTimeSheet", Name = "GetTimeSheet")]
        public TimeSheetOutModel GetTimeSheet(Guid timeSheetID, int timezoneOffsetInMinutes)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new TimeSheetOutModel() { Value = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDPaiController.GetTimeSheet userID={0}, userName={1}, timeSheetID={2}, timezoneOffsetInMinutes={3}", user.Id, user.UserName, timeSheetID, timezoneOffsetInMinutes);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!user.User.HasRoles)
                    {
                        Logger.Error("SDApiController.GetActiveTimeSheet userID={0}, userName={1}, timeSheetID={2}, failed (access denied)", user.Id, user.UserName, timeSheetID);
                        return new TimeSheetOutModel { Value = null, Result = RequestResponceType.AccessError };
                    }
                    //
                    var retval = TimeSheet.Get(timeSheetID, user.User.ID, timezoneOffsetInMinutes, dataSource);
                    return new TimeSheetOutModel { Value = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения табеля");
                return new TimeSheetOutModel { Value = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetNextTimeSheet      
        [HttpGet]
        [Route("sdApi/GetNextTimeSheet", Name = "GetNextTimeSheet")]
        public TimeSheetOutModel GetNextTimeSheet(Guid timeSheetID, int timezoneOffsetInMinutes)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new TimeSheetOutModel() { Value = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDPaiController.GetNextTimeSheet userID={0}, userName={1}, timeSheetID={2}, timezoneOffsetInMinutes={3}", user.Id, user.UserName, timeSheetID, timezoneOffsetInMinutes);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!user.User.HasRoles || !TimeSheet.AccessIsGranted(timeSheetID, user.User.ID, dataSource))
                    {
                        Logger.Error("SDApiController.GetNextTimeSheet userID={0}, userName={1}, timeSheetID={2} failed (access denied)", user.Id, user.UserName, timeSheetID);
                        return new TimeSheetOutModel { Value = null, Result = RequestResponceType.AccessError };
                    }
                    //
                    var retval = TimeSheet.GetNextTimeSheet(timeSheetID, user.User, timezoneOffsetInMinutes, dataSource);
                    return new TimeSheetOutModel { Value = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения последующего табеля");
                return new TimeSheetOutModel { Value = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetPreviousTimeSheet      
        [HttpGet]
        [Route("sdApi/GetPreviousTimeSheet", Name = "GetPreviousTimeSheet")]
        public TimeSheetOutModel GetPreviousTimeSheet(Guid timeSheetID, int timezoneOffsetInMinutes)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new TimeSheetOutModel() { Value = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDPaiController.GetPreviousTimeSheet userID={0}, userName={1}, timeSheetID={2}, timezoneOffsetInMinutes={3}", user.Id, user.UserName, timeSheetID, timezoneOffsetInMinutes);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!user.User.HasRoles || !TimeSheet.AccessIsGranted(timeSheetID, user.User.ID, dataSource))
                    {
                        Logger.Error("SDApiController.GetPreviousTimeSheet userID={0}, userName={1}, timeSheetID={2} failed (access denied)", user.Id, user.UserName, timeSheetID);
                        return new TimeSheetOutModel { Value = null, Result = RequestResponceType.AccessError };
                    }
                    //
                    var retval = TimeSheet.GetPreviousTimeSheet(timeSheetID, user.User, timezoneOffsetInMinutes, dataSource);
                    return new TimeSheetOutModel { Value = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения предшествующего табеля");
                return new TimeSheetOutModel { Value = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion


        #region method GetTimeSheetObjectList        
        public sealed class GetTimeSheetItemOutModel
        {
            public TimeSheetItem[] List { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [Route("sdApi/GetTimeSheetItemList", Name = "GetTimeSheetItemList")]
        public GetTimeSheetItemOutModel GetTimeSheetItemList(Guid timeSheetID, int timezoneOffsetInMinutes)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetTimeSheetItemOutModel() { List = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDPaiController.GetTimeSheetObjectList userID={0}, userName={1}, timeSheetID={2}, timezoneOffsetInMinutes={3}", user.Id, user.UserName, timeSheetID, timezoneOffsetInMinutes);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!user.User.HasRoles || !TimeSheet.AccessIsGranted(timeSheetID, user.User.ID, dataSource))
                    {
                        Logger.Error("SDApiController.GetTimeSheetObjectList userID={0}, userName={1}, timeSheetID={2} timezoneOffsetInMinutes={3} failed (access denied)", user.Id, user.UserName, timeSheetID, timezoneOffsetInMinutes);
                        return new GetTimeSheetItemOutModel { List = null, Result = RequestResponceType.AccessError };
                    }
                    //
                    var retval = TimeSheetItem.GetList(timeSheetID, user.User.ID, timezoneOffsetInMinutes, dataSource);
                    return new GetTimeSheetItemOutModel { List = retval.ToArray(), Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка связей табеля");
                return new GetTimeSheetItemOutModel { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetAvailableStateInfos       
        public sealed class GetAvailableStateInfosOutModel
        {
            public TimeSheetStateInfo[] List { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [Route("sdApi/GetAvailableStateInfos", Name = "GetAvailableStateInfos")]
        public GetAvailableStateInfosOutModel GetAvailableStateInfos(Guid timeSheetID)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetAvailableStateInfosOutModel() { List = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDPaiController.GetAvailableStateInfos userID={0}, userName={1}, timeSheetID={2}", user.Id, user.UserName, timeSheetID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!user.User.HasRoles || !TimeSheet.AccessIsGranted(timeSheetID, user.User.ID, dataSource))
                    {
                        Logger.Error("SDApiController.GetAvailableStateInfos userID={0}, userName={1}, timeSheetID={2} failed (access denied)", user.Id, user.UserName, timeSheetID);
                        return new GetAvailableStateInfosOutModel { List = null, Result = RequestResponceType.AccessError };
                    }
                    //
                    var timeSheet = TimeSheet.Get(timeSheetID, user.User.ID, 0, dataSource);//not metter
                    var retval = timeSheet.GetAvailableStateInfos(user.User.ID, dataSource);
                    //
                    return new GetAvailableStateInfosOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException)
            {
                Logger.Trace("SDPaiController.GetAvailableStateInfos userID={0}, userName={1}, timeSheetID={2} timeSheet not found!", user.Id, user.UserName, timeSheetID);
                return new GetAvailableStateInfosOutModel() { List = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка состояний табеля");
                return new GetAvailableStateInfosOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method SetTimeSheetState       
        public sealed class SetTimeSheetStateModel
        {
            public string TimeSheetIDs { get; set; }
            public TimeSheetState StateID { get; set; }
            public String HTMLMessage { get; set; }
            public int TimezoneOffsetInMinutes { get; set; }
            public bool NotifyByEmail { get; set; }
        }

        [HttpPost]
        [Route("sdApi/SetTimeSheetState", Name = "SetTimeSheetState")]
        public RequestResponceType SetTimeSheetState(SetTimeSheetStateModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return RequestResponceType.NullParamsError;
            //
            Logger.Trace("SDPaiController.SetTimeSheetState userID={0}, userName={1}, timeSheetIDs={2}, stateID={3}", user.Id, user.UserName, model.TimeSheetIDs, model.StateID);
            try
            {
                List<Guid> timeSheetIDList = new List<Guid>();
                JArray jArray = JArray.Parse(model.TimeSheetIDs);
                foreach (var item in jArray)
                    timeSheetIDList.Add(Guid.Parse(item.Value<string>()));
                //
                if (timeSheetIDList.Count == 0)
                {
                    Logger.Error("SDApiController.SetTimeSheetState userID={0}, userName={1}, timeSheetIDs={2}, stateID={3} failed (identifiers not set)", user.Id, user.UserName, model.TimeSheetIDs, model.StateID);
                    return RequestResponceType.OperationError;
                }
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction();
                    //
                    foreach (Guid timeSheetID in timeSheetIDList)
                    {
                        if (!user.User.HasRoles || !TimeSheet.AccessIsGranted(timeSheetID, user.User.ID, dataSource))
                        {
                            Logger.Error("SDApiController.SetTimeSheetState userID={0}, userName={1}, timeSheetIDs={2}, stateID={3} failed (access denied)", user.Id, user.UserName, model.TimeSheetIDs, model.StateID);
                            return RequestResponceType.AccessError;
                        }
                        //
                        var timeSheet = TimeSheet.Get(timeSheetID, user.User.ID, 0, dataSource);//not metter                    
                        var availableStates = timeSheet.GetAvailableStateInfos(user.User.ID, dataSource);
                        if (!availableStates.Any(x => x.ID == (byte)model.StateID))
                        {
                            Logger.Error("SDApiController.SetTimeSheetState userID={0}, userName={1}, timeSheetIDs={2}, stateID={3} failed (state not available)", user.Id, user.UserName, model.TimeSheetIDs, model.StateID);
                            return RequestResponceType.OperationError;
                        }
                        //
                        var oldState = timeSheet.State;
                        timeSheet.State = model.StateID;
                        timeSheet.Update(dataSource);
                        //
                        if (oldState != model.StateID)
                        {//history: change state
                            var html = string.Format(
                                model.NotifyByEmail ? Resources.TM_TimeSheet_StateChangedWithNotified : Resources.TM_TimeSheet_StateChanged,
                                TypeHelper.GetFriendlyEnumFieldName(oldState),
                                TypeHelper.GetFriendlyEnumFieldName(model.StateID));
                            var note = new TimeSheetNote(timeSheet.ID, user.User, html);
                            note.Insert(user.User.ID, dataSource);
                        }
                        //
                        if (!string.IsNullOrWhiteSpace(model.HTMLMessage))
                        {
                            var note = new TimeSheetNote(timeSheetID, user.User, model.HTMLMessage);
                            note.Insert(user.User.ID, dataSource);
                        }
                        if (model.NotifyByEmail)
                            TimeSheet.SendUserEmailToManagers(user.User, timeSheet, oldState, model.HTMLMessage, dataSource);
                    }
                    //
                    dataSource.CommitTransaction();
                    //
                    return RequestResponceType.Success;
                }
            }
            catch (ObjectDeletedException)
            {
                Logger.Trace("SDPaiController.SetTimeSheetState userID={0}, userName={1}, timeSheetID={2} timeSheet not found!", user.Id, user.UserName, model.TimeSheetIDs);
                return RequestResponceType.ObjectDeleted;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка смены состояния табеля");
                return RequestResponceType.GlobalError;
            }
        }
        #endregion


        #region method GetTimeSheetInfoList
        public sealed class TimeSheetInfoOutModel
        {
            public TimeSheetInfo[] List { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [Route("sdApi/GetTimeSheetInfoList", Name = "GetTimeSheetInfoList")]
        public TimeSheetInfoOutModel GetTimeSheetInfoList(int minRowIndex, int maxRowIndex, string stateID)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new TimeSheetInfoOutModel() { List = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDPaiController.GetTimeSheetInfoList userID={0}, userName={1}, minRowIndex={2}, stateID={3}, maxRowIndex={4}", user.Id, user.UserName, minRowIndex, stateID, maxRowIndex);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!user.User.HasRoles ||
                            (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_Call_ShowCallsForITSubdivisionInWeb) &&
                            !user.User.OperationIsGranted(IMSystem.Global.OPERATION_WorkOrder_ShowWorkOrdersForITSubdivisionInWeb) &&
                            !user.User.OperationIsGranted(IMSystem.Global.OPERATION_Problem_ShowProblemsForITSubdivisionInWeb))
                        )
                    {
                        Logger.Error("SDApiController.GetTimeSheetInfoList userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                        return new TimeSheetInfoOutModel { List = null, Result = RequestResponceType.AccessError };
                    }
                    //
                    BLL.TimeManagement.TimeSheetState? state = null;
                    byte sID;
                    if (!string.IsNullOrWhiteSpace(stateID) && byte.TryParse(stateID, out sID))
                        state = (TimeSheetState)sID;
                    //
                    var retval = TimeSheetInfo.GetList(user.User.ID, minRowIndex, maxRowIndex, state, dataSource);
                    return new TimeSheetInfoOutModel { List = retval.ToArray(), Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения информации по табелям сотрудников");
                return new TimeSheetInfoOutModel { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion


        #region method AddTimeSheetNote
        public sealed class AddTimeSheetNoteModel
        {
            public Guid TimeSheetID { get; set; }
            public String HTMLMessage { get; set; }
            public int TimezoneOffsetInMinutes { get; set; }
        }
        public sealed class AddTimeSheetNoteOutModel
        {
            public TimeSheetNote Value { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [Route("sdApi/AddTimeSheetNote", Name = "AddTimeSheetNote")]
        public AddTimeSheetNoteOutModel AddTimeSheetNote(AddTimeSheetNoteModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new AddTimeSheetNoteOutModel() { Value = null, Result = RequestResponceType.NullParamsError };
                if (model == null)
                    return new AddTimeSheetNoteOutModel() { Value = null, Result = RequestResponceType.NullParamsError };
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!user.User.HasRoles || !TimeSheet.AccessIsGranted(model.TimeSheetID, user.User.ID, dataSource))
                    {
                        Logger.Error("SDApiController.AddTimeSheetNote userID={0}, userName={1}, timeSheetID={2} failed (access denied)", user.Id, user.UserName, model.TimeSheetID);
                        return new AddTimeSheetNoteOutModel { Result = RequestResponceType.AccessError, Value = null };
                    }
                    //
                    if (String.IsNullOrWhiteSpace(model.HTMLMessage))
                        return new AddTimeSheetNoteOutModel { Result = RequestResponceType.BadParamsError, Value = null };
                    //
                    var retval = new TimeSheetNote(model.TimeSheetID, user.User, model.HTMLMessage);
                    retval.Insert(user.User.ID, dataSource);
                    //
                    return new AddTimeSheetNoteOutModel { Result = RequestResponceType.Success, Value = retval };
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "Ошибка добавления комментария табеля");
                return new AddTimeSheetNoteOutModel { Result = RequestResponceType.GlobalError, Value = null };
            }
        }
        #endregion

        #region method SetTimeSheetNoteReaded
        public sealed class SetTimeSheetNoteReadedModel
        {
            public Guid TimeSheetID { get; set; }
            public Guid NoteID { get; set; }
        }
        [HttpPost]
        [Route("sdApi/setTimeSheetNoteReaded", Name = "SetTimeSheetNoteReaded")]
        public RequestResponceType SetTimeSheetNoteReaded(SetTimeSheetNoteReadedModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return RequestResponceType.NullParamsError;
                if (model == null)
                    return RequestResponceType.NullParamsError;
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!user.User.HasRoles || !TimeSheet.AccessIsGranted(model.TimeSheetID, user.User.ID, dataSource))
                    {
                        Logger.Error("SDApiController.setTimeSheetNoteReaded userID={0}, userName={1}, timeSheetID={2} failed (access denied)", user.Id, user.UserName, model.TimeSheetID);
                        return RequestResponceType.AccessError;
                    }
                    //
                    TimeSheetNote.SetReadedState(model.NoteID, model.TimeSheetID, user.User.ID, dataSource);
                    return RequestResponceType.Success;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "Ошибка установки прочтения комментария табеля");
                return RequestResponceType.GlobalError;
            }
        }
        #endregion

        #region method GetTimeSheetNote
        public sealed class GetTimeSheetNoteOutModel
        {
            public TimeSheetNote Note { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/getTimeSheetNote", Name = "GetTimeSheetNote")]
        public GetTimeSheetNoteOutModel GetTimeSheetNote([FromQuery] Guid noteID, Guid timeSheetID)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetTimeSheetNoteOutModel() { Note = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetTimeSheetNote userID={0}, userName={1}, noteID={2}", user.Id, user.UserName, noteID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!user.User.HasRoles || !TimeSheet.AccessIsGranted(timeSheetID, user.User.ID, dataSource))
                    {
                        Logger.Error("SDApiController.GetTimeSheetNote userID={0}, userName={1}, noteID={2} failed (access denied)", user.Id, user.UserName, noteID);
                        return new GetTimeSheetNoteOutModel() { Note = null, Result = RequestResponceType.AccessError };
                    }
                    //
                    var retval = TimeSheetNote.Get(noteID, user.User.ID, dataSource);
                    return new GetTimeSheetNoteOutModel() { Note = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения комментария табеля");
                return new GetTimeSheetNoteOutModel() { Note = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetTimeSheetNote
        public sealed class GetTimesheetGlobalInfoOutModel
        {
            public int UnreadedMessagesInMyTimesheets { get; set; }
            public int UnreadedMessagesInTotalTimesheet { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/getTimesheetGlobalInfo", Name = "GetTimesheetGlobalInfo")]
        public GetTimesheetGlobalInfoOutModel GetTimesheetGlobalInfo()
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetTimesheetGlobalInfoOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetTimesheetGlobalInfo userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!user.User.HasRoles)
                    {
                        Logger.Error("SDApiController.GetTimesheetGlobalInfo userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                        return new GetTimesheetGlobalInfoOutModel() { Result = RequestResponceType.AccessError };
                    }
                    //
                    var retval = TimeManagement.GetUnreadedCountForUser(user.User.ID, dataSource);
                    return new GetTimesheetGlobalInfoOutModel() { UnreadedMessagesInMyTimesheets = retval.Item1, UnreadedMessagesInTotalTimesheet = retval.Item2, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения информации по непрочитанным сообщениям табеля");
                return new GetTimesheetGlobalInfoOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetTimeSheetNoteList
        public sealed class GetTimeSheetNoteListOutModel
        {
            public TimeSheetNote[] List { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetTimeSheetNoteList", Name = "GetTimeSheetNoteList")]
        public GetTimeSheetNoteListOutModel GetTimeSheetNoteList([FromQuery] Guid timeSheetID)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetTimeSheetNoteListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetTimeSheetNoteList userID={0}, userName={1}, timeSheetID={2}", user.Id, user.UserName, timeSheetID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!user.User.HasRoles || !TimeSheet.AccessIsGranted(timeSheetID, user.User.ID, dataSource))
                    {
                        Logger.Error("SDApiController.GetTimeSheetNoteList userID={0}, userName={1}, timeSheetID={2} failed (access denied)", user.Id, user.UserName, timeSheetID);
                        return new GetTimeSheetNoteListOutModel() { List = null, Result = RequestResponceType.AccessError };
                    }
                    //
                    var retval = TimeSheetNote.GetList(timeSheetID, user.User.ID, dataSource);
                    return new GetTimeSheetNoteListOutModel() { List = retval.ToArray(), Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка комментариев табеля");
                return new GetTimeSheetNoteListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetTimeSheetLastNoteList
        public sealed class GetTimeSheetLastNoteListOutModel
        {
            public String NotesString { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetTimeSheetLastNoteList", Name = "GetTimeSheetLastNoteList")]
        public GetTimeSheetLastNoteListOutModel GetTimeSheetLastNoteList([FromQuery] Guid timeSheetID)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetTimeSheetLastNoteListOutModel() { NotesString = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetTimeSheetLastNoteList userID={0}, userName={1}, timeSheetID={2}", user.Id, user.UserName, timeSheetID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (!user.User.HasRoles || !TimeSheet.AccessIsGranted(timeSheetID, user.User.ID, dataSource))
                    {
                        Logger.Error("SDApiController.GetTimeSheetLastNoteList userID={0}, userName={1}, timeSheetID={2} failed (access denied)", user.Id, user.UserName, timeSheetID);
                        return new GetTimeSheetLastNoteListOutModel() { NotesString = null, Result = RequestResponceType.AccessError };
                    }
                    //
                    var retval = TimeSheetNote.GetLastsList(timeSheetID, user.User.ID, dataSource);
                    return new GetTimeSheetLastNoteListOutModel() { NotesString = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка комментариев табеля");
                return new GetTimeSheetLastNoteListOutModel() { NotesString = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion


        #region method GetUserCalendar
        public sealed class CalendarOutModel
        {
            public Calendar Value { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [Route("sdApi/GetUserCalendar", Name = "GetUserCalendar")]
        public CalendarOutModel GetUserCalendar(int year, int dayOfYear, int timezoneOffsetInMinutes, Guid? userID)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new CalendarOutModel() { Value = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDPaiController.GetUserCalendar userID={0}, userName={1}, year={2}, dayOfYear={3}, timezoneOffsetInMinutes={4}, userID={5}", user.Id, user.UserName, year, dayOfYear, timezoneOffsetInMinutes, userID.HasValue ? userID.Value.ToString() : string.Empty);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_Exclusion_Properties))
                {
                    Logger.Trace("SDApiController.GetUserCalendar userID={0}, userName={1} failed (operation denied)", user.Id, user.UserName);
                    return new CalendarOutModel() { Value = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var targetUser = user.User;
                    if (userID.HasValue)
                        targetUser = BLL.Users.UserInfo.Get(userID.Value, dataSource);
                    //
                    if (!targetUser.HasRoles)
                    {
                        Logger.Error("SDApiController.GetUserCalendar userID={0}, userName={1}, year={2}, dayOfYear={3} failed (access denied)", targetUser.ID, targetUser.FullName, year, dayOfYear);
                        return new CalendarOutModel { Value = null, Result = RequestResponceType.AccessError };
                    }
                    //
                    var retval = Calendar.Get(targetUser, year, dayOfYear, timezoneOffsetInMinutes, dataSource);
                    return new CalendarOutModel { Value = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения календаря пользователя");
                return new CalendarOutModel { Value = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetExclusionList
        public sealed class GetExclusionListOutModel
        {
            public Exclusion[] List { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetExclusionList", Name = "GetExclusionList")]
        public GetExclusionListOutModel GetExclusionList()
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetExclusionListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetExclusionList userID={0}, userName={1}", user.Id, user.UserName);
                //
                if (!user.User.HasRoles)
                {
                    Logger.Error("SDApiController.GetExclusionList userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                    return new GetExclusionListOutModel() { List = null, Result = RequestResponceType.AccessError };
                }
                //
                var retval = Exclusion.GetList();
                return new GetExclusionListOutModel() { List = retval.ToArray(), Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка причин отклонения от графика.");
                return new GetExclusionListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method SaveCalendarExclusion
        public sealed class SaveCalendarExclusionModel
        {
            public Guid ID { get; set; }
            public string StartPeriod { get; set; }
            public string EndPeriod { get; set; }
            public bool IsWorking { get; set; }
            public Guid ExclusionID { get; set; }
            public Guid ObjectID { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/SaveCalendarExclusion", Name = "SaveCalendarExclusion")]
        public RequestResponceType SaveCalendarExclusion(SaveCalendarExclusionModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return RequestResponceType.NullParamsError;
                //
                using (var dataSource = DataSource.GetDataSource())
                    try
                    {
                        dataSource.BeginTransaction();
                        DateTime? startPeriod = BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(model.StartPeriod);
                        DateTime? endPeriod = BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(model.EndPeriod);
                        if (!startPeriod.HasValue || !endPeriod.HasValue)
                            return RequestResponceType.BadParamsError;
                        //
                        bool retval;
                        if (model.ID == Guid.Empty)
                        {
                            if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_Exclusion_Add))
                            {
                                Logger.Trace("SDApiController.SaveCalendarExclusion userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                                return RequestResponceType.OperationError;
                            }
                            retval = CalendarExclustion.Insert(startPeriod.Value, endPeriod.Value, model.IsWorking, model.ExclusionID, model.ObjectID, dataSource);
                        }
                        else
                        {
                            if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_Exclusion_Update))
                            {
                                Logger.Trace("SDApiController.SaveCalendarExclusion userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                                return RequestResponceType.OperationError;
                            }
                            retval = CalendarExclustion.Update(model.ID, startPeriod.Value, endPeriod.Value, model.IsWorking, model.ExclusionID, model.ObjectID, dataSource);
                        }
                        //
                        dataSource.CommitTransaction();
                        return retval ? RequestResponceType.Success : RequestResponceType.ValidationError;
                    }
                    catch
                    {
                        dataSource.RollbackTransaction();
                        throw;
                    }
            }
            catch (TimeSheetObjectInUseException)
            {
                return RequestResponceType.OperationError;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка причин отклонения от графика.");
                return RequestResponceType.GlobalError;
            }
        }
        #endregion

        #region method RemoveCalendarExclusion
        public sealed class RemoveCalendarExclusionModel
        {
            public List<Guid> IDList { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/RemoveCalendarExclusion", Name = "RemoveCalendarExclusion")]
        public RequestResponceType RemoveCalendarExclusion(RemoveCalendarExclusionModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return RequestResponceType.NullParamsError;
                //
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_Exclusion_Delete))
                {
                    Logger.Trace("SDApiController.RemoveCalendarExclusion userID={0}, userName={1} failed (operation denied)", user.Id, user.UserName);
                    return RequestResponceType.OperationError;
                }
                using (var dataSource = DataSource.GetDataSource())
                    try
                    {
                        dataSource.BeginTransaction();
                        CalendarExclustion.Remove(model.IDList, dataSource);
                        dataSource.CommitTransaction();
                        return RequestResponceType.Success;
                    }
                    catch
                    {
                        dataSource.RollbackTransaction();
                        throw;
                    }
            }
            catch (TimeSheetObjectInUseException)
            {
                return RequestResponceType.OperationError;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при удалении отклонения.");
                return RequestResponceType.GlobalError;
            }
        }
        #endregion


        #region method GetProjectStateList
        [HttpGet]
        [Route("sdApi/GetProjectStateList", Name = "GetProjectStateList")]
        public ProjectState[] GetProjectStateList()
        {
            try
            {
                Logger.Trace("SDApiController.GetProjectStateList");
                var list = ProjectState.GetList();
                //
                var retval = list.
                    OrderBy(x => x.Name).
                    ToArray();
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка состояний проекта.");
                return null;
            }
        }
        #endregion

        #region method GetProject
        public sealed class GetProjectOutModel
        {
            public Project Project { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetProject", Name = "GetProject")]
        public GetProjectOutModel GetProject([FromQuery]Guid id)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetProjectOutModel() { Project = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetProject userID={0}, userName={1}, ID={2}", user.Id, user.UserName, id);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = Project.Get(id, user.User.ID, dataSource);
                    if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_Project_Properties))
                    {
                        Logger.Trace("SDApiController.GetProject userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, id);
                        return new GetProjectOutModel() { Project = null, Result = RequestResponceType.OperationError };
                    }
                    return new GetProjectOutModel() { Project = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetProject is NULL, id: '{0}'", id);
                return new GetProjectOutModel() { Project = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetProject, id: '{0}'", id);
                return new GetProjectOutModel() { Project = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetProjectReference
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetProjectReference", Name = "GetProjectReference")]
        public GetProjectOutModel GetProjectReference([FromQuery]Guid id)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetProjectOutModel() { Project = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetProjectReference userID={0}, userName={1}, ID={2}", user.Id, user.UserName, id);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = Project.Get(id, user.User.ID, dataSource);
                    return new GetProjectOutModel() { Project = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetProject is NULL, id: '{0}'", id);
                return new GetProjectOutModel() { Project = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetProject, id: '{0}'", id);
                return new GetProjectOutModel() { Project = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion  
    }
}