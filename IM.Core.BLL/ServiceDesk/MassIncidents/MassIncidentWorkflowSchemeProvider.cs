using InfraManager.BLL.Settings;
using InfraManager.BLL.Workflow;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentWorkflowSchemeProvider :
        ISelectWorkflowScheme<MassIncident>,
        ISelfRegisteredService<ISelectWorkflowScheme<MassIncident>>
    {
        private readonly IFinder<MassIncidentType> _types;
        private readonly ISettingsBLL _settings;
        private readonly IConvertSettingValue<string> _converter;

        public MassIncidentWorkflowSchemeProvider(
            IFinder<MassIncidentType> types, 
            ISettingsBLL settings, 
            IConvertSettingValue<string> converter)
        {
            _types = types;
            _settings = settings;
            _converter = converter;
        }

        public async Task<string> SelectIdentifierAsync(MassIncident massiveIncident, CancellationToken cancellationToken = default)
        {
            var type = await _types.FindAsync(massiveIncident.TypeID, cancellationToken);
            var workflowScheme = type?.WorkflowSchemeIdentifier;

            if (!string.IsNullOrWhiteSpace(workflowScheme))
            {
                return workflowScheme;
            }

            var defaultSchemeSetting = await _settings.GetValueAsync(SystemSettings.DefaultMassIncidentsWorkflowSchemeIdentifier, cancellationToken);
            return defaultSchemeSetting == null ? null : _converter.Convert(defaultSchemeSetting);
        }
    }
}
