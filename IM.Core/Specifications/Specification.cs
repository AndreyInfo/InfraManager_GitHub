using InfraManager.Expressions;
using InfraManager.Linq;
using System;
using System.Linq.Expressions;

namespace Inframanager
{
    /// <summary>
    /// Этот класс представляет спецификацию объекта типа T
    /// </summary>
    /// <typeparam name="T">Тип объекта</typeparam>
    public class Specification<T>
    {        
        protected readonly Expression<Func<T, bool>> _rule;

        public Specification(bool value) : this(x => value)
        {
        }

        public Specification(Expression<Func<T, bool>> rule)
        {
            _rule = rule ?? throw new ArgumentNullException(nameof(rule));
        }

        private Func<T, bool> _func;
        protected virtual Func<T, bool> GetFunc() => _func = _func ?? _rule.Compile();

        /// <summary>
        /// Задает признак того что спецификация обладает свойством аддитивности.
        /// Spec(a && / || b) = Spec(a) && / || Spec(b)
        /// </summary>
        protected virtual bool Additive => true;

        public bool IsSatisfiedBy(T entity)
        {
            var func = GetFunc();

            return func(entity);
        }

        public Specification<V> Convert<V>(Expression<Func<V, T>> convertor)
        {
            var newExpression = new ExpressionReplacer(_rule.Parameters[0], convertor.Body).Visit(_rule.Body);
            return new Specification<V>(Expression.Lambda<Func<V, bool>>(newExpression, convertor.Parameters[0]));
        }

        public static implicit operator Expression<Func<T, bool>>(Specification<T> spec) => spec._rule;
        public static implicit operator Func<T, bool>(Specification<T> spec) => spec.GetFunc();

        public static bool operator true(Specification<T> spec) => false;
        public static bool operator false(Specification<T> spec) => false;
        public static Specification<T> operator &(Specification<T> first, Specification<T> second) =>
            first.Additive && second.Additive 
                ? new Specification<T>(first._rule.And(second._rule)) 
                : new Conjunction<T>(first, second);
        public static Specification<T> operator |(Specification<T> first, Specification<T> second) => 
            first.Additive && second.Additive
                ? new Specification<T>(first._rule.Or(second._rule))
                : new Disjunction<T>(first, second);
        public static Specification<T> operator !(Specification<T> spec) => 
            spec.Additive ? new Specification<T>(spec._rule.Not()) : new Negation<T>(spec);
    }
}
