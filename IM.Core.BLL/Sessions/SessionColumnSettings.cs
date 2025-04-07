using System;
using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.Sessions;

namespace InfraManager.BLL.Sessions;

public class SessionColumnSettings :
    IColumnMapperSetting<Session, SessionListItem>,
    ISelfRegisteredService<IColumnMapperSetting<Session, SessionListItem>>
{
    public void Configure(IColumnMapperSettingsBase<Session, SessionListItem> configurer)
    {
        configurer.ShouldBe(x => x.UserSubdivisionFullName, x => x.User.Subdivision.Name);
        configurer.ShouldBe(x => x.UserName, x => x.User.Surname).Then(x => x.User.Name).Then(x => x.User.Patronymic);
        configurer.ShouldBe(x => x.DurationInMinutes, x => (x.UtcDateClosed ?? DateTime.UtcNow) - x.UtcDateOpened);
        configurer.ShouldBe(x => x.UserLogin, x => x.User.LoginName);
        configurer.ShouldBe(x => x.LocationName, x => x.Location);
    }
}