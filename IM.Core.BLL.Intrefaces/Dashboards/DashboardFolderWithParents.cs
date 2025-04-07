using InfraManager.DAL.Dashboards;
using System;
using System.Collections.Generic;


namespace InfraManager.BLL.Dashboards;

public class DashboardFolderWithParents
{
    public Guid ID { get; init; }

    public string Name { get; set; }

    public Guid? ParentDashboardFolderID { get; set; }
    public DashboardFolder Parent { get; set; }

    public IEnumerable<DashboardFolder> Parents { get; private set; }
    public byte[] RowVersion { get; set; }

    public void BuildParents()
    {
        var result = new Queue<DashboardFolder>();
        if (Parent is not null)
        {
            var type = Parent;
            
            result.Enqueue(type);
            while (type.ParentDashboardFolderID.HasValue)
            {
                type = type.Parent
                    ?? throw new ArgumentNullException("Должен быть инициализрованный родитель");
                result.Enqueue(type);
            }
        }
        Parents = result;
    }
}
