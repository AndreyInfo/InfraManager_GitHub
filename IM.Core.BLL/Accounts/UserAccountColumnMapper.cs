using System;
using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.Accounts;

namespace InfraManager.BLL.Accounts;

public class UserAccountColumnMapper : IColumnMapperSetting<UserAccount, UserAccountListItem>,
    ISelfRegisteredService<IColumnMapperSetting<UserAccount, UserAccountListItem>>
{
    public void Configure(IColumnMapperSettingsBase<UserAccount, UserAccountListItem> configurer)
    {
        configurer.ShouldBe(x => x.TypeText, x => x.Type);
    }
}