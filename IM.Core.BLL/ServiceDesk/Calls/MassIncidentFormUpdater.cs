using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.FormBuilder;
using InfraManager.BLL.FormBuilder.Contracts;
using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk.Calls;

internal class MassIncidentFormUpdater : FormUpdaterBase<MassIncident>,
    IVisitModifiedEntity<MassIncident>,
    ISelfRegisteredService<IVisitModifiedEntity<MassIncident>>
{
    private const SystemSettings Option = SystemSettings.RecalculateMassIncidentsAdditionalParametersWithTypeChange;

    private readonly IFullFormBLL _fullFormBLL;
    private readonly ISettingsBLL _settings;
    private readonly IConvertSettingValue<bool> _converter;

    public MassIncidentFormUpdater(
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

    protected override bool ShouldRecalculate(IEntityState originalState, MassIncident currentState)
    {
        return IsTypeModified(originalState, currentState) && _converter.Convert(_settings.GetValue(Option));
    }

    protected override async Task<bool> ShouldRecalculateAsync(IEntityState originalState, MassIncident currentState, CancellationToken cancellationToken)
    {
        return IsTypeModified(originalState, currentState) && _converter.Convert(await _settings.GetValueAsync(Option, cancellationToken));
    }

    protected override FormBuilderFullFormDetails GetTemplate(MassIncident currentState)
    {
        return _fullFormBLL.GetAsync(ObjectClass.MassIncidentType, currentState.Type.IMObjID).GetAwaiter().GetResult();
    }

    protected override async Task<FormBuilderFullFormDetails> GetTemplateAsync(MassIncident currentState, CancellationToken cancellationToken)
    {
        return await _fullFormBLL.GetAsync(ObjectClass.MassIncidentType, currentState.Type.IMObjID, cancellationToken);
    }

    private static bool IsTypeModified(IEntityState originalState, MassIncident currentState)
    {
        return (int) originalState[nameof(MassIncident.TypeID)] != currentState.TypeID;
    }
}