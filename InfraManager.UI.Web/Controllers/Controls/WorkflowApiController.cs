using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.BLL.Workflow;
using InfraManager.Core.Data;
using InfraManager.Core.Logging;
using InfraManager.Web.BLL.SD.Calls;
using InfraManager.Web.BLL.SD.MyWorkplace;
using InfraManager.Web.BLL.SD.Problems;
using InfraManager.Web.BLL.SD.RFC;
using InfraManager.Web.BLL.SD.WorkOrders;
using InfraManager.Web.DTL.Workflow;
using InfraManager.WebApi.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.ServiceBase.WorkflowService;
using Resources = InfraManager.ResourcesArea.Resources;
using InfraManager.BLL;

namespace InfraManager.Web.Controllers
{
    public class WorkflowApiController : BaseApiController
    {
        private readonly ICallBLL _calls;
        private readonly IWorkflowServiceApi _workflow;
        private readonly ICurrentUser _currentUser;

        public WorkflowApiController(ICallBLL calls, IWorkflowServiceApi workflow, ICurrentUser currentUser)
        {
            _calls = calls;
            _workflow = workflow;
            _currentUser = currentUser;
        }

        #region method GetStates
        [HttpGet]
        [Route("workflowApi/getStates", Name = "GetStates")]
        public async Task<WorkflowResponse<WorkflowInfo>> GetStates([FromQuery]Guid entityID, [FromQuery]int entityClassID, [FromQuery]string viewName, CancellationToken cancellationToken = default)
        {
            var call = await _calls.DetailsAsync(entityID, cancellationToken);
            var transitions = await _workflow.GetTransitionsAsync(entityID, entityClassID, _currentUser.UserId, cancellationToken);
            var states = await _workflow.GetStatesAsync(entityID, cancellationToken);
            return new WorkflowResponse<WorkflowInfo>
            {
                 Data = new WorkflowInfo 
                 { 
                     EntityStateName = call.EntityStateName, 
                     HasExternalEvents = false, 
                     StateList = states
                        .Where(x => transitions.Any(t => t.TargetStateID == x.ID))
                        .Select(
                            m => new WorkflowStateInfo
                            {
                                ImageBase64 = m.ImageBase64String, StateID = m.ID, Text = m.Name
                            })                        
                        .ToList()
                 },
                 Message = string.Empty, 
                 Result = WorkflowResponseType.Success
            };

            try
            {
                var user = base.CurrentUser;
                var userSettings = BLL.Settings.UserSettings.TryGetOrCreate(user.User, user.DefaultUserCulture);
                bool isEngineerMode = true;
                if (userSettings == null || (!user.User.HasRoles &&
                            (viewName == CommonForTable.VIEW_NAME ||
                            viewName == CallForTable.VIEW_NAME ||
                            viewName == ProblemForTable.VIEW_NAME ||
                            viewName == WorkOrderForTable.VIEW_NAME)) ||
                            (viewName == ClientCallForTable.VIEW_NAME ||
                            viewName == CommonWithNegotiationsForTable.VIEW_NAME ||
                            viewName == CustomControlForTable.VIEW_NAME ||
                            viewName == RFCForTable.VIEW_NAME))
                    isEngineerMode = false;
                //
                string entityStateName;
                var megaStates = BLL.WorkflowWrapper.GetStates(entityID, entityClassID, isEngineerMode, user.User, out entityStateName);
                var hasExternalEvents = BLL.WorkflowWrapper.IsExternalEventsExists(entityID);
                //
                var data = new WorkflowInfo() { StateList = megaStates.Data, HasExternalEvents = hasExternalEvents ?? true, EntityStateName = entityStateName };
                var retval = megaStates.Result == WorkflowResponseType.Success ? WorkflowResponse<WorkflowInfo>.Create(data) : WorkflowResponse<WorkflowInfo>.Create(megaStates.Result, megaStates.Message);
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return WorkflowResponse<WorkflowInfo>.Create(WorkflowResponseType.GlobalError, Resources.UnhandledErrorServer);
            }
        }
        #endregion

        #region method RemoveWorkflowScheme
        
        [HttpDelete("workflowApi/workflowScheme/{id}")]
        public async Task<bool> DeleteWorkflowSchemeAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            return await _workflow.DeleteWorkflowSchemeAsync(id, cancellationToken);
        }
        
        #endregion

        #region method GetList

        [HttpPost]
        [Route("workflowApi/workflowScheme/GetList", Name = "GetList")]
        public async Task<WorkflowSchemeModel[]> WorkflowSchemeGetList([FromBody] WorkflowSchemeFilter request,
            CancellationToken cancellationToken = default)
        {
            return await _workflow.WorkflowSchemeGetListAsync(request.StartRecordIndex, request.CountRecords,
                request.SearchString, request.ClassID, request.Status, cancellationToken);
        }
        
        #endregion

        #region method GetWorkflowSchemeProperties
        public sealed class GetWorkflowSchemePropertiesRequest
        {
            public Guid ID { get; set; }
            public bool withScheme { get; set; }
            public bool newScheme { get; set; }
        }

