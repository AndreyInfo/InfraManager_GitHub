using Inframanager.BLL;
using System;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using System.Threading;
using AutoMapper;
using InfraManager.BLL.Workflow;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    internal class ChangeRequestModifier :
        IModifyObject<ChangeRequest, ChangeRequestData>,
        ISelfRegisteredService<IModifyObject<ChangeRequest, ChangeRequestData>>
    {
        private readonly IMapper _mapper;
        private readonly ISelectWorkflowScheme<ChangeRequest> _workflowSchemeProvider;

        public ChangeRequestModifier(IMapper mapper,
            ISelectWorkflowScheme<ChangeRequest> workflowSchemeProvider)
        {
            _mapper = mapper;
            _workflowSchemeProvider = workflowSchemeProvider;
        }

        public async Task ModifyAsync(ChangeRequest changeRequest, ChangeRequestData data, CancellationToken cancellationToken = default)
        {
            var typeChanged = data.TypeID.HasValue && data.TypeID != changeRequest.RFCTypeID;
            _mapper.Map(data, changeRequest);
            
            if (typeChanged)
            {
                changeRequest.WorkflowSchemeIdentifier = 
                    await _workflowSchemeProvider.SelectIdentifierAsync(changeRequest, cancellationToken);
            }
        }

        public void SetModifiedDate(ChangeRequest changeRequest)
        {
            changeRequest.UtcDateModified = DateTime.UtcNow;
        }

    }
}
