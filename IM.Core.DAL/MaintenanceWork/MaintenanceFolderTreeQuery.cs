using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.MaintenanceWork;

internal sealed class MaintenanceFolderTreeQuery :
    MaintenanceBaseTreeQuery<MaintenanceFolder>
    , IMaintenanceNodeTreeQuery
{
    protected override ObjectClass ClassID => ObjectClass.MaintenanceFolder;

    public MaintenanceFolderTreeQuery(DbSet<ClassIcon> classIcons
        , DbSet<MaintenanceFolder> folders) 
        : base(classIcons, folders)
    {
    }

    public Task<MaintenanceNodeTree[]> ExecuteAsync(Guid? parentID, CancellationToken cancellationToken)
    {
        var query = Entities.AsNoTracking()
            .Where(c=> c.ParentID == parentID)
            .Select(c=> new MaintenanceNodeTree()
            {
                ID = c.ID,
                Name = c.Name,
                ParentID = c.ParentID,
                RowVersion = c.RowVersion,
                ClassID = ClassID,
                HasChild = c.Maintenances.Any() || c.SubFolders.Any(),
                IconName = ClassIcons.Where(IconNameExpression)
                                     .Select(c=> c.IconName)
                                     .FirstOrDefault()
            });

        return query.ToArrayAsync(cancellationToken);
    }
}
