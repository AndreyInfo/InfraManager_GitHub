using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.Events;

namespace InfraManager.BLL.Events;

public class EventSubjectFiltrationColumnSettings : IColumnMapperSetting<Event, EventSubjectFiltrationOptions>,
    ISelfRegisteredService<IColumnMapperSetting<Event, EventSubjectFiltrationOptions>>
{
    public void Configure(IColumnMapperSettingsBase<Event, EventSubjectFiltrationOptions> configurer)
    {
        configurer.ShouldBe(x => x.UserName,
            x => (x.User.Surname + " " + x.User.Name + " " + x.User.Patronymic).Trim());

        configurer.ShouldBe(x => x.Description,
            x => (x.Message));
    }
}

public class EventSubjectFiltrationOptions
{
    public string UserName { get; init; }
    public string Description { get; init; }
}