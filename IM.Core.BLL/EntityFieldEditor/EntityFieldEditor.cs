using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.EntityFieldEditor
{
    public class EntityFieldEditor<TEntity, TInput, TOutput>
        where TEntity : class
        where TOutput : EntityFieldEditorOutput, new()
    {
        private readonly IFieldAccessor<TEntity, TInput> _fieldAccessor;

        private readonly IValidator<TEntity, TOutput> _validator;

        public EntityFieldEditor(
            IFieldAccessor<TEntity, TInput> fieldAccessor,
            IValidator<TEntity, TOutput> validator)
        {
            _fieldAccessor = fieldAccessor;
            _validator = validator;
        }

        public EntityFieldEditor(IFieldAccessor<TEntity, TInput> fieldAccessor)
            : this(fieldAccessor, new FormalValidator<TEntity, TOutput>())
        {
        }

        public TOutput SetField(TEntity entity, TInput data)
        {
            if (!_fieldAccessor.MatchesCurrent(entity, data))
            {
                return new TOutput
                {
                    Result = BaseError.ConcurrencyError,
                    MessageKey = "ConcurrencyError"
                };
            }

            var setterResult = _fieldAccessor.SetValue(entity, data);

            if (setterResult != BaseError.Success)
            {
                return new TOutput
                {
                    Result = setterResult
                };
            }

            return _validator.Validate(entity);
        }
    }
}
