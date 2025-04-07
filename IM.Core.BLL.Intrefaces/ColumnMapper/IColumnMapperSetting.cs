namespace InfraManager.BLL.ColumnMapper;
/// <summary>
/// Класс настроек для <see cref="IColumnMapper"/>, который позволяет создать настройку для маппинга полей из таблицы в свойства сущности
/// </summary>
/// <typeparam name="TEntity">Сущность, свойства которой будут использоваться при сортировки</typeparam>
/// <typeparam name="TTable">Модель таблицы</typeparam>
public interface IColumnMapperSetting<TEntity, TTable>
{
    public void Configure(IColumnMapperSettingsBase<TEntity, TTable> configurer);
}

