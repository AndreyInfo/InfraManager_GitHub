using InfraManager.BLL.ColumnMapper;
using InfraManager.BLL.Software.SoftwareModels.ForTable;
using InfraManager.DAL.Software;

namespace InfraManager.BLL.Software;

internal sealed class SoftwareModelColumnSettings : 
    IColumnMapperSetting<SoftwareModel, SoftwareModelForTable>,
    ISelfRegisteredService<IColumnMapperSetting<SoftwareModel, SoftwareModelForTable>>
{
    public void Configure(IColumnMapperSettingsBase<SoftwareModel, SoftwareModelForTable> configurer)
    {
        configurer.ShouldBe(x => x.TemplateText, x => x.Template);
        configurer.ShouldBe(x => x.SoftwareModelUsingTypeName, x => x.SoftwareModelUsingType);
    }
}
