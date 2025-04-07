using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.BLL.Asset;
using InfraManager.DAL;

namespace InfraManager.BLL.OrganizationStructure.Groups
{
    internal class GroupLoader :
        ILoadEntity<Guid, Group, GroupDetails>,
        ISelfRegisteredService<ILoadEntity<Guid, Group, GroupDetails>>
    {
        IReadonlyRepository<Group> _finder;

        public GroupLoader(IReadonlyRepository<Group> finder)
        {
            _finder = finder;
        }

        public Task<Group> LoadAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _finder
                .WithMany(x => x.QueueUsers)
                    .ThenWith(x => x.User)
                .FirstOrDefaultAsync(x => x.IMObjID == id, cancellationToken);
        }
    }
}
