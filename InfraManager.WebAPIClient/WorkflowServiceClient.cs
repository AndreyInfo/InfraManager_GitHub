using InfraManager.BLL.Workflow;
using InfraManager.ComponentModel;
using InfraManager.Core.Data;
using InfraManager.ServiceBase.WorkflowService.WebAPIModels;
using InfraManager.Services;
using InfraManager.Services.WorkflowService;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL;
using InfraManager.ServiceBase.WorkflowService;
using System.Net.Http;
using static InfraManager.BLL.Workflow.IWorkflowServiceApi;

namespace InfraManager.WebAPIClient
{
    public class WorkflowServiceClient : WebAPIBaseClient, IWorkflowService, IWorkflowServiceApi
    {
        private const string CreateWorkflowPath = "workflow/create-workflow";
        
        private readonly DataSourceLink _dataSourceLink;

        public WorkflowServiceClient(string baseUrl, DataSourceLink dataSourceLink) 
            : base(baseUrl)
        {
            _dataSourceLink = dataSourceLink;
        }

        #region IWorkflowServiceApi

        public async Task<TransitionIsAllowedResult> TransitionIsAllowedAsync(
            Guid entityID, 
            int entityClassID, 
            string stateID, 
            Guid identityID, 
            CancellationToken cancellationToken = default)
        {
            WorkflowResultWithBoolValue response = null;

            try
            {
                response = await PostAsync<WorkflowResultWithBoolValue, WorkflowEntityStateRequest2>(
                    "workflow/transition-isallowed",
                    new WorkflowEntityStateRequest2
                    {
                        EntityStateID = stateID,
                        ClassID = entityClassID,
                        EntityID = entityID,
                        IdentityID = identityID, 
                        DataSourceLink = _dataSourceLink
                    });
            }
            catch(HttpRequestException ex)
            {
                throw new WebApiException(ex.Message);
            }

            if (response.OperationResult.Type != OperationResultType.Success)
            {
                return new TransitionIsAllowedResult
                {
                    IsAllowed = false,
                    Message = response.OperationResult.Message
                };
            }

            return new TransitionIsAllowedResult
            {
                IsAllowed = true
            };
        }

        public async Task<WorkflowSchemeModel> CreateWorkflowAsync(Guid id, string workflowSchemeIdentifier, CancellationToken cancellationToken = default)
        {
            WorkflowResultWithSchemaInfo result = null;
            try
            {
                result = await PostAsync<WorkflowResultWithSchemaInfo, WorkflowSchemeIdentifierRequest>(
                CreateWorkflowPath,
                new WorkflowSchemeIdentifierRequest
                {
                    DataSourceLink = _dataSourceLink,
                    EntityID = id,
                    WorkflowSchemeIdentifier = workflowSchemeIdentifier,
                }, cancellationToken: cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                throw new WebApiException(ex.Message);
            }
            

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new WebApiException($"Workflow api error {result.OperationResult.Message}");
            }

            return new WorkflowSchemeModel
            {
                ID = result.WorkflowSchemeID,
                Identifier = workflowSchemeIdentifier,
                Name = null,
                Version = result.WorkflowSchemeVersion
            };
        }

        public async Task<WorkflowStateModel[]> GetInitialStatesAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var result = await PostAsync<WorkflowResultWithStateInfos, WorkflowSchemeIdentifierRequest>("workflow/get-initial-states-by-entity", new WorkflowSchemeIdentifierRequest()
            {
                DataSourceLink = _dataSourceLink,
                EntityID = entityId,
            }, cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }

            return result.ToModel();
        }

