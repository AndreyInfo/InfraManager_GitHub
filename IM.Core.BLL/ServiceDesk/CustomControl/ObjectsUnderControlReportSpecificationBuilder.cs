using Inframanager;
using Inframanager.BLL.ListView;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System;

namespace InfraManager.BLL.ServiceDesk.CustomControl
{
    internal class ObjectsUnderControlReportSpecificationBuilder<TEntity> : IListViewUserSpecification<TEntity, ObjectUnderControl>
        where TEntity : IGloballyIdentifiedEntity
    {
        private readonly IBuildObjectIsUnderControlSpecification<TEntity> _specificationBuilder;

        public ObjectsUnderControlReportSpecificationBuilder(
            IBuildObjectIsUnderControlSpecification<TEntity> specificationBuilder)
        {
            _specificationBuilder = specificationBuilder;
        }

        public Specification<TEntity> Build(Guid userID)
        {
            return _specificationBuilder.Build(userID);
        }
    }
}
