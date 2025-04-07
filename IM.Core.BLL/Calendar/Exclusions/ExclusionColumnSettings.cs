using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.Calendar;

namespace InfraManager.BLL.Calendar.Exclusions;

internal sealed class ExclusionColumnSettings : IColumnMapperSetting<Exclusion, ExclusionForTable>
    , ISelfRegisteredService<IColumnMapperSetting<Exclusion, ExclusionForTable>>
{
    public void Configure(IColumnMapperSettingsBase<Exclusion, ExclusionForTable> configurer)
    {
        configurer.ShouldBe(x => x.TypeName, x => x.Type);
    }
}
