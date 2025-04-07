using InfraManager.CrossPlatform.BLL.Intrefaces.Schedule;
using InfraManager.CrossPlatform.WebApi.Contracts.Scheduler;
using Microsoft.Extensions.Logging;
/*  Till Migrate SchedulerServiceusing InfraManager.Services.ScheduleService;*/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfraManager.BLL.Scheduler
{
    public class ScheduleBLL : IScheduleBLL
    {
        private readonly string _endPoint;
/*  Till Migrate SchedulerService
        private IScheduleService _scheduleService;
        private DuplexChannelFactory<IScheduleService> _channelFactory;
*/
        private readonly ILogger _logger;
        private static object _locker = new object();
        public ScheduleBLL(string endpoint, ILogger<ScheduleBLL> logger)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ArgumentNullException(nameof(endpoint));
            _endPoint = endpoint;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

/*  Till Migrate SchedulerService
        private void CloseFactory()
        {
            if (_channelFactory.State == CommunicationState.Faulted)
                _channelFactory.Abort();
            else
                try
                {
                    _channelFactory.Close();
                }
                catch
                {
                    _channelFactory.Abort();
                }
            _channelFactory = null;
            _scheduleService = null;
        }

        private IScheduleService ScheduleService
        {
            get
            {
                lock (_locker)
                {
                    if (_channelFactory != null && _channelFactory.State != CommunicationState.Opened)
                    {
                        CloseFactory();
                    }
                    if (_channelFactory == null)
                    {
                        _channelFactory = new DuplexChannelFactory<IScheduleService>(new InstanceContext(new SchedulerCallBAck()), new NetTcpBinding(), new EndpointAddress(_endPoint));

                        try
                        {
                            _channelFactory.Open();
                        }
                        catch (CommunicationException e)
                        {
                            _logger.LogError(e, $"Не удалось открыть фабрику соединений с сервисом");
                            CloseFactory();
                        }
                        catch (TimeoutException e)
                        {
                            _logger.LogError(e, $"Не удалось открыть фабрику соединений с сервисом");
                            CloseFactory();
                        }
                        if (_channelFactory == null)
                            return null;
                    }


                    if (_scheduleService == null)
                    {
                        _scheduleService = ((ChannelFactory<IScheduleService>)_channelFactory).CreateChannel();
                    }
                }
                return _scheduleService;
            }
        }
*/

        public async Task<List<ScheduleTask>> GetTasks()
        {
/*  Till Migrate SchedulerService
            List<ScheduledTask> tasks = null;
            Services.OperationResult getResult = null;
            try
            {
                getResult = ScheduleService.GetScheduledTasks(out tasks);
            }
            catch(CommunicationException ex)
            {
                lock(_locker){
                    CloseFactory();
                }
                getResult = ScheduleService.GetScheduledTasks(out tasks);
            }

            if (getResult.Type == Services.OperationResultType.Success && tasks != null)
                return tasks.Select(x => new ScheduleTask()
                {
                    LastRun = x.LastRunAt,
                    NextRun = x.NextRunAt,
                    ScheduleLabel = string.IsNullOrWhiteSpace(x.ScheduleString) ? (x.Schedules != null ? string.Join(";", x.Schedules.Select(s => s.ToString())) : string.Empty) : x.ScheduleString,
                    SettingID = x.TaskSettingID,
                    StateLabel = x.TaskStateString,
                    TaskID = x.ID,
                }).ToList();
*/
            return new List<ScheduleTask>();
        }
    }
}