        [HttpPost]
        [Route("workflowApi/workflowScheme/GetProperties", Name = "GetWorkflowSchemeProperties")]
        public async Task<WorkflowSchemeModel> GetWorkflowSchemeProperties([FromBody] GetWorkflowSchemePropertiesRequest request, CancellationToken cancellationToken = default)
        {
            return await _workflow.GetWorkflowSchemePropertiesAsync(request.ID, request.withScheme, request.newScheme, cancellationToken);
        }

        #endregion

        #region method CopyWorkflowScheme

        [HttpPost]
        [Route("workflowApi/workflowScheme/Copy", Name = "Copy")]
        public async Task<Guid> CopyAsync([FromBody] WorkflowSchemeModel workflowScheme, CancellationToken cancellationToken = default)
        {
            return await _workflow.CopyWorkflowSchemeAsync(workflowScheme, cancellationToken);
        }
        #endregion

        #region method Ensure
        [HttpGet]
        [Route("workflowApi/workflow/Ensure", Name = "Ensure")]
        public async Task<bool> EnsureAsync(CancellationToken cancellationToken = default)
        {
            return await _workflow.EnsureAsync(cancellationToken);
        }
        #endregion

        #region WorkflowSchemeSave

        [HttpPost]
        [Route("workflowApi/workflowScheme/Save", Name = "Save")]
        public async Task<Guid> SaveWorkflowSchemePropertiesAsync([FromBody] WorkflowSchemeModel workflowScheme,
            CancellationToken cancellationToken = default)
        {
            return await _workflow.CreateOrSaveWorkflowSchemeAsync(workflowScheme, cancellationToken);
        }

        public sealed class WorkflowSchemeSave
        {
            public WorkflowSchemeModel WorkflowScheme { get; set; }
            public bool NewScheme { get; set; }
        }

        [HttpPost]
        [Route("workflowApi/workflowScheme/SaveScheme", Name = "SaveScheme")] //saving only scheme
        public async Task<Guid> SaveWorkflowSchemeAsync([FromBody] WorkflowSchemeSave request)
        {
            return await _workflow.SaveWorkflowSchemeAsync(request.WorkflowScheme, request.NewScheme,
                CancellationToken.None);
        }

        #endregion

        //#region method IsExternalEventsExists
        //[HttpGet]
        //[Route("workflowApi/isExternalEventsExists", Name = "IsExternalEventsExists")]
        //public bool? IsExternalEventsExists([FromQuery]Guid entityID)
        //{
        //    try
        //    {
        //        var retval = BLL.WorkflowWrapper.IsExternalEventsExists(entityID);
        //        return retval;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //        return null;//for lock ui
        //    }
        //}
        //#endregion

        #region method SetState
        [HttpPost]
        [Route("workflowApi/setState", Name = "SetState")]
        public WorkflowResponse SetState([FromQuery]Guid entityID, [FromQuery]int entityClassID, [FromQuery]string targetStateID)
        {
            try
            {
                var retval = BLL.WorkflowWrapper.SetState(entityID, entityClassID, targetStateID, false, CurrentUser.User);
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return WorkflowResponse.Create(WorkflowResponseType.GlobalError, Resources.UnhandledErrorServer);
            }
        }
        #endregion

        #region method SetStateMobile
        [HttpPost]
        [Route("workflowApi/setStateMobile", Name = "SetStateMobile")]
        //Временно используется в мобильном вебе пока там не работает signalR
        public WorkflowResponse SetStateMobile([FromQuery]Guid entityID, [FromQuery]int entityClassID, [FromQuery]string targetStateID)
        {
            try
            {
                var retval = BLL.WorkflowWrapper.SetStateMobile(entityID, entityClassID, targetStateID, false, CurrentUser.User);
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return WorkflowResponse.Create(WorkflowResponseType.GlobalError, Resources.UnhandledErrorServer);
            }
        }
        #endregion

        #region method RestartWorkflow
        [HttpPost]
        [Route("workflowApi/restartWorkflow", Name = "RestartWorkflow")]
        public WorkflowResponse RestartWorkflow([FromQuery]Guid entityID, [FromQuery]int entityClassID)
        {
            try
            {
                var retval = BLL.WorkflowWrapper.RestartWorkflow(entityID, entityClassID, CurrentUser.User);
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return WorkflowResponse.Create(WorkflowResponseType.GlobalError, Resources.UnhandledErrorServer);
            }
        }
        #endregion

        #region method OnSave
        [HttpPost]
        [Route("workflowApi/onSave", Name = "OnSave")]
        public WorkflowResponse OnSave([FromQuery]Guid entityID, [FromQuery]int entityClassID)
        {
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = BLL.WorkflowWrapper.OnSave(entityID, entityClassID, false, dataSource, base.CurrentUser?.User);
                    return retval;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return WorkflowResponse.Create(WorkflowResponseType.GlobalError, Resources.UnhandledErrorServer);
            }
        }
        #endregion

        
    }
}
