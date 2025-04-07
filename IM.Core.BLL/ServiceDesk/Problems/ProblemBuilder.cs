using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.Workflow;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    internal class ProblemBuilder :
        IBuildObject<Problem, ProblemData>,
        ISelfRegisteredService<IBuildObject<Problem, ProblemData>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        private readonly IReadonlyRepository<Priority> _priorityRepository;
        private readonly ISupportSettingsBll _supportSettingsBll;
        private readonly ICalendarServiceBLL _calendarServiceBLL;
        private readonly ICreateWorkflow<Problem> _workflow;

        public ProblemBuilder(
            ICurrentUser currentUser,
            IMapper mapper,
            IReadonlyRepository<Priority> priorityRepository,
            ISupportSettingsBll supportSettingsBll,
            ICalendarServiceBLL calendarServiceBLL,
            ICreateWorkflow<Problem> workflow)
        {
            _currentUser = currentUser;
            _mapper = mapper;
            _priorityRepository = priorityRepository;
            _supportSettingsBll = supportSettingsBll;
            _calendarServiceBLL = calendarServiceBLL;
            _workflow = workflow;
        }

        public async Task<Problem> BuildAsync(ProblemData data, CancellationToken cancellationToken = default)
        {
            var problem = _mapper.Map<Problem>(data);

            if (!data.PriorityID.HasValue)
            {
                problem.PriorityID = await _priorityRepository.GetDefaultPriorityIDAsync() 
                    ?? throw new InvalidObjectException("Priority is missing"); //TODO локализация
            }

            problem.InitiatorID = data.InitiatorID.Value ?? _currentUser.UserId;

            var problemPromisedDeltaTicks = _supportSettingsBll.GetProblemsSettings().Ticks;
            var utcDatePromised = await _calendarServiceBLL.GetUtcFinishDateByCalendarAsync(DateTime.UtcNow, new TimeSpan(problemPromisedDeltaTicks), null, null);

            problem.UtcDatePromised = utcDatePromised;
            problem.UtcDateDetected = DateTime.UtcNow;
            await _workflow.TryStartNewAsync(problem, cancellationToken);

            return problem;
        }

        public Task<IEnumerable<Problem>> BuildManyAsync(IEnumerable<ProblemData> dataItems, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
