using InfraManager.BLL.Localization;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.AssetsManagement.Hardware;

public class LocationOnStoreLookupQuery : BooleanToLocalizedTextLookupQuery
{
    public LocationOnStoreLookupQuery(ILocalizeText localizer)
        : base(localizer)
    {
    }

    protected override string TrueValueResourceName => nameof(Resources.LocationOnStore_True);
    protected override string FalseValueResourceName => nameof(Resources.LocationOnStore_False);
}