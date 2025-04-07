using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.SlotTemplates;
internal sealed class SlotTemplateCollumnSettings : IColumnMapperSetting<SlotTemplate, SlotTemplateColumns>
    , ISelfRegisteredService<IColumnMapperSetting<SlotTemplate, SlotTemplateColumns>>
{
    public void Configure(IColumnMapperSettingsBase<SlotTemplate, SlotTemplateColumns> configurer)
    {
        configurer.ShouldBe(x => x.SlotTypeName, x => x.SlotType.Name);
    }
}
