using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.Import;

internal class ContextRequestService : IContextRequestService,ISelfRegisteredService<IContextRequestService>
{
    private readonly CrossPlatformDbContext _context;

    public ContextRequestService(CrossPlatformDbContext context)
    {
        _context = context;
    }

    public DbConnection GetDbConnection()
    {
        return _context.Database.GetDbConnection();
    }

}