using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.EntityFieldEditor
{
    public class FormalValidator<TEntity, TResult> : IValidator<TEntity, TResult>
        where TEntity : class
        where TResult : EntityFieldEditorOutput, new()
    {
        public TResult Validate(TEntity entity)
        {
            return new TResult
            {
                Result = BaseError.Success
            };
        }
    }
}