        public async Task<WorkflowStateModel[]> GetStatesAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var result = await PostAsync<WorkflowResultWithStateInfos, WorkflowEntityRequest>(
                "workflow/get-states-by-entity",
                new WorkflowEntityRequest
                {
                    DataSourceLink = _dataSourceLink,
                    EntityID = entityId,
                });

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }

            return result.ToModel();
        }

        public async Task<bool> HasExternalEventsAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var result = await PostAsync<WorkflowResultWithBoolValue, WorkflowEntityRequest>(
                "workflow/has-entity-external-events",
                new WorkflowEntityRequest
                {
                    DataSourceLink = _dataSourceLink,
                    EntityID = entityId,
                });

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }

            return result.Value;
        }

        public async Task<TransitionInfo[]> GetTransitionsAsync(Guid entityId, int classId, Guid identityID, CancellationToken cancellationToken = default)
        {
            var req = new WorkflowWorkflowEntityRequest2()
            {
                DataSourceLink = _dataSourceLink,
                IdentityID = identityID,
                ClassID = classId,
                EntityID = entityId
            };
            WorkflowResultWithTransitionInfos result = null;
            try
            {
                result = await PostAsync<WorkflowResultWithTransitionInfos, WorkflowWorkflowEntityRequest2>(
                    "workflow/get-transitions", req, cancellationToken: cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                throw new WebApiException(ex.Message);
            }

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new WebApiException($"Workflow api error: {result.OperationResult.Message}");
            }

            return result.Transitions.ToArray();
        }

        public async Task<bool> DeleteWorkflowSchemeAsync(Guid workflowSchemeID, CancellationToken cancellationToken = default)
        {
            var task = await PostAsync<WorkflowResultBase, WorkflowSchemeEntityRequest>("workflowScheme/delete-byid", new WorkflowSchemeEntityRequest()
            {
                DataSourceLink = _dataSourceLink,
                EntityID = workflowSchemeID
            }, cancellationToken: cancellationToken);

            if (task.OperationResult.Type != OperationResultType.Success)
            {
                throw new InvalidObjectException(task.OperationResult.Message);
            }

            return true;
        }

        public Task<Guid> InsertWorkflowSchemeAsync(WorkflowSchemeModel workflowScheme, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> SaveWorkflowSchemeAsync(WorkflowSchemeModel workflowScheme, bool newScheme, CancellationToken cancellationToken = default)
        {
            var result = await PostAsync<WorkflowResultWithGuid, WorkflowSchemeRequest.WorkflowSchemeSaveRequest>(
                "/WorkflowScheme/save",
                new WorkflowSchemeRequest.WorkflowSchemeSaveRequest
                {
                    DataSourceLink = _dataSourceLink,       
                    WorkflowSchemeModel = workflowScheme,
                    NewScheme = newScheme
                }, cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new InvalidObjectException(result.OperationResult.Message);
            }
            return result.id;
        }

        public async Task<WorkflowSchemeModel[]> WorkflowSchemeGetListAsync(int skip, int take, string searchString,
            int? classID, byte? status, CancellationToken cancellationToken)
        {
            var result = await PostAsync<WorkflowResultWithWorkflowShemesModels, WorkflowGetListRequest>(
                "/WorkflowScheme/GetList",
                new WorkflowGetListRequest
                {
                    DataSourceLink = _dataSourceLink,
                    SearchString = searchString,
                    Skip = skip,
                    Take = take,
                    ClassID = classID,
                    Status = status
                }, cancellationToken: cancellationToken);
            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }

            return result.WorkflowSchemeModels;
        }


        public async Task<WorkflowSchemeModel> GetWorkflowSchemePropertiesAsync(Guid id, bool withScheme, bool newScheme, CancellationToken cancellationToken)
        {
            var result = await PostAsync<WorkflowResultWithWorkflowShemesModels, WorkflowSchemeProperties>(
               "/WorkflowScheme/Properties",
               new WorkflowSchemeProperties
               {
                   DataSourceLink = _dataSourceLink,
                   EntityID = id,
                   withScheme = withScheme,
                   newScheme = newScheme
               }, cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }

            return result.WorkflowSchemeModels[0];
        }

        public async Task<WorkflowSchemeModel> GetWorkFlowSchemeByIdentifierAsync(string identifier, CancellationToken cancellationToken)
        {
            var result = await GetAsync<WorkflowResultWithWorkflowShemesModels>(
                $"WorkflowScheme/{identifier}/Published", cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }
            
            return result.WorkflowSchemeModels[0];
        }

        public async Task<Guid> CopyWorkflowSchemeAsync(WorkflowSchemeModel workflowScheme, CancellationToken cancellationToken)
        {
            var result = await PostAsync<WorkflowSchemeRequest.WorkflowSchemeCreateResponse, WorkflowSchemeWithModel>(
               "/WorkflowScheme/Copy",
               new WorkflowSchemeWithModel
               {
                 WorkflowSchemeModel = workflowScheme,
                 DataSourceLink = _dataSourceLink
               }, cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }

            return result.WorkflowSchemeID;
        }

        public async Task<bool> EnsureAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await GetAsync<OperationResult>(
                    "/Workflow/Ensure",
                    null,
                    cancellationToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public async Task<Guid> CreateOrSaveWorkflowSchemeAsync(WorkflowSchemeModel workflowScheme, CancellationToken cancellationToken = default)
        {
            var result = await PostAsync<WorkflowSchemeRequest.WorkflowSchemeCreateResponse, WorkflowSchemeWithModel>(
                "/WorkflowScheme",
                new WorkflowSchemeWithModel
                {
                    DataSourceLink = _dataSourceLink,
                    WorkflowSchemeModel = workflowScheme
                }, cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new InvalidObjectException(result.OperationResult.Message);
            }

            return result.WorkflowSchemeID;
        }

        public async Task PublishWorkflowSchemeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await PostAsync<WorkflowResultBase, WorkflowSchemeIDRequest>(
                "/WorkflowScheme/Publish",
                new WorkflowSchemeIDRequest
                {
                    DataSourceLink = _dataSourceLink,
                    SchemeID = id
                }, cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success) throw new Exception("Workflow api error");
        }

        public async Task ChangeConnectionStringAsync(string server, int port, string dataBase, string login, string password,
            string additionalField, CancellationToken cancellationToken = default)
        {
            var result = await PostAsync<WorkflowResultWithBoolValue, ConnectionStringChangeRequest>(
                "/Workflow/ConnectionString",
                new ConnectionStringChangeRequest
                {
                    DataSourceLink = _dataSourceLink,
                    Server = server,
                    Login = login,
                    Password = password,
                    Database = dataBase,
                    Port = port,
                    AdditionalField = additionalField
                }, cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }
        }

        public async Task<WorkflowTrackingModel[]> GetWorkflowTrackingListAsync(int skip, int take, string searchString, CancellationToken cancellationToken = default)
        {
            var result = await GetAsync<WorkflowResultWithWorkflowTrackings, WorkflowPaggingRequest>(
                "/api/WorkflowTrackings",
                new WorkflowPaggingRequest
                {
                    Skip = skip,
                    Take = take,
                    SearchString = searchString
                }, cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }

            return result.WorkflowTrackings;
        }

        public async Task<WorkflowTrackingModel> GetWorkflowTrackingAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await GetAsync<WorkflowResultWithWorkflowTrackings>(
                $"/api/WorkflowTrackings/{id}", cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }

            return result.WorkflowTrackings[0];
        }

        public async Task<WorkflowEvent[]> GetWorkflowTrackingEventsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await GetAsync<WorkflowResultWithWorkflowEvents>(
                $"/api/WorkflowTrackings/{id}/Events", cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }

            return result.Messages;
        }

        public async Task<WorkflowStateTracking[]> GetWorkflowStateTrackingsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await GetAsync<WorkflowResultWithWorkflowStates>(
                $"/api/WorkflowTrackings/{id}/States", cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }

            return result.States;
        }

        public async Task<string> GetDebugInfoAsync(Guid id, Guid? currentUserID = null, CancellationToken cancellationToken = default)
        {
            var result = await PostAsync<WorkflowResultWithStringValue, WorkflowEntityRequest>(
                "/Workflow/DebugInfo",
                new WorkflowEntityRequest
                {
                    DataSourceLink = _dataSourceLink,
                    EntityID = id,
                    CurrentUser = new UserModel { ID = currentUserID ?? Guid.Empty }
                }, 
                cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new InvalidObjectException(result.OperationResult.Message);
            }

            return result.Value;
        }

        public async Task RestartWorkflowAsync(int entityClassID, Guid id, Guid? currentUserID,
            CancellationToken cancellationToken = default)
        {
            var result = await PostAsync<WorkflowResultWithSchemaInfo, WorkflowEntityClassRequest>(
                "/Workflow/restart-workflow",
                new WorkflowEntityClassRequest
                {
                    DataSourceLink = _dataSourceLink,
                    EntityID = id,
                    EntityClassID = entityClassID,
                    CurrentUser = new UserModel { ID = currentUserID ?? Guid.Empty }
                }, 
                cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }
        }

        public async Task DeleteWorkflowWithClearingAsync(int entityClassID, Guid id, Guid? currentUserID,
            CancellationToken cancellationToken = default)
        {
            var result = await PostAsync<WorkflowResultBase, WorkflowEntityClassRequest>(
                "/Workflow/DeleteWithClearing",
                new WorkflowEntityClassRequest
                {
                    DataSourceLink = _dataSourceLink,
                    EntityID = id,
                    EntityClassID = entityClassID,
                    CurrentUser = new UserModel { ID = currentUserID ?? Guid.Empty }
                }, 
                cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }
        }

        public async Task<WorkflowResultExport> ExportAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await PostAsync<WorkflowResultExport, WorkflowEntityRequest>(
                "/WorkflowScheme/Export",
                new WorkflowEntityRequest
                {
                    DataSourceLink = _dataSourceLink,
                    EntityID = id
                }, 
                cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }

            return result;
        }

        public async Task ImportAsync(string packedWF, string name, string description,
            CancellationToken cancellationToken = default)
        {
            var result = await PostAsync<WorkflowResultBase, WorkflowImportRequest>(
                "/WorkflowScheme/Import",
                new WorkflowImportRequest
                {
                    DataSourceLink = _dataSourceLink,
                    PackedWF = packedWF,
                    Name = name,
                    Descriptions = description
                }, 
                cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new InvalidObjectException(result.OperationResult.Message);
            }
        }

        public async Task<WorkflowEvent[]> GetWorkflowLastActionsAsync(DateTime? utcStartDate, CancellationToken cancellationToken = default)
        {
            var result = await GetAsync<WorkflowResultWithWorkflowEvents>(
                $"/Workflow/last-actions/{utcStartDate}", 
                cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }

            return result.Messages;
        }

        public async Task<ExternalEventModel[]> GetWorkflowExternalEvents(CancellationToken cancellationToken = default)
        {
            var result = await GetAsync<WorkflowResultWithExternalEvents>(
                "/Workflow/ExternalEvents", 
                cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }

            return result.ExternalEvents;
        }

        public async Task InformExternalEventAsync(CancellationToken cancellationToken = default)
        {
            var result = await GetAsync<WorkflowResultBase>(
                "/Workflow/inform-external", 
                cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }
        }

        public async Task RemoveRedundantWorkflowAsync(CancellationToken cancellationToken = default)
        {
            var result = await PostAsync<WorkflowResultBase>(
                "/Workflow/RemoveRedundantWorkflow", 
                cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }
        }

        public async Task RestartServiceAsync(CancellationToken cancellationToken = default)
        {
            var result = await GetAsync<WorkflowResultBase>(
                "/Workflow/restart", 
                cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }
        }

        public async Task EntityModifiedAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await PostAsync<WorkflowResultBase>(
                "/Workflow/EntityModifiedAll", 
                cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }
        }

        public async Task DeleteEntityOrEnvironmentEventAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await DeleteAsync<WorkflowResultBase>($"/Workflow/DeleteEvent/{id}",
                cancellationToken: cancellationToken);
                
            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }
        }

        #endregion

        public OperationResult SaveWorkflowScheme(DataSourceLink dataSourceLink, WorkflowSchemeModel workflowScheme,
            out Guid id, bool newScheme)
        {
            id = Guid.Empty;
            var task = PostAsync<WorkflowResultBase, WorkflowSchemeRequest.WorkflowSchemeSaveRequest>("workflowScheme/Save", new WorkflowSchemeRequest.WorkflowSchemeSaveRequest()
            {
                DataSourceLink = _dataSourceLink,
                NewScheme = newScheme,
                WorkflowSchemeModel = workflowScheme
            });

            task.Wait();

            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result.OperationResult;
            }

            return new OperationResult() { 
                Type = OperationResultType.Failure, 
                Message = task.Exception?.Message ?? "Fail to call WorkflowService"
            };
        }

        public OperationResult GetWorkflowSchemeList(DataSourceLink dataSourceLink, int skip, int take, string searhString,
            int? classID, byte? status, out List<WorkflowSchemeModel> workflowSchemeModels)
        {
            throw new NotImplementedException();
        }

        public OperationResult CreateWorkflow(DataSourceLink dataSourceLink, string workflowSchemeIdentifier, Guid entityID, out Guid workflowSchemeID, out string workflowSchemeVersion)
        {
            var task = PostAsync<WorkflowResultWithSchemaInfo, WorkflowSchemeIdentifierRequest>(CreateWorkflowPath, new WorkflowSchemeIdentifierRequest()
            {
                DataSourceLink = dataSourceLink,
                EntityID= entityID,
                WorkflowSchemeIdentifier = workflowSchemeIdentifier,
            });
            task.Wait();
            if(task.IsCompleted && !task.IsFaulted)
            {
                workflowSchemeID = task.Result.WorkflowSchemeID;
                workflowSchemeVersion = task.Result.WorkflowSchemeVersion;
                return task.Result.OperationResult;
            }
            workflowSchemeID = Guid.Empty;
            workflowSchemeVersion = string.Empty;
            return new OperationResult() {Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call WorkflowService" };
        }

        public OperationResult CreateWorkflowBySchemeID(DataSourceLink dataSourceLink, Guid workflowSchemeID, Guid entityID)
        {
            var task = PostAsync<WorkflowResultBase, WorkflowSchemeIDRequest>("workflow/create-byid", new WorkflowSchemeIDRequest()
            {
                DataSourceLink = dataSourceLink,
                EntityID = entityID,
                SchemeID = workflowSchemeID,
            });
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result.OperationResult;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call WorkflowService" };
        }

        public OperationResult DeleteWorkflow(DataSourceLink dataSourceLink, Guid entityID)
        {
            var task = PostAsync<WorkflowResultBase, WorkflowEntityRequest>("workflow/delete-byid", new WorkflowEntityRequest()
            {
                DataSourceLink = dataSourceLink,
                //не потерялся ли ID сущности для удаления?
            });
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result.OperationResult;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call WorkflowService" };
        }

        public OperationResult EnsureAvailability()
        {
            var task = GetAsync<OperationResult>("workflow/ensure");
            task.Wait();
            if(task.IsCompleted && !task.IsFaulted)
            {
                return task.Result;
            }
            return new OperationResult() {Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call WorkflowService" };
        }

        public void ForceWorkflowSchemeUpdate(DataSourceLink dataSourceLink)
        {
            var task = PostAsync<OperationResult, WorkflowRequest>("workflow/update", new WorkflowRequest()
            {
                DataSourceLink = dataSourceLink,
            });

            task.Wait();

            if (task.IsCompleted && !task.IsFaulted)
            {
                return;
            }
            throw new ApplicationException(task.Exception?.Message ?? "Fail to call WorkflowService");
        }

        public OperationResult GetInitialStates(DataSourceLink dataSourceLink, string workflowSchemeIdentifier, out List<StateInfo> initialStates)
        {
            var task = PostAsync<WorkflowResultWithStateInfos, WorkflowSchemeIdentifierRequest>("workflow/get-initial-states-by-scheme", new WorkflowSchemeIdentifierRequest()
            {
                DataSourceLink = dataSourceLink,
                WorkflowSchemeIdentifier = workflowSchemeIdentifier,
            });
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                initialStates = task.Result.States;
                return task.Result.OperationResult;
            }
            initialStates = null;
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call WorkflowService" };
        }

        public OperationResult GetInitialStates(DataSourceLink dataSourceLink, Guid entityID, out List<StateInfo> initialStates)
        {
            var task = PostAsync<WorkflowResultWithStateInfos, WorkflowSchemeIdentifierRequest>("workflow/get-initial-states-by-entity", new WorkflowSchemeIdentifierRequest()
            {
                DataSourceLink = dataSourceLink,
                EntityID = entityID,
            });
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                initialStates = task.Result.States;
                return task.Result.OperationResult;
            }
            initialStates = null;
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call WorkflowService" };
        }

        public OperationResult GetLastActions(DateTime? utcStartDate, out WorkflowEvent[] messages)
        {
            var task = GetAsync<WorkflowResultWithWorkflowEvents>($"workflow/last-actions/{utcStartDate}");
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                messages = task.Result.Messages;
                return task.Result.OperationResult;
            }
            messages = null;
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call WorkflowService" };
        }

        public OperationResult GetStates(DataSourceLink dataSourceLink, string workflowSchemeIdentifier, out List<StateInfo> states)
        {
            var task = PostAsync<WorkflowResultWithStateInfos, WorkflowSchemeIdentifierRequest>("workflow/get-states-by-schema", new WorkflowSchemeIdentifierRequest()
            {
                DataSourceLink = dataSourceLink,
                WorkflowSchemeIdentifier = workflowSchemeIdentifier,
            });
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                states = task.Result.States;
                return task.Result.OperationResult;
            }
            states = null;
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call WorkflowService" };
        }

        public OperationResult GetStates(DataSourceLink dataSourceLink, Guid entityID, out List<StateInfo> states)
        {
            var req = new WorkflowEntityRequest()
            {
                DataSourceLink = dataSourceLink,
                EntityID = entityID,
            };
            var task = PostAsync<WorkflowResultWithStateInfos, WorkflowEntityRequest>("workflow/get-states-by-entity", req);
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                states = task.Result.States;
                return task.Result.OperationResult;
            }
            states = null;
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call WorkflowService" };
        }

        public OperationResult GetTransitions(DataSourceLink dataSourceLink, Guid identityID, IWorkflowEntity entity, out List<TransitionInfo> transitions)
        {
            var req = new WorkflowWorkflowEntityRequest()
            {
                DataSourceLink = dataSourceLink,
                IdentityID = identityID,
                WorkflowEntity = entity,
            };
            var task = PostAsync<WorkflowResultWithTransitionInfos, WorkflowWorkflowEntityRequest>("workflow/get-transitions", req);
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                transitions = task.Result.Transitions;
                return task.Result.OperationResult;
            }
            transitions = null;
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call WorkflowService" };
        }

        public OperationResult GetWorkflowDebugInfo(DataSourceLink dataSourceLink, Guid entityID, Guid? currentUserID = null)
        {
            var req = new WorkflowEntityRequest()
            {
                DataSourceLink = dataSourceLink,
                EntityID = entityID,
            };
            var task = PostAsync<WorkflowResultWithSchemaInfo, WorkflowEntityRequest>("workflow/get-debuginfo", req);
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result.OperationResult;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call WorkflowService" };
        }

        public void InformClientsEntityEvent(Guid applicationID, EventType eventType, Guid entityID, int entityClassID, DataSourceLink dataSourceLink)
        {
            var req = new WorkflowInformClientsRequest()
            {
                DataSourceLink = dataSourceLink,
                ApplicationID = applicationID,
                EntityClassID = entityClassID,
                EntityID = entityID,
                EventType = eventType,
            };
            var task = PostAsync<WorkflowResultBase, WorkflowInformClientsRequest>("workflow/inform-clients", req);
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return;
            }
            throw new ApplicationException(task.Exception?.Message ?? "Fail to call WorkflowService");
        }

        public void InformExternalEvent()
        {
            var task = GetAsync<WorkflowResultBase>("workflow/inform-external");
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return;
            }
            throw new ApplicationException(task.Exception?.Message ?? "Fail to call WorkflowService");
        }

        public void InformExternalEvent(DataSourceLink dataSourceLink)
        {
            var req = new WorkflowRequest()
            {
                DataSourceLink = dataSourceLink,
            };
            var task = PostAsync<WorkflowResultBase, WorkflowRequest>("workflow/inform-external", req);
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return;
            }
            throw new ApplicationException(task.Exception?.Message ?? "Fail to call WorkflowService");
        }

        public OperationResult RestartService()
        {
            var task = GetAsync<WorkflowResultBase>($"workflow/restart");
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result.OperationResult;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call WorkflowService" };
        }

        public OperationResult RestartWorkflow(DataSourceLink dataSourceLink, Guid entityID, int entityClassID, out Guid workflowSchemeID, out string workflowSchemeVersion, IUserIdentifier currentUser)
        {
            var req = new WorkflowEntityClassRequest()
            {
                CurrentUser = new UserModel { ID = currentUser.ID },
                DataSourceLink = dataSourceLink,
                EntityID = entityID,
                EntityClassID = entityClassID,
            };
            var task = PostAsync<WorkflowResultWithSchemaInfo, WorkflowEntityClassRequest>("workflow/restart-workflow", req);
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                workflowSchemeID = task.Result.WorkflowSchemeID;
                workflowSchemeVersion = task.Result.WorkflowSchemeVersion;
                return task.Result.OperationResult;
            }
            workflowSchemeVersion = string.Empty;
            workflowSchemeID = Guid.Empty;
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call WorkflowService" };
        }

        public OperationResult SetWorkflowVariable(DataSourceLink dataSourceLink, Guid entityID, string variableName, object variableValue)
        {
            var req = new WorkflowVariableRequest()
            {
                DataSourceLink = dataSourceLink,
                EntityID = entityID,
                VariableName = variableName,
                VariableValue = variableValue,
            };
            var task = PostAsync<WorkflowResultBase, WorkflowVariableRequest>("workflow/set-variable", req);
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result.OperationResult;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call WorkflowService" };
        }

        public OperationResult Subscribe(Guid applicationID)
        {
            var task = GetAsync<WorkflowResultBase>($"workflow/subscribe/{applicationID}");
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result.OperationResult;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call WorkflowService" };
        }

        public OperationResult TransitionIsAllowed(DataSourceLink dataSourceLink, Guid identityID, string entityStateID, IWorkflowEntity entity, out bool isAllowed)
        {
            throw new NotSupportedException("По http это работать не будет.");
        }

        public OperationResult Unsubscribe(Guid applicationID)
        {
            var task = GetAsync<WorkflowResultBase>($"workflow/unsubscribe/{applicationID}");
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result.OperationResult;
            }
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call WorkflowService" };
        }

        public OperationResult WorkflowIsReadOnlyExtended(DataSourceLink dataSourceLink, IWorkflowEntity entity, bool checkWorkflowAvailable, bool checkLane, bool checkIsTerminalState, out bool isReadOnly)
        {
            var req = new WorkflowIsReadOnlyRequest()
            {
                DataSourceLink = dataSourceLink,
                WorkflowEntity = new WorkflowEntityModel(entity),
                checkIsTerminalState = checkIsTerminalState,
                checkLane = checkLane,
                checkWorkflowAvailable = checkWorkflowAvailable,
            };
            var task = PostAsync<WorkflowResultWithBoolValue, WorkflowIsReadOnlyRequest>("workflow/transition-isreadonly", req);
            task.Wait();
            if (task.IsCompleted && !task.IsFaulted)
            {
                isReadOnly = task.Result.Value;
                return task.Result.OperationResult;
            }
            isReadOnly = true;
            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call WorkflowService" };
        }

        public OperationResult DeleteWorkflowScheme(DataSourceLink dataSourceLink, Guid workflowSchemeID)
        {
            var task = PostAsync<WorkflowResultBase, WorkflowSchemeEntityRequest>("workflowScheme/delete-byid", new WorkflowSchemeEntityRequest()
            {
                DataSourceLink = dataSourceLink,
                EntityID = workflowSchemeID
            });

            task.Wait();

            if (task.IsCompleted && !task.IsFaulted)
            {
                return task.Result.OperationResult;
            }

            return new OperationResult() { Type = OperationResultType.Failure, Message = task.Exception?.Message ?? "Fail to call WorkflowService" };
        }

        public OperationResult GetTransitions(DataSourceLink dataSourceLink, Guid identityID, int entityClassId, Guid entityID, out List<TransitionInfo> transitions)
        {
            throw new NotImplementedException();
        }

        public OperationResult GetWorkflowSchemeByIdentifier(string identifier, out List<WorkflowSchemeModel> workflowSchemes)
        {
            throw new NotImplementedException();
        }

        public OperationResult GetWorkflowSchemeProperties(Guid id, bool withScheme, bool newScheme, DataSourceLink dataSourceLink, out WorkflowSchemeModel workflowSchemeModel)
        {
            throw new NotImplementedException();
        }

        public OperationResult CopyWorkflowScheme(WorkflowSchemeModel workflowScheme, DataSourceLink dataSourceLink,
            out Guid workflowSchemeID)
        {
            throw new NotImplementedException();
        }
        
        public OperationResult CreateOrSaveWorkflowScheme(WorkflowSchemeModel workflowSchemeModel, out Guid workflowSchemeID)
        {
            throw new NotImplementedException();
        }

        public OperationResult PublishWorkflowScheme(Guid id)
        {
            throw new NotImplementedException();
        }

        public OperationResult DeleteWorkflowWithClearing(DataSourceLink dataSourceLink, Guid entityID, int entityClassID,
            IUserIdentifier currentUser)
        {
            throw new NotImplementedException();
        }

        public OperationResult HasExternalEvents(DataSourceLink dataSourceLink, Guid entityID, out bool isExists)
        {
            throw new NotImplementedException();
        }

        public OperationResult Export(Guid id, out string packedWFScheme, out int majorVersion, out int minorVersion, out string name)
        {
            throw new NotImplementedException();
        }

        public OperationResult Import(string packedWF, string name, string description)
        {
            throw new NotImplementedException();
        }

        public OperationResult TransitionIsAllowed(DataSourceLink dataSourceLink, Guid identityID, string entityStateID, Guid entityID, int entityClassID, out bool isAllowed)
        {
            throw new NotImplementedException();
        }

        public OperationResult GetTransitions(DataSourceLink dataSourceLink, Guid identityID, int entityClassId, Guid entityID, Guid userID, out List<TransitionInfo> transitions)
        {
            throw new NotImplementedException();
        }

        public OperationResult WorkflowIsReadonly(DataSourceLink dataSourceLink, Guid entityID, int entityClassID, Guid userID, bool checkWorkflowAvailable, bool checkLane, bool checkIsTerminalState, out bool isReadonly)
        {
            throw new NotImplementedException();
        }

        public OperationResult RestartAs(DataSourceLink dataSourceLink, Guid entityID, int entityClassID, Guid workflofSchemeID,
            IUserIdentifier currentUser)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> WorkflowIsReadonlyAsync(Guid id, int classID, Guid userID, CancellationToken cancellationToken = default)
        {
            var req = new WorkflowIsReadonlyRequest2
            {
                DataSourceLink = _dataSourceLink,
                EntityID = id,
                EntityClassID = classID,
                CurrentUser = new UserModel { ID = userID }
            };
            var result = await PostAsync<WorkflowResultWithBoolValue, WorkflowIsReadonlyRequest2>("workflow/workflow-isreadonly", req);

            return result.OperationResult.Type == OperationResultType.Success
                ? result.Value
                : throw new Exception($"Workflow API error: {result.OperationResult.Message}.");
        }

        public async Task RestartAsAsync(Guid workflowSchemeID, int entityClassID, Guid entityID, Guid currentUser,
            CancellationToken cancellationToken = default)
        {
            var result = await PostAsync<WorkflowResultBase, WorkflowRestartAsRequest>(
                "/Workflow/RestartAs",
                new WorkflowRestartAsRequest
                {
                    DataSourceLink = _dataSourceLink,
                    CurrentUser = new UserModel {ID = currentUser},
                    EntityID = entityID,
                    EntityClassID = entityClassID,
                    WorkflowSchemeID = workflowSchemeID
                }, cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Workflow api error");
            }
        }
    }
}
