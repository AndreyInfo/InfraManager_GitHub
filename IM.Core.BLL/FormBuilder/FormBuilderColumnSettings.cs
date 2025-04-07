using InfraManager.BLL.ColumnMapper;
using InfraManager.DAL.FormBuilder;

namespace InfraManager.BLL.FormBuilder;

public class FormBuilderColumnSettings : IColumnMapperSetting<Form, FormBuilderForTable>,
    ISelfRegisteredService<IColumnMapperSetting<Form, FormBuilderForTable>>
{
    public void Configure(IColumnMapperSettingsBase<Form, FormBuilderForTable> configurer)
    {
        configurer.ShouldBe(x => x.Version, x => x.MajorVersion).Then(x => x.MinorVersion);
        configurer.ShouldBe(x => x.Class, x => x.ClassID);
    }
}