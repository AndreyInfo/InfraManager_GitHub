using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk;
using InfraManager.Core.Data;
using InfraManager.Core.Exceptions;
using InfraManager.Core.Logging;
using InfraManager.UI.Web.ModelBinding;
using InfraManager.Web.BLL;
using InfraManager.Web.BLL.Assets;
using InfraManager.Web.BLL.Catalogs;
using InfraManager.Web.BLL.Dashboards;
using InfraManager.Web.BLL.SD;
using InfraManager.Web.BLL.SD.Calls;
using InfraManager.Web.BLL.SD.Manhours;
using InfraManager.Web.BLL.SD.Negotiations;
using InfraManager.Web.BLL.SD.Problems;
using InfraManager.Web.BLL.SD.RFC;
using InfraManager.Web.BLL.SD.ServiceCatalogue;
using InfraManager.Web.BLL.SD.WorkOrders;
using InfraManager.Web.BLL.Tables;
using InfraManager.Web.BLL.TimeManagement;
using InfraManager.Web.DTL.Tables;
using InfraManager.Web.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.UI.Web.Services.Search;
using ProblemBll = InfraManager.Web.BLL.SD.Problems.Problem;
using Resources = InfraManager.ResourcesArea.Resources;
using System.Globalization;
using InfraManager.BLL.ServiceDesk.ChangeRequests.RFCGantt;
using InfraManager.BLL.Dashboards;
using InfraManager.BLL.Settings;

namespace InfraManager.Web.Controllers.SD
{
    public sealed partial class SDApiController : BaseApiController
    {
        private readonly IWebHostEnvironment _environment;

        private readonly IConfiguration _configuration;

        private readonly DashboardHelper _dashboardHelper;

        private readonly IPriorityBLL _priorityBll;
        private readonly IUrgencyBLL _urgencyBll;
        private readonly IInfluenceBLL _influenceBll;
        private readonly IConcordanceBLL _concordanceBll;
        private readonly ServiceDeskSearchService _serviceDeskSearchService;

        private readonly IKnowledgeBaseBLL _knowledgeBaseBLL;
        private readonly IRFCGanttBLL _irfcGanttBLL;
        private readonly IDashboardBLL _dashboardBll;
        private readonly IGetDashboard _getDashboard;
        private readonly IAppSettingsBLL _appSettings;

        public SDApiController(
            IWebHostEnvironment environment,
            IConfiguration configuration,
            DashboardHelper dashboardHelper,
            IPriorityBLL priorityBll,
            IUrgencyBLL urgencyBll,
            IInfluenceBLL influenceBll,
            IConcordanceBLL concordanceBll,
            ServiceDeskSearchService serviceDeskSearchService,
            IKnowledgeBaseBLL knowledgeBaseBLL,
            IRFCGanttBLL irfcGanttBLL,
            IDashboardBLL dashboardBll,
            IGetDashboard getDashboard,
            IAppSettingsBLL appSettings
            )
        {
            _environment = environment;
            _configuration = configuration;
            _dashboardHelper = dashboardHelper;
            _priorityBll = priorityBll;
            _urgencyBll = urgencyBll;
            _influenceBll = influenceBll;
            _concordanceBll = concordanceBll;
            _serviceDeskSearchService = serviceDeskSearchService;
            _knowledgeBaseBLL = knowledgeBaseBLL;
            _irfcGanttBLL = irfcGanttBLL;
            _dashboardBll = dashboardBll;
            _getDashboard = getDashboard;
            _appSettings = appSettings;
    }

        #region method IsPrivilegeAccess
        private bool IsPrivilegeAccess(Guid objectID, DataSource dataSource)
        {
            var user = base.CurrentUser;
            if (user == null)
                return false;
            //
            if (Negotiation.ExistsByObject(objectID, user.User.ID, dataSource))
                return true;
            //
            if (BLL.Global.IsInControl(objectID, user.User.ID, dataSource))
                return true;
            //
            return false;
        }
        #endregion

        #region method ClientHasNegotiations
        [HttpGet]
        [Obsolete("GET api/negotiations?objectID={objectID}&objectClassID={objectClassID}&userID={userID}")]
        [Route("sdApi/ClientHasNegotiations", Name = "ClientHasNegotiations")]
        public bool ClientHasNegotiations(Guid objectID)
        {
            using (var dataSource = DataSource.GetDataSource())
            {
                var user = base.CurrentUser;
                if (user == null)
                    return false;
                //
                if (Negotiation.ExistsByObject(objectID, user.User.ID, dataSource))
                    return true;
            }
            return false;
        }
        #endregion

        #region method GetPositionList
        [HttpGet]
        [Route("sdApi/GetPositionList", Name = "GetPositionList")]
        public DTL.SimpleEnum[] GetPositionList()
        {
            try
            {
                Logger.Trace("GetPositionList");
                //
                var list = ContactPerson.GetPositionList();
                var listRetval = new List<DTL.SimpleEnum>();
                //
                foreach (var x in list)
                {
                    listRetval.Add(new DTL.SimpleEnum()
                    {
                        ID = x.Key,
                        Name = x.Value.Item2
                    });
                }
                var retval = listRetval.ToArray();
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения перечня должностей");
                return null;
            }
        }
        #endregion

        #region method GetCallTypeListForClient
        [HttpGet]
        [Route("sdApi/GetCallTypeListForClient", Name = "GetCallTypeListForClient")]
        public DTL.SD.Calls.CallType[] GetCallTypeListForClient()
        {
            try
            {
                var callTypeList = CallType.GetCallTypeListForRegistration();
                //
                var retval = callTypeList.
                    Select(x => new DTL.SD.Calls.CallType
                    {
                        ID = x.ID,
                        Name = x.Name,
                        IsRFC = x.IsRFC,
                        IsIncident = x.IsIncident,
                        ImageSource = x.ImageSource
                    }).
                    OrderBy(x => x.Name).
                    ToArray();
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения типов заявок для клиента");
                return null;
            }
        }
        #endregion

        #region method GetCallTypeList
        [HttpGet]
        [Route("sdApi/GetCallTypeList", Name = "GetCallTypeList")]
        [Obsolete("Use api/calltypes instead")]
        public DTL.SD.Calls.CallType[] GetCallTypeList()
        {
            var user = base.CurrentUser;
            if (!user.User.HasRoles)
            {
                Logger.Trace("SDApiController.GetCallTypeList canceled (access denied)");
                return null;
            }
            try
            {
                var callTypeList = CallType.GetList();
                //
                var retval = callTypeList.
                    Select(x => x.DTL).
                    OrderBy(x => x.Name).
                    ToArray();
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения типов заявок для инженера");
                return null;
            }
        }
        #endregion

        #region method GetCallType
        [HttpGet]
        [Route("sdApi/GetCallType", Name = "GetCallType")]
        public DTL.SD.Calls.CallType GetCallType(Guid callTypeID)
        {
            var user = base.CurrentUser;
            if (!user.User.HasRoles)
            {
                Logger.Trace("SDApiController.GetCallType canceled (access denied)");
                return null;
            }
            try
            {
                var callType = CallType.Get(callTypeID);
                var retval = callType.DTL;
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения типа заявки");
                return null;
            }
        }
        #endregion

        #region method GetReceiptTypeList
        [HttpGet]
        [Route("sdApi/GetReceiptTypeList", Name = "GetReceiptTypeList")]
        public DTL.SimpleEnum[] GetReceiptTypeList()
        {
            try
            {
                Logger.Trace("SDApiController.GetReceiptTypeList");
                var list = BLL.SD.Calls.Call.GetReceiptTypeList(HttpContext?.GetCurrentCulture() ?? CultureInfo.CurrentCulture.Name);
                //
                var retval = list.
                    OrderBy(x => x.Name).
                    ToArray();
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения перечня доступных способов получения");
                return null;
            }
        }
        #endregion

        #region method GetRFCResultList
        [HttpGet]
        [Route("sdApi/GetRFCResultList", Name = "GetRFCResultList")]
        [Obsolete("Use api/rfcResults instead")]
        public DTL.SimpleDictionary[] GetRFCResultList()
        {
            try
            {
                var urgencyList = SimpleDictionary.GetRFCResultList();
                //
                var retval = urgencyList.
                    Select(x => x.DTL).
                    OrderBy(x => x.Name).
                    ToArray();
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка результатов завершений услуг");
                return null;
            }
        }
        #endregion

        #region method GetCallInfoList
        [HttpGet]
        [Route("sdApi/GetCallInfoList", Name = "GetCallInfoList")]
        public DTL.SD.Calls.CallInfo[] GetCallInfoList(Guid callTypeID, Guid? userID)
        {
            return Array.Empty<DTL.SD.Calls.CallInfo>();

            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetCallInfoList userID={0}, userName={1}, callTypeID={2}, userID(override)={3}", user.Id, user.UserName, callTypeID, userID.HasValue ? userID.Value.ToString() : "not set");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var clientID = userID.HasValue ? userID.Value : user.User.ID;
                    var calls = CallInfo.GetList(callTypeID, clientID, dataSource);//top 50
                    var availableIDs = ServiceCatalogueHelper.GetAvailableObjectsByUserSLA(clientID, dataSource);
                    //
                    var retval = calls.
                        Where(x =>
                                x.ServiceID.HasValue && availableIDs.Contains(x.ServiceID.Value) ||
                                x.ServiceItemID.HasValue && availableIDs.Contains(x.ServiceItemID.Value) ||
                                x.ServiceAttendanceID.HasValue && availableIDs.Contains(x.ServiceAttendanceID.Value)
                             ).
                        Select(x => x.DTL).
                        OrderByDescending(x => x.Number).
                        Take(10).
                        ToArray();
                    //
                    return retval;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения информационного списка заявок");
                return null;
            }
        }
        #endregion

        #region method GetCallInfo
        public sealed class GetCallInfoOutModel
        {
            public CallReferenceLegacyResponse Call { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [Route("sdApi/GetCallInfo", Name = "GetCallInfo")]
        public GetCallInfoOutModel GetCallInfo(Guid ID)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetCallInfo userID={0}, userName={1}, callID={2}", user.Id, user.UserName, ID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var call = CallReferenceLegacyResponse.Get(ID, dataSource);
                    //
                    return new GetCallInfoOutModel() { Call = call, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения заявки");
                return new GetCallInfoOutModel() { Call = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetShortCallInfoList
        [HttpGet]
        [Route("sdApi/GetShortCallInfoList", Name = "GetShortCallInfoList")]
        public ShortCallInfo[] GetShortCallInfoList(Guid? userID)
        {
            return Array.Empty<ShortCallInfo>();

            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetShortCallInfoList userID={0}, userName={1}, userID(override)={2}", user.Id, user.UserName, userID.HasValue ? userID.Value.ToString() : "not set");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var clientID = userID.HasValue ? userID.Value : user.User.ID;
                    var calls = ShortCallInfo.GetList(clientID, dataSource);//top 500
                    var availableIDs = ServiceCatalogueHelper.GetAvailableObjectsByUserSLA(clientID, dataSource);
                    //
                    var retval = calls.
                        Where(x =>
                                x.ServiceID.HasValue && availableIDs.Contains(x.ServiceID.Value) ||
                                x.ServiceItemID.HasValue && availableIDs.Contains(x.ServiceItemID.Value) ||
                                x.ServiceAttendanceID.HasValue && availableIDs.Contains(x.ServiceAttendanceID.Value)
                             ).
                        OrderByDescending(x => x.Number).
                        ToArray();
                    //
                    return retval;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения простого информационного списка заявок");
                return null;
            }
        }
        #endregion

        #region method GetCallRegistrationAvailable
        [HttpGet]
        [Route("sdApi/getCallRegistrationAvailable", Name = "GetCallRegistrationAvailable")]
        public CallRegistrationResponse GetCallRegistrationAvailable(Guid callTypeID, Guid? userID)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new CallRegistrationResponse(RequestResponceType.NullParamsError, Resources.ErrorCaption, null);
            //
            Logger.Trace("SDApiController.GetCallRegistrationAvailable userID={0}, userName={1}, callType={2}, userID(override)={3}", user.Id, user.UserName, callTypeID, userID.HasValue ? userID.Value.ToString() : "not set");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var isRFC = CallType.IsRFCCallType(callTypeID, dataSource);
                    //
                    int availableClassID = isRFC ? IMSystem.Global.OBJ_ServiceAttendance : IMSystem.Global.OBJ_ServiceItem;
                    //
                    var availableIDs = ServiceCatalogueHelper.GetAvailableObjectsByUserSLA(userID.HasValue ? userID.Value : user.User.ID, dataSource);
                    var serviceItemAttendanceList = ServiceItemAttendance.GetList(userID.HasValue ? userID.Value : user.User.ID, dataSource);
                    var accessList = serviceItemAttendanceList.
                        Where(x => x.ClassID == availableClassID).
                        Where(x => availableIDs.Contains(x.ServiceID) || availableIDs.Contains(x.ID)).
                        ToArray();
                    //
                    if (accessList.Length == 0)
                    {
                        if (isRFC)
                            return new CallRegistrationResponse(RequestResponceType.Success, Resources.SLANotFoundForRFC, null);//AccessError in old web, check it on registration
                        else
                            return new CallRegistrationResponse(RequestResponceType.Success, Resources.SLANotFound, null);
                    }
                }
                return new CallRegistrationResponse(RequestResponceType.Success, string.Empty, null);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new CallRegistrationResponse(RequestResponceType.GlobalError, Resources.ErrorCaption, null);
            }
        }
        #endregion

