using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.MaintenanceWork;

internal class MaintenanceTreeQuery : 
    MaintenanceBaseTreeQuery<Maintenance>
    , IMaintenanceNodeTreeQuery
{
    protected override ObjectClass ClassID => ObjectClass.Maintenance;

    public MaintenanceTreeQuery(DbSet<ClassIcon> classIcons
        , DbSet<Maintenance> maintenances)
        : base(classIcons, maintenances)
    {
    }

    public Task<MaintenanceNodeTree[]> ExecuteAsync(Guid? parentID, CancellationToken cancellationToken)
    {
        var query = Entities.AsNoTracking()
            .Where(c => c.FolderID == parentID)
            .Select(c => new MaintenanceNodeTree()
            {
                ID = c.ID,
                Name = c.Name,
                ParentID = c.FolderID,
                RowVersion = c.RowVersion,
                ClassID = ClassID,
                IconName = ClassIcons.Where(IconNameExpression)
                                     .Select(c => c.IconName)
                                     .FirstOrDefault()
            });

        return query.ToArrayAsync(cancellationToken);
    }
}
