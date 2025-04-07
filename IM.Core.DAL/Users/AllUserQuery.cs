using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfraManager.DAL.Users;

internal class AllUserQuery : IAllUserQuery, ISelfRegisteredService<IAllUserQuery>
{
    private readonly CrossPlatformDbContext _context;

    public AllUserQuery(CrossPlatformDbContext context)
    {
        _context = context;
    }

    public IEnumerable<User> ExecuteQuery()
    {
        return _context.Set<User>().AsEnumerable();
    }
}