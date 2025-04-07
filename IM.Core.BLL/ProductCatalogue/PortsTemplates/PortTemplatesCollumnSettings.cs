using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.Asset;
using InfraManager.BLL.ProductCatalogue.PortTemplates;

namespace InfraManager.BLL.ProductCatalogue.PortsTemplates;

internal sealed class PortTemplatesCollumnSettings : 
    IColumnMapperSetting<PortTemplate, PortTemplateColumns>
    , ISelfRegisteredService<IColumnMapperSetting<PortTemplate, PortTemplateColumns>>
{
    public void Configure(IColumnMapperSettingsBase<PortTemplate, PortTemplateColumns> configurer)
    {
        configurer.ShouldBe(x => x.PortNumber, x => x.PortNumber);
        configurer.ShouldBe(x => x.JackTypeName, x => x.JackType.Name);
        configurer.ShouldBe(x => x.TechnologyTypeName, x => x.TechnologyType.Name);
    }
}
