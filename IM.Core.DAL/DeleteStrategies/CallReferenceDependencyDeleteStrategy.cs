using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InfraManager.DAL.DeleteStrategies
{
    internal class CallReferenceDependencyDeleteStrategy : 
        IDependentDeleteStrategy<Call>,
        IDependentDeleteStrategy<Problem>,
        IDependentDeleteStrategy<ChangeRequest>,
        ISelfRegisteredService<IDependentDeleteStrategy<Call>>,
        ISelfRegisteredService<IDependentDeleteStrategy<Problem>>,
        ISelfRegisteredService<IDependentDeleteStrategy<ChangeRequest>>
    {
        private readonly DbContext _context;        

        public CallReferenceDependencyDeleteStrategy(CrossPlatformDbContext context)
        {
            _context = context;
        }

        public void OnDelete(Call entity)
        {
            var references = _context.Set<CallReference>();
            foreach (var reference in references.Where(x => x.CallID == entity.IMObjID).ToArray())
            {
                references.Remove(reference);
            }
        }

        public void OnDelete(Problem problem) => OnDelete<Problem>(problem);

        public void OnDelete(ChangeRequest rfc) => OnDelete<ChangeRequest>(rfc);

        private void OnDelete<TRef>(TRef referencedObject) where TRef : IHaveUtcModifiedDate, IGloballyIdentifiedEntity
        {
            var references = _context.Set<CallReference<TRef>>();
            foreach (var reference in references.Where(x => x.ObjectID == referencedObject.IMObjID).ToArray())
            {
                references.Remove(reference);
            }
        }
    }
}
