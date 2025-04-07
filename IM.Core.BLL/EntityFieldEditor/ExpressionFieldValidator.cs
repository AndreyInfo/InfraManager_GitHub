using InfraManager.BLL.ExpressionBuilder;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using System.Linq.Expressions;

namespace InfraManager.BLL.EntityFieldEditor
{
    /// <summary>
    /// Этот класс реализует валидатор выражения в поле сущности
    /// </summary>
    /// <typeparam name="TEntity">Тип валидируемой сущности</typeparam>
    /// <typeparam name="TParam">Тип параметра выражения</typeparam>
    /// <typeparam name="TResult">Тип значения выражения</typeparam>
    public class ExpressionFieldValidator<TEntity, TParam, TResult> : IValidator<TEntity, ExpressionFieldValidationResult>
        where TEntity : class
        where TResult : struct
    {
        private readonly ExpressionBuilder<TParam, TResult> _expressionBuilder;

        private readonly Func<TEntity, string> _expressionAccessor;

        /// <summary>
        /// Создает экземпляр типа ExpressionFieldValidator
        /// </summary>
        /// <param name="expressionBuilder">Ссылка на строителя выражений</param>
        /// <param name="expressionAccessor">Ссылка на выражение, которое получает текст проверяемого выражения из валидируемого объекта</param>
        public ExpressionFieldValidator(
            ExpressionBuilder<TParam, TResult> expressionBuilder, 
            Expression<Func<TEntity, string>> expressionAccessor)
        {
            _expressionBuilder = expressionBuilder;
            _expressionAccessor = expressionAccessor.Compile();
        }

        public ExpressionFieldValidationResult Validate(TEntity entity)
        {
            return Validate(_expressionAccessor(entity));
        }

        public ExpressionFieldValidationResult Validate(string expression)
        {
            try
            {
                _expressionBuilder.Build(expression);

                return new ExpressionFieldValidationResult
                {
                    Result = BaseError.Success
                };
            }
            catch(ExpressionValidationException validationError)
            {
                return new ExpressionFieldValidationResult
                {
                    Result = BaseError.ValidationError,
                    MessageKey = validationError.MessageKey,
                    MessageArguments = validationError.MessageArguments
                };
            }
        }
    }
}
