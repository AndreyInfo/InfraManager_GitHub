using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.EntityFieldEditor
{
    public class ReflectionFieldAccessor<TEntity> : IFieldAccessor<TEntity, SetFieldData>
        where TEntity : class
    {
        private readonly string _fieldName;

        public ReflectionFieldAccessor(string fieldName)
        {
            _fieldName = fieldName;
        }

        public object GetValue(TEntity entity)
        {
            return typeof(TEntity).GetProperty(_fieldName).GetValue(entity);
        }

        public bool MatchesCurrent(TEntity entity, SetFieldData data)
        {
            try
            {
                var currentValue = typeof(TEntity).GetProperty(_fieldName).GetValue(entity);
                return (data.OldValue == null && currentValue == null)
                    || (data.OldValue != null && data.OldValue.Equals(currentValue));
            }
            catch
            {
                return false;
            }
        }

        public BaseError SetValue(TEntity entity, SetFieldData data)
        {
            try
            {
                typeof(TEntity).GetProperty(_fieldName).SetValue(entity, data.NewValue);
                return BaseError.Success;
            }
            catch
            {
                return BaseError.BadParamsError;
            }
        }
    }
}
