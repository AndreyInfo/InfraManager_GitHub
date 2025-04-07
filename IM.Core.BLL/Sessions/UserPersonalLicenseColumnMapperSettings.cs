using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.Sessions;

namespace InfraManager.BLL.Sessions;

public class UserPersonalLicenseColumnMapperSettings :
    IColumnMapperSetting<UserPersonalLicence, UserPersonalLicenceListItem>,
    ISelfRegisteredService<IColumnMapperSetting<UserPersonalLicence, UserPersonalLicenceListItem>>
{
    public void Configure(IColumnMapperSettingsBase<UserPersonalLicence, UserPersonalLicenceListItem> configurer)
    {
        configurer.ShouldBe(x => x.FullName, x => x.User.Surname).Then(x => x.User.Name)
            .Then(x => x.User.Patronymic);
        configurer.ShouldBe(x => x.LoginName, x => x.User.LoginName);
        configurer.ShouldBe(x => x.Number, x => x.User.Number);
        configurer.ShouldBe(x => x.SubdivisionFullName, x => x.User.Subdivision.Name);
    }
}