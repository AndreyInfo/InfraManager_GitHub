namespace InfraManager.BLL.EntityFieldEditor
{
    public interface IValidator<TEntity, TResult> 
        where TEntity : class
        where TResult : EntityFieldEditorOutput, new()
    {
        TResult Validate(TEntity entity);
    }
}
