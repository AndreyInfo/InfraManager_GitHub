using InfraManager.BLL.AccessManagement.ForTable;
using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.AccessManagement;

namespace InfraManager.BLL.AccessManagement;

internal class AccessPermissionColumnSettings : IColumnMapperSetting<AccessPermissionModelItem, AccessPermissionForTable>
    , ISelfRegisteredService<IColumnMapperSetting<AccessPermissionModelItem, AccessPermissionForTable>>
{
    public void Configure(IColumnMapperSettingsBase<AccessPermissionModelItem, AccessPermissionForTable> configurer)
    {
        configurer.ShouldBe(c => c.Name, c => c.Name);
        configurer.ShouldBe(c => c.Rights, c => c.Properties);
        configurer.ShouldBe(c => c.Rights, c => c.Add);
        configurer.ShouldBe(c => c.Rights, c => c.Update);
        configurer.ShouldBe(c => c.Rights, c => c.Delete);
        configurer.ShouldBe(c => c.Rights, c => c.AccessManage);
    }
}
