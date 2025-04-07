using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.Workflow;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    internal class ChangeRequestBuilder :
        IBuildObject<ChangeRequest, ChangeRequestData>,
        ISelfRegisteredService<IBuildObject<ChangeRequest, ChangeRequestData>>
    {
        private IReadonlyRepository<Priority> _priorityRepository;
        private readonly IMapper _mapper;
        private readonly ICreateWorkflow<ChangeRequest> _workflow;

        public ChangeRequestBuilder(
            IReadonlyRepository<Priority> priorityRepository, 
            IMapper mapper, 
            ICreateWorkflow<ChangeRequest> workflow)
        {
            _priorityRepository = priorityRepository;
            _mapper = mapper;
            _workflow = workflow;
        }

        public async Task<ChangeRequest> BuildAsync(ChangeRequestData data, CancellationToken cancellationToken = default)
        {
            var changeRequest = _mapper.Map<ChangeRequest>(data);

            if (!data.PriorityID.HasValue)
            {
                changeRequest.PriorityID = await _priorityRepository.GetDefaultPriorityIDAsync(cancellationToken: cancellationToken)
                    ?? throw new InvalidObjectException("Priority is missing"); //TODO локализация
            }

            changeRequest.UtcDatePromised = DateTime.UtcNow.AddDays(3); // TODO: Необходимо реализовать функционал определения DatePromised
            changeRequest.UtcDateDetected = DateTime.UtcNow;
            changeRequest.UtcDateModified = DateTime.UtcNow;

            await _workflow.TryStartNewAsync(changeRequest, cancellationToken);

            return changeRequest;
        }

        public Task<IEnumerable<ChangeRequest>> BuildManyAsync(IEnumerable<ChangeRequestData> dataItems, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
