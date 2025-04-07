using System;
using System.Collections.Generic;
using System.Linq;

namespace Inframanager
{
    public abstract class CompositeSpecificationBuilder<TEntity, TFilter> : IBuildSpecification<TEntity, TFilter>
    {
        private readonly IEnumerable<IBuildSpecification<TEntity, TFilter>> _builders;

        protected CompositeSpecificationBuilder(IEnumerable<IBuildSpecification<TEntity, TFilter>> builders)
        {
            _builders = builders;
        }

        public Specification<TEntity> Build(TFilter filterBy)
        {
            if (!_builders.Any())
            {
                throw new InvalidOperationException("No specification builders to aggregate");
            }

            if (_builders.Count() == 1)
            {
                return _builders.First().Build(filterBy);
            }

            return _builders.Select(b => b.Build(filterBy)).Aggregate((result, next) => Aggregate(result, next));
        }

        protected abstract Specification<TEntity> Aggregate(Specification<TEntity> aggregation, Specification<TEntity> spec);
    }
}
