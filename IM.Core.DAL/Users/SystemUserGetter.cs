using InfraManager.DAL.AccessManagement;
using Microsoft.EntityFrameworkCore;
using RTools_NTS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Users
{
internal class SystemUserGetter : ISystemUserGetter, ISelfRegisteredService<ISystemUserGetter>
{
    private readonly DbSet<User> _db;
    public SystemUserGetter(DbSet<User> db)
    {
        _db = db;
    }
    public Task<User> GetAsync(CancellationToken cancellationToken = default)
    {
        return _db.AsQueryable().IgnoreQueryFilters().Where(x => x.IMObjID == User.SystemUserGlobalIdentifier).SingleOrDefaultAsync(cancellationToken);
    }

}
}