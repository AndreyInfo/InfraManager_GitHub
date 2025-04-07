using InfraManager.Expressions;
using System;
using System.Linq.Expressions;

namespace Inframanager
{
    public class SpecificationBuilder<TEntity, TFilter> : IBuildSpecification<TEntity, TFilter>
    {
        private readonly Expression<Func<TEntity, TFilter, bool>> _expression;

        public SpecificationBuilder(Expression<Func<TEntity, TFilter, bool>> expression)
        {
            _expression = expression;
        }

        public SpecificationBuilder(Expression<Func<TFilter, bool>> expression)
            : this(Expression.Lambda<Func<TEntity, TFilter, bool>>(
                expression.Body, 
                Expression.Parameter(typeof(TEntity)), expression.Parameters[0]))
        {
        }

        public Specification<TEntity> Build(TFilter filterBy)
        {
            var specificationExpressionBody =
                new ExpressionReplacer(_expression.Parameters[1], Expression.Constant(filterBy))
                    .Visit(_expression.Body);

            return new Specification<TEntity>(
                Expression.Lambda<Func<TEntity, bool>>(
                    specificationExpressionBody, 
                    _expression.Parameters[0]));
        }

        public static implicit operator SpecificationBuilder<TEntity, TFilter>(Specification<TEntity> spec)
        {
            Expression<Func<TEntity, bool>> specExpression = spec;
            return new SpecificationBuilder<TEntity, TFilter>(
                Expression.Lambda<Func<TEntity, TFilter, bool>>(
                    specExpression.Body, 
                    specExpression.Parameters[0], 
                    Expression.Parameter(typeof(TFilter))));
        }

        public static implicit operator SpecificationBuilder<TEntity, TFilter>(Specification<TFilter> spec)
        {
            Expression<Func<TFilter, bool>> specExpression = spec;
            return new SpecificationBuilder<TEntity, TFilter>(
                Expression.Lambda<Func<TEntity, TFilter, bool>>(
                    specExpression.Body,
                    Expression.Parameter(typeof(TEntity)),
                    specExpression.Parameters[0]));
        }

        public static IBuildSpecification<T, F> Any<T, F>(params IBuildSpecification<T, F>[] builders) =>
            new SpecificationUnion<T, F>(builders);

        public static IBuildSpecification<T, F> All<T, F>(params IBuildSpecification<T, F>[] builders) =>
            new SpecificationInterception<T, F>(builders);
    }
}
