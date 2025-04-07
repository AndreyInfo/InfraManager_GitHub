using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.Events;


namespace InfraManager.BLL.Events;

public class EventSubjectColumnSettings : IColumnMapperSetting<Event, EventSubjectListItem>,
    ISelfRegisteredService<IColumnMapperSetting<Event, EventSubjectListItem>>
{
    public void Configure(IColumnMapperSettingsBase<Event, EventSubjectListItem> configurer)
    {
        configurer.ShouldBe(x => x.Description, x => x.Message);
        
        configurer.ShouldBe(x => x.UserName, x => x.User.Surname).Then(x => x.User.Name)
            .Then(x => x.User.Patronymic);
    }
}