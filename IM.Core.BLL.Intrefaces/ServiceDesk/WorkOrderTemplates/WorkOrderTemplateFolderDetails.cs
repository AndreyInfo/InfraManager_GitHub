using System;

namespace InfraManager.BLL.ServiceDesk;

public class WorkOrderTemplateFolderDetails
{
    public Guid ID { get; init; }

    public string Name { get; init; }

    public Guid? ParentID { get; init; }

    public string ParentName { get; init; } 

    public byte[] RowVersion { get; init; }
}
