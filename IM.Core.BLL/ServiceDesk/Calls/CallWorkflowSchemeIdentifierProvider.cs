using InfraManager.BLL.Settings;
using InfraManager.BLL.Workflow;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class CallWorkflowSchemeIdentifierProvider :
        ISelectWorkflowScheme<Call>,
        ISelfRegisteredService<ISelectWorkflowScheme<Call>>
    {
        private readonly IReadonlyRepository<CallType> _callTypes;
        private readonly IFinder<ServiceAttendance> _serviceAttendanceFinder;
        private readonly ISettingsBLL _settings;
        private readonly IConvertSettingValue<string> _converter;

        public CallWorkflowSchemeIdentifierProvider(
            IReadonlyRepository<CallType> callTypes,
            IFinder<ServiceAttendance> serviceAttendanceFinder,
            ISettingsBLL settings,
            IConvertSettingValue<string> converter)
        {
            _callTypes = callTypes;
            _serviceAttendanceFinder = serviceAttendanceFinder;
            _settings = settings;
            _converter = converter;
        }

        public async Task<string> SelectIdentifierAsync(Call call, CancellationToken cancellationToken = default)
        {
            if (call.CallService.IsNull)
            {
                return null; // Для заявок без сервиса схема рабочей процедуры не предусмотрена
            }

            var allCallTypes = await _callTypes.With(x => x.Parent).ToArrayAsync(cancellationToken);
            var callType = allCallTypes.SingleOrDefault(x => x.ID == call.CallTypeID)
                ?? throw new InvalidObjectException("Call Type is either deleted or does not exist");
            // TODO: user-friendly message + localization
            ServiceAttendance serviceAttendance = null;

            if (call.CallService.ServiceAttendanceID.HasValue)
            {
                serviceAttendance = await _serviceAttendanceFinder.FindAsync(
                    call.CallService.ServiceAttendanceID.Value,
                    cancellationToken)
                    ?? throw new InvalidObjectException("Service Attendance is either deleted or does not exist");
                // TODO: user-friendly message + localization
            }

            var defaultSchemeIdentifier = await _settings.GetValueAsync(SystemSettings.DefaultCallWorkflowSchemeIdentifier, cancellationToken);
            return callType.GetWorkflowSchemeIdentifier(serviceAttendance) ?? _converter.Convert(defaultSchemeIdentifier);
        }
    }
}
