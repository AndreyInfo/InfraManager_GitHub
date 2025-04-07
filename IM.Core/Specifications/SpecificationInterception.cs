using System.Collections.Generic;

namespace Inframanager
{
    public class SpecificationInterception<TEntity, TFilter> : CompositeSpecificationBuilder<TEntity, TFilter>
    {
        public SpecificationInterception(IEnumerable<IBuildSpecification<TEntity, TFilter>> builders) : base(builders)
        {
        }

        protected override Specification<TEntity> Aggregate(Specification<TEntity> aggregation, Specification<TEntity> spec)
        {
            return aggregation && spec;
        }
    }
}