        #region method RegisterCallEngineer
        [HttpPost]
        [Route("sdApi/registerCallEngineer", Name = "RegisterCallEngineer")]
        public CallRegistrationResponse RegisterCallEngineer(DTL.SD.Calls.CallRegistrationEngineerInfo info)
        {
            var user = base.CurrentUser;
            if (info == null || user == null)
                return new CallRegistrationResponse(RequestResponceType.NullParamsError, Resources.ErrorCaption, null);
            //
            Logger.Trace("SDApiController.RegisterCallEngineer userID={0}, userName={1}", user.Id, user.UserName);
            if (!user.User.HasRoles || !user.User.OperationIsGranted(IMSystem.Global.OPERATION_Call_Add))
                return new CallRegistrationResponse(RequestResponceType.AccessError, Resources.AccessError, null);
            //
            return RegisterCall(info);
        }
        #endregion

        #region method RegisterCall
        [HttpPost]
        [Route("sdApi/registerCall", Name = "RegisterCall")]
        public CallRegistrationResponse RegisterCall(DTL.SD.Calls.CallRegistrationInfo info)
        {
            var user = base.CurrentUser;
            if (info == null || user == null)
                return new CallRegistrationResponse(RequestResponceType.NullParamsError, Resources.ErrorCaption, null);
            //
            Logger.Trace("SDApiController.RegisterCall userID={0}, userName={1}", user.Id, user.UserName);
            //
            if (!info.CallTypeID.HasValue)
                return new CallRegistrationResponse(RequestResponceType.BadParamsError, Resources.ErrorCaption, null);
            //
            try
            {
                List<object> documentList;
                List<string> paths;
                var api = new FileApiController(_environment);
                if (!api.GetDocumentFromFiles(info.Files, out documentList, out paths, user))
                    return new CallRegistrationResponse(RequestResponceType.BadParamsError, Resources.UploadedFileNotFoundAtServerSide, null);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var isRFC = CallType.IsRFCCallType(info.CallTypeID.Value, dataSource);
                    //                    
                    if (info.ServiceAttendanceID.HasValue || info.ServiceItemID.HasValue)//не затрудняюсь выбрать
                    {
                        if (info.ServiceItemID.HasValue && isRFC)
                            return new CallRegistrationResponse(RequestResponceType.BadParamsError, Resources.RegisterCallForIncidentForThisTypeUnavailable, null);
                        if (info.ServiceAttendanceID.HasValue && !isRFC)
                            return new CallRegistrationResponse(RequestResponceType.BadParamsError, Resources.RegisterCallForAttendanceForThisTypeUnavailable, null);
                        //
                        Guid id = info.ServiceItemID.HasValue ? info.ServiceItemID.Value : info.ServiceAttendanceID.Value;
                        var serviceInfo = ServiceItemAttendanceInfo.Get(id, dataSource);
                        Guid? serviceID = serviceInfo == null ? null : (Guid?)serviceInfo.ServiceID;
                        //
                        var clientID = info.UserID.HasValue ? info.UserID.Value : user.User.ID;
                        var availableIDs = ServiceCatalogueHelper.GetAvailableObjectsByUserSLA(clientID, dataSource);
                        if (!availableIDs.Any(x => x == id || x == serviceID))
                        {
                            if (isRFC)
                            {
                                if (info is DTL.SD.Calls.CallRegistrationEngineerInfo)
                                    return new CallRegistrationResponse(RequestResponceType.AccessError, Resources.SLANotFoundForRFC_Engineer, null);
                                else
                                    return new CallRegistrationResponse(RequestResponceType.AccessError, Resources.SLANotFoundForRFC, null);
                            }
                            else if (info is DTL.SD.Calls.CallRegistrationEngineerInfo)
                                return new CallRegistrationResponse(RequestResponceType.AccessError, Resources.SLANotFound_Engineer, null);
                        }
                    }
                    else if (isRFC == true)
                    {//услуга
                        var clientID = info.UserID.HasValue ? info.UserID.Value : user.User.ID;
                        var availableIDs = ServiceCatalogueHelper.GetAvailableObjectsByUserSLA(clientID, dataSource);
                        if (availableIDs.Count == 0)
                        {
                            if (info is DTL.SD.Calls.CallRegistrationEngineerInfo)
                                return new CallRegistrationResponse(RequestResponceType.AccessError, Resources.SLANotFoundForRFC_Engineer, null);
                            else
                                return new CallRegistrationResponse(RequestResponceType.AccessError, Resources.SLANotFoundForRFC, null);
                        }
                    }
                    //
                    //
                    dataSource.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);
                    Core.BaseObject bo;
                    if (info.KBArticleID.HasValue)
                        bo = Call.RegisterCallByKBArticle(info, documentList, user.User, info.KBArticleID.Value, dataSource);
                    else
                        bo = Call.RegisterCall(info, documentList, user.User, dataSource);
                    dataSource.CommitTransaction();
                    //                    
                    foreach (var filePath in paths)
                        System.IO.File.Delete(filePath);
                    //
                    var call = Call.Get(bo.ID, null, dataSource);
                    return new CallRegistrationResponse(RequestResponceType.Success, string.Format(Resources.CallRegisteredMessage, call.Number), bo.ID);
                }
            }
            catch (DemoVersionException)
            {
                return new CallRegistrationResponse(RequestResponceType.AccessError, Resources.DemoVersionException, null);
            }
            catch (OutOfMemoryException ex)
            {
                Logger.Warning(ex);
                return new CallRegistrationResponse(RequestResponceType.GlobalError, Resources.OutOfMemoryException, null);
            }
            catch (ArgumentValidationException ex)
            {
                return new CallRegistrationResponse(RequestResponceType.BadParamsError, string.Format(Resources.ArgumentValidationException, ex.Message), null);
            }
            catch (ObjectInUseException)
            {
                return new CallRegistrationResponse(RequestResponceType.ConcurrencyError, Resources.ConcurrencyError, null);
            }
            catch (ObjectConstraintException)
            {
                return new CallRegistrationResponse(RequestResponceType.BadParamsError, Resources.SaveError, null);
            }
            catch (ObjectDeletedException)
            {
                return new CallRegistrationResponse(RequestResponceType.ObjectDeleted, Resources.SaveError, null);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new CallRegistrationResponse(RequestResponceType.GlobalError, Resources.ErrorCaption, null);
            }
        }
        #endregion

        #region helper class CallRegistrationResponse
        public sealed class CallRegistrationResponse
        {
            public CallRegistrationResponse(RequestResponceType type, string message, Guid? callID)
            {
                this.Type = type;
                this.Message = message;
                this.CallID = callID;
            }

            public RequestResponceType Type { get; private set; }
            public string Message { get; private set; }
            public Guid? CallID { get; private set; }
        }
        #endregion

        #region method GetCall
        public sealed class GetCallOutModel
        {
            public Call Call { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetCall", Name = "GetCall")]
        public GetCallOutModel GetCall([FromQuery] Guid id)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetCallOutModel() { Call = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetCall userID={0}, userName={1}, callID={2}", user.Id, user.UserName, id);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = Call.Get(id, user.User.ID, dataSource);
                    if (!IsPrivilegeAccess(id, dataSource))
                    {
                        if (!retval.AccessIsGranted(user.User, dataSource))
                        {
                            Logger.Trace("SDApiController.GetCall userID={0}, userName={1}, callID={2} failed (access denied)", user.Id, user.UserName, id);
                            return new GetCallOutModel() { Call = null, Result = RequestResponceType.AccessError };
                        }
                        //
                        if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_CALL_Properties) && !user.IsClientView(dataSource))
                        {
                            Logger.Trace("SDApiController.GetCall userID={0}, userName={1}, callID={2} failed (operation denied)", user.Id, user.UserName, id);
                            return new GetCallOutModel() { Call = null, Result = RequestResponceType.OperationError };
                        }
                    }
                    //
                    retval.InitializeCallType(dataSource);//для того, чтобы понимать какое поле показывать - результат запроса на услугу / результат завершения инцидента
                    retval.InitializeUserFieldNames(dataSource);
                    retval.CheckHaveNegotiationsWithCurrentUser(user.User.ID, dataSource);
                    return new GetCallOutModel() { Call = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException)
            {
                Logger.Trace("GetCall is NULL, id: '{0}'", id);
                return new GetCallOutModel() { Call = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetCall, id: '{0}'", id);
                return new GetCallOutModel() { Call = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetCallID
        [HttpGet]
        [Route("sdApi/GetCallID", Name = "GetCallID")]
        public Guid? GetCallID([FromQuery] int number)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetCallID userID={0}, userName={1}, number={2}", user.Id, user.UserName, number);
            try
            {
                var retval = Call.GetIDByNumber(number);
                return retval;
            }
            catch (Exception e)
            {
                Logger.Error(e, "GetCallID error");
                return null;
            }
        }
        #endregion

        #region method GetCallReference
        public sealed class GetCallReferenceIncomingModel
        {
            public Guid EntityID { get; set; }
            public int EntityClassId { get; set; }
            public Guid CallID { get; set; }
            public bool ReferenceExists { get; set; }
        }
        public sealed class GetCallReferenceOutModel
        {
            public CallReferenceLegacyResponse Elem { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetCallReference", Name = "GetCallReference")]
        public GetCallReferenceOutModel GetCallReference([FromQuery] GetCallReferenceIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetCallReferenceOutModel() { Elem = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetCallReference userID={0}, userName={1}, objID={2}, objClassId={3}, CallID={4}", user.Id, user.UserName, model.EntityID, model.EntityClassId, model.CallID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.EntityClassId == IMSystem.Global.OBJ_PROBLEM)
                    {
                        if (!ProblemBll.AccessIsGranted(model.EntityID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetCallReference userID={0}, userName={1}, objID={2}, objClassId={3}, CallID={4} failed (access denied)", user.Id, user.UserName, model.EntityID, model.EntityClassId, model.CallID);
                            return new GetCallReferenceOutModel() { Elem = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_RFC)
                    {
                        if (!RFC.AccessIsGranted(model.EntityID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetCallReference userID={0}, userName={1}, objID={2}, objClassId={3}, CallID={4} failed (access denied)", user.Id, user.UserName, model.EntityID, model.EntityClassId, model.CallID);
                            return new GetCallReferenceOutModel() { Elem = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_WORKORDER)
                    {
                        if (!WorkOrder.AccessIsGranted(model.EntityID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetCallReference userID={0}, userName={1}, objID={2}, objClassId={3}, CallID={4} failed (access denied)", user.Id, user.UserName, model.EntityID, model.EntityClassId, model.CallID);
                            return new GetCallReferenceOutModel() { Elem = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else
                        throw new NotSupportedException("entityClassID not valid");
                    //
                    var retval = CallReferenceLegacyResponse.Get(model.EntityID, model.EntityClassId, model.CallID, dataSource);
                    return new GetCallReferenceOutModel() { Elem = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                RequestResponceType result;
                if (model.ReferenceExists)
                {
                    result = RequestResponceType.ObjectDeleted;
                    Logger.Error(ex, "GetCallReference is NULL, model: '{0}'", model);
                }
                else
                    result = RequestResponceType.Success;
                //
                return new GetCallReferenceOutModel() { Elem = null, Result = result };
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetCallReference not supported, model: '{0}'", model));
                return new GetCallReferenceOutModel() { Elem = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetCallReference, model: {0}.", model);
                return new GetCallReferenceOutModel() { Elem = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetCallReferenceList
        public sealed class GetCallReferenceListIncomingModel
        {
            public Guid ID { get; set; }
            public int EntityClassId { get; set; }
        }
        public sealed class GetCallReferenceListOutModel
        {
            public IList<CallReferenceLegacyResponse> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetCallReferenceList", Name = "GetCallReferenceList")]
        public GetCallReferenceListOutModel GetCallReferenceList([FromQuery] GetCallReferenceListIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetCallReferenceListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetCallReferenceList userID={0}, userName={1}, objID={2}, objClassId={3}", user.Id, user.UserName, model.ID, model.EntityClassId);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.EntityClassId == IMSystem.Global.OBJ_PROBLEM)
                    {
                        if (!ProblemBll.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetCallReferenceList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetCallReferenceListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_RFC)
                    {
                        if (!RFC.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetCallReferenceList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetCallReferenceListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_WORKORDER)
                    {
                        if (!WorkOrder.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetCallReferenceList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetCallReferenceListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId != IMSystem.Global.OBJ_USER)
                        throw new NotSupportedException("entityClassID not valid");
                    //
                    var retval = CallReferenceLegacyResponse.GetList(model.ID, model.EntityClassId, dataSource);
                    return new GetCallReferenceListOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetCallReferenceList not supported, model: '{0}'", model));
                return new GetCallReferenceListOutModel() { List = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetCallReferenceList, model: {0}.", model);
                return new GetCallReferenceListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/GetCallListUserSearch", Name = "GetCallListUserSearch")]
        public ResultData<List<BaseForTable>> GetCallListUserSearch([FromForm] TableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.CallUsersList);
        }

        #region method GetWorkOrderTypeList
        [HttpGet]
        [Route("sdApi/GetWorkOrderTypeList", Name = "GetWorkOrderTypeList")]
        public DTL.SD.WorkOrders.WorkOrderType[] GetWorkOrderTypeList()
        {
            var user = base.CurrentUser;
            if (!user.User.HasRoles)
            {
                Logger.Trace("SDApiController.GetWorkOrderTypeList canceled (access denied)");
                return null;
            }
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var workOrderTypeList = WorkOrderType.GetList(user.User.ID, dataSource);
                    //
                    var retval = workOrderTypeList.
                        Select(x => x.DTL).
                        OrderBy(x => x.Name).
                        ToArray();
                    //
                    return retval;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения типов заданий для инженера");
                return null;
            }
        }
        #endregion

        #region method GetWorkOrderTypeTree
        [HttpGet]
        [Route("sdApi/GetWorkOrderTypeTree", Name = "GetWorkOrderTypeTree")]
        public DTL.SD.WorkOrders.WorkOrderTypeTreeNode[] GetWorkOrderTypeTree()
        {
            var user = base.CurrentUser;
            if (!user.User.HasRoles)
            {
                Logger.Trace("SDApiController.GetWorkOrderTypeTree canceled (access denied)");
                return null;
            }
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var workOrderTypeTree = WorkOrderType.GetTree(user.User.ID, dataSource);
                    return workOrderTypeTree.ToArray();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения дерева видов деястельности и типов заданий");
                return null;
            }
        }
        #endregion

        #region method IsWorkOrderHasDetailBudget
        [HttpGet]
        [Route("sdApi/IsWorkOrderHasDetailBudget", Name = "IsWorkOrderHasDetailBudget")]
        public bool? IsWorkOrderHasDetailBudget(Guid workOrderID)
        {
            var user = base.CurrentUser;
            if (!user.User.HasRoles)
            {
                Logger.Trace("SDApiController.IsWorkOrderHasDetailBudget canceled (access denied)");
                return null;
            }
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                    return WorkOrder.IsWorkOrderHasDetailBudget(workOrderID, dataSource);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }
        #endregion


        #region method GetWorkOrderInfoList
        [HttpGet]
        [Route("sdApi/GetWorkOrderInfoList", Name = "GetWorkOrderInfoList")]
        public DTL.SD.WorkOrders.WorkOrderInfo[] GetWorkOrderInfoList(Guid workOrderTypeID, Guid? userID)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetWorkOrderInfoList userID={0}, userName={1}, workOrderTypeID={2}, userID(override)={3}", user.Id, user.UserName, workOrderTypeID, userID.HasValue ? userID.Value.ToString() : "not set");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var initiatorID = userID.HasValue ? userID.Value : user.User.ID;
                    var workOrders = WorkOrderInfo.GetList(workOrderTypeID, initiatorID, dataSource);//top 10
                    //
                    var retval = workOrders.
                        Select(x => x.DTL).
                        OrderByDescending(x => x.Number).
                        Take(10).
                        ToArray();
                    //
                    return retval;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения информационного списка заданий");
                return null;
            }
        }
        #endregion

        #region method GetWorkOrderTemplateList
        [HttpGet]
        [Route("sdApi/getWorkOrderTemplateList", Name = "GetWorkOrderTemplateList")]
        [Obsolete("Use api/workordertemplates/")]
        public DTL.SD.WorkOrders.WorkOrderTemplate[] GetWorkOrderTemplateList(Guid? workOrderTypeID)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetWorkOrderTemplateList userID={0}, userName={1}, workOrderTypeID={2}", user.Id, user.UserName, workOrderTypeID.HasValue ? workOrderTypeID.Value.ToString() : "not set");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var templates = WorkOrderTemplate.GetList(workOrderTypeID, user.User.ID, dataSource);//top 50
                    //
                    var retval = templates.
                        Select(x => x.DTL).
                        OrderByDescending(x => x.Name).
                        ToArray();
                    //
                    return retval;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения информационного списка шаблонов заданий");
                return null;
            }
        }
        #endregion

        #region method GetWorkOrderTemplate
        [HttpGet]
        [Route("sdApi/getWorkOrderTemplate", Name = "GetWorkOrderTemplate")]
        [Obsolete("Use api/workordertemplates/{id}")]
        public DTL.SD.WorkOrders.WorkOrderTemplate GetWorkOrderTemplate(Guid workOrderTemplateID)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetWorkOrderTemplate userID={0}, userName={1}, workOrderTemplateID={2}", user.Id, user.UserName, workOrderTemplateID.ToString());
            try
            {
                var template = WorkOrderTemplate.GetWithCalculatedExecutor(workOrderTemplateID);
                //
                return template.DTL;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения шаблона задания с вычисленным исполнителем");
                return null;
            }
        }
        #endregion

        #region method GetWorkOrder
        public sealed class GetWorkOrderOutModel
        {
            public WorkOrder WorkOrder { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetWorkOrder", Name = "GetWorkOrder")]
        [Obsolete("GET api/workorders/{id}")]
        public GetWorkOrderOutModel GetWorkOrder([FromQuery] Guid id)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetWorkOrderOutModel() { WorkOrder = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetWorkOrder userID={0}, userName={1}, ID={2}", user.Id, user.UserName, id);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = WorkOrder.Get(id, user.User.ID, dataSource);
                    if (!IsPrivilegeAccess(id, dataSource))
                    {
                        if (!retval.AccessIsGranted(user.User, dataSource))
                        {
                            Logger.Trace("SDApiController.GetWorkOrder userID={0}, userName={1}, ID={2} failed (access denied)", user.Id, user.UserName, id);
                            return new GetWorkOrderOutModel() { WorkOrder = null, Result = RequestResponceType.AccessError };
                        }
                        //
                        if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_WorkOrder_Properties) && !user.IsClientView(dataSource))
                        {
                            Logger.Trace("SDApiController.GetWorkOrder userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, id);
                            return new GetWorkOrderOutModel() { WorkOrder = null, Result = RequestResponceType.OperationError };
                        }
                    }
                    //
                    retval.InitializeUserFieldNames(dataSource);
                    retval.InitializeActivesRequestSpecification(dataSource);
                    retval.InitializePurchaseSpecification(dataSource);
                    retval.InitializeInventory(dataSource);
                    return new GetWorkOrderOutModel() { WorkOrder = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetWorkOrder is NULL, id: '{0}'", id);
                return new GetWorkOrderOutModel() { WorkOrder = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetWorkOrder, id: '{0}'", id);
                return new GetWorkOrderOutModel() { WorkOrder = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetWorkOrderID
        [HttpGet]
        [Route("sdApi/GetWorkOrderID", Name = "GetWorkOrderID")]
        public Guid? GetWorkOrderID([FromQuery] int number)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetWorkOrderID userID={0}, userName={1}, number={2}", user.Id, user.UserName, number);
            try
            {
                var retval = WorkOrder.GetIDByNumber(number);
                return retval;
            }
            catch (Exception e)
            {
                Logger.Error(e, "GetWorkOrderID error");
                return null;
            }
        }
        #endregion

        #region method RegisterWorkOrder
        [HttpPost]
        [Route("sdApi/registerWorkOrder", Name = "RegisterWorkOrder")]
        [Obsolete("POST to api/workorders")]
        public WorkOrderRegistrationResponse RegisterWorkOrder(DTL.SD.WorkOrders.WorkOrderRegistrationInfo info)
        {
            var user = base.CurrentUser;
            if (info == null || user == null)
                return new WorkOrderRegistrationResponse(RequestResponceType.NullParamsError, Resources.ErrorCaption, null);
            //
            Logger.Trace("SDApiController.RegisterWorkOrder userID={0}, userName={1}", user.Id, user.UserName);
            if (!user.User.HasRoles || !user.User.OperationIsGranted(IMSystem.Global.OPERATION_WorkOrder_Add))
                return new WorkOrderRegistrationResponse(RequestResponceType.AccessError, Resources.AccessError, null);
            //
            if (!info.WorkOrderTypeID.HasValue)
                return new WorkOrderRegistrationResponse(RequestResponceType.BadParamsError, Resources.ErrorCaption, null);
            //
            try
            {
                List<object> documentList;
                List<string> paths;
                var api = new FileApiController(_environment);
                if (!api.GetDocumentFromFiles(info.Files, out documentList, out paths, user))
                    return new WorkOrderRegistrationResponse(RequestResponceType.BadParamsError, Resources.UploadedFileNotFoundAtServerSide, null);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);
                    Core.BaseObject bo;
                    if (info.WorkOrderTemplateID.HasValue)
                        bo = WorkOrder.RegisterWorkOrderByTemplate(info, documentList, user.User, info.WorkOrderTemplateID.Value, dataSource);
                    else
                        bo = WorkOrder.RegisterWorkOrder(info, documentList, user.User, dataSource);
                    dataSource.CommitTransaction();
                    //                    
                    foreach (var filePath in paths)
                        System.IO.File.Delete(filePath);
                    //
                    var workOrder = WorkOrder.Get(bo.ID, null, dataSource);
                    return new WorkOrderRegistrationResponse(RequestResponceType.Success, string.Format(Resources.WorkOrderRegisteredMessage, workOrder.Number), bo.ID);
                }
            }
            catch (DemoVersionException)
            {
                return new WorkOrderRegistrationResponse(RequestResponceType.AccessError, Resources.DemoVersionException, null);
            }
            catch (OutOfMemoryException ex)
            {
                Logger.Warning(ex);
                return new WorkOrderRegistrationResponse(RequestResponceType.GlobalError, Resources.OutOfMemoryException, null);
            }
            catch (ArgumentValidationException ex)
            {
                return new WorkOrderRegistrationResponse(RequestResponceType.BadParamsError, string.Format(Resources.ArgumentValidationException, ex.Message), null);
            }
            catch (ObjectInUseException)
            {
                return new WorkOrderRegistrationResponse(RequestResponceType.ConcurrencyError, Resources.ConcurrencyError, null);
            }
            catch (ObjectConstraintException)
            {
                return new WorkOrderRegistrationResponse(RequestResponceType.BadParamsError, Resources.SaveError, null);
            }
            catch (ObjectDeletedException)
            {
                return new WorkOrderRegistrationResponse(RequestResponceType.ObjectDeleted, Resources.SaveError, null);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new WorkOrderRegistrationResponse(RequestResponceType.GlobalError, Resources.ErrorCaption, null);
            }
        }
        #endregion

        #region method GetProblem
        public sealed class GetProblemOutModel
        {
            public ProblemBll Problem { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetProblem", Name = "GetProblem")]
        public GetProblemOutModel GetProblem([FromQuery] Guid id)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetProblemOutModel() { Problem = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetProblem userID={0}, userName={1}, ID={2}", user.Id, user.UserName, id);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = ProblemBll.Get(id, user.User.ID, dataSource);
                    if (!IsPrivilegeAccess(id, dataSource))
                    {
                        if (!retval.AccessIsGranted(user.User, dataSource))
                        {
                            Logger.Trace("SDApiController.GetProblem userID={0}, userName={1}, ID={2} failed (access denied)", user.Id, user.UserName, id);
                            return new GetProblemOutModel() { Problem = null, Result = RequestResponceType.AccessError };
                        }
                        //
                        if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_Problem_Properties) && !user.IsClientView(dataSource))
                        {
                            Logger.Trace("SDApiController.GetProblem userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, id);
                            return new GetProblemOutModel() { Problem = null, Result = RequestResponceType.OperationError };
                        }
                    }
                    //
                    retval.InitializeUserFieldNames(dataSource);
                    return new GetProblemOutModel() { Problem = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetProblem is NULL, id: '{0}'", id);
                return new GetProblemOutModel() { Problem = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetProblem, id: '{0}'", id);
                return new GetProblemOutModel() { Problem = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetProblemID
        [HttpGet]
        [Route("sdApi/GetProblemID", Name = "GetProblemID")]
        public Guid? GetProblemID([FromQuery] int number)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetProblemID userID={0}, userName={1}, number={2}", user.Id, user.UserName, number);
            try
            {
                var retval = ProblemBll.GetIDByNumber(number);
                return retval;
            }
            catch (Exception e)
            {
                Logger.Error(e, "GetProblemID error");
                return null;
            }
        }
        #endregion

        #region method GetProblemReference
        public sealed class GetProblemReferenceIncomingModel
        {
            public Guid EntityID { get; set; }
            public int EntityClassId { get; set; }
            public Guid ProblemID { get; set; }
            public bool ReferenceExists { get; set; }
        }
        public sealed class GetProblemReferenceOutModel
        {
            public ProblemReference Elem { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetPBReference", Name = "GetProblemReference")]
        public GetProblemReferenceOutModel GetProblemReference([FromQuery] GetProblemReferenceIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetProblemReferenceOutModel() { Elem = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetProblemReference userID={0}, userName={1}, objID={2}, objClassId={3}, ProblemID={4}", user.Id, user.UserName, model.EntityID, model.EntityClassId, model.ProblemID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.EntityClassId == IMSystem.Global.OBJ_CALL)
                    {
                        if (!Call.AccessIsGranted(model.EntityID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetProblemReference userID={0}, userName={1}, objID={2}, objClassId={3}, ProblemID={4} failed (access denied)", user.Id, user.UserName, model.EntityID, model.EntityClassId, model.ProblemID);
                            return new GetProblemReferenceOutModel() { Elem = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_WORKORDER)
                    {
                        if (!WorkOrder.AccessIsGranted(model.EntityID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetProblemReference userID={0}, userName={1}, objID={2}, objClassId={3}, ProblemID={4} failed (access denied)", user.Id, user.UserName, model.EntityID, model.EntityClassId, model.ProblemID);
                            return new GetProblemReferenceOutModel() { Elem = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else
                        throw new NotSupportedException("entityClassID not valid");
                    //
                    var retval = ProblemReference.Get(model.EntityID, model.EntityClassId, model.ProblemID, dataSource);
                    return new GetProblemReferenceOutModel() { Elem = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                RequestResponceType result;
                if (model.ReferenceExists)
                {
                    result = RequestResponceType.ObjectDeleted;
                    Logger.Error(ex, "GetProblemReference is NULL, model: '{0}'", model);
                }
                else
                    result = RequestResponceType.Success;
                //
                return new GetProblemReferenceOutModel() { Elem = null, Result = result };
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetProblemReference not supported, model: '{0}'", model));
                return new GetProblemReferenceOutModel() { Elem = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetProblemReference, model: {0}.", model);
                return new GetProblemReferenceOutModel() { Elem = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetProblemReferenceList
        public sealed class GetProblemReferenceListIncomingModel
        {
            public Guid ID { get; set; }
            public int EntityClassId { get; set; }
        }
        public sealed class GetProblemReferenceListOutModel
        {
            public IList<ProblemReference> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetPBReferenceList", Name = "GetProblemReferenceList")]
        public GetProblemReferenceListOutModel GetProblemReferenceList([FromQuery] GetProblemReferenceListIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetProblemReferenceListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetProblemReferenceList userID={0}, userName={1}, objID={2}, objClassId={3}", user.Id, user.UserName, model.ID, model.EntityClassId);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.EntityClassId == IMSystem.Global.OBJ_CALL)
                    {
                        if (!Call.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetProblemReferenceList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetProblemReferenceListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_WORKORDER)
                    {
                        if (!WorkOrder.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetProblemReferenceList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetProblemReferenceListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else
                        throw new NotSupportedException("entityClassID not valid");
                    //
                    var retval = ProblemReference.GetList(model.ID, model.EntityClassId, dataSource);
                    return new GetProblemReferenceListOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetProblemReferenceList not supported, model: '{0}'", model));
                return new GetProblemReferenceListOutModel() { List = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetProblemReferenceList, model: {0}.", model);
                return new GetProblemReferenceListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method RegisterProblem
        [HttpPost]
        [Route("sdApi/registerProblem", Name = "RegisterProblem")]
        [Obsolete("POST to api/problems instead")]
        public ProblemRegistrationResponse RegisterProblem([FromBodyOrForm] DTL.SD.Problems.ProblemRegistrationInfo info)
        {
            var user = base.CurrentUser;
            if (info == null || user == null)
                return new ProblemRegistrationResponse(RequestResponceType.NullParamsError, Resources.ErrorCaption, null);
            //
            Logger.Trace("SDApiController.RegisterProblem userID={0}, userName={1}", user.Id, user.UserName);
            if (!user.User.HasRoles || !user.User.OperationIsGranted(IMSystem.Global.OPERATION_Problem_Add))
                return new ProblemRegistrationResponse(RequestResponceType.AccessError, Resources.AccessError, null);
            //
            if (!info.ProblemTypeID.HasValue)
                return new ProblemRegistrationResponse(RequestResponceType.BadParamsError, Resources.ErrorCaption, null);
            //
            try
            {
                List<object> documentList;
                List<string> paths;
                var api = new FileApiController(_environment);
                if (!api.GetDocumentFromFiles(info.Files, out documentList, out paths, user))
                    return new ProblemRegistrationResponse(RequestResponceType.BadParamsError, Resources.UploadedFileNotFoundAtServerSide, null);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);
                    Core.BaseObject bo = ProblemBll.RegisterProblem(info, documentList, user.User, dataSource);
                    dataSource.CommitTransaction();
                    //                    
                    foreach (var filePath in paths)
                        System.IO.File.Delete(filePath);
                    //
                    var problem = ProblemBll.Get(bo.ID, null, dataSource);
                    return new ProblemRegistrationResponse(RequestResponceType.Success, string.Format(Resources.ProblemRegisteredMessage, problem.Number), bo.ID);
                }
            }
            catch (DemoVersionException)
            {
                return new ProblemRegistrationResponse(RequestResponceType.AccessError, Resources.DemoVersionException, null);
            }
            catch (OutOfMemoryException ex)
            {
                Logger.Warning(ex);
                return new ProblemRegistrationResponse(RequestResponceType.GlobalError, Resources.OutOfMemoryException, null);
            }
            catch (ArgumentValidationException ex)
            {
                return new ProblemRegistrationResponse(RequestResponceType.BadParamsError, string.Format(Resources.ArgumentValidationException, ex.Message), null);
            }
            catch (ObjectInUseException)
            {
                return new ProblemRegistrationResponse(RequestResponceType.ConcurrencyError, Resources.ConcurrencyError, null);
            }
            catch (ObjectConstraintException)
            {
                return new ProblemRegistrationResponse(RequestResponceType.BadParamsError, Resources.SaveError, null);
            }
            catch (ObjectDeletedException)
            {
                return new ProblemRegistrationResponse(RequestResponceType.ObjectDeleted, Resources.SaveError, null);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new ProblemRegistrationResponse(RequestResponceType.GlobalError, Resources.ErrorCaption, null);
            }
        }
        #endregion

        #region method GetProblemTypeList
        [HttpGet]
        [Route("sdApi/GetProblemTypeList", Name = "GetProblemTypeList")]
        [Obsolete("Use api/problemtypes instead.")]
        public DTL.SD.Problems.ProblemType[] GetProblemTypeList()
        {
            var user = base.CurrentUser;
            if (!user.User.HasRoles)
            {
                Logger.Trace("SDApiController.GetProblemTypeList canceled (access denied)");
                return null;
            }
            try
            {
                var problemTypeList = ProblemType.GetList();
                //
                var retval = problemTypeList.
                    Select(x => x.DTL).
                    OrderBy(x => x.Name).
                    ToArray();
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения типов проблем для инженера");
                return null;
            }
        }
        #endregion

        #region method GetProblemCauseList
        [HttpGet]
        [Route("sdApi/GetProblemCauseList", Name = "GetProblemCauseList")]
        [Obsolete("Use api/problemcauses instead")]
        public DTL.SD.Problems.ProblemCause[] GetProblemCauseList()
        {
            var user = base.CurrentUser;
            if (!user.User.HasRoles)
            {
                Logger.Trace("SDApiController.GetProblemCauseList canceled (access denied)");
                return null;
            }
            try
            {
                var problemCauseList = ProblemCause.GetList();
                //
                var retval = problemCauseList.
                    Select(x => x.DTL).
                    OrderBy(x => x.Name).
                    ToArray();
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения причин проблем");
                return null;
            }
        }
        #endregion

        #region helper class ProblemRegistrationResponse
        public sealed class ProblemRegistrationResponse
        {
            public ProblemRegistrationResponse(RequestResponceType type, string message, Guid? problemID)
            {
                this.Type = type;
                this.Message = message;
                this.ProblemID = problemID;
            }

            public RequestResponceType Type { get; private set; }
            public string Message { get; private set; }
            public Guid? ProblemID { get; private set; }
        }
        #endregion

        #region method GetDependencyList
        public sealed class GetDependencyListIncomingModel
        {
            public Guid ID { get; set; }
            public int EntityClassId { get; set; }
            public bool[] Parameters { get; set; }
        }
        public sealed class GetDependencyListOutModel
        {
            public IList<DependencyObject> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetLinksList", Name = "GetDependencyList")]
        public GetDependencyListOutModel GetDependencyList([FromQuery] GetDependencyListIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetDependencyListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetDependencyList userID={0}, userName={1}, objID={2}, objClassId={3}", user.Id, user.UserName, model.ID, model.EntityClassId);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.EntityClassId == IMSystem.Global.OBJ_CALL)
                    {
                        if (!Call.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetDependencyList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetDependencyListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_WORKORDER)
                    {
                        if (!WorkOrder.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetDependencyList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetDependencyListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_PROBLEM)
                    {
                        if (!ProblemBll.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetDependencyList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetDependencyListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_RFC)
                    {
                        if (!RFC.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetDependencyList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetDependencyListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else
                        throw new NotSupportedException("entityClassID not valid");
                    //
                    var retval = DependencyObject.GetList(model.EntityClassId, model.ID, null, dataSource, model.Parameters);
                    return new GetDependencyListOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetDependencyList not supported, model: '{0}'", model));
                return new GetDependencyListOutModel() { List = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetDependencyList, model: {0}.", model);
                return new GetDependencyListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method NegotiationExists
        [HttpGet]
        [Obsolete("api/negotiations?userID={currentUserID}&orderByProperty=ID&take=1")]
        [Route("sdApi/NegotiationExists", Name = "NegotiationExists")]
        public bool NegotiationExists()
        {
            var user = base.CurrentUser;
            if (user == null)
                return false;
            //
            Logger.Trace("SDApiController.NegotiationExists userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                var retval = Negotiation.ExistsByUser(user.User.ID);
                return retval;
            }
            catch (Exception e)
            {
                Logger.Error(e, "NegotiationExists error");
                return false;
            }
        }
        #endregion

        #region method GetNegotiationInfo
        [HttpGet]
        [Route("sdApi/GetNegotiationInfo", Name = "GetNegotiationInfo")]
        public NegotiationInfo GetNegotiationInfo([FromQuery] Guid negotiationID)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetNegotiationInfo userID={0}, userName={1}, negotiationID={2}", user.Id, user.UserName, negotiationID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var negotiation = Negotiation.Get(negotiationID, dataSource);
                    return new NegotiationInfo(negotiation.ID, negotiation.ObjectClassID, negotiation.ObjectID);
                }
            }
            catch (ObjectDeletedException e)
            {
                Logger.Error(e, "GetNegotiationInfo object deleted error");
                return null;
            }
            catch (Exception e)
            {
                Logger.Error(e, "GetNegotiationInfo error");
                return null;
            }
        }
        #endregion

        #region method GetNegotiation
        public sealed class GetNegotiationIncomingModel
        {
            public Guid EntityID { get; set; }
            public int EntityClassId { get; set; }
            public Guid NegotiationID { get; set; }
        }
        public sealed class GetNegotiationOutModel
        {
            public Negotiation Elem { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetNegotiation", Name = "GetNegotiation")]
        [Obsolete("GET api/negotiations/{id}")]
        public GetNegotiationOutModel GetNegotiationList([FromQuery] GetNegotiationIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetNegotiationOutModel() { Elem = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetNegotiation userID={0}, userName={1}, objID={2}, objClassId={3}, negotiationID={4}", user.Id, user.UserName, model.EntityID, model.EntityClassId, model.NegotiationID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.EntityClassId == IMSystem.Global.OBJ_CALL)
                    {
                        if (!Call.AccessIsGranted(model.EntityID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetNegotiation userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.EntityID, model.EntityClassId);
                            return new GetNegotiationOutModel() { Elem = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_WORKORDER)
                    {
                        if (!WorkOrder.AccessIsGranted(model.EntityID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetNegotiation userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.EntityID, model.EntityClassId);
                            return new GetNegotiationOutModel() { Elem = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_PROBLEM)
                    {
                        if (!ProblemBll.AccessIsGranted(model.EntityID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetNegotiation userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.EntityID, model.EntityClassId);
                            return new GetNegotiationOutModel() { Elem = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_RFC)
                    {
                        if (!RFC.AccessIsGranted(model.EntityID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetNegotiation userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.EntityID, model.EntityClassId);
                            return new GetNegotiationOutModel() { Elem = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else
                        throw new NotSupportedException("entityClassID not valid");
                    //
                    var retval = Negotiation.Get(model.NegotiationID, dataSource);
                    if (retval.ObjectClassID != model.EntityClassId || retval.ObjectID != model.EntityID)
                        retval = null;//не принадлежит этому объекту
                    return new GetNegotiationOutModel() { Elem = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException)
            {
                return new GetNegotiationOutModel() { Elem = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetNegotiationList, model: {0}.", model);
                return new GetNegotiationOutModel() { Elem = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetNegotiationList
        public sealed class GetNegotiationListIncomingModel
        {
            public Guid ID { get; set; }
            public int EntityClassId { get; set; }
        }
        public sealed class GetNegotiationListOutModel
        {
            public IList<Negotiation> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetNegotiationList", Name = "GetNegotiationList")]
        [Obsolete("GET api/negotiations/")]
        public GetNegotiationListOutModel GetNegotiationList([FromQuery] GetNegotiationListIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetNegotiationListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetNegotiationList userID={0}, userName={1}, objID={2}, objClassId={3}", user.Id, user.UserName, model.ID, model.EntityClassId);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.EntityClassId == IMSystem.Global.OBJ_CALL)
                    {
                        if (!Call.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetNegotiationList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetNegotiationListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_WORKORDER)
                    {
                        if (!WorkOrder.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetNegotiationList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetNegotiationListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_PROBLEM)
                    {
                        if (!ProblemBll.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetNegotiationList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetNegotiationListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_RFC)
                    {
                        if (!RFC.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetNegotiationList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetNegotiationListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else
                        throw new NotSupportedException("entityClassID not valid");
                    //
                    var retval = Negotiation.GetList(model.ID, model.EntityClassId, dataSource);
                    retval.Sort((x, y) =>
                    {
                        if (x.IsFinished == y.IsFinished)
                        {
                            if (x.IsFinished)
                                return Negotiation.CompareByDateVoteEnd(x, y);
                            else
                            {
                                if (x.IsNotStarted && y.IsNotStarted)
                                    return 0;
                                else if (x.IsNotStarted)
                                    return 1;
                                else if (y.IsNotStarted)
                                    return -1;
                                else return Negotiation.CompareByDateVoteStart(x, y);
                            }
                        }
                        else if (x.IsFinished)
                            return 1;
                        else return -1;
                    });
                    return new GetNegotiationListOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetNegotiationList not supported, model: '{0}'", model));
                return new GetNegotiationListOutModel() { List = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetNegotiationList, model: {0}.", model);
                return new GetNegotiationListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetAssetList
        public sealed class GetAssetListIncomingModel
        {
            public Guid ID { get; set; }
            public int EntityClassId { get; set; }
        }
        public sealed class GetAssetListOutModel
        {
            public IList<Asset> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetAssetList", Name = "GetAssetList")]
        public GetAssetListOutModel GetAssetList([FromQuery] GetAssetListIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetAssetListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetAssetList userID={0}, userName={1}, objID={2}, objClassId={3}", user.Id, user.UserName, model.ID, model.EntityClassId);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.EntityClassId != IMSystem.Global.OBJ_USER)
                        throw new NotSupportedException("entityClassID not valid");
                    //
                    var retval = Asset.GetList(model.ID, model.EntityClassId, dataSource);
                    foreach (var el in retval)
                    {
                        if (!string.IsNullOrEmpty(el.State))
                        {
                            string stateRes = Resources.ResourceManager.GetString(el.State, Thread.CurrentThread.CurrentUICulture);
                            if (!string.IsNullOrEmpty(stateRes))
                                el.State = stateRes;
                        }
                        if (el.ClassID == IMSystem.Global.OBJ_NETWORKDEVICE)
                            el.Type = Resources.NetworkDevice;
                        else //if (el.ClassID == IMSystem.Global.OBJ_TERMINALDEVICE) TODO!
                            el.Type = Resources.TerminalDevice;
                    }
                    //
                    return new GetAssetListOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetAssetList not supported, model: '{0}'", model));
                return new GetAssetListOutModel() { List = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetAssetList, model: {0}.", model);
                return new GetAssetListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetNegotiationModeAndStatus
        public sealed class GetNegotiationModeAndStatusOutModel
        {
            public IList<ListInfo> ModeList { get; set; }
            public IList<ListInfo> StatusList { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetNegotiationModeAndStatus", Name = "GetNegotiationModeAndStatus")]
        [Obsolete("GET api/negotiationmodes, api/negotiationstatuses instead")]
        public GetNegotiationModeAndStatusOutModel GetNegotiationModeAndStatus()
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetNegotiationModeAndStatusOutModel() { ModeList = null, StatusList = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetNegotiationModeAndStatus userID={0}, userName={1}", user.Id, user.UserName);
                //
                var modeList = Negotiation.GetModeList();
                var statusList = Negotiation.GetStatusList();
                return new GetNegotiationModeAndStatusOutModel() { ModeList = modeList, StatusList = statusList, Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetNegotiationModeAndStatus");
                return new GetNegotiationModeAndStatusOutModel() { ModeList = null, StatusList = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetManhoursList
        public sealed class GetManhoursListIncomingModel
        {
            public Guid WorkID { get; set; }
        }
        public sealed class GetManhoursListOutModel
        {
            public IList<Manhours> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetManhoursList", Name = "GetManhoursList")]
        public GetManhoursListOutModel GetManhoursList([FromQuery] GetManhoursListIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetManhoursListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetManhoursList userID={0}, userName={1}, workID={2}", user.Id, user.UserName, model.WorkID);
                //
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_ManhoursWork_Properties))
                {
                    Logger.Trace("SDApiController.GetManhoursList userID={0}, userName={1}, WorkID={2} failed (operation denied)", user.Id, user.UserName, model.WorkID);
                    return new GetManhoursListOutModel() { List = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = Manhours.GetList(model.WorkID, dataSource);
                    return new GetManhoursListOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetManhoursList not supported, model: '{0}'", model));
                return new GetManhoursListOutModel() { List = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetManhoursList, model: {0}.", model);
                return new GetManhoursListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetManhour
        public sealed class GetManhourIncomingModel
        {
            public Guid ID { get; set; }
        }
        public sealed class GetManhourOutModel
        {
            public Manhours Manhour { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetManhour", Name = "GetManhour")]
        public GetManhourOutModel GetManhour([FromQuery] GetManhourIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetManhourOutModel() { Manhour = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetManhour userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
                //
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_ManhoursWork_Properties))
                {
                    Logger.Trace("SDApiController.GetManhour userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return new GetManhourOutModel() { Manhour = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = Manhours.Get(model.ID, dataSource);
                    return new GetManhourOutModel() { Manhour = retval, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetManhour not supported, model: '{0}'", model));
                return new GetManhourOutModel() { Manhour = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetManhour, model: {0}.", model);
                return new GetManhourOutModel() { Manhour = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetManhoursWorkList
        public sealed class GetManhoursWorkListIncomingModel
        {
            public Guid? ID { get; set; }
            public int? EntityClassId { get; set; }
        }
        public sealed class GetManhoursWorkListOutModel
        {
            public IList<ManhoursWork> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetManhoursWorkList", Name = "GetManhoursWorkList")]
        public GetManhoursWorkListOutModel GetManhoursWorkList([FromQuery] GetManhoursWorkListIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetManhoursWorkListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetManhoursWorkList userID={0}, userName={1}, objID={2}, objClassId={3}", user.Id, user.UserName, model.ID.HasValue ? model.ID.Value.ToString() : "null", model.EntityClassId.HasValue ? model.EntityClassId.Value.ToString() : "null");
                //
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_ManhoursWork_Properties))
                {
                    Logger.Trace("SDApiController.GetManhoursWorkList userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return new GetManhoursWorkListOutModel() { List = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.EntityClassId.HasValue && model.ID.HasValue)
                    {
                        if (model.EntityClassId.Value == IMSystem.Global.OBJ_CALL)
                        {
                            if (!Call.AccessIsGranted(model.ID.Value, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.GetManhoursWorkList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID.Value, model.EntityClassId.Value);
                                return new GetManhoursWorkListOutModel() { List = null, Result = RequestResponceType.AccessError };
                            }
                        }
                        else if (model.EntityClassId.Value == IMSystem.Global.OBJ_WORKORDER)
                        {
                            if (!WorkOrder.AccessIsGranted(model.ID.Value, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.GetManhoursWorkList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID.Value, model.EntityClassId.Value);
                                return new GetManhoursWorkListOutModel() { List = null, Result = RequestResponceType.AccessError };
                            }
                        }
                        else if (model.EntityClassId.Value == IMSystem.Global.OBJ_PROBLEM)
                        {
                            if (!ProblemBll.AccessIsGranted(model.ID.Value, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.GetManhoursWorkList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID.Value, model.EntityClassId.Value);
                                return new GetManhoursWorkListOutModel() { List = null, Result = RequestResponceType.AccessError };
                            }
                        }
                        else if (model.EntityClassId.Value == IMSystem.Global.OBJ_RFC)
                        {
                            if (!RFC.AccessIsGranted(model.ID.Value, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.GetManhoursWorkList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID.Value, model.EntityClassId.Value);
                                return new GetManhoursWorkListOutModel() { List = null, Result = RequestResponceType.AccessError };
                            }
                        }
                        else if (model.EntityClassId.Value == IMSystem.Global.OBJ_Project)
                        {
                            if (!Project.AccessIsGranted(model.ID.Value, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.GetManhoursWorkList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID.Value, model.EntityClassId.Value);
                                return new GetManhoursWorkListOutModel() { List = null, Result = RequestResponceType.AccessError };
                            }
                        }
                        else
                            throw new NotSupportedException("entityClassID not valid");
                    }
                    //
                    var retval = ManhoursWork.GetList(model.ID, model.EntityClassId, dataSource);
                    return new GetManhoursWorkListOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetManhoursWorkList not supported, model: '{0}'", model));
                return new GetManhoursWorkListOutModel() { List = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetManhoursWorkList, model: {0}.", model);
                return new GetManhoursWorkListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetManhoursWork
        public sealed class GetManhoursWorkIncomingModel
        {
            public Guid ID { get; set; }
            public Guid? EntityID { get; set; }
            public int? EntityClassId { get; set; }
        }
        public sealed class GetManhoursWorkOutModel
        {
            public ManhoursWork Work { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetManhoursWork", Name = "GetManhoursWork")]
        public GetManhoursWorkOutModel GetManhoursWork([FromQuery] GetManhoursWorkIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetManhoursWorkOutModel() { Work = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetManhoursWork userID={0}, userName={1}, objID={2}, objClassId={3}, ID={4}", user.Id, user.UserName, model.EntityID.HasValue ? model.EntityID.Value.ToString() : "null", model.EntityClassId.HasValue ? model.EntityClassId.Value.ToString() : "null", model.ID);
                //
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_ManhoursWork_Properties))
                {
                    Logger.Trace("SDApiController.GetManhoursWork userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return new GetManhoursWorkOutModel() { Work = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.EntityClassId.HasValue && model.EntityID.HasValue)
                    {
                        if (model.EntityClassId.Value == IMSystem.Global.OBJ_CALL)
                        {
                            if (!Call.AccessIsGranted(model.EntityID.Value, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.GetManhoursWork userID={0}, userName={1}, objID={2}, objClassId={3}, ID={4} failed (access denied)", user.Id, user.UserName, model.EntityID.Value, model.EntityClassId.Value, model.ID);
                                return new GetManhoursWorkOutModel() { Work = null, Result = RequestResponceType.AccessError };
                            }
                        }
                        else if (model.EntityClassId.Value == IMSystem.Global.OBJ_WORKORDER)
                        {
                            if (!WorkOrder.AccessIsGranted(model.EntityID.Value, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.GetManhoursWork userID={0}, userName={1}, objID={2}, objClassId={3}, ID={4} failed (access denied)", user.Id, user.UserName, model.EntityID.Value, model.EntityClassId.Value, model.ID);
                                return new GetManhoursWorkOutModel() { Work = null, Result = RequestResponceType.AccessError };
                            }
                        }
                        else if (model.EntityClassId.Value == IMSystem.Global.OBJ_PROBLEM)
                        {
                            if (!ProblemBll.AccessIsGranted(model.EntityID.Value, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.GetManhoursWork userID={0}, userName={1}, objID={2}, objClassId={3}, ID={4} failed (access denied)", user.Id, user.UserName, model.EntityID.Value, model.EntityClassId.Value, model.ID);
                                return new GetManhoursWorkOutModel() { Work = null, Result = RequestResponceType.AccessError };
                            }
                        }
                        else if (model.EntityClassId.Value == IMSystem.Global.OBJ_Project)
                        {
                            if (!Project.AccessIsGranted(model.EntityID.Value, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiController.GetManhoursWork userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.EntityID.Value, model.EntityClassId.Value);
                                return new GetManhoursWorkOutModel() { Work = null, Result = RequestResponceType.AccessError };
                            }
                        }
                        else
                            throw new NotSupportedException("entityClassID not valid");
                    }
                    //
                    var retval = ManhoursWork.Get(model.ID, dataSource);
                    return new GetManhoursWorkOutModel() { Work = retval, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetManhoursWork not supported, model: '{0}'", model));
                return new GetManhoursWorkOutModel() { Work = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetManhoursWork, model: {0}.", model);
                return new GetManhoursWorkOutModel() { Work = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetSolution
        public sealed class GetSolutionIncomingModel
        {
            public Guid EntityID { get; set; }
            public int EntityClassId { get; set; }
            public Guid SolutionID { get; set; }
        }
        public sealed class GetSolutionOutModel
        {
            public Solution Elem { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetSolution", Name = "GetSolution")]
        public GetSolutionOutModel GetSolution([FromQuery] GetSolutionIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetSolutionOutModel() { Elem = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetSolution userID={0}, userName={1}, objID={2}, objClassId={3}, solutionID={4}", user.Id, user.UserName, model.EntityID, model.EntityClassId, model.SolutionID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.EntityClassId == IMSystem.Global.OBJ_CALL)
                    {
                        if (!Call.AccessIsGranted(model.EntityID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetSolution userID={0}, userName={1}, objID={2}, objClassId={3}, solutionID={4} failed (access denied)", user.Id, user.UserName, model.EntityID, model.EntityClassId, model.SolutionID);
                            return new GetSolutionOutModel() { Elem = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_PROBLEM)
                    {
                        if (!ProblemBll.AccessIsGranted(model.EntityID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetSolution userID={0}, userName={1}, objID={2}, objClassId={3}, solutionID={4} failed (access denied)", user.Id, user.UserName, model.EntityID, model.EntityClassId, model.SolutionID);
                            return new GetSolutionOutModel() { Elem = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else
                        throw new NotSupportedException("entityClassID not valid");
                    //
                    var retval = Solution.Get(model.SolutionID, dataSource);
                    return new GetSolutionOutModel() { Elem = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetSolution is NULL, model: '{0}'", model);
                return new GetSolutionOutModel() { Elem = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetSolution not supported, model: '{0}'", model));
                return new GetSolutionOutModel() { Elem = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetSolution, model: {0}.", model);
                return new GetSolutionOutModel() { Elem = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetSolutionList
        public sealed class GetSolutionListIncomingModel
        {
            public Guid ID { get; set; }
            public int EntityClassId { get; set; }
        }
        public sealed class GetSolutionListOutModel
        {
            public IList<Solution> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetSolutionList", Name = "GetSolutionList")]
        public GetSolutionListOutModel GetSolutionList([FromQuery] GetSolutionListIncomingModel model)
        {
            // TODO: Пока выключен список Solutions
            return new GetSolutionListOutModel()
            {
                List = new List<Solution>(),
                Result = RequestResponceType.Success
            };

            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetSolutionListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetSolutionList userID={0}, userName={1}, objID={2}, objClassId={3}", user.Id, user.UserName, model.ID, model.EntityClassId);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.EntityClassId == IMSystem.Global.OBJ_CALL)
                    {
                        if (!Call.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetSolutionList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetSolutionListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_PROBLEM)
                    {
                        if (!ProblemBll.AccessIsGranted(model.ID, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetSolutionList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetSolutionListOutModel() { List = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else
                        throw new NotSupportedException("entityClassID not valid");
                    //
                    var retval = Solution.GetList(dataSource);
                    return new GetSolutionListOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetSolutionList not supported, model: '{0}'", model));
                return new GetSolutionListOutModel() { List = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetSolutionList, model: {0}.", model);
                return new GetSolutionListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetUrgencyList
        [HttpGet]
        [Route("sdApi/GetUrgencyList", Name = "GetUrgencyList")]
        public DTL.SimpleDictionary[] GetUrgencyList()
        {
            try
            {
                var urgencyList = SimpleDictionary.GetUrgencyList();
                //
                var retval = urgencyList.
                    Select(x => x.DTL).
                    OrderBy(x => x.Sequence).
                    ToArray();
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка срочностей");
                return null;
            }
        }
        #endregion

        #region method GetIncidentResultList
        [HttpGet]
        [Route("sdApi/GetIncidentResultList", Name = "GetIncidentResultList")]
        [Obsolete("Use /api/incidentresults resource instead")]
        public DTL.SimpleDictionary[] GetIncidentResultList()
        {
            try
            {
                var urgencyList = SimpleDictionary.GetIncidentResultList();
                //
                var retval = urgencyList.
                    Select(x => x.DTL).
                    OrderBy(x => x.Name).
                    ToArray();
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка результатов завершений инцидентов");
                return null;
            }
        }
        #endregion

        #region method GetFullPriorityList
        public sealed class GetFullPriorityListIncomingModel
        {
            public Guid? ID { get; set; }
            public int EntityClassId { get; set; }
        }
        public sealed class GetFullPriorityListOutModel
        {
            public IList<Priority> PriorityList { get; set; }
            public IList<Influence> InfluenceList { get; set; }
            public IList<Urgency> UrgencyList { get; set; }
            public IList<Concordance> ConcordanceList { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetFullPriorityList", Name = "GetFullPriorityList")]
        public GetFullPriorityListOutModel GetFullPriorityList([FromQuery] GetFullPriorityListIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetFullPriorityListOutModel() { ConcordanceList = null, InfluenceList = null, PriorityList = null, UrgencyList = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetFullPriorityList userID={0}, userName={1}, objID={2}, objClassId={3}", user.Id, user.UserName, model.ID, model.EntityClassId);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.EntityClassId == IMSystem.Global.OBJ_CALL)
                    {
                        if (model.ID.HasValue && !Call.AccessIsGranted(model.ID.Value, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetFullPriorityList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetFullPriorityListOutModel() { ConcordanceList = null, InfluenceList = null, PriorityList = null, UrgencyList = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_WORKORDER)
                    {
                        if (model.ID.HasValue && !WorkOrder.AccessIsGranted(model.ID.Value, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetFullPriorityList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetFullPriorityListOutModel() { ConcordanceList = null, InfluenceList = null, PriorityList = null, UrgencyList = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_PROBLEM)
                    {
                        if (model.ID.HasValue && !ProblemBll.AccessIsGranted(model.ID.Value, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetFullPriorityList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetFullPriorityListOutModel() { ConcordanceList = null, InfluenceList = null, PriorityList = null, UrgencyList = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else if (model.EntityClassId == IMSystem.Global.OBJ_RFC)
                    {
                        if (model.ID.HasValue && !RFC.AccessIsGranted(model.ID.Value, user.User, true, dataSource))
                        {
                            Logger.Error("SDApiController.GetFullPriorityList userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, model.ID, model.EntityClassId);
                            return new GetFullPriorityListOutModel() { ConcordanceList = null, InfluenceList = null, PriorityList = null, UrgencyList = null, Result = RequestResponceType.AccessError };
                        }
                    }
                    else
                        throw new NotSupportedException("entityClassID not valid");
                    //
                    if (model.EntityClassId == IMSystem.Global.OBJ_WORKORDER)
                    {
                        var priorityList = Priority.GetListForWorkOrder(dataSource);
                        //
                        return new GetFullPriorityListOutModel() { ConcordanceList = null, InfluenceList = null, PriorityList = priorityList, UrgencyList = null, Result = RequestResponceType.Success };
                    }
                    else
                    {
                        var priorityList = Priority.GetList(dataSource);
                        var urgencyList = Urgency.GetList(dataSource).OrderBy(u => u.Sequence).Take(Priority.MAX_TABLE_COUNT).ToList();
                        var concordanceList = Concordance.GetList(dataSource);
                        var influenceList = Influence.GetList(dataSource).OrderBy(i => i.Sequence).Take(Priority.MAX_TABLE_COUNT).ToList();
                        //
                        return new GetFullPriorityListOutModel() { ConcordanceList = concordanceList, InfluenceList = influenceList, PriorityList = priorityList, UrgencyList = urgencyList, Result = RequestResponceType.Success };
                    }
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetFullPriorityList not supported, model: '{0}'", model));
                return new GetFullPriorityListOutModel() { ConcordanceList = null, InfluenceList = null, PriorityList = null, UrgencyList = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetFullPriorityList, model: {0}.", model);
                return new GetFullPriorityListOutModel() { ConcordanceList = null, InfluenceList = null, PriorityList = null, UrgencyList = null, Result = RequestResponceType.GlobalError };
            }
        }

        public sealed class GetFullPriorityListOutModel2
        {
            public PriorityDetailsModel[] PriorityList { get; set; }
            public InfluenceListItemModel[] InfluenceList { get; set; }
            public UrgencyListItemModel[] UrgencyList { get; set; }
            public ConcordanceModel[] ConcordanceList { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet("sdApi/GetFullPriorityList2")] // TODO: not for production
        public async Task<GetFullPriorityListOutModel2> GetFullPriorityList2Async(
            [FromQuery] GetFullPriorityListIncomingModel model, CancellationToken cancellationToken)
        {
            PriorityDetailsModel[] priorityList = await _priorityBll.ListAsync(new LookupListFilterModel(), cancellationToken);
            InfluenceListItemModel[] influenceList = await _influenceBll.ListAsync(cancellationToken);
            UrgencyListItemModel[] urgencyList = await _urgencyBll.ListAsync(cancellationToken);

            return new GetFullPriorityListOutModel2
            {
                PriorityList = priorityList.Take(Priority.MAX_TABLE_COUNT).ToArray(),
                InfluenceList = influenceList.Take(Priority.MAX_TABLE_COUNT).ToArray(),
                UrgencyList = urgencyList.Take(Priority.MAX_TABLE_COUNT).ToArray(),
                ConcordanceList = await _concordanceBll.ListAsync(cancellationToken)
            };
        }

        #endregion

        #region method GetDefaultParametes
        [HttpGet]
        [Route("sdApi/getDefaultParameters", Name = "GetDefaultParametes")]
        public DTL.Parameters.ParameterInfo[] GetDefaultParametes(int objectClassID, Guid templateObjectID)
        {
            return Array.Empty<DTL.Parameters.ParameterInfo>();

            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetDefaultParametes userID={0}, userName={1}, templateObjectID={2}, objectClassID={3}", user.Id, user.UserName, templateObjectID, objectClassID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {

                    //TODO удалить после скрипта конвертации старые->новые шаблоны параметров
                    var oldParameterTemplateList = BLL.Parameters.ParameterValue.GetListByDefault(
                    InfraManager.IM.BusinessLayer.User.Get(user.User.ID),
                    objectClassID,
                    templateObjectID).ToList();


                    //Загрузка новых параметров
                    var newParameterTemplateList = BLL.FormBuilder.Form.GetNewParameterTamplate(templateObjectID, objectClassID, InfraManager.IM.BusinessLayer.User.Get(user.User.ID), dataSource);
                    newParameterTemplateList.AddRange(oldParameterTemplateList);
                    return newParameterTemplateList.ToArray();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения параметров по идентификатору шаблона.");
                return new DTL.Parameters.ParameterInfo[0];
            }
        }
        #endregion

        #region method GetParametersCopy
        [HttpGet]
        [Route("sdApi/getParametersCopy", Name = "GetParametersCopy")]
        public DTL.Parameters.ParameterInfo[] GetParametersCopy(int objectClassID, Guid objectID)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetParametersCopy userID={0}, userName={1}, objectClassID={2}, objectID={3}", user.Id, user.UserName, objectClassID, objectID);
            try
            {
                var retval = BLL.Parameters.ParameterValue.GetCopy(
                    InfraManager.IM.BusinessLayer.User.Get(user.User.ID),
                    objectClassID,
                    objectID);
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения копии параметров по объекту.");
                return null;
            }
        }
        #endregion

        #region method GetOrCreateParameters
        [HttpGet]
        [Route("sdApi/getOrCreateParameters", Name = "GetOrCreateParameters")]
        public DTL.Parameters.ParameterInfo[] GetOrCreateParameters(int objectClassID, Guid objectID, bool recalculateParameters)
        {
            return Array.Empty<DTL.Parameters.ParameterInfo>();
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetOrCreateParameters userID={0}, userName={1}, objectID={2}, objectClassID={3}, recalculateParameters={4}", user.Id, user.UserName, objectID, objectClassID, recalculateParameters);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = BLL.Parameters.ParameterValue.GetOrCreateByDefault(
                        InfraManager.IM.BusinessLayer.User.Get(user.User.ID, dataSource),
                        objectClassID,
                        objectID,
                        recalculateParameters,
                        dataSource);
                    return retval;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка создания параметров по идентификатору шаблона.");
                return null;
            }
        }
        #endregion

        #region method GetParametes
        [HttpGet]
        [Route("sdApi/getParameters", Name = "GetParametes")]
        public DTL.Parameters.ParameterInfo[] GetParametes(int objectClassID, Guid objectID)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetParametes userID={0}, userName={1}, objectID={2}, objectClassID={3}", user.Id, user.UserName, objectID, objectClassID);
            try
            {
                var retval = BLL.Parameters.ParameterValue.GetList(
                    InfraManager.IM.BusinessLayer.User.Get(user.User.ID),
                    objectClassID,
                    objectID);
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения параметров по идентификатору объекта.");
                return null;
            }
        }
        #endregion

        #region method GetParametValueList
        [HttpPost]
        [Route("sdApi/getParameterValueList", Name = "GetParameterValueList")]
        public DTL.Parameters.ParameterValue[] GetParameterValueList([FromBodyOrForm] GetParameterValueListRequest data)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetParameterValueList userID={0}, userName={1}, parametersCount={2}", user.Id, user.UserName, data == null || data.Params == null ? -1 : data.Params.Length);
            try
            {
                List<string> paramList = new List<string>();
                JArray jArray = JArray.Parse(data.Params);
                foreach (var item in jArray)
                    paramList.Add(item.Value<string>());
                // for user searching TODO maybe move it to UserBLL
                string searchName = "";
                if (paramList[0] == "6" && paramList.Count == 6)
                {
                    searchName = paramList[5];
                    paramList.RemoveAt(5);
                }
                var retval = BLL.Parameters.ParameterValue.GetParameterValueList(paramList);
                if (!string.IsNullOrEmpty(searchName))
                {
                    retval = retval.Where(i => i.Name.ToLower().Contains(searchName.ToLower())).ToList();
                }
                //
                return retval.ToArray();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка значений параметра.");
                return null;
            }
        }
        public sealed class GetParameterValueListRequest
        {
            public string Params { get; set; }
        }
        #endregion

        #region method GetUserFieldsName
        [HttpGet]
        [Route("sdApi/getUserFieldsName", Name = "GetUserFieldsName")]
        [Obsolete("Use api/userfields instead")]
        public DTL.UserFieldsInfo GetUserFieldsName(int userFieldType)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetUserFieldsName userID={0}, userName={1}, userFieldType={2}", user.Id, user.UserName, userFieldType);
            try
            {
                var retval = BLL.UserFields.UserFieldHelper.Get(userFieldType);
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения значений пользовательских полей.");
                return null;
            }
        }
        #endregion

        #region method GetGrantedOperations        
        public class GrantedOperationResponseModel
        {
            public IEnumerable<OperationID> OperationList { get; set; }
        }

        [Obsolete("Legacy Path. Use /api/users/current/granted-operations path instead")]
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetGrantedOperations", Name = "GetGrantedOperations")]
        public GrantedOperationResponseModel GetGrantedOperations()
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetGrantedOperations userID={0}, userName={1}", user.Id, user.UserName);

            try
            {
                return new GrantedOperationResponseModel()
                {
                    OperationList = user.User.GrantedOperations
                        .Select(go => (OperationID)go)
                        .ToList()
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка доступных операций.");
                return null;
            }
        }
        #endregion

        #region helper class WorkOrderRegistrationResponse
        public sealed class WorkOrderRegistrationResponse
        {
            public WorkOrderRegistrationResponse(RequestResponceType type, string message, Guid? workOrderID)
            {
                this.Type = type;
                this.Message = message;
                this.WorkOrderID = workOrderID;
            }

            public RequestResponceType Type { get; private set; }
            public string Message { get; private set; }
            public Guid? WorkOrderID { get; private set; }
        }
        #endregion

        #region helper class NegotiationInfo
        public sealed class NegotiationInfo
        {
            public NegotiationInfo(Guid id, int objectClassID, Guid objectID)
            {
                this.ID = id;
                this.ObjectClassID = objectClassID;
                this.ObjectID = objectID;
            }

            public Guid ID { get; private set; }
            public int ObjectClassID { get; private set; }
            public Guid ObjectID { get; private set; }
        }
        #endregion

        #region method GetCall
        public sealed class GetEmailTemplateString
        {
            public string BodyTemplate { get; set; }
            public string SubjectTemplate { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetEmailTemplate", Name = "GetEmailTemplate")]
        public GetEmailTemplateString GetEmailTemplate(Guid id, int classID)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetEmailTemplateString() { BodyTemplate = string.Empty, SubjectTemplate = string.Empty, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetEmailTemplate userID={0}, userName={1}, objID={2}", user.Id, user.UserName, id);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = Call.GetEmailForm(id);

                    return new GetEmailTemplateString() { BodyTemplate = retval.Item2, SubjectTemplate = retval.Item1, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetEmailTemplate, id: '{0}'", id);
                return new GetEmailTemplateString() { BodyTemplate = string.Empty, SubjectTemplate = string.Empty, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region helper class RFCRegistrationResponse
        public sealed class RFCRegistrationResponse
        {
            public RFCRegistrationResponse(RequestResponceType type, string message, Guid? problemID)
            {
                this.Type = type;
                this.Message = message;
                this.RFCID = problemID;
            }

            public RequestResponceType Type { get; private set; }
            public string Message { get; private set; }
            public Guid? RFCID { get; private set; }
        }
        #endregion

        #region method RegisterRFC
        [HttpPost]
        [Route("sdApi/registerRFC", Name = "registerRFC")]
        public RFCRegistrationResponse RegisterRFC(DTL.SD.Problems.RFCRegistrationInfo info)
        {
            var user = base.CurrentUser;
            if (info == null || user == null)
                return new RFCRegistrationResponse(RequestResponceType.NullParamsError, Resources.ErrorCaption, null);
            //
            Logger.Trace("SDApiController.RegisterProblem userID={0}, userName={1}", user.Id, user.UserName);
            if (!user.User.HasRoles || !user.User.OperationIsGranted(IMSystem.Global.OPERATION_RFC_Add))
                return new RFCRegistrationResponse(RequestResponceType.AccessError, Resources.AccessError, null);
            //
            try
            {
                //
                List<object> documentList;
                List<string> paths;
                List<string> realizationpaths;
                List<string> rollbackpaths;
                List<object> realizationFile;
                List<object> rollbackFile;
                var api = new FileApiController(_environment);
                if (!api.GetDocumentFromFiles(info.Files, out documentList, out paths, user))
                    return new RFCRegistrationResponse(RequestResponceType.BadParamsError, Resources.UploadedFileNotFoundAtServerSide, null);
                if (!api.GetDocumentFromFiles(info.RealizationFile, out realizationFile, out realizationpaths, user))
                    return new RFCRegistrationResponse(RequestResponceType.BadParamsError, Resources.UploadedFileNotFoundAtServerSide, null);
                if (!api.GetDocumentFromFiles(info.RollbackFile, out rollbackFile, out rollbackpaths, user))
                    return new RFCRegistrationResponse(RequestResponceType.BadParamsError, Resources.UploadedFileNotFoundAtServerSide, null);
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);
                    //Core.BaseObject bo = Problem.RegisterProblem(info, documentList, user.User, dataSource);
                    Core.BaseObject bo = RFC.RegisterRFC(info, documentList, realizationFile, rollbackFile, user.User, dataSource);
                    dataSource.CommitTransaction();
                    //                    
                    foreach (var filePath in paths)
                        System.IO.File.Delete(filePath);
                    //                    
                    foreach (var filePath in realizationpaths)
                        System.IO.File.Delete(filePath);
                    //                    
                    foreach (var filePath in rollbackpaths)
                        System.IO.File.Delete(filePath);
                    //  
                    var rfc = RFC.Get(bo.ID, null, dataSource);
                    return new RFCRegistrationResponse(RequestResponceType.Success, string.Format(Resources.RFCRegisteredMessage, rfc.Number), bo.ID);
                }
            }
            catch (DemoVersionException)
            {
                return new RFCRegistrationResponse(RequestResponceType.AccessError, Resources.DemoVersionException, null);
            }
            catch (OutOfMemoryException ex)
            {
                Logger.Warning(ex);
                return new RFCRegistrationResponse(RequestResponceType.GlobalError, Resources.OutOfMemoryException, null);
            }
            catch (ArgumentValidationException ex)
            {
                return new RFCRegistrationResponse(RequestResponceType.BadParamsError, string.Format(Resources.ArgumentValidationException, ex.Message), null);
            }
            catch (ObjectInUseException)
            {
                return new RFCRegistrationResponse(RequestResponceType.ConcurrencyError, Resources.ConcurrencyError, null);
            }
            catch (ObjectConstraintException)
            {
                return new RFCRegistrationResponse(RequestResponceType.BadParamsError, Resources.SaveError, null);
            }
            catch (ObjectDeletedException)
            {
                return new RFCRegistrationResponse(RequestResponceType.ObjectDeleted, Resources.SaveError, null);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new RFCRegistrationResponse(RequestResponceType.GlobalError, Resources.ErrorCaption, null);
            }
        }
        #endregion

        #region method GetRFCTypeList
        [HttpGet]
        [Route("sdApi/GetRFCTypeList", Name = "GetRFCTypeList")]
        public DTL.SD.RFC.RFCType[] GetRFCTypeList()
        {
            var user = base.CurrentUser;
            if (!user.User.HasRoles)
            {
                Logger.Trace("SDApiController.GetRFCTypeList canceled (access denied)");
                return null;
            }
            try
            {
                var rfcTypeList = RFCType.GetList();
                //
                var retval = rfcTypeList.
                    Select(x => x.DTL).
                    OrderBy(x => x.Name).
                    ToArray();
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения типов RFC для инженера");
                return null;
            }
        }
        #endregion

        #region method GetProblemID
        [HttpGet]
        [Route("sdApi/GetRFCID", Name = "GetRFCID")]
        public Guid? GetRFCID([FromQuery] int number)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetRFCID userID={0}, userName={1}, number={2}", user.Id, user.UserName, number);
            try
            {
                var retval = RFC.GetIDByNumber(number);
                return retval;
            }
            catch (Exception e)
            {
                Logger.Error(e, "GetProblemID error");
                return null;
            }
        }
        #endregion
        #region method GetRFCCategoryList
        [HttpGet]
        [Route("sdApi/GetRFCCategoryList", Name = "GetRFCCategoryList")]
        public DTL.Catalogs.RFCCategory[] GetRFCCategoryList()
        {
            var user = base.CurrentUser;
            if (!user.User.HasRoles)
            {
                Logger.Trace("SDApiController.GetRFCCategoryList canceled (access denied)");
                return null;
            }
            try
            {
                var rfccategoryList = RFCCategory.GetList();
                //
                var retval = rfccategoryList.
                    Select(x => x.DTL).
                    OrderBy(x => x.Name).
                    ToArray();
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения категорий RFC для инженера");
                return null;
            }
        }
        #endregion

        #region method GetRFC
        public sealed class GetProblemOutRFC
        {
            public RFC RFC { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetRFC", Name = "GetRFC")]
        public GetProblemOutRFC GetRFC([FromQuery] Guid id)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetProblemOutRFC() { RFC = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetProblem userID={0}, userName={1}, ID={2}", user.Id, user.UserName, id);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = RFC.Get(id, user.User.ID, dataSource);
                    if (!IsPrivilegeAccess(id, dataSource))
                    {
                        if (!retval.AccessIsGranted(user.User, dataSource))
                        {
                            Logger.Trace("SDApiController.GetProblem userID={0}, userName={1}, ID={2} failed (access denied)", user.Id, user.UserName, id);
                            return new GetProblemOutRFC() { RFC = null, Result = RequestResponceType.AccessError };
                        }
                        //
                        if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_RFC_Properties) && !user.IsClientView(dataSource))
                        {
                            Logger.Trace("SDApiController.GetProblem userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, id);
                            return new GetProblemOutRFC() { RFC = null, Result = RequestResponceType.OperationError };
                        }
                    }
                    //
                    return new GetProblemOutRFC() { RFC = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetProblem is NULL, id: '{0}'", id);
                return new GetProblemOutRFC() { RFC = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetProblem, id: '{0}'", id);
                return new GetProblemOutRFC() { RFC = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetRFC
        public sealed class GetRFCGanttOut
        {
            public IEnumerable<RFCGanttDetails> RFCGannt { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetRFCGanntList", Name = "GetRFCGanntList")]
        public async Task<GetRFCGanttOut> GetRFCGanntList(CancellationToken cancellationToken = default)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetRFCGanttOut() { RFCGannt = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetRFCGanttOut userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                var retval = await _irfcGanttBLL.GetAsync(cancellationToken);

                return new GetRFCGanttOut() { RFCGannt = retval, Result = RequestResponceType.Success };
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetRFCGanttOut is NULL");
                return new GetRFCGanttOut() { RFCGannt = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetRFCGanttOut is NULL");
                return new GetRFCGanttOut() { RFCGannt = null, Result = RequestResponceType.GlobalError };
            }
        }

        public sealed class GetRFCGanttViewSizeOut
        {
            public int? Size { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetRFCGanntViewSize", Name = "GetRFCGanntViewSize")]
        public async Task<GetRFCGanttViewSizeOut> GetRFCGanntViewSize(CancellationToken cancellationToken = default)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetRFCGanttViewSizeOut() { Size = null, Result = RequestResponceType.NullParamsError };
            Logger.Trace("SDApiController.GetRFCGanntViewSize userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                var retval = await _irfcGanttBLL.GetViewSizeAsync(cancellationToken);

                return new GetRFCGanttViewSizeOut() { Size = retval, Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetRFCGanntViewSize");
                return new GetRFCGanttViewSizeOut() { Size = null, Result = RequestResponceType.GlobalError };
            }
        }

        #endregion


        /*#region method GetSchemeInfoByObject
        [HttpGet]
        [Route("sdApi/GetSchemeInfoByObject", Name = "GetSchemeInfoByObject")]
        public SchemeInfo GetSchemeInfoByObject([FromQuery] Guid objectID)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetSchemeInfoByObject userID={0}, userName={1}, objectID={2}", user.Id, user.UserName, objectID);
            try
            {
                var retval = SchemeHelper.GetSchemeInfoByObject(IMSystem.Global.OBJ_Service, objectID);
                return retval;
            }
            catch (Exception e)
            {
                Logger.Error(e, "GetSchemeInfoByObject error");
                return null;
            }
        }
        #endregion*/

        #region method GetStateInfo

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/stateInfo/{classId:int}/{objectId}/")]
        public ObjectStateInfo GetObjectStateInfo(Guid objectId, int classId)
        {
            var user = CurrentUser;

            if (user?.User?.ID == null)
            {
                return null;
            }

            Logger.Trace("SDApiController.GetObjectStateInfo objectId={0}, classId={1}, userId={2}", objectId, classId, user.User.ID);

            try
            {
                return ObjectStateInfo.Get(objectId, classId, CurrentUser.User.ID);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }

        }

        #endregion

        public sealed class GeneralResponce<T>
        {
            public T Data { get; set; } = default(T);
            public RequestResponceType Result { get; set; } = RequestResponceType.BadParamsError;
        }

        public sealed class GeneralResponceList<T>
        {
            public List<T> Data { get; set; } = null;
            public RequestResponceType Result { get; set; } = RequestResponceType.BadParamsError;
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/GetNotificationList", Name = "GetNotificationList")]
        public ResultData<List<BaseForTable>> GetNotificationList(TableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.CallUsersList);
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/GetNotification", Name = "GetNotification")]
        public GeneralResponce<Notification> GetNotification(Guid id)
        {
            var user = base.CurrentUser;
            if (user == null) return new GeneralResponce<Notification>();

            var result = Notification.Get(id, user.User.ID, DataSource.GetDataSource());

            return new GeneralResponce<Notification>() { Data = result, Result = RequestResponceType.Success };
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/RemoveNotification", Name = "RemoveNotification")]
        public GeneralResponce<bool> RemoveNotification(DTL.ListInfoWithRowVersion model)
        {
            var user = base.CurrentUser;
            if (user == null) return new GeneralResponce<bool>() { Data = false };

            Notification.Remove(model, DataSource.GetDataSource());

            return new GeneralResponce<bool>() { Data = true, Result = RequestResponceType.Success };
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/RemoveNotificationRecipient", Name = "RemoveNotificationRecipient")]
        public GeneralResponce<bool> RemoveNotificationRecipient(DTL.SD.NotificationRecipient model)
        {
            var user = base.CurrentUser;
            if (user == null) return new GeneralResponce<bool>() { Data = false };

            Notification.RemoveRecipient(model, DataSource.GetDataSource());

            return new GeneralResponce<bool>() { Data = true, Result = RequestResponceType.Success };
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/RemoveNotificationUser", Name = "RemoveNotificationUser")]
        public GeneralResponce<bool> RemoveNotificationUser(DTL.SD.NotificationUser model)
        {
            var user = base.CurrentUser;
            if (user == null) return new GeneralResponce<bool>() { Data = false };

            Notification.RemoveUser(model, DataSource.GetDataSource());

            return new GeneralResponce<bool>() { Data = true, Result = RequestResponceType.Success };
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/SaveNotification", Name = "SaveNotification")]
        public GeneralResponce<bool> SaveNotification(DTL.SD.Notification model)
        {
            var user = base.CurrentUser;
            if (user == null) return new GeneralResponce<bool>() { Data = false };

            Notification.Save(model, user.User.ID, DataSource.GetDataSource());

            return new GeneralResponce<bool>() { Data = true, Result = RequestResponceType.Success };
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/AddNotificationRecipient", Name = "AddNotificationRecipient")]
        public GeneralResponce<bool> AddNotificationRecipient(DTL.SD.NotificationRecipient model)
        {
            var user = base.CurrentUser;
            if (user == null) return new GeneralResponce<bool>() { Data = false };

            Notification.AddRecipient(model, DataSource.GetDataSource());

            return new GeneralResponce<bool>() { Data = true, Result = RequestResponceType.Success };
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/AddNotificationUser", Name = "AddNotificationUser")]
        public GeneralResponce<bool> AddNotificationUser(DTL.SD.NotificationUser model)
        {
            var user = base.CurrentUser;
            if (user == null) return new GeneralResponce<bool>() { Data = false };

            Notification.AddUser(model, DataSource.GetDataSource());

            return new GeneralResponce<bool>() { Data = true, Result = RequestResponceType.Success };
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/NotificationExistsByName", Name = "NotificationExistsByName")]
        public GeneralResponce<bool> NotificationExistsByName(DTL.SD.Notification model)
        {
            var user = base.CurrentUser;
            if (user == null) return new GeneralResponce<bool>();

            var result = Notification.ExistsByName(model, DataSource.GetDataSource());

            return new GeneralResponce<bool>() { Data = result, Result = RequestResponceType.Success };
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/NotificationExistsById", Name = "NotificationExistsById")]
        public GeneralResponce<bool> NotificationExistsById(DTL.SD.Notification model)
        {
            var user = base.CurrentUser;
            if (user == null) return new GeneralResponce<bool>();

            var result = Notification.ExistsById(model, DataSource.GetDataSource());

            return new GeneralResponce<bool>() { Data = result, Result = RequestResponceType.Success };
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/GetNotificationRecipientList", Name = "GetNotificationRecipientList")]
        public GeneralResponceList<DTL.SD.NotificationRecipient> GetNotificationRecipientList(DTL.SD.Notification model)
        {
            var user = base.CurrentUser;
            if (user == null) return new GeneralResponceList<DTL.SD.NotificationRecipient>();

            var result = Notification.GetRecipientList(model, DataSource.GetDataSource());

            return new GeneralResponceList<DTL.SD.NotificationRecipient>() { Data = result, Result = RequestResponceType.Success };
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/GetNotificationUserList", Name = "GetNotificationUserList")]
        public GeneralResponceList<DTL.SD.NotificationUser> GetNotificationUserList(DTL.SD.Notification model)
        {
            var user = base.CurrentUser;
            if (user == null) return new GeneralResponceList<DTL.SD.NotificationUser>();

            var result = Notification.GetUserList(model, DataSource.GetDataSource());

            return new GeneralResponceList<DTL.SD.NotificationUser>() { Data = result, Result = RequestResponceType.Success };
        }

        //for hasmik...
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetProblemTypeNewAll", Name = "GetProblemTypeNewAll")]
        public GeneralResponce<List<DTL.SD.Problems.ProblemTypeNew>> GetProblemTypeNewAll()
        {
            var user = base.CurrentUser;
            if (user == null) return null;

            var list = BLL.SD.Problems.ProblemTypeNew.GetAll();

            if (list != null) return new GeneralResponce<List<DTL.SD.Problems.ProblemTypeNew>>() { Data = list, Result = RequestResponceType.Success };
            else return null;
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/GetProblemTypeNew", Name = "GetProblemTypeNew")]
        public GeneralResponce<ProblemTypeNew> GetProblemTypeNew(Guid id)
        {
            var user = base.CurrentUser;
            if (user == null) return new GeneralResponce<ProblemTypeNew>();

            var result = BLL.SD.Problems.ProblemTypeNew.Get(id, user.User.ID, DataSource.GetDataSource());

            return new GeneralResponce<ProblemTypeNew>() { Data = result, Result = RequestResponceType.Success };
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/GetProblemTypeNewList", Name = "GetProblemTypeNewList")]
        public ResultData<List<BaseForTable>> GetProblemTypeNewList(TableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.ProblemTypeNew);
        }

        //[HttpPost]
        //[AcceptVerbs("POST")]
        //[Route("sdApi/AddProblemTypeNew", Name = "AddProblemTypeNew")]
        //public GeneralResponce<bool> AddProblemTypeNew(DTL.SD.Problems.ProblemTypeNew model)
        //{
        //    try
        //    {
        //        using (DataSource dataSource = DataSource.GetDataSource())
        //        {
        //            var user = base.CurrentUser;
        //            if (user == null || model == null)
        //            {
        //                return new GeneralResponce<bool>() { Result = RequestResponceType.NullParamsError };
        //            }

        //            if (BLL.SD.Problems.ProblemTypeNew.ExistsByName(model, dataSource))
        //            {
        //                return new GeneralResponce<bool>() { Result = RequestResponceType.ExistsByName };
        //            }

        //            BLL.SD.Problems.ProblemTypeNew.Save(model, user.User.ID, DataSource.GetDataSource());
        //            return new GeneralResponce<bool>() { Data = true, Result = RequestResponceType.Success };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex, $"Ошибка при сохранении Типы проблем. probplemID = {model.ID}");
        //        return new GeneralResponce<bool>();
        //    }
        //}

        //[HttpPost]
        //[AcceptVerbs("POST")]
        //[Route("sdApi/RemoveProblemTypeNew", Name = "RemoveProblemTypeNew")]
        //public GeneralResponce<bool> RemoveProblemTypeNew(DTL.ListInfoWithRowVersion model)
        //{
        //    var user = base.CurrentUser;
        //    if (user == null) return new GeneralResponce<bool>();

        //    BLL.SD.Problems.ProblemTypeNew.Remove(model, DataSource.GetDataSource());

        //    return new GeneralResponce<bool>() { Data = true, Result = RequestResponceType.Success };
        //}


        #region method GetDashboardTreeItems
        [HttpGet]
        [Route("sdApi/GetDashboardTreeItems", Name = "GetDashboardTreeItems")]
        public async Task<IEnumerable<DashboardTreeItemDetails>> GetDashboardTreeItems([FromQuery] string parentID, CancellationToken cancellationToken)
        {
            var user = base.CurrentUser;
            if (user == null)
                return null;
            //
            Logger.Trace("SDApiController.GetDashboardTreeItems userID={0}, userName={1}, parentID='{2}'", user.Id, user.UserName, parentID ?? string.Empty);
            if (!user.User.HasRoles)
            {
                Logger.Warning("SDApiController.GetDashboardTreeItems userID={0}, userName={1}, parentID='{2}' failed (user is client)", user.Id, user.UserName, parentID ?? string.Empty);
                return null;
            }
            //
            Guid? id = null;
            Guid tempId;
            if (Guid.TryParse(parentID, out tempId))
                id = tempId;
            else if (!String.IsNullOrWhiteSpace(parentID))
                return null;
            //
            try
            {
                var retval = await _dashboardBll.GetTreeListAsync(id, user.User.ID, cancellationToken);

                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, String.Concat("Ошибка запроса элементов дерева панели статистики. ID родителя: ", parentID ?? string.Empty));
                return null;
            }
        }
        #endregion

        #region method GetDashboardXmlData
        [HttpGet]
        [Route("sdApi/getDashboardXmlData", Name = "GetDashboardXmlData")]
        public async Task<string> GetDashboardXmlData([FromQuery] string dashboardID)
        {
            var user = base.CurrentUser;
            if (user == null)
                return null;
            //
            Guid? id = null;
            Guid tempId;
            if (Guid.TryParse(dashboardID, out tempId))
                id = tempId;
            else if (!String.IsNullOrWhiteSpace(dashboardID))
                return null;
            //
            string xml = null;
            try
            {
                xml = await _getDashboard.GetAsync(id.Value);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, String.Concat("Ошибка запроса xml данных панели статистики. ID панели: ", dashboardID ?? string.Empty));
                return ex.Message;
            }
            //
            if (xml != null)
                try
                {
                    _dashboardHelper.ResetDashboard(dashboardID, xml);
                    _dashboardHelper.RegisterDashboard();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, String.Concat("Ошибка при обновлении панели статистики. ID панели: ", dashboardID ?? string.Empty));
                }
            else
                return Resources.ObjectDeleted;
            //
            return null;
        }
        #endregion
    }
}
