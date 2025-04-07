using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    internal class WorkOrderReferenceProvider : 
        IProvideWorkOrderReference, 
        ISelfRegisteredService<IProvideWorkOrderReference>
    {
        private readonly IReadonlyRepository<WorkOrderReference> _repository;
        private readonly IServiceMapper<ObjectClass, IAbstractFinder<ICreateWorkOrderReference, Guid>> _finders;
        private readonly IReadonlyRepository<MassIncident> _massIncidents;

        public WorkOrderReferenceProvider(
            IReadonlyRepository<WorkOrderReference> repository,
            IServiceMapper<ObjectClass, IAbstractFinder<ICreateWorkOrderReference, Guid>> finders,
            IReadonlyRepository<MassIncident> massIncidents)
        {
            _repository = repository;
            _finders = finders;
            _massIncidents = massIncidents;
        }

        public async Task<WorkOrderReference> GetOrCreateAsync(InframanagerObject? referencedObject, CancellationToken cancellationToken = default)
        {
            var reference = await (
                referencedObject.HasValue && !referencedObject.Value.IsDefault
                    ? _repository.FirstOrDefaultAsync(
                        WorkOrderReference.ByReferencedObject(referencedObject.Value),
                        cancellationToken)
                    : _repository.FirstOrDefaultAsync(WorkOrderReference.NullReference, cancellationToken));

            if (reference == null)
            {
                if (!referencedObject.HasValue || referencedObject.Value.IsDefault)
                {
                    throw new Exception("Missing Null WorkOrderReference."); // TODO: Подпилить структуру БД, чтобы можно было и нулевую запись создавать если ее нет
                }

                if (!_finders.HasKey(referencedObject.Value.ClassId) && referencedObject.Value.ClassId != ObjectClass.MassIncident)
                {
                    throw new InvalidObjectException($"{referencedObject.Value.ClassId} cannot reference work order.");
                }

                var referencedEntity = referencedObject.Value.ClassId == ObjectClass.MassIncident
                    ? await _massIncidents.SingleAsync(x => x.IMObjID == referencedObject.Value.Id, cancellationToken)
                    : await _finders
                        .Map(referencedObject.Value.ClassId)
                        .FindAsync(referencedObject.Value.Id, cancellationToken);
                reference = referencedEntity.CreateWorkOrderReference();
            }

            return reference;
        }
    }
}
