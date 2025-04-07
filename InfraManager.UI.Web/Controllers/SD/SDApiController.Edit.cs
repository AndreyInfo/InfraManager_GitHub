using InfraManager.Core.Data;
using InfraManager.Core.Exceptions;
using InfraManager.Core.Extensions;
using InfraManager.Core.Logging;
using InfraManager.Web.BLL;
using InfraManager.Web.BLL.Assets;
using InfraManager.Web.BLL.Fields;
using InfraManager.Web.BLL.Helpers;
using InfraManager.Web.BLL.SD;
using InfraManager.Web.BLL.SD.Calls;
using InfraManager.Web.BLL.SD.Manhours;
using InfraManager.Web.BLL.SD.Negotiations;
using InfraManager.Web.BLL.SD.Problems;
using ProblemBll = InfraManager.Web.BLL.SD.Problems.Problem;
using InfraManager.Web.BLL.SD.RFC;
using InfraManager.Web.BLL.SD.SDNote;
using InfraManager.Web.BLL.SD.WorkOrders;
using InfraManager.Web.BLL.TimeManagement;
using InfraManager.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using InfraManager.UI.Web.Models.ServiceDesk;
using Microsoft.AspNetCore.Mvc;
using Resources = InfraManager.ResourcesArea.Resources;
using InfraManager.Web.BLL.ProductCatalog.Models;

namespace InfraManager.Web.Controllers.SD
{
    partial class SDApiController
    {
        #region method NegotiationVote
        public sealed class NegotiationVoteInputModel
        {
            public Guid NegotiationID { get; set; }
            public String Comment { get; set; }
            public VotingType Type { get; set; }
            public int ClassID { get; set; }
            public Guid ObjectID { get; set; }
            public bool IsFinance { get; set; }
            public Guid VotedUserID { get; set; }
        }
        public sealed class NegotiationVoteOutputModel
        {
            public RequestResponceType Result { get; set; }
            public String UtcDateTimeVote { get; set; }
        }
        [HttpPost]
        [Route("sdApi/NegotiationVote", Name = "NegotiationVote")]
        [Obsolete("PATCH to /api/negotiations/{id}/users/{userId}")]
        public NegotiationVoteOutputModel NegotiationVote(NegotiationVoteInputModel model)
        {
            try
            {
                Guid deputyUserID = model.VotedUserID;
                var user = base.CurrentUser;
                if (user == null)
                {
                    Logger.Error(String.Format(@"NegotiationVote null user, model: '{0}'", model));
                    return new NegotiationVoteOutputModel() { Result = RequestResponceType.NullParamsError, UtcDateTimeVote = String.Empty };
                }
                //
                if (model.Type == VotingType.None)
                {
                    Logger.Error(String.Format(@"NegotiationVote bad params, model: '{0}'", model));
                    return new NegotiationVoteOutputModel() { Result = RequestResponceType.BadParamsError, UtcDateTimeVote = String.Empty };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (user.User.ID != deputyUserID)
                    {
                        var dm = DataManager.GetDataManager<DAL.Users.IUserSettingsDataManager>(dataSource);
                        var UserList = dm.GetDeputyListID(user.User.ID);
                        if (!UserList.Contains(deputyUserID)){                            
                            Logger.Error(String.Format(@"NegotiationVote acces error, model: '{0}'", model));
                            return new NegotiationVoteOutputModel() { Result = RequestResponceType.AccessError };
                        }
                    }
                    if (model.IsFinance)
                    {
                        if (model.ClassID != IMSystem.Global.OBJ_WORKORDER)
                        {
                            Logger.Error(String.Format(@"NegotiationVote finance not in workOrder, bad params, model: '{0}'", model));
                            return new NegotiationVoteOutputModel() { Result = RequestResponceType.BadParamsError, UtcDateTimeVote = String.Empty };
                        }
                        //
                        var dt = FinanceNegotiation.Vote(model.NegotiationID, model.ObjectID, deputyUserID, model.Comment, model.Type, null, dataSource, user.User);
                        return new NegotiationVoteOutputModel() { Result = RequestResponceType.Success, UtcDateTimeVote = JSDateTimeHelper.ToJS(dt) };
                    }
                    else
                    {
                        var dt = Negotiation.Vote(model.NegotiationID, model.ClassID, model.ObjectID, deputyUserID, user.User.ID,  model.Comment, model.Type, dataSource);
                        return new NegotiationVoteOutputModel() { Result = RequestResponceType.Success, UtcDateTimeVote = JSDateTimeHelper.ToJS(dt) };
                    }
                }
            }
            catch (NotSupportedException e)
            {
                Logger.Error(e, String.Format(@"NegotiationVote not supported, model: '{0}'", model));
                return new NegotiationVoteOutputModel() { Result = RequestResponceType.BadParamsError, UtcDateTimeVote = String.Empty };
            }
            catch (ObjectDeletedException e)
            {
                Logger.Error(e, String.Format(@"NegotiationVote object deleted, model: '{0}'", model));
                return new NegotiationVoteOutputModel() { Result = RequestResponceType.ObjectDeleted };
            }
            catch (FieldConcurrencyException e)
            {
                Logger.Error(e, String.Format(@"NegotiationVote field concurrency, model: '{0}'", model));
                return new NegotiationVoteOutputModel() { Result = RequestResponceType.ConcurrencyError };
            }
            catch (ObjectConcurrencyException e)
            {
                Logger.Error(e, String.Format(@"Negotiation already ended, model: '{0}'", model));
                return new NegotiationVoteOutputModel() { Result = RequestResponceType.NegotiationEnded };
            }
            catch (ArgumentValidationException e)
            {
                Logger.Error(e, String.Format(@"NegotiationVote validation error, model: '{0}'", model));
                return new NegotiationVoteOutputModel() { Result = RequestResponceType.ValidationError, UtcDateTimeVote = String.Empty };
            }
            catch (Exception e)
            {
                Logger.Error(e, String.Format(@"NegotiationVote, model: '{0}'", model));
                return new NegotiationVoteOutputModel() { Result = RequestResponceType.GlobalError, UtcDateTimeVote = String.Empty };
            }
        }
        #endregion

