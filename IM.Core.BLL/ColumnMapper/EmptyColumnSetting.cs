namespace InfraManager.BLL.ColumnMapper;

public class EmptyColumnSetting<TEntity,TTable> : IColumnMapperSetting<TEntity,TTable>
{
    public void Configure(IColumnMapperSettingsBase<TEntity, TTable> configurer)
    {
    }
}