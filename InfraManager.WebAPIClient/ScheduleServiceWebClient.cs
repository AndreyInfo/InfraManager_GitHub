using InfraManager.ServiceBase.ScheduleService;
using InfraManager.Services;
using InfraManager.Services.ScheduleService;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.ServiceBase.ScheduleService.WebApiModels;
using InfraManager.ServiceBase.WorkflowService.WebAPIModels;
using InfraManager.BLL.Import;

namespace InfraManager.WebAPIClient
{
    public class ScheduleServiceWebClient : ClientWithAuthorizationBase, IScheduleServiceWebApi
    {
        public ScheduleServiceWebClient(string baseUrl)
            : base(baseUrl)
        {
        }

        public async Task<ScheduleTask[]> GetScheduleTasksAsync(ScheduleFilterRequest filter, Guid currentUser,
            CancellationToken cancellationToken = default)
        {
            return await GetAsync<ScheduleTask[], ScheduleFilterRequest>("Schedule/list", filter, currentUser,
                cancellationToken);
        }

        public async Task<Guid> AddScheduleTaskAsync(ScheduleTask task, Guid currentUser,
            CancellationToken cancellationToken = default)
        {
            return await PostAsync<Guid, ScheduleTask>("Schedule", task, currentUser, cancellationToken);
        }

        public async Task<OperationResult> UpdateScheduleTaskAsync(ScheduleTask task, Guid currentID,
            CancellationToken cancellationToken = default)
        {
            var result = await PutAsync<OperationResult, ScheduleTask>("Schedule", task, currentID, cancellationToken);
            
            if (result.Type != OperationResultType.Success)
            {
                throw new Exception("Schedule api error");
            }
            
            return result;
        }

        public async Task<bool> StopTaskAsync(TaskCallbackRequest task, Guid currentUser,
            CancellationToken cancellationToken = default)
        {
            return await PutAsync<bool, TaskCallbackRequest>("Schedule/stop", task, currentUser, cancellationToken);
        }

        public async Task<ScheduleTask> GetAsync(Guid id, Guid currentUser,
            CancellationToken cancellationToken = default)
        {
            return await GetAsync<ScheduleTask>($"Schedule/{id}", currentUser, cancellationToken);
        }

        public async Task<bool> StopTaskAsync(TaskCallbackRequest task, CancellationToken cancellationToken = default)
        {
            return await PostAsync<bool, TaskCallbackRequest>($"schedule/stop", task, null, cancellationToken);
        }

        public async Task DeleteAsync(Guid id, Guid currentID, CancellationToken cancellationToken = default)
        {
            await DeleteAsync($"Schedule/{id}", currentID, cancellationToken);
        }
        
        public async Task<bool> EnsureAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await GetAsync<OperationResult>(
                    "schedule/ensure", cancellationToken: cancellationToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<TaskState> RunScheduleTaskAsync(RunTaskRequest runTaskRequest, Guid currentUser,
            CancellationToken cancellationToken = default)
        {
            return await PostAsync<TaskState, RunTaskRequest>("Schedule/run", runTaskRequest, currentUser,
                cancellationToken: cancellationToken);
        }

        public async Task ChangeConnectionStringAsync(string server, int port, string dataBase, string login, string password,
            string additionalField, CancellationToken cancellationToken = default)
        {
            var result = await PostAsync<ScheduleResultWithBool, ConnectionStringChangeRequest>(
                "/Settings/ConnectionString",
                new ConnectionStringChangeRequest
                {
                    Server = server,
                    Login = login,
                    Password = password,
                    Database = dataBase,
                    Port = port,
                    AdditionalField = additionalField
                }, cancellationToken: cancellationToken);

            if (result.OperationResult.Type != OperationResultType.Success)
            {
                throw new Exception("Schedule api error");
            }
        }

        public async Task<bool> DeleteTaskAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await DeleteAsync<bool>($"Schedule/byUISettingId/{id}", cancellationToken: cancellationToken);
        }
    }
}
