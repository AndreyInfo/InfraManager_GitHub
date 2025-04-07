using InfraManager.BLL.ColumnMapper;
using InfraManager.BLL.Software.SoftwareLicenceSchemes;

namespace InfraManager.BLL.Software
{
    public class SoftwareLicenceSchemeColumnMapperSettings :
        IColumnMapperSetting<DAL.Software.SoftwareLicenceScheme,
            SoftwareLicenceSchemeListItemForTable>,
        ISelfRegisteredService<IColumnMapperSetting<DAL.Software.SoftwareLicenceScheme,
            SoftwareLicenceSchemeListItemForTable>>
    {
        public void Configure(IColumnMapperSettingsBase<DAL.Software.SoftwareLicenceScheme,
            SoftwareLicenceSchemeListItemForTable> configurer)
        {
        }
    }
}