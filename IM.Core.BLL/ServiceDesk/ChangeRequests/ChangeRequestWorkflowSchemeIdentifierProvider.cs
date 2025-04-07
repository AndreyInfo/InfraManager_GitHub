using InfraManager.BLL.ServiceDesk.ChangeRequests;
using InfraManager.BLL.Settings;
using InfraManager.BLL.Workflow;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class ChangeRequestWorkflowSchemeIdentifierProvider :
        ISelectWorkflowScheme<ChangeRequest>,
        ISelfRegisteredService<ISelectWorkflowScheme<ChangeRequest>>
    {
        private readonly IReadonlyRepository<ChangeRequestType> _changeRequestTypes;
        private readonly ISettingsBLL _settings;
        private readonly IConvertSettingValue<string> _converter;

        public ChangeRequestWorkflowSchemeIdentifierProvider(
            IReadonlyRepository<ChangeRequestType> changeRequestTypes, 
            ISettingsBLL settings,
            IConvertSettingValue<string> converter)
        {
            _changeRequestTypes = changeRequestTypes;
            _settings = settings;
            _converter = converter;
        }

        public async Task<string> SelectIdentifierAsync(ChangeRequest data, CancellationToken cancellationToken = default)
        {
            var changeRequestType = _changeRequestTypes.SingleOrDefault(x => x.ID == data.RFCTypeID)
                ?? throw new InvalidObjectException("ChangeRequest Type is either deleted or does not exist");  //TODO локализация

            var defaultSchemeIdentifier = await _settings.GetValueAsync(SystemSettings.DefaultRFCWorkflowSchemeIdentifier, cancellationToken);
            return changeRequestType.WorkflowSchemeIdentifier ?? _converter.Convert(defaultSchemeIdentifier);
        }
    }
}
