using System;
using System.Linq;
using Inframanager;
using InfraManager.DAL.AccessManagement;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ServiceDesk.Calls;

internal class CallIsAvailableToExecutorViaToz :
    IBuildAvailableToExecutorViaToz<User, Call>,
    IBuildAvailableToExecutorViaToz<Group, Call>,
    ISelfRegisteredService<CallIsAvailableToExecutorViaToz>
{
    private const int NullWorkplace = 0;

    private readonly IQueryable<Call> _calls;

    public CallIsAvailableToExecutorViaToz(DbSet<Call> calls)
    {
        _calls = calls.AsNoTracking();
    }

    internal Specification<TExecutor> BuildSpecification<TExecutor>(Guid callID, ObjectClass ownerClassID)
        where TExecutor : IGloballyIdentifiedEntity
    {
        var calls = _calls
            .Where(x => x.IMObjID == callID
                        && x.Client != null
                        && x.Client.Workplace != null
                        && x.Client.Workplace.ID != NullWorkplace
                        && x.Client.SubdivisionID.HasValue);
        
        return new Specification<TExecutor>(user =>
            !calls.Any()
            || calls.All(call =>
                DbFunctions.AccessIsGranted(
                    ObjectClass.Workplace,
                    call.Client.Workplace.IMObjID,
                    user.IMObjID,
                    ownerClassID,
                    AccessTypes.TOZ_sks,
                    false)
                && DbFunctions.AccessIsGranted(
                    ObjectClass.Division,
                    call.ClientSubdivisionID.Value,
                    user.IMObjID,
                    ownerClassID,
                    AccessTypes.TOZ_org,
                    false)));
    }

    Specification<User> IBuildSpecification<User, Call>.Build(Call filterBy)
    {
        return BuildSpecification<User>(filterBy.IMObjID, ObjectClass.User);
    }

    Specification<Group> IBuildSpecification<Group, Call>.Build(Call filterBy)
    {
        return BuildSpecification<Group>(filterBy.IMObjID, ObjectClass.Group);
    }
}