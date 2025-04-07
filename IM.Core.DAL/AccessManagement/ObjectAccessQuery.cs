using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.AccessManagement;

internal class ObjectAccessQuery<TEntity> : IObjectAccessQuery 
    where TEntity : class, IGloballyIdentifiedEntity
{
    private readonly DbSet<TEntity> _owners;
    private readonly ObjectClass _ownerClass;

    public ObjectAccessQuery(DbSet<TEntity> owners, ObjectClass ownerClass)
    {
        _owners = owners;
        _ownerClass = ownerClass;
    }

    public async Task<bool> QueryAsync(ObjectAccessQueryParameter parameter, CancellationToken cancellationToken = default)
    {
        var query = from owner in _owners
                    where owner.IMObjID == parameter.OwnerId
                    select DbFunctions.AccessIsGranted(
                        parameter.ObjectClass,
                        parameter.ObjectId,
                        parameter.OwnerId,
                        _ownerClass,
                        parameter.Type,
                        parameter.Propagate);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }
}
