using InfraManager.BLL.ColumnMapper;
using InfraManager.BLL.Software.SoftwareModelTabs.Licenses;
using InfraManager.DAL.Software;

namespace InfraManager.BLL.Software.SoftwareModelTabs;

public class SoftwareModelLicenseColumnSettings :
    IColumnMapperSetting<SoftwareLicence, SoftwareModelLicenseForTable>,
    ISelfRegisteredService<IColumnMapperSetting<SoftwareLicence, SoftwareModelLicenseForTable>>
{
    public void Configure(IColumnMapperSettingsBase<SoftwareLicence, SoftwareModelLicenseForTable> configurer)
    {
        configurer.ShouldBe(t => t.SoftwareModelName, e => e.SoftwareModel.Name);
        configurer.ShouldBe(t => t.SoftwareModelVersion, e => e.SoftwareModel.Version);
        configurer.ShouldBe(t => t.SoftwareTypeName, e => e.SoftwareModel.SoftwareType.Name);
        configurer.ShouldBe(t => t.SoftwareLicenceTypeModelName, e => e.ProductCatalogType.Name);
    }
}
