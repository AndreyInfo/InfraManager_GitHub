using System;
using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.Sessions;

namespace InfraManager.BLL.Sessions;

public class UserSessionHistoryColumnMapperSettings :
    IColumnMapperSetting<UserSessionHistory, UserSessionHistoryListItem>,
    ISelfRegisteredService<IColumnMapperSetting<UserSessionHistory, UserSessionHistoryListItem>>
{
    public void Configure(IColumnMapperSettingsBase<UserSessionHistory, UserSessionHistoryListItem> configurer)
    {
        configurer.ShouldBe(x => x.TypeString, x => x.Type);
        configurer.ShouldBe(x => x.ExecutorFullName, x => x.Executor.Surname).Then(x => x.Executor.Name)
            .Then(x => x.Executor.Patronymic);
        configurer.ShouldBe(x => x.UserFullName, x => x.User.Surname).Then(x => x.User.Name)
            .Then(x => x.User.Patronymic);
    }
}