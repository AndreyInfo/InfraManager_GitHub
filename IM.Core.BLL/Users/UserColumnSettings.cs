using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL;

namespace InfraManager.BLL.Users;

internal sealed class UserColumnSettings : IColumnMapperSetting<User, UserForTable>
    , ISelfRegisteredService<IColumnMapperSetting<User, UserForTable>>
{
    public void Configure(IColumnMapperSettingsBase<User, UserForTable> configurer)
    {
        configurer.ShouldBe(x => x.SurName, x => x.Surname); //TODO бессмысленно, нужно будет убрать
        configurer.ShouldBe(x => x.Organization, x => x.Subdivision.Organization.Name);
        configurer.ShouldBe(x => x.Department, x => x.Subdivision.Name);
        configurer.ShouldBe(x => x.Building, x => x.Workplace.Room.Floor.Building.Name);
        configurer.ShouldBe(x => x.Floor, x => x.Workplace.Room.Floor.Name);
        configurer.ShouldBe(x => x.Room, x => x.Workplace.Room.Name);
    }
}