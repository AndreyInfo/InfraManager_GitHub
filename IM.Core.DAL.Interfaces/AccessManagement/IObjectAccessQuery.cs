using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.AccessManagement
{
    public class ObjectAccessQueryParameter
    {
        public Guid OwnerId { get; init; }
        public Guid ObjectId { get; init; }
        public ObjectClass ObjectClass { get; init; }
        public AccessTypes Type { get; init; } = AccessTypes.DeviceCatalogue;
        public bool Propagate { get; init; } = false;
    }

    public interface IObjectAccessQuery
    {
        Task<bool> QueryAsync(ObjectAccessQueryParameter parameter, CancellationToken cancellationToken = default);
    }
}
