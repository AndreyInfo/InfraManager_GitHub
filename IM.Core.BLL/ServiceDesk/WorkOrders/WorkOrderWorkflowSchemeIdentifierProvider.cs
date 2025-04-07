using InfraManager.BLL.Settings;
using InfraManager.BLL.Workflow;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    public class WorkOrderWorkflowSchemeIdentifierProvider : 
        ISelectWorkflowScheme<WorkOrder>,
        ISelfRegisteredService<ISelectWorkflowScheme<WorkOrder>>
    {
        private readonly IFinder<WorkOrderType> _workorderTypeFinder;
        private readonly ISettingsBLL _settings;
        private readonly IConvertSettingValue<string> _converter;

        public WorkOrderWorkflowSchemeIdentifierProvider(
            IFinder<WorkOrderType> workorderTypeFinder, 
            ISettingsBLL settings, 
            IConvertSettingValue<string> converter)
        {
            _workorderTypeFinder = workorderTypeFinder;
            _settings = settings;
            _converter = converter;
        }

        public async Task<string> SelectIdentifierAsync(WorkOrder data, CancellationToken cancellationToken = default)
        {
            var workorderType = await _workorderTypeFinder.FindAsync(new object[] { data.TypeID }, cancellationToken);
            var defaultSchemeIdentifier = await _settings.GetValueAsync(SystemSettings.DefaultWorkOrderWorkflowSchemeIdentifier, cancellationToken);

            return workorderType?.WorkflowSchemeIdentifier ?? _converter.Convert(defaultSchemeIdentifier);
        }
    }
}
