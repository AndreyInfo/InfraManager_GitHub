using InfraManager.BLL.OrganizationStructure;

namespace InfraManager.BLL.ColumnMapper;

public sealed class GroupColumnSettings : IColumnMapperSetting<Group, GroupQueueForTable>
    , ISelfRegisteredService<IColumnMapperSetting<Group, GroupQueueForTable>>
{
    public void Configure(IColumnMapperSettingsBase<Group, GroupQueueForTable> configurer)
    {
        configurer.ShouldBe(x => x.ResponsibleName, x => x.ResponsibleUser.Surname)
            .Then(x => x.ResponsibleUser.Name)
            .Then(x => x.ResponsibleUser.Patronymic);
    }
}