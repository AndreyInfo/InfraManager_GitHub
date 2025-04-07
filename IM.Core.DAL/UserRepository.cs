using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL
{
    internal class UserRepository : Repository<User>, //TODO: Это временно, чтобы убиранием QueryFilter на сущность User не сломать сразу кучу BLL
        ISelfRegisteredService<IRepository<User>>,
        ISelfRegisteredService<IReadonlyRepository<User>>
    {
        public UserRepository(DbSet<User> set, IDeleteStrategy<User> deleteStrategy) : base(set, deleteStrategy)
        {
        }

        public override IExecutableQuery<User> Query()
        {
            return new ExecutableQuery<User>(Queryable.Where(User.ExceptSystemUsers).Where(user => !user.Removed));
        }

        public override IExecutableQuery<User> Query(Expression<Func<User, bool>> predicate)
        {
            return Query().Where(predicate);
        }
    }
}
