namespace Inframanager
{
    public class NegativeSpecificationBuilder<TEntity, TFilter> : IBuildSpecification<TEntity, TFilter>
    {
        private readonly IBuildSpecification<TEntity, TFilter> _originalBuilder;

        public NegativeSpecificationBuilder(IBuildSpecification<TEntity, TFilter> originalBuilder)
        {
            _originalBuilder = originalBuilder;
        }

        public Specification<TEntity> Build(TFilter filterBy)
        {
            return !_originalBuilder.Build(filterBy);
        }
    }
}
