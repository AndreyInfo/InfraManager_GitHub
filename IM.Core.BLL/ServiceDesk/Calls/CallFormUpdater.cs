using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.FormBuilder;
using InfraManager.BLL.FormBuilder.Contracts;
using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Calls;

internal class CallFormUpdater :
    FormUpdaterBase<Call>,
    IVisitModifiedEntity<Call>,
    ISelfRegisteredService<IVisitModifiedEntity<Call>>
{
    private const SystemSettings Option = SystemSettings.RecalculateCallAdditionalParametersWithServiceChange;

    private readonly IFullFormBLL _fullFormBLL;
    private readonly ISettingsBLL _settings;
    private readonly IConvertSettingValue<bool> _converter;

    public CallFormUpdater(
        IReadonlyRepository<Subdivision> subdivisions,
        IReadonlyRepository<User> users,
        IFullFormBLL fullFormBLL,
        ISettingsBLL settings,
        IConvertSettingValue<bool> converter)
        : base(subdivisions, users)
    {
        _fullFormBLL = fullFormBLL;
        _settings = settings;
        _converter = converter;
    }

    protected override FormBuilderFullFormDetails GetTemplate(Call currentState)
    {
        return currentState.CallService.ServiceItemID.HasValue
            ? _fullFormBLL.GetAsync(ObjectClass.ServiceItem, currentState.CallService.ServiceItemID.Value).GetAwaiter().GetResult()
            : currentState.CallService.ServiceAttendanceID.HasValue
                ? _fullFormBLL.GetAsync(ObjectClass.ServiceAttendance, currentState.CallService.ServiceAttendanceID.Value).GetAwaiter().GetResult()
                : null;
    }

    protected override async Task<FormBuilderFullFormDetails> GetTemplateAsync(Call currentState, CancellationToken cancellationToken)
    {
        return currentState.CallService.ServiceItemID.HasValue
            ? await _fullFormBLL.GetAsync(ObjectClass.ServiceItem, currentState.CallService.ServiceItemID.Value, cancellationToken)
            : currentState.CallService.ServiceAttendanceID.HasValue
                ? await _fullFormBLL.GetAsync(ObjectClass.ServiceAttendance, currentState.CallService.ServiceAttendanceID.Value, cancellationToken)
                : null;
    }

    protected override bool ShouldRecalculate(IEntityState originalState, Call currentState)
    {
        return IsCallServiceModified(originalState,currentState) && _converter.Convert(_settings.GetValue(Option));
    }

    protected override async Task<bool> ShouldRecalculateAsync(IEntityState originalState, Call currentState, CancellationToken cancellationToken)
    {
        return IsCallServiceModified(originalState,currentState) && _converter.Convert(await _settings.GetValueAsync(Option, cancellationToken));
    }

    private static bool IsCallServiceModified(IEntityState originalState, Call currentState)
    {
        var previousCallServiceID = (Guid) originalState[nameof(Call.CallServiceID)];
        return previousCallServiceID != currentState.CallServiceID;
    }
}