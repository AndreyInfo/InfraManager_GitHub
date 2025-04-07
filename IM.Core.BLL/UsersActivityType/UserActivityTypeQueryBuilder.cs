using System.Linq;
using Inframanager.BLL;
using InfraManager.DAL;

namespace InfraManager.BLL.UsersActivityType;

internal class UserActivityTypeQueryBuilder :
    IBuildEntityQuery<UserActivityType, UserActivityTypeDetails, UserActivityTypeFilter>,
    ISelfRegisteredService<IBuildEntityQuery<UserActivityType, UserActivityTypeDetails, UserActivityTypeFilter>>
{
    private readonly IReadonlyRepository<UserActivityType> _repository;
    private readonly ICurrentUser _currentUser;

    public UserActivityTypeQueryBuilder(
        IReadonlyRepository<UserActivityType> repository,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public IExecutableQuery<UserActivityType> Query(UserActivityTypeFilter filterBy)
    {
        var query = _repository
            .With(uat => uat.References)
            .Query();

        if (filterBy.ParentID.HasValue)
        {
            query = query.Where(x => x.ParentID == filterBy.ParentID);
        }

        if (filterBy.OnlyCurrentUser)
        {
            query = query.Where(x => x.References
                .Any(r => r.ObjectClassID == ObjectClass.User
                          && r.ObjectID == _currentUser.UserId));
        }

        if (filterBy.ReferencedObjectClasses?.Any() == true)
        {
            query = query.Where(x => x.References
                .Any(r => r.ReferenceObjectID.HasValue
                          && filterBy.ReferencedObjectClasses.Contains(r.ObjectClassID)));
        }

        if (filterBy.ReferencedObjectID.HasValue)
        {
            query = query.Where(x => x.References
                .Any(r => r.ReferenceObjectID == filterBy.ReferencedObjectID));
        }

        return query;
    }
}