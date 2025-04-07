using InfraManager.BLL.Workflow;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentWorkflowCreator :
        IVisitNewEntity<MassIncident>,
        ISelfRegisteredService<IVisitNewEntity<MassIncident>>
    {
        private readonly ICreateWorkflow<MassIncident> _workflow;

        public MassIncidentWorkflowCreator(ICreateWorkflow<MassIncident> workflow)
        {
            _workflow = workflow;
        }

        public void Visit(MassIncident entity)
        {
            throw new NotSupportedException();
        }

        public async Task VisitAsync(MassIncident entity, CancellationToken cancellationToken)
        {
            await _workflow.TryStartNewAsync(entity, cancellationToken);
        }
    }
}
