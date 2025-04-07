using System.Collections.Generic;
using System.Linq;

namespace InfraManager.DAL.Users;

internal class GetUserByCriteriaQuery : IGetUserByCriteriaQuery,ISelfRegisteredService<IGetUserByCriteriaQuery>
{
    private readonly CrossPlatformDbContext _dbContext;

    public GetUserByCriteriaQuery(CrossPlatformDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<User> ExecuteQuery(UserCriteria criteria)
    {
        var query = _dbContext.Set<User>().AsQueryable();
        
        if (criteria.Emails?.Any() ?? false)
            query = query.Where(x => criteria.Emails.Contains(x.Email));
        
        if (criteria.Logins?.Any() ?? false)
            query = query.Where(x => criteria.Logins.Contains(x.LoginName));
        
        if (criteria.Numbers?.Any() ?? false)
            query = query.Where(x => criteria.Numbers.Contains(x.Number));
        
        if (criteria.ExternalIDs?.Any() ?? false)
            query = query.Where(x => criteria.ExternalIDs.Contains(x.ExternalID));
        
        if (criteria.SIDs?.Any() ?? false)
            query = query.Where(x => criteria.SIDs.Contains(x.SID));

        if (criteria.NameSurnames?.Any() ?? false)
        {
            var nameSurname = criteria.NameSurnames.Select(x => $"{x.Name}|{x.Surname}")
                .ToList();
           
            query = query.Select(x => new {Data = x, NameSurname = x.Name+"|"+x.Surname})
                .Where(x => nameSurname.Contains(x.NameSurname))
                .Select(x => x.Data);
        }

        if (criteria.FIOs?.Any() ?? false)
        {
            var fios = criteria.FIOs.Select(x => $"{x.Name}|{x.Surname}|{x.Patronymic}").ToList();
            
            query = query.Select(x => new {Data = x, FIO = x.Name+"|"+x.Surname+"|"+x.Patronymic})
                .Where(x => fios.Contains(x.FIO)).Select(x => x.Data);
        }

        if (!criteria.WithRemoved)
            query = query.Where(x => !x.Removed);

        return query.AsEnumerable();
    }
}