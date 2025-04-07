using System.Collections.Generic;

namespace Inframanager
{
    public class SpecificationUnion<TEntity, TFilter> : CompositeSpecificationBuilder<TEntity, TFilter>
    {
        public SpecificationUnion(IEnumerable<IBuildSpecification<TEntity, TFilter>> builders) : base(builders)
        {
        }

        protected override Specification<TEntity> Aggregate(Specification<TEntity> aggregation, Specification<TEntity> spec)
        {
            return aggregation || spec;
        }
    }
}