        #region method NegotiationMessage
        public sealed class NegotiationMessageInputModel
        {
            public Guid NegotiationID { get; set; }
            public Guid VotedUserID { get; set; }
            public String Comment { get; set; }
        }
        public sealed class NegotiationMessageOutputModel
        {
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [Route("sdApi/NegotiationMessage", Name = "NegotiationMessage")]
        [Obsolete("PATCH to /api/negotiations/{id}/users/{userId}")]
        public NegotiationMessageOutputModel NegotiationMessage(NegotiationMessageInputModel model)
        {
            try
            {
                Guid deputyUserID = model.VotedUserID;
                var user = base.CurrentUser;
                if (user == null)
                {
                    Logger.Error(String.Format(@"NegotiationVote null user, model: '{0}'", model));
                    return new NegotiationMessageOutputModel() { Result = RequestResponceType.NullParamsError};
                }
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (user.User.ID != deputyUserID)
                    {
                        var dm = DataManager.GetDataManager<DAL.Users.IUserSettingsDataManager>(dataSource);
                        var UserList = dm.GetDeputyListID(user.User.ID);
                        if (!UserList.Contains(deputyUserID))
                        {
                            Logger.Error(String.Format(@"NegotiationVote acces error, model: '{0}'", model));
                            return new NegotiationMessageOutputModel() { Result = RequestResponceType.AccessError };
                        }
                    }
                    Negotiation.SetNegotiationMessage(model.NegotiationID, deputyUserID, model.Comment, dataSource);
                        return new NegotiationMessageOutputModel() { Result = RequestResponceType.Success };                    
                }
            }
            catch (NotSupportedException e)
            {
                Logger.Error(e, String.Format(@"NegotiationVote not supported, model: '{0}'", model));
                return new NegotiationMessageOutputModel() { Result = RequestResponceType.BadParamsError, };
            }
            catch (ObjectDeletedException e)
            {
                Logger.Error(e, String.Format(@"NegotiationVote object deleted, model: '{0}'", model));
                return new NegotiationMessageOutputModel() { Result = RequestResponceType.ObjectDeleted };
            }
            catch (FieldConcurrencyException e)
            {
                Logger.Error(e, String.Format(@"NegotiationVote field concurrency, model: '{0}'", model));
                return new NegotiationMessageOutputModel() { Result = RequestResponceType.ConcurrencyError };
            }
            catch (ObjectConcurrencyException e)
            {
                Logger.Error(e, String.Format(@"Negotiation already ended, model: '{0}'", model));
                return new NegotiationMessageOutputModel() { Result = RequestResponceType.NegotiationEnded };
            }
            catch (ArgumentValidationException e)
            {
                Logger.Error(e, String.Format(@"NegotiationVote validation error, model: '{0}'", model));
                return new NegotiationMessageOutputModel() { Result = RequestResponceType.ValidationError };
            }
            catch (Exception e)
            {
                Logger.Error(e, String.Format(@"NegotiationVote, model: '{0}'", model));
                return new NegotiationMessageOutputModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion
        #region method AddNote
        public sealed class AddNoteModel
        {
            public Guid Id { get; set; }
            public String Message { get; set; }
            public SDNoteType Type { get; set; }
            public int EntityClassId { get; set; }
        }
        public sealed class AddNoteOutModel
        {
            public SDNote Elem { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [Route("sdApi/AddNote", Name = "AddNote")]
        public AddNoteOutModel AddNote(AddNoteModel model)
        {
            var user = base.CurrentUser;
            if (model != null && user != null)
            {
                try
                {
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        if (model.EntityClassId == IMSystem.Global.OBJ_CALL)
                        {
                            if (!Call.AccessIsGranted(model.Id, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.AddNote userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.Id);
                                return new AddNoteOutModel { Result = RequestResponceType.AccessError, Elem = null };
                            }
                        }
                        else if (model.EntityClassId == IMSystem.Global.OBJ_WORKORDER)
                        {
                            if (!WorkOrder.AccessIsGranted(model.Id, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.AddNote userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.Id);
                                return new AddNoteOutModel { Result = RequestResponceType.AccessError, Elem = null };
                            }
                        }
                        else if (model.EntityClassId == IMSystem.Global.OBJ_PROBLEM)
                        {
                            if (!ProblemBll.AccessIsGranted(model.Id, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.AddNote userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.Id);
                                return new AddNoteOutModel { Result = RequestResponceType.AccessError, Elem = null };
                            }
                        }
                        else if (model.EntityClassId == IMSystem.Global.OBJ_RFC)
                        {
                            if (!RFC.AccessIsGranted(model.Id, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.AddNote userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.Id);
                                return new AddNoteOutModel { Result = RequestResponceType.AccessError, Elem = null };
                            }
                        }
                        else throw new NotSupportedException("entityClassID not valid");
                        //
                        if (String.IsNullOrWhiteSpace(model.Message))
                            return new AddNoteOutModel { Result = RequestResponceType.BadParamsError, Elem = null };
                        //
                        var note = SDNote.AddNote(model.EntityClassId, model.Id, this.CurrentUser.User, model.Type, model.Message, dataSource);
                        WorkflowWrapper.MakeOnSaveReaction(model.Id, model.EntityClassId, dataSource, user.User);
                        //
                        return new AddNoteOutModel { Result = RequestResponceType.Success, Elem = note };
                    }
                }
                catch (NotSupportedException e)
                {
                    Logger.Error(e, String.Format(@"Add Note not supported, model: '{0}'", model));
                    return new AddNoteOutModel { Result = RequestResponceType.BadParamsError, Elem = null };
                }
                catch (ArgumentValidationException e)
                {
                    Logger.Error(e, String.Format(@"Add Note validation error, model: '{0}'", model));
                    return new AddNoteOutModel { Result = RequestResponceType.ValidationError, Elem = null };
                }
                catch (Exception e)
                {
                    Logger.Error(e, String.Format(@"Add Note, model: '{0}'", model));
                    return new AddNoteOutModel { Result = RequestResponceType.GlobalError, Elem = null };
                }
            }
            else
                return new AddNoteOutModel { Result = RequestResponceType.NullParamsError, Elem = null };
        }
        #endregion

        #region method SetNoteState
        public sealed class SetNoteStateModel
        {
            public Guid Id { get; set; }
            public Guid NoteId { get; set; }
            public Boolean NewState { get; set; }
            public int EntityClassId { get; set; }
        }
        [HttpPost]
        [Route("sdApi/SetNoteState", Name = "SetNoteState")]
        public RequestResponceType SetNoteState(SetNoteStateModel model)
        {
            try
            {
                SDNote.SetNoteReadedState(model.EntityClassId, this.CurrentUser.User.ID, model.Id, model.NoteId, model.NewState);
                return RequestResponceType.Success;
            }
            catch (NotSupportedException e)
            {
                Logger.Error(e, String.Format(@"Set Note State not supported, model: '{0}'", model));
                return RequestResponceType.BadParamsError;
            }
            catch (Exception e)
            {
                Logger.Error(e, String.Format(@"Set Note State, model: '{0}'", model));
                return RequestResponceType.GlobalError;
            }
        }
        #endregion

        #region method SetField
        public sealed class SetFieldOutModel
        {
            public ResultWithMessage ResultWithMessage { get; set; }
            //
            public Guid? CurrentObjectID { get; set; }
            public int? CurrentObjectClassID { get; set; }
            public object CurrentObjectValue { get; set; }
        }

        [HttpPost]
        [Obsolete("Patch to api/calls/{id}, api/problems/{id} etc.")]
        [Route("sdApi/SetField", Name = "SDSetField")]
        public SetFieldOutModel SetField([FromForm]SetValueModel model)
        {
            const int maxMessageLength = 100;
            var user = base.CurrentUser;
            if (model != null && user != null)
            {
                if (model.Params == null)
                    model.Params = new string[0];
                //
                Logger.Trace(String.Format(@"Set Field. ObjClassID: '{6}', Id: '{0}'. Field: '{1}'. OldValue: '{2}'; NewValue: '{3}'; Replace Anyway: '{4}'; Params: '{5}'",
                             model.ID.ToString(),
                             model.Field,
                             model.OldValue.Truncate(maxMessageLength),
                             model.NewValue.Truncate(maxMessageLength),
                             model.ReplaceAnyway.ToString(),
                             String.Concat(model.Params),
                             model.ObjClassID));
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    bool needCommite = false;
                    try
                    {
                        dataSource.BeginTransaction();
                        if (model.ObjClassID == IMSystem.Global.OBJ_CALL)
                        {
                            if (!IsPrivilegeAccess(model.ID, dataSource))
                            {
                                if (!Call.AccessIsGranted(model.ID, user.User, false, dataSource))
                                {
                                    Logger.Error("SDApiController.SetField userID={0}, userName={1}, ID={2}, objClassID={3} failed (access denied)", user.Id, user.UserName, model.ID, model.ObjClassID);
                                    return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.AccessError) };
                                }
                                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_Call_Update) && !user.IsClientView(dataSource))
                                {
                                    Logger.Trace("SDApiController.SetField userID={0}, userName={1}, ID={2}, objClassID={3} failed (operation denied)", user.Id, user.UserName, model.ID, model.ObjClassID);
                                    return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.OperationError) };
                                }
                            }
                            Call.SetField(model, CurrentUser.User.ID, dataSource, CurrentUser.User);
                            WorkflowWrapper.MakeOnSaveReaction(model.ID, model.ObjClassID, dataSource, CurrentUser.User);
                            needCommite = true;
                            return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.Success) };
                        }
                        else if (model.ObjClassID == IMSystem.Global.OBJ_WORKORDER)
                        {
                            if (!IsPrivilegeAccess(model.ID, dataSource))
                            {
                                if (!WorkOrder.AccessIsGranted(model.ID, user.User, false, dataSource))
                                {
                                    Logger.Error("SDApiController.SetField userID={0}, userName={1}, ID={2} failed (access denied)", user.Id, user.UserName, model.ID);
                                    return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.AccessError) };
                                }
                                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_WorkOrder_Update))
                                {
                                    Logger.Trace("SDApiController.SetField userID={0}, userName={1}, ID={2}, objClassID={3} failed (operation denied)", user.Id, user.UserName, model.ID, model.ObjClassID);
                                    return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.OperationError) };
                                }
                            }
                            WorkOrder.SetField(model, CurrentUser.User, dataSource);
                            WorkflowWrapper.MakeOnSaveReaction(model.ID, model.ObjClassID, dataSource, CurrentUser.User);
                            needCommite = true;
                            return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.Success) };
                        }
                        else if (model.ObjClassID == IMSystem.Global.OBJ_PROBLEM)
                        {
                            if (!IsPrivilegeAccess(model.ID, dataSource))
                            {
                                if (!ProblemBll.AccessIsGranted(model.ID, user.User, false, dataSource))
                                {
                                    Logger.Error("SDApiController.SetField userID={0}, userName={1}, ID={2} failed (access denied)", user.Id, user.UserName, model.ID);
                                    return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.AccessError) };
                                }
                                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_Problem_Update))
                                {
                                    Logger.Trace("SDApiController.SetField userID={0}, userName={1}, ID={2}, objClassID={3} failed (operation denied)", user.Id, user.UserName, model.ID, model.ObjClassID);
                                    return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.OperationError) };
                                }
                            }
                            ProblemBll.SetField(model, CurrentUser.User.ID, dataSource, CurrentUser.User);
                            WorkflowWrapper.MakeOnSaveReaction(model.ID, model.ObjClassID, dataSource, CurrentUser.User);
                            needCommite = true;
                            return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.Success) };
                        }
                        //
                        else if (model.ObjClassID == IMSystem.Global.OBJ_RFC)
                        {
                            if (!IsPrivilegeAccess(model.ID, dataSource))
                            {
                                if (!RFC.AccessIsGranted(model.ID, user.User, false, dataSource))
                                {
                                    Logger.Error("SDApiController.SetField userID={0}, userName={1}, ID={2} failed (access denied)", user.Id, user.UserName, model.ID);
                                    return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.AccessError) };
                                }
                                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_RFC_Update))
                                {
                                    Logger.Trace("SDApiController.SetField userID={0}, userName={1}, ID={2}, objClassID={3} failed (operation denied)", user.Id, user.UserName, model.ID, model.ObjClassID);
                                    return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.OperationError) };
                                }
                            }
                            RFC.SetField(model, CurrentUser.User, dataSource);
                            WorkflowWrapper.MakeOnSaveReaction(model.ID, model.ObjClassID, dataSource, CurrentUser.User);
                            needCommite = true;
                            return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.Success) };
                        }
                        //
                        else if (model.ObjClassID == IMSystem.Global.OBJ_Negotiation)
                        {
                            bool hasDuplicates;
                            string msg;
                            //
                            Negotiation.SetField(model, CurrentUser.User.ID, dataSource, out hasDuplicates, out msg);
                            needCommite = true;
                            return new SetFieldOutModel { ResultWithMessage = !hasDuplicates ? ResultWithMessage.Create(RequestResponceType.Success) : ResultWithMessage.Create(RequestResponceType.HasDuplicates, msg) };
                        }
                        //
                        else if (model.ObjClassID == IMSystem.Global.OBJ_Project)
                        {
                            if (!IsPrivilegeAccess(model.ID, dataSource))
                            {
                                if (!Project.AccessIsGranted(model.ID, user.User, false, dataSource))
                                {
                                    Logger.Error("SDApiController.SetField userID={0}, userName={1}, ID={2}, objClassID={3} failed (access denied)", user.Id, user.UserName, model.ID, model.ObjClassID);
                                    return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.AccessError) };
                                }
                                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_Project_Update) && !user.IsClientView(dataSource))
                                {
                                    Logger.Trace("SDApiController.SetField userID={0}, userName={1}, ID={2}, objClassID={3} failed (operation denied)", user.Id, user.UserName, model.ID, model.ObjClassID);
                                    return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.OperationError) };
                                }
                            }
                            Project.SetField(model, CurrentUser.User.ID, dataSource);
                            needCommite = true;
                            return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.Success) };
                        }
                        else if (model.ObjClassID == IMSystem.Global.OBJ_NETWORKDEVICE)
                        {
                            if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_UPDATE_NETWORKDEVICE) && !user.IsClientView(dataSource))
                            {
                                Logger.Trace("SDApiController.SetField userID={0}, userName={1}, ID={2}, objClassID={3} failed (operation denied)", user.Id, user.UserName, model.ID, model.ObjClassID);
                                return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.OperationError) };
                            }
                            //
                            bool hasDuplicates;
                            string msg;
                            //
                            NetworkDevice.SetField(model, CurrentUser.User.ID, dataSource, out hasDuplicates, out msg);
                            needCommite = true;
                            return new SetFieldOutModel { ResultWithMessage = !hasDuplicates ? ResultWithMessage.Create(RequestResponceType.Success) : ResultWithMessage.Create(RequestResponceType.HasDuplicates, msg) };

                        }
                        else if (model.ObjClassID == IMSystem.Global.OBJ_TERMINALDEVICE)
                        {
                            if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_UPDATE_TERMINALDEVICE) && !user.IsClientView(dataSource))
                            {
                                Logger.Trace("SDApiController.SetField userID={0}, userName={1}, ID={2}, objClassID={3} failed (operation denied)", user.Id, user.UserName, model.ID, model.ObjClassID);
                                return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.OperationError) };
                            }
                            //
                            bool hasDuplicates;
                            string msg;
                            //
                            TerminalDevice.SetField(model, CurrentUser.User.ID, dataSource, out hasDuplicates, out msg);
                            needCommite = true;
                            return new SetFieldOutModel { ResultWithMessage = !hasDuplicates ? ResultWithMessage.Create(RequestResponceType.Success) : ResultWithMessage.Create(RequestResponceType.HasDuplicates, msg) };
                        }
                        else if (model.ObjClassID == IMSystem.Global.OBJ_ADAPTER)
                        {
                            if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_UPDATE_ADAPTER) && !user.IsClientView(dataSource))
                            {
                                Logger.Trace("SDApiController.SetField userID={0}, userName={1}, ID={2}, objClassID={3} failed (operation denied)", user.Id, user.UserName, model.ID, model.ObjClassID);
                                return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.OperationError) };
                            }
                            //
                            bool hasDuplicates;
                            string msg;
                            //
                            Adapter.SetField(model, CurrentUser.User.ID, dataSource, out hasDuplicates, out msg);
                            needCommite = true;
                            return new SetFieldOutModel { ResultWithMessage = !hasDuplicates ? ResultWithMessage.Create(RequestResponceType.Success) : ResultWithMessage.Create(RequestResponceType.HasDuplicates, msg) };
                        }
                        else if (model.ObjClassID == IMSystem.Global.OBJ_PERIPHERAL)
                        {
                            if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_UPDATE_PERIPHERAL) && !user.IsClientView(dataSource))
                            {
                                Logger.Trace("SDApiController.SetField userID={0}, userName={1}, ID={2}, objClassID={3} failed (operation denied)", user.Id, user.UserName, model.ID, model.ObjClassID);
                                return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.OperationError) };
                            }
                            //
                            bool hasDuplicates;
                            string msg;
                            //
                            Peripheral.SetField(model, CurrentUser.User.ID, dataSource, out hasDuplicates, out msg);
                            needCommite = true;
                            return new SetFieldOutModel { ResultWithMessage = !hasDuplicates ? ResultWithMessage.Create(RequestResponceType.Success) : ResultWithMessage.Create(RequestResponceType.HasDuplicates, msg) };
                        }
                        else if (model.ObjClassID == IMSystem.Global.OBJ_SoftwareLicenceModel)
                        {
                            if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_UPDATE_SOFTWAREMODEL) && !user.IsClientView(dataSource))
                            {
                                Logger.Trace("SDApiController.SetField userID={0}, userName={1}, ID={2}, objClassID={3} failed (operation denied)", user.Id, user.UserName, model.ID, model.ObjClassID);
                                return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.OperationError) };
                            }
                            //
                            bool hasDuplicates;
                            string msg;
                            //
                            SoftwareModelForForm.SetField(model, CurrentUser.User.ID, dataSource, out hasDuplicates, out msg);
                            needCommite = true;
                            return new SetFieldOutModel { ResultWithMessage = !hasDuplicates ? ResultWithMessage.Create(RequestResponceType.Success) : ResultWithMessage.Create(RequestResponceType.HasDuplicates, msg) };
                        }
                        else if (model.ObjClassID == IMSystem.Global.OBJ_ASSET)
                        {
                            AssetFields.SetField(model, CurrentUser.User.ID, dataSource);
                            needCommite = true;
                            return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.Success) };
                        }
                        else if (model.ObjClassID == IMSystem.Global.OBJ_ServiceContract)
                        {
                            BLL.Contracts.Contract.SetField(model, CurrentUser.User.ID, dataSource);
                            needCommite = true;
                            return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.Success) };
                        }
                        else if (model.ObjClassID == IMSystem.Global.OBJ_ServiceContractAgreement)
                        {
                            BLL.Contracts.ContractAgreement.SetField(model, CurrentUser.User.ID, dataSource);
                            needCommite = true;
                            return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.Success) };
                        }
                        else if (model.ObjClassID == IMSystem.Global.OBJ_InventorySpecification)
                        {
                            BLL.Inventories.InventorySpecification.SetField(model, CurrentUser.User.ID, dataSource);
                            needCommite = true;
                            return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.Success) };
                        }
                        else if (model.ObjClassID == IMSystem.Global.OBJ_SOFTWARE_LICENSE)
                        {
                            try
                            {
                                BLL.Software.SoftwareLicence.SetField(model, CurrentUser.User.ID, dataSource);
                            }
                            catch (ConstraintException ex)
                            {
                                return new SetFieldOutModel
                                {
                                    ResultWithMessage = ResultWithMessage.Create(RequestResponceType.ValidationError, ex.Message)
                                };
                            }

                            needCommite = true;
                            return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.Success) };
                        }
                        else if (model.ObjClassID == IMSystem.Global.OBJ_ConfigurationUnit)
                        {
                            bool hasDuplicates;
                            string msg;
                            //
                            ConfigurationUnit.SetField(model, CurrentUser.User.ID, dataSource, out hasDuplicates, out msg);
                            needCommite = true;
                            return new SetFieldOutModel { ResultWithMessage = !hasDuplicates ? ResultWithMessage.Create(RequestResponceType.Success) : ResultWithMessage.Create(RequestResponceType.HasDuplicates, msg) };
                        }
                        else if (model.ObjClassID == IMSystem.Global.OBJ_Cluster)
                        {
                            bool hasDuplicates;
                            string msg;
                            //
                            Cluster.SetField(model, CurrentUser.User.ID, dataSource, out hasDuplicates, out msg);
                            needCommite = true;
                            return new SetFieldOutModel { ResultWithMessage = !hasDuplicates ? ResultWithMessage.Create(RequestResponceType.Success) : ResultWithMessage.Create(RequestResponceType.HasDuplicates, msg) };
                        }
                        else if (model.ObjClassID == IMSystem.Global.OBJ_LogicalObject || model.ObjClassID == IMSystem.Global.OBJ_LogicalServer||
                            model.ObjClassID == IMSystem.Global.OBJ_LogicalComputer || model.ObjClassID == IMSystem.Global.OBJ_LogicalCommutator || model.ObjClassID == IMSystem.Global.CAT_LogicalComponent)
                        {
                            bool hasDuplicates;
                            string msg;
                            //
                            LogicalObject.SetField(model, CurrentUser.User.ID, dataSource, out hasDuplicates, out msg);
                            needCommite = true;
                            return new SetFieldOutModel { ResultWithMessage = !hasDuplicates ? ResultWithMessage.Create(RequestResponceType.Success) : ResultWithMessage.Create(RequestResponceType.HasDuplicates, msg) };
                        }
                        else if (model.ObjClassID == IMSystem.Global.OBJ_DataEntity)
                        {
                            bool hasDuplicates;
                            string msg;
                            //
                            DataEntityObject.SetField(model, CurrentUser.User.ID, dataSource, out hasDuplicates, out msg);
                            needCommite = true;
                            return new SetFieldOutModel { ResultWithMessage = !hasDuplicates ? ResultWithMessage.Create(RequestResponceType.Success) : ResultWithMessage.Create(RequestResponceType.HasDuplicates, msg) };
                        }
                        else throw new NotImplementedException("model.ObjClassID");
                    }
                    catch (InvalidOperationException e)
                    {
                        Logger.Error(e, String.Format(@"operation denied error. ObjClassID: '{6}'. Id: '{0}'. Field: '{1}'. OldValue: '{2}'; NewValue: '{3}'; Replace Anyway: '{4}'; Params: '{5}'",
                                 model.ID.ToString(),
                                 model.Field,
                                 model.OldValue.Truncate(maxMessageLength),
                                 model.NewValue.Truncate(maxMessageLength),
                                 model.ReplaceAnyway.ToString(),
                                 String.Concat(model.Params),
                                 model.ObjClassID));
                        return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.OperationError) };
                    }
                    catch (NotImplementedException e)
                    {
                        Logger.Error(e, String.Format(@"Set Field parameters error. ObjClassID: '{6}'. Id: '{0}'. Field: '{1}'. OldValue: '{2}'; NewValue: '{3}'; Replace Anyway: '{4}'; Params: '{5}'",
                                 model.ID.ToString(),
                                 model.Field,
                                 model.OldValue.Truncate(maxMessageLength),
                                 model.NewValue.Truncate(maxMessageLength),
                                 model.ReplaceAnyway.ToString(),
                                 String.Concat(model.Params),
                                 model.ObjClassID));
                        return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.BadParamsError) };
                    }
                    catch (FieldConcurrencyException e)
                    {
                        Logger.Error(e, String.Format(@"Set Field concurency error. ObjClassID: '{6}'. Id: '{0}'. Field: '{1}'. OldValue: '{2}'; NewValue: '{3}'; Replace Anyway: '{4}'; Params: '{5}'",
                                 model.ID.ToString(),
                                 model.Field,
                                 model.OldValue.Truncate(maxMessageLength),
                                 model.NewValue.Truncate(maxMessageLength),
                                 model.ReplaceAnyway.ToString(),
                                 String.Concat(model.Params),
                                 model.ObjClassID));
                        return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.ConcurrencyError), CurrentObjectID = e.CurrentObjectID, CurrentObjectClassID = e.CurrentObjectClassID, CurrentObjectValue = e.CurrentObjectValue };
                    }
                    catch (ObjectConcurrencyException e)
                    {
                        Logger.Error(e, String.Format(@"Set Field OBJECT concurency error. ObjClassID: '{6}'. Id: '{0}'. Field: '{1}'. OldValue: '{2}'; NewValue: '{3}'; Replace Anyway: '{4}'; Params: '{5}'",
                                 model.ID.ToString(),
                                 model.Field,
                                 model.OldValue.Truncate(maxMessageLength),
                                 model.NewValue.Truncate(maxMessageLength),
                                 model.ReplaceAnyway.ToString(),
                                 String.Concat(model.Params),
                                 model.ObjClassID));
                        return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.ConcurrencyError), CurrentObjectID = e.ObjectID, CurrentObjectClassID = model.ObjClassID };
                    }
                    catch (ObjectDeletedException e)
                    {
                        Logger.Error(e, String.Format(@"Set Field object deleted error. ObjClassID: '{6}'.  Id: '{0}'. Field: '{1}'. OldValue: '{2}'; NewValue: '{3}'; Replace Anyway: '{4}'; Params: '{5}'",
                                    model.ID.ToString(),
                                    model.Field,
                                    model.OldValue.Truncate(maxMessageLength),
                                    model.NewValue.Truncate(maxMessageLength),
                                    model.ReplaceAnyway.ToString(),
                                    String.Concat(model.Params),
                                    model.ObjClassID));
                        return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.ObjectDeleted) };
                    }
                    catch (ArgumentValidationException e)
                    {
                        Logger.Error(e, String.Format(@"Set Field argument validation error. ObjClassID: '{6}'. Id: '{0}'. Field: '{1}'. OldValue: '{2}'; NewValue: '{3}'; Replace Anyway: '{4}'; Params: '{5}'",
                                    model.ID.ToString(),
                                    model.Field,
                                    model.OldValue.Truncate(maxMessageLength),
                                    model.NewValue.Truncate(maxMessageLength),
                                    model.ReplaceAnyway.ToString(),
                                    String.Concat(model.Params),
                                    model.ObjClassID));
                        return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.ValidationError, e.Message) };
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e, String.Format(@"Set Field. ObjClassID: '{6}'. Id: '{0}'. Field: '{1}'. OldValue: '{2}'; NewValue: '{3}'; Replace Anyway: '{4}'; Params: '{5}'",
                                 model.ID.ToString(),
                                 model.Field,
                                 model.OldValue.Truncate(maxMessageLength),
                                 model.NewValue.Truncate(maxMessageLength),
                                 model.ReplaceAnyway.ToString(),
                                 String.Concat(model.Params),
                                 model.ObjClassID));
                        return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.GlobalError) };
                    }
                    finally
                    {
                        if (needCommite)
                            dataSource.CommitTransaction();
                    }
                }
            }
            else return new SetFieldOutModel { ResultWithMessage = ResultWithMessage.Create(RequestResponceType.NullParamsError) };
        }
        #endregion

        #region method SaveTableParameterWidths
        public sealed class SaveTableParameterWidthsIn
        {
            public Guid ID { get; set; }
            public Guid ObjectID { get; set; }
            public int[] Widths { get; set; }
        }

        [HttpPost]
        [Route("sdApi/SaveTableParameterWidths", Name = "SaveTableParameterWidths")]
        public bool SaveTableParameterWidths([FromForm]SaveTableParameterWidthsIn model)
        {
            try
            {
                Logger.Trace("SaveTableParameterWidths id={0}, objectID={1}", model.ID, model.ObjectID);
                BLL.Parameters.ParameterValue.SaveTableParameterWidths(model.ID, model.ObjectID, model.Widths);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }
        #endregion

        #region method EditManhours
        public sealed class EditManhoursOutModel
        {
            public Guid ID { get; set; }
            public DateTime UtcDate { get; set; }
            public ResultWithMessage Response { get; set; }
        }
        [HttpPost]
        [Route("sdApi/EditManhours", Name = "EditManhours")]
        public EditManhoursOutModel EditManhours(ManhoursInfo model)
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
                        case FieldOperation.Edit:
                        case FieldOperation.Remove:
                            operationID = IMSystem.Global.OPERATION_ManhoursWork_Update;
                            break;
                    }
                    if (!user.User.OperationIsGranted(operationID))
                    {
                        Logger.Trace("SDApiController.EditManhours userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                        return new EditManhoursOutModel() { Response = ResultWithMessage.Create(RequestResponceType.OperationError) };
                    }
                    //
                    if (!String.IsNullOrEmpty(model.UtcDateMilliseconds))
                        model.UtcDate = BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(model.UtcDateMilliseconds).Value; //парсим
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        if (model.ObjectClassID.HasValue && model.ObjectID.HasValue)
                        {
                            if (model.ObjectClassID == IMSystem.Global.OBJ_CALL)
                            {
                                if (!Call.AccessIsGranted(model.ObjectID.Value, user.User, true, dataSource))
                                {
                                    Logger.Error("SDApiController.EditManhours userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID.Value);
                                    return new EditManhoursOutModel() { Response = ResultWithMessage.Create(RequestResponceType.AccessError) };
                                }
                            }
                            else if (model.ObjectClassID == IMSystem.Global.OBJ_WORKORDER)
                            {
                                if (!WorkOrder.AccessIsGranted(model.ObjectID.Value, user.User, true, dataSource))
                                {
                                    Logger.Error("SDApiController.EditManhours userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID.Value);
                                    return new EditManhoursOutModel() { Response = ResultWithMessage.Create(RequestResponceType.AccessError) };
                                }
                            }
                            else if (model.ObjectClassID == IMSystem.Global.OBJ_PROBLEM)
                            {
                                if (!ProblemBll.AccessIsGranted(model.ObjectID.Value, user.User, true, dataSource))
                                {
                                    Logger.Error("SDApiController.EditManhours userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID.Value);
                                    return new EditManhoursOutModel() { Response = ResultWithMessage.Create(RequestResponceType.AccessError) };
                                }
                            }
                            else if (model.ObjectClassID == IMSystem.Global.OBJ_RFC)
                            {
                                if (!RFC.AccessIsGranted(model.ObjectID.Value, user.User, true, dataSource))
                                {
                                    Logger.Error("SDApiController.EditManhours userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID.Value);
                                    return new EditManhoursOutModel() { Response = ResultWithMessage.Create(RequestResponceType.AccessError) };
                                }
                            }
                            else if (model.ObjectClassID == IMSystem.Global.OBJ_Project)
                            {
                                if (!Project.AccessIsGranted(model.ObjectID.Value, user.User, true, dataSource))
                                {
                                    Logger.Error("SDApiController.EditManhours userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID.Value);
                                    return new EditManhoursOutModel() { Response = ResultWithMessage.Create(RequestResponceType.AccessError) };
                                }
                            }
                            else throw new NotSupportedException("ObjectClassID not valid");
                        }
                        //
                        var result = Manhours.Edit(model, dataSource, user.User);
                        //
                        if (model.ObjectClassID.HasValue && model.ObjectID.HasValue)
                            WorkflowWrapper.MakeOnSaveReaction(model.ObjectID.Value, model.ObjectClassID.Value, dataSource, user.User);
                        //
                        return new EditManhoursOutModel() { Response = ResultWithMessage.Create(RequestResponceType.Success), ID = result.Item1, UtcDate = result.Item2 };
                    }
                }
                catch (NotSupportedException e)
                {
                    Logger.Error(e, String.Format(@"EditManhours not supported, model: '{0}'", model));
                    return new EditManhoursOutModel() { Response = ResultWithMessage.Create(RequestResponceType.BadParamsError) };
                }
                catch (ObjectDeletedException)
                {
                    Logger.Trace(String.Format(@"EditManhours object deleted, model: '{0}'", model));
                    return new EditManhoursOutModel() { Response = ResultWithMessage.Create(RequestResponceType.ObjectDeleted) };
                }
                catch (ArgumentValidationException e)
                {
                    Logger.Trace(String.Format(@"EditManhours validation error, model: '{0}'", model));
                    return new EditManhoursOutModel() { Response = ResultWithMessage.Create(RequestResponceType.ValidationError, ValidatorHelper.CreateErrorMessage(e)) };
                }
                catch (TimeSheetObjectInUseException)
                {
                    return new EditManhoursOutModel() { Response = ResultWithMessage.Create(RequestResponceType.OperationError, Resources.TM_ObjectInUse) };
                }
                catch (Exception e)
                {
                    Logger.Error(e, String.Format(@"EditManhours, model: '{0}'", model));
                    return new EditManhoursOutModel() { Response = ResultWithMessage.Create(RequestResponceType.GlobalError) };
                }
            }
            else return new EditManhoursOutModel() { Response = ResultWithMessage.Create(RequestResponceType.NullParamsError) };
        }
        #endregion

        #region method EditManhoursWork

        public class EditManhoursWorkOutModel
        {
            public Guid ID { get; set; }
            public int Number { get; set; }
            public ResultWithMessage Response { get; set; }
        }

        [HttpPost]
        [Route("sdApi/EditManhoursWork", Name = "EditManhoursWork")]
        public EditManhoursWorkOutModel EditWorkManhours(ManhoursWorkInfo model)
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
                            operationID = IMSystem.Global.OPERATION_ManhoursWork_Add;
                            break;
                        case FieldOperation.Edit:
                            operationID = IMSystem.Global.OPERATION_ManhoursWork_Update;
                            break;
                        case FieldOperation.Remove:
                            operationID = IMSystem.Global.OPERATION_ManhoursWork_Delete;
                            break;
                    }
                    if (!user.User.OperationIsGranted(operationID))
                    {
                        Logger.Trace("SDApiController.EditWorkManhours userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                        return new EditManhoursWorkOutModel() { Response = ResultWithMessage.Create(RequestResponceType.OperationError) };
                    }
                    //
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        if (model.ObjectClassID.HasValue && model.ObjectID.HasValue)
                        {
                            if (model.ObjectClassID == IMSystem.Global.OBJ_CALL)
                            {
                                if (!Call.AccessIsGranted(model.ObjectID.Value, user.User, true, dataSource))
                                {
                                    Logger.Error("SDApiController.EditManhoursWork userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID.Value);
                                    return new EditManhoursWorkOutModel() { Response = ResultWithMessage.Create(RequestResponceType.AccessError) };
                                }
                            }
                            else if (model.ObjectClassID == IMSystem.Global.OBJ_WORKORDER)
                            {
                                if (!WorkOrder.AccessIsGranted(model.ObjectID.Value, user.User, true, dataSource))
                                {
                                    Logger.Error("SDApiController.EditManhoursWork userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID.Value);
                                    return new EditManhoursWorkOutModel() { Response = ResultWithMessage.Create(RequestResponceType.AccessError) };
                                }
                            }
                            else if (model.ObjectClassID == IMSystem.Global.OBJ_PROBLEM)
                            {
                                if (!ProblemBll.AccessIsGranted(model.ObjectID.Value, user.User, true, dataSource))
                                {
                                    Logger.Error("SDApiController.EditManhoursWork userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID.Value);
                                    return new EditManhoursWorkOutModel() { Response = ResultWithMessage.Create(RequestResponceType.AccessError) };
                                }
                            }
                            else if (model.ObjectClassID == IMSystem.Global.OBJ_RFC)
                            {
                                if (!RFC.AccessIsGranted(model.ObjectID.Value, user.User, true, dataSource))
                                {
                                    Logger.Error("SDApiController.EditManhoursWork userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID.Value);
                                    return new EditManhoursWorkOutModel() { Response = ResultWithMessage.Create(RequestResponceType.AccessError) };
                                }
                            }
                            else if (model.ObjectClassID == IMSystem.Global.OBJ_Project)
                            {
                                if (!Project.AccessIsGranted(model.ObjectID.Value, user.User, true, dataSource))
                                {
                                    Logger.Error("SDApiController.EditManhoursWork userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID.Value);
                                    return new EditManhoursWorkOutModel() { Response = ResultWithMessage.Create(RequestResponceType.AccessError) };
                                }
                            }
                            else throw new NotSupportedException("ObjectClassID not valid");
                        }
                        //
                        var result = ManhoursWork.Edit(model, dataSource, user.User);
                        //
                        if (model.ObjectClassID.HasValue && model.ObjectID.HasValue)
                            WorkflowWrapper.MakeOnSaveReaction(model.ObjectID.Value, model.ObjectClassID.Value, dataSource, user.User);
                        //
                        return new EditManhoursWorkOutModel() { Response = ResultWithMessage.Create(RequestResponceType.Success), ID = result.Item1, Number = result.Item2 };
                    }
                }
                catch (NotSupportedException e)
                {
                    Logger.Error(e, String.Format(@"EditManhoursWork not supported, model: '{0}'", model));
                    return new EditManhoursWorkOutModel() { Response = ResultWithMessage.Create(RequestResponceType.BadParamsError) };
                }
                catch (FieldConcurrencyException)
                {
                    Logger.Trace(String.Format(@"EditManhoursWork field concurrency, model: '{0}'", model));
                    return new EditManhoursWorkOutModel() { Response = ResultWithMessage.Create(RequestResponceType.ConcurrencyError) };
                }
                catch (ObjectConcurrencyException)
                {
                    Logger.Trace(String.Format(@"EditManhoursWork OBJECT concurrency, model: '{0}'", model));
                    return new EditManhoursWorkOutModel() { Response = ResultWithMessage.Create(RequestResponceType.ConcurrencyError) };
                }
                catch (ObjectDeletedException)
                {
                    Logger.Trace(String.Format(@"EditManhoursWork object deleted, model: '{0}'", model));
                    return new EditManhoursWorkOutModel() { Response = ResultWithMessage.Create(RequestResponceType.ObjectDeleted) };
                }
                catch (ArgumentValidationException e)
                {
                    Logger.Trace(String.Format(@"EditManhoursWork validation error, model: '{0}'", model));
                    return new EditManhoursWorkOutModel() { Response = ResultWithMessage.Create(RequestResponceType.ValidationError, ValidatorHelper.CreateErrorMessage(e)) };
                }
                catch (TimeSheetObjectInUseException)
                {
                    return new EditManhoursWorkOutModel() { Response = ResultWithMessage.Create(RequestResponceType.OperationError, Resources.TM_ObjectInUse) };
                }
                catch (Exception e)
                {
                    Logger.Error(e, String.Format(@"EditManhoursWork, model: '{0}'", model));
                    return new EditManhoursWorkOutModel() { Response = ResultWithMessage.Create(RequestResponceType.GlobalError) };
                }
            }
            else return new EditManhoursWorkOutModel() { Response = ResultWithMessage.Create(RequestResponceType.NullParamsError) };
        }
        #endregion

        #region method EditPriority
        [HttpPost]
        [Route("sdApi/EditPriority", Name = "EditPriority")]
        public RequestResponceType EditPriority(PriorityInfo model)
        {
            var user = base.CurrentUser;
            if (model != null && user != null)
            {
                try
                {
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        if (model.ObjectClassID == IMSystem.Global.OBJ_CALL)
                        {
                            if (!Call.AccessIsGranted(model.ObjectID, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.EditPriority userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID);
                                return RequestResponceType.AccessError;
                            }
                        }
                        else if (model.ObjectClassID == IMSystem.Global.OBJ_PROBLEM)
                        {
                            if (!ProblemBll.AccessIsGranted(model.ObjectID, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.EditPriority userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID);
                                return RequestResponceType.AccessError;
                            }
                        }
                        else if (model.ObjectClassID == IMSystem.Global.OBJ_RFC)
                        {
                            if (!RFC.AccessIsGranted(model.ObjectID, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.EditPriority userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID);
                                return RequestResponceType.AccessError;
                            }
                        }
                        else throw new NotSupportedException("ObjectClassID not valid");
                        //
                        Priority.Edit(model, dataSource, user.User);
                        WorkflowWrapper.MakeOnSaveReaction(model.ObjectID, model.ObjectClassID, dataSource, user.User);
                        return RequestResponceType.Success;
                    }
                }
                catch (NotSupportedException e)
                {
                    Logger.Error(e, String.Format(@"EditPriority not supported, model: '{0}'", model));
                    return RequestResponceType.BadParamsError;
                }
                catch (ArgumentValidationException e)
                {
                    Logger.Error(e, String.Format(@"EditPriority validation error, model: '{0}'", model));
                    return RequestResponceType.ValidationError;
                }
                catch (ObjectDeletedException e)
                {
                    Logger.Error(e, String.Format(@"EditPriority object deleted error. model: '{0}'", model));
                    return RequestResponceType.ObjectDeleted;
                }
                catch (Exception e)
                {
                    Logger.Error(e, String.Format(@"EditPriority, model: '{0}'", model));
                    return RequestResponceType.GlobalError;
                }
            }
            else return RequestResponceType.NullParamsError;
        }
        #endregion

        #region method EditWorkOrderPriority
        [HttpPost]
        [Route("sdApi/EditWorkOrderPriority", Name = "EditWorkOrderPriority")]
        public RequestResponceType EditWorkOrderPriority(WorkOrderPriorityInfo model)
        {
            var user = base.CurrentUser;
            if (model != null && user != null)
            {
                try
                {
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        if (model.ObjectClassID == IMSystem.Global.OBJ_WORKORDER)
                        {
                            if (!WorkOrder.AccessIsGranted(model.ObjectID, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.EditWorkOrderPriority userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID);
                                return RequestResponceType.AccessError;
                            }
                        }
                        else throw new NotSupportedException("ObjectClassID not valid");
                        //
                        Priority.Edit(model, dataSource, user.User);
                        WorkflowWrapper.MakeOnSaveReaction(model.ObjectID, model.ObjectClassID, dataSource, user.User);
                        return RequestResponceType.Success;
                    }
                }
                catch (NotSupportedException e)
                {
                    Logger.Error(e, String.Format(@"EditPriority not supported, model: '{0}'", model));
                    return RequestResponceType.BadParamsError;
                }
                catch (ArgumentValidationException e)
                {
                    Logger.Error(e, String.Format(@"EditPriority validation error, model: '{0}'", model));
                    return RequestResponceType.ValidationError;
                }
                catch (ObjectDeletedException e)
                {
                    Logger.Error(e, String.Format(@"EditPriority object deleted error. model: '{0}'", model));
                    return RequestResponceType.ObjectDeleted;
                }
                catch (Exception e)
                {
                    Logger.Error(e, String.Format(@"EditPriority, model: '{0}'", model));
                    return RequestResponceType.GlobalError;
                }
            }
            else return RequestResponceType.NullParamsError;
        }
        #endregion

        #region method EditOrCreateNegotiation        
        public sealed class EditOrCreateNegotiationOutModel
        {
            public Guid ID { get; set; }
            public ResultWithMessage Response { get; set; }
        }
        [HttpPost]
        [Route("sdApi/EditOrCreateNegotiation", Name = "EditOrCreateNegotiation")]
        [Obsolete("POST api/negotiations or PUT api/negotiations/{id}")]
        public EditOrCreateNegotiationOutModel EditOrCreateNegotiation(EditNegotiationInfo model)
        {
            var user = base.CurrentUser;
            if (model != null && user != null)
            {
                try
                {
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        if (model.ObjectClassID == IMSystem.Global.OBJ_CALL)
                        {
                            if (!Call.AccessIsGranted(model.ObjectID, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.EditOrCreateNegotiation userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID);
                                return new EditOrCreateNegotiationOutModel { Response = ResultWithMessage.Create(RequestResponceType.AccessError) };
                            }
                        }
                        else if (model.ObjectClassID == IMSystem.Global.OBJ_WORKORDER)
                        {
                            if (!WorkOrder.AccessIsGranted(model.ObjectID, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.EditOrCreateNegotiation userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID);
                                return new EditOrCreateNegotiationOutModel { Response = ResultWithMessage.Create(RequestResponceType.AccessError) };
                            }
                        }
                        else if (model.ObjectClassID == IMSystem.Global.OBJ_PROBLEM)
                        {
                            if (!ProblemBll.AccessIsGranted(model.ObjectID, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.EditOrCreateNegotiation userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID);
                                return new EditOrCreateNegotiationOutModel { Response = ResultWithMessage.Create(RequestResponceType.AccessError) };
                            }
                        }
                        else if (model.ObjectClassID == IMSystem.Global.OBJ_RFC)
                        {
                            if (!RFC.AccessIsGranted(model.ObjectID, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.EditOrCreateNegotiation userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID);
                                return new EditOrCreateNegotiationOutModel { Response = ResultWithMessage.Create(RequestResponceType.AccessError) };
                            }
                        }
                        else throw new NotSupportedException("ObjectClassID not valid");
                        //
                        var retval = Negotiation.Edit(model, user.User, dataSource);
                        WorkflowWrapper.MakeOnSaveReaction(model.ObjectID, model.ObjectClassID, dataSource, user.User);
                        return new EditOrCreateNegotiationOutModel { Response = ResultWithMessage.Create(RequestResponceType.Success), ID = retval };
                    }
                }
                catch (ObjectDeletedException e)
                {
                    Logger.Error(e, String.Format(@"EditOrCreateNegotiation Object was Deleted, model: '{0}'", model));
                    return new EditOrCreateNegotiationOutModel { Response = ResultWithMessage.Create(RequestResponceType.ObjectDeleted) };
                }
                catch (ArgumentNullException e)
                {
                    Logger.Error(e, String.Format(@"EditOrCreateNegotiation validation error, model: '{0}'", model));
                    return new EditOrCreateNegotiationOutModel { Response = ResultWithMessage.Create(RequestResponceType.ValidationError) };
                }
                catch (FieldConcurrencyException e)
                {
                    Logger.Error(e, String.Format(@"EditOrCreateNegotiation field concurrency, model: '{0}'", model));
                    return new EditOrCreateNegotiationOutModel { Response = ResultWithMessage.Create(RequestResponceType.ConcurrencyError) };
                }
                catch (ObjectConcurrencyException e)
                {
                    Logger.Error(e, String.Format(@"EditOrCreateNegotiation OBJECT concurrency, model: '{0}'", model));
                    return new EditOrCreateNegotiationOutModel { Response = ResultWithMessage.Create(RequestResponceType.ConcurrencyError) };
                }
                catch (ArgumentValidationException e)
                {
                    Logger.Error(e, String.Format(@"EditOrCreateNegotiation validation error, model: '{0}', message: '{1}'", model, e.Message));
                    return new EditOrCreateNegotiationOutModel { Response = ResultWithMessage.Create(RequestResponceType.ValidationError, ValidatorHelper.CreateErrorMessage(e)) };
                }
                catch (Exception e)
                {
                    Logger.Error(e, String.Format(@"EditOrCreateNegotiation, model: '{0}'", model));
                    return new EditOrCreateNegotiationOutModel { Response = ResultWithMessage.Create(RequestResponceType.GlobalError) };
                }
            }
            else return new EditOrCreateNegotiationOutModel { Response = ResultWithMessage.Create(RequestResponceType.NullParamsError) };
        }
        #endregion

        #region method NegotiationOperation
        public sealed class NegotiationOperationInputModel
        {
            public List<OperationNegotiationInfo> List { get; set; }
        }
        [HttpPost]
        [Route("sdApi/NegotiationOperation", Name = "NegotiationOperation")]
        public RequestResponceType NegotiationOperation(NegotiationOperationInputModel model)
        {
            var user = base.CurrentUser;
            if (user != null && model != null && model.List != null)
            {
                try
                {
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        foreach (var neg in model.List)
                        {
                            if (neg.ObjectClassID == IMSystem.Global.OBJ_CALL)
                            {
                                if (!Call.AccessIsGranted(neg.ObjectID, user.User, true, dataSource))
                                {
                                    Logger.Error("SDApiController.NegotiationOperation userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, neg.ObjectID);
                                    return RequestResponceType.AccessError;
                                }
                            }
                            else if (neg.ObjectClassID == IMSystem.Global.OBJ_WORKORDER)
                            {
                                if (!WorkOrder.AccessIsGranted(neg.ObjectID, user.User, true, dataSource))
                                {
                                    Logger.Error("SDApiController.NegotiationOperation userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, neg.ObjectID);
                                    return RequestResponceType.AccessError;
                                }
                            }
                            else if (neg.ObjectClassID == IMSystem.Global.OBJ_PROBLEM)
                            {
                                if (!ProblemBll.AccessIsGranted(neg.ObjectID, user.User, true, dataSource))
                                {
                                    Logger.Error("SDApiController.NegotiationOperation userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, neg.ObjectID);
                                    return RequestResponceType.AccessError;
                                }
                            }
                            else if (neg.ObjectClassID == IMSystem.Global.OBJ_RFC)
                            {
                                if (!RFC.AccessIsGranted(neg.ObjectID, user.User, true, dataSource))
                                {
                                    Logger.Error("SDApiController.NegotiationOperation userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, neg.ObjectID);
                                    return RequestResponceType.AccessError;
                                }
                            }
                            else throw new NotSupportedException("ObjectClassID not valid");
                            //
                            Negotiation.PerformOperation(neg, user.User, dataSource);
                            WorkflowWrapper.MakeOnSaveReaction(neg.ObjectID, neg.ObjectClassID, dataSource, user.User);
                        }
                        return RequestResponceType.Success;
                    }
                }
                catch (NotSupportedException e)
                {
                    Logger.Error(e, String.Format(@"NegotiationOperation not supported, model: '{0}'", model));
                    return RequestResponceType.BadParamsError;
                }
                catch (ObjectDeletedException e)
                {
                    Logger.Error(e, String.Format(@"NegotiationOperation object deleted, model: '{0}'", model));
                    return RequestResponceType.ObjectDeleted;
                }
                catch (FieldConcurrencyException e)
                {
                    Logger.Error(e, String.Format(@"NegotiationOperation field concurrency, model: '{0}'", model));
                    return RequestResponceType.ConcurrencyError;
                }
                catch (ObjectConcurrencyException e)
                {
                    Logger.Error(e, String.Format(@"NegotiationOperation OBJECT concurrency, model: '{0}'", model));
                    return RequestResponceType.ConcurrencyError;
                }
                catch (ArgumentValidationException e)
                {
                    Logger.Error(e, String.Format(@"NegotiationOperation validation error, model: '{0}'", model));
                    return RequestResponceType.ValidationError;
                }
                catch (Exception e)
                {
                    Logger.Error(e, String.Format(@"NegotiationOperation, model: '{0}'", model));
                    return RequestResponceType.GlobalError;
                }
            }
            else return RequestResponceType.NullParamsError;
        }
        #endregion

        #region method AddReference
        public sealed class ObjectReferenceModel
        {
            public int ReferenceClassID { get; set; }
            public List<Guid> ReferenceIDList { get; set; }
            public int ObjectClassID { get; set; }
            public Guid ObjectID { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/AddReference", Name = "AddReference")]
        public ResultWithMessage AddReference(ObjectReferenceModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultWithMessage.Create(RequestResponceType.NullParamsError);
            //
            Logger.Trace("SDApiController.AddReference userID={0}, userName={1}, ReferenceClassID={2}, ReferenceListCount={3}, ObjectClassID={4}, ObjectID={5}",
            user.Id, user.UserName, model.ReferenceClassID, model.ReferenceIDList.Count, model.ObjectClassID, model.ObjectID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.ObjectClassID == IMSystem.Global.OBJ_CALL)
                    {
                        if (model.ReferenceClassID == IMSystem.Global.OBJ_WORKORDER)
                            Call.AddWorkOrderReference(model.ObjectID, model.ReferenceIDList, dataSource, user.User);
                        else if (model.ReferenceClassID == IMSystem.Global.OBJ_PROBLEM)
                            Call.AddProblemReference(model.ObjectID, model.ReferenceIDList, dataSource, user.User);
                    }
                    else if (model.ObjectClassID == IMSystem.Global.OBJ_PROBLEM)
                    {
                        if (model.ReferenceClassID == IMSystem.Global.OBJ_WORKORDER)
                            ProblemBll.AddWorkOrderReference(model.ObjectID, model.ReferenceIDList, dataSource, user.User);
                        else if (model.ReferenceClassID == IMSystem.Global.OBJ_CALL)
                            ProblemBll.AddCallReference(model.ObjectID, model.ReferenceIDList, dataSource, user.User);
                    }
                    else if (model.ObjectClassID == IMSystem.Global.OBJ_RFC)
                    {
                        if (model.ReferenceClassID == IMSystem.Global.OBJ_WORKORDER)
                            RFC.AddWorkOrderReference(model.ObjectID, model.ReferenceIDList, dataSource, user.User);
                        else if (model.ReferenceClassID == IMSystem.Global.OBJ_CALL)
                            RFC.AddCallReference(model.ObjectID, model.ReferenceIDList, dataSource, user.User);
                    }
                    else
                        throw new NotSupportedException("model.ObjectClassID");
                    //
                    WorkflowWrapper.MakeOnSaveReaction(model.ObjectID, model.ObjectClassID, dataSource, user.User);
                    return ResultWithMessage.Create(RequestResponceType.Success);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при добавлении связей.");
                return ResultWithMessage.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method AddAssetReferences
        public sealed class AddAssetReferencesInputModel
        {
            public List<DTL.ObjectInfo> DependencyList { get; set; }
            public int ObjectClassID { get; set; }
            public Guid ObjectID { get; set; }
            public bool[] Parameters { get; set; }
        }
        public sealed class AddAssetReferencesOutModel
        {
            public IList<BLL.SD.DependencyObject> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/AddAssetReferences", Name = "AddAssetReferences")]
        public AddAssetReferencesOutModel AddAssetReferences(AddAssetReferencesInputModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new AddAssetReferencesOutModel() { List = null, Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace("SDApiController.AddReference userID={0}, userName={1}, ReferenceListCount={2}, ObjectClassID={3}, ObjectID={4}",
            user.Id, user.UserName, model.DependencyList.Count, model.ObjectClassID, model.ObjectID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    IList<BLL.SD.DependencyObject> retval = null;
                    //
                    if (model.ObjectClassID == IMSystem.Global.OBJ_CALL)
                    {
                        retval = Call.AddLinkedAssets(model.ObjectID, model.DependencyList, dataSource, user.User);
                    }
                    else if (model.ObjectClassID == IMSystem.Global.OBJ_PROBLEM)
                    {
                        retval = ProblemBll.AddLinkedAssets(model.ObjectID, model.DependencyList, dataSource, user.User);
                    }
                    else if (model.ObjectClassID == IMSystem.Global.OBJ_RFC)
                    {
                        if(model.Parameters.Length != 0)
                            retval = RFC.AddLinkedAssets(model.ObjectID, model.DependencyList,model.Parameters[0], dataSource, user.User);
                        else
                            retval = RFC.AddLinkedAssets(model.ObjectID, model.DependencyList, false, dataSource, user.User);
                    }
                    else if (model.ObjectClassID == IMSystem.Global.OBJ_WORKORDER)
                    {
                        retval = WorkOrder.AddLinkedAssets(model.ObjectID, model.DependencyList, dataSource, user.User);
                    }
                    else
                        throw new NotSupportedException("model.ObjectClassID");
                    //
                    WorkflowWrapper.MakeOnSaveReaction(model.ObjectID, model.ObjectClassID, dataSource, user.User);
                    return new AddAssetReferencesOutModel() { List = retval, Result = (RequestResponceType.Success) };
                }
            }
            catch (FieldConcurrencyException e)
            {
                Logger.Error(e, String.Format(@"AddAssetReferences concurency error. ObjClassID: '{2}'. Id: '{0}'. Assets: '{1}'",
                        model.ObjectID.ToString(),
                        String.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString())),
                        model.ObjectClassID));
                return new AddAssetReferencesOutModel() { List = null, Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectConcurrencyException e)
            {
                Logger.Error(e, String.Format(@"AddAssetReferences OBJECT concurency error. ObjClassID: '{2}'. Id: '{0}'. Assets: '{1}'",
                         model.ObjectID.ToString(),
                        String.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString())),
                        model.ObjectClassID));
                return new AddAssetReferencesOutModel() { List = null, Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectDeletedException e)
            {
                Logger.Error(e, String.Format(@"AddAssetReferences object deleted error. ObjClassID: '{2}'. Id: '{0}'. Assets: '{1}'",
                            model.ObjectID.ToString(),
                        String.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString())),
                        model.ObjectClassID));
                return new AddAssetReferencesOutModel() { List = null, Result = (RequestResponceType.ObjectDeleted) };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при добавлении связей.");
                return new AddAssetReferencesOutModel() { List = null, Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion

        #region method RemoveReference
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/RemoveReference", Name = "RemoveReference")]
        public ResultWithMessage RemoveReference(ObjectReferenceModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultWithMessage.Create(RequestResponceType.NullParamsError);
            //
            Logger.Trace("SDApiController.RemoveReference userID={0}, userName={1}, ReferenceClassID={2}, ReferenceListCount={3}, ObjectClassID={4}, ObjectID={5}",
            user.Id, user.UserName, model.ReferenceClassID, model.ReferenceIDList.Count, model.ObjectClassID, model.ObjectID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.ObjectClassID == IMSystem.Global.OBJ_CALL)
                    {
                        if (model.ReferenceClassID == IMSystem.Global.OBJ_WORKORDER)
                            Call.RemoveWorkOrderReference(model.ObjectID, model.ReferenceIDList, dataSource);
                        else if (model.ReferenceClassID == IMSystem.Global.OBJ_PROBLEM)
                            Call.RemoveProblemReference(model.ObjectID, model.ReferenceIDList, dataSource, user.User);
                    }
                    else if (model.ObjectClassID == IMSystem.Global.OBJ_PROBLEM)
                    {
                        if (model.ReferenceClassID == IMSystem.Global.OBJ_WORKORDER)
                            ProblemBll.RemoveWorkOrderReference(model.ObjectID, model.ReferenceIDList, dataSource);
                        else if (model.ReferenceClassID == IMSystem.Global.OBJ_CALL)
                            ProblemBll.RemoveCallReference(model.ObjectID, model.ReferenceIDList, dataSource, user.User);
                    }
                    else if (model.ObjectClassID == IMSystem.Global.OBJ_RFC)
                    {
                        if (model.ReferenceClassID == IMSystem.Global.OBJ_WORKORDER)
                            RFC.RemoveWorkOrderReference(model.ObjectID, model.ReferenceIDList, dataSource);
                    }
                    else
                        throw new NotSupportedException("model.ObjectClassID");
                    //
                    WorkflowWrapper.MakeOnSaveReaction(model.ObjectID, model.ObjectClassID, dataSource, user.User);
                    return ResultWithMessage.Create(RequestResponceType.Success);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при удалении связей.");
                return ResultWithMessage.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method RemoveReferenceObject
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/RemoveReferenceObject", Name = "RemoveReferenceObject")]
        public ResultWithMessage RemoveReferenceObject(ObjectReferenceModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultWithMessage.Create(RequestResponceType.NullParamsError);
            //
            Logger.Trace("SDApiController.RemoveReferenceObject userID={0}, userName={1}, ReferenceClassID={2}, ReferenceListCount={3}, ObjectClassID={4}, ObjectID={5}",
            user.Id, user.UserName, model.ReferenceClassID, model.ReferenceIDList.Count, model.ObjectClassID, model.ObjectID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.ObjectClassID == IMSystem.Global.OBJ_CALL)
                    {
                        if (model.ReferenceClassID == IMSystem.Global.OBJ_WORKORDER)
                            Call.RemoveWorkOrder(model.ObjectID, model.ReferenceIDList, dataSource, user.User);
                        else if (model.ReferenceClassID == IMSystem.Global.OBJ_PROBLEM)
                            Call.RemoveProblem(model.ObjectID, model.ReferenceIDList, dataSource, user.User);
                    }
                    else if (model.ObjectClassID == IMSystem.Global.OBJ_PROBLEM)
                    {
                        if (model.ReferenceClassID == IMSystem.Global.OBJ_WORKORDER)
                            ProblemBll.RemoveWorkOrder(model.ObjectID, model.ReferenceIDList, dataSource, user.User);
                        else if (model.ReferenceClassID == IMSystem.Global.OBJ_CALL)
                            ProblemBll.RemoveCall(model.ObjectID, model.ReferenceIDList, dataSource, user.User);
                    }
                    else if (model.ObjectClassID == IMSystem.Global.OBJ_RFC)
                    {
                        if (model.ReferenceClassID == IMSystem.Global.OBJ_WORKORDER)
                            RFC.RemoveWorkOrder(model.ObjectID, model.ReferenceIDList, dataSource, user.User);
                    }
                    else
                        throw new NotSupportedException("model.ObjectClassID");
                    //
                    WorkflowWrapper.MakeOnSaveReaction(model.ObjectID, model.ObjectClassID, dataSource, user.User);
                    return ResultWithMessage.Create(RequestResponceType.Success);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при удалении связей.");
                return ResultWithMessage.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        public sealed class ObjectAssetMixedReferenceModel
        {
            public List<DTL.ObjectInfo> ReferenceIDList { get; set; }
            public int ObjectClassID { get; set; }
            public Guid ObjectID { get; set; }
        }

        #region method RemoveReference
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/RemoveAssetMixedReference", Name = "RemoveAssetMixedReference")]
        public ResultWithMessage RemoveAssetMixedReference(ObjectAssetMixedReferenceModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultWithMessage.Create(RequestResponceType.NullParamsError);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.ObjectClassID == IMSystem.Global.OBJ_CALL)
                    {
                        Call.RemoveAssetReference(model.ObjectID, model.ReferenceIDList, dataSource, user.User);
                    }
                    else if (model.ObjectClassID == IMSystem.Global.OBJ_PROBLEM)
                    {
                        ProblemBll.RemoveAssetReference(model.ObjectID, model.ReferenceIDList, dataSource, user.User);
                    }
                    else if (model.ObjectClassID == IMSystem.Global.OBJ_RFC)
                    {
                        RFC.RemoveAssetReference(model.ObjectID, model.ReferenceIDList, dataSource, user.User);
                    }
                    else if (model.ObjectClassID == IMSystem.Global.OBJ_WORKORDER)
                    {
                        WorkOrder.RemoveAssetReference(model.ObjectID, model.ReferenceIDList, dataSource, user.User);
                    }
                    else
                        throw new NotSupportedException("model.ObjectClassID");
                    //
                    WorkflowWrapper.MakeOnSaveReaction(model.ObjectID, model.ObjectClassID, dataSource, user.User);
                    return ResultWithMessage.Create(RequestResponceType.Success);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при удалении связей.");
                return ResultWithMessage.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method RefreshSLA
        [HttpPost]
        [Route("sdApi/RefreshSLA", Name = "RefreshSLA")]
        public ResultWithMessage RefreshSLA([FromQuery]Guid callID,[FromQuery]bool customerChoice)
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultWithMessage.Create(RequestResponceType.NullParamsError, Resources.ErrorCaption);
            //
            Logger.Trace("SDApiController.RefreshSLA userID={0}, userName={1}", user.Id, user.UserName);
            //
            try
            {
                //if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_Call_PowerfullAccess))
                //{
                //    Logger.Trace("SDApiController.RefreshSLA userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, callID);
                //    return ResultWithMessage.Create(RequestResponceType.OperationError);
                //}
                //
                BLL.SD.Calls.Call.RefreshSLA(callID, user.User, customerChoice);
                return ResultWithMessage.Create(RequestResponceType.Success);
            }
            catch (InvalidOperationException ex)
            {
                return ResultWithMessage.Create(RequestResponceType.OperationError, ex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ResultWithMessage.Create(RequestResponceType.GlobalError, Resources.ErrorCaption);
            }
        }
        #endregion
    }
}