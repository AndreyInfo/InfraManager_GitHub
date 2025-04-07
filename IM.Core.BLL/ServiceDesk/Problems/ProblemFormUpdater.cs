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

namespace InfraManager.BLL.ServiceDesk.Problems;

internal class ProblemFormUpdater :
    FormUpdaterBase<Problem>,
    IVisitModifiedEntity<Problem>,
    ISelfRegisteredService<IVisitModifiedEntity<Problem>>
{
    private const SystemSettings Option = SystemSettings.RecalculateProblemAdditionalParametersWithProblemTypeChange;

    private readonly IFullFormBLL _fullFormBLL;
    private readonly ISettingsBLL _settings;
    private readonly IConvertSettingValue<bool> _converter;

    public ProblemFormUpdater(
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

    protected override FormBuilderFullFormDetails GetTemplate(Problem currentState)
    {
        return _fullFormBLL.GetAsync(ObjectClass.ProblemType, currentState.TypeID).GetAwaiter().GetResult();
    }

    protected override Task<FormBuilderFullFormDetails> GetTemplateAsync(Problem currentState, CancellationToken cancellationToken)
    {
        return _fullFormBLL.GetAsync(ObjectClass.ProblemType, currentState.TypeID, cancellationToken);
    }

    protected override bool ShouldRecalculate(IEntityState originalState, Problem currentState)
    {
        var previousTypeID = (Guid)originalState[nameof(Problem.TypeID)];
        return previousTypeID != currentState.TypeID && _converter.Convert(_settings.GetValue(Option));
    }

    protected override async Task<bool> ShouldRecalculateAsync(IEntityState originalState, Problem currentState, CancellationToken cancellationToken)
    {
        var previousTypeID = (Guid)originalState[nameof(Problem.TypeID)];
        return previousTypeID != currentState.TypeID && _converter.Convert(await _settings.GetValueAsync(Option, cancellationToken));
    }
}