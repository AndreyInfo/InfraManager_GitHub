using InfraManager.BLL.Localization;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.AssetsManagement.Hardware;

public class IsWorkingLookupQuery : BooleanToLocalizedTextLookupQuery 
{
    public IsWorkingLookupQuery(ILocalizeText localizer)
        : base(localizer)
    {
    }

    protected override string TrueValueResourceName => nameof(Resources.IsWorking_True);
    protected override string FalseValueResourceName => nameof(Resources.IsWorking_False);
}