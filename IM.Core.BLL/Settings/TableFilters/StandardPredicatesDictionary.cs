using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters
{
    internal class StandardPredicatesDictionary<TEntity>
        : IStandardPredicatesProvider<TEntity>
    {
        private readonly Dictionary<string, Func<Guid, Expression<Func<TEntity, bool>>>> _predicateBuilders =
            new Dictionary<string, Func<Guid, Expression<Func<TEntity, bool>>>>();

        public bool Contains(string name)
        {
            return _predicateBuilders.ContainsKey(name);
        }

        public Expression<Func<TEntity, bool>> Get(string name, Guid userId)
        {
            return _predicateBuilders[name](userId);
        }

        protected StandardPredicatesDictionary<TEntity> Add(
            string name,
            Func<Guid, Expression<Func<TEntity, bool>>> predicateBuilder)
        {
            _predicateBuilders.Add(name, predicateBuilder);
            return this;
        }
    }
}
